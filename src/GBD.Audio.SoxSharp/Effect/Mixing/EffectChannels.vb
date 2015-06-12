Namespace Effect.Mixing

    ''' <summary>
    '''  Remix the Audio into x number of channels in a simple way Good for Stereo to Mono Coversion
    '''  etc
    '''  
    '''  channels is just a shortcut to the remix effect, but with fewer options / simpler
    '''  implementation remix / channels has the advantage over mixer in that it can handle any number
    '''  of input audio channels
    '''  
    '''  you give it a single number and the audio is remixed to that number of channels useful for
    '''  stereo to mono conversions.
    ''' </summary>
    Public Class EffectChannels
        Inherits EffectBase

#Region "Properties"

        ''' <summary> The Effect Name. </summary>
        ''' <value> The Effect Name. </value>
        Public Overrides ReadOnly Property Name As String
            Get
                Return "Channels Effect"
            End Get
        End Property

        ''' <summary> Name of the Underlying Sox Effect to open. </summary>
        ''' <value> The name of the sox effect. </value>
        <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        Protected Overrides Property _SoxEffectName As String = "channels"

        ''' <summary> Number of Channels to DownMix / UpMix to. </summary>
        ''' <value> Number of Channels to DownMix / UpMix to. </value>
        Public Property OutChannels As Integer = 1

        ''' <summary> Description. </summary>
        ''' <value> The description. </value>
        Public Property Description As String = _
        "This Effect can be used to remix any number of channels to any other number of channels" & vbCrLf & _
        "e.g. for example a parameter of 1 will automatically remix any stereo or audio with more " & _
        "than one channel down to mono" & vbCrLf _
        & vbCrLf & _
        "Invoke a simple algorithm to change the number of channels in the audio signal to the given " & _
        " number CHANNELS: mixing if decreasing the number of channels or duplicating if increasing the number of channels."

#End Region

#Region "Constructors"

        ''' <summary> Default Constructor. </summary>
        Public Sub New()
        End Sub

        ''' <summary> Default Constructor. </summary>
        ''' <param name="output_channels"> The output channels. </param>
        Public Sub New(output_channels As Integer)
            ' Setup the options
            OutChannels = output_channels
        End Sub

#End Region

#Region "Methods"

        ''' <summary> Setup the SoxEffecOptions class. </summary>
        Public Overrides Sub SetupOptionList()
            Options.Clear()
            Options.Add(OutChannels.ToString)
        End Sub

        ''' <summary> Set Default Values. </summary>
        Public Overrides Sub SetDefaults()
            OutChannels = 1
        End Sub

#End Region

    End Class

End Namespace
