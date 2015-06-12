Namespace Effect.Mixing

    ''' <summary>
    '''  More Complicated version of the Mixer effect, for merging / splitting audio into different
    '''  channels Basic implementation at this stage for mixing 2 channels into 1.
    ''' </summary>
    Public Class EffectMixer
        Inherits EffectBase

#Region "Properties"

        ''' <summary> The Effect Name. </summary>
        ''' <value> The Effect Name. </value>
        Public Overrides ReadOnly Property Name As String
            Get
                Return "Mixer Effect"
            End Get
        End Property

        ''' <summary> Name of the Underlying Sox Effect to open. </summary>
        ''' <value> The name of the sox effect. </value>
        <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        Protected Overrides Property _SoxEffectName As String = "mixer"

        ''' <summary> List of channels to mix. </summary>
        ''' <value> A List of channels. </value>
        Public Property ChannelList As New List(Of ChannelMix)

        ''' <summary> represents a single channel of audio for conversion. </summary>
        Public Class ChannelMix
            Protected Property m_ChannelPercent As Integer

            ''' <summary> 0 - 100 range value, represents the percentage used for the given channel. </summary>
            ''' <value> The channel percent. </value>
            Public Property ChannelPercent() As Integer
                Get
                    Return m_ChannelPercent
                End Get
                Set(value As Integer)
                    m_ChannelPercent = value
                    If m_ChannelPercent > 100 Then m_ChannelPercent = 100
                    If m_ChannelPercent < 0 Then m_ChannelPercent = 0
                End Set
            End Property

            ''' <summary> give String Value. </summary>
            ''' <returns> A String that represents this object. </returns>
            Public Overrides Function ToString() As String
                Dim tmp_str As String
                Dim tmp_double As Double
                tmp_double = m_ChannelPercent / 100
                tmp_str = tmp_double & "," & Unknown2 & "," & Unknown3 & "," & Unknown4
                Return tmp_str
            End Function

            ''' <summary>
            ''' TODO
            ''' </summary>
            Public Property Unknown2 As Integer = 0
            Public Property Unknown3 As Integer = 0
            Public Property Unknown4 As Integer = 0
        End Class

        ''' <summary> Description </summary>
        Public Description As String = _
        "Mixer, this effect allows any number of channels to be re-mixed to any number of other channels " & vbCrLf & _
        "The idea being you can change a couple of mono channels into a single stereo channel " & _
        "as one example"

#End Region

#Region "Constructors"

        ''' <summary> Default Constructor. </summary>
        Public Sub New()
        End Sub

#End Region

#Region "Methods"

        ''' <summary> Setup the SoxEffecOptions class. </summary>
        Public Overrides Sub SetupOptionList()
            Options.Clear()
            Dim tmp_str As String = ""
            For Each item As ChannelMix In ChannelList
                tmp_str &= item.ToString & " "
            Next
            Options.Add(tmp_str)
        End Sub

        ''' <summary> Set Default Values. </summary>
        Public Overrides Sub SetDefaults()
            ChannelList = New List(Of ChannelMix)
        End Sub

#End Region

    End Class

End Namespace
