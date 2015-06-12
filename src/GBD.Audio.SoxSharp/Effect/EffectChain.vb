Imports System.Collections.ObjectModel
Imports System.Runtime.InteropServices
Imports System.Xml.Serialization
Imports GBD.Audio.SoxSharp.DataTypes
Imports GBD.Audio.SoxSharp.swig
Imports GBD.Audio.SoxSharp.UnmanagedHelper

Namespace Effect

    ''' <summary> Effect Chain. </summary>
    Public Class EffectChain
        Implements IDisposable

#Region "Properties - Local Storage"

        ''' <summary> Underlying Swig Storage. </summary>
        ''' <value> Underlying Swig Storage. </value>
        Public ReadOnly Property SwigStorage As sox_effects_chain_t
            Get
                Return _SwigStorage
            End Get
        End Property
        <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        Protected Property _SwigStorage As sox_effects_chain_t

#End Region

#Region "Properties"

        ''' <summary> List of Effects to apply to the Chain. </summary>
        ''' <value> List of Effects to apply to the Chain. </value>
        Public Property Effects As New List(Of EffectBase)

        ''' <summary> Input Encoding for the chain. </summary>
        ''' <value> The input encoding. </value>
        Public Property Input_Encoding As EncodingInfo

        ''' <summary> Output Encoding for the chain. </summary>
        ''' <value> The output encoding. </value>
        Public Property Output_Encoding As EncodingInfo

        ''' <summary> If the Chain has been opened with sox. </summary>
        ''' <value> If the chain is open. </value>
        Public ReadOnly Property IsOpen As Boolean
            Get
                Return _IsOpen
            End Get
        End Property
        Protected Property _IsOpen As Boolean

        ''' <summary> If the Chain is Running. </summary>
        ''' <value> If the chain is running. </value>
        Public ReadOnly Property IsRunning As Boolean
            Get
                Return _IsRunning
            End Get
        End Property
        Protected Property _IsRunning As Boolean = False

#End Region

#Region "Properties - Swig Wrapper"

        ''' <summary> List of Effects attached to the sox chain. </summary>
        ''' <value> List of effects that are attached. </value>
        <XmlIgnore()> _
        Public ReadOnly Property Sox_Effects As ReadOnlyCollection(Of EffectBase)
            Get
                If _SwigStorage.effects Is Nothing Then Return Nothing
                Dim efflist As New List(Of EffectBase)
                Dim effptr As IntPtr = _SwigStorage.effects.GetswigCPtr
                For i As UInteger = 1 To Sox_Length
                    Dim effptr2 As IntPtr = CHelper.Umg_PtrToStructure(Of IntPtr)(effptr)
                    Dim newitem As New EffectBase(New sox_effect_t(effptr2))
                    efflist.Add(newitem)
                    effptr = effptr + IntPtr.Size
                Next
                Return efflist.AsReadOnly
            End Get
        End Property

        ''' <summary> Number of Entries in Effects Table. </summary>
        ''' <value> The size of the sox table. </value>
        Public ReadOnly Property Sox_TableSize As UInteger
            Get
                Return _SwigStorage.table_size
            End Get
        End Property

        ''' <summary> Number of Effects to be applied. </summary>
        ''' <value> Number of Effects to be applied. </value>
        Public ReadOnly Property Sox_Length As UInteger
            Get
                Return _SwigStorage.length
            End Get
        End Property

        ''' <summary> Channel interleave buffer Input. </summary>
        ''' <value> Channel interleave buffer Input. </value>
        <XmlIgnore()> _
        Public ReadOnly Property Sox_IBufC As SWIGTYPE_p_p_int
            Get
                Return _SwigStorage.ibufc
            End Get
        End Property

        ''' <summary> Channel interleave buffer Output. </summary>
        ''' <value> Channel interleave buffer Output. </value>
        <XmlIgnore()> _
        Public ReadOnly Property Sox_OBufC As SWIGTYPE_p_p_int
            Get
                Return _SwigStorage.obufc
            End Get
        End Property

        ''' <summary> Copy of global effects settings. </summary>
        ''' <value> Copy of global effects settings. </value>
        Public ReadOnly Property Sox_GlobalInfo As EffectGlobals
            Get
                Return _Sox_GlobalInfo
            End Get
        End Property
        <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        Protected Property _Sox_GlobalInfo As EffectGlobals

        ''' <summary> Input Encoding. </summary>
        ''' <value> Input Encoding. </value>
        Public ReadOnly Property Sox_InputEncoding As EncodingInfo
            Get
                Return New EncodingInfo(_SwigStorage.in_enc)
            End Get
        End Property

        ''' <summary> Output Encoding. </summary>
        ''' <value> Output Encoding. </value>
        Public ReadOnly Property Sox_OutputEncoding As EncodingInfo
            Get
                Return New EncodingInfo(_SwigStorage.out_enc)
            End Get
        End Property

#End Region

#Region "Constructors"

        ''' <summary> Default Constructor. </summary>
        Public Sub New()
        End Sub

        ''' <summary> Default Constructor. </summary>
        ''' <param name="storage"> Use this as the swig storage value. </param>
        Public Sub New(storage As sox_effects_chain_t)
            _SwigStorage = storage
        End Sub

        ''' <summary> Default Constructor. </summary>
        ''' <param name="inenc">  Input Encoding. </param>
        ''' <param name="outenc"> Output Encoding. </param>
        Public Sub New(inenc As EncodingInfo, outenc As EncodingInfo)
            Input_Encoding = inenc
            Output_Encoding = outenc
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

#Region "Methods - Open / Close"

        ''' <summary> Open the Chain with Sox. </summary>
        ''' <exception cref="InvalidOperationException"> Thrown when the requested operation is invalid. </exception>
        ''' <exception cref="SoxException">              Thrown when a Sox error condition occurs. </exception>
        Public Sub Open()
            If _IsOpen = True Then Throw New InvalidOperationException("Effect Chain is already Open")
            ' Initialise the Chain with libsox
            Try
                _SwigStorage = libsox.sox_create_effects_chain(Input_Encoding.SwigStorage, Output_Encoding.SwigStorage)
            Catch ex As Exception
                Throw New SoxException("Error Creating the Effects Chain", ex)
            End Try
            If _SwigStorage Is Nothing Then Throw New SoxException("Error creating the sox chain")
            _Sox_GlobalInfo = New EffectGlobals(_SwigStorage.global_info)
            _IsOpen = True
        End Sub

        ''' <summary> Close the Chain with Sox. </summary>
        ''' <exception cref="SoxException"> Thrown when a Sox error condition occurs. </exception>
        Public Sub Close()
            If _IsOpen = False Then Exit Sub
            For Each item In Effects
                item.Close()
            Next
            If _SwigStorage IsNot Nothing Then
                Try
                    libsox.sox_delete_effects_chain(_SwigStorage)
                Catch ex As Exception
                    Throw New SoxException("Error Deleting the Effects Chain", ex)
                End Try
            End If
            If _SwigStorage IsNot Nothing Then _SwigStorage.Dispose()
            _Sox_GlobalInfo = Nothing
            _IsOpen = False
        End Sub

        ''' <summary> Attach All Effects to the underlying chain. </summary>
        ''' <exception cref="InvalidOperationException"> Thrown when the requested operation is invalid. </exception>
        Public Sub AttachAll()
            If _IsOpen = False Then Throw New InvalidOperationException("Effect Chain Not yet Open")
            For Each item As EffectBase In Effects
                item.Attach(Me)
            Next
        End Sub

        ''' <summary> Gets the number of clips that occurred while running an effects chain. </summary>
        ''' <exception cref="SoxException"> Thrown when a Sox error condition occurs. </exception>
        ''' <returns> The number of clips. </returns>
        Public Function GetClips() As Integer
            If _IsOpen = False Then Return -1
            Dim ret As Integer
            Try
                ret = CInt(libsox.sox_effects_clips(_SwigStorage))
            Catch ex As Exception
                Throw New SoxException("Error Reading the Clips Details", ex)
            End Try
            Return ret
        End Function

#End Region

#Region "Methods - Start / Stop"

        ''' <summary> If the Conversion is to be aborted. </summary>
        ''' <exception cref="SoxException"> Thrown when a Sox error condition occurs. </exception>
        ''' <value> The abort conversion flag. </value>
        Protected Property AbortConversionFlag As Boolean

        ''' <summary> Start the Conversion. </summary>
        ''' <exception cref="SoxException"> Thrown when a Sox error condition occurs. </exception>
        ''' <returns> Return value from Flow Effects. </returns>
        Public Function StartConversion() As Integer
            DoChain_Started()
            ' Setup CallBack
            Dim callbackptr As IntPtr = Marshal.GetFunctionPointerForDelegate(Chain_Processing_Delegate)
            Dim callbackptr2 As New SWIGTYPE_p_f_enum_sox_bool_p_void__int(callbackptr)
            AbortConversionFlag = False
            _ConversionCount = 0
            Dim ret As Integer
            Try
                ret = libsox.sox_flow_effects(_SwigStorage, callbackptr2, Nothing)
            Catch ex As Exception
                Throw New SoxException("Error Processing the Chain", ex)
            End Try
            DoChain_Stopped()
            Return ret
        End Function

        ''' <summary> Abort the current conversion. </summary>
        Public Sub AbortConversion()
            AbortConversionFlag = True
        End Sub

#End Region

#Region "Events - Chain Processing"

        ''' <summary> Conversion Count. </summary>
        ''' <value> The number of conversions. </value>
        Public ReadOnly Property ConversionCount As Integer
            Get
                Return _ConversionCount
            End Get
        End Property
        Protected Property _ConversionCount As Integer

        ''' <summary> CallBack Implementation for StartChain </summary>
        Protected Delegate Function Chain_Processing_Delegate_Type() As Integer

        ''' <summary> CallBack Implementation for StartChain. </summary>
        ''' <value> The chain processing delegate. </value>
        Protected Property Chain_Processing_Delegate As New Chain_Processing_Delegate_Type(AddressOf Chain_Processing_Handler)

        ''' <summary> Event for Chain Processing </summary>
        Public Event Chain_Processing()

        ''' <summary> Handler for Sox Effect Chain Processing. </summary>
        ''' <returns> An Integer. </returns>
        Protected Function Chain_Processing_Handler() As Integer
            ' TODO This isn't much good at this stage as I haven't found a way
            ' to calculate the percentage complete based on the below
            ' in theory this is raised on each processing of a buffer but I'm finding it difficult to
            ' work out how many buffers there are to process before hand
            ' so I just increment _ConversionCount
            ' see the sox.exe for a better example, this uses a custom effect to track the progress
            RaiseEvent Chain_Processing()

            _ConversionCount = _ConversionCount + 1
            ' Abort Conversion if required
            If AbortConversionFlag = True Then Return sox_error_t.SOX_EOF
            ' Continue Conversion
            Return sox_error_t.SOX_SUCCESS
        End Function

        ''' <summary> Chain Processing Started </summary>
        Public Event Chain_Started()

        ''' <summary> Chain Processing Started. </summary>
        Protected Sub DoChain_Started()
            _IsRunning = True
            RaiseEvent Chain_Started()
        End Sub

        ''' <summary> Chain Processing Stopped </summary>
        Public Event Chain_Stopped()

        ''' <summary> Chain Processing Started. </summary>
        Protected Sub DoChain_Stopped()
            _IsRunning = False
            RaiseEvent Chain_Stopped()
        End Sub

#End Region

    End Class

End Namespace
