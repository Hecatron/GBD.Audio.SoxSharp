Imports GBD.Audio.SoxSharp.DataTypes
Imports GBD.Audio.SoxSharp.Effect
Imports GBD.Audio.SoxSharp.Effect.Misc
Imports GBD.Audio.SoxSharp.UnmanagedHelper

Namespace Helper

    ''' <summary> Helper Class for performing file based conversions. </summary>
    Public Class FileConvert
        Implements IDisposable

#Region "Properties"

        ''' <summary> The first input file effect. </summary>
        ''' <value> The first input file effect. </value>
        Public Property Input As EffectNativeFileIn

        ''' <summary> The last output file effect. </summary>
        ''' <value> The last output file effect. </value>
        Public Property Output As EffectNativeFileOut

        ''' <summary> Out of Bounds Data such as Comments for MP3's. </summary>
        ''' <value> Out of Bounds Data such as Comments for MP3's. </value>
        Public Property OOB_Data As OutOfBound

        ''' <summary> List of Effects to apply inbetween file input / output. </summary>
        ''' <value> List of Effects to apply inbetween file input / output. </value>
        Public Property Effects As New List(Of EffectBase)

        ''' <summary> Libsox Effects Chain. </summary>
        ''' <value> Libsox Effects Chain. </value>
        Public Property Chain As EffectChain

#End Region

#Region "Constructors / Destructors"

        ''' <summary> Default Constructor. </summary>
        Public Sub New()
            DllLoader.Register(Me)
            Input = New EffectNativeFileIn
            Output = New EffectNativeFileOut
        End Sub

        ''' <summary> Dispose of the Object. </summary>
        Public Sub Dispose() Implements IDisposable.Dispose
            DllLoader.UnRegister(Me)
        End Sub

#End Region

#Region "Methods"

        ''' <summary> Start the Conversion. </summary>
        ''' <exception cref="ArgumentException"> Thrown when one or more arguments have unsupported or
        '''                                      illegal values. </exception>
        Public Sub Start()
            If System.IO.File.Exists(Input.FilePath) = False Then _
                Throw New ArgumentException("Input File Does Not Exist")
            SetupIO() ' Setup Intput / Output
            SetupChain() ' Setup the Effects Chain
            RunChain() ' Run the Effects through the chain
        End Sub

        ''' <summary> Setup Input / Output. </summary>
        Public Sub SetupIO()
            ' Setup Input
            Input.Detect_All()

            ' Copy ID3 / Mp3 Comments, use OOBData if specified
            If OOB_Data IsNot Nothing Then Output.OOB_Data = OOB_Data

            ' If OOBData is not specified then look for comments in the Input
            If OOB_Data Is Nothing AndAlso Input.OOB_Data IsNot Nothing AndAlso Input.OOB_Data.ID3TagGroup IsNot Nothing Then
                OOB_Data = New OutOfBound
                OOB_Data.ID3TagGroup = Input.OOB_Data.ID3TagGroup.Clone
                OOB_Data.OpenComments()
                Output.OOB_Data = OOB_Data
            End If

            ' Setup Output
            Output.Detect_All(Input.Signal_Input)
        End Sub

        ''' <summary> Setup the Efffects on the Conversion Chain. </summary>
        Public Sub SetupChain()
            Chain = New EffectChain(Input.Encoding_Input, Output.Encoding_Output) ' Setup a new effects Chain
            Chain.Effects.Add(Input) ' Add Input File Effect
            ' Add User Specified effects
            For Each item As EffectBase In Effects
                Chain.Effects.Add(item)
            Next
            Chain.Effects.Add(Output) ' Add Output File Effect
        End Sub

        ''' <summary> Run the Effects through the chain. </summary>
        Public Sub RunChain()
            Chain.Open() ' Open the Chain
            Chain.AttachAll() ' Attach All Effects
            Chain.StartConversion() ' Start the Conversion
            Chain.Close() ' Close the Chain
        End Sub

#End Region

    End Class

End Namespace
