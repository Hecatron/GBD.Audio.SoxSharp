Namespace DataTypes

    ''' <summary> Exception class for Sox based errors. </summary>
    Public Class SoxException
        Inherits ApplicationException

#Region "Constructors"

        ''' <summary> Default Constructor. </summary>
        ''' <param name="message"> Exception message description. </param>
        Public Sub New(message As String)
            MyBase.New(message)
        End Sub

        ''' <summary> Default Constructor. </summary>
        ''' <param name="message"> Exception message description. </param>
        ''' <param name="inner">   The inner exception. </param>
        Public Sub New(message As String, inner As Exception)
            MyBase.New(message, inner)
        End Sub

#End Region

    End Class

End Namespace
