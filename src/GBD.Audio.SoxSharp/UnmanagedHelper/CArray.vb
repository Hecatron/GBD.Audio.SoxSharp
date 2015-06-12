Imports System.Runtime.InteropServices
Imports GBD.Audio.SoxSharp.IO

Namespace UnmanagedHelper

    ''' <summary>
    '''  Representation of a Unmanaged C Array of items such as strings or structures.
    ''' </summary>
    Public Class CArray(Of customtype)
        Inherits List(Of IntPtr)
        Implements IDisposable

#Region "Properties"

        ''' <summary>
        '''  Keep track of any CStructs added in, to make sure they're not finalized before we are When
        '''  this class is Finalized, so it the below list, and so is each of the CStructs which should
        '''  auto de-allocate the memory.
        ''' </summary>
        ''' <value> A List of structures. </value>
        Protected Property CStruct_List As New List(Of CStruct(Of customtype))

        ''' <summary>
        '''  Keep track of any CStructs added in, to make sure they're not finalized before we are When
        '''  this class is Finalized, so it the below list, and so is each of the CStructs which should
        '''  auto de-allocate the memory.
        ''' </summary>
        ''' <value> The c structure list string. </value>
        Protected Property CStruct_List_String As New List(Of CStruct(Of Byte()))

        ''' <summary> Pointer to Final C Array. </summary>
        ''' <value> The pointer. </value>
        Public ReadOnly Property Pointer As IntPtr
            Get
                Return _Pointer
            End Get
        End Property
        Protected Property _Pointer As IntPtr

        ''' <summary> Keep track of how the memory was allocated. </summary>
        ''' <value> The pointer allocated. </value>
        Public ReadOnly Property Pointer_Allocated As CStruct(Of customtype).AllocatedMode
            Get
                Return _Pointer_Allocated
            End Get
        End Property
        Protected Property _Pointer_Allocated As CStruct(Of customtype).AllocatedMode

#End Region

#Region "Constructors"

        ''' <summary> Default Constructor. </summary>
        Public Sub New()
        End Sub

#End Region

#Region "Destructors"

        ''' <summary> Dispose of List. </summary>
        Public Sub Dispose() Implements IDisposable.Dispose
            For Each memitem As CStruct(Of customtype) In CStruct_List
                memitem.Dispose()
            Next
            For Each memitem As CStruct(Of Byte()) In CStruct_List_String
                memitem.Dispose()
            Next
        End Sub

        ''' <summary>
        '''  Called by the .Net platform when the class goes out of scope and needs to be destroyed by the
        '''  garbage collector.
        ''' </summary>
        Protected Overloads Overrides Sub Finalize()
            MyBase.Finalize()
            DeAllocate()
            Dispose()
        End Sub

#End Region

#Region "Methods"

        ''' <summary> Add an IntPtr Pointer to an Object. </summary>
        ''' <param name="val"> The value to add. </param>
        Public Shadows Sub Add(val As IntPtr)
            MyBase.Add(val)
        End Sub

        ''' <summary> Add a FormatFile Object. </summary>
        ''' <param name="val"> The value to add. </param>
        Public Shadows Sub Add(val As FormatFile)
            MyBase.Add(val.SwigStorage.GetswigCPtr)
        End Sub

        ''' <summary> Add a String. </summary>
        ''' <param name="val"> The value to add. </param>
        Public Shadows Sub Add(val As String)
            ' Copy the string into a Byte Array
            Dim tmp_arr() As Byte
            tmp_arr = CHelper.StringToCByteArr(val)

            ' Manually Allocate the memory as CStruct doesn't like Byte Arrays with Generics
            Dim tmp_area As New CStruct(Of Byte())
            tmp_area.Pointer = CHelper.UmgMem_Alloc(tmp_arr)
            tmp_area.Pointer_Allocated = CStruct(Of Byte()).AllocatedMode.viaNet
            ' Garbage collector will still dispose correctly

            ' Copy to the List
            CStruct_List_String.Add(tmp_area)
            ' Add the pointer to the main underlying list
            MyBase.Add(tmp_area.Pointer)
        End Sub

        ''' <summary> Add a CustomType. </summary>
        ''' <param name="val"> The value to add. </param>
        Public Shadows Sub Add(val As customtype)
            ' Copy the Byte Array into unmanaged memory
            Dim tmp_area As New CStruct(Of customtype)(val, CStruct(Of customtype).AllocatedMode.viaNet)
            ' Copy to the List
            CStruct_List.Add(tmp_area)
            ' Add the pointer to the main underlying list
            MyBase.Add(tmp_area.Pointer)
        End Sub

#End Region

#Region "Methods - Allocate / DeAllocate"

        ''' <summary> Allocate Memory. </summary>
        ''' <exception cref="ArgumentException"> Thrown when one or more arguments have unsupported or
        '''                                      illegal values. </exception>
        ''' <returns> An IntPtr. </returns>
        Public Function AllocateAndCopy() As IntPtr
            If _Pointer_Allocated <> CStruct(Of customtype).AllocatedMode.None Then
                Throw New ArgumentException("Memory Already Allocated")
            End If
            ' Allocate some memory for the pointer array
            _Pointer = CHelper.UmgMem_Alloc(IntPtr.Size * Me.Count)
            ' Copy Pointer Array to Allocated Memory
            Marshal.Copy(Me.ToArray, 0, _Pointer, Me.Count)
            _Pointer_Allocated = CStruct(Of customtype).AllocatedMode.viaNet
            Return _Pointer
        End Function

        ''' <summary> De-Allocate Memory. </summary>
        Public Sub DeAllocate()
            ' De Allocate Memory for Pointer Array
            If _Pointer_Allocated = CStruct(Of customtype).AllocatedMode.viaNet Then
                If _Pointer <> IntPtr.Zero Then CHelper.UmgMem_DeAlloc(_Pointer)
                _Pointer_Allocated = CStruct(Of customtype).AllocatedMode.None
            End If
        End Sub

#End Region

    End Class

End Namespace
