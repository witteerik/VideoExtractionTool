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
                Dim OutputPath2 = IO.Path.Combine(IO.Path.GetDirectoryName(OutputPath), IO.Path.GetFileNameWithoutExtension(ExtractionVideo.Item2) & "_B.mp4")
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
                OutputPath2 = Chr(34) & OutputPath2 & Chr(34)

                Dim StartTimeString As String = (StartTime).ToString.Replace(",", ".")
                Dim DurationString As String = Duration.ToString.Replace(",", ".")
                Dim To_String As String = (StartTime + Duration).ToString.Replace(",", ".")


                'Creating resampled file
                Dim ffmpegProcessStartInfo As New ProcessStartInfo()
                ffmpegProcessStartInfo.FileName = ffmpegPath

                If StartTime = 0 Then
                    'ffmpegProcessStartInfo.Arguments = "-i " & InputPath & " -muxdelay 0 -map 0 -c copy -muxpreload 0" & " -t " & DurationString & " " & OutputPath
                    ffmpegProcessStartInfo.Arguments = "-i " & InputPath & " -c copy -t " & DurationString & " " & OutputPath
                Else
                    'N.B. For A/V sync to work well, the -ss argument needs to be an input argument (i.e. specified before -i). Cf https://ffmpeg.org/ffmpeg.html section on "-ss position (input/output)"
                    'ffmpegProcessStartInfo.Arguments = "-ss " & StartTimeString & " -i " & InputPath & " -c copy -t " & DurationString & " " & OutputPath
                    'ffmpegProcessStartInfo.Arguments = "-ss " & StartTimeString & " -i " & InputPath & " -muxdelay 0 -map 0 -c copy -muxpreload 0 -t " & DurationString & " " & OutputPath
                    'ffmpegProcessStartInfo.Arguments = "-ss " & StartTimeString & " -to " & To_String & " -i " & InputPath & " -c copy " & OutputPath

                    ffmpegProcessStartInfo.Arguments = "-ss " & StartTimeString & " -to " & To_String & " -i " & InputPath & " -muxdelay 0 -muxpreload 0 -c copy " & OutputPath

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

                Dim Fade As Boolean = False
                If Fade = True Then

                    'Cf. http://underpop.online.fr/f/ffmpeg/help/fade.htm.gz

                    Dim FadeDuration As Double = 0.3
                    Dim FadeOutStartString As String = Math.Max(0, Duration - FadeDuration).ToString.Replace(",", ".")
                    Dim FadeDurationString As String = FadeDuration.ToString.Replace(",", ".")

                    Dim ffmpegProcessStartInfo2 As New ProcessStartInfo()
                    ffmpegProcessStartInfo2.FileName = ffmpegPath

                    'ffmpegProcessStartInfo2.Arguments = "-i " & OutputPath & "-vf fade=t=in:st=0:d=10,fade=t=out:st=10:d=5"

                    'ffmpegProcessStartInfo2.Arguments = "-i " & OutputPath & " -vf chromakey=#0047bb " & OutputPath2

                    'ffmpegProcessStartInfo2.Arguments = "-f lavfi -i color=c=black:s=3840x2160 -i " & OutputPath & " -shortest " &
                    '    " -filter_complex " & Chr(34) & "[1:v]chromakey=#0047bb:0.1:0.2[ckout]; [0:v][ckout]overlay[out]" & Chr(34) &
                    '    " -map " & Chr(34) & "[out]" & Chr(34) & " " & OutputPath2

                    ffmpegProcessStartInfo2.Arguments = "-f lavfi -i " & Chr(34) & "C:\Temp\1109183.jpg" & Chr(34) & " -i " & OutputPath &
                        " -filter_complex " & Chr(34) & "[1:v]chromakey=#0047bb:0.1:0.2[ckout]; [0:v][ckout]overlay[out]" & Chr(34) &
                        " -map " & Chr(34) & "[out]" & Chr(34) & " " & OutputPath2


                    'For continued work. Most things tested in command promt only

                    '# Fading
                    'https://dev.to/dak425/add-fade-in-and-fade-out-effects-with-ffmpeg-2bj7
                    '
                    '-vf "fade=t=in:st=0:d=3:color=white" -c:a copy
                    '
                    'ffmpeg -i "D:\Eikholt\Testvideos\Output\mix.mp4" -vf "fade=t=in:st=0:d=0.3" -c:a copy "D:\Eikholt\Testvideos\Output\mix_fade.mp4"
                    '
                    '# Bluescreen
                    '
                    'ffmpeg -i "C:\Temp\1109183.jpg" -i "D:\Eikholt\Testvideos\Output\Actor_1_L16S00_M1.mp4" -filter_complex "[1:v]chromakey=#0047bb:0.1:0.13[ckout]; [0:v][ckout]overlay'[out]" -map "[out]" -map 1:a "D:\Eikholt\Testvideos\Output\Actor_1_L16S00_M1_B.mp4"
                    '
                    '
                    '# Getting audio
                    '# https://stackoverflow.com/questions/9913032/how-can-i-extract-audio-from-video-with-ffmpeg
                    'ffmpeg -i <video_file_name.extension> <audio_file_name.extension>
                    '
                    'ffmpeg -i "D:\Eikholt\Testvideos\Output\Actor_1_L16S00_M1.mp4" "D:\Eikholt\Testvideos\Output\Actor_1_L16S00_M1.wav"
                    '
                    '
                    '# IR-Reverb
                    '# https://medium.com/@glynn_bird/applying-reverb-to-audio-with-ffmpeg-and-impulse-responses-81b4480cf5aa
                    '
                    'ffmpeg -i "D:\Eikholt\Testvideos\Output\Actor_1_L16S00_M1.wav" -i "D:\Eikholt\Testvideos\S2R4_sweep4000.wav" -filter_complex "[0] [1] afir=dry=10:wet=10" "D:\Eikholt\Testvideos\Output\output.wav"
                    '
                    'ffmpeg -i "D:\Eikholt\Testvideos\Output\Actor_1_L16S00_M1.wav" -i "D:\Eikholt\Testvideos\S2R4_sweep4000.wav" -filter_complex "[0] [1] afir=dry=10:wet=10 [reverb]; [0] [reverb] amix=inputs=2:weights=10 1" "D:\Eikholt\Testvideos\Output\mix.wav"
                    '
                    'ffmpeg -i "D:\Eikholt\Testvideos\Output\Actor_1_L16S00_M1.wav" -i "D:\Eikholt\Testvideos\S2R4_sweep4000.wav" -filter_complex "[0] [1] afir=dry=10:wet=10 [reverb]; [0] [reverb] amix=inputs=2:weights=3 1" "D:\Eikholt\Testvideos\Output\mix.wav"
                    '
                    '# Mixing video and audio
                    '# https://superuser.com/questions/590201/add-audio-to-video-using-ffmpeg
                    '
                    'ffmpeg -i input.mp4 -i input.mp3 -c copy -map 0:v:0 -map 1:a:0 output.mp4
                    '
                    'ffmpeg -i "D:\Eikholt\Testvideos\Output\Actor_1_L16S00_M1_B.mp4" -i "D:\Eikholt\Testvideos\Output\mix.wav" -c copy -map 0:v:0 -map 1:a:0 "D:\Eikholt\Testvideos\Output\mix.mp4"
                    '
                    'ffmpeg -i "D:\Eikholt\Testvideos\Output\Actor_1_L16S00_M1_B.mp4" -i "D:\Eikholt\Testvideos\Output\mix.wav" -c:v copy -map 0:v:0 -map 1:a:0 -c:a aac -b:a 192k "D:\Eikholt\Testvideos\Output\mix.mp4"




                    'Dim base_video_path As String = Chr(34) & "D:\Eikholt\Testvideos\Base.mp4" & Chr(34)

                    'ffmpegProcessStartInfo2.Arguments =
                    '    "-hwaccel cuda -hwaccel_output_format cuda -i " & OutputPath &
                    '    " -hwaccel cuda -hwaccel_output_format cuda -i " & base_video_path &
                    '    " -init_hw_device cuda -filter_complex " &
                    '    Chr(34) & "[0:v]chromakey_cuda=0x25302D:0.1:0.12:1[overlay_video]; [1:v]scale_cuda=format=yuv420p[base]; [base][overlay_video]overlay_cuda" & Chr(34) &
                    '    " -an -sn -c:v h264_nvenc - cq 20 " & OutputPath2

                    'ffmpegProcessStartInfo2.Arguments =
                    '    "-hwaccel cuda -hwaccel_output_format cuda -i " & OutputPath &
                    '    " -hwaccel cuda -hwaccel_output_format cuda -i " & base_video_path &
                    '    " -init_hw_device cuda -filter_complex " &
                    '    Chr(34) & "[0:v]chromakey_cuda=0x25302D:0.1:0.12:1[overlay_video]; [1:v]scale_cuda=format=yuv420p[base]; [base][overlay_video]overlay_cuda" & Chr(34) &
                    '    " " & OutputPath2


                    'ffmpegProcessStartInfo2.Arguments =
                    '    "-hwaccel cuda -hwaccel_output_format cuda -i " & OutputPath &
                    '    " -hwaccel cuda -hwaccel_output_format cuda -i " & base_video_path &
                    '    " -init_hw_device cuda -filter_complex " &
                    '    Chr(34) & "[0:v]chromakey_cuda=0x25302D:0.1:0.12:1[overlay_video]; [1:v]scale_cuda=format=yuv420p[base]; [base][overlay_video]overlay_cuda" & Chr(34) &
                    '    " " & OutputPath2


                    'ffmpegProcessStartInfo2.Arguments = "-i " & OutputPath & " -vf fade=t=in:st=0:d=" & FadeDurationString & ",fade=t=out:st=" & FadeOutStartString & ":d=" & FadeDurationString & " " & OutputPath2

                    If ShowProcessWindow_CheckBox.Checked = False Then
                        ffmpegProcessStartInfo2.CreateNoWindow = True
                    End If

                    Dim sp2 = Process.Start(ffmpegProcessStartInfo2)
                    sp2.WaitForExit()

                    If sp2.ExitCode = 1 Then
                        FailedItems.Add(InputPath & vbTab & OutputPath2 & vbTab & "Fading process failed!")
                    End If

                    sp2.Close()

                End If

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