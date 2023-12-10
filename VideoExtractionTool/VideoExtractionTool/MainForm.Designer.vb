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
        Dim resources As ComponentModel.ComponentResourceManager = New ComponentModel.ComponentResourceManager(GetType(MainForm))
        TableLayoutPanel1 = New TableLayoutPanel()
        Label1 = New Label()
        Label3 = New Label()
        Info_RichTextBox = New RichTextBox()
        ffmpegPath_TextBox = New TextBox()
        Data_RichTextBox = New RichTextBox()
        Start_Button = New Button()
        ShowProcessWindow_CheckBox = New CheckBox()
        Label2 = New Label()
        ffprobePath_TextBox = New TextBox()
        StatusStrip1 = New StatusStrip()
        Process_ToolStripStatusLabel = New ToolStripStatusLabel()
        Panel1 = New Panel()
        MenuStrip1 = New MenuStrip()
        CustomToolsToolStripMenuItem = New ToolStripMenuItem()
        TableLayoutPanel1.SuspendLayout()
        StatusStrip1.SuspendLayout()
        Panel1.SuspendLayout()
        MenuStrip1.SuspendLayout()
        SuspendLayout()
        ' 
        ' TableLayoutPanel1
        ' 
        TableLayoutPanel1.ColumnCount = 2
        TableLayoutPanel1.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 19F))
        TableLayoutPanel1.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 81F))
        TableLayoutPanel1.Controls.Add(Label1, 0, 1)
        TableLayoutPanel1.Controls.Add(Label3, 0, 3)
        TableLayoutPanel1.Controls.Add(Info_RichTextBox, 0, 0)
        TableLayoutPanel1.Controls.Add(ffmpegPath_TextBox, 1, 1)
        TableLayoutPanel1.Controls.Add(Data_RichTextBox, 0, 4)
        TableLayoutPanel1.Controls.Add(Start_Button, 0, 6)
        TableLayoutPanel1.Controls.Add(ShowProcessWindow_CheckBox, 0, 5)
        TableLayoutPanel1.Controls.Add(Label2, 0, 2)
        TableLayoutPanel1.Controls.Add(ffprobePath_TextBox, 1, 2)
        TableLayoutPanel1.Dock = DockStyle.Fill
        TableLayoutPanel1.Location = New Point(0, 0)
        TableLayoutPanel1.Name = "TableLayoutPanel1"
        TableLayoutPanel1.RowCount = 7
        TableLayoutPanel1.RowStyles.Add(New RowStyle(SizeType.Absolute, 138F))
        TableLayoutPanel1.RowStyles.Add(New RowStyle(SizeType.Absolute, 30F))
        TableLayoutPanel1.RowStyles.Add(New RowStyle(SizeType.Absolute, 30F))
        TableLayoutPanel1.RowStyles.Add(New RowStyle(SizeType.Absolute, 20F))
        TableLayoutPanel1.RowStyles.Add(New RowStyle(SizeType.Percent, 100F))
        TableLayoutPanel1.RowStyles.Add(New RowStyle(SizeType.Absolute, 25F))
        TableLayoutPanel1.RowStyles.Add(New RowStyle(SizeType.Absolute, 67F))
        TableLayoutPanel1.RowStyles.Add(New RowStyle(SizeType.Absolute, 20F))
        TableLayoutPanel1.Size = New Size(807, 446)
        TableLayoutPanel1.TabIndex = 0
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.Dock = DockStyle.Fill
        Label1.Location = New Point(3, 138)
        Label1.Name = "Label1"
        Label1.Size = New Size(147, 30)
        Label1.TabIndex = 0
        Label1.Text = "ffmpeg.exe file path:"
        Label1.TextAlign = ContentAlignment.MiddleRight
        ' 
        ' Label3
        ' 
        Label3.AutoSize = True
        Label3.Dock = DockStyle.Fill
        Label3.Location = New Point(3, 198)
        Label3.Name = "Label3"
        Label3.Size = New Size(147, 20)
        Label3.TabIndex = 2
        Label3.Text = "Input data"
        Label3.TextAlign = ContentAlignment.BottomLeft
        ' 
        ' Info_RichTextBox
        ' 
        TableLayoutPanel1.SetColumnSpan(Info_RichTextBox, 2)
        Info_RichTextBox.Dock = DockStyle.Fill
        Info_RichTextBox.Location = New Point(3, 3)
        Info_RichTextBox.Name = "Info_RichTextBox"
        Info_RichTextBox.ReadOnly = True
        Info_RichTextBox.Size = New Size(801, 132)
        Info_RichTextBox.TabIndex = 4
        Info_RichTextBox.Text = resources.GetString("Info_RichTextBox.Text")
        ' 
        ' ffmpegPath_TextBox
        ' 
        ffmpegPath_TextBox.Dock = DockStyle.Fill
        ffmpegPath_TextBox.Location = New Point(156, 141)
        ffmpegPath_TextBox.Name = "ffmpegPath_TextBox"
        ffmpegPath_TextBox.Size = New Size(648, 23)
        ffmpegPath_TextBox.TabIndex = 5
        ' 
        ' Data_RichTextBox
        ' 
        TableLayoutPanel1.SetColumnSpan(Data_RichTextBox, 2)
        Data_RichTextBox.Dock = DockStyle.Fill
        Data_RichTextBox.Location = New Point(3, 221)
        Data_RichTextBox.Name = "Data_RichTextBox"
        Data_RichTextBox.Size = New Size(801, 130)
        Data_RichTextBox.TabIndex = 6
        Data_RichTextBox.Text = ""
        ' 
        ' Start_Button
        ' 
        TableLayoutPanel1.SetColumnSpan(Start_Button, 2)
        Start_Button.Dock = DockStyle.Fill
        Start_Button.Location = New Point(3, 382)
        Start_Button.Name = "Start_Button"
        Start_Button.Size = New Size(801, 61)
        Start_Button.TabIndex = 7
        Start_Button.Text = "Start extraction process"
        Start_Button.UseVisualStyleBackColor = True
        ' 
        ' ShowProcessWindow_CheckBox
        ' 
        ShowProcessWindow_CheckBox.AutoSize = True
        ShowProcessWindow_CheckBox.Location = New Point(3, 357)
        ShowProcessWindow_CheckBox.Name = "ShowProcessWindow_CheckBox"
        ShowProcessWindow_CheckBox.Size = New Size(143, 19)
        ShowProcessWindow_CheckBox.TabIndex = 8
        ShowProcessWindow_CheckBox.Text = "Show process window"
        ShowProcessWindow_CheckBox.UseVisualStyleBackColor = True
        ' 
        ' Label2
        ' 
        Label2.AutoSize = True
        Label2.Dock = DockStyle.Fill
        Label2.Location = New Point(3, 168)
        Label2.Name = "Label2"
        Label2.Size = New Size(147, 30)
        Label2.TabIndex = 9
        Label2.Text = "ffprobe.exe file path:"
        Label2.TextAlign = ContentAlignment.MiddleRight
        ' 
        ' ffprobePath_TextBox
        ' 
        ffprobePath_TextBox.Dock = DockStyle.Fill
        ffprobePath_TextBox.Location = New Point(156, 171)
        ffprobePath_TextBox.Name = "ffprobePath_TextBox"
        ffprobePath_TextBox.Size = New Size(648, 23)
        ffprobePath_TextBox.TabIndex = 10
        ' 
        ' StatusStrip1
        ' 
        StatusStrip1.Items.AddRange(New ToolStripItem() {Process_ToolStripStatusLabel})
        StatusStrip1.Location = New Point(0, 470)
        StatusStrip1.Name = "StatusStrip1"
        StatusStrip1.RightToLeft = RightToLeft.Yes
        StatusStrip1.Size = New Size(807, 22)
        StatusStrip1.TabIndex = 1
        StatusStrip1.Text = "Idle"
        ' 
        ' Process_ToolStripStatusLabel
        ' 
        Process_ToolStripStatusLabel.Name = "Process_ToolStripStatusLabel"
        Process_ToolStripStatusLabel.Size = New Size(26, 17)
        Process_ToolStripStatusLabel.Text = "Idle"
        ' 
        ' Panel1
        ' 
        Panel1.Controls.Add(TableLayoutPanel1)
        Panel1.Dock = DockStyle.Fill
        Panel1.Location = New Point(0, 24)
        Panel1.Name = "Panel1"
        Panel1.Size = New Size(807, 446)
        Panel1.TabIndex = 2
        ' 
        ' MenuStrip1
        ' 
        MenuStrip1.Items.AddRange(New ToolStripItem() {CustomToolsToolStripMenuItem})
        MenuStrip1.Location = New Point(0, 0)
        MenuStrip1.Name = "MenuStrip1"
        MenuStrip1.Size = New Size(807, 24)
        MenuStrip1.TabIndex = 3
        MenuStrip1.Text = "MenuStrip1"
        ' 
        ' CustomToolsToolStripMenuItem
        ' 
        CustomToolsToolStripMenuItem.Name = "CustomToolsToolStripMenuItem"
        CustomToolsToolStripMenuItem.Size = New Size(90, 20)
        CustomToolsToolStripMenuItem.Text = "Custom tools"
        ' 
        ' MainForm
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(807, 492)
        Controls.Add(Panel1)
        Controls.Add(StatusStrip1)
        Controls.Add(MenuStrip1)
        MainMenuStrip = MenuStrip1
        Name = "MainForm"
        Text = "Video extraction tool"
        TableLayoutPanel1.ResumeLayout(False)
        TableLayoutPanel1.PerformLayout()
        StatusStrip1.ResumeLayout(False)
        StatusStrip1.PerformLayout()
        Panel1.ResumeLayout(False)
        MenuStrip1.ResumeLayout(False)
        MenuStrip1.PerformLayout()
        ResumeLayout(False)
        PerformLayout()
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
    Friend WithEvents Label2 As Label
    Friend WithEvents ffprobePath_TextBox As TextBox
    Friend WithEvents MenuStrip1 As MenuStrip
    Friend WithEvents CustomToolsToolStripMenuItem As ToolStripMenuItem
End Class
