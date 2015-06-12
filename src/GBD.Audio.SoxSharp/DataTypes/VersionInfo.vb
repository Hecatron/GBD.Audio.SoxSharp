Imports GBD.Audio.SoxSharp.swig

Namespace DataTypes

    ''' <summary> Returns Version Information about the version of libsox in use. </summary>
    Public Class VersionInfo
        Implements IDisposable

#Region "Types"

        ''' <summary> Feature flags = popen | magic | threads | memopen. </summary>
        <Flags()> _
        Public Enum VersionFlags
            None = 0
            Popen = 1
            Magic = 2
            Threads = 4
            Memopen = 8
        End Enum

#End Region

#Region "Properties - Local Storage"

        ''' <summary> Underlying Swig Storage. </summary>
        ''' <value> Underlying Swig Storage. </value>
        Public ReadOnly Property SwigStorage As sox_version_info_t
            Get
                Return _SwigStorage
            End Get
        End Property
        <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        Protected Property _SwigStorage As sox_version_info_t

#End Region

#Region "Properties - Swig Wrapper"

        ''' <summary> Structure size = sizeof(sox_version_info_t) </summary>
        ''' <value> Structure size. </value>
        Public ReadOnly Property Size As UInteger
            Get
                Return _SwigStorage.size
            End Get
        End Property

        ''' <summary> Feature flags = popen | magic | threads | memopen. </summary>
        ''' <value> Feature flags. </value>
        Public ReadOnly Property Flags As VersionFlags
            Get
                Dim ret As VersionFlags
                [Enum].TryParse(Of VersionFlags)(_SwigStorage.flags.ToString, ret)
                Return ret
            End Get
        End Property

        ''' <summary> Version number = 0x140400. </summary>
        ''' <value> The version code. </value>
        Public ReadOnly Property VersionCode As UInteger
            Get
                Return _SwigStorage.version_code
            End Get
        End Property

        ''' <summary> Version string = sox_version(), for example, "14.4.0". </summary>
        ''' <value> The version string. </value>
        Public ReadOnly Property Version As String
            Get
                Return _SwigStorage.version
            End Get
        End Property

        ''' <summary> Version extra info or null = "PACKAGE_EXTRA", for example, "beta". </summary>
        ''' <value> The version extra info. </value>
        Public ReadOnly Property VersionExtra As String
            Get
                Return _SwigStorage.version_extra
            End Get
        End Property

        ''' <summary> Build time = "__DATE__ __TIME__", for example, "Jan  7 2010 03:31:50". </summary>
        ''' <value> The Build time. </value>
        Public ReadOnly Property Time As String
            Get
                Return _SwigStorage.time
            End Get
        End Property

        ''' <summary> Distro or null = "DISTRO", for example, "Debian". </summary>
        ''' <value> The distro. </value>
        Public ReadOnly Property Distro As String
            Get
                Return _SwigStorage.distro
            End Get
        End Property

        ''' <summary> Compiler info or null, for example, "msvc 160040219". </summary>
        ''' <value> The compiler. </value>
        Public ReadOnly Property Compiler As String
            Get
                Return _SwigStorage.compiler
            End Get
        End Property

        ''' <summary> Arch, for example, "1248 48 44 L OMP". </summary>
        ''' <value> The arch. </value>
        Public ReadOnly Property Arch As String
            Get
                Return _SwigStorage.arch
            End Get
        End Property

#End Region

#Region "Constructors"

        ''' <summary> Default Constructor. </summary>
        Public Sub New()
            _SwigStorage = New sox_version_info_t
        End Sub

        ''' <summary> Default Constructor. </summary>
        ''' <param name="storage"> Use this as the swig storage value. </param>
        Public Sub New(storage As sox_version_info_t)
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

        ''' <summary> Returns version number string of libSoX, for example, "14.4.0". </summary>
        ''' <returns> The version. </returns>
        Public Shared Function GetVersion() As String
            Dim ret As String
            Try
                ret = libsox.sox_version()
            Catch ex As Exception
                Throw New SoxException("Error Getting the Version Info", ex)
            End Try
            Return ret
        End Function

        ''' <summary> Returns information about this build of libsox. </summary>
        ''' <returns> The version information. </returns>
        Public Shared Function GetVersionInfo() As VersionInfo
            Dim ret As VersionInfo
            Try
                Dim tmpval As sox_version_info_t
                tmpval = libsox.sox_version_info()
                ret = New VersionInfo(tmpval)
            Catch ex As Exception
                Throw New SoxException("Error Getting the Version Info", ex)
            End Try
            Return ret
        End Function

#End Region

    End Class

End Namespace
