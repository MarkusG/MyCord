using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.Modules;
using Newtonsoft.Json;

namespace MyCord
{
    class Program
    {
        static void Main(string[] args) => new Program().Start(args);

        private DiscordClient _client;
        private Config _settings;

        public void Start(string[] args)
        {
            Console.Title = "MyCord";
            _settings = UseConfig();

            _client = new DiscordClient(x =>
            {
                x.AppName = "MyCord";
                x.MessageCacheSize = 0;
                x.LogHandler = Log;
                x.LogLevel = LogSeverity.Info;
            });

            _client.UsingCommands(x =>
            {
                x.IsSelfBot = true;
                x.ErrorHandler = CommandError;
                x.AllowMentionPrefix = _settings.MentionPrefix;
                x.CustomPrefixHandler = (m) =>
                {
                    if (m.RawText.StartsWith(_settings.Prefix))
                        return _settings.Prefix.Length;
                    return -1;
                };
            });

            _client.UsingModules();

            _client.AddModule<UtilityModule>("Utility Module", ModuleFilter.None);
            _client.AddModule<TagModule>("Tag Module", ModuleFilter.None);
            _client.AddModule<FunModule>("Fun Module", ModuleFilter.None);

            _client.ExecuteAndWait(async () =>
            {
                await _client.Connect(_settings.Token, TokenType.User);
            });
        }

        public Config UseConfig()
        {
            Config result;
            string loadConfig;

            // Try loading the settings from a config file. If none exists, create one.
            try { loadConfig = File.ReadAllText("config.json"); }
            catch (FileNotFoundException)
            {
                File.Create("config.json");
                loadConfig = "";
            }

            // Load the config into a new Config object.
            result = JsonConvert.DeserializeObject<Config>(loadConfig);

            // If the new object is null, ask for the user's desired token and prefix character via console.
            if (result == null)
            {
                result = new Config();

                Console.WriteLine("Enter your bot's token (Control+Shift+I in Discord and type localStorage.token): ");
                result.Token = Console.ReadLine();
                Console.WriteLine("Enter desired command prefix: ");
                result.Prefix = Console.ReadLine();
                Console.WriteLine("Allow mentioning yourself as a substitute for a command prefix? (y/n, leave blank for no): ");
                char input = Console.ReadLine().ToLower()[0];
                switch (input)
                {
                    case 'y': result.MentionPrefix = true; break;
                    case 'n': result.MentionPrefix = false; break;
                    default: result.MentionPrefix = false; break;
                }
            }

            // Save newly created config to the config file.
            try
            {
                string saveSettings = JsonConvert.SerializeObject(result);
                File.WriteAllText("config.json", saveSettings);
            }
            catch (IOException)
            {
                Console.WriteLine("Please restart the bot.");
                Console.ReadKey();
                Environment.Exit(0);
            }

            return result;
        }

        public void Log(object sender, LogMessageEventArgs e)
        {
            switch (e.Severity)
            {
                case LogSeverity.Error: Console.ForegroundColor = ConsoleColor.Red; break;
                case LogSeverity.Warning: Console.ForegroundColor = ConsoleColor.Yellow; break;
                case LogSeverity.Debug: Console.ForegroundColor = ConsoleColor.Green; break;
            }

            Console.WriteLine($"[{DateTime.Now}] [{e.Severity}] [{e.Source}] {e.Message}");
            Console.ForegroundColor = ConsoleColor.Gray;
        }
        public async void CommandError(object sender, CommandErrorEventArgs e)
        {
            await e.Message.Edit($":no_entry: :no_entry: :no_entry: :no_entry: :no_entry: :no_entry: :no_entry: :no_entry: :no_entry: :no_entry:\n```{e.Exception}```\n:no_entry: :no_entry: :no_entry: :no_entry: :no_entry: :no_entry: :no_entry: :no_entry: :no_entry: :no_entry: ");
        }
    }

    class Config
    {
        public string Token { get; set; }
        public string Prefix { get; set; }
        public bool MentionPrefix { get; set; }
    }
}
