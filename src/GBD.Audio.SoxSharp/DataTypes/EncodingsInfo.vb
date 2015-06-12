Imports GBD.Audio.SoxSharp.swig

Namespace DataTypes

    ''' <summary> Basic information about an encoding. </summary>
    Public Class EncodingsInfo
        Implements IDisposable

#Region "Properties - Local Storage"

        ''' <summary> Underlying Swig Storage. </summary>
        ''' <value> Underlying Swig Storage. </value>
        Public ReadOnly Property SwigStorage As sox_encodings_info_t
            Get
                Return _SwigStorage
            End Get
        End Property
        <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        Protected Property _SwigStorage As sox_encodings_info_t

#End Region

#Region "Properties - Swig Wrapper"

        ''' <summary>
        '''  Flags for sox_encodings_info_t: lossless/lossy1/lossy2. no flags specified (implies lossless
        '''  encoding) = 0 encode, decode: lossy once = 1 encode, decode, encode, decode: lossy twice = 2.
        ''' </summary>
        ''' <value> Encodings flags. </value>
        Public Property Flags As sox_encodings_flags_t
            Get
                Return _SwigStorage.flags
            End Get
            Set(value As sox_encodings_flags_t)
                _SwigStorage.flags = value
            End Set
        End Property

        ''' <summary> Encoding Name. </summary>
        ''' <value> Encoding Name. </value>
        Public Property Name As String
            Get
                Return _SwigStorage.name
            End Get
            Set(value As String)
                _SwigStorage.name = value
            End Set
        End Property

        ''' <summary> Encoding Description. </summary>
        ''' <value> Encoding Description. </value>
        Public Property Description As String
            Get
                Return _SwigStorage.desc
            End Get
            Set(value As String)
                _SwigStorage.desc = value
            End Set
        End Property

#End Region

#Region "Constructors"

        ''' <summary> Default Constructor. </summary>
        Public Sub New()
            _SwigStorage = New sox_encodings_info_t
        End Sub

        ''' <summary> Default Constructor. </summary>
        ''' <param name="storage"> Use this as the swig storage value. </param>
        Public Sub New(storage As sox_encodings_info_t)
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

        ''' <summary> Get a list of available Encodings Infos. </summary>
        ''' <exception cref="SoxException"> Thrown when a Sox error condition occurs. </exception>
        ''' <returns> A list of inbuilt encodings infos. </returns>
        Public Shared Function GetEncodingsInfo() As List(Of EncodingsInfo)
            Dim ret As New List(Of EncodingsInfo)
            Dim tmpencinfo As sox_encodings_info_t
            Try
                tmpencinfo = libsox.sox_get_encodings_info()
            Catch ex As Exception
                Throw New SoxException("Error Getting the Encodings Values", ex)
            End Try
            While tmpencinfo IsNot Nothing
                Dim tmpitem As New EncodingsInfo(tmpencinfo)
                ret.Add(tmpitem)
                Dim tmpptr As IntPtr = tmpencinfo.GetswigCPtr
                tmpptr = tmpptr + IntPtr.Size ' Flags
                tmpptr = tmpptr + IntPtr.Size ' Name
                tmpptr = tmpptr + IntPtr.Size ' Description
                tmpencinfo = New sox_encodings_info_t(tmpptr)
                If tmpencinfo.name = Nothing Then tmpencinfo = Nothing
            End While
            Return ret
        End Function

#End Region

    End Class

End Namespace
