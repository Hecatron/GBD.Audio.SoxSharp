Imports GBD.Audio.SoxSharp.DataTypes
Imports GBD.Audio.SoxSharp.swig

Namespace Effect

    ''' <summary> Effect Global Settings. </summary>
    Public Class EffectGlobals
        Implements IDisposable

#Region "Properties - Local Storage"

        ''' <summary> Underlying Swig Storage. </summary>
        ''' <value> Underlying Swig Storage. </value>
        Public ReadOnly Property SwigStorage As sox_effects_globals_t
            Get
                Return _SwigStorage
            End Get
        End Property
        <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        Protected Property _SwigStorage As sox_effects_globals_t

#End Region

#Region "Properties - Swig Wrapper"

        ''' <summary> To help the user choose effect / options. </summary>
        ''' <value> To help the user choose effect / options. </value>
        Public Property Plot As sox_plot_t
            Get
                Return _SwigStorage.plot
            End Get
            Set(value As sox_plot_t)
                _SwigStorage.plot = value
            End Set
        End Property

        ''' <summary> Pointer to associated Sox globals. </summary>
        ''' <value> Pointer to associated Sox globals. </value>
        Public ReadOnly Property GlobalInfo As GlobalInfo
            Get
                Return _GlobalInfo
            End Get
        End Property
        <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        Protected Property _GlobalInfo As GlobalInfo

#End Region

#Region "Constructors"

        ''' <summary> Default Constructor. </summary>
        Public Sub New()
            _SwigStorage = New sox_effects_globals_t
            _GlobalInfo = New GlobalInfo(_SwigStorage.global_info)
        End Sub

        ''' <summary> Default Constructor. </summary>
        ''' <param name="storage"> Use this as the swig storage value. </param>
        Public Sub New(storage As sox_effects_globals_t)
            _SwigStorage = storage
            _GlobalInfo = New GlobalInfo(_SwigStorage.global_info)
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

        ''' <summary> Get Global Parameters for Effects. </summary>
        ''' <returns> Global Parameters. </returns>
        Public Shared Function GetGlobals() As EffectGlobals
            Dim tmpitem As sox_effects_globals_t = libsox.sox_get_effects_globals()
            Dim newitem As New EffectGlobals(tmpitem)
            Return newitem
        End Function

#End Region

    End Class

End Namespace
