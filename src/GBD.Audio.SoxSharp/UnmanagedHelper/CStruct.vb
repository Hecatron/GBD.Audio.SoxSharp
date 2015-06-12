
' Used for containing representations of low level C Structures
' Also keeps tabs on memory pointers, and if the underlying memory was allocated
' via us (.Net) or the DLL

' The best way to think of unmanaged memory is a block of no-mans land that sits between VB .Net / the DLL
' Typically you have a memory pointer which points to the first byte of the block
' and a way of letting the operating system know that you want to allocate a block for use

' If a block is allocated by something outside of the .Net platform then it needs to be the one to unallocate it
' In the same way if .Net allocates memory then .Net needs to free it up
' Because the Garbage collector can't always capture this automatically for unmanaged code areas
' We use the below class as a sort of wrapper to auto clean up after itself, and to interpret the data into
' a structure of some kind that we can read

Imports System
Imports System.Runtime.InteropServices

Namespace UnmanagedHelper

    ''' <summary>
    '''  Represents an unmanaged C Structure held in memory
    '''  
    '''  Note Pointer is a memory address pointer to the block of unmanaged memory and struct is a
    '''  representation of what the data looks like (a copy)
    '''  
    '''  Unmanaged means the block of data was ether created outside of the .Net platform (e.g.
    '''  allocated via a DLL) or allocated via UnmgdMem_Alloc Any memory allocated via a DLL must be
    '''  freed via the DLL (usually via a function call)
    '''  and any memory allocated via UnmgdMem_Alloc in .Net must be freed via UnmgdMem_DeAlloc.
    ''' </summary>
    Public Class CStruct(Of customtype)

#Region "Types"

        ''' <summary> Determines how the unmanaged memory that is pointed to was allocated. </summary>
        Public Enum AllocatedMode As Integer

            ''' <summary>
            ''' No Memory allocated at the pointer
            ''' </summary>
            None = 0

            ''' <summary>
            ''' Memory was allocated by .Net via UnmgdMem_Alloc
            ''' and must be freed via UnmgdMem_DeAlloc
            ''' </summary>
            viaNet = 1

            ''' <summary>
            ''' Memory was allocated by a DLL or external source via C new
            ''' and must be freed via the Dll (usually via a function call)
            ''' </summary>
            viaDll = 2

            ''' <summary>
            ''' The memory pointer points to something, but it's just a piece of a larger memory block
            ''' which means we're not worried about if the memory is allocated / de-allocated
            ''' </summary>
            Nested = 3

        End Enum

#End Region

#Region "Properties"

        ''' <summary> Memory Address pointer to the block of memory which is unmanaged. </summary>
        ''' <value> The pointer. </value>
        Public Property Pointer As IntPtr

        ''' <summary> Keep track of how the memory was allocated. </summary>
        ''' <value> The pointer allocated. </value>
        Public Property Pointer_Allocated As AllocatedMode

        ''' <summary>
        '''  Structure which is a direct copy of the unamanged data stored in a managed object (structure)
        '''  Note accessing this structure does not directly access the unamanged data you still need to
        '''  copy from this to / from the above memory pointer to access / change the unmanaged memory.
        ''' </summary>
        ''' <remarks>
        '''  any structures used here should be declared StructLayout(LayoutKind.Sequential)
        ''' </remarks>
        ''' <value> The structure. </value>
        Public Property Struct As customtype

#End Region

