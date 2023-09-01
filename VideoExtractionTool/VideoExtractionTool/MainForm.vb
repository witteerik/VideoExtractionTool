Imports System.Globalization


Public Class MainForm

    Private ffmpegPath As String
    Private ffprobePath As String

    Private Sub MainForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        SetStartButtonLook(StartButtonLooks.Start)

    End Sub

    Private Sub Start_Button_Click(sender As Object, e As EventArgs) Handles Start_Button.Click

        Try

            Dim ExtractionData = ParseInputData()
            If ExtractionData Is Nothing Then Exit Sub


            'Everything seems fine, changing the start button to stop mode
            SetStartButtonLook(StartButtonLooks.Stop)

            'Noting progress
            Dim ProcessedItems As Integer = 0
            UpdateProgressLabel(ProcessedItems, ExtractionData.Count)

            Dim FailedItems As New List(Of String)

            'Starting extraction
            For Each ExtractionVideo In ExtractionData

                Dim InputPath = ExtractionVideo.Item1
                Dim VideoOutputPath = ExtractionVideo.Item2
                Dim VideoOutputPath2 = IO.Path.Combine(IO.Path.GetDirectoryName(VideoOutputPath), IO.Path.GetFileNameWithoutExtension(ExtractionVideo.Item2) & "_Mix.mp4")
                Dim VideoStartReadTime = ExtractionVideo.Item3
                Dim VideoReadDuration = ExtractionVideo.Item4

                'Getting the closest keyframe time
                Dim StartKeyFrameTime As Double = GetKeyFrameTime(VideoStartReadTime, InputPath, IO.Path.GetDirectoryName(VideoOutputPath))
                'And calculating the amount of time in the video which will be read before the VideoStartReadTime
                Dim VideoAddedInitialTime As Double = VideoStartReadTime - StartKeyFrameTime

                'AudioInputFile	AudioStartReadTime	AudioReadDuration	AudioOutputFileAudioDelay
                Dim AudioInputFile As String = ""
                If ExtractionVideo.Item5 <> "" Then AudioInputFile = ExtractionVideo.Item5

                Dim AudioOutputFile As String = ""
                If ExtractionVideo.Item6 <> "" Then AudioOutputFile = ExtractionVideo.Item6

                Dim AudioStartReadTime As Double
                If ExtractionVideo.Item7 IsNot Nothing Then
                    AudioStartReadTime = ExtractionVideo.Item7.Value
                End If

                Dim AudioReadDuration As Double
                Dim AudioOutputInitialPadding As Double
                Dim AudioOutputFinalPadding As Double
                If ExtractionVideo.Rest IsNot Nothing Then
                    AudioReadDuration = ExtractionVideo.Rest.Item1
                    AudioOutputInitialPadding = ExtractionVideo.Rest.Item2
                    AudioOutputFinalPadding = ExtractionVideo.Rest.Item3
                End If


                'Creating the output folder
                IO.Directory.CreateDirectory(IO.Path.GetDirectoryName(VideoOutputPath))

                'Adding " characters to the paths to allow empty spaces in them
                InputPath = Chr(34) & InputPath & Chr(34)
                VideoOutputPath = Chr(34) & VideoOutputPath & Chr(34)
                VideoOutputPath2 = Chr(34) & VideoOutputPath2 & Chr(34)

                Dim VideoStartTimeString As String = (VideoStartReadTime).ToString.Replace(",", ".")
                Dim VideoDurationString As String = VideoReadDuration.ToString.Replace(",", ".")
                Dim VideoTo_String As String = (VideoStartReadTime + VideoAddedInitialTime + VideoReadDuration).ToString.Replace(",", ".")

                Dim AudioStartTimeString As String = (AudioStartReadTime).ToString.Replace(",", ".")
                Dim AudioTo_String As String = (AudioStartReadTime + AudioReadDuration).ToString.Replace(",", ".")
                AudioOutputInitialPadding = AudioOutputInitialPadding + VideoAddedInitialTime
                Dim AudioOutputInitialPaddingString As String = AudioOutputInitialPadding.ToString.Replace(",", ".")
                Dim AudioOutputFinalPaddingString As String = AudioOutputFinalPadding.ToString.Replace(",", ".")

                'Creating resampled file
                Dim ffmpegProcessStartInfo As New ProcessStartInfo()
                ffmpegProcessStartInfo.FileName = ffmpegPath

                If VideoStartReadTime = 0 Then
                    'ffmpegProcessStartInfo.Arguments = "-i " & InputPath & " -muxdelay 0 -map 0 -c copy -muxpreload 0" & " -t " & DurationString & " " & OutputPath
                    'ffmpegProcessStartInfo.Arguments = "-i " & InputPath & " -c copy -t " & VideoDurationString & " " & VideoOutputPath

                    ffmpegProcessStartInfo.Arguments = " -to " & VideoTo_String & " -i " & InputPath & " -c copy " & VideoOutputPath

                Else
                    'N.B. For A/V sync to work well, the -ss argument needs to be an input argument (i.e. specified before -i). Cf https://ffmpeg.org/ffmpeg.html section on "-ss position (input/output)"
                    'ffmpegProcessStartInfo.Arguments = "-ss " & StartTimeString & " -i " & InputPath & " -c copy -t " & DurationString & " " & OutputPath
                    'ffmpegProcessStartInfo.Arguments = "-ss " & StartTimeString & " -i " & InputPath & " -muxdelay 0 -map 0 -c copy -muxpreload 0 -t " & DurationString & " " & OutputPath
                    'ffmpegProcessStartInfo.Arguments = "-ss " & StartTimeString & " -to " & To_String & " -i " & InputPath & " -c copy " & OutputPath

                    'ffmpegProcessStartInfo.Arguments = "-ss " & VideoStartTimeString & " -to " & VideoTo_String & " -i " & InputPath & " -muxdelay 0 -muxpreload 0 -c copy " & VideoOutputPath

                    ffmpegProcessStartInfo.Arguments = "-ss " & VideoStartTimeString & " -to " & VideoTo_String & " -i " & InputPath & " -c copy " & VideoOutputPath

                    'ffmpegProcessStartInfo.Arguments = "-i " & InputPath & " -ss " & VideoStartTimeString & " -to " & VideoTo_String & " -c copy " & VideoOutputPath

                    'ffmpegProcessStartInfo.Arguments = "-ss " & VideoInitialSeekTime & " -i " & InputPath & "-ss " & VideoStartTimeString & " -t " & VideoDurationString & " -muxdelay 0 -muxpreload 0 -c copy " & VideoOutputPath
                    'ffmpegProcessStartInfo.Arguments = "-ss " & VideoInitialSeekTimeString & " -i " & InputPath & " -ss " & VideoStartTimeString & " -t " & VideoDurationString & " -muxdelay 0 -muxpreload 0 -c copy " & VideoOutputPath

                End If

                If ShowProcessWindow_CheckBox.Checked = False Then
                    ffmpegProcessStartInfo.CreateNoWindow = True
                End If

                Dim sp = Process.Start(ffmpegProcessStartInfo)
                sp.WaitForExit()

                If sp.ExitCode = 1 Then
                    FailedItems.Add(InputPath & vbTab & VideoOutputPath & vbTab & VideoStartTimeString & vbTab & VideoDurationString)
                End If

                sp.Close()

                If AudioInputFile <> "" Then

                    Dim ffmpegProcessStartInfo3 As New ProcessStartInfo()
                    ffmpegProcessStartInfo3.FileName = ffmpegPath

                    '-ss 1 -t 8 -i D:\Eikholt\Testvideos\Noise.wav -filter_complex "[0:a]afade=duration=0.5,areverse,afade=duration=0.5,apad=pad_dur=1,areverse,apad=pad_dur=1[out_a]" -map "[out_a]" out11.wav

                    ffmpegProcessStartInfo3.Arguments = "-ss " & AudioStartTimeString & " -to " & AudioTo_String & " -i " & AudioInputFile & " -filter_complex " & Chr(34) & "[0:a]afade=duration=0.01,areverse,afade=duration=0.01,apad=pad_dur=" & AudioOutputInitialPaddingString & ",areverse,apad=pad_dur=" & AudioOutputFinalPaddingString & "[out_a]" & Chr(34) & " -map " & Chr(34) & "[out_a]" & Chr(34) & " " & AudioOutputFile

                    If ShowProcessWindow_CheckBox.Checked = False Then
                        ffmpegProcessStartInfo3.CreateNoWindow = True
                    End If

                    Dim sp3 = Process.Start(ffmpegProcessStartInfo3)
                    sp3.WaitForExit()

                    If sp3.ExitCode = 1 Then
                        FailedItems.Add(AudioInputFile & vbTab & AudioOutputFile & vbTab & AudioStartTimeString & vbTab & AudioTo_String)
                    End If

                    sp3.Close()

                    Dim ReplaceSound As Boolean = True
                    If ReplaceSound = True Then

                        Dim DumpSoundFile = ExportSoundFromVideo(VideoOutputPath, IO.Path.Combine(IO.Path.GetDirectoryName(VideoOutputPath), "TemporarySoundFiles"))

                        Dim SyncedAudioFile As String = AudioOutputFile

                        SyncSounds(DumpSoundFile, AudioOutputFile, SyncedAudioFile)

                        'Replacing the sound in the initial video

                        Dim ffmpegProcessStartInfo4 As New ProcessStartInfo()
                        ffmpegProcessStartInfo4.FileName = ffmpegPath

                        'ffmpeg -i input.mp4 -i input.mp3 -c copy -map 0:v:0 -map 1:a:0 output.mp4

                        'ffmpegProcessStartInfo4.Arguments = "-i " & VideoOutputPath & " -i " & Chr(34) & AudioOutputFile & Chr(34) & " -c:v copy -map 0:v:0 -c:a aac -b:a 192k -map 1:a:0 -shortest " & VideoOutputPath2
                        ffmpegProcessStartInfo4.Arguments = "-i " & VideoOutputPath & " -i " & Chr(34) & AudioOutputFile & Chr(34) & " -c:v copy -map 0:v:0 -c:a aac -map 1:a:0 -shortest " & VideoOutputPath2

                        If ShowProcessWindow_CheckBox.Checked = False Then
                            ffmpegProcessStartInfo4.CreateNoWindow = True
                        End If

                        Dim sp4 = Process.Start(ffmpegProcessStartInfo4)
                        sp4.WaitForExit()

                        If sp4.ExitCode = 1 Then
                            FailedItems.Add(AudioInputFile & vbTab & AudioOutputFile & vbTab & AudioStartTimeString & vbTab & AudioTo_String)
                        End If

                        sp4.Close()

                    End If

                End If


                Dim Fade As Boolean = False
                If Fade = True Then

                    'Cf. http://underpop.online.fr/f/ffmpeg/help/fade.htm.gz

                    Dim FadeDuration As Double = 0.3
                    Dim FadeOutStartString As String = Math.Max(0, VideoReadDuration - FadeDuration).ToString.Replace(",", ".")
                    Dim FadeDurationString As String = FadeDuration.ToString.Replace(",", ".")

                    Dim ffmpegProcessStartInfo2 As New ProcessStartInfo()
                    ffmpegProcessStartInfo2.FileName = ffmpegPath

                    'ffmpegProcessStartInfo2.Arguments = "-i " & OutputPath & "-vf fade=t=in:st=0:d=10,fade=t=out:st=10:d=5"

                    'ffmpegProcessStartInfo2.Arguments = "-i " & OutputPath & " -vf chromakey=#0047bb " & OutputPath2

                    'ffmpegProcessStartInfo2.Arguments = "-f lavfi -i color=c=black:s=3840x2160 -i " & OutputPath & " -shortest " &
                    '    " -filter_complex " & Chr(34) & "[1:v]chromakey=#0047bb:0.1:0.2[ckout]; [0:v][ckout]overlay[out]" & Chr(34) &
                    '    " -map " & Chr(34) & "[out]" & Chr(34) & " " & OutputPath2

                    ffmpegProcessStartInfo2.Arguments = "-f lavfi -i " & Chr(34) & "C:\Temp\1109183.jpg" & Chr(34) & " -i " & VideoOutputPath &
                        " -filter_complex " & Chr(34) & "[1:v]chromakey=#0047bb:0.1:0.2[ckout]; [0:v][ckout]overlay[out]" & Chr(34) &
                        " -map " & Chr(34) & "[out]" & Chr(34) & " " & VideoOutputPath2


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
                        FailedItems.Add(InputPath & vbTab & VideoOutputPath2 & vbTab & "Fading process failed!")
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

    Private Function ParseInputData() As List(Of Tuple(Of String, String, Double, Double, String, String, Double?, Tuple(Of Double?, Double?, Double?)))

        'Checking that the ffmpeg.exe file exists
        ffmpegPath = ffmpegPath_TextBox.Text.Trim
        If ffmpegPath.Trim = "" Then
            MsgBox("You must indicate the path to an ffmpeg.exe file!", MsgBoxStyle.Exclamation, "Video Extraction Tool")
            ffmpegPath = ""
            Return Nothing
        End If
        If IO.File.Exists(ffmpegPath) = False Then
            MsgBox("The supplied path to the ffmpeg.exe file does not exist!", MsgBoxStyle.Exclamation, "Video Extraction Tool")
            ffmpegPath = ""
            Return Nothing
        End If

        ffprobePath = ffprobePath_TextBox.Text.Trim
        If ffprobePath.Trim = "" Then
            MsgBox("You must indicate the path to an ffprobe.exe file!", MsgBoxStyle.Exclamation, "Video Extraction Tool")
            ffprobePath = ""
            Return Nothing
        End If
        If IO.File.Exists(ffprobePath) = False Then
            MsgBox("The supplied path to the ffprobe.exe file does not exist!", MsgBoxStyle.Exclamation, "Video Extraction Tool")
            ffprobePath = ""
            Return Nothing
        End If

        'Getting data
        Dim ExtractionData As New List(Of Tuple(Of String, String, Double, Double, String, String, Double?, Tuple(Of Double?, Double?, Double?)))

        Dim VideoOutputCheckList As New SortedSet(Of String)
        Dim AudioOutputCheckList As New SortedSet(Of String)

        Dim DataLines = Data_RichTextBox.Lines

        For Each DataLine In DataLines

            'Skipping empty lines
            If DataLine.Trim = "" Then Continue For

            'Checking that there are four tab-delimited items on the line
            Dim LineCols = DataLine.Trim.Split(vbTab)
            If LineCols.Length <> 4 Then
                If LineCols.Length <> 9 Then
                    MsgBox("The following line does not have four or nine tab-delimited values (as required)!" & vbCrLf & vbCrLf & DataLine, MsgBoxStyle.Exclamation, "Video Extraction Tool")
                    Return Nothing
                End If
            End If

            Dim VideoInputPath = LineCols(0).Trim
            Dim VideoOutputPath = LineCols(1).Trim
            Dim VideoStartTimeString = LineCols(2).Trim
            Dim VideoDurationString = LineCols(3).Trim
            Dim VideoStartTime As Double
            Dim VideoDuration As Double

            'Parsing and checking values
            If IO.File.Exists(VideoInputPath) = False Then
                MsgBox("The input file indicated on the following line does not exist!" & vbCrLf & vbCrLf & DataLine, MsgBoxStyle.Exclamation, "Video Extraction Tool")
                Return Nothing
            End If
            If Double.TryParse(VideoStartTimeString.Replace(",", "."), NumberStyles.Float, CultureInfo.InvariantCulture, VideoStartTime) = False Then
                MsgBox("Unable to parse the start time on the following line!" & vbCrLf & vbCrLf & DataLine, MsgBoxStyle.Exclamation, "Video Extraction Tool")
                Return Nothing
            End If
            If Double.TryParse(VideoDurationString.Replace(",", "."), NumberStyles.Float, CultureInfo.InvariantCulture, VideoDuration) = False Then
                MsgBox("Unable to parse the duration on the following line!" & vbCrLf & vbCrLf & DataLine, MsgBoxStyle.Exclamation, "Video Extraction Tool")
                Return Nothing
            End If

            'Checks that video output files are unique
            If VideoOutputCheckList.Contains(VideoOutputPath) Then
                MsgBox("The video output file indicated on the following line is listed more than once!" & vbCrLf & vbCrLf & DataLine, MsgBoxStyle.Exclamation, "Video Extraction Tool")
                Return Nothing
            Else
                VideoOutputCheckList.Add(VideoOutputPath)
            End If

            'Checks that the video output file does not exist
            If IO.File.Exists(VideoOutputPath) Then
                MsgBox("The video output file indicated on the following line already exist! Remove it and try again! (Overwriting files is not supported.)" & vbCrLf & vbCrLf & DataLine, MsgBoxStyle.Exclamation, "Video Extraction Tool")
                Return Nothing
            End If

            Dim AudioInputPath As String = ""
            Dim AudioOutputPath As String = ""
            Dim AudioStartTimeString As String = ""
            Dim AudioDurationString As String = ""
            Dim AudioOutputInitialPaddingString As String = ""
            Dim AudioOutputFinalPaddingString As String = ""
            Dim AudioStartTime As Double
            Dim AudioDuration As Double
            Dim AudioOutputInitialPadding As Double
            Dim AudioOutputFinalPadding As Double

            If LineCols.Length > 4 Then

                AudioInputPath = LineCols(4).Trim
                AudioStartTimeString = LineCols(5).Trim
                AudioDurationString = LineCols(6).Trim
                AudioOutputInitialPaddingString = LineCols(7).Trim
                AudioOutputFinalPaddingString = LineCols(8).Trim
                AudioOutputPath = IO.Path.Combine(IO.Path.GetDirectoryName(VideoOutputPath), IO.Path.GetFileNameWithoutExtension(VideoOutputPath) & ".wav")

                'Parsing and checking values
                If IO.File.Exists(AudioInputPath) = False Then
                    MsgBox("The input file indicated on the following line does not exist!" & vbCrLf & vbCrLf & DataLine, MsgBoxStyle.Exclamation, "Audio Extraction Tool")
                    Return Nothing
                End If
                If Double.TryParse(AudioStartTimeString.Replace(",", "."), NumberStyles.Float, CultureInfo.InvariantCulture, AudioStartTime) = False Then
                    MsgBox("Unable to parse the start time on the following line!" & vbCrLf & vbCrLf & DataLine, MsgBoxStyle.Exclamation, "Audio Extraction Tool")
                    Return Nothing
                End If
                If Double.TryParse(AudioDurationString.Replace(",", "."), NumberStyles.Float, CultureInfo.InvariantCulture, AudioDuration) = False Then
                    MsgBox("Unable to parse the duration on the following line!" & vbCrLf & vbCrLf & DataLine, MsgBoxStyle.Exclamation, "Audio Extraction Tool")
                    Return Nothing
                End If
                If Double.TryParse(AudioOutputInitialPaddingString.Replace(",", "."), NumberStyles.Float, CultureInfo.InvariantCulture, AudioOutputInitialPadding) = False Then
                    MsgBox("Unable to parse the audio initial padding on the following line!" & vbCrLf & vbCrLf & DataLine, MsgBoxStyle.Exclamation, "Audio Extraction Tool")
                    Return Nothing
                End If
                If Double.TryParse(AudioOutputFinalPaddingString.Replace(",", "."), NumberStyles.Float, CultureInfo.InvariantCulture, AudioOutputFinalPadding) = False Then
                    MsgBox("Unable to parse the audio final padding on the following line!" & vbCrLf & vbCrLf & DataLine, MsgBoxStyle.Exclamation, "Audio Extraction Tool")
                    Return Nothing
                End If

                'Checks that Audio output files are unique
                If AudioOutputCheckList.Contains(AudioOutputPath) Then
                    MsgBox("The audio output file indicated on the following line is listed more than once!" & vbCrLf & vbCrLf & DataLine, MsgBoxStyle.Exclamation, "Audio Extraction Tool")
                    Return Nothing
                Else
                    AudioOutputCheckList.Add(AudioOutputPath)
                End If

                'Checks that the Audio output file does not exist
                If IO.File.Exists(AudioOutputPath) Then
                    MsgBox("The output file indicated on the following line already exist! Remove it and try again! (Overwriting files is not supported.)" & vbCrLf & vbCrLf & DataLine, MsgBoxStyle.Exclamation, "Audio Extraction Tool")
                    Return Nothing
                End If


            End If

            ExtractionData.Add(New Tuple(Of String, String, Double, Double, String, String, Double?, Tuple(Of Double?, Double?, Double?))(VideoInputPath, VideoOutputPath, VideoStartTime, VideoDuration, AudioInputPath, AudioOutputPath, AudioStartTime, New Tuple(Of Double?, Double?, Double?)(AudioDuration, AudioOutputInitialPadding, AudioOutputFinalPadding)))

        Next

        Return ExtractionData

    End Function

    Private Function GetKeyFrameTime(ByVal VideoStartReadTime As Double, ByVal InputPath As String, ByVal OutputFolder As String) As Double

        InputPath = Chr(34) & InputPath & Chr(34)

        ' ffprobe -v error -select_streams v:0 -skip_frame nokey -show_entries frame=pts_time -read_intervals 82%+#1 -of csv=print_section=0 "O:\34_SwEikholt\Steg 2. Inspelning\230517_speaker4\speaker4_video_main\speaker4_part7_main_P1012300.MP4" > D:\Eikholt\Testvideos\Output2\keyframes4.txt

        Dim VideoStartTimeString As String = (VideoStartReadTime).ToString.Replace(",", ".")

        Dim ffmpegProcessStartInfo As New ProcessStartInfo()
        ffmpegProcessStartInfo.RedirectStandardError = True
        ffmpegProcessStartInfo.RedirectStandardInput = True
        ffmpegProcessStartInfo.RedirectStandardOutput = True
        ffmpegProcessStartInfo.FileName = ffprobePath

        ffmpegProcessStartInfo.Arguments = "-v error -select_streams v:0 -skip_frame nokey -show_entries frame=pts_time -read_intervals " & VideoStartTimeString & "%+#1 -of csv=print_section=0 " & InputPath
        'ffmpegProcessStartInfo.Arguments = "-v error -select_streams v:0 -skip_frame nokey -show_entries frame=pts_time -read_intervals " & VideoStartTimeString & "%+#100 -of csv=print_section=0 " & InputPath

        If ShowProcessWindow_CheckBox.Checked = False Then
            ffmpegProcessStartInfo.CreateNoWindow = True
        End If

        Dim sp = Process.Start(ffmpegProcessStartInfo)
        sp.WaitForExit()

        Dim output As String = sp.StandardOutput.ReadToEnd()
        Dim SError As String = sp.StandardError.ReadToEnd()

        If sp.ExitCode = 1 Then
            MsgBox("Unable to get the key frame time from the video " & InputPath, MsgBoxStyle.Exclamation, "Video Extraction Tool")
            sp.Close()
            Return 0
        End If

        sp.Close()

        'Reading the value from the textfile
        'Dim ReturnValueString = output.Trim(vbCrLf).Split(vbCrLf)(0).Trim.TrimEnd(",")
        Dim ReturnValueString = output.Trim(vbCrLf).Trim.TrimEnd(",")
        Dim ReturnValue As Double
        If Double.TryParse(ReturnValueString.Replace(",", "."), NumberStyles.Float, CultureInfo.InvariantCulture, ReturnValue) = False Then
            MsgBox("Unable to parse key frame time!", MsgBoxStyle.Exclamation, "Video Extraction Tool")
            Return 0
        End If

        Return ReturnValue

    End Function

    Private Function ExportSoundFromVideo(ByVal VideoFile As String, ByVal OutputFolder As String, Optional ByVal OutputFilenameSuffix As String = "") As String

        IO.Directory.CreateDirectory(OutputFolder.Trim(Chr(34)))

        Dim OutputFile As String = IO.Path.Combine(OutputFolder.Trim(Chr(34)), IO.Path.GetFileNameWithoutExtension(VideoFile.Trim(Chr(34))) & OutputFilenameSuffix & ".wav")

        'VideoFile = Chr(34) & VideoFile & Chr(34)
        Dim OutputFile2 = Chr(34) & OutputFile & Chr(34)

        Dim ffmpegProcessStartInfo As New ProcessStartInfo()
        ffmpegProcessStartInfo.RedirectStandardError = True
        ffmpegProcessStartInfo.RedirectStandardInput = True
        ffmpegProcessStartInfo.RedirectStandardOutput = True
        ffmpegProcessStartInfo.FileName = ffmpegPath

        ffmpegProcessStartInfo.Arguments = "-i " & VideoFile & " " & OutputFile2

        If ShowProcessWindow_CheckBox.Checked = False Then
            ffmpegProcessStartInfo.CreateNoWindow = True
        End If

        Dim sp = Process.Start(ffmpegProcessStartInfo)
        sp.WaitForExit()

        Dim output As String = sp.StandardOutput.ReadToEnd()
        Dim SError As String = sp.StandardError.ReadToEnd()

        If sp.ExitCode = 1 Then
            MsgBox("Unable to export the sound from video file " & VideoFile & " to wave file " & OutputFile, MsgBoxStyle.Exclamation, "Video Extraction Tool")
            sp.Close()
            Return ""
        End If

        sp.Close()

        Return OutputFile

    End Function

    Private Function SyncSounds(ByVal VideoDumpFile As String, ByVal NewSoundFile As String, ByVal SyncedSoundFile As String) As Boolean

        Dim VideoDump As SpeechTestFramework.Audio.Sound = SpeechTestFramework.Audio.AudioIOs.LoadWaveFile(VideoDumpFile)

        Dim NewSound As SpeechTestFramework.Audio.Sound = SpeechTestFramework.Audio.AudioIOs.LoadWaveFile(NewSoundFile)

        SpeechTestFramework.Audio.DSP.MaxAmplitudeNormalizeSection(VideoDump)
        SpeechTestFramework.Audio.DSP.MaxAmplitudeNormalizeSection(NewSound)

        Dim DumpEnvelope = SpeechTestFramework.Audio.DSP.GetEnvelope(VideoDump, 1, Math.Round(VideoDump.WaveFormat.SampleRate / 100))
        Dim NewSoundEnvelope = SpeechTestFramework.Audio.DSP.GetEnvelope(NewSound, 1, Math.Round(NewSound.WaveFormat.SampleRate / 100))

        'replacing negative infinity values
        Dim NewSoundMin As Double = Double.PositiveInfinity
        For Each value In NewSoundEnvelope
            If Double.IsNegativeInfinity(value) = False Then
                NewSoundMin = Math.Min(NewSoundMin, value)
            End If
        Next
        For n = 0 To NewSoundEnvelope.Count - 1
            If Double.IsNegativeInfinity(NewSoundEnvelope(n)) Then NewSoundEnvelope(n) = NewSoundMin
        Next

        Dim DumpEnvelopeMin As Double = Double.PositiveInfinity
        For Each value In DumpEnvelope
            If Double.IsNegativeInfinity(value) = False Then
                DumpEnvelopeMin = Math.Min(DumpEnvelopeMin, value)
            End If
        Next
        For n = 0 To DumpEnvelope.Count - 1
            If Double.IsNegativeInfinity(DumpEnvelope(n)) Then DumpEnvelope(n) = DumpEnvelopeMin
        Next

        'Shifting
        For n = 0 To DumpEnvelope.Count - 1
            DumpEnvelope(n) -= DumpEnvelopeMin
        Next
        For n = 0 To NewSoundEnvelope.Count - 1
            NewSoundEnvelope(n) -= NewSoundMin
        Next

        'Normalizing (into Lists of Single)
        Dim DumpEnvelopeSingle As New List(Of Single)
        Dim NewSoundEnvelopeSingle As New List(Of Single)
        Dim DumpEnvelopeMax As Double = DumpEnvelope.Max
        Dim NewSoundMax As Double = NewSoundEnvelope.Max
        For n = 0 To DumpEnvelope.Count - 1
            DumpEnvelopeSingle.Add(DumpEnvelope(n) / DumpEnvelopeMax)
        Next
        For n = 0 To NewSoundEnvelope.Count - 1
            NewSoundEnvelopeSingle.Add(NewSoundEnvelope(n) / NewSoundMax)
        Next

        'Padding with zeros
        For i = 0 To NewSoundEnvelope.Count - 1
            DumpEnvelopeSingle.Insert(0, 0)
            DumpEnvelopeSingle.Add(0)
        Next

        Dim ResultList As New List(Of Double)
        Dim NewSoundArray As Single() = NewSoundEnvelopeSingle.ToArray()
        For i = 0 To DumpEnvelopeSingle.Count - NewSoundArray.Count - 1
            ResultList.Add(SpeechTestFramework.Utils.PearsonsCorrelation(DumpEnvelopeSingle.GetRange(i, NewSoundArray.Length).ToArray, NewSoundArray))
        Next

        Dim IndexOfMax = ResultList.IndexOf(ResultList.Max)

        Dim Shift = IndexOfMax - NewSoundArray.Count
        Dim TimeShift = Shift * 0.01

        'Reloading the sound
        Dim NewSoundReloaded As SpeechTestFramework.Audio.Sound = SpeechTestFramework.Audio.AudioIOs.LoadWaveFile(NewSoundFile)
        If TimeShift <> 0 Then
            If TimeShift < 0 Then
                SpeechTestFramework.Audio.DSP.CropSection(NewSoundReloaded, Math.Round(-TimeShift * NewSoundReloaded.WaveFormat.SampleRate))
            Else
                Dim SilenceArray(Math.Round(TimeShift * NewSoundReloaded.WaveFormat.SampleRate) - 1) As Single
                Dim NewArray = SilenceArray.ToList
                Dim CurrentArray = NewSoundReloaded.WaveData.SampleData(1)
                NewArray.AddRange(CurrentArray)
                NewSoundReloaded.WaveData.SampleData = NewArray.ToArray
            End If
        End If

        'Saving the synced sound
        'Here we should store the sync data in the SMA chunk!
        'NewSoundReloaded.SMA.AddChannelData()

        NewSoundReloaded.RemoveUnparsedWaveChunks()

        NewSoundReloaded.WriteWaveFile(SyncedSoundFile)

        Return True

    End Function

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