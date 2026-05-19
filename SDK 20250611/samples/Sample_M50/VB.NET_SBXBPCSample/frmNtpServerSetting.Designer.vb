<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmNtpServerSetting
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
        Me.txtInterval = New System.Windows.Forms.TextBox()
        Me.label1 = New System.Windows.Forms.Label()
        Me.label13 = New System.Windows.Forms.Label()
        Me.label12 = New System.Windows.Forms.Label()
        Me.btnGetNtpServerSettings = New System.Windows.Forms.Button()
        Me.btnSetNtpServerSettings = New System.Windows.Forms.Button()
        Me.txtServerAddress = New System.Windows.Forms.TextBox()
        Me.txtTimezone = New System.Windows.Forms.TextBox()
        Me.SuspendLayout()
        '
        'txtInterval
        '
        Me.txtInterval.AcceptsReturn = True
        Me.txtInterval.BackColor = System.Drawing.SystemColors.Window
        Me.txtInterval.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtInterval.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtInterval.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtInterval.Location = New System.Drawing.Point(205, 106)
        Me.txtInterval.MaxLength = 0
        Me.txtInterval.Name = "txtInterval"
        Me.txtInterval.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtInterval.Size = New System.Drawing.Size(183, 26)
        Me.txtInterval.TabIndex = 48
        Me.txtInterval.Text = "60"
        '
        'label1
        '
        Me.label1.AutoSize = True
        Me.label1.Location = New System.Drawing.Point(44, 113)
        Me.label1.Name = "label1"
        Me.label1.Size = New System.Drawing.Size(54, 16)
        Me.label1.TabIndex = 41
        Me.label1.Text = "Interval:"
        '
        'label13
        '
        Me.label13.AutoSize = True
        Me.label13.Location = New System.Drawing.Point(44, 77)
        Me.label13.Name = "label13"
        Me.label13.Size = New System.Drawing.Size(71, 16)
        Me.label13.TabIndex = 42
        Me.label13.Text = "Timezone:"
        '
        'label12
        '
        Me.label12.AutoSize = True
        Me.label12.Location = New System.Drawing.Point(44, 39)
        Me.label12.Name = "label12"
        Me.label12.Size = New System.Drawing.Size(136, 16)
        Me.label12.TabIndex = 43
        Me.label12.Text = "NTP Server Address:"
        '
        'btnGetNtpServerSettings
        '
        Me.btnGetNtpServerSettings.Location = New System.Drawing.Point(84, 151)
        Me.btnGetNtpServerSettings.Name = "btnGetNtpServerSettings"
        Me.btnGetNtpServerSettings.Size = New System.Drawing.Size(81, 33)
        Me.btnGetNtpServerSettings.TabIndex = 44
        Me.btnGetNtpServerSettings.Text = "Get"
        Me.btnGetNtpServerSettings.UseVisualStyleBackColor = True
        '
        'btnSetNtpServerSettings
        '
        Me.btnSetNtpServerSettings.Location = New System.Drawing.Point(228, 151)
        Me.btnSetNtpServerSettings.Name = "btnSetNtpServerSettings"
        Me.btnSetNtpServerSettings.Size = New System.Drawing.Size(81, 33)
        Me.btnSetNtpServerSettings.TabIndex = 45
        Me.btnSetNtpServerSettings.Text = "Set"
        Me.btnSetNtpServerSettings.UseVisualStyleBackColor = True
        '
        'txtServerAddress
        '
        Me.txtServerAddress.AcceptsReturn = True
        Me.txtServerAddress.BackColor = System.Drawing.SystemColors.Window
        Me.txtServerAddress.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtServerAddress.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtServerAddress.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtServerAddress.Location = New System.Drawing.Point(205, 32)
        Me.txtServerAddress.MaxLength = 0
        Me.txtServerAddress.Name = "txtServerAddress"
        Me.txtServerAddress.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtServerAddress.Size = New System.Drawing.Size(183, 26)
        Me.txtServerAddress.TabIndex = 46
        Me.txtServerAddress.Text = "1.cn.pool.ntp.org"
        '
        'txtTimezone
        '
        Me.txtTimezone.AcceptsReturn = True
        Me.txtTimezone.BackColor = System.Drawing.SystemColors.Window
        Me.txtTimezone.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtTimezone.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTimezone.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtTimezone.Location = New System.Drawing.Point(205, 70)
        Me.txtTimezone.MaxLength = 0
        Me.txtTimezone.Name = "txtTimezone"
        Me.txtTimezone.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtTimezone.Size = New System.Drawing.Size(183, 26)
        Me.txtTimezone.TabIndex = 47
        Me.txtTimezone.Text = "480"
        '
        'frmNtpServerSetting
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(445, 212)
        Me.Controls.Add(Me.txtInterval)
        Me.Controls.Add(Me.label1)
        Me.Controls.Add(Me.label13)
        Me.Controls.Add(Me.label12)
        Me.Controls.Add(Me.btnGetNtpServerSettings)
        Me.Controls.Add(Me.btnSetNtpServerSettings)
        Me.Controls.Add(Me.txtServerAddress)
        Me.Controls.Add(Me.txtTimezone)
        Me.Name = "frmNtpServerSetting"
        Me.Text = "frmNtpServerSetting"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Public WithEvents txtInterval As TextBox
    Private WithEvents label1 As Label
    Private WithEvents label13 As Label
    Private WithEvents label12 As Label
    Private WithEvents btnGetNtpServerSettings As Button
    Private WithEvents btnSetNtpServerSettings As Button
    Public WithEvents txtServerAddress As TextBox
    Public WithEvents txtTimezone As TextBox
End Class
