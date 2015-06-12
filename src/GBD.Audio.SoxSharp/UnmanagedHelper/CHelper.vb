Imports System
Imports System.Collections.ObjectModel
Imports System.Runtime.InteropServices
Imports GBD.Audio.SoxSharp.swig

Namespace UnmanagedHelper

    ''' <summary> Helper Code for Low Level C Unmanaged structures. </summary>
    Public Class CHelper

#Region "Methods - Managed"

        ''' <summary> Return True if we're on a 64 bit Operating system. </summary>
        ''' <returns> true if 64bit, false if not. </returns>
        Public Shared Function Is64bit() As Boolean
            Return (Runtime.InteropServices.Marshal.SizeOf(GetType(IntPtr)) * 8) = 64
        End Function

        ''' <summary> Convert a Soxbool type to a regular boolean. </summary>
        ''' <param name="input"> The input. </param>
        ''' <returns> true if it succeeds, false if it fails. </returns>
        Public Shared Function SoxBoolToBoolean(input As sox_bool) As Boolean
            If input = sox_bool.sox_true Then Return True
            Return False
        End Function

        ''' <summary> Convert a regular boolean to a Soxbool. </summary>
        ''' <param name="input"> The input. </param>
        ''' <returns> A sox_bool. </returns>
        Public Shared Function BooleanToSoxBool(input As Boolean) As sox_bool
            If input = True Then Return sox_bool.sox_true
            Return sox_bool.sox_false
        End Function

        ''' <summary> Convert a C Byte Array to a String. </summary>
        ''' <param name="ByteArr"> Array of bytes. </param>
        ''' <returns> A String. </returns>
        Public Shared Function CByteArrToString(ByteArr() As Byte) As String
            Dim retval As String = ""
            If ByteArr Is Nothing Then Return retval
            For Each item As Byte In ByteArr
                retval &= Chr(item)
            Next
            retval = retval.TrimEnd(CChar(" "))
            Return retval
        End Function

        ''' <summary> Convert a String to a C Byte Array, Terminated with a Null (0) value. </summary>
        ''' <param name="input"> The input. </param>
        ''' <returns> A Byte() </returns>
        Public Shared Function StringToCByteArr(input As String) As Byte()
            If input = "" Then Return Nothing
            Dim retval(input.Length) As Byte
            For count1 As Integer = 0 To (input.Length - 1)
                retval(count1) = CByte(Asc(Mid(input, count1 + 1, 1)))
            Next
            Return retval
        End Function

        ''' <summary>
        '''  Adds an offset to a memory pointer Typically not supported in the framework until .Net 4, so
        '''  we implement our own methods here.
        ''' </summary>
        ''' <exception cref="NotSupportedException"> Thrown when the requested operation is not supported. </exception>
        ''' <param name="src">    Source for the. </param>
        ''' <param name="offset"> The offset. </param>
        ''' <returns> An IntPtr. </returns>
        Public Shared Function MemOffset(src As IntPtr, offset As Integer) As IntPtr
            Dim bitsize As Integer = Runtime.InteropServices.Marshal.SizeOf(GetType(IntPtr)) * 8
            Select Case bitsize
                Case 64
                    Return New IntPtr(src.ToInt64() + offset)
                Case 32
                    Return New IntPtr(src.ToInt32() + offset)
                Case Else
                    Throw New NotSupportedException("This is running on a machine where pointers are " & IntPtr.Size & " bytes which is currently unsupported")
            End Select
        End Function

        ''' <summary>
        '''  Adds an offset to a memory pointer Typically not supported in the frameword until .Net 4, so
        '''  we implement our own methods here.
        ''' </summary>
        ''' <exception cref="NotSupportedException"> Thrown when the requested operation is not supported. </exception>
        ''' <param name="src">    Source for the. </param>
        ''' <param name="offset"> The offset. </param>
        ''' <returns> An IntPtr. </returns>
        Public Shared Function MemOffset(src As IntPtr, offset As Long) As IntPtr
            Dim bitsize As Integer = Runtime.InteropServices.Marshal.SizeOf(GetType(IntPtr)) * 8
            Select Case bitsize
                Case 64
                    Return New IntPtr(src.ToInt64() + offset)
                Case 32
                    ' May generate funny results
                    ' And Not the "funny ha ha" kind
                    Return New IntPtr(src.ToInt32() + offset)
                Case Else
                    Throw New NotSupportedException("This is running on a machine where pointers are " & IntPtr.Size & " bytes which is currently unsupported")
            End Select
        End Function

#End Region

#Region "Methods - UnManaged Conversion"

        ''' <summary> Convert an InPtr to a String. </summary>
        ''' <param name="Pointer"> The pointer. </param>
        ''' <returns> A String. </returns>
        Public Shared Function Umg_PtrToString(Pointer As IntPtr) As String
            Dim retval As String = ""
            If Pointer.ToInt64 <> 0 Then retval = Marshal.PtrToStringAnsi(Pointer)
            Return retval
        End Function

        ''' <summary>
        '''  Copy an IntPtr memory pointer to an actual Data Structure, Typically used for unamanged data
        '''  Note the data is copied from the pointer (unmanaged) to the Structure (managed)
        ''' </summary>
        ''' <remarks>
        '''  The pointer can be freed after the copy, but if the memory was allocated via an external
        '''  source (e.g. a DLL) then it must also be freed via the external source.
        ''' </remarks>
        ''' <param name="Pointer"> The pointer. </param>
        ''' <returns> A customtype. </returns>
        Public Shared Function Umg_PtrToStructure(Of customtype)(Pointer As IntPtr) As customtype
            Dim retval As customtype
            If Pointer.ToInt64 = 0 Then Return Nothing
            retval = CType(Marshal.PtrToStructure(Pointer, GetType(customtype)), customtype)
            Return retval
        End Function

        ''' <summary>
        '''  Copy a Data Structure to an IntPtr, Typically used for unamanged data Note the data is copied
        '''  from the structure (managed) to the Pointer (unmanaged)
        ''' </summary>
        ''' <remarks>
        '''  The pointer must point to a block of memory that has already been allocated ether via an
        '''  external source (e.g. a DLL) or via managed code in .net (e.g. UnmgdMemAlloc)
        ''' </remarks>
        ''' <param name="ptr">   The pointer. </param>
        ''' <param name="Struc"> [in,out] The struc. </param>
        ''' <returns> An IntPtr. </returns>
        Public Shared Function Umg_StructureToPtr(Of customtype)(ptr As IntPtr, ByRef Struc As customtype) As IntPtr
            Dim retval As IntPtr = ptr
            If Struc Is Nothing Then Return retval
            Marshal.StructureToPtr(Struc, retval, False)
            Return retval
        End Function

        ''' <summary>
        '''  Copy a Byte Array to an IntPtr, Typically used for unamanged data Note the data is copied
        '''  from the structure (managed) to the Pointer (unmanaged)
        ''' </summary>
        ''' <remarks>
        '''  The pointer must point to a block of memory that has already been allocated ether via an
        '''  external source (e.g. a DLL) or via managed code in .net (e.g. UnmgdMemAlloc)
        ''' </remarks>
        ''' <param name="pointer"> The pointer. </param>
        ''' <param name="input">   The input. </param>
        ''' <returns> An IntPtr. </returns>
        Public Shared Function Umg_CByteArrToPtr(pointer As IntPtr, input() As Byte) As IntPtr
            Dim retval As IntPtr = pointer
            If input Is Nothing Then Throw (New Exception("Byte Array conversion Error"))
            Marshal.Copy(input, 0, retval, input.Length)
            Return retval
        End Function

        ''' <summary> Copy Data from an unmanaged memory block to a Byte Array. </summary>
        ''' <param name="pointer"> The pointer. </param>
        ''' <param name="length">  The length. </param>
        ''' <param name="Offset">  The offset. </param>
        ''' <returns> A Byte() </returns>
        Public Shared Function Umg_PtrToCByteArr(pointer As IntPtr, length As Integer, Optional Offset As Integer = 0) As Byte()
            Dim ret(length) As Byte
            Marshal.Copy(pointer, ret, Offset, length)
            Return ret
        End Function

        ''' <summary> Convert a Unmanaged Pointer to a List of Objects. </summary>
        ''' <param name="Pointer">  The pointer. </param>
        ''' <param name="MaxCount"> Number of maximums. </param>
        ''' <returns> A list of. </returns>
        Public Shared Function Umg_PtrToList(Of customtype)(Pointer As IntPtr, _
                Optional MaxCount As Integer = Integer.MaxValue) As ReadOnlyCollection(Of customtype)
            Dim retval As New List(Of customtype)

            ' Set the Value Pointer to the First Item in the List
            Dim ArrayPtr As IntPtr = Pointer
            Dim ValPtr As IntPtr = Umg_PtrToStructure(Of IntPtr)(ArrayPtr)

            ' Loop Until no more items are left
            While (ValPtr.ToInt64 <> 0 And MaxCount > 0)
                ' Pull the Next value from the Array
                Dim tmpobj As Object
                If GetType(customtype) = GetType(String) Then
                    tmpobj = Umg_PtrToString(ValPtr)
                Else
                    tmpobj = Umg_PtrToStructure(Of customtype)(ValPtr)
                End If
                retval.Add(CType(tmpobj, customtype))
                ' Increment the Array Pointer to the next value in the array
                ArrayPtr = New IntPtr(ArrayPtr.ToInt64() + IntPtr.Size)
                ValPtr = Umg_PtrToStructure(Of IntPtr)(ArrayPtr)
                MaxCount -= 1
            End While

            Return retval.AsReadOnly
        End Function

#End Region

#Region "Methods - UnManaged Memory Allocation"

        ''' <summary>
        '''  Allocates unmanaged memory, and returns a pointer This form just allocates the memory block,
        '''  without copying data in A memory Pointer of the type IntPtr is returned Note this memory must
        '''  be freed via Marshal.FreeHGlobal.
        ''' </summary>
        ''' <param name="length"> The length. </param>
        ''' <returns> An IntPtr. </returns>
        Public Shared Function UmgMem_Alloc(length As Integer) As IntPtr
            Dim retval As IntPtr
            If length < 1 Then Throw (New Exception("Memory Length conversion Error"))
            retval = Marshal.AllocHGlobal(length)
            Return retval
        End Function

        ''' <summary>
        '''  Allocates unmanaged memory, and returns a pointer This form copies a managed Structure into
        '''  the memory that has been allocated A memory Pointer of the type IntPtr is returned Note this
        '''  memory must be freed via Marshal.FreeHGlobal.
        ''' </summary>
        ''' <param name="Struc"> The struc. </param>
        ''' <returns> An IntPtr. </returns>
        Public Shared Function UmgMem_Alloc(Of customtype)(Struc As customtype) As IntPtr
            Dim retval As IntPtr
            If Struc Is Nothing Then Throw (New Exception("Structure conversion Error"))
            retval = Marshal.AllocHGlobal(Marshal.SizeOf(Struc))
            Marshal.StructureToPtr(Struc, retval, False)
            Return retval
        End Function

        ''' <summary>
        '''  Allocates unmanaged memory, and returns a pointer This form copies a managed Byte Array into
        '''  the memory that has been allocated A memory Pointer of the type IntPtr is returned Note this
        '''  memory must be freed via Marshal.FreeHGlobal.
        ''' </summary>
        ''' <param name="input"> The input. </param>
        ''' <returns> An IntPtr. </returns>
        Public Shared Function UmgMem_Alloc(input() As Byte) As IntPtr
            Dim retval As IntPtr
            If input Is Nothing Then Throw (New Exception("Byte Array conversion Error"))
            retval = Marshal.AllocHGlobal(input.Length)
            Marshal.Copy(input, 0, retval, input.Length)
            Return retval
        End Function

        ''' <summary> De-Allocate / Free memory allocated via the above. </summary>
        ''' <param name="pointer"> The pointer. </param>
        Public Shared Sub UmgMem_DeAlloc(pointer As IntPtr)
            Marshal.FreeHGlobal(pointer)
        End Sub

#End Region

    End Class

End Namespace