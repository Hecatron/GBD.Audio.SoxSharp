Namespace Effect.Mixing

    ''' <summary>
    '''  Remix / Newer version of the Mixer Effect for merging / splitting audio into different
    '''  channels.
    ''' </summary>
    Public Class EffectRemix
        Inherits EffectBase

#Region "Types"

        ''' <summary> Type of Scaling. </summary>
        Public Enum ScalingMode_Type
            None
            Automatic
            Manual
            Power
        End Enum

        ''' <summary> Type of Volume. </summary>
        Public Enum VolumeType
            None
            PowerAdjust
            PowerAdjustInvert
            VoltageMultiplier
        End Enum

        ''' <summary> Group of Input Channels. </summary>
        Public Class InChannelGroup
            Inherits List(Of InputChannelSpec)
        End Class

        ''' <summary> A Single Input Channel. </summary>
        Public Class InputChannelSpec

            ''' <summary> Start Number of the Input Channel. </summary>
            ''' <value> The channel start. </value>
            Public Property Channel_Start As Integer? = Nothing

            ''' <summary> For a Range of Channels set this to a value. </summary>
            ''' <value> The channel end. </value>
            Public Property Channel_End As Integer? = Nothing

            ''' <summary> If the Input Specification is a range of values. </summary>
            ''' <value> The is range. </value>
            Public Property IsRange As Boolean = False

            ''' <summary> Volume Specifier - type. </summary>
            ''' <value> The type of the volume. </value>
            Public Property VolType As VolumeType = VolumeType.None

            ''' <summary> Volume Specifier - value. </summary>
            ''' <value> The volume. </value>
            Public Property Volume As Decimal? = Nothing

            ''' <summary> Return String based on set options. </summary>
            ''' <returns> A String that represents this object. </returns>
            Public Overrides Function ToString() As String
                Dim tmpstr As String = ""
                If Channel_Start IsNot Nothing Then tmpstr &= Channel_Start
                If IsRange = True Then tmpstr &= "-"
                If Channel_End IsNot Nothing Then tmpstr &= Channel_End
                If Volume IsNot Nothing Then
                    Select Case VolType
                        Case VolumeType.PowerAdjust
                            tmpstr &= "p" & Volume
                        Case VolumeType.PowerAdjustInvert
                            tmpstr &= "i" & Volume
                        Case VolumeType.VoltageMultiplier
                            tmpstr &= "v" & Volume
                    End Select
                End If
                Return tmpstr
            End Function

        End Class


#End Region

#Region "Properties"

        ''' <summary> The Effect Name. </summary>
        ''' <value> The Effect Name. </value>
        Public Overrides ReadOnly Property Name As String
            Get
                Return "Remix Effect"
            End Get
        End Property

        ''' <summary> Name of the Underlying Sox Effect to open. </summary>
        ''' <value> The name of the sox effect. </value>
        <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        Protected Overrides Property _SoxEffectName As String = "remix"

        ''' <summary> Output Channel Specifications. </summary>
        ''' <value> The out channels. </value>
        Public Property OutChannels As New List(Of InChannelGroup)

        ''' <summary> If to use Power Scaling. </summary>
        ''' <value> The scalling. </value>
        Public Property Scalling As ScalingMode_Type = ScalingMode_Type.None

        ''' <summary>
        ''' Description
        ''' </summary>
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

        ''' <summary> Set Options for Mono Output. </summary>
        Public Sub SetMono()
            Dim ingroup As New InChannelGroup
            Dim monospec As New InputChannelSpec
            monospec.Channel_Start = Nothing
            monospec.Channel_End = Nothing
            monospec.IsRange = True
            monospec.VolType = VolumeType.None
            OutChannels.Clear()
            OutChannels.Add(ingroup)
            ingroup.Add(monospec)
            Scalling = ScalingMode_Type.None
        End Sub

        ''' <summary> Setup the SoxEffecOptions class. </summary>
        Public Overrides Sub SetupOptionList()
            Options.Clear()
            Select Case Scalling
                Case ScalingMode_Type.Automatic
                    Options.Add("-a")
                Case ScalingMode_Type.Manual
                    Options.Add("-m")
                Case ScalingMode_Type.Power
                    Options.Add("-p")
            End Select
            For Each inchangrp As InChannelGroup In OutChannels
                Dim tmpstr As String = ""
                For Each inchanspec As InputChannelSpec In inchangrp
                    tmpstr &= inchanspec.ToString
                    tmpstr &= ","
                Next
                tmpstr = tmpstr.TrimEnd(CChar(","))
                Options.Add(tmpstr)
            Next
        End Sub

        ''' <summary> Set Default Values. </summary>
        Public Overrides Sub SetDefaults()
            SetMono()
        End Sub

#End Region

    End Class

End Namespace
