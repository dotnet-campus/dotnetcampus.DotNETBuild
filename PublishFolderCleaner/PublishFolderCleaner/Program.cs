﻿#region

using System.IO;
using dotnetCampus.Cli;

#endregion

namespace PublishFolderCleaner;

internal class Program
{
    #region Methods

    private static void Main(string[] args)
    {
        var options = CommandLine.Parse(args).As<Options>();
        if (options.PublishSingleFile) return;
        const string libFolderName = "lib";
        var publishFolder = options.PublishFolder.Trim();
        var libFolder = Path.GetFullPath(Path.Combine(publishFolder, libFolderName));
        var tempFolder = Path.GetFullPath(Path.Combine(publishFolder, @"..", Path.GetRandomFileName()));
        if (Directory.Exists(libFolder)) Directory.Delete(libFolder, true);
        Directory.Move(publishFolder, tempFolder);
        Directory.CreateDirectory(publishFolder);
        Directory.Move(tempFolder, libFolder);

        var appHostFilePath = Path.Combine(libFolder, options.ApplicationName + ".exe");
        var newAppHostFilePath = Path.Combine(publishFolder, options.ApplicationName + ".exe");

        File.Move(appHostFilePath, newAppHostFilePath);

        AppHostPatcher.Patch(newAppHostFilePath, Path.Combine("lib", options.ApplicationName + ".dll"));
    }

    #endregion
}