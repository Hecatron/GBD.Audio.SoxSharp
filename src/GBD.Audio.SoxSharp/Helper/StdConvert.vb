' TODO Test Alaw

Imports GBD.Audio.SoxSharp.DataTypes
Imports GBD.Audio.SoxSharp.Effect.Mastering
Imports GBD.Audio.SoxSharp.Effect.Mixing
Imports GBD.Audio.SoxSharp.Effect.Tone
Imports GBD.Audio.SoxSharp.IO

Namespace Helper

    ''' <summary> Standard Conversion Methods. </summary>
    Public Class StdConvert

        Public Shared ConversionSetup_SyncLock As New Object

        ''' <summary> Standard Convert to Wav. </summary>
        ''' <exception cref="ArgumentException"> Thrown when one or more arguments have unsupported or
        '''                                      illegal values. </exception>
        ''' <param name="Sourcepath">    The sourcepath. </param>
        ''' <param name="Destpath">      The destpath. </param>
        ''' <param name="min_outputKhz"> The minimum output kHz. </param>
        ''' <param name="bit_type">      Type of the bit. </param>
        Public Shared Sub ConvertToWav(Sourcepath As String, Destpath As String, _
            Optional min_outputKhz As EffectRate.StdBitRate = EffectRate.StdBitRate.Khz22, _
            Optional bit_type As Integer = 16)

            If System.IO.File.Exists(Sourcepath) = False Then _
                Throw New ArgumentException("Input File Does Not Exist")

            Dim conv As New FileConvert
            SyncLock ConversionSetup_SyncLock
                conv.Input.FilePath = Sourcepath
                conv.Input.Detect_All()
                conv.Output.FilePath = Destpath
                conv.Output.Format = "wav"
                conv.Output.Detect_All(conv.Input.Signal_Input)

                ' Add the bit rate converter - to upsample to the minimum if needed
                If conv.Input.Signal_Input.Rate < min_outputKhz Then
                    Dim Eff_BirRate As New EffectRate(min_outputKhz)
                    conv.Effects.Add(Eff_BirRate)
                End If

                ' If the input is a vc / vox file then add a highpass filter to get rid of the DC offset
                If conv.Input.Format = "vox" Then
                    Dim Eff_HighPass As New EffectHighPass(10)
                    conv.Effects.Add(Eff_HighPass)
                End If

                conv.SetupChain()
                conv.Chain.Open() ' Open the Chain
                conv.Chain.AttachAll() ' Attach All Effects
                ' Override Signal Precision
                conv.Output.Signal_Output.Precision = 16
                If bit_type = 8 Then conv.Output.Signal_Output.Precision = 8
            End SyncLock

            conv.Chain.StartConversion() ' Start the Conversion

            SyncLock ConversionSetup_SyncLock
                conv.Chain.Close() ' Close the Chain
                conv.Dispose()
            End SyncLock
        End Sub

        ''' <summary> Standard Convert to Mp3. </summary>
        ''' <exception cref="ArgumentException"> Thrown when one or more arguments have unsupported or
        '''                                      illegal values. </exception>
        ''' <param name="Sourcepath">    The sourcepath. </param>
        ''' <param name="Destpath">      The destpath. </param>
        ''' <param name="min_outputKhz"> The minimum output kHz. </param>
        ''' <param name="Mp3LameOpts">   Options for controlling the mp 3 lame. </param>
        ''' <param name="Id3Details">    The identifier 3 details. </param>
        Public Shared Sub ConvertToMp3(Sourcepath As String, Destpath As String, _
            Optional min_outputKhz As EffectRate.StdBitRate = EffectRate.StdBitRate.Khz22, _
            Optional Mp3LameOpts As Mp3LameOpts = Nothing, _
            Optional Id3Details As ID3TagGroup = Nothing)

            If System.IO.File.Exists(Sourcepath) = False Then _
                Throw New ArgumentException("Input File Does Not Exist")

            ' Default to Variable Bit Rate if not Specified
            If Mp3LameOpts Is Nothing Then
                Mp3LameOpts = New Mp3LameOpts
                Mp3LameOpts.BitRateType = Mp3LameOpts.BitRate_Type.Variable
                Mp3LameOpts.VariableBitRate = Mp3LameOpts.Mp3Quality_Type.Quality9
                Mp3LameOpts.Quality = Mp3LameOpts.Mp3Quality_Type.Quality0
            End If

            Dim conv As New FileConvert
            SyncLock ConversionSetup_SyncLock
                conv.Input.FilePath = Sourcepath
                conv.Input.Detect_All()
                conv.Output.FilePath = Destpath
                conv.Output.Format = "mp3"
                conv.Output.Mp3Opts = Mp3LameOpts
                conv.Output.Detect_All(conv.Input.Signal_Input)

                ' Add ID3Tag Information
                If Id3Details IsNot Nothing Then
                    conv.OOB_Data = New OutOfBound
                    conv.OOB_Data.ID3TagGroup = Id3Details
                    conv.OOB_Data.OpenComments()
                End If

                ' Add the bit rate converter - to upsample to the minimum if needed
                If conv.Input.Signal_Input.Rate < min_outputKhz Then
                    Dim Eff_BirRate As New EffectRate(min_outputKhz)
                    conv.Effects.Add(Eff_BirRate)
                End If

                ' If the input is a vc / vox file then add a highpass filter to get rid of the DC offset
                If conv.Input.Format = "vox" Then
                    Dim Eff_HighPass As New EffectHighPass(10)
                    conv.Effects.Add(Eff_HighPass)
                End If

                conv.SetupChain() ' Setup the Effects Chain
                conv.Chain.Open() ' Open the Chain
                conv.Chain.AttachAll() ' Attach All Effects
            End SyncLock

            conv.Chain.StartConversion() ' Run the Effects through the chain

            SyncLock ConversionSetup_SyncLock
                conv.Chain.Close() ' Close the Chain
                conv.Dispose()
            End SyncLock
        End Sub

        ''' <summary> Standard Convert to VC / Vox. </summary>
        ''' <exception cref="ArgumentException"> Thrown when one or more arguments have unsupported or
        '''                                      illegal values. </exception>
        ''' <param name="Sourcepath"> The sourcepath. </param>
        ''' <param name="Destpath">   The destpath. </param>
        Public Shared Sub ConvertToVox(Sourcepath As String, Destpath As String)

            If System.IO.File.Exists(Sourcepath) = False Then _
                Throw New ArgumentException("Input File Does Not Exist")

            Dim conv As New FileConvert
            SyncLock ConversionSetup_SyncLock
                conv.Input.FilePath = Sourcepath
                conv.Input.Detect_All()
                conv.Output.FilePath = Destpath
                conv.Output.Format = "vox"
                conv.Output.Detect_All(conv.Input.Signal_Input)

                ' Add the bit rate converter - VC always needs to be 8Khz
                Dim Eff_BirRate As New EffectRate(EffectRate.StdBitRate.Khz08)
                conv.Effects.Add(Eff_BirRate)

                ' stereo to mono conversion
                Dim num_channels As UInteger
                num_channels = conv.Input.Signal_Input.Channels
                If num_channels > 1 Then
                    ' Down mix to just one channel
                    Dim Eff_Channels As New EffectChannels(1)
                    conv.Effects.Add(Eff_Channels)
                End If

                conv.SetupChain() ' Setup the Effects Chain
                conv.Chain.Open() ' Open the Chain
                conv.Chain.AttachAll() ' Attach All Effects
            End SyncLock

            conv.Chain.StartConversion() ' Run the Effects through the chain

            SyncLock ConversionSetup_SyncLock
                conv.Chain.Close() ' Close the Chain
                conv.Dispose()
            End SyncLock
        End Sub

        ''' <summary> Standard Convert to VC / Vox. </summary>
        ''' <exception cref="ArgumentException"> Thrown when one or more arguments have unsupported or
        '''                                      illegal values. </exception>
        ''' <param name="Sourcepath"> The sourcepath. </param>
        ''' <param name="Destpath">   The destpath. </param>
        Public Shared Sub ConvertToAlaw(Sourcepath As String, Destpath As String)

            If System.IO.File.Exists(Sourcepath) = False Then _
                Throw New ArgumentException("Input File Does Not Exist")

            Dim conv As New FileConvert
            SyncLock ConversionSetup_SyncLock
                conv.Input.FilePath = Sourcepath
                conv.Input.Detect_All()
                conv.Output.FilePath = Destpath
                conv.Output.Format = "raw"
                conv.Output.Detect_All(conv.Input.Signal_Input)

                ' Add the bit rate converter - VC always needs to be 8Khz
                Dim Eff_BirRate As New EffectRate(EffectRate.StdBitRate.Khz08)
                conv.Effects.Add(Eff_BirRate)

                ' stereo to mono conversion
                Dim num_channels As UInteger
                num_channels = conv.Input.Signal_Input.Channels
                If num_channels > 1 Then
                    ' Down mix to just one channel
                    Dim Eff_Channels As New EffectChannels(1)
                    conv.Effects.Add(Eff_Channels)
                End If

                ' Override the Output Encoding
                conv.Output.Encoding_Output.Encoding = swig.sox_encoding_t.SOX_ENCODING_ALAW
                conv.Output.Encoding_Output.BitsPerSample = 8

                conv.SetupChain() ' Setup the Effects Chain
                conv.Chain.Open() ' Open the Chain
                conv.Chain.AttachAll() ' Attach All Effects
            End SyncLock

            conv.Chain.StartConversion() ' Run the Effects through the chain

            SyncLock ConversionSetup_SyncLock
                conv.Chain.Close() ' Close the Chain
                conv.Dispose()
            End SyncLock
        End Sub

    End Class

End Namespace
