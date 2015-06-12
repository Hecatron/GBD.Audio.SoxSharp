Imports GBD.Audio.SoxSharp.DataTypes
Imports GBD.Audio.SoxSharp.swig
Imports GBD.Audio.SoxSharp.UnmanagedHelper

Namespace IO

    ''' <summary> Base Class for Sox Format. </summary>
    Public Class FormatBase

#Region "Types"

        ''' <summary> Type of Open. </summary>
        Public Enum OpenMode As Byte
            Undefined = 0
            Read = 1
            Write = 2
        End Enum

#End Region

#Region "Properties - Local Storage"

        ''' <summary> Underlying Swig Storage. </summary>
        ''' <value> Underlying Swig Storage. </value>
        Public ReadOnly Property SwigStorage As sox_format_t
            Get
                Return _SwigStorage
            End Get
        End Property
        <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        Protected Property _SwigStorage As sox_format_t

        ''' <summary> If the file is open. </summary>
        ''' <value> If the file is open. </value>
        Public ReadOnly Property IsOpen As Boolean
            Get
                Return _IsOpen
            End Get
        End Property
        <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        Protected Property _IsOpen As Boolean = False

        ''' <summary> Handle used for Opening / Closing the sox_format_t. </summary>
        ''' <value> The open handle. </value>
        Protected Property OpenHandle As SWIGTYPE_p_sox_format_t

#End Region

#Region "Properties - Swig Wrapper"

        ''' <summary> File Name. </summary>
        ''' <value> The name of the file. </value>
        Public Property FileName As String
            Get
                If _SwigStorage Is Nothing Then Return Nothing
                Return _SwigStorage.filename
            End Get
            Set(value As String)
                _SwigStorage.filename = value
            End Set
        End Property

        ''' <summary> Signal Information on the opened stream. </summary>
        ''' <value> Information describing the signal. </value>
        Public ReadOnly Property SignalInfo As SignalInfo
            Get
                Return _SignalInfo
            End Get
        End Property
        <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        Protected Property _SignalInfo As SignalInfo

        ''' <summary> Encoding Information on the File. </summary>
        ''' <value> Information describing the encoding. </value>
        Public ReadOnly Property EncodingInfo As EncodingInfo
            Get
                Return _EncodingInfo
            End Get
        End Property
        <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        Protected Property _EncodingInfo As EncodingInfo

        ''' <summary> Type of File. </summary>
        ''' <value> The type of the file. </value>
        Public ReadOnly Property FileType As String
            Get
                If _SwigStorage Is Nothing Then Return Nothing
                Return _SwigStorage.filetype
            End Get
        End Property

        ''' <summary> Out of Bound Data, comments, instrument info, loop info. </summary>
        ''' <value> Information describing the oob. </value>
        Public ReadOnly Property OOBData As OutOfBound
            Get
                Return _OOBData
            End Get
        End Property
        <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        Protected Property _OOBData As OutOfBound

        ''' <summary> Can seek on this file. </summary>
        ''' <value> Can seek on this file. </value>
        Public Property IsSeekable As Boolean
            Get
                Return CHelper.SoxBoolToBoolean(_SwigStorage.seekable)
            End Get
            Set(value As Boolean)
                _SwigStorage.seekable = CHelper.BooleanToSoxBool(value)
            End Set
        End Property

        ''' <summary> If the File has been opened Read / Write. </summary>
        ''' <value> If the File has been opened Read / Write. </value>
        Public Property Mode As OpenMode
            Get
                If _SwigStorage.mode = "r" Then Return OpenMode.Read
                If _SwigStorage.mode = "w" Then Return OpenMode.Write
                Return OpenMode.Undefined
            End Get
            Set(value As OpenMode)
                If value = OpenMode.Read Then _SwigStorage.mode = CChar("r")
                If value = OpenMode.Write Then _SwigStorage.mode = CChar("w")
            End Set
        End Property

        ''' <summary> Samples * Channels written to file. </summary>
        ''' <value> Samples * Channels written to file. </value>
        Public Property OLength As UInteger
            Get
                Return _SwigStorage.olength
            End Get
            Set(value As UInteger)
                _SwigStorage.olength = value
            End Set
        End Property

        ''' <summary> Incremented if clipping occurs. </summary>
        ''' <value> Incremented if clipping occurs. </value>
        Public Property Clips As UInteger
            Get
                Return _SwigStorage.clips
            End Get
            Set(value As UInteger)
                _SwigStorage.clips = value
            End Set
        End Property

        ''' <summary> Failure error code. </summary>
        ''' <value> The error code. </value>
        Public Property ErrorCode As Integer
            Get
                Return _SwigStorage.sox_errno
            End Get
            Set(value As Integer)
                _SwigStorage.sox_errno = value
            End Set
        End Property

        ''' <summary> Failure error text. </summary>
        ''' <value> The error string. </value>
        Public Property ErrorString As String
            Get
                Return _SwigStorage.sox_errstr
            End Get
            Set(value As String)
                _SwigStorage.sox_errstr = value
            End Set
        End Property

        ''' <summary> File stream pointer. </summary>
        ''' <value> File Stream pointer. </value>
        Public ReadOnly Property FilePointer As SWIGTYPE_p_void
            Get
                Return _SwigStorage.fp
            End Get
        End Property

        ''' <summary> Stores whether this is a file, pipe or URL. </summary>
        ''' <value> The type of the i/o. </value>
        Public Property IO_Type As lsx_io_type
            Get
                Return _SwigStorage.io_type
            End Get
            Set(value As lsx_io_type)
                _SwigStorage.io_type = value
            End Set
        End Property

        ''' <summary> Current offset within file. </summary>
        ''' <value> The tell off. </value>
        Public Property Tell_Off As UInteger
            Get
                Return _SwigStorage.tell_off
            End Get
            Set(value As UInteger)
                _SwigStorage.tell_off = value
            End Set
        End Property

        ''' <summary>
        '''  Data Start - Offset at which headers end and sound data begins (set by lsx_check_read_params)
        ''' </summary>
        ''' <value> The data start. </value>
        Public Property DataStart As UInteger
            Get
                Return _SwigStorage.data_start
            End Get
            Set(value As UInteger)
                _SwigStorage.data_start = value
            End Set
        End Property

        ''' <summary> Format handler for this file. </summary>
        ''' <value> The handler. </value>
        Public ReadOnly Property Handler As FormatHandler
            Get
                Return _Handler
            End Get
        End Property
        <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        Protected Property _Handler As FormatHandler

        ''' <summary> Format handler's private data area. </summary>
        ''' <value> The handler private area. </value>
        Public ReadOnly Property Handler_PrivateArea As SWIGTYPE_p_void
            Get
                Return _SwigStorage.priv
            End Get
        End Property

#End Region

    End Class

End Namespace
