using System;
using System.Diagnostics;
using System.IO;

namespace matterai.AudioConverter
{
    public class FfmpegConverter
    {
        private readonly string _ffmpegBinPath;

        public FfmpegConverter(string ffmpegBinPath)
        {
            _ffmpegBinPath = ffmpegBinPath ?? throw new ArgumentNullException(nameof(ffmpegBinPath));
        }

        public void OggToMp3Async(string input, string output)
        {
            if (!IsFileExists(_ffmpegBinPath))
                throw new FileNotFoundException("Could not find ffmpeg bin.", _ffmpegBinPath);
            
            if (!IsFileExists(input))
                throw new FileNotFoundException("Could not find input file.", input);
            
            if (string.IsNullOrEmpty(output))
                throw new ArgumentNullException(nameof(output));
            
            if (IsFileExists(output))
                File.Delete(output);

            var p = new Process
            {
                StartInfo =
                {
                    FileName = _ffmpegBinPath,
                    
                    Arguments = $"-i {input} -ar 44100 -ac 2 -acodec libmp3lame {output}",
                    
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardInput = true,
                    RedirectStandardError = true,
                    RedirectStandardOutput = true
                },
                
                EnableRaisingEvents = true
            };

            p.ErrorDataReceived += (sender, args) =>
            {
                Console.WriteLine($"[ffmpeg] {args.Data}");
            };

            p.Start();
            p.BeginErrorReadLine();
            p.WaitForExit();
        }

        private static bool IsFileExists(string ffmpegBinPath) => File.Exists(ffmpegBinPath);
    }
}