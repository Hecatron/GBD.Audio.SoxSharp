Imports GBD.Audio.SoxSharp.swig
Imports GBD.Audio.SoxSharp.UnmanagedHelper

Namespace DataTypes

    ''' <summary> Global parameters - for effects / formats. </summary>
    Public Class GlobalInfo
        Implements IDisposable

#Region "Properties - Local Storage"

        ''' <summary> Underlying Swig Storage. </summary>
        ''' <value> Underlying Swig Storage. </value>
        Public ReadOnly Property SwigStorage As sox_globals_t
            Get
                Return _SwigStorage
            End Get
        End Property
        <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        Protected Property _SwigStorage As sox_globals_t

#End Region

#Region "Properties - Swig Wrapper"

        ''' <summary> Verbosity messages are only written if globals.verbosity >= message.level. </summary>
        ''' <value> The verbosity level. </value>
        Public Property Verbosity As UInteger
            Get
                Return _SwigStorage.verbosity
            End Get
            Set(value As UInteger)
                _SwigStorage.verbosity = value
            End Set
        End Property

        ''' <summary> Client-Specified message output callback. </summary>
        ''' <value> The output message handler. </value>
        Public ReadOnly Property Output_Message_Handler As SWIGTYPE_p_f_unsigned_int_p_q_const__char_p_q_const__char_va_list__void
            Get
                Return _SwigStorage.output_message_handler
            End Get
        End Property

        ''' <summary> True to use pre-determined timestamps and PRNG seed. </summary>
        ''' <value> True to use pre-determined timestamps and PRNG seed. </value>
        Public Property Repeatable As Boolean
            Get
                Return CHelper.SoxBoolToBoolean(_SwigStorage.repeatable)
            End Get
            Set(value As Boolean)
                _SwigStorage.repeatable = CHelper.BooleanToSoxBool(value)
            End Set
        End Property

        ''' <summary>
        '''  Default size (in bytes) used by libSoX for blocks of sample data. Plugins should use
        '''  similarly-sized buffers to get best performance.
        ''' </summary>
        ''' <value> The size of the buffer. </value>
        Public Property BufferSize As UInteger
            Get
                Return _SwigStorage.bufsiz
            End Get
            Set(value As UInteger)
                _SwigStorage.bufsiz = value
            End Set
        End Property

        ''' <summary>
        '''  Default size (in bytes) used by libSoX for blocks of input sample data. Plugins should use
        '''  similarly-sized buffers to get best performance.
        ''' </summary>
        ''' <value> The size of the input buffer. </value>
        Public Property InputBufferSize As UInteger
            Get
                Return _SwigStorage.input_bufsiz
            End Get
            Set(value As UInteger)
                _SwigStorage.input_bufsiz = value
            End Set
        End Property

        ''' <summary> Can be used to re-seed libSoX's PRNG. </summary>
        ''' <value> Can be used to re-seed libSoX's PRNG. </value>
        Public Property Ranqd1 As Integer
            Get
                Return _SwigStorage.ranqd1
            End Get
            Set(value As Integer)
                _SwigStorage.ranqd1 = value
            End Set
        End Property

        ''' <summary> Private: tracks the name of the handler currently using stdin. </summary>
        ''' <value> Private: tracks the name of the handler currently using stdin. </value>
        Public Property StdIn_In_Useby As String
            Get
                Return _SwigStorage.stdin_in_use_by
            End Get
            Set(value As String)
                _SwigStorage.stdin_in_use_by = value
            End Set
        End Property

        ''' <summary> Private: tracks the name of the handler currently using stdout. </summary>
        ''' <value> Private: tracks the name of the handler currently using stdout. </value>
        Public Property Stdout_In_Useby As String
            Get
                Return _SwigStorage.stdout_in_use_by
            End Get
            Set(value As String)
                _SwigStorage.stdout_in_use_by = value
            End Set
        End Property

        ''' <summary>
        '''  Private: tracks the name of the handler currently writing an output message.
        ''' </summary>
        ''' <value> Private: tracks the name of the handler currently writing an output message. </value>
        Public Property Subsystem As String
            Get
                Return _SwigStorage.subsystem
            End Get
            Set(value As String)
                _SwigStorage.subsystem = value
            End Set
        End Property

        ''' <summary> Private: client-configured path to use for temporary files. </summary>
        ''' <value> Private: client-configured path to use for temporary files. </value>
        Public Property tmp_path As String
            Get
                Return _SwigStorage.tmp_path
            End Get
            Set(value As String)
                _SwigStorage.tmp_path = value
            End Set
        End Property

        ''' <summary> Private: true if client has requested use of 'magic' file-type detection. </summary>
        ''' <value> Private: true if client has requested use of 'magic' file-type detection. </value>
        Public Property UseMagic As Boolean
            Get
                Return CHelper.SoxBoolToBoolean(_SwigStorage.use_magic)
            End Get
            Set(value As Boolean)
                _SwigStorage.use_magic = CHelper.BooleanToSoxBool(value)
            End Set
        End Property

        ''' <summary> Private: true if client has requested parallel effects processing. </summary>
        ''' <value> Private: true if client has requested parallel effects processing. </value>
        Public Property UseThreads As Boolean
            Get
                Return CHelper.SoxBoolToBoolean(_SwigStorage.use_threads)
            End Get
            Set(value As Boolean)
                _SwigStorage.use_threads = CHelper.BooleanToSoxBool(value)
            End Set
        End Property

#End Region

#Region "Constructors"

        ''' <summary> Default Constructor. </summary>
        Public Sub New()
            _SwigStorage = New sox_globals_t
        End Sub

        ''' <summary> Default Constructor. </summary>
        ''' <param name="storage"> Use this as the swig storage value. </param>
        Public Sub New(storage As sox_globals_t)
            _SwigStorage = storage
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

        ''' <summary> Get List of Global Infos. </summary>
        ''' <returns> A list of available inbuilt globals info. </returns>
        Public Shared Function GetGlobalInfo() As GlobalInfo
            Dim tmpval As sox_globals_t = libsox.sox_get_globals
            Dim ret As New GlobalInfo(tmpval)
            Return ret
        End Function

#End Region

    End Class

End Namespace