#Region "Constructors"

        ''' <summary> Default Empty Constructor. </summary>
        Public Sub New()
            Pointer = IntPtr.Zero
            _Pointer_Allocated = AllocatedMode.None
            Struct = Nothing
        End Sub

        ''' <summary> Constructor based on a memory pointer. </summary>
        ''' <param name="ptr_param">   The pointer parameter. </param>
        ''' <param name="Alloc_param"> The allocate parameter. </param>
        Public Sub New(ptr_param As IntPtr, Alloc_param As AllocatedMode)
            Pointer = ptr_param
            _Pointer_Allocated = Alloc_param
            CopyPtrToStruct()
        End Sub

        ''' <summary> Constructor based on a Structure. </summary>
        ''' <exception cref="ArgumentException"> Thrown when one or more arguments have unsupported or
        '''                                      illegal values. </exception>
        ''' <param name="struc_param"> The struc parameter. </param>
        ''' <param name="Alloc_param"> The allocate parameter. </param>
        Public Sub New(struc_param As customtype, Optional Alloc_param As AllocatedMode = AllocatedMode.None)
            ' Make a copy of the supplied Structure
            Struct = struc_param

            If Alloc_param = AllocatedMode.None Then
                Pointer = IntPtr.Zero
                _Pointer_Allocated = AllocatedMode.None
            ElseIf Alloc_param = AllocatedMode.viaNet Then
                ' Do we need to allocate some memory to copy this into (unmanaged)
                MemoryAllocate()
                CopyStructToPtr()
            ElseIf Alloc_param = AllocatedMode.viaDll Then
                Throw New ArgumentException("AllocatedMode = viaDll not supported in this context")
            ElseIf Alloc_param = AllocatedMode.Nested Then
                Throw New ArgumentException("AllocatedMode = Nested not supported in this context")
            End If
        End Sub


#End Region

#Region "Destructors"

        ''' <summary> Called by the user to close / de-allocate all memory. </summary>
        Public Sub Dispose()
            ' If the memory has been allocated via .Net then make sure we free it
            If _Pointer_Allocated = AllocatedMode.viaNet Then MemoryFree()
            ' If the memory has been allocated outside .Net then the pointer should already have been freed
            ' therefore mark the block as freed as Dispose is called by the user
            If _Pointer_Allocated = AllocatedMode.viaDll Then
                _Pointer_Allocated = AllocatedMode.None
                Pointer = IntPtr.Zero
                Struct = Nothing
            End If
        End Sub

        ''' <summary>
        '''  Called by the .Net platform when the class goes out of scope and needs to be destroyed by the
        '''  garbage collector.
        ''' </summary>
        Protected Overloads Overrides Sub Finalize()
            ' If the memory has been allocated via .Net then make sure we free it
            If _Pointer_Allocated = AllocatedMode.viaNet Then MemoryFree()
            ' If the memory has been allocated outside .Net then the
            ' pointer should already have been freed and this should never happend
            ' therefore constitutes a memory leak
            If _Pointer_Allocated = AllocatedMode.viaDll Then
                'Throw New Exception("Memory Leak detected with the dll, class has been closed but memory is still allocated")
            End If
        End Sub

#End Region

#Region "Methods"

        ''' <summary> Allocate memory for the unmanaged data block. </summary>
        ''' <returns> true if it succeeds, false if it fails. </returns>
        Public Function MemoryAllocate() As Boolean
            If _Pointer_Allocated = AllocatedMode.None Then
                Pointer = CHelper.UmgMem_Alloc(Marshal.SizeOf(Struct))
                _Pointer_Allocated = AllocatedMode.viaNet
                Return True
            End If
            Return False
        End Function

        ''' <summary> Free up any Allocated unmanaged memory. </summary>
        ''' <returns> true if it succeeds, false if it fails. </returns>
        Public Function MemoryFree() As Boolean
            If _Pointer_Allocated = AllocatedMode.viaNet Then
                CHelper.UmgMem_DeAlloc(Pointer)
                _Pointer_Allocated = AllocatedMode.None
                Pointer = IntPtr.Zero
                Return True
            End If
            Return False
        End Function

        ''' <summary> Copy the unmanaged memory from the address pointer into the structure. </summary>
        ''' <exception cref="Exception"> Thrown when an exception error condition occurs. </exception>
        Public Sub CopyPtrToStruct()
            If _Pointer_Allocated = AllocatedMode.None Or Pointer.ToInt64 = 0 Then _
                Throw New Exception("Error Pointer undefined, no data to copy from")
            Struct = CHelper.Umg_PtrToStructure(Of customtype)(Pointer)
        End Sub

        ''' <summary>
        '''  Copy the structure into the memory allocated at the address pointer Note the memory pointed
        '''  to via pointer must be allocated ether via the dll or via .net.
        ''' </summary>
        Public Sub CopyStructToPtr()
            If _Pointer_Allocated = AllocatedMode.None Then MemoryAllocate()
            Pointer = CHelper.Umg_StructureToPtr(Of customtype)(Pointer, Struct)
        End Sub

#End Region

    End Class

End Namespace