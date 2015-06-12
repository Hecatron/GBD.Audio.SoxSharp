Namespace Effect.Mastering

    ''' <summary>
    '''  Rate Effect for changing / upsampling / downsampling the audio bits per second.
    ''' </summary>
    Public Class EffectRate
        Inherits EffectBase

#Region "Properties - BitRate"

        ''' <summary> The Effect Name. </summary>
        ''' <value> The Effect Name. </value>
        Public Overrides ReadOnly Property Name As String
            Get
                Return "Rate Effect"
            End Get
        End Property

        ''' <summary> Name of the Underlying Sox Effect to open. </summary>
        ''' <value> The name of the sox effect. </value>
        <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        Protected Overrides Property _SoxEffectName As String = "rate"

        ''' <summary> Effect Description. </summary>
        ''' <value> Effect Description. </value>
        Public Property Description As String = _
        "This effect changes the bitrate of audio, typically all VC / VOX's have to be 8Khz" & vbCrLf & _
        "Also it's a good idea when generating an MP3 to at least set the bitrate to 22Khz " & _
        "otherwise the compression of a 8Khz file will sound pretty bad with an MP3 file "

        ''' <summary>
        '''  Bit Rate the audio needs to be converted to
        '''  i.e. typically VC files are 8Khz, MP3's are 22Khz / 44Khz.
        ''' </summary>
        ''' <value> The bit rate. </value>
        Public Property BitRate As Integer = StdBitRate.Khz22

        ''' <summary> List of Standard bit rates. </summary>
        Public Enum StdBitRate
            Khz04 = 4000
            Khz06 = 6000
            Khz08 = 8000
            Khz11 = 11025
            Khz22 = 22050
            Khz24 = 24000
            Khz32 = 32000
            Khz44 = 44100
            Khz48 = 48000
            Khz88 = 88200
            Khz96 = 96000
            Khz192 = 192000
        End Enum

#End Region

#Region "Properties - Quality"

        ''' <summary> Quality of the Conversion Default = High. </summary>
        ''' <value> Quality of the Conversion. </value>
        Public Property Quality As Quality_Type = Quality_Type.SysDefault

        ''' <summary> List of Available Quality's. </summary>
        Public Enum Quality_Type
            SysDefault = 0
            Quick = 1
            Low = 2
            Medium = 3
            High = 4
            VeryHigh = 5
        End Enum

        ''' <summary> Reports the bandwidth of the current Quality Setting. </summary>
        ''' <value> Bandwidth of the current Quality Setting. </value>
        Public ReadOnly Property Quality_BandWidth() As String
            Get
                Dim tmp_int As Integer = Determine_Quality_Bandwidth(Quality)
                If tmp_int = -1 Then Return "n/a"
                Return tmp_int & "%"
            End Get
        End Property

        ''' <summary> Reports the Rejection DB of the current Quality Setting. </summary>
        ''' <value> Rejection DB of the current Quality Setting. </value>
        Public ReadOnly Property Quality_RejectDB() As Integer
            Get
                Return Determine_Quality_RejectDB(Quality)
            End Get
        End Property

        ''' <summary> Reports the Typical use of the current Quality Setting. </summary>
        ''' <value> Quality Description. </value>
        Public ReadOnly Property Quality_Description() As String
            Get
                Return Determine_Quality_Description(Quality)
            End Get
        End Property

#End Region

#Region "Properties - Override Options"

        ' These can only be set when the Quality is medium or greater

        ''' <summary> Phase Response Default = Linear. </summary>
        ''' <value> The phase response mode. </value>
        Public Property PhaseResponseMode As PhaseResponseMode_Type = PhaseResponseMode_Type.SysDefault

        ''' <summary> List of Available Phase Responses Linear is the default. </summary>
        Public Enum PhaseResponseMode_Type
            SysDefault = 0
            Minimum = 1
            Intermediate = 2
            Linear = 3
        End Enum

        ''' <summary> Steep Filter When Enabled band-width = 99%. </summary>
        ''' <value> Enable steep filter. </value>
        Public Property Phase_SteepFilter As Boolean = False

        ''' <summary> Allow aliasing above the pass-band. </summary>
        ''' <value> Enable allow aliasing. </value>
        Public Property Phase_AllowAliasing As Boolean = False

        ''' <summary> Any band-width %. </summary>
        ''' <value> Any band-width %. </value>
        Public Property Phase_BandWidth As Double = -1

        ''' <summary>
        '''  Any phase response (0 = minimum, 25 = intermediate, 50 = linear, 100 = maximum)
        ''' </summary>
        ''' <value> The phase response. </value>
        Public Property PhaseResponse As Integer = -1

#End Region

#Region "Constructors"

        ''' <summary> Default Constructor. </summary>
        Public Sub New()
        End Sub

        ''' <summary> Default Constructor. </summary>
        ''' <param name="bitrate_param"> The bitrate. </param>
        Public Sub New(bitrate_param As Integer)
            BitRate = bitrate_param
        End Sub

#End Region

#Region "Methods"

        ''' <summary> Setup the SoxEffecOptions class. </summary>
        Public Overrides Sub SetupOptionList()
            Options.Clear()

            ' Add Quality
            Select Case Quality
                Case Quality_Type.Quick
                    Options.Add("-q")
                Case Quality_Type.Low
                    Options.Add("-l")
                Case Quality_Type.Medium
                    Options.Add("-m")
                Case Quality_Type.High
                    Options.Add("-h")
                Case Quality_Type.VeryHigh
                    Options.Add("-v")
            End Select

            ' Set the Override properties
            Select Case PhaseResponseMode
                Case PhaseResponseMode_Type.Minimum
                    Options.Add("-M")
                Case PhaseResponseMode_Type.Intermediate
                    Options.Add("-I")
                Case PhaseResponseMode_Type.Linear
                    Options.Add("-L")
            End Select
            If Phase_SteepFilter = True Then Options.Add("-s")
            If Phase_AllowAliasing = True Then Options.Add("-a")
            If (Phase_BandWidth <> -1) And (Phase_BandWidth >= 74) And (Phase_BandWidth <= 99.7) Then
                Dim tmp_str As String = "-b " & Phase_BandWidth.ToString
                Options.Add(tmp_str)
            End If
            If (PhaseResponse >= 0) And (PhaseResponse <= 100) Then
                Dim tmp_str As String = "-p " & PhaseResponse.ToString
                Options.Add(tmp_str)
            End If

            ' Add Bit Rate
            Options.Add(BitRate.ToString)
        End Sub

        ''' <summary> Set Default Values. </summary>
        Public Overrides Sub SetDefaults()
            BitRate = StdBitRate.Khz22
            Quality = Quality_Type.SysDefault
            PhaseResponseMode = PhaseResponseMode_Type.SysDefault
            Phase_SteepFilter = False
            Phase_AllowAliasing = False
            Phase_BandWidth = -1
            PhaseResponse = -1
            SetupOptionList()
        End Sub

        ''' <summary>
        '''  Returns a percentage representing the bandwidth for the given quality setting.
        ''' </summary>
        ''' <param name="qual"> The quality. </param>
        ''' <returns> An Integer percentage. </returns>
        Public Function Determine_Quality_Bandwidth(qual As Quality_Type) As Integer
            Select Case qual
                Case Quality_Type.Quick
                    Return -1
                Case Quality_Type.Low
                    Return 80
                Case Quality_Type.Medium
                    Return 95
                Case Quality_Type.High
                    Return 95
                Case Quality_Type.VeryHigh
                    Return 95
                Case Else
                    Return -1
            End Select
        End Function

        ''' <summary> Returns Rejection DB for the given Quality Setting. </summary>
        ''' <param name="qual"> The quality. </param>
        ''' <returns> An Integer representing the Rejection DB. </returns>
        Public Function Determine_Quality_RejectDB(qual As Quality_Type) As Integer
            Select Case qual
                Case Quality_Type.Quick
                    Return 30
                Case Quality_Type.Low
                    Return 100
                Case Quality_Type.Medium
                    Return 100
                Case Quality_Type.High
                    Return 125
                Case Quality_Type.VeryHigh
                    Return 175
                Case Else
                    Return -1
            End Select
        End Function

        ''' <summary> Returns a description of the typical use for the Quality Setting. </summary>
        ''' <param name="qual"> The quality. </param>
        ''' <returns> A String providing the description. </returns>
        Public Function Determine_Quality_Description(qual As Quality_Type) As String
            Select Case qual
                Case Quality_Type.Quick
                    Return "playback on ancient hardware"
                Case Quality_Type.Low
                    Return "playback on old hardware"
                Case Quality_Type.Medium
                    Return "audio playback"
                Case Quality_Type.High
                    Return "6-bit mastering (use with dither)"
                Case Quality_Type.VeryHigh
                    Return "24-bit mastering"
                Case Else
                    Return "n/a"
            End Select
        End Function

#End Region

    End Class

End Namespace