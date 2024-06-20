using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Platform;
using System;
using System.IO;
using System.Text.Json;

namespace GameMasterHub.Infrastructure.Managers
{
    internal class AppConfiguration
    {
        public string? Token { get; set; }
    }

    public class ApplicationDataManager
    {
        private const string ConfigFileName = "application.json";
        private const string TokenKey = "Token";

        private static AppConfiguration? _config;

        static ApplicationDataManager()
        {
            _config = LoadConfig();
        }

        private static AppConfiguration? LoadConfig()
        {
            AppConfiguration? config = new AppConfiguration();

            try
            {
                if (File.Exists(ConfigFileName))
                {
                    string json = File.ReadAllText(ConfigFileName);
                    config = JsonSerializer.Deserialize<AppConfiguration>(json);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading configuration: {ex.Message}");
            }

            return config;
        }

        private static void SaveConfig()
        {
            try
            {
                string json = JsonSerializer.Serialize(_config);
                File.WriteAllText(ConfigFileName, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving configuration: {ex.Message}");
            }
        }

        public static string? GetAuthToken()
        {
            return _config?.Token;
        }

        public static void SaveAuthToken(string? token)
        {
            if (_config != null) _config.Token = token;
            SaveConfig();
        }

        public static void RemoveAuthToken()
        {
            if (_config != null) _config.Token = null;
            SaveConfig();
        }
    }
}
