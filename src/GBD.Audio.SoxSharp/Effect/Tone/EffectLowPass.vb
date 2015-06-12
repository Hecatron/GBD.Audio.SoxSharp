Namespace Effect.Tone

    ''' <summary>
    '''  A lowPass filter which allows all frequencies through below a certain limit This is useful
    '''  for VBR MP3's to get the file size down.
    ''' </summary>
    Public Class EffectLowPass
        Inherits EffectBase

#Region "Properties"

        ''' <summary> The Effect Name. </summary>
        ''' <value> The Effect Name. </value>
        Public Overrides ReadOnly Property Name As String
            Get
                Return "LowPass Effect"
            End Get
        End Property

        ''' <summary> Name of the Underlying Sox Effect to open. </summary>
        ''' <value> The name of the sox effect. </value>
        <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        Protected Overrides Property _SoxEffectName As String = "lowpass"

        ''' <summary> Pole Mode for the filter, default is 0. </summary>
        ''' <value> The pole mode. </value>
        Public Property PoleMode As PoleMode_Type = PoleMode_Type.SysDefault

        ''' <summary> Represents a single / double pole mode for the filter. </summary>
        Public Enum PoleMode_Type As Integer
            SysDefault = 0
            SinglePole = 1
            DoublePole = 2
        End Enum

        ''' <summary> Frequency setting for the filter in Hz. </summary>
        ''' <value> The frequency. </value>
        Public Property Frequency As Integer = 9500

        ''' <summary> Width setting for double pole mode. </summary>
        ''' <value> The width. </value>
        Public Property Width As EffectAllPass.Width_Type = EffectAllPass.Width_Type.SysDefault

        ''' <summary> Description. </summary>
        ''' <value> The description. </value>
        Public Property Description As String = _
        "Low Pass Filter, that can be used to allow all frequencies through except for those " & _
        "above the specified frequency. " & vbCrLf & _
        "Apply a low-pass filter with 3dB point frequency. The filter can be either single-pole (with -1) " & _
        ", or double-pole (the default, or with -2). width applies only to double-pole filters; the default " & _
        " is Q = 0.707 and gives a Butterworth response. The filters roll off at 6dB per pole per octave (20dB " & _
        "per pole per decade)."

#End Region

#Region "Constructors"

        ''' <summary> Default Constructor. </summary>
        Public Sub New()
        End Sub

        ''' <summary> Default Constructor. </summary>
        ''' <param name="freq"> The frequency. </param>
        Public Sub New(freq As Integer)
            ' Setup the options. 
            Frequency = freq
        End Sub

#End Region

#Region "Methods"

        ''' <summary> Setup the SoxEffectOptions class. </summary>
        Public Overrides Sub SetupOptionList()
            Options.Clear()

            ' Add the pole mode if specified
            If PoleMode = PoleMode_Type.SinglePole Then
                Options.Add("-1")
            ElseIf PoleMode = PoleMode_Type.DoublePole Then
                Options.Add("-2")
            End If

            ' Add the Frequency
            Options.Add(Frequency.ToString)

            ' Add the width if specified
            ' this only applies to double pole
            If PoleMode = PoleMode_Type.DoublePole Then
                If Width = EffectAllPass.Width_Type.Q Then
                    Options.Add("q")
                ElseIf Width = EffectAllPass.Width_Type.O Then
                    Options.Add("o")
                ElseIf Width = EffectAllPass.Width_Type.H Then
                    Options.Add("h")
                ElseIf Width = EffectAllPass.Width_Type.K Then
                    Options.Add("k")
                End If
            End If

        End Sub

        ''' <summary> Set Default Values. </summary>
        Public Overrides Sub SetDefaults()
            PoleMode = PoleMode_Type.SysDefault
            Frequency = 9500
            Width = EffectAllPass.Width_Type.SysDefault
        End Sub

#End Region

    End Class

End Namespace
