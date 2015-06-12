Imports GBD.Audio.SoxSharp.swig

Namespace DataTypes

    ''' <summary> Instrument Information. </summary>
    Public Class InstrumentInfo
        Implements IDisposable

#Region "Properties - Local Storage"

        ''' <summary> Underlying Swig Storage. </summary>
        ''' <value> Underlying Swig Storage. </value>
        Public ReadOnly Property SwigStorage As sox_instrinfo_t
            Get
                Return _SwigStorage
            End Get
        End Property
        <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        Protected Property _SwigStorage As sox_instrinfo_t

#End Region

#Region "Properties - Swig Wrapper"

        ''' <summary> For unity pitch playback. </summary>
        ''' <value> The MIDI note. </value>
        Public Property MIDI_Note As SByte
            Get
                Return _SwigStorage.MIDInote
            End Get
            Set(value As SByte)
                _SwigStorage.MIDInote = value
            End Set
        End Property

        ''' <summary> MIDI pitch-bend low range. </summary>
        ''' <value> The MIDI low. </value>
        Public Property MIDI_Low As SByte
            Get
                Return _SwigStorage.MIDIlow
            End Get
            Set(value As SByte)
                _SwigStorage.MIDIlow = value
            End Set
        End Property

        ''' <summary> MIDI pitch-bend high range. </summary>
        ''' <value> The MIDI high. </value>
        Public Property MIDI_Hi As SByte
            Get
                Return _SwigStorage.MIDIhi
            End Get
            Set(value As SByte)
                _SwigStorage.MIDIhi = value
            End Set
        End Property

        ''' <summary> Loop Mode. </summary>
        ''' <value> The loop mode. </value>
        Public Property LoopMode As LoopingInfo.LoopMode_Type
            Get
                Dim ret As LoopingInfo.LoopMode_Type
                [Enum].TryParse(_SwigStorage.loopmode.ToString, ret)
                Return ret
            End Get
            Set(value As LoopingInfo.LoopMode_Type)
                _SwigStorage.loopmode = value
            End Set
        End Property

        ''' <summary> Number of active loops. </summary>
        ''' <value> The number of loops. </value>
        Public Property LoopCount As UInteger
            Get
                Return _SwigStorage.nloops
            End Get
            Set(value As UInteger)
                _SwigStorage.nloops = value
            End Set
        End Property

#End Region

#Region "Constructors"

        ''' <summary> Default Constructor. </summary>
        Public Sub New()
            _SwigStorage = New sox_instrinfo_t
        End Sub

        ''' <summary> Default Constructor. </summary>
        ''' <param name="storage"> Use this as the swig storage value. </param>
        Public Sub New(storage As sox_instrinfo_t)
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

    End Class

End Namespace
