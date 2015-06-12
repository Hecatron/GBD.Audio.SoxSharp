Imports GBD.Audio.SoxSharp.DataTypes
Imports GBD.Audio.SoxSharp.swig

Namespace Effect

    ''' <summary> Effect handler information. </summary>
    Public Class EffectHandler
        Implements IDisposable

#Region "Types"

        ''' <summary> Effect Flags. </summary>
        <Flags()> _
        Public Enum EffectFlags

            ''' <summary> Effect will alter the number of channels </summary> 
            Channel = 1

            ''' <summary> Effect will alter the sample rate </summary>
            Rate = 2

            ''' <summary>
            '''  Effect does its own calculation of output sample precision
            '''  (otherwise a default value is taken, depending on the presence of SOX_EFF_MODIFY)
            ''' </summary>
            Precision = 4

            ''' <summary>
            ''' Effect might alter audio length (as measured in time units, not necessarily in samples)
            ''' </summary>
            Length = 8

            ''' <summary> Effect handles multiple channels internally </summary>
            MultiChannel = 16

            ''' <summary> Effect does nothing (can be optimized out of chain) </summary>
            Null = 32

            ''' <summary> Effect will soon be removed from SoX </summary>
            Deprecated = 64

            ''' <summary> Effect does not support gain -r </summary>
            Gain = 128

            ''' <summary>
            '''  Effect does not modify sample values (but might remove or duplicate samples or insert zeros)
            ''' </summary>
            Modify = 256

            ''' <summary> Effect is experimental/incomplete </summary>
            Alpha = 512

            ''' <summary> Effect present in libSoX but not valid for use by SoX command-line tools </summary>
            Internal = 1024

        End Enum

#End Region

#Region "Properties - Local Storage"

        ''' <summary> Underlying Swig Storage. </summary>
        ''' <value> Underlying Swig Storage. </value>
        Public ReadOnly Property SwigStorage As sox_effect_handler_t
            Get
                Return _SwigStorage
            End Get
        End Property
        Protected Property _SwigStorage As sox_effect_handler_t

        ''' <summary> Locking Object used for FindEffect. </summary>
        ''' <value> The find effect synchronise lock. </value>
        Protected Shared Property FindEffect_SyncLock As New Object

#End Region

#Region "Properties - Swig Wrapper"

        ''' <summary> Effect Name. </summary>
        ''' <value> Effect Name. </value>
        Public ReadOnly Property Name As String
            Get
                Return _SwigStorage.name
            End Get
        End Property

        ''' <summary> Short explanation of parameters accepted by effect. </summary>
        ''' <value> Short explanation of parameters accepted by effect. </value>
        Public ReadOnly Property Usage As String
            Get
                Return _SwigStorage.usage
            End Get
        End Property

        ''' <summary> Combination of SOX_EFF_* flags. </summary>
        ''' <value> Combination of SOX_EFF_* flags. </value>
        Public ReadOnly Property Flags As EffectFlags
            Get
                Dim ret As EffectFlags
                [Enum].TryParse(Of EffectFlags)(_SwigStorage.flags.ToString, ret)
                Return ret
            End Get
        End Property

        ''' <summary> Called to parse command-line arguments (called once per effect) </summary>
        ''' <value> Called to parse command-line arguments (called once per effect) </value>
        Public ReadOnly Property FP_GetOpts As SWIGTYPE_p_f_p_struct_sox_effect_t_int_a___p_char__int
            Get
                Return _SwigStorage.getopts
            End Get
        End Property

        ''' <summary> Called to initialize effect (called once per flow) </summary>
        ''' <value> Called to initialize effect (called once per flow) </value>
        Public ReadOnly Property FP_Start As SWIGTYPE_p_f_p_struct_sox_effect_t__int
            Get
                Return _SwigStorage.start
            End Get
        End Property

        ''' <summary> Called to process samples. </summary>
        ''' <value> Called to process samples. </value>
        Public ReadOnly Property FP_Flow As SWIGTYPE_p_f_p_struct_sox_effect_t_p_q_const__int_p_int_p_size_t_p_size_t__int
            Get
                Return _SwigStorage.flow
            End Get
        End Property

        ''' <summary> Called to finish getting output after input is complete. </summary>
        ''' <value> Called to finish getting output after input is complete. </value>
        Public ReadOnly Property FP_Drain As SWIGTYPE_p_f_p_struct_sox_effect_t_p_int_p_size_t__int
            Get
                Return _SwigStorage.drain
            End Get
        End Property

        ''' <summary> Called to shut down effect (called once per flow). </summary>
        ''' <value> Called to shut down effect (called once per flow). </value>
        Public ReadOnly Property FP_Stop As SWIGTYPE_p_f_p_struct_sox_effect_t__int
            Get
                Return _SwigStorage.stop
            End Get
        End Property

        ''' <summary> Called to shut down effect (called once per effect). </summary>
        ''' <value> Called to shut down effect (called once per effect). </value>
        Public ReadOnly Property FP_Kill As SWIGTYPE_p_f_p_struct_sox_effect_t__int
            Get
                Return _SwigStorage.kill
            End Get
        End Property

        ''' <summary> Size of private data SoX should pre-allocate for effect. </summary>
        ''' <value> Size of private data SoX should pre-allocate for effect. </value>
        Public ReadOnly Property PrivateSize As UInteger
            Get
                Return _SwigStorage.priv_size
            End Get
        End Property

#End Region

#Region "Constructors"

        ''' <summary> Default Constructor. </summary>
        Public Sub New()
            _SwigStorage = New sox_effect_handler_t
        End Sub

        ''' <summary> Default Constructor. </summary>
        ''' <param name="storage"> Use this as the swig storage value. </param>
        Public Sub New(storage As sox_effect_handler_t)
            _SwigStorage = storage
        End Sub

        ''' <summary> Default Constructor. </summary>
        ''' <param name="storage"> Use this as the swig storage value. </param>
        Public Sub New(storage As SWIGTYPE_p_sox_effect_handler_t)
            _SwigStorage = New sox_effect_handler_t(storage.GetswigCPtr)
        End Sub

        ''' <summary> Default Constructor. </summary>
        ''' <param name="effectname"> The effectname. </param>
        Public Sub New(effectname As String)
            SetupHandler(effectname)
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

        ''' <summary> Find Effect Handler based on name. </summary>
        ''' <param name="effectname"> The effectname. </param>
        Public Sub SetupHandler(effectname As String)
            SyncLock FindEffect_SyncLock
                Dim tmpptr As IntPtr
                Try
                    tmpptr = libsox.sox_find_effect(effectname).GetswigCPtr()
                Catch ex As Exception
                    Throw New SoxException("Error Finding the Effect Handler", ex)
                End Try
                Dim tmphndlr As New sox_effect_handler_t(tmpptr)
                _SwigStorage = tmphndlr
            End SyncLock
        End Sub

        ''' <summary> Get the Swig Form of Pointer for the Handler. </summary>
        ''' <returns> The swig pointer. </returns>
        Public Function GetSwigPointer() As SWIGTYPE_p_sox_effect_handler_t
            Dim ptr As New SWIGTYPE_p_sox_effect_handler_t(_SwigStorage.GetswigCPtr)
            Return ptr
        End Function

        ''' <summary> Find Effect Handler based on name. </summary>
        ''' <param name="effectname"> The effectname. </param>
        ''' <returns> The found handler. </returns>
        Public Shared Function FindHandler(effectname As String) As EffectHandler
            SyncLock FindEffect_SyncLock
                Dim tmpptr As IntPtr
                Try
                    tmpptr = libsox.sox_find_effect(effectname).GetswigCPtr()
                Catch ex As Exception
                    Throw New SoxException("Error Finding the Effect Handler", ex)
                End Try
                Dim tmphndlr As New sox_effect_handler_t(tmpptr)
                Dim retval As New EffectHandler(tmphndlr)
                Return retval
            End SyncLock
        End Function

#End Region

    End Class

End Namespace
