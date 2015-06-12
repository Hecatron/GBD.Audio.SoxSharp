Imports System.Collections.ObjectModel
Imports System.Runtime.InteropServices
Imports GBD.Audio.SoxSharp.DataTypes
Imports GBD.Audio.SoxSharp.swig
Imports GBD.Audio.SoxSharp.UnmanagedHelper

Namespace IO

    ''' <summary> Handler structure defined by each format. </summary>
    <DebuggerDisplay("Description = {Description}")> _
    Public Class FormatHandler
        Implements IDisposable

#Region "Types"

        ''' <summary> File Flags. </summary>
        <Flags()> _
        Public Enum FileFlags

            ''' <summary> Does not use stdio routines </summary>
            NoStdio = &H1

            ''' <summary> File is an audio device </summary>
            Device = &H2

            ''' <summary> Phony file/device (for example /dev/null) </summary>
            Phony = &H4

            ''' <summary> File should be rewound to write header </summary>
            Rewind = &H8

            ''' <summary> Is file bit-reversed? </summary>
            Bit_Rev = &H10

            ''' <summary> Is file nibble-reversed? </summary>
            Nib_Rev = &H20

            ''' <summary> Is file format endian? </summary>
            Endian = &H40

            ''' <summary> For endian file format, is it big endian? </summary>
            Endian_Big = &H80

            ''' <summary> Do channel restrictions allow mono? </summary>
            Mono = &H100

            ''' <summary> Do channel restrictions allow stereo? </summary>
            Stereo = &H200

            ''' <summary> Do channel restrictions allow quad? </summary>
            Quad = &H400

        End Enum

#End Region

#Region "Properties - Local Storage"

        ''' <summary> Underlying Swig Storage. </summary>
        ''' <value> Underlying Swig Storage. </value>
        Public ReadOnly Property SwigStorage As sox_format_handler_t
            Get
                Return _SwigStorage
            End Get
        End Property
        <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        Protected Property _SwigStorage As sox_format_handler_t

#End Region

#Region "Properties - Swig Wrapper"

        ''' <summary> Version Code checked on Load of the Format Plugin. </summary>
        ''' <value> Version Code checked on Load of the Format Plugin. </value>
        Public ReadOnly Property Version_Code As UInteger
            Get
                Return _SwigStorage.sox_lib_version_code
            End Get
        End Property

        ''' <summary> Short Description of Format. </summary>
        ''' <value> The description. </value>
        Public ReadOnly Property Description As String
            Get
                Return _SwigStorage.description
            End Get
        End Property

        ''' <summary> Names of Extensions associated with the Format Handler. </summary>
        ''' <value> The names. </value>
        Public ReadOnly Property Names As ReadOnlyCollection(Of String)
            Get
                Dim ret As ReadOnlyCollection(Of String)
                ret = CHelper.Umg_PtrToList(Of String)(_SwigStorage.names.GetswigCPtr)
                Return ret
            End Get
        End Property

        ''' <summary> Single String list of names. </summary>
        ''' <value> The name. </value>
        Public ReadOnly Property Name As String
            Get
                Dim ret As String = ""
                For Each item As String In Names
                    ret &= item & " "
                Next
                ret = ret.TrimEnd(CType(" ", Char))
                Return ret
            End Get
        End Property

        ''' <summary> File based flags. </summary>
        ''' <value> The flags. </value>
        Public ReadOnly Property Flags As FileFlags
            Get
                Dim ret As FileFlags
                [Enum].TryParse(Of FileFlags)(_SwigStorage.flags.ToString, ret)
                Return ret
            End Get
        End Property

        ''' <summary> Format Header / Name / Description. </summary>
        ''' <value> The header. </value>
        Public ReadOnly Property Header As String
            Get
                Return Description & " (" & Name & ")"
            End Get
        End Property

#End Region

#Region "Properties - Function Pointers"

        ''' <summary> Function Pointer to StartRead Called to initialize reader (decoder) </summary>
        ''' <value> The fp start read. </value>
        Public ReadOnly Property FP_StartRead As SWIGTYPE_p_f_p_struct_sox_format_t__int
            Get
                Return _SwigStorage.startread
            End Get
        End Property

        ''' <summary> Function Pointer to Read Called to read (decode) a block of samples. </summary>
        ''' <value> The fp read. </value>
        Public ReadOnly Property FP_Read As SWIGTYPE_p_f_p_struct_sox_format_t_p_int_size_t__size_t
            Get
                Return _SwigStorage.read
            End Get
        End Property

        ''' <summary>
        '''  Function Pointer to StopRead called to close reader (decoder); may be null if no closing
        '''  necessary.
        ''' </summary>
        ''' <value> The fp stop read. </value>
        Public ReadOnly Property FP_StopRead As SWIGTYPE_p_f_p_struct_sox_format_t__int
            Get
                Return _SwigStorage.stopread
            End Get
        End Property

        ''' <summary> Function Pointer to StartWrite called to initialize writer (encoder) </summary>
        ''' <value> The fp start write. </value>
        Public ReadOnly Property FP_StartWrite As SWIGTYPE_p_f_p_struct_sox_format_t__int
            Get
                Return _SwigStorage.startwrite
            End Get
        End Property

        ''' <summary> Function Pointer to Write called to write (encode) a block of samples. </summary>
        ''' <value> The fp write. </value>
        Public ReadOnly Property FP_Write As SWIGTYPE_p_f_p_struct_sox_format_t_p_q_const__int_size_t__size_t
            Get
                Return _SwigStorage.write
            End Get
        End Property

        ''' <summary>
        '''  Function Pointer to StopWrite called to close writer (decoder); may be null if no closing
        '''  necessary.
        ''' </summary>
        ''' <value> The fp stop write. </value>
        Public ReadOnly Property FP_StopWrite As SWIGTYPE_p_f_p_struct_sox_format_t__int
            Get
                Return _SwigStorage.stopwrite
            End Get
        End Property

        ''' <summary>
        '''  Function Pointer to Seek called to reposition reader; may be null if not supported.
        ''' </summary>
        ''' <value> The fp seek. </value>
        Public ReadOnly Property FP_Seek As SWIGTYPE_p_f_p_struct_sox_format_t_unsigned_long__int
            Get
                Return _SwigStorage.seek
            End Get
        End Property

#End Region

#Region "Constructors"

        ''' <summary> Default Constructor. </summary>
        Public Sub New()
            _SwigStorage = New sox_format_handler_t
        End Sub

        ''' <summary> Default Constructor. </summary>
        ''' <param name="storage"> The storage. </param>
        Public Sub New(storage As sox_format_handler_t)
            _SwigStorage = storage
        End Sub

        ''' <summary> Default Constructor. </summary>
        ''' <param name="pointer"> The pointer. </param>
        Public Sub New(pointer As SWIGTYPE_p_sox_format_handler_t)
            _SwigStorage = New sox_format_handler_t(pointer.GetswigCPtr())
        End Sub

#End Region

#Region "Destructors"

        ''' <summary> Destructor. </summary>
        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            Dispose()
        End Sub

        ''' <summary> Dispose of Memory Storage. </summary>
        Public Sub Dispose() Implements IDisposable.Dispose
            If _SwigStorage IsNot Nothing Then _SwigStorage.Dispose()
        End Sub

#End Region

#Region "Methods"

        ''' <summary> Temporary Delegate used for retrieving Format Handler Information </summary>
        Protected Delegate Function GetHandlerDelegate() As IntPtr

        ''' <summary> Get Full List of Format Handlers. </summary>
        ''' <exception cref="SoxException"> Thrown when a Sox error condition occurs. </exception>
        ''' <returns>
        '''  An enumerator that allows foreach to be used to process the handlers in this collection.
        ''' </returns>
        Public Shared Function GetHandlers() As IEnumerable(Of FormatHandler)
            Dim ret As New List(Of FormatHandler)

            Dim tmpformattab As sox_format_tab_t
            Try
                tmpformattab = libsox.sox_get_format_fns()
            Catch ex As Exception
                Throw New SoxException("Error Getting the Format Tab Values", ex)
            End Try
            While tmpformattab IsNot Nothing
                Dim dele1 As GetHandlerDelegate = CType(Marshal.GetDelegateForFunctionPointer(tmpformattab.fn.GetswigCPtr, GetType(GetHandlerDelegate)), GetHandlerDelegate)
                Dim ptr1 As IntPtr = dele1.Invoke
                Dim newitem As New FormatHandler(New sox_format_handler_t(ptr1))
                ret.Add(newitem)

                Dim tmpptr As IntPtr = tmpformattab.GetswigCPtr
                tmpptr = tmpptr + IntPtr.Size ' Name
                tmpptr = tmpptr + IntPtr.Size ' Function Pointer
                tmpformattab = New sox_format_tab_t(tmpptr)
                If tmpformattab.fn Is Nothing Then tmpformattab = Nothing
            End While

            Dim ret2 As IEnumerable(Of FormatHandler) = ret.OrderBy(Function(x) x.Description)
            Return ret2
        End Function

        ''' <summary> Get a List of all available names. </summary>
        ''' <returns> The headers. </returns>
        Public Shared Function GetHeaders() As List(Of String)
            Dim tmplst1 As IEnumerable(Of FormatHandler) = GetHandlers()
            Dim ret As New List(Of String)
            For Each item As FormatHandler In tmplst1
                ret.Add(item.Header)
            Next
            Return ret
        End Function

#End Region

    End Class

End Namespace
