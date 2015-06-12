Imports GBD.Audio.SoxSharp.swig
Imports GBD.Audio.SoxSharp.UnmanagedHelper

Namespace DataTypes

    ''' <summary> Signal Information within the Audio. </summary>
    Public Class SignalInfo
        Implements IDisposable

#Region "Properties - Local Storage"

        ''' <summary> Underlying Swig Storage. </summary>
        ''' <value> Underlying Swig Storage. </value>
        Public ReadOnly Property SwigStorage As sox_signalinfo_t
            Get
                Return _SwigStorage
            End Get
        End Property
        <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        Protected Property _SwigStorage As sox_signalinfo_t

#End Region

#Region "Properties - Swig Wrapper"

        ''' <summary> Samples per second, 0 if unknown. </summary>
        ''' <value> Sample rate. </value>
        Public Property Rate As Double
            Get
                Return _SwigStorage.rate
            End Get
            Set(value As Double)
                _SwigStorage.rate = value
            End Set
        End Property

        ''' <summary> Number of sound channels, 0 if unknown. </summary>
        ''' <value> Number of sound channels. </value>
        Public Property Channels As UInteger
            Get
                Return _SwigStorage.channels
            End Get
            Set(value As UInteger)
                _SwigStorage.channels = value
            End Set
        End Property

        ''' <summary> Bits per Sample, 0 if unknown. </summary>
        ''' <value> The precision. </value>
        Public Property Precision As UInteger
            Get
                Return _SwigStorage.precision
            End Get
            Set(value As UInteger)
                _SwigStorage.precision = value
            End Set
        End Property

        ''' <summary> Samples * Channels in file, 0 if unknown, -1 if unspecified. </summary>
        ''' <value> The total length. </value>
        Public Property TotalLength As UInteger
            Get
                Return _SwigStorage.length
            End Get
            Set(value As UInteger)
                _SwigStorage.length = value
            End Set
        End Property

        ''' <summary> Effects headroom multiplier, will be 0 if Null. </summary>
        ''' <value> Effects headroom multiplier. </value>
        Public ReadOnly Property Multiplier As Double
            Get
                Dim ret As Double = 0
                ' Defreference mult if it exists
                If _SwigStorage.mult IsNot Nothing Then
                    Dim pointer As IntPtr = _SwigStorage.mult.GetswigCPtr
                    ret = CHelper.Umg_PtrToStructure(Of Double)(pointer)
                End If
                Return ret
            End Get
        End Property

#End Region

#Region "Constructors"

        ''' <summary> Default Constructor. </summary>
        Public Sub New()
            _SwigStorage = New sox_signalinfo_t
        End Sub

        ''' <summary> Default Constructor. </summary>
        ''' <param name="storage"> Use this as the swig storage value. </param>
        Public Sub New(storage As sox_signalinfo_t)
            _SwigStorage = storage
        End Sub

#End Region

#Region "Methods"

        ''' <summary> Clone the Object. </summary>
        ''' <returns> A copy of this object. </returns>
        Public Function Clone() As SignalInfo
            Dim ret As New SignalInfo
            ret.Rate = Rate
            ret.Channels = Channels
            ret.Precision = Precision
            ret.TotalLength = TotalLength
            Return ret
        End Function

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
