<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class MainForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MainForm))
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Info_RichTextBox = New System.Windows.Forms.RichTextBox()
        Me.ffmpegPath_TextBox = New System.Windows.Forms.TextBox()
        Me.Data_RichTextBox = New System.Windows.Forms.RichTextBox()
        Me.Start_Button = New System.Windows.Forms.Button()
        Me.ShowProcessWindow_CheckBox = New System.Windows.Forms.CheckBox()
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.Process_ToolStripStatusLabel = New System.Windows.Forms.ToolStripStatusLabel()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.StatusStrip1.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.ColumnCount = 2
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 19.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 81.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.Label1, 0, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.Label3, 0, 2)
        Me.TableLayoutPanel1.Controls.Add(Me.Info_RichTextBox, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.ffmpegPath_TextBox, 1, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.Data_RichTextBox, 0, 3)
        Me.TableLayoutPanel1.Controls.Add(Me.Start_Button, 0, 5)
        Me.TableLayoutPanel1.Controls.Add(Me.ShowProcessWindow_CheckBox, 0, 4)
        Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 6
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 138.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 67.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(807, 470)
        Me.TableLayoutPanel1.TabIndex = 0
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label1.Location = New System.Drawing.Point(3, 138)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(147, 30)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "ffmpeg.exe file path:"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label3.Location = New System.Drawing.Point(3, 168)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(147, 20)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "Input data"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.BottomLeft
        '
        'Info_RichTextBox
        '
        Me.TableLayoutPanel1.SetColumnSpan(Me.Info_RichTextBox, 2)
        Me.Info_RichTextBox.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Info_RichTextBox.Location = New System.Drawing.Point(3, 3)
        Me.Info_RichTextBox.Name = "Info_RichTextBox"
        Me.Info_RichTextBox.ReadOnly = True
        Me.Info_RichTextBox.Size = New System.Drawing.Size(801, 132)
        Me.Info_RichTextBox.TabIndex = 4
        Me.Info_RichTextBox.Text = resources.GetString("Info_RichTextBox.Text")
        '
        'ffmpegPath_TextBox
        '
        Me.ffmpegPath_TextBox.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ffmpegPath_TextBox.Location = New System.Drawing.Point(156, 141)
        Me.ffmpegPath_TextBox.Name = "ffmpegPath_TextBox"
        Me.ffmpegPath_TextBox.Size = New System.Drawing.Size(648, 23)
        Me.ffmpegPath_TextBox.TabIndex = 5
        '
        'Data_RichTextBox
        '
        Me.TableLayoutPanel1.SetColumnSpan(Me.Data_RichTextBox, 2)
        Me.Data_RichTextBox.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Data_RichTextBox.Location = New System.Drawing.Point(3, 191)
        Me.Data_RichTextBox.Name = "Data_RichTextBox"
        Me.Data_RichTextBox.Size = New System.Drawing.Size(801, 184)
        Me.Data_RichTextBox.TabIndex = 6
        Me.Data_RichTextBox.Text = ""
        '
        'Start_Button
        '
        Me.TableLayoutPanel1.SetColumnSpan(Me.Start_Button, 2)
        Me.Start_Button.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Start_Button.Location = New System.Drawing.Point(3, 406)
        Me.Start_Button.Name = "Start_Button"
        Me.Start_Button.Size = New System.Drawing.Size(801, 61)
        Me.Start_Button.TabIndex = 7
        Me.Start_Button.Text = "Start extraction process"
        Me.Start_Button.UseVisualStyleBackColor = True
        '
        'ShowProcessWindow_CheckBox
        '
        Me.ShowProcessWindow_CheckBox.AutoSize = True
        Me.ShowProcessWindow_CheckBox.Location = New System.Drawing.Point(3, 381)
        Me.ShowProcessWindow_CheckBox.Name = "ShowProcessWindow_CheckBox"
        Me.ShowProcessWindow_CheckBox.Size = New System.Drawing.Size(143, 19)
        Me.ShowProcessWindow_CheckBox.TabIndex = 8
        Me.ShowProcessWindow_CheckBox.Text = "Show process window"
        Me.ShowProcessWindow_CheckBox.UseVisualStyleBackColor = True
        '
        'StatusStrip1
        '
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.Process_ToolStripStatusLabel})
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 470)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.StatusStrip1.Size = New System.Drawing.Size(807, 22)
        Me.StatusStrip1.TabIndex = 1
        Me.StatusStrip1.Text = "Idle"
        '
        'Process_ToolStripStatusLabel
        '
        Me.Process_ToolStripStatusLabel.Name = "Process_ToolStripStatusLabel"
        Me.Process_ToolStripStatusLabel.Size = New System.Drawing.Size(26, 17)
        Me.Process_ToolStripStatusLabel.Text = "Idle"
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.TableLayoutPanel1)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(807, 470)
        Me.Panel1.TabIndex = 2
        '
        'MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(807, 492)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.StatusStrip1)
        Me.Name = "MainForm"
        Me.Text = "Video extraction tool"
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.TableLayoutPanel1.PerformLayout()
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents TableLayoutPanel1 As TableLayoutPanel
    Friend WithEvents Label1 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents Info_RichTextBox As RichTextBox
    Friend WithEvents ffmpegPath_TextBox As TextBox
    Friend WithEvents Data_RichTextBox As RichTextBox
    Friend WithEvents Start_Button As Button
    Friend WithEvents StatusStrip1 As StatusStrip
    Friend WithEvents Panel1 As Panel
    Friend WithEvents Process_ToolStripStatusLabel As ToolStripStatusLabel
    Friend WithEvents ShowProcessWindow_CheckBox As CheckBox
End Class
