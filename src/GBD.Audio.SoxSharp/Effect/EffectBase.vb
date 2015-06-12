Imports System.Reflection
Imports System.Runtime.InteropServices
Imports System.Xml.Serialization
Imports GBD.Audio.SoxSharp.DataTypes
Imports GBD.Audio.SoxSharp.Effect.Mastering
Imports GBD.Audio.SoxSharp.Effect.Misc
Imports GBD.Audio.SoxSharp.Effect.Mixing
Imports GBD.Audio.SoxSharp.Effect.Tone
Imports GBD.Audio.SoxSharp.Effect.Volume
Imports GBD.Audio.SoxSharp.swig
Imports GBD.Audio.SoxSharp.UnmanagedHelper

Namespace Effect

    ''' <summary> Base Class for All Effects. </summary>
    <XmlInclude(GetType(EffectRate))> _
    <XmlInclude(GetType(EffectNativeFileIn))> _
    <XmlInclude(GetType(EffectNativeFileOut))> _
    <XmlInclude(GetType(EffectSpeexDSP))> _
    <XmlInclude(GetType(EffectChannels))> _
    <XmlInclude(GetType(EffectRemix))> _
    <XmlInclude(GetType(EffectAllPass))> _
    <XmlInclude(GetType(EffectHighPass))> _
    <XmlInclude(GetType(EffectCompand))> _
    Public Class EffectBase
        Implements IDisposable

#Region "Properties - Local Storage"

        ''' <summary> The Effect Name. </summary>
        ''' <value> The Effect Name. </value>
        Public Overridable ReadOnly Property Name As String
            Get
                Return "Base Effect Class"
            End Get
        End Property

        ''' <summary> Underlying Swig Storage. </summary>
        ''' <value> Underlying Swig Storage. </value>
        Public ReadOnly Property SwigStorage As sox_effect_t
            Get
                Return _SwigStorage
            End Get
        End Property
        <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        Protected Property _SwigStorage As sox_effect_t

        ''' <summary> Name of the Underlying Sox Effect to open. </summary>
        ''' <value> Name of the Underlying Sox Effect to open. </value>
        Public ReadOnly Property SoxEffectName As String
            Get
                Return _SoxEffectName
            End Get
        End Property
        <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        Protected Overridable Property _SoxEffectName As String

        ''' <summary> If the Effect has been opened with sox. </summary>
        ''' <value> If the Effect has been opened with sox. </value>
        Public ReadOnly Property IsOpen As Boolean
            Get
                Return _IsOpen
            End Get
        End Property
        <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        Protected Property _IsOpen As Boolean

        ''' <summary> Keep Track of Effects that have been attached to a chain. </summary>
        ''' <value> If the effect is attached. </value>
        <XmlIgnore()> _
        Public Property IsAttached As Boolean = False

        ''' <summary> List of Added Options. </summary>
        ''' <value> List of Added Options. </value>
        Protected Property Options As New CArray(Of String)

        ''' <summary> Return ToString. </summary>
        ''' <returns> A String that represents this object. </returns>
        Public Overrides Function ToString() As String
            Return Name
        End Function

#End Region

#Region "Properties - Swig Wrapper"

        ''' <summary> Global Effect Parameters. </summary>
        ''' <value> Global Effect Parameters. </value>
        Public Property GlobalInfo As EffectGlobals

        ''' <summary> Information about the incoming data stream. </summary>
        ''' <value> The signal input. </value>
        Public Property Signal_Input As SignalInfo

        ''' <summary> Information about the outgoing data stream. </summary>
        ''' <value> The signal output. </value>
        Public Property Signal_Output As SignalInfo

        ''' <summary> Used to store the effects previous signal state before attachment. </summary>
        ''' <value> Used to store the effects previous signal state before attachment. </value>
        <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        Protected Property UnAtt_SigIn As SignalInfo
        <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        Protected Property UnAtt_SigOut As SignalInfo

        ''' <summary> Information about the incoming data encoding. </summary>
        ''' <value> The encoding input. </value>
        Public Property Encoding_Input As EncodingInfo

        ''' <summary> Information about the outgoing data encoding. </summary>
        ''' <value> The encoding output. </value>
        Public Property Encoding_Output As EncodingInfo

        ''' <summary> The handler for this effect. </summary>
        ''' <value> The handler. </value>
        <XmlIgnore()> _
        Public Property Handler As EffectHandler

        ''' <summary> Output Buffer. </summary>
        ''' <value> A Buffer for output data. </value>
        <XmlIgnore()> _
        Public ReadOnly Property OutputBuffer As SWIGTYPE_p_int
            Get
                Return _SwigStorage.obuf
            End Get
        End Property

        ''' <summary> Output Buffer: start of valid data section. </summary>
        ''' <value> The output buffer begin. </value>
        <XmlIgnore()> _
        Public Property OutputBuffer_Begin As UInteger
            Get
                Return _SwigStorage.obeg
            End Get
            Set(value As UInteger)
                _SwigStorage.obeg = value
            End Set
        End Property

        ''' <summary>
        '''  Output Buffer: one past valid data section (oend-obeg is length of current content)
        ''' </summary>
        ''' <value> The output buffer end. </value>
        <XmlIgnore()> _
        Public Property OutputBuffer_End As UInteger
            Get
                Return _SwigStorage.oend
            End Get
            Set(value As UInteger)
                _SwigStorage.oend = value
            End Set
        End Property

        ''' <summary>
        '''  Minimum input buffer content required for calling this effect's flow function; set via
        '''  lsx_effect_set_imin()
        ''' </summary>
        ''' <value> The minimum value. </value>
        <XmlIgnore()> _
        Public Property IMin As UInteger
            Get
                Return _SwigStorage.imin
            End Get
            Set(value As UInteger)
                _SwigStorage.imin = value
            End Set
        End Property

        ''' <summary> Increment if clipping occurs. </summary>
        ''' <value> Increment if clipping occurs. </value>
        <XmlIgnore()> _
        Public Property Clips As UInteger
            Get
                Return _SwigStorage.clips
            End Get
            Set(value As UInteger)
                _SwigStorage.clips = value
            End Set
        End Property

        ''' <summary> 1 if MCHAN, number of chans otherwise. </summary>
        ''' <value> The number of flows. </value>
        <XmlIgnore()> _
        Public Property Flows As UInteger
            Get
                Return _SwigStorage.flows
            End Get
            Set(value As UInteger)
                _SwigStorage.flows = value
            End Set
        End Property

        ''' <summary> Flow number. </summary>
        ''' <value> Flow number. </value>
        <XmlIgnore()> _
        Public Property Flow As UInteger
            Get
                Return _SwigStorage.flow
            End Get
            Set(value As UInteger)
                _SwigStorage.flow = value
            End Set
        End Property

        ''' <summary> Effect's private data area (each flow has a separate copy) </summary>
        ''' <value> The private store. </value>
        <XmlIgnore()> _
        Public Property Private_Store As SWIGTYPE_p_void
            Get
                Return _SwigStorage.priv
            End Get
            Set(value As SWIGTYPE_p_void)
                _SwigStorage.priv = value
            End Set
        End Property

        ''' <summary> SyncLock for Attaching Effects. </summary>
        ''' <value> The attach synchronise lock. </value>
        Protected Property Attach_SyncLock As New Object

#End Region

#Region "Constructors"

        ''' <summary> Default Constructor. </summary>
        Public Sub New()
        End Sub

        ''' <summary> Default Constructor. </summary>
        ''' <param name="name"> The Effect Name. </param>
        Public Sub New(name As String)
            _SoxEffectName = name
            ' _SwigStorage is init via Open / Close
        End Sub

        ''' <summary> Default Constructor. </summary>
        ''' <param name="storage"> Use this as the swig storage value. </param>
        Public Sub New(storage As sox_effect_t)
            _SwigStorage = storage
            SetupProperties()
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
            Close()
        End Sub

#End Region

#Region "Methods"

        ''' <summary> Set Default Options. </summary>
        Public Overridable Sub SetDefaults()
            Options.Clear()
        End Sub

        ''' <summary> Setup Options for the Effect. </summary>
        Public Overridable Sub SetupOptionList()
        End Sub

        ''' <summary> Open the Effect. </summary>
        ''' <exception cref="InvalidOperationException"> Thrown when the requested operation is invalid. </exception>
        ''' <exception cref="ArgumentException">         Thrown when one or more arguments have
        '''                                              unsupported or illegal values. </exception>
        ''' <exception cref="SoxException">              Thrown when a Sox error condition occurs. </exception>
        Public Overridable Sub Open()
            If _IsOpen = True Then Throw New InvalidOperationException("Effect is already Open")
            If String.IsNullOrEmpty(SoxEffectName) Then Throw New ArgumentException("Effect Name not specified")
            ' Setup a New Handler
            Handler = New EffectHandler(SoxEffectName)
            ' Create the effect
            Dim effptr As SWIGTYPE_p_sox_effect_t
            Try
                effptr = libsox.sox_create_effect(Handler.GetSwigPointer)
            Catch ex As Exception
                Throw New SoxException("Error Creating the Effect", ex)
            End Try
            _SwigStorage = New sox_effect_t(effptr.GetswigCPtr)

            ' Ignore the Signal Input returned by sox_create_effect
            SetupProperties()
            Signal_Input = Nothing

            ' Setup Options / Add to the Effect
            SetupOptionList()

            Dim tmpptr As IntPtr = Options.AllocateAndCopy()
            Dim swigptr2 As New SWIGTYPE_p_p_char(tmpptr)
            Try
                libsox.sox_effect_options(effptr, Options.Count, swigptr2)
            Catch ex As Exception
                Throw New SoxException("Error Setting The Effect Options", ex)
            End Try
            Options.DeAllocate()

            _IsOpen = True
        End Sub

        ''' <summary> Close the Effect. </summary>
        Public Overridable Sub Close()
            If _IsOpen = False Then Exit Sub
            If String.IsNullOrEmpty(SoxEffectName) Then Throw New ArgumentException("Effect Name not specified")
            ' Delete the Effect
            If _SwigStorage IsNot Nothing Then
                If IsAttached = False Then
                    Dim effptr As New SWIGTYPE_p_sox_effect_t(_SwigStorage.GetswigCPtr)
                    Try
                        libsox.sox_delete_effect(effptr)
                    Catch ex As Exception
                        Throw New SoxException("Error Deleting the Effect", ex)
                    End Try
                Else
                    ' Free Memory Allocated to Effect Pointer
                    ' Once an effect is attached the chain then takes ownership, but we still need to free a pointer
                    Dim tmpptr As HandleRef = New HandleRef(Me, _SwigStorage.GetswigCPtr)
                    libsoxPINVOKE.delete_sox_effect_t(tmpptr)
                End If
            End If
            If Handler IsNot Nothing Then Handler.Dispose()
            If _SwigStorage IsNot Nothing Then _SwigStorage.Dispose()
            _SwigStorage = Nothing
            _IsOpen = False
            If _IsAttached = True Then
                Signal_Input = UnAtt_SigIn
                Signal_Output = UnAtt_SigIn
                _IsAttached = False
            End If
        End Sub

        ''' <summary> Attach Effect to the Chain. </summary>
        ''' <param name="chain"> The chain to attach to. </param>
        Public Overridable Sub Attach(chain As EffectChain)
            ' Open the Effect
            If IsOpen = False Then Open()
            Dim effptr As New SWIGTYPE_p_sox_effect_t(_SwigStorage.GetswigCPtr)

            ' Make a copy of the input / output signal info to pass to sox_add_effect
            ' this is because sox_add_effect as a way of tampering / altering the values we pass in
            ' If Singal is not specified then use
            UnAtt_SigIn = Signal_Input
            Dim att_insig As SignalInfo
            If Signal_Input Is Nothing Then _
                Signal_Input = (From item In chain.Effects Where item.IsAttached = True).Last.Signal_Output.Clone
            att_insig = Signal_Input.Clone

            UnAtt_SigOut = Signal_Output
            Dim att_outsig As SignalInfo
            If Signal_Output Is Nothing Then _
                Signal_Output = (From item In chain.Effects Where item.IsAttached = True).Last.Signal_Output.Clone
            att_outsig = Signal_Output.Clone

            ' Add the Effect
            SyncLock Attach_SyncLock
                Try
                    Dim ret As Integer = libsox.sox_add_effect(chain.SwigStorage, effptr, att_insig.SwigStorage, att_outsig.SwigStorage)
                    If ret <> sox_error_t.SOX_SUCCESS Then Throw New SoxException("Error Adding the Effect to the Chain")
                    IsAttached = True
                Catch ex As Exception
                    Throw New SoxException("Error Adding the Effect to the Chain", ex)
                End Try
            End SyncLock

        End Sub

        ''' <summary> Setup Sub Class Properties. </summary>
        Protected Sub SetupProperties()
            If GlobalInfo Is Nothing And _SwigStorage.global_info IsNot Nothing Then _
                GlobalInfo = New EffectGlobals(_SwigStorage.global_info)
            If Signal_Input Is Nothing And _SwigStorage.in_signal IsNot Nothing Then _
                Signal_Input = New SignalInfo(_SwigStorage.in_signal)
            If Signal_Output Is Nothing And _SwigStorage.out_signal IsNot Nothing Then _
                Signal_Output = New SignalInfo(_SwigStorage.out_signal)
            If Encoding_Input Is Nothing And _SwigStorage.in_encoding IsNot Nothing Then _
                Encoding_Input = New EncodingInfo(_SwigStorage.in_encoding)
            If Encoding_Output Is Nothing And _SwigStorage.out_encoding IsNot Nothing Then _
                Encoding_Output = New EncodingInfo(_SwigStorage.out_encoding)
            If Handler Is Nothing And _SwigStorage.handler IsNot Nothing Then _
                Handler = New EffectHandler(_SwigStorage.handler)
        End Sub

        ''' <summary> Get a list of Effects / sub class's that inherit from EffectBase. </summary>
        ''' <returns> The list of Effects. </returns>
        Public Shared Function GetSubClassList() As List(Of Type)
            Dim typs As New List(Of Type)
            For Each typ As Type In Assembly.GetAssembly(GetType(EffectBase)).GetTypes _
                .Where(Function(myType) myType.IsClass AndAlso (Not myType.IsAbstract) AndAlso myType.IsSubclassOf(GetType(EffectBase)))
                typs.Add(typ)
            Next
            Return typs
        End Function

#End Region

    End Class

End Namespace
