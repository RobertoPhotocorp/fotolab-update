<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form reemplaza a Dispose para limpiar la lista de componentes.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Requerido por el Diseñador de Windows Forms
    Private components As System.ComponentModel.IContainer

    'NOTA: el Diseñador de Windows Forms necesita el siguiente procedimiento
    'Se puede modificar usando el Diseñador de Windows Forms.  
    'No lo modifique con el editor de código.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.Estadoproceso = New System.Windows.Forms.ProgressBar()
        Me.Estado = New System.Windows.Forms.Label()
        Me.procesados = New System.Windows.Forms.Label()
        Me.Lbversion = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'Estadoproceso
        '
        Me.Estadoproceso.Location = New System.Drawing.Point(12, 230)
        Me.Estadoproceso.Name = "Estadoproceso"
        Me.Estadoproceso.Size = New System.Drawing.Size(596, 60)
        Me.Estadoproceso.TabIndex = 4
        '
        'Estado
        '
        Me.Estado.AutoSize = True
        Me.Estado.Location = New System.Drawing.Point(12, 196)
        Me.Estado.Name = "Estado"
        Me.Estado.Size = New System.Drawing.Size(40, 13)
        Me.Estado.TabIndex = 5
        Me.Estado.Text = "Estado"
        '
        'procesados
        '
        Me.procesados.AutoSize = True
        Me.procesados.Location = New System.Drawing.Point(196, 196)
        Me.procesados.Name = "procesados"
        Me.procesados.Size = New System.Drawing.Size(63, 13)
        Me.procesados.TabIndex = 6
        Me.procesados.Text = "Procesados"
        '
        'Lbversion
        '
        Me.Lbversion.AutoSize = True
        Me.Lbversion.Location = New System.Drawing.Point(526, 9)
        Me.Lbversion.Name = "Lbversion"
        Me.Lbversion.Size = New System.Drawing.Size(41, 13)
        Me.Lbversion.TabIndex = 7
        Me.Lbversion.Text = "version"
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(620, 300)
        Me.Controls.Add(Me.Lbversion)
        Me.Controls.Add(Me.procesados)
        Me.Controls.Add(Me.Estado)
        Me.Controls.Add(Me.Estadoproceso)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "Form1"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Form1"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Estadoproceso As ProgressBar
    Friend WithEvents Estado As Label
    Friend WithEvents procesados As Label
    Friend WithEvents Lbversion As Label
End Class
