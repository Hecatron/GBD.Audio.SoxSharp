Namespace Effect.Tone

    ''' <summary>
    '''  An AllPass filter two-pole all-pass filter with central frequency (in Hz) frequency.
    ''' </summary>
    Public Class EffectAllPass
        Inherits EffectBase

#Region "Properties"

        ''' <summary> The Effect Name. </summary>
        ''' <value> The Effect Name. </value>
        Public Overrides ReadOnly Property Name As String
            Get
                Return "AllPass Effect"
            End Get
        End Property

        ''' <summary> Name of the Underlying Sox Effect to open. </summary>
        ''' <value> The name of the sox effect. </value>
        <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        Protected Overrides Property _SoxEffectName As String = "allpass"

        ''' <summary> Frequency setting for the filter in Hz. </summary>
        ''' <value> Frequency Hz. </value>
        Public Property Frequency As Integer = 1

        ''' <summary> Width setting for double pole mode. </summary>
        ''' <value> The width. </value>
        Public Property Width As Width_Type = Width_Type.SysDefault

        ''' <summary> Represents the width. </summary>
        Public Enum Width_Type As Integer
            SysDefault = 0
            Q = 1
            O = 2
            H = 3
            K = 4
        End Enum

        ''' <summary> Description. </summary>
        ''' <value> The description. </value>
        Public Property Description As String = _
        "Apply a two-pole all-pass filter with central frequency (in Hz) frequency, and filter-width width." & _
        vbCrLf & _
        "audio's frequency to phase relationship without changing its frequency to amplitude relationship."

#End Region

#Region "Constructors"

        ''' <summary> Default Constructor. </summary>
        Public Sub New()
        End Sub

        ''' <summary> Default Constructor. </summary>
        ''' <param name="freq"> The frequency. </param>
        Public Sub New(freq As Integer)
            ' Setup the options
            Frequency = freq
        End Sub

#End Region

#Region "Methods"

        ''' <summary> Setup the SoxEffecOptions class. </summary>
        Public Overrides Sub SetupOptionList()
            Options.Clear()

            ' Add the Frequency
            Options.Add(Frequency.ToString)

            ' Add in the Width
            Select Case Width
                Case Width_Type.Q
                    Options.Add("q")
                Case Width_Type.O
                    Options.Add("o")
                Case Width_Type.H
                    Options.Add("h")
                Case Width_Type.K
                    Options.Add("k")
            End Select
        End Sub

        ''' <summary> Set Default Values. </summary>
        Public Overrides Sub SetDefaults()
            Frequency = 1
            Width = Width_Type.SysDefault
        End Sub

#End Region

    End Class

End Namespace
