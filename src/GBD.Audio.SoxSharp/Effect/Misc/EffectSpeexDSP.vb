Namespace Effect.Misc

    ''' <summary>
    '''  The same as a SoxEffect, but assumes that this is to be used for the effect "speexdsp" for
    '''  auto gain control / noise reduction etc
    '''  
    '''  this requires the new version of the dll, also the C source is a bit hacked in for this one
    '''  so testing is advised see
    '''  http://www.speex.org/docs/manual/speex-manual/node4.html#SECTION00430000000000000000.
    ''' </summary>
    Public Class EffectSpeexDSP
        Inherits EffectBase

#Region "Properties"

        ''' <summary> The Effect Name. </summary>
        ''' <value> The Effect Name. </value>
        Public Overrides ReadOnly Property Name As String
            Get
                Return "SpeexDSP Effect"
            End Get
        End Property

        ''' <summary> Name of the Underlying Sox Effect to open. </summary>
        ''' <value> The name of the sox effect. </value>
        <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        Protected Overrides Property _SoxEffectName As String = "speexdsp"

        ''' <summary>
        '''  Automatic Gain Control, value is a target volume level from 0 - 99 0 = off, default is 99.
        ''' </summary>
        ''' <value> The agc. </value>
        Public Property AGC As Integer = 99

        ''' <summary> Noise Reduction, value is the max attenuation 0 = off, default is 15. </summary>
        ''' <value> The de noise. </value>
        Public Property DeNoise As Integer = 15

        ''' <summary> Enable reverb reduction. </summary>
        ''' <value> The de reverb. </value>
        Public Property DeReverb As Boolean = False

        ''' <summary> Number of Frames per Second, default is 50 in the C code. </summary>
        ''' <value> The Frames per Second. </value>
        Public Property FPS As Integer = 0

        ''' <summary> Number of Samples per Frame, default is the same as the FPS Setting (-1) </summary>
        ''' <value> The samples per frame. </value>
        Public Property SPF As Integer = 0

        ''' <summary> Description. </summary>
        ''' <value> The description. </value>
        Public Property Description As String = _
        "This effect can apply aguto gain control and denoise / dereverb effects " & vbCrLf & _
        "using the speexdsp library which has been added into the libsox dll"

#End Region

#Region "Constructors"

        ''' <summary> Default Constructor. </summary>
        Public Sub New()
        End Sub

#End Region

#Region "Methods"

        ''' <summary> Setup the SoxEffectOptions class. </summary>
        Public Overrides Sub SetupOptionList()
            Options.Clear()

            ' Add in the AGC Settings
            If AGC >= 0 Then
                Options.Add("-agc")
                Options.Add(AGC.ToString)
            Else
                Options.Add("-agc")
                Options.Add("0")
            End If

            ' Add in the de-noise Settings
            If DeNoise >= 0 Then
                Options.Add("-denoise")
                Options.Add(DeNoise.ToString)
            Else
                Options.Add("-denoise")
                Options.Add("0")
            End If

            ' Add in the de-reverb Settings
            If DeReverb = True Then Options.Add("-dereverb")

            ' Add in the FPS settings
            If FPS > 0 Then
                Options.Add("-fps")
                Options.Add(FPS.ToString)
            End If

            ' Add in the SPF settings
            If SPF > 0 Then
                Options.Add("-spf")
                Options.Add(SPF.ToString)
            End If
        End Sub

        ''' <summary> Set Default Values. </summary>
        Public Overrides Sub SetDefaults()
            AGC = 100
            DeNoise = 15
            DeReverb = False
            FPS = 50
            SPF = -1
            SetupOptionList()
        End Sub

#End Region

    End Class

End Namespace