Imports System.Xml
Imports System.IO
Imports System.Xml.Linq
Imports System.Data.OleDb
Imports System.Data
Imports System
Imports System.ComponentModel
Imports System.Collections
Imports MySql.Data.MySqlClient



Public Class Form1
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim MysqlCommand As New MySqlCommand
        Dim MysqlConnString As String = "server=88.98.99.6; user id=fotolab ; password=Fotolabx3; port=3306; DataBase=Db_remote_printspot_sender"
        Dim MysqlConexion As MySqlConnection = New MySqlConnection(MysqlConnString)
        Try
            MysqlConexion.Open()
            MsgBox("la conexión fue exitosa")
            MysqlConexion.Close()
        Catch ex As Exception
            MsgBox("La conexión no fue exitosa")
        End Try
    End Sub



    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim StrVar As String
        Dim rd As MySqlDataReader
        Dim data As DataTable = New DataTable
        Dim dataacess As DataTable = New DataTable
        Dim dataprocess As DataTable = New DataTable
        Dim row As DataRowView
        Dim cmd As New MySqlCommand
        Dim MysqlConnString As String = "server=88.98.99.6; user id=fotolab ; password=Fotolabx3; port=3306; DataBase=Db_remote_printspot_sender"
        Dim MysqlConexion As MySqlConnection = New MySqlConnection(MysqlConnString)
        MysqlConexion.Open()
        MsgBox("la conexión fue exitosa")

        cmd.CommandText = "Select * from Producto"
        cmd.Connection = MysqlConexion
        rd = cmd.ExecuteReader
        data.Load(rd)
        dataprocess = data.Clone
        'For a = 0 To data.Rows.Count - 1
        '    row = data.DefaultView.Item(a)
        '    RichTextBox1.Text = RichTextBox1.Text & row.Row(1).ToString & vbCrLf

        'Next

        'While rd.Read()


        '    '    RichTextBox1.Text = RichTextBox1.Text & rd.GetString(1)


        'End While
        dataacess = LeerDatosAccesproducto()
        '''' dataacess = LeerDatosAccesfuji()
        For b = 0 To data.Rows.Count - 1
            Dim flagdata As Integer = 0
            For a = 0 To dataacess.Rows.Count - 1
                Dim rowmysql As DataRowView
                Dim rowaccess As DataRowView

                rowmysql = data.DefaultView.Item(b)
                rowaccess = dataacess.DefaultView.Item(a)
                If rowmysql.Row(0).ToString = rowaccess.Row(0).ToString Then
                    flagdata = 1
                    Exit For
                    'RichTextBox1.Text = RichTextBox1.Text & rowaccess.Row(1).ToString & vbCrLf
                End If
            Next
            If flagdata = 0 Then
                Dim rowprocess As DataRowView
                rowprocess = data.DefaultView.Item(b)

                dataprocess.ImportRow(rowprocess.Row)
            End If
            flagdata = 0
        Next
        For a = 0 To dataprocess.Rows.Count - 1

            Dim rowaccess As DataRowView

            rowaccess = dataprocess.DefaultView.Item(a)
            RichTextBox1.Text = RichTextBox1.Text & rowaccess.Row(1).ToString & vbCrLf
            '''''LoadMysqlproducto(rowaccess.Item(0), rowaccess.Item(1))
            ''LoadMysqlfuji(rowaccess.Item(0), rowaccess.Item(1))
        Next

        rd.Close()
    End Sub
    Private Function LeerDatosAccesproducto() As DataTable
        Dim BdDataadapter As OleDbDataAdapter
        Dim datasetped As DataTable = New DataTable
        Dim ConnectList As New BdConnectList
        Dim row As DataRowView
        ConnectList.SqlQuery = "SELECT * FROM producto '; "
        ConnectList.Connect.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\config\bd_config.accdb"
        ConnectList.Connect.Open()
        ConnectList.ComandSql = New System.Data.OleDb.OleDbCommand(ConnectList.SqlQuery, ConnectList.Connect)
        BdDataadapter = New OleDbDataAdapter(ConnectList.SqlQuery, ConnectList.Connect)

        BdDataadapter.Fill(datasetped)
        For a = 0 To datasetped.Rows.Count - 1
            row = datasetped.DefaultView.Item(a)
            'RichTextBox1.Text = RichTextBox1.Text & row.Row(1).ToString & vbCrLf

        Next
        ConnectList.ComandSql.ExecuteNonQuery()
        ConnectList.Connect.Close()
        Return datasetped
    End Function
    Private Function LeerDatosAccesfuji() As DataTable
        Dim BdDataadapter As OleDbDataAdapter
        Dim datasetped As DataTable = New DataTable
        Dim ConnectList As New BdConnectList
        Dim row As DataRowView
        ConnectList.SqlQuery = "SELECT * FROM Fuji '; "
        ConnectList.Connect.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\config\bd_config.accdb"
        ConnectList.Connect.Open()
        ConnectList.ComandSql = New System.Data.OleDb.OleDbCommand(ConnectList.SqlQuery, ConnectList.Connect)
        BdDataadapter = New OleDbDataAdapter(ConnectList.SqlQuery, ConnectList.Connect)

        BdDataadapter.Fill(datasetped)
        For a = 0 To datasetped.Rows.Count - 1
            row = datasetped.DefaultView.Item(a)
            'RichTextBox1.Text = RichTextBox1.Text & row.Row(1).ToString & vbCrLf

        Next
        ConnectList.ComandSql.ExecuteNonQuery()
        ConnectList.Connect.Close()
        Return datasetped
    End Function
    Public Sub LoadMysqlproducto(ByVal producto As String, ByVal nombre As String)


        Dim sql1 As String = "INSERT INTO Producto (Canal,Nom_Prod,Id_tipo) VALUES('" & producto.ToString & "','" & nombre.ToString & "','0')"

        Dim count As Integer


        Dim cmd As New MySqlCommand
        Dim MysqlConnString As String = "server=88.98.99.6; user id=fotolab ; password=Fotolabx3; port=3306; DataBase=Db_remote_printspot_sender"
        Dim MysqlConexion As MySqlConnection = New MySqlConnection(MysqlConnString)
        MysqlConexion.Open()
        'MsgBox("la conexión fue exitosa")

        cmd.CommandText = sql1
        cmd.Connection = MysqlConexion
        'cmd.Parameters.Add("@Cod_pro", MySqlDbType.VarChar).Value = producto
        'cmd.Parameters.Add("@Nom_Prod", MySqlDbType.VarChar).Value = nombre
        'cmd.Parameters.Add("@Id_tipo", MySqlDbType.VarChar).Value = "0"
        cmd.ExecuteNonQuery()
        MysqlConexion.Close()


    End Sub
    Public Sub LoadMysqlfuji(ByVal canal As String, ByVal nombre As String)


        Dim sql1 As String = "INSERT INTO Fuji (Canal,Nom_machine) VALUES('" & canal.ToString & "','" & nombre.ToString & "')"

        Dim count As Integer


        Dim cmd As New MySqlCommand
        Dim MysqlConnString As String = "server=88.98.99.6; user id=fotolab ; password=Fotolabx3; port=3306; DataBase=Db_remote_printspot_sender"
        Dim MysqlConexion As MySqlConnection = New MySqlConnection(MysqlConnString)
        MysqlConexion.Open()
        'MsgBox("la conexión fue exitosa")

        cmd.CommandText = sql1
        cmd.Connection = MysqlConexion
        'cmd.Parameters.Add("@Cod_pro", MySqlDbType.VarChar).Value = producto
        'cmd.Parameters.Add("@Nom_Prod", MySqlDbType.VarChar).Value = nombre
        'cmd.Parameters.Add("@Id_tipo", MySqlDbType.VarChar).Value = "0"
        cmd.ExecuteNonQuery()
        MysqlConexion.Close()


    End Sub
End Class
