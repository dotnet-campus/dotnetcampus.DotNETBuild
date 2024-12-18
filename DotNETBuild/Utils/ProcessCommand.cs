﻿using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace dotnetCampus.DotNETBuild.Utils
{
    public static class ProcessCommand
    {
        public static string ToArgumentPath(string filePath)
        {
            if (filePath.StartsWith("\""))
            {
                return filePath;
            }

            return $"\"{filePath}\"";
        }

        /// <summary>
        /// 运行命令，没有成功抛异常
        /// </summary>
        /// <param name="exeName"></param>
        /// <param name="arguments"></param>
        /// <param name="workingDirectory"></param>
        public static void RunCommand(string exeName, string arguments, string workingDirectory = "")
        {
            var (success, output) = ExecuteCommand(exeName, arguments, workingDirectory);

            if (!success)
            {
                throw new ArgumentException(output);
            }
        }

        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="exeName"></param>
        /// <param name="arguments"></param>
        /// <param name="workingDirectory"></param>
        /// <returns></returns>
        public static (bool success, string output) ExecuteCommand(string exeName, string arguments, string workingDirectory = "")
        {
            if (string.IsNullOrEmpty(workingDirectory))
            {
                workingDirectory = Environment.CurrentDirectory;
            }

            var processStartInfo = new ProcessStartInfo
            {
                WorkingDirectory = workingDirectory,
                FileName = exeName,

                Arguments = arguments,
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardOutput = true,
            };
            return ExecuteCommand(processStartInfo);
        }

        /// <inheritdoc cref="ExecuteCommand(string,string,string)"/>
        public static (bool success, string output) ExecuteCommand(ProcessStartInfo processStartInfo)
        {
            var process = Process.Start(processStartInfo);

            if (process is null)
            {
                return (false, "Process.Start return null.");
            }

            var output = process.StandardOutput.ReadToEnd();
            bool success = true;
            if (process.HasExited)
            {
                success = process.ExitCode == 0;
            }

            return (success, output);
        }

        /// <inheritdoc cref="ExecuteCommand(string,string,string)"/>
        public static async Task<string> ExecuteCommandAsync(string exeName, string arguments)
        {
            var task = Task.Run(() => ExecuteCommand(exeName, arguments));
            await Task.WhenAny(task, Task.Delay(TimeSpan.FromMinutes(1)));
            if (task.IsCompleted)
            {
                return task.Result.output;
            }

            return "";
        }
    }
}