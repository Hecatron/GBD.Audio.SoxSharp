Imports System.Collections.ObjectModel

Namespace Effect.Volume

    ''' <summary>
    '''  Compand (compress or expand) the dynamic range of the audio. (Automatic Gain Control)
    '''  up or down the volume of the audio automatically see 
    '''  http://old.nabble.com/Automatic-Gain-Control-td26974467.html
    '''  The speex filter may be better than this one.
    ''' </summary>
    Public Class EffectCompand
        Inherits EffectBase

#Region "Properties"

        ''' <summary> The Effect Name. </summary>
        ''' <value> The Effect Name. </value>
        Public Overrides ReadOnly Property Name As String
            Get
                Return "Compand Effect"
            End Get
        End Property

        ''' <summary> Name of the Underlying Sox Effect to open. </summary>
        ''' <value> The name of the sox effect. </value>
        <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        Protected Overrides Property _SoxEffectName As String = "compand"

        ''' <summary> List of Attack / Delay Values. </summary>
        ''' <value> List of Attack / Delay Values. </value>
        Public Property AttackDelayList As New ObservableCollection(Of KeyValuePair(Of Decimal?, Decimal?))

        ''' <summary> Soft-Knee (in dB) </summary>
        ''' <value> Soft-Knee (in dB) </value>
        Public Property Soft_Knee As Integer? = Nothing

        ''' <summary> List of Attack / Delay Values. </summary>
        ''' <value> List of Attack / Delay Values. </value>
        Public Property InDBOutDBList As New ObservableCollection(Of KeyValuePair(Of Decimal?, Decimal?))

        ''' <summary> Gain (in dB) </summary>
        ''' <value> Gain (in dB) </value>
        Public Property Gain As Integer? = Nothing

        ''' <summary> Initial_Volume (in dB) </summary>
        ''' <value> Initial_Volume (in dB) </value>
        Public Property Initial_Volume As Integer? = Nothing

        ''' <summary> Delay (in seconds) </summary>
        ''' <value> Delay (in seconds) </value>
        Public Property Delay As Double? = Nothing

        ''' <summary> Description. </summary>
        ''' <value> Description. </value>
        Public Property Description As String = _
        "This Effect can be used compress or expand) the dynamic range of the audio. " & vbCrLf & _
        "I think this is the closest match to Automatic Gain Control "

#End Region

#Region "Constructors"

        ''' <summary> Default Constructor. </summary>
        Public Sub New()
        End Sub

#End Region

#Region "Methods"

        ''' <summary> Setup the SoxEffecOptions class. </summary>
        Public Overrides Sub SetupOptionList()
            Dim tmp_str As String
            Options.Clear()

            ' Add the Attack / Delay for each of the Channels
            If AttackDelayList Is Nothing Then Exit Sub
            If AttackDelayList.Count <= 0 Then Exit Sub
            tmp_str = ""
            For Each item As KeyValuePair(Of Decimal?, Decimal?) In AttackDelayList
                If item.Key IsNot Nothing Then tmp_str &= Math.Round(CType(item.Key, Decimal), 1) & ","
                If item.Value IsNot Nothing Then tmp_str &= Math.Round(CType(item.Value, Decimal), 1) & ","
            Next
            tmp_str = tmp_str.TrimEnd(CChar(","))
            Options.Add(tmp_str)

            ' Add the In_db / Out_db for each of the channels
            ' out_dB defaults to in_dB if not specified
            If InDBOutDBList Is Nothing Then Exit Sub
            If InDBOutDBList.Count <= 0 Then Exit Sub
            tmp_str = ""
            If Soft_Knee IsNot Nothing Then tmp_str &= Soft_Knee.ToString & ":"
            For Each item As KeyValuePair(Of Decimal?, Decimal?) In InDBOutDBList
                If item.Key IsNot Nothing Then tmp_str &= Math.Round(CType(item.Key, Decimal), 1) & ","
                If item.Value IsNot Nothing Then tmp_str &= Math.Round(CType(item.Value, Decimal), 1) & ","
            Next
            tmp_str = tmp_str.TrimEnd(CChar(","))
            Options.Add(tmp_str)

            If Gain IsNot Nothing Then
                ' Add in Gain if specified
                Options.Add(Gain.ToString)
                If Initial_Volume IsNot Nothing Then
                    ' Add in Initial Volume if Gain / Initial Volume is specified
                    Options.Add(Initial_Volume.ToString)
                    If Delay IsNot Nothing Then
                        ' Add in Delay if Gain / Initial Volume / Delay is specified
                        Options.Add(Math.Round(CDbl(Delay), 1).ToString)
                    End If
                End If
            End If
        End Sub

        ''' <summary> Set Default Values. </summary>
        Public Overrides Sub SetDefaults()
            If AttackDelayList IsNot Nothing Then AttackDelayList.Clear()
            Soft_Knee = Nothing
            If InDBOutDBList IsNot Nothing Then AttackDelayList.Clear()
            Gain = Nothing
            Initial_Volume = Nothing
            Delay = Nothing
        End Sub

#End Region

    End Class

End Namespace
