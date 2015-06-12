Imports System.Xml.Serialization
Imports GBD.Audio.SoxSharp.swig

Namespace DataTypes

    ''' <summary> Out of Bound Data, comments, instrument info, loop info. </summary>
    Public Class OutOfBound
        Implements IDisposable

#Region "Properties - Local Storage"

        ''' <summary> Underlying Swig Storage. </summary>
        ''' <value> Underlying Swig Storage. </value>
        Public ReadOnly Property SwigStorage As sox_oob_t
            Get
                Return _SwigStorage
            End Get
        End Property
        <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        Protected Property _SwigStorage As sox_oob_t

#End Region

#Region "Properties - Swig Wrapper"

        ''' <summary> Pointer to the Comments Information. </summary>
        ''' <value> Pointer to the Comments Information. </value>
        <XmlIgnore()> _
        Public Property CommentsPtr As SWIGTYPE_p_p_char
            Get
                Return _SwigStorage.comments
            End Get
            Set(value As SWIGTYPE_p_p_char)
                _SwigStorage.comments = value
            End Set
        End Property

        ''' <summary> Group of ID3 Tags based on the Comments Pointer. </summary>
        ''' <value> Group of ID3 Tags based on the Comments Pointer. </value>
        Public Property ID3TagGroup As ID3TagGroup

        ''' <summary> Instrument Information. </summary>
        ''' <value> Instrument Information. </value>
        Public ReadOnly Property InstrumentInfo As InstrumentInfo
            Get
                Return _InstrumentInfo
            End Get
        End Property
        <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        Protected Property _InstrumentInfo As InstrumentInfo

        ''' <summary> Looping Information. </summary>
        ''' <value> Looping Information. </value>
        Public ReadOnly Property LoopingInfo As LoopingInfo
            Get
                Return _LoopingInfo
            End Get
        End Property
        <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        Protected Property _LoopingInfo As LoopingInfo

#End Region

#Region "Constructors"

        ''' <summary> Default Constructor. </summary>
        Public Sub New()
            _SwigStorage = New sox_oob_t
            _InstrumentInfo = New InstrumentInfo(_SwigStorage.instr)
            _LoopingInfo = New LoopingInfo(_SwigStorage.loops)
            _ID3TagGroup = New ID3TagGroup
        End Sub

        ''' <summary> Default Constructor. </summary>
        ''' <param name="storage"> Use this as the swig storage value. </param>
        Public Sub New(storage As sox_oob_t)
            _SwigStorage = storage
            _InstrumentInfo = New InstrumentInfo(_SwigStorage.instr)
            _LoopingInfo = New LoopingInfo(_SwigStorage.loops)
            _ID3TagGroup = New ID3TagGroup(_SwigStorage.comments)
        End Sub

#End Region

#Region "Destructors"

        ''' <summary> Destructor. </summary>
        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            CloseComments()
            Dispose()
        End Sub

        ''' <summary> Dispose of Memory Storage. </summary>
        Public Sub Dispose() Implements IDisposable.Dispose
            If _SwigStorage IsNot Nothing Then _SwigStorage.Dispose()
        End Sub

#End Region

#Region "Methods"

        ''' <summary> Open the Comments Structure. </summary>
        Public Sub OpenComments()
            _ID3TagGroup.Open()
            Dim tmpptr As New SWIGTYPE_p_p_char(_ID3TagGroup.CommentsPtr)
            CommentsPtr = tmpptr
        End Sub

        ''' <summary> Close the Comments Structure. </summary>
        Public Sub CloseComments()
            _ID3TagGroup.Close()
        End Sub

#End Region

    End Class

End Namespace
