Imports System.Collections.ObjectModel
Imports System.IO
Imports System.Runtime.InteropServices
Imports GBD.Audio.SoxSharp.DataTypes
Imports GBD.Audio.SoxSharp.swig

Namespace UnmanagedHelper

    ''' <summary> Dll Loader Helper Code Handles the Loading and init of the libsox library. </summary>
    Public Class DllLoader

#Region "Properties"

        ''' <summary> If the External Audio Conversion Dll's have been loaded into memory. </summary>
        ''' <value> The is DLL loaded. </value>
        Public Shared ReadOnly Property IsDllLoaded As Boolean
            Get
                Return _IsDllLoaded
            End Get
        End Property
        Protected Shared Property _IsDllLoaded As Boolean = False

        ''' <summary>
        '''  Contains a List of Class's that require the Library to be initialised / Open for Business.
        ''' </summary>
        ''' <value> The clients. </value>
        Public Shared ReadOnly Property Clients As ReadOnlyCollection(Of Object)
            Get
                Return _Clients.AsReadOnly
            End Get
        End Property
        Protected Shared Property _Clients As New List(Of Object)

        ''' <summary> If the libsox library is active. </summary>
        ''' <value> The is active. </value>
        Public Shared ReadOnly Property IsActive As Boolean
            Get
                Return _IsActive
            End Get
        End Property

        ''' <summary> Gets or sets the is active. </summary>
        ''' <value> The is active. </value>
        Protected Shared Property _IsActive As Boolean

        ''' <summary>
        ''' Make this function thread safe
        ''' </summary>
        Protected Shared Register_SyncLock As New Object

#End Region

#Region "Shared Methods - Pinvoke"

        ''' <summary> PInvoke Load dll into memory for 32bit / 64bit. </summary>
        ''' <param name="lpFileName"> Filename of the file. </param>
        ''' <returns> The library. </returns>
        <DllImport("kernel32.dll", SetLastError:=True)> _
        Public Shared Function LoadLibrary(ByVal lpFileName As String) As IntPtr
        End Function

#End Region

#Region "Shared Methods - Load DLL"

        ''' <summary> PreLoad external Dll's into memory. </summary>
        ''' <exception cref="SoxException"> Thrown when a Sox error condition occurs. </exception>
        Public Shared Sub LoadDlls()
            If _IsDllLoaded = True Then Exit Sub
            Dim libsoxpath As String = FindLibSoxDll()
            If libsoxpath Is Nothing Then Throw New SoxException("Unable to locate libsox.dll")
            Dim ret As IntPtr = LoadLibrary(libsoxpath) ' Main LibSox library
            If ret = IntPtr.Zero Then Throw New SoxException("Failure to Load dll :" & libsoxpath)
            ' The other dll's are loaded via libsox directly via LoadLibrary along the same path
            _IsDllLoaded = True
        End Sub

        ''' <summary> Find the Path to the LibSox.dll. </summary>
        ''' <returns> The found library sox DLL. </returns>
        Public Shared Function FindLibSoxDll() As String
            Dim windir As String = Environment.GetEnvironmentVariable("WINDIR")
            Dim curdir As String = AppDomain.CurrentDomain.BaseDirectory
            If Is64bit() Then
                If File.Exists(Path.Combine(curdir, "sox-14.4.1-x64\LibSoX.dll")) Then _
                    Return Path.Combine(curdir, "sox-14.4.1-x64\LibSoX.dll")
                If File.Exists(Path.Combine(windir, "System32", "sox-14.4.1\LibSoX.dll")) Then _
                    Return Path.Combine(windir, "System32", "sox-14.4.1\LibSoX.dll")
            Else
                If File.Exists(Path.Combine(curdir, "sox-14.4.1-x32\LibSoX.dll")) Then _
                    Return Path.Combine(curdir, "sox-14.4.1-x32\LibSoX.dll")
                If File.Exists(Path.Combine(windir, "SysWOW64", "sox-14.4.1\LibSoX.dll")) Then _
                    Return Path.Combine(windir, "SysWOW64", "sox-14.4.1\LibSoX.dll")
                If File.Exists(Path.Combine(windir, "System32", "sox-14.4.1\LibSoX.dll")) Then _
                    Return Path.Combine(windir, "System32", "sox-14.4.1\LibSoX.dll")
            End If
            Return Nothing
        End Function

        ''' <summary> Return True if we're on a 64 bit Operating system. </summary>
        ''' <returns> true if 64bit, false if not. </returns>
        Public Shared Function Is64bit() As Boolean
            Return (Marshal.SizeOf(GetType(IntPtr)) * 8) = 64
        End Function

#End Region

#Region "Shared Methods - Library Init"

        ''' <summary> Register a client with the library. </summary>
        ''' <exception cref="SoxException"> Thrown when a Sox error condition occurs. </exception>
        ''' <param name="registered_class"> The registered class. </param>
        Public Shared Sub Register(registered_class As Object)
            If _IsDllLoaded = False Then LoadDlls()
            SyncLock Register_SyncLock
                ' Already Registered so don't increment
                If _Clients.Contains(registered_class) Then Exit Sub
                ' Add to the List
                _Clients.Add(registered_class)
                ' Init the library if we have at least one item in the list
                If _Clients.Count > 0 And _IsActive = False Then
                    Dim retval As Integer
                    Try
                        retval = libsox.sox_init
                    Catch ex As Exception
                        Throw New SoxException("Error Opening LibSox", ex)
                    End Try
                    If retval <> 0 Then Throw New SoxException("Error Opening LibSox")
                    _IsActive = True
                End If
            End SyncLock
        End Sub

        ''' <summary> UnRegister a client with the library. </summary>
        ''' <exception cref="SoxException"> Thrown when a Sox error condition occurs. </exception>
        ''' <param name="registered_class"> The registered class. </param>
        ''' <returns> true if it succeeds, false if it fails. </returns>
        Public Shared Function UnRegister(registered_class As Object) As Boolean
            SyncLock Register_SyncLock
                ' Not Already Registered so don't decrement
                If Not _Clients.Contains(registered_class) Then Return False
                ' Remove from the List
                _Clients.Remove(registered_class)
                ' If the number of items in the list is 0 then Shut down the library
                If _Clients.Count <= 0 And _IsActive = True Then
                    Dim retval As Integer
                    Try
                        retval = libsox.sox_quit
                    Catch ex As Exception
                        Throw New SoxException("Error Closing LibSox", ex)
                    End Try
                    If retval <> 0 Then Throw New SoxException("Error Closing LibSox")
                    _IsActive = False
                    Return True
                End If
                Return False
            End SyncLock
        End Function

#End Region

    End Class

End Namespace
