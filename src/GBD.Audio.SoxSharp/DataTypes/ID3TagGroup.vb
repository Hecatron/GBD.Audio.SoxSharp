Imports System.Collections.ObjectModel
Imports System.Xml.Serialization
Imports GBD.Audio.SoxSharp.swig
Imports GBD.Audio.SoxSharp.UnmanagedHelper

Namespace DataTypes

    ''' <summary>
    '''  Represents a Group of ID3 Tags / Comments read via Libsox. I think Only ID3v1 is supported via
    '''  libsox. We don't bother to use the libsox comments methods instead we create our own /
    '''  destruct a pointer to an array of strings.
    ''' </summary>
    Public Class ID3TagGroup
        Implements IDisposable

#Region "Types"

        ''' <summary> Mp3 ID3 Genre. </summary>
        Public Enum Genres
            Unknown = -1
            Blues = 0
            Classic_Rock = 1
            Country = 2
            Dance = 3
            Disco = 4
            Funk = 5
            Grunge = 6
            HipHop = 7
            Jazz = 8
            Metal = 9
            New_Age = 10
            Oldies = 11
            Other = 12
            Pop = 13
            RAndB = 14
            Rap = 15
            Reggae = 16
            Rock = 17
            Techno = 18
            Industrial = 19
            Alternative = 20
            Ska = 21
            Death_Metal = 22
            Pranks = 23
            Soundtrack = 24
            EuroTechno = 25
            Ambient = 26
            TripHop = 27
            Vocal = 28
            JazzFunk = 29
            Fusion = 30
            Trance = 31
            Classical = 32
            Instrumental = 33
            Acid = 34
            House = 35
            Game = 36
            Sound_Clip = 37
            Gospel = 38
            Noise = 39
            Alternative_Rock = 40
            Bass = 41
            Soul = 42
            Punk = 43
            Space = 44
            Meditative = 45
            Instrumental_Pop = 46
            Instrumental_Rock = 47
            Ethnic = 48
            Gothic = 49
            Darkwave = 50
            TechnoIndustrial = 51
            Electronic = 52
            PopFolk = 53
            Eurodance = 54
            Dream = 55
            Southern_Rock = 56
            Comedy = 57
            Cult = 58
            Gangsta = 59
            Top_40 = 60
            Christian_Rap = 61
            PopFunk = 62
            Jungle = 63
            Native_US = 64
            Cabaret = 65
            New_Wave = 66
            Psychadelic = 67
            Rave = 68
            Showtunes = 69
            Trailer = 70
            LoFi = 71
            Tribal = 72
            Acid_Punk = 73
            Acid_Jazz = 74
            Polka = 75
            Retro = 76
            Musical = 77
            RockRoll = 78
            Hard_Rock = 79
            Folk = 80
            FolkRock = 81
            National_Folk = 82
            Swing = 83
            Fast_Fusion = 84
            Bebob = 85
            Latin = 86
            Revival = 87
            Celtic = 88
            Bluegrass = 89
            Avantgarde = 90
            Gothic_Rock = 91
            Progressive_Rock = 92
            Psychedelic_Rock = 93
            Symphonic_Rock = 94
            Slow_Rock = 95
            Big_Band = 96
            Chorus = 97
            Easy_Listening = 98
            Acoustic = 99
            Humour = 100
            Speech = 101
            Chanson = 102
            Opera = 103
            Chamber_Music = 104
            Sonata = 105
            Symphony = 106
            Booty_Bass = 107
            Primus = 108
            Porn_Groove = 109
            Satire = 110
            Slow_Jam = 111
            Club = 112
            Tango = 113
            Samba = 114
            Folklore = 115
            Ballad = 116
            Power_Ballad = 117
            Rhythmic_Soul = 118
            Freestyle = 119
            Duet = 120
            Punk_Rock = 121
            Drum_Solo = 122
            Acapella = 123
            EuroHouse = 124
            Dance_Hall = 125
            Goa = 126
            DrumBass = 127
            ClubHouse = 128
            Hardcore = 129
            Terror = 130
            Indie = 131
            BritPop = 132
            Negerpunk = 133
            Polsk_Punk = 134
            Beat = 135
            Christian_Gangsta_Rap = 136
            Heavy_Metal = 137
            Black_Metal = 138
            Crossover = 139
            Contemporary_Christian = 140
            Christian_Rock = 141
            Merengue = 142
            Salsa = 143
            Thrash_Metal = 144
            Anime = 145
            JPop = 146
            Synthpop = 147
        End Enum

#End Region

#Region "Properties - Swig Wrapper"

        ''' <summary> Main Pointer to the Comments Block. </summary>
        ''' <value> Main Pointer to the Comments Block. </value>
        <XmlIgnore()> _
        <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        Public ReadOnly Property CommentsPtr As IntPtr
            Get
                Return _CommentsPtr
            End Get
        End Property
        <XmlIgnore()> _
        <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        Protected Property _CommentsPtr As IntPtr

        ''' <summary> List of Pointers within the Comments Block. </summary>
        ''' <value> List of Pointers within the Comments Block. </value>
        <XmlIgnore()> _
        <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        Public ReadOnly Property PtrList As ReadOnlyCollection(Of IntPtr)
            Get
                Return _PtrList.AsReadOnly
            End Get
        End Property
        <XmlIgnore()> _
        <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        Protected Property _PtrList As List(Of IntPtr)

        ''' <summary> List of Strings within the Comments Block. </summary>
        ''' <value> List of Strings within the Comments Block. </value>
        <XmlIgnore()> _
        Public ReadOnly Property StrList As ReadOnlyCollection(Of String)
            Get
                Return _StrList.AsReadOnly
            End Get
        End Property
        <XmlIgnore()> _
        <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        Protected Property _StrList As List(Of String)

        ''' <summary> If the Memory Allocation is Open. </summary>
        ''' <value> If the Memory Allocation is Open. </value>
        <XmlIgnore()> _
        Public ReadOnly Property IsOpen As Boolean
            Get
                Return _IsOpen
            End Get
        End Property
        <XmlIgnore()> _
        Protected Property _IsOpen As Boolean = False

#End Region

#Region "Properties"

        ''' <summary> ID3 Title Tag. </summary>
        ''' <value> The title. </value>
        Public Property Title As String

        ''' <summary> ID3 Artist Tag. </summary>
        ''' <value> The artist. </value>
        Public Property Artist As String

        ''' <summary> ID3 Album Tag. </summary>
        ''' <value> The album. </value>
        Public Property Album As String

        ''' <summary> ID3 TrackNumber Tag. </summary>
        ''' <value> The track number. </value>
        Public Property TrackNumber As String

        ''' <summary> ID3 Year Tag. </summary>
        ''' <value> The year. </value>
        Public Property Year As String

        ''' <summary> ID3 Comment Tag. </summary>
        ''' <value> The comment. </value>
        Public Property Comment As String

        ''' <summary> ID3 Genre Tag. </summary>
        ''' <value> The genre. </value>
        Public Property Genre As Genres = Genres.Unknown

        ''' <summary> ID3 Discnumber Tag. </summary>
        ''' <value> The discnumber. </value>
        Public Property Discnumber As String

        ''' <summary> C Array used for Storing the Comments. </summary>
        ''' <value> The c array storage. </value>
        <XmlIgnore()> _
        Protected Property CArray As CArray(Of String)

#End Region

#Region "Constructors"

        ''' <summary> Default Constructor. </summary>
        Public Sub New()
        End Sub

        ''' <summary> Default Constructor. </summary>
        ''' <param name="cmmts"> The cmmts c structure to use. </param>
        Public Sub New(cmmts As IntPtr)
            _CommentsPtr = cmmts
            ReadComments()
        End Sub

        ''' <summary> Default Constructor. </summary>
        ''' <param name="cmmts"> The cmmts unmanaged class to use. </param>
        Public Sub New(cmmts As SWIGTYPE_p_p_char)
            If cmmts IsNot Nothing Then _CommentsPtr = cmmts.GetswigCPtr
            ReadComments()
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
            Close()
        End Sub

#End Region

#Region "Methodss - Read"

        ''' <summary> Read in the Comments from the Block Pointer. </summary>
        Public Sub ReadComments()
            ExtractPtrs()
            ExtractStrings()
        End Sub

        ''' <summary> Extract Pointers from the Comment Block. </summary>
        Protected Sub ExtractPtrs()
            If _CommentsPtr = IntPtr.Zero Then Exit Sub
            _PtrList = New List(Of IntPtr)
            Dim loop1 As Boolean = True
            Dim count1 As Long = 0
            While loop1 = True
                Dim listptr As IntPtr
                Dim cmtptr As IntPtr = New IntPtr(_CommentsPtr.ToInt64 + count1)
                listptr = CHelper.Umg_PtrToStructure(Of IntPtr)(cmtptr)
                If listptr <> IntPtr.Zero Then _PtrList.Add(listptr)
                If listptr = IntPtr.Zero Then loop1 = False
                count1 = count1 + IntPtr.Size
            End While
        End Sub

        ''' <summary> Extract Strings from the Comment Block. </summary>
        Protected Sub ExtractStrings()
            If _PtrList Is Nothing Then Exit Sub
            _StrList = New List(Of String)
            For Each item As IntPtr In _PtrList
                Dim tmpstr As String = CHelper.Umg_PtrToString(item)
                _StrList.Add(tmpstr)
            Next
            ' Pull out the string data
            For Each item As String In _StrList
                If String.IsNullOrEmpty(item) Then Continue For
                Dim tmparr As String() = item.Split(CChar("="))
                If tmparr.Count < 2 Then Continue For
                Dim header As String = tmparr(0)
                Dim content As String = tmparr(1)

                ' Note for some reason the reading of Comment doesn't seem to work
                ' Although the Comment field can be set via libsox
                Select Case LCase(header)
                    Case "title"
                        _Title = content
                    Case "artist"
                        _Artist = content
                    Case "album"
                        _Album = content
                    Case "tracknumber"
                        _TrackNumber = content
                    Case "year"
                        _Year = content
                    Case "comment"
                        _Comment = content
                    Case "genre"
                        Genre = Genre_FromString(content)
                    Case "discnumber"
                        _Discnumber = content
                End Select
            Next
        End Sub

#End Region

#Region "Methods - Write"

        ''' <summary> Open the UnManaged Memory / Copy Across the Values. </summary>
        Public Sub Open()
            Dim taglist As List(Of String) = BuildPropList()
            CArray = New CArray(Of String)
            For Each item As String In taglist
                CArray.Add(item)
            Next
            CArray.Add(IntPtr.Zero) ' Zero Terminated Array
            CArray.AllocateAndCopy()
            _CommentsPtr = CArray.Pointer
            _IsOpen = True
        End Sub

        ''' <summary> Close the UnManaged Memory. </summary>
        Public Sub Close()
            If _IsOpen = False Then Exit Sub
            CArray.DeAllocate()
            _IsOpen = False
        End Sub

        ''' <summary> Build a String List based on the available properties. </summary>
        ''' <returns> A List(Of String) </returns>
        Protected Function BuildPropList() As List(Of String)
            Dim ret As New List(Of String)
            If _Title IsNot Nothing Then ret.Add("Title=" & _Title)
            If _Artist IsNot Nothing Then ret.Add("Artist=" & _Artist)
            If _Album IsNot Nothing Then ret.Add("Album=" & _Album)
            If _TrackNumber IsNot Nothing Then ret.Add("Tracknumber=" & _TrackNumber)
            If _Year IsNot Nothing Then ret.Add("Year=" & _Year)
            If _Comment IsNot Nothing Then ret.Add("Comment=" & _Comment)
            If Genre <> Genres.Unknown Then ret.Add("Genre=" & Genre_ToString(Genre))
            If _Discnumber IsNot Nothing Then ret.Add("Discnumber=" & _Discnumber)
            Return ret
        End Function

