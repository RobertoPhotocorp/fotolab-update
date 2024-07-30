Imports System.Xml
Imports System.IO
Imports System.Xml.Linq
Imports System.Data.OleDb
Imports System.Data
Imports System
Imports System.ComponentModel
Imports System.Collections
Imports MySql.Data.MySqlClient
Public Class BdConnectList

    Public RutaBd As String
    Public Connect As New OleDbConnection
    Public SqlQuery As String
    Public ComandSql As OleDbCommand

End Class
