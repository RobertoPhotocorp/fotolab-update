Imports System.Xml
Imports System.IO
Imports System.Xml.Linq
Imports System.Data.OleDb
Imports System.Threading
Imports System.Windows.Threading
Imports System.Data
Imports System
Imports System.ComponentModel
Imports System.Collections
Imports MySql.Data.MySqlClient



Public Class Form1
    Private Sub Button1_Click(sender As Object, e As EventArgs)
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



    Private Sub Form1_load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim hilo1 As New Thread(AddressOf Cargadedatos)

        FileVersionInfo.GetVersionInfo("c:\config\PrintSpotSender V2.exe")
        Dim myFileVersionInfo As FileVersionInfo = FileVersionInfo.GetVersionInfo("c:\config\PrintSpotSender V2.exe")


        ' Print the file name and version number.
        Lbversion.Text = "version: " & myFileVersionInfo.FileVersion.ToString


        hilo1.Start()
    End Sub
    Private Sub Cargadedatos()

        Dim tablaproductos As DataTable
        Dim tablarelacionfuji As DataTable
        Dim tablafuji As DataTable
        Dim rutabd As String
        Dim rowproductos As DataRowView
        Dim rowfuji As DataRowView
        Dim rowRelacionFuji As DataRowView
        Dim MysqlConnString As String = "server=88.98.99.6; user id=fotolab ; password=Fotolabx3; port=3306; DataBase=Db_remote_printspot_sender"
        Dim MysqlConexion As MySqlConnection = New MySqlConnection(MysqlConnString)
        Dim conexionflag As String
        Try
            MysqlConexion.Open()
        Catch
            conexionflag = 1
        End Try
        MysqlConexion.Close()
        CheckForIllegalCrossThreadCalls = False
        rutabd = CallConfig()
        If conexionflag = 0 Then
            If rutabd <> "" Then
                tablaproductos = Compararbdtablaproductos()
                tablafuji = CompararbdtablaFuji()
                tablarelacionfuji = CompararbdtablaRelacionFuji()
                If tablafuji.Rows.Count <> 0 Or tablaproductos.Rows.Count <> 0 Or tablarelacionfuji.Rows.Count <> 0 Then
                    Estadoproceso.Value = Estadoproceso.Value + 10
                    My.Computer.FileSystem.CopyFile(rutabd, "C:\config\bd_" & DateTime.Now.ToString("ddMMyyyyhhmmss") & ".accdb", True)
                    procesados.Text = ""
                    Estado.Text = "Haciendo copia de seguridad de base de datos"
                    System.Threading.Thread.Sleep(3000)
                End If
                Estadoproceso.Value = Estadoproceso.Value + 25
                Estado.Text = "Copiando datos de producto -------->"
                For a = 0 To tablaproductos.Rows.Count - 1
                    rowproductos = tablaproductos.DefaultView.Item(a)
                    procesados.Text = rowproductos.Row(0).ToString & "," & rowproductos.Row(1).ToString
                    Saveaccessproductos(rowproductos.Row(0).ToString, rowproductos.Row(1).ToString, rowproductos.Row(2).ToString, rutabd)

                Next

                Estadoproceso.Value = Estadoproceso.Value + 25
                Estado.Text = "Copiando datos de fuji -------->"
                For a = 0 To tablafuji.Rows.Count - 1
                    rowfuji = tablafuji.DefaultView.Item(a)
                    procesados.Text = rowfuji.Row(0).ToString & "," & rowfuji.Row(1).ToString
                    SaveaccessFuji(rowfuji.Row(0).ToString, rowfuji.Row(1).ToString, rutabd)

                Next

                Estadoproceso.Value = Estadoproceso.Value + 25
                Estado.Text = "Copiando datos de Relacion fuji -------->"
                For a = 0 To tablarelacionfuji.Rows.Count - 1
                    rowRelacionFuji = tablarelacionfuji.DefaultView.Item(a)
                    procesados.Text = rowRelacionFuji.Row(0).ToString & "," & rowRelacionFuji.Row(1).ToString
                    Saveaccessrelacionfuji(rowRelacionFuji.Row(0).ToString, rowRelacionFuji.Row(1).ToString, rowRelacionFuji.Row(2).ToString, rutabd)

                Next
                Estado.Text = "Cargado todo correctamente"
                procesados.Text = ""
            End If
            Estadoproceso.Value = 100
        Else
            Estado.Text = "Error al conectar con la base de datos"
        End If
        System.Threading.Thread.Sleep(10000)
        Dim myProcess As Process
        ' myProcess = System.Diagnostics.Process.Start(Application.StartupPath & "\PrintSpotSender V2.exe", "")

        myProcess = System.Diagnostics.Process.Start("c:\config\PrintSpotSender V2.exe", "")
        Me.Close()
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
    Private Function LeerDatosAccesRelacionFuji() As DataTable
        Dim BdDataadapter As OleDbDataAdapter
        Dim datasetped As DataTable = New DataTable
        Dim ConnectList As New BdConnectList
        Dim row As DataRowView
        ConnectList.SqlQuery = "SELECT * FROM RelacionFuji '; "
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
    Public Sub LoadMysqlproducto(ByVal producto As String, ByVal nombre As String, ByVal dsc As String)


        Dim sql1 As String = "INSERT INTO RelacionFuji (Canal,Cod_prod,Dsc) VALUES('" & producto.ToString & "','" & nombre.ToString & "','" & dsc & "')"

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
    Public Function Compararbdtablaproductos() As DataTable
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
        'MsgBox("la conexión fue exitosa")

        cmd.CommandText = "Select * from Producto"
        cmd.Connection = MysqlConexion
        rd = cmd.ExecuteReader
        data.Load(rd)
        dataprocess = data.Clone
        dataacess = LeerDatosAccesproducto()

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
        Next

        rd.Close()

        Return dataprocess
    End Function
    Public Function CompararbdtablaRelacionFuji() As DataTable
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
        'MsgBox("la conexión fue exitosa")

        cmd.CommandText = "Select * from RelacionFuji"
        cmd.Connection = MysqlConexion
        rd = cmd.ExecuteReader
        data.Load(rd)
        dataprocess = data.Clone
        dataacess = LeerDatosAccesRelacionFuji()

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
        Next

        rd.Close()

        Return dataprocess
    End Function
    Public Function CompararbdtablaFuji() As DataTable
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
        'MsgBox("la conexión fue exitosa")

        cmd.CommandText = "Select * from Fuji"
        cmd.Connection = MysqlConexion
        rd = cmd.ExecuteReader
        data.Load(rd)
        dataprocess = data.Clone
        dataacess = LeerDatosAccesfuji()

        For b = 0 To data.Rows.Count - 1
            Dim flagdata As Integer = 0
            For a = 0 To dataacess.Rows.Count - 1
                Dim rowmysql As DataRowView
                Dim rowaccess As DataRowView

                rowmysql = data.DefaultView.Item(b)
                rowaccess = dataacess.DefaultView.Item(a)
                If rowmysql.Row(1).ToString = rowaccess.Row(1).ToString Then
                    flagdata = 1
                    Exit For

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
        Next

        rd.Close()

        Return dataprocess
    End Function
    Public Shared Function CallConfig() As String

        Dim archivo As String = ("C:/config/" & "config.txt")
        Dim index As Integer
        Dim fs As FileStream
        Dim flag As Double = 0
        Dim temp As String = ""
        Dim ped As String = ""
        Dim fuji As String = ""
        Dim bd As String = ""
        Dim tienda As String = ""

        While flag = 0
            If File.Exists(archivo) Then
                Dim leer As New StreamReader("/config/" & "config.txt")
                While leer.Peek <> -1
                    ':::Leemos cada linea del archivo TXT
                    Dim linea As String = leer.ReadLine()
                    If linea.StartsWith("RutaEntrada: ") Then

                        index = linea.LastIndexOf(" ")
                        ped = linea.Substring(index)
                    ElseIf linea.StartsWith("RutaSalida: ") Then

                        index = linea.LastIndexOf(" ")
                        fuji = linea.Substring(index)
                    ElseIf linea.StartsWith("RutaTemporal: ") Then

                        index = linea.LastIndexOf(" ")
                        temp = linea.Substring(index)
                    ElseIf linea.StartsWith("RutaBaseDeDatos: ") Then

                        index = linea.LastIndexOf(" ")
                        bd = linea.Substring(index)
                    ElseIf linea.StartsWith("Tienda: ") Then

                        index = linea.LastIndexOf(" ")
                        tienda = linea.Substring(index)
                    End If

                End While
                flag = 1
                leer.Close()
            Else
                Directory.CreateDirectory("/config/")
                fs = File.Create("/config/config.txt")
                fs.Close()
                Return bd

            End If
        End While
        Return bd
    End Function
    Public Sub Saveaccessproductos(ByVal Cod_prod As String, ByVal Nom_Prod As String, ByVal Id_Tipo As String, ByVal PathBd As String)


        Dim conexion As OleDbConnection
        Dim sql As String = "INSERT INTO producto (Cod_prod,Nom_Prod,Id_Tipo) VALUES(@Cod_prod,@Nom_Prod,@Id_Tipo)"


        conexion = New OleDbConnection()


        conexion.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & PathBd



        conexion.Open()
        Dim command As OleDbCommand = New System.Data.OleDb.OleDbCommand(sql, conexion)

        command.Parameters.Add("@Cod_prod", OleDbType.VarChar).Value = Cod_prod
        command.Parameters.Add("@Nom_Prod", OleDbType.VarChar).Value = Nom_Prod
        command.Parameters.Add("@Id_Tipo", OleDbType.VarChar).Value = Id_Tipo
        command.ExecuteNonQuery()
        conexion.Close()

    End Sub
    Public Sub SaveaccessFuji(ByVal Cod_prod As String, ByVal Nom_Prod As String, ByVal PathBd As String)


        Dim conexion As OleDbConnection
        Dim sql As String = "INSERT INTO fuji (Canal,Nom_Machine,Dsc) VALUES(@Canal,@Nom_Machine,@Dsc)"


        conexion = New OleDbConnection()


        conexion.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & PathBd



        conexion.Open()

        Dim command As OleDbCommand = New System.Data.OleDb.OleDbCommand(sql, conexion)

        command.Parameters.Add("@Canal", OleDbType.VarChar).Value = Cod_prod
        command.Parameters.Add("@Nom_Machine", OleDbType.VarChar).Value = Nom_Prod
        command.Parameters.Add("@Dsc", OleDbType.Boolean).Value = False
        command.ExecuteNonQuery()
        conexion.Close()

    End Sub
    Public Sub Saveaccessrelacionfuji(ByVal Canal As String, ByVal Cod_Prod As String, ByVal dsc As String, ByVal PathBd As String)


        Dim conexion As OleDbConnection
        Dim sql As String = "INSERT INTO RelacionFuji (Canal,Cod_prod,dsc) VALUES(@Canal,@Cod_prod,@dsc)"


        conexion = New OleDbConnection()


        conexion.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & PathBd



        conexion.Open()
        Dim command As OleDbCommand = New System.Data.OleDb.OleDbCommand(sql, conexion)

        command.Parameters.Add("@Canal", OleDbType.VarChar).Value = Canal
        command.Parameters.Add("@Cod_prod", OleDbType.VarChar).Value = Cod_Prod
        command.Parameters.Add("@dsc", OleDbType.VarChar).Value = dsc
        command.ExecuteNonQuery()
        conexion.Close()

    End Sub
End Class
'//////////////////////funcion inicial de recuperacion de datos de las tablas mysql y access
'Dim StrVar As String
'Dim rd As MySqlDataReader
'Dim data As DataTable = New DataTable
'Dim dataacess As DataTable = New DataTable
'Dim dataprocess As DataTable = New DataTable
'Dim row As DataRowView
'Dim cmd As New MySqlCommand
'Dim MysqlConnString As String = "server=88.98.99.6; user id=fotolab ; password=Fotolabx3; port=3306; DataBase=Db_remote_printspot_sender"
'Dim MysqlConexion As MySqlConnection = New MySqlConnection(MysqlConnString)
'MysqlConexion.Open()
'MsgBox("la conexión fue exitosa")

'cmd.CommandText = "Select * from Producto"
'cmd.Connection = MysqlConexion
'rd = cmd.ExecuteReader
'data.Load(rd)
'dataprocess = data.Clone
''For a = 0 To data.Rows.Count - 1
''    row = data.DefaultView.Item(a)
''    RichTextBox1.Text = RichTextBox1.Text & row.Row(1).ToString & vbCrLf

''Next

''While rd.Read()


''    '    RichTextBox1.Text = RichTextBox1.Text & rd.GetString(1)


''End While
'dataacess = LeerDatosAccesproducto()
''''' dataacess = LeerDatosAccesfuji()
'For b = 0 To data.Rows.Count - 1
'Dim flagdata As Integer = 0
'For a = 0 To dataacess.Rows.Count - 1
'Dim rowmysql As DataRowView
'Dim rowaccess As DataRowView

'rowmysql = data.DefaultView.Item(b)
'rowaccess = dataacess.DefaultView.Item(a)
'If rowmysql.Row(0).ToString = rowaccess.Row(0).ToString Then
'flagdata = 1
'Exit For
''RichTextBox1.Text = RichTextBox1.Text & rowaccess.Row(1).ToString & vbCrLf
'End If
'Next
'If flagdata = 0 Then
'Dim rowprocess As DataRowView
'rowprocess = data.DefaultView.Item(b)

'dataprocess.ImportRow(rowprocess.Row)
'End If
'flagdata = 0
'Next
'For a = 0 To dataprocess.Rows.Count - 1

'Dim rowaccess As DataRowView

'rowaccess = dataprocess.DefaultView.Item(a)
'RichTextBox1.Text = RichTextBox1.Text & rowaccess.Row(1).ToString & vbCrLf

'Next

'rd.Close()

'Return data table

'///////introduccion datos en tabla mysql