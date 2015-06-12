Imports System.IO
Imports System.Xml.Serialization
Imports GBD.Audio.SoxSharp.DataTypes
Imports GBD.Audio.SoxSharp.IO
Imports GBD.Audio.SoxSharp.swig

Namespace Effect.Misc

    ''' <summary> File Input Effect For streaming data from a file via libsox. </summary>
    Public Class EffectNativeFileIn
        Inherits EffectBase

#Region "Properties"

        ''' <summary> The Effect Name. </summary>
        ''' <value> The Effect Name. </value>
        Public Overrides ReadOnly Property Name As String
            Get
                Return "File Input Effect"
            End Get
        End Property

        ''' <summary> Name of the Underlying Sox Effect to open. </summary>
        ''' <value> The name of the sox effect. </value>
        <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        Protected Overrides Property _SoxEffectName As String = "input"

        ''' <summary> Path to the file to be opened for reading. </summary>
        ''' <value> The full pathname of the file. </value>
        Public Property FilePath As String

        ''' <summary> File Format for input. </summary>
        ''' <value> File Format for input. </value>
        Public Property Format As String

        ''' <summary> Input File Stream. </summary>
        ''' <value> Input File Stream. </value>
        <XmlIgnore()> _
        Public Property FormatFile As FormatFile = Nothing

        ''' <summary> Description. </summary>
        ''' <value> Description. </value>
        Public Property Description As String = _
        "Used for reading files in, this is usually the first effect on the chain for input "

        ''' <summary> Out Of Bounds Data (suvh as ID3 Comments) detected on the input. </summary>
        ''' <value> Information describing the oob. </value>
        Public Property OOB_Data As OutOfBound

#End Region

#Region "Constructors"

        ''' <summary> Default Constructor. </summary>
        Public Sub New()
        End Sub

        ''' <summary> Default Constructor. </summary>
        ''' <param name="filestream"> The filestream. </param>
        Public Sub New(filestream As FormatFile)
            FormatFile = filestream
        End Sub

        ''' <summary> Default Constructor. </summary>
        ''' <param name="fpath">    The file path. </param>
        ''' <param name="sigin">    The input signal. </param>
        ''' <param name="encin">    The input encoding. </param>
        ''' <param name="formatin"> The file format. </param>
        Public Sub New(fpath As String, sigin As SignalInfo, encin As EncodingInfo, formatin As String)
            FilePath = fpath
            Signal_Input = sigin
            Encoding_Input = encin
            Signal_Output = sigin
            Format = formatin
        End Sub

        ''' <summary> Destructor. </summary>
        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            Close()
        End Sub

        ''' <summary> Return ToString. </summary>
        ''' <returns> A String that represents this object. </returns>
        Public Overrides Function ToString() As String
            Return Name
        End Function

#End Region

#Region "Methods - Open / Close"

        ''' <summary> Open the Effect / File Handle. </summary>
        Public Overrides Sub Open()
            ' Create a New File Handle and open for writing
            FormatFile = New FormatFile(FilePath, FormatBase.OpenMode.Read, Signal_Input, Encoding_Input, Format)

            ' Since this is typically the first effect on the chain, we need to maintain the input signal
            Dim tmpsig As SignalInfo = Signal_Input
            MyBase.Open()
            Signal_Input = tmpsig
        End Sub

        ''' <summary> Close the Effect / File Handle. </summary>
        Public Overrides Sub Close()
            If FormatFile IsNot Nothing Then FormatFile.Close()
            MyBase.Close()
        End Sub

        ''' <summary> Setup the SoxEffecOptions class. </summary>
        Public Overrides Sub SetupOptionList()
            Options.Clear()
            Options.Add(FormatFile)
        End Sub

        ''' <summary> Set Default Values. </summary>
        Public Overrides Sub SetDefaults()
            FormatFile = Nothing
        End Sub

#End Region

#Region "Methods - Detection"

        ''' <summary> Detect All Parameters. </summary>
        Public Sub Detect_All()
            Detect_Headerless()
            Detect_Format()
            Detect_Encoding()
            Detect_Signal()
            Detect_Comments()
        End Sub

        ''' <summary>
        '''  Check for Headerless File Types and Setup the Encoding / Signal / Format accordingly.
        ''' </summary>
        ''' <exception cref="ArgumentException"> Thrown when one or more arguments have unsupported or
        '''                                      illegal values. </exception>
        Public Sub Detect_Headerless()
            If File.Exists(FilePath) = False Then Throw New ArgumentException("FilePath not found")
            Dim fileext As String = Path.GetExtension(FilePath)
            Select Case LCase(fileext)

                Case ".vc"
                    ' VC Files are actually VOX Files
                    If String.IsNullOrEmpty(Format) Then Format = "vox"

                Case ".lin"
                    ' If the format is linear PCM output with no header, detect this here
                    ' Note for Linear, The audacity settings to import are
                    ' Signed 16bit PCM, Little-endian, 1 Channel Mono, 8000 Hz Sample Rate
                    If String.IsNullOrEmpty(Format) Then Format = "raw"
                    If Encoding_Input Is Nothing Then
                        Encoding_Input = New EncodingInfo
                        Encoding_Input.BitsPerSample = 16
                        Encoding_Input.Encoding = sox_encoding_t.SOX_ENCODING_SIGN2
                    End If
                    If Signal_Input Is Nothing Then
                        Signal_Input = New SignalInfo
                        Signal_Input.Rate = 8000
                        Signal_Input.Channels = 1
                        Signal_Input.Precision = 16
                    End If
                    If Signal_Output Is Nothing Then Signal_Output = Signal_Input

                Case ".alw"
                    ' If the format is alaw with no header, detect this here
                    If String.IsNullOrEmpty(Format) Then Format = "raw"
                    If Encoding_Input Is Nothing Then
                        Encoding_Input = New EncodingInfo
                        Encoding_Input.BitsPerSample = 8
                        Encoding_Input.Encoding = sox_encoding_t.SOX_ENCODING_ALAW
                    End If
                    If Signal_Input Is Nothing Then
                        Signal_Input = New SignalInfo
                        Signal_Input.Rate = 8000
                        Signal_Input.Channels = 1
                        Signal_Input.Precision = 8
                    End If
                    If Signal_Output Is Nothing Then Signal_Output = Signal_Input

            End Select
        End Sub

        ''' <summary> Detect format. </summary>
        Public Sub Detect_Format()
            If String.IsNullOrEmpty(Format) Then Format = FormatFile.GetFormat(FilePath)
        End Sub

        ''' <summary> Detect Encoding. </summary>
        Public Sub Detect_Encoding()
            If Encoding_Input Is Nothing Then Encoding_Input = FormatFile.GetEncoding(FilePath, Format)
        End Sub

        ''' <summary> Detect Signal. </summary>
        Public Sub Detect_Signal()
            If Signal_Input Is Nothing Then Signal_Input = FormatFile.GetSignal(FilePath, Format)
        End Sub

        ''' <summary> Detect Comments. </summary>
        Public Sub Detect_Comments()
            ' Detect Comments if there are any
            Try
                If OOB_Data Is Nothing Then OOB_Data = FormatFile.GetComments(FilePath, Format)
            Catch ex As Exception
            End Try
        End Sub

#End Region

    End Class

End Namespace
