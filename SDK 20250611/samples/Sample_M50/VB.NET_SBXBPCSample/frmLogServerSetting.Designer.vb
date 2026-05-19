<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmLogServerSetting
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
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

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.cmbLogServerMode = New System.Windows.Forms.ComboBox()
        Me.label1 = New System.Windows.Forms.Label()
        Me.label13 = New System.Windows.Forms.Label()
        Me.label12 = New System.Windows.Forms.Label()
        Me.btnGetDnsSettings = New System.Windows.Forms.Button()
        Me.btnSetDnsSettings = New System.Windows.Forms.Button()
        Me.txtServerDomainName = New System.Windows.Forms.TextBox()
        Me.textBgServerPort = New System.Windows.Forms.TextBox()
        Me.SuspendLayout()
        '
        'cmbLogServerMode
        '
        Me.cmbLogServerMode.FormattingEnabled = True
        Me.cmbLogServerMode.Items.AddRange(New Object() {"None", "Network", "RS485"})
        Me.cmbLogServerMode.Location = New System.Drawing.Point(225, 109)
        Me.cmbLogServerMode.Name = "cmbLogServerMode"
        Me.cmbLogServerMode.Size = New System.Drawing.Size(237, 24)
        Me.cmbLogServerMode.TabIndex = 48
        '
        'label1
        '
        Me.label1.AutoSize = True
        Me.label1.Location = New System.Drawing.Point(39, 113)
        Me.label1.Name = "label1"
        Me.label1.Size = New System.Drawing.Size(100, 16)
        Me.label1.TabIndex = 41
        Me.label1.Text = "Send event via:"
        '
        'label13
        '
        Me.label13.AutoSize = True
        Me.label13.Location = New System.Drawing.Point(39, 77)
        Me.label13.Name = "label13"
        Me.label13.Size = New System.Drawing.Size(77, 16)
        Me.label13.TabIndex = 42
        Me.label13.Text = "Server port:"
        '
        'label12
        '
        Me.label12.AutoSize = True
        Me.label12.Location = New System.Drawing.Point(39, 39)
        Me.label12.Name = "label12"
        Me.label12.Size = New System.Drawing.Size(166, 16)
        Me.label12.TabIndex = 43
        Me.label12.Text = "Server domain name or IP:"
        '
        'btnGetDnsSettings
        '
        Me.btnGetDnsSettings.Location = New System.Drawing.Point(143, 151)
        Me.btnGetDnsSettings.Name = "btnGetDnsSettings"
        Me.btnGetDnsSettings.Size = New System.Drawing.Size(81, 33)
        Me.btnGetDnsSettings.TabIndex = 44
        Me.btnGetDnsSettings.Text = "Get"
        Me.btnGetDnsSettings.UseVisualStyleBackColor = True
        '
        'btnSetDnsSettings
        '
        Me.btnSetDnsSettings.Location = New System.Drawing.Point(281, 151)
        Me.btnSetDnsSettings.Name = "btnSetDnsSettings"
        Me.btnSetDnsSettings.Size = New System.Drawing.Size(81, 33)
        Me.btnSetDnsSettings.TabIndex = 45
        Me.btnSetDnsSettings.Text = "Set"
        Me.btnSetDnsSettings.UseVisualStyleBackColor = True
        '
        'txtServerDomainName
        '
        Me.txtServerDomainName.AcceptsReturn = True
        Me.txtServerDomainName.BackColor = System.Drawing.SystemColors.Window
        Me.txtServerDomainName.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtServerDomainName.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtServerDomainName.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtServerDomainName.Location = New System.Drawing.Point(225, 32)
        Me.txtServerDomainName.MaxLength = 0
        Me.txtServerDomainName.Name = "txtServerDomainName"
        Me.txtServerDomainName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtServerDomainName.Size = New System.Drawing.Size(237, 26)
        Me.txtServerDomainName.TabIndex = 46
        Me.txtServerDomainName.Text = "logserver.test.domain"
        '
        'textBgServerPort
        '
        Me.textBgServerPort.AcceptsReturn = True
        Me.textBgServerPort.BackColor = System.Drawing.SystemColors.Window
        Me.textBgServerPort.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.textBgServerPort.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.textBgServerPort.ForeColor = System.Drawing.SystemColors.WindowText
        Me.textBgServerPort.Location = New System.Drawing.Point(225, 70)
        Me.textBgServerPort.MaxLength = 0
        Me.textBgServerPort.Name = "textBgServerPort"
        Me.textBgServerPort.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.textBgServerPort.Size = New System.Drawing.Size(237, 26)
        Me.textBgServerPort.TabIndex = 47
        Me.textBgServerPort.Text = "5005"
        '
        'frmLogServerSetting
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(525, 207)
        Me.Controls.Add(Me.cmbLogServerMode)
        Me.Controls.Add(Me.label1)
        Me.Controls.Add(Me.label13)
        Me.Controls.Add(Me.label12)
        Me.Controls.Add(Me.btnGetDnsSettings)
        Me.Controls.Add(Me.btnSetDnsSettings)
        Me.Controls.Add(Me.txtServerDomainName)
        Me.Controls.Add(Me.textBgServerPort)
        Me.Name = "frmLogServerSetting"
        Me.Text = "frmLogServerSetting"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Private WithEvents cmbLogServerMode As ComboBox
    Private WithEvents label1 As Label
    Private WithEvents label13 As Label
    Private WithEvents label12 As Label
    Private WithEvents btnGetDnsSettings As Button
    Private WithEvents btnSetDnsSettings As Button
    Public WithEvents txtServerDomainName As TextBox
    Public WithEvents textBgServerPort As TextBox
End Class