#End Region

#Region "Methods - Genre"

        ''' <summary> Convert a Generes Enum to a string suitable for a MP3 ID3 Tag. </summary>
        ''' <param name="inp"> The input enum to convert. </param>
        ''' <returns> A String. </returns>
        Public Shared Function Genre_ToString(inp As Genres) As String
            Dim ret As String = inp.ToString
            ret = ret.Replace("_", " ")
            ' Special Cases
            Select Case ret
                Case "HipHop"
                    ret = "Hip-Hop"
                Case "RAndB"
                    ret = "R&B"
                Case "EuroTechno"
                    ret = "Euro-Techno"
                Case "TripHop"
                    ret = "Trip-Hop"
                Case "JazzFunk"
                    ret = "Jazz+Funk"
                Case "TechnoIndustrial"
                    ret = "Techno-Industrial"
                Case "PopFolk"
                    ret = "Pop-Folk"
                Case "PopFunk"
                    ret = "Pop/Funk"
                Case "LoFi"
                    ret = "Lo-Fi"
                Case "RockRoll"
                    ret = "Rock & Roll"
                Case "FolkRock"
                    ret = "Folk-Rock"
                Case "EuroHouse"
                    ret = "Euro-House"
                Case "DrumBass"
                    ret = "Drum & Bass"
                Case "ClubHouse"
                    ret = "Club - House"
            End Select
            Return ret
        End Function

        ''' <summary> Convert a String From a MP3 ID3 Tag to a Generes Enum. </summary>
        ''' <param name="inp"> The input string to analyse. </param>
        ''' <returns> A Genres enum representation. </returns>
        Public Shared Function Genre_FromString(inp As String) As Genres
            ' Special Cases
            Select Case inp
                Case "Hip-Hop"
                    inp = "HipHop"
                Case "R&B"
                    inp = "RAndB"
                Case "Euro-Techno"
                    inp = "EuroTechno"
                Case "Trip-Hop"
                    inp = "TripHop"
                Case "Jazz+Funk"
                    inp = "JazzFunk"
                Case "Techno-Industrial"
                    inp = "TechnoIndustrial"
                Case "Pop-Folk"
                    inp = "PopFolk"
                Case "Pop/Funk"
                    inp = "PopFunk"
                Case "Lo-Fi"
                    inp = "LoFi"
                Case "Rock & Roll"
                    inp = "RockRoll"
                Case "Folk-Rock"
                    inp = "FolkRock"
                Case "Euro-House"
                    inp = "EuroHouse"
                Case "Drum & Bass"
                    inp = "DrumBass"
                Case "Club - House"
                    inp = "ClubHouse"
            End Select
            inp = inp.Replace(" ", "_")
            Dim ret As Genres = Genres.Unknown
            [Enum].TryParse(Of Genres)(inp, ret)
            Return ret
        End Function

#End Region

#Region "Methods - Clone"

        ''' <summary> Clone this object. </summary>
        ''' <returns> A copy of this object. </returns>
        Public Function Clone() As ID3TagGroup
            Dim ret As New ID3TagGroup
            ret.Title = Title
            ret.Artist = Artist
            ret.Album = Album
            ret.TrackNumber = TrackNumber
            ret.Year = Year
            ret.Comment = Comment
            ret.Genre = Genre
            ret.Discnumber = Discnumber
            Return ret
        End Function

#End Region

    End Class

End Namespace
