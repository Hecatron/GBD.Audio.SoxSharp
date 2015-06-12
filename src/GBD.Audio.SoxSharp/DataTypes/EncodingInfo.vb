Imports GBD.Audio.SoxSharp.swig
Imports GBD.Audio.SoxSharp.UnmanagedHelper

Namespace DataTypes

    ''' <summary> Information about the audio encoding. </summary>
    Public Class EncodingInfo
        Implements IDisposable

#Region "Properties - Local Storage"

        ''' <summary> Underlying Swig Storage. </summary>
        ''' <value> Underlying Swig Storage. </value>
        Public ReadOnly Property SwigStorage As sox_encodinginfo_t
            Get
                Return _SwigStorage
            End Get
        End Property
        <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        Protected Property _SwigStorage As sox_encodinginfo_t

#End Region

#Region "Properties - Swig Wrapper"

        ''' <summary> Encoding Format of sample numbers. </summary>
        ''' <value> Encoding Format of sample numbers. </value>
        Public Property Encoding As sox_encoding_t
            Get
                Return _SwigStorage.encoding
            End Get
            Set(value As sox_encoding_t)
                _SwigStorage.encoding = value
            End Set
        End Property

        ''' <summary>
        '''  Bits per sample 0 if unknown or variable, uncompressed value if lossless, compressed value if
        '''  lossy.
        ''' </summary>
        ''' <value> The bits per sample. </value>
        Public Property BitsPerSample As UInteger
            Get
                Return _SwigStorage.bits_per_sample
            End Get
            Set(value As UInteger)
                _SwigStorage.bits_per_sample = value
            End Set
        End Property

        ''' <summary> Compression Factor (where applicable) </summary>
        ''' <value> Compression Factor (where applicable) </value>
        Public Property Compression As Double
            Get
                Return _SwigStorage.compression
            End Get
            Set(value As Double)
                _SwigStorage.compression = value
            End Set
        End Property

        ''' <summary>
        '''  Should bytes be reversed? If this is default during sox_open_read or sox_open_write, libSoX
        '''  will set them to either no or yes according to the machine or format default.
        ''' </summary>
        ''' <value> If to reverse the byte order. </value>
        Public Property ReverseBytes As sox_option_t
            Get
                Return _SwigStorage.reverse_bytes
            End Get
            Set(value As sox_option_t)
                _SwigStorage.reverse_bytes = value
            End Set
        End Property

        ''' <summary>
        '''  Should nibbles be reversed? If this is default during sox_open_read or sox_open_write, libSoX
        '''  will set them to either no or yes according to the machine or format default.
        ''' </summary>
        ''' <value> If to reverse the nibble order. </value>
        Public Property ReverseNibbles As sox_option_t
            Get
                Return _SwigStorage.reverse_nibbles
            End Get
            Set(value As sox_option_t)
                _SwigStorage.reverse_nibbles = value
            End Set
        End Property

        ''' <summary>
        '''  Should bits be reversed? If this is default during sox_open_read or sox_open_write, libSoX
        '''  will set them to either no or yes according to the machine or format default.
        ''' </summary>
        ''' <value> If to reverse the bit order. </value>
        Public Property ReverseBits As sox_option_t
            Get
                Return _SwigStorage.reverse_bits
            End Get
            Set(value As sox_option_t)
                _SwigStorage.reverse_bits = value
            End Set
        End Property

        ''' <summary> If set to true, the format should reverse its default endianness. </summary>
        ''' <value> If to reverse the default endianness. </value>
        Public Property OppositeEndian As Boolean
            Get
                Return CHelper.SoxBoolToBoolean(_SwigStorage.opposite_endian)
            End Get
            Set(value As Boolean)
                _SwigStorage.opposite_endian = CHelper.BooleanToSoxBool(value)
            End Set
        End Property

#End Region

#Region "Constructors"

        ''' <summary> Default Constructor. </summary>
        Public Sub New()
            _SwigStorage = New sox_encodinginfo_t
        End Sub

        ''' <summary> Default Constructor. </summary>
        ''' <param name="storage"> Use this as the swig storage value. </param>
        Public Sub New(storage As sox_encodinginfo_t)
            _SwigStorage = storage
        End Sub

#End Region

#Region "Destructors"

        ''' <summary> Destructor. </summary>
        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            Dispose()
        End Sub

        ''' <summary> Dispose of Memory Storage. </summary>
        Public Sub Dispose() Implements IDisposable.Dispose
            If _SwigStorage IsNot Nothing Then _SwigStorage.Dispose()
        End Sub

#End Region

#Region "Methods"

        ''' <summary> Clone the Object. </summary>
        ''' <returns> A copy of this object. </returns>
        Public Function Clone() As EncodingInfo
            Dim ret As New EncodingInfo
            ret.Encoding = Encoding
            ret.BitsPerSample = BitsPerSample
            ret.Compression = Compression
            ret.ReverseBytes = ReverseBytes
            ret.ReverseNibbles = ReverseNibbles
            ret.ReverseBits = ReverseBits
            ret.OppositeEndian = OppositeEndian
            Return ret
        End Function

        ''' <summary> Set the EncodingInfo to default values. </summary>
        Public Sub SetDefaults()
            Try
                libsox.sox_init_encodinginfo(_SwigStorage)
            Catch ex As Exception
                Throw New SoxException("Error Setting Default Values", ex)
            End Try
        End Sub

#End Region

    End Class

End Namespace
