Namespace IO

    ''' <summary> Options for LameMP3 within Sox. </summary>
    Public Class Mp3LameOpts

#Region "Types"

        ''' <summary>
        '''  Quality for Encoding the MP3's 9 = Highest / Slowest 0 = Lowest Quality / Fastest.
        ''' </summary>
        Public Enum Mp3Quality_Type
            Quality0 = 0
            Quality1 = 1
            Quality2 = 2
            Quality3 = 3
            Quality4 = 4
            Quality5 = 5
            Quality6 = 6
            Quality7 = 7
            Quality8 = 8
            Quality9 = 9
        End Enum

        ''' <summary> Type of BitRate. </summary>
        Public Enum BitRate_Type
            Constant = 0
            Variable = 1
        End Enum

#End Region

#Region "Properties"

        ''' <summary> Type of BitRate Constant / Variable. </summary>
        ''' <value> The type of the bit rate. </value>
        Public Property BitRateType As BitRate_Type = BitRate_Type.Constant

        ''' <summary> Constant BitRate to use for the encoding in Kbps. </summary>
        ''' <value> The constant bit rate. </value>
        Public Property ConstantBitRate As Integer = 32

        ''' <summary> Variable BitRate to use for the encoding - 0 to 9. </summary>
        ''' <value> The variable bit rate. </value>
        Public Property VariableBitRate As Mp3Quality_Type = Mp3Quality_Type.Quality4

        ''' <summary> Quality Setting to use for the encoding - 0 to 9. </summary>
        ''' <value> The quality. </value>
        Public Property Quality As Mp3Quality_Type = Mp3Quality_Type.Quality5

#End Region

#Region "Constructors"

        ''' <summary> Default Constructor. </summary>
        Public Sub New()
        End Sub

#End Region

#Region "Methods"

        ' See http://sox.sourceforge.net/soxformat.html

        ''' <summary> Generate Value Output for Encoding.Compression. </summary>
        ''' <returns> The output. </returns>
        Public Function GenerateOutput() As Double
            Dim tmpstr As String = ""
            If BitRateType = BitRate_Type.Constant Then
                If Quality = Mp3Quality_Type.Quality0 Then
                    tmpstr = ConstantBitRate & "." & "01"
                Else
                    tmpstr = ConstantBitRate & "." & Quality
                End If
            End If
            If BitRateType = BitRate_Type.Variable Then
                Dim VariableBitRate_int As Integer = VariableBitRate
                Dim Quality_int As Integer = Quality
                tmpstr = "-" & VariableBitRate_int & "." & Quality_int
            End If
            Dim ret As Double
            Double.TryParse(tmpstr, ret)
            Return ret
        End Function

#End Region

    End Class

End Namespace
