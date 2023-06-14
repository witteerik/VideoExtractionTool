Imports System.Globalization

Public Class MainForm

    Private Sub MainForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        SetStartButtonLook(StartButtonLooks.Start)

    End Sub

    Private Sub Start_Button_Click(sender As Object, e As EventArgs) Handles Start_Button.Click

        Try

            'Checking that the ffmpeg.exe file exists
            Dim ffmpegPath = ffmpegPath_TextBox.Text.Trim
            If ffmpegPath.Trim = "" Then
                MsgBox("You must indicate the path to an ffmpeg.exe file!", MsgBoxStyle.Exclamation, "Video Extraction Tool")
                Exit Sub
            End If
            If IO.File.Exists(ffmpegPath) = False Then
                MsgBox("The supplied path to the ffmpeg.exe file does not exist!", MsgBoxStyle.Exclamation, "Video Extraction Tool")
                Exit Sub
            End If

            'Getting data
            Dim ExtractionData As New List(Of Tuple(Of String, String, Double, Double))

            Dim OutputCheckList As New SortedSet(Of String)

            Dim DataLines = Data_RichTextBox.Lines

            For Each DataLine In DataLines

                'Skipping empty lines
                If DataLine.Trim = "" Then Continue For

                'Checking that there are four tab-delimited items on the line
                Dim LineCols = DataLine.Trim.Split(vbTab)
                If LineCols.Length <> 4 Then
                    MsgBox("The following line does not have four tab-delimited values (as required)!" & vbCrLf & vbCrLf & DataLine, MsgBoxStyle.Exclamation, "Video Extraction Tool")
                    Exit Sub
                End If

                Dim InputPath = LineCols(0).Trim
                Dim OutputPath = LineCols(1).Trim
                Dim StartTimeString = LineCols(2).Trim
                Dim DurationString = LineCols(3).Trim
                Dim StartTime As Double
                Dim Duration As Double

                'Parsing and checking values
                If IO.File.Exists(InputPath) = False Then
                    MsgBox("The input file indicated on the following line does not exist!" & vbCrLf & vbCrLf & DataLine, MsgBoxStyle.Exclamation, "Video Extraction Tool")
                    Exit Sub
                End If
                If Double.TryParse(StartTimeString.Replace(",", "."), NumberStyles.Float, CultureInfo.InvariantCulture, StartTime) = False Then
                    MsgBox("Unable to parse the start time on the following line!" & vbCrLf & vbCrLf & DataLine, MsgBoxStyle.Exclamation, "Video Extraction Tool")
                    Exit Sub
                End If
                If Double.TryParse(DurationString.Replace(",", "."), NumberStyles.Float, CultureInfo.InvariantCulture, Duration) = False Then
                    MsgBox("Unable to parse the duration on the following line!" & vbCrLf & vbCrLf & DataLine, MsgBoxStyle.Exclamation, "Video Extraction Tool")
                    Exit Sub
                End If

                'Checks that output files are unique
                If OutputCheckList.Contains(OutputPath) Then
                    MsgBox("The output file indicated on the following line is listed more than once!" & vbCrLf & vbCrLf & DataLine, MsgBoxStyle.Exclamation, "Video Extraction Tool")
                    Exit Sub
                Else
                    OutputCheckList.Add(OutputPath)
                End If

                'Checks that the output file does not exist
                If IO.File.Exists(OutputPath) Then
                    MsgBox("The output file indicated on the following line already exist! Remove it and try again! (Overwriting files is not supported.)" & vbCrLf & vbCrLf & DataLine, MsgBoxStyle.Exclamation, "Video Extraction Tool")
                    Exit Sub
                End If

                ExtractionData.Add(New Tuple(Of String, String, Double, Double)(InputPath, OutputPath, StartTime, Duration))

            Next

            'Everything seems fine, changing the start button to stop mode
            SetStartButtonLook(StartButtonLooks.Stop)

            'Noting progress
            Dim ProcessedItems As Integer = 0
            UpdateProgressLabel(ProcessedItems, ExtractionData.Count)

            Dim FailedItems As New List(Of String)

            'Starting extraction
            For Each ExtractionVideo In ExtractionData


                Dim InputPath = ExtractionVideo.Item1
                Dim OutputPath = ExtractionVideo.Item2
                Dim StartTime = ExtractionVideo.Item3
                Dim Duration = ExtractionVideo.Item4

                'Modifying the starttime and duration to account for the (rather odd) way of specifying times (or maybe I've gotten it wrong..., it seems to work though...)
                Dim ModifyTimes As Boolean = False
                If ModifyTimes = True Then
                    Duration = Duration + StartTime
                    StartTime = -StartTime
                    If StartTime = -0 Then StartTime = 0
                End If

                'Creating the output folder
                IO.Directory.CreateDirectory(IO.Path.GetDirectoryName(OutputPath))

                'Adding " characters to the paths to allow empty spaces in them
                InputPath = Chr(34) & InputPath & Chr(34)
                OutputPath = Chr(34) & OutputPath & Chr(34)

                Dim StartTimeString As String = StartTime.ToString.Replace(",", ".")
                Dim DurationString As String = Duration.ToString.Replace(",", ".")

                'Creating resampled file
                Dim ffmpegProcessStartInfo As New ProcessStartInfo()
                ffmpegProcessStartInfo.FileName = ffmpegPath
                If StartTime = 0 Then
                    ffmpegProcessStartInfo.Arguments = "-i " & InputPath & " -c copy " & " -t " & DurationString & " " & OutputPath
                Else
                    ffmpegProcessStartInfo.Arguments = "-i " & InputPath & " -c copy -ss " & StartTimeString & " -t " & DurationString & " " & OutputPath
                End If

                If ShowProcessWindow_CheckBox.Checked = False Then
                    ffmpegProcessStartInfo.CreateNoWindow = True
                End If

                Dim sp = Process.Start(ffmpegProcessStartInfo)
                sp.WaitForExit()

                If sp.ExitCode = 1 Then
                    FailedItems.Add(InputPath & vbTab & OutputPath & vbTab & StartTimeString & vbTab & DurationString)
                End If

                sp.Close()

                ProcessedItems += 1
                UpdateProgressLabel(ProcessedItems, ExtractionData.Count)

                'Exits sub if the user pressed the stop button
                If CurrentStartButtonLook = StartButtonLooks.Start Then Exit Sub

            Next

            If FailedItems.Count > 0 Then
                'Error message
                Dim ErrorForm As New Windows.Forms.Form
                ErrorForm.Text = "Failed video extractions"
                ErrorForm.Height = 300
                ErrorForm.Width = 500

                Dim FailedItemCount As Integer = FailedItems.Count
                FailedItems.Insert(0, "The following " & FailedItemCount & " items could not be extracted:")
                FailedItems.Add("")
                FailedItems.Add("Close this window to continue!")

                Dim ErrorTextBox As New RichTextBox With {.Dock = DockStyle.Fill, .Lines = FailedItems.ToArray}
                ErrorForm.Controls.Add(ErrorTextBox)

                ErrorForm.ShowDialog()
            Else
                MsgBox("Finished extracting all videos.", MsgBoxStyle.Information, "Video Extraction Tool")
            End If

        Catch ex As Exception

            MsgBox("The following error occurred:" & vbCrLf & ex.ToString, MsgBoxStyle.Exclamation, "Video Extraction Tool")

        End Try

        'Resetting things
        ResetProgressLabel()
        SetStartButtonLook(StartButtonLooks.Start)

    End Sub

    Private Sub UpdateProgressLabel(ByVal ProcessedItems As Integer, ByVal TotalCount As Integer)

        Process_ToolStripStatusLabel.Text = "Processing " & ProcessedItems & " / " & TotalCount
        Process_ToolStripStatusLabel.Invalidate()
        Me.Update()

    End Sub

    Private Sub ResetProgressLabel()

        Process_ToolStripStatusLabel.Text = "Idle"
        Process_ToolStripStatusLabel.Invalidate()

    End Sub

    Private Enum StartButtonLooks
        Start
        [Stop]
    End Enum

    Private CurrentStartButtonLook As StartButtonLooks

    Private Sub SetStartButtonLook(ByVal Look As StartButtonLooks)

        CurrentStartButtonLook = Look

        Select Case Look
            Case StartButtonLooks.Start

                Start_Button.Text = "Start extraction process"
                Start_Button.Invalidate()
                Start_Button.Update()

            Case StartButtonLooks.Stop

                Start_Button.Text = "Stop processing"
                Start_Button.Invalidate()
                Start_Button.Update()

        End Select

    End Sub

End Class