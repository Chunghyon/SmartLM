
Partial Class frmDeviceSearch
    ''' <summary>
    ''' Required designer variable.
    ''' </summary>
    Private components As System.ComponentModel.IContainer = Nothing

    ''' <summary>
    ''' Clean up any resources being used.
    ''' </summary>
    ''' <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    Protected Overrides Sub Dispose(disposing As Boolean)
        If disposing AndAlso (components IsNot Nothing) Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

#Region "Windows Form Designer generated code"

    ''' <summary>
    ''' Required method for Designer support - do not modify
    ''' the contents of this method with the code editor.
    ''' </summary>
    Private Sub InitializeComponent()
        Me.label1 = New System.Windows.Forms.Label()
        Me.txtProductName = New System.Windows.Forms.TextBox()
        Me.label2 = New System.Windows.Forms.Label()
        Me.txtDuration = New System.Windows.Forms.TextBox()
        Me.btnSearch = New System.Windows.Forms.Button()
        Me.txtResult = New System.Windows.Forms.TextBox()
        Me.SuspendLayout()
        ' 
        ' label1
        ' 
        Me.label1.AutoSize = True
        Me.label1.Location = New System.Drawing.Point(12, 15)
        Me.label1.Name = "label1"
        Me.label1.Size = New System.Drawing.Size(72, 13)
        Me.label1.TabIndex = 0
        Me.label1.Text = "ProductName"
        ' 
        ' txtProductName
        ' 
        Me.txtProductName.Location = New System.Drawing.Point(121, 13)
        Me.txtProductName.Name = "txtProductName"
        Me.txtProductName.Size = New System.Drawing.Size(100, 20)
        Me.txtProductName.TabIndex = 1
        Me.txtProductName.Text = "Sx00-"
        ' 
        ' label2
        ' 
        Me.label2.AutoSize = True
        Me.label2.Location = New System.Drawing.Point(12, 41)
        Me.label2.Name = "label2"
        Me.label2.Size = New System.Drawing.Size(103, 13)
        Me.label2.TabIndex = 0
        Me.label2.Text = "Search Duration(ms)"
        ' 
        ' txtDuration
        ' 
        Me.txtDuration.Location = New System.Drawing.Point(121, 39)
        Me.txtDuration.Name = "txtDuration"
        Me.txtDuration.Size = New System.Drawing.Size(100, 20)
        Me.txtDuration.TabIndex = 1
        Me.txtDuration.Text = "2000"
        ' 
        ' btnSearch
        ' 
        Me.btnSearch.Location = New System.Drawing.Point(248, 30)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(111, 29)
        Me.btnSearch.TabIndex = 2
        Me.btnSearch.Text = "Search"
        Me.btnSearch.UseVisualStyleBackColor = True
        ' 
        ' txtResult
        ' 
        Me.txtResult.Font = New System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CByte(0))
        Me.txtResult.Location = New System.Drawing.Point(15, 74)
        Me.txtResult.Multiline = True
        Me.txtResult.Name = "txtResult"
        Me.txtResult.[ReadOnly] = True
        Me.txtResult.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.txtResult.Size = New System.Drawing.Size(344, 446)
        Me.txtResult.TabIndex = 3
        ' 
        ' frmDeviceSearch
        ' 
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0F, 13.0F)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(375, 531)
        Me.Controls.Add(Me.txtResult)
        Me.Controls.Add(Me.btnSearch)
        Me.Controls.Add(Me.txtDuration)
        Me.Controls.Add(Me.txtProductName)
        Me.Controls.Add(Me.label2)
        Me.Controls.Add(Me.label1)
        Me.Name = "frmDeviceSearch"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "frmDeviceSearch"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

    Private WithEvents label1 As System.Windows.Forms.Label
    Private WithEvents txtProductName As System.Windows.Forms.TextBox
    Private WithEvents label2 As System.Windows.Forms.Label
    Private WithEvents txtDuration As System.Windows.Forms.TextBox
    Private WithEvents btnSearch As System.Windows.Forms.Button
    Private WithEvents txtResult As System.Windows.Forms.TextBox
End Class
