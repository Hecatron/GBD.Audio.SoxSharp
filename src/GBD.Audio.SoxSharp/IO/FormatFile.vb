Imports System.IO
Imports GBD.Audio.SoxSharp.DataTypes
Imports GBD.Audio.SoxSharp.swig

Namespace IO

    ''' <summary>
    '''  This represents a Sox format type, a type of class returned for opened files.
    ''' </summary>
    Public Class FormatFile
        Inherits FormatBase
        Implements IDisposable

#Region "Constructors"

        ''' <summary> Default Constructor. </summary>
        Public Sub New()
            _SwigStorage = New sox_format_t
        End Sub

        ''' <summary> Default Constructor. </summary>
        ''' <param name="struct"> The structure. </param>
        Public Sub New(struct As sox_format_t)
            _SwigStorage = struct
        End Sub

        ''' <summary> Default Constructor. </summary>
        ''' <param name="path">     Full pathname of the file. </param>
        ''' <param name="mode">     The mode. </param>
        ''' <param name="signal">   The signal. </param>
        ''' <param name="encoding"> The encoding. </param>
        ''' <param name="ftype">    The ftype. </param>
        ''' <param name="oob">      The oob. </param>
        Public Sub New(path As String, mode As OpenMode, Optional signal As SignalInfo = Nothing _
                , Optional encoding As EncodingInfo = Nothing, Optional ftype As String = Nothing _
                , Optional oob As OutOfBound = Nothing)
            _SwigStorage = New sox_format_t
            If mode = OpenMode.Read Then OpenRead(path, signal, encoding, ftype)
            If mode = OpenMode.Write Then OpenWrite(path, signal, encoding, ftype, oob)
        End Sub

#End Region

#Region "Destructors"

        ''' <summary> Destructor. </summary>
        Protected Overrides Sub Finalize()
            Dispose()
            MyBase.Finalize()
        End Sub

        ''' <summary> Dispose of Memory Storage. </summary>
        Public Sub Dispose() Implements IDisposable.Dispose
            Close()
        End Sub

#End Region

#Region "Methods - Open Read"

        ''' <summary> Open a file for Reading. </summary>
        ''' <param name="filepath"> The filepath. </param>
        Public Sub OpenRead(filepath As String)
            OpenRead(filepath, Nothing, Nothing, Nothing)
        End Sub

        ''' <summary> Open a file for Reading. </summary>
        ''' <param name="filepath"> The filepath. </param>
        ''' <param name="signal">   The signal. </param>
        Public Sub OpenRead(filepath As String, signal As SignalInfo)
            OpenRead(filepath, signal, Nothing, Nothing)
        End Sub

        ''' <summary> Open a file for Reading. </summary>
        ''' <param name="filepath"> The filepath. </param>
        ''' <param name="signal">   The signal. </param>
        ''' <param name="encoding"> The encoding. </param>
        Public Sub OpenRead(filepath As String, signal As SignalInfo, encoding As EncodingInfo)
            OpenRead(filepath, signal, encoding, Nothing)
        End Sub

        ''' <summary> Open a file for Reading. </summary>
        ''' <exception cref="InvalidOperationException"> Thrown when the requested operation is invalid. </exception>
        ''' <exception cref="ArgumentException">         Thrown when one or more arguments have
        '''                                              unsupported or illegal values. </exception>
        ''' <exception cref="SoxException">              Thrown when a Sox error condition occurs. </exception>
        ''' <param name="filepath"> The filepath. </param>
        ''' <param name="signal">   The signal. </param>
        ''' <param name="encoding"> The encoding. </param>
        ''' <param name="ftype">    The ftype. </param>
        Public Sub OpenRead(filepath As String, signal As SignalInfo, encoding As EncodingInfo, ftype As String)
            If _IsOpen Then Throw New InvalidOperationException("This class already has another file opened")
            If File.Exists(filepath) = False Then Throw New ArgumentException("Filepath does not exist: " & filepath)

            ' Parse the Input Options
            Dim signal_sox As sox_signalinfo_t = Nothing
            If signal IsNot Nothing Then signal_sox = signal.SwigStorage
            Dim encoding_sox As sox_encodinginfo_t = Nothing
            If encoding IsNot Nothing Then encoding_sox = encoding.SwigStorage
            If ftype IsNot Nothing AndAlso ftype.Length > 10 Then ftype = ftype.Substring(0, 10)

            Try
                OpenHandle = libsox.sox_open_read(filepath, signal_sox, encoding_sox, ftype)
                _SwigStorage = New sox_format_t(OpenHandle.GetswigCPtr)
                _SignalInfo = New SignalInfo(_SwigStorage.signal)
                _EncodingInfo = New EncodingInfo(_SwigStorage.encoding)
                _OOBData = New OutOfBound(_SwigStorage.oob)
                _Handler = New FormatHandler(_SwigStorage.handler)
                _IsOpen = True
            Catch ex As Exception
                Throw New SoxException("Unknown problem opening the filestream for Reading")
            End Try

        End Sub

#End Region

#Region "Methods - Open Write"

        ''' <summary> Open a file for Writing. </summary>
        ''' <param name="filepath"> The filepath. </param>
        Public Sub OpenWrite(filepath As String)
            OpenWrite(filepath, Nothing, Nothing, Nothing)
        End Sub

        ''' <summary> Open a file for Writing. </summary>
        ''' <param name="filepath"> The filepath. </param>
        ''' <param name="signal">   The signal. </param>
        Public Sub OpenWrite(filepath As String, signal As SignalInfo)
            OpenWrite(filepath, signal, Nothing, Nothing)
        End Sub

        ''' <summary> Open a file for Writing. </summary>
        ''' <param name="filepath"> The filepath. </param>
        ''' <param name="signal">   The signal. </param>
        ''' <param name="encoding"> The encoding. </param>
        Public Sub OpenWrite(filepath As String, signal As SignalInfo, encoding As EncodingInfo)
            OpenWrite(filepath, signal, encoding, Nothing)
        End Sub

        ''' <summary> Open a file for Writing. </summary>
        ''' <exception cref="InvalidOperationException"> Thrown when the requested operation is invalid. </exception>
        ''' <exception cref="SoxException">              Thrown when a Sox error condition occurs. </exception>
        ''' <param name="filepath"> The filepath. </param>
        ''' <param name="signal">   The signal. </param>
        ''' <param name="encoding"> The encoding. </param>
        ''' <param name="ftype">    The ftype. </param>
        ''' <param name="oob">      The oob. </param>
        Public Sub OpenWrite(filepath As String, signal As SignalInfo, encoding As EncodingInfo, ftype As String, _
                             Optional oob As OutOfBound = Nothing)
            If _IsOpen Then Throw New InvalidOperationException("This class already has another file opened")
            If File.Exists(filepath) = True Then File.Delete(filepath)

            ' Parse the Input Options
            Dim signal_sox As sox_signalinfo_t = Nothing
            If signal IsNot Nothing Then signal_sox = signal.SwigStorage
            Dim encoding_sox As sox_encodinginfo_t = Nothing
            If encoding IsNot Nothing Then encoding_sox = encoding.SwigStorage
            Dim oob_sox As sox_oob_t = Nothing
            If oob IsNot Nothing Then oob_sox = oob.SwigStorage
            If ftype IsNot Nothing AndAlso ftype.Length > 10 Then ftype = ftype.Substring(0, 10)

            Try
                OpenHandle = libsox.sox_open_write(filepath, signal_sox, encoding_sox, ftype, oob_sox, Nothing)
                _SwigStorage = New sox_format_t(OpenHandle.GetswigCPtr)
                _SignalInfo = New SignalInfo(_SwigStorage.signal)
                _EncodingInfo = New EncodingInfo(_SwigStorage.encoding)
                _OOBData = New OutOfBound(_SwigStorage.oob)
                _Handler = New FormatHandler(_SwigStorage.handler)
                _IsOpen = True
            Catch ex As Exception
                Throw New SoxException("Unknown problem opening the filestream for Writing", ex)
            End Try

        End Sub

