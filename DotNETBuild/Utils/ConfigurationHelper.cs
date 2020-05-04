﻿using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using dotnetCampus.Configurations.Core;

namespace dotnetCampus.DotNETBuild.Utils
{
    public static class ConfigurationHelper
    {
        /// <summary>
        /// 获取当前配置
        /// <para></para>
        /// </summary>
        internal static IConfigurationRepo GetCurrentConfiguration()
        {
            var file = GetCurrentConfigurationFile();

            FileConfigurationRepo fileConfigurationRepo = ConfigurationFactory.FromFile(file.FullName);

            return fileConfigurationRepo;
        }

        public static FileInfo GetCurrentConfigurationFile()
        {
            Log.Debug("获取当前配置文件路径");
            Log.Debug($"工作路径 " + Environment.CurrentDirectory);
            string config = "build.coin";
            config = Path.GetFullPath(config);
            Log.Debug($"配置文件路径" + config);
            return new FileInfo(config);
        }

        /// <summary>
        /// 获取机器级配置文件
        /// </summary>
        public static FileInfo GetMachineConfigurationFile()
        {
            var folder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            folder = Path.Combine(folder,"dotnet campus","BuildKit");
            var file = new FileInfo(Path.Combine(folder,"configuration.coin"));
            return file;
        }
    }
}