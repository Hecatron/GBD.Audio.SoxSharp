Imports GBD.Audio.SoxSharp.swig

Namespace DataTypes

    ''' <summary> Looping Information. </summary>
    Public Class LoopingInfo
        Implements IDisposable

#Region "Types"

        ''' <summary> Type of Loop Mode. </summary>
        <Flags()> _
        Public Enum LoopMode_Type As Byte
            None = 0
            Loop_Forward = 1
            Loop_Forward_Back = 2
            Loop_8 = 32
            Sustain_Decay = 64
        End Enum

#End Region

#Region "Properties - Local Storage"

        ''' <summary> Underlying Swig Storage. </summary>
        ''' <value> Underlying Swig Storage. </value>
        Public ReadOnly Property SwigStorage As sox_loopinfo_t
            Get
                Return _SwigStorage
            End Get
        End Property
        <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        Protected Property _SwigStorage As sox_loopinfo_t

#End Region

#Region "Properties - Swig Wrapper"

        ''' <summary> First Sample. </summary>
        ''' <value> First Sample. </value>
        Public Property Start As UInteger
            Get
                Return _SwigStorage.start
            End Get
            Set(value As UInteger)
                _SwigStorage.start = value
            End Set
        End Property

        ''' <summary> Length. </summary>
        ''' <value> The length. </value>
        Public Property Length As UInteger
            Get
                Return _SwigStorage.length
            End Get
            Set(value As UInteger)
                _SwigStorage.length = value
            End Set
        End Property

        ''' <summary> Number of Repeats, 0=forever. </summary>
        ''' <value> The count. </value>
        Public Property Count As UInteger
            Get
                Return _SwigStorage.count
            End Get
            Set(value As UInteger)
                _SwigStorage.count = value
            End Set
        End Property

        ''' <summary> Type of Loop. </summary>
        ''' <value> The type of the loop. </value>
        Public Property LoopType As LoopMode_Type
            Get
                Dim ret As LoopMode_Type
                [Enum].TryParse(_SwigStorage.type.ToString, ret)
                Return ret
            End Get
            Set(value As LoopMode_Type)
                _SwigStorage.type = value
            End Set
        End Property

        ''' <summary> Maximum number of Loops. </summary>
        ''' <value> The loop maximum. </value>
        Public ReadOnly Property LoopMax As Integer
            Get
                Return libsox.SOX_MAX_NLOOPS
            End Get
        End Property

#End Region

#Region "Constructors"

        ''' <summary> Default Constructor. </summary>
        Public Sub New()
            _SwigStorage = New sox_loopinfo_t
        End Sub

        ''' <summary> Default Constructor. </summary>
        ''' <param name="storage"> The storage. </param>
        Public Sub New(storage As sox_loopinfo_t)
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

    End Class

End Namespace