#End Region

#Region "Methods - Close"

        ''' <summary> Close the File Handle. </summary>
        ''' <exception cref="SoxException"> Thrown when a Sox error condition occurs. </exception>
        Public Sub Close()
            If _IsOpen = True And OpenHandle IsNot Nothing Then
                Try
                    Dim ret As Integer = libsox.sox_close(OpenHandle)
                    If ret <> sox_error_t.SOX_SUCCESS Then Throw New SoxException("Unknown problem closing the filestream")
                Catch ex As Exception
                    _IsOpen = False
                    Throw New SoxException("Unknown problem closing the filestream")
                End Try
            End If
            OpenHandle = Nothing
            _SignalInfo = Nothing
            _EncodingInfo = Nothing
            _OOBData = Nothing
            _Handler = Nothing
            If _SwigStorage IsNot Nothing Then _SwigStorage.Dispose()
            _SwigStorage = Nothing
            _IsOpen = False
        End Sub

#End Region

#Region "Shared Methods - Detect Encoding / Signal"

        ''' <summary> Get the Format for a given file path. </summary>
        ''' <exception cref="ArgumentException"> Thrown when one or more arguments have unsupported or
        '''                                      illegal values. </exception>
        ''' <param name="filepath"> The filepath. </param>
        ''' <returns> The format. </returns>
        Public Shared Function GetFormat(filepath As String) As String
            Dim ret As String
            If File.Exists(filepath) = False Then Throw New ArgumentException("Input File does not exist: " & filepath)
            Dim tmpfile As New FormatFile
            tmpfile.OpenRead(filepath, Nothing, Nothing, Nothing)
            ret = tmpfile.FileType
            tmpfile.Close()
            Return ret
        End Function

        ''' <summary> Get the Encoding for a given file path. </summary>
        ''' <exception cref="ArgumentException"> Thrown when one or more arguments have unsupported or
        '''                                      illegal values. </exception>
        ''' <param name="filepath"> The filepath. </param>
        ''' <param name="filetype"> The filetype. </param>
        ''' <returns> The encoding. </returns>
        Public Shared Function GetEncoding(filepath As String, Optional filetype As String = Nothing) As EncodingInfo
            Dim ret As EncodingInfo
            If File.Exists(filepath) = False Then Throw New ArgumentException("Input File does not exist: " & filepath)
            Dim tmpfile As New FormatFile
            tmpfile.OpenRead(filepath, Nothing, Nothing, filetype)
            ret = tmpfile.EncodingInfo.Clone
            tmpfile.Close()
            Return ret
        End Function

        ''' <summary> Get the Signal for a given file path. </summary>
        ''' <exception cref="ArgumentException"> Thrown when one or more arguments have unsupported or
        '''                                      illegal values. </exception>
        ''' <param name="filepath"> The filepath. </param>
        ''' <param name="filetype"> The filetype. </param>
        ''' <returns> The signal. </returns>
        Public Shared Function GetSignal(filepath As String, Optional filetype As String = Nothing) As SignalInfo
            Dim ret As SignalInfo
            If File.Exists(filepath) = False Then Throw New ArgumentException("Input File does not exist: " & filepath)
            Dim tmpfile As New FormatFile
            tmpfile.OpenRead(filepath, Nothing, Nothing, filetype)
            ret = tmpfile.SignalInfo.Clone
            tmpfile.Close()
            Return ret
        End Function

        ''' <summary> Get Out of Bounds Data. </summary>
        ''' <exception cref="ArgumentException"> Thrown when one or more arguments have unsupported or
        '''                                      illegal values. </exception>
        ''' <param name="filepath"> The filepath. </param>
        ''' <param name="filetype"> The filetype. </param>
        ''' <returns> The comments. </returns>
        Public Shared Function GetComments(filepath As String, Optional filetype As String = Nothing) As OutOfBound
            Dim ret As OutOfBound
            If File.Exists(filepath) = False Then Throw New ArgumentException("Input File does not exist: " & filepath)
            Dim tmpfile As New FormatFile
            tmpfile.OpenRead(filepath, Nothing, Nothing, filetype)
            ret = tmpfile.OOBData
            ret.ID3TagGroup.ReadComments()
            tmpfile.Close()
            Return ret
        End Function

        ''' <summary>
        '''  Gets an encoding specification basd on Signal / Encoding / format type it does this via the
        '''  creation of a temporary file, typically used for output encodings.
        ''' </summary>
        ''' <param name="signal">   The signal. </param>
        ''' <param name="encoding"> The encoding. </param>
        ''' <param name="filetype"> The filetype. </param>
        ''' <returns> The encoding. </returns>
        Public Shared Function GetEncoding( _
                     signal As SignalInfo, _
                     encoding As EncodingInfo, _
                     filetype As String _
                    ) As EncodingInfo

            Dim retval As EncodingInfo = Nothing
            Dim tmpformat As New FormatFile
            Dim destpath As String ' location of temporary file
            destpath = Reflection.Assembly.GetExecutingAssembly.Location
            destpath = Path.GetDirectoryName(destpath)
            destpath &= "\" & Guid.NewGuid().ToString() & "-tmpenc.tmp"

            Try
                tmpformat.OpenWrite(destpath, signal, encoding, filetype)
                retval = tmpformat.EncodingInfo.Clone
            Catch ex As Exception
            Finally
                If tmpformat IsNot Nothing Then tmpformat.Close()
                If File.Exists(destpath) = True Then File.Delete(destpath)
            End Try
            Return retval
        End Function

#End Region

    End Class

End Namespace
