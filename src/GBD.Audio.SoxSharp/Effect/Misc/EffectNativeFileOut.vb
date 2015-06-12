Imports System.Xml.Serialization
Imports GBD.Audio.SoxSharp.DataTypes
Imports GBD.Audio.SoxSharp.IO

Namespace Effect.Misc

    ''' <summary> File Output Effect For streaming data to a file via libsox. </summary>
    Public Class EffectNativeFileOut
        Inherits EffectBase

#Region "Properties"

        ''' <summary> The Effect Name. </summary>
        ''' <value> The Effect Name. </value>
        Public Overrides ReadOnly Property Name As String
            Get
                Return "File Output Effect"
            End Get
        End Property

        ''' <summary> Name of the Underlying Sox Effect to open. </summary>
        ''' <value> Name of the Underlying Sox Effect to open. </value>
        <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        Protected Overrides Property _SoxEffectName As String = "output"

        ''' <summary> Path to the file to be opened for writing. </summary>
        ''' <value> Path to the file to be opened for writing. </value>
        Public Property FilePath As String

        ''' <summary> File Format for output. </summary>
        ''' <value> The file format. </value>
        Public Property Format As String

        ''' <summary> Output File Stream. </summary>
        ''' <value> Output File Stream. </value>
        <XmlIgnore()> _
        Public Property FormatFile As FormatFile = Nothing

        ''' <summary> The description. </summary>
        ''' <value> The description. </value>
        Public Property Description As String = _
        "Used for writing files out, this is usually the last effect on the chain for output "

        ''' <summary> Out Of Bounds Data (suvh as ID3 Comments) to attach to the output file. </summary>
        ''' <value> Information describing the oob. </value>
        Public Property OOB_Data As OutOfBound

        ''' <summary> MP3 Lame Options for Output / Writing. </summary>
        ''' <value> Options that control the mp3 format. </value>
        Public Property Mp3Opts As Mp3LameOpts

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
        ''' <param name="fpath">     The file path. </param>
        ''' <param name="sigout">    The output signal. </param>
        ''' <param name="encout">    The output encoding. </param>
        ''' <param name="formatout"> The output format. </param>
        ''' <param name="oobdata">   The output oobdata. </param>
        Public Sub New(fpath As String, sigout As SignalInfo, encout As EncodingInfo, _
                formatout As String, Optional oobdata As OutOfBound = Nothing)
            FilePath = fpath
            Signal_Output = sigout
            Encoding_Output = encout
            Format = formatout
            OOB_Data = oobdata
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
            FormatFile = New FormatFile(FilePath, FormatBase.OpenMode.Write, Signal_Output, Encoding_Output, Format, OOB_Data)
            MyBase.Open()
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

        ''' <summary> Attach Effect to the Chain. </summary>
        ''' <param name="chain"> The chain to attach to. </param>
        Public Overrides Sub Attach(chain As EffectChain)
            ' Since this is typicaly the last effect in the chain we need to specify the signal output before opening
            Dim tmpeff = (From item In chain.Effects Where item.IsAttached = True).LastOrDefault
            If tmpeff IsNot Nothing AndAlso tmpeff.Signal_Output IsNot Nothing Then
                Signal_Output = tmpeff.Signal_Output.Clone
            End If
            MyBase.Attach(chain)
        End Sub

#End Region

#Region "Methods - Detection"

        ''' <summary> Detect All Parameters, provide input file signal as signalinfo. </summary>
        ''' <param name="sig"> The signal. </param>
        Public Sub Detect_All(sig As SignalInfo)
            CheckValid()
            Detect_Encoding(sig)
            Set_MP3LameOpts()
        End Sub

        ''' <summary> Check all supplied paramters have been provided. </summary>
        ''' <exception cref="ArgumentException"> Thrown when one or more arguments have unsupported or
        '''                                      illegal values. </exception>
        Public Sub CheckValid()
            ' Check Output File is specified
            If String.IsNullOrEmpty(FilePath) Then _
                Throw New ArgumentException("Output FilePath not specified")
            ' Check Output Format Specified
            If String.IsNullOrEmpty(Format) Then _
                Throw New ArgumentException("Output Format not specified")
        End Sub

        ''' <summary> Detect Encoding. </summary>
        ''' <param name="sig"> The signal. </param>
        Public Sub Detect_Encoding(sig As SignalInfo)
            If Encoding_Output Is Nothing Then Encoding_Output = FormatFile.GetEncoding(sig, Nothing, Format)
        End Sub

        ''' <summary> Set Mp3 Lame Options. </summary>
        Public Sub Set_MP3LameOpts()
            ' Set Mp3 Lame Options
            If Mp3Opts IsNot Nothing Then Encoding_Output.Compression = Mp3Opts.GenerateOutput
        End Sub

#End Region

    End Class

End Namespace
