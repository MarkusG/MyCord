using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Discord;
using Discord.Modules;
using Discord.Commands;
using Newtonsoft.Json;

namespace MyCord
{
    class TagModule : IModule
    {
        private ModuleManager _manager;
        private Dictionary<string, string> Tags { get; set; }

        void IModule.Install(ModuleManager manager)
        {
            _manager = manager;

            Tags = LoadTags();

            manager.CreateCommands("", cgb =>
            {
                cgb.CreateCommand("tag")
                .Parameter("action", ParameterType.Required)
                .Parameter("tag", ParameterType.Optional)
                .Parameter("text", ParameterType.Optional)
                .Do(async (e) =>
                {
                    switch (e.Args[0])
                    {
                        case "list":
                            string response = "```\n";
                            foreach (var p in Tags)
                                response += $"{p.Key}\n";
                            response += "```";
                            await e.Message.Edit(response);
                            break;
                        case "add":
                            Tags.Add(e.Args[1], e.Args[2]);
                            SaveTags();
                            await e.Message.Edit(":white_check_mark:");
                            await Task.Delay(500);
                            await e.Message.Delete();
                            break;
                        case "remove":
                            Tags.Remove(e.Args[1]);
                            SaveTags();
                            await e.Message.Edit(":white_check_mark:");
                            await Task.Delay(500);
                            await e.Message.Delete();
                            break;
                        case "editkey":
                            string value = Tags[e.Args[1]];
                            Tags.Remove(e.Args[1]);
                            Tags.Add(e.Args[2], value);
                            SaveTags();
                            await e.Message.Edit(":white_check_mark:");
                            await Task.Delay(500);
                            await e.Message.Delete();
                            break;
                        case "editvalue":
                            Tags[e.Args[1]] = e.Args[2];
                            SaveTags();
                            await e.Message.Edit(":white_check_mark:");
                            await Task.Delay(500);
                            await e.Message.Delete();
                            break;
                        case "reset":
                            Tags = new Dictionary<string, string>();
                            SaveTags();
                            await e.Message.Edit(":white_check_mark:");
                            await Task.Delay(500);
                            await e.Message.Delete();
                            break;
                        default:
                            try { await e.Message.Edit(Tags[e.Args[0]]); }
                            catch (KeyNotFoundException)
                            {
                                await e.Message.Edit(":no_entry: Key not found! :no_entry:");
                                await Task.Delay(500);
                                await e.Message.Delete();
                            }
                            break;
                    }
                });
            });
        }

        Dictionary<string, string> LoadTags()
        {
            Dictionary<string, string> result;
            string loadTags = "";

            if (File.Exists("tags.json"))
                loadTags = File.ReadAllText("tags.json");
            else
                File.Create("tags.json");

            result = JsonConvert.DeserializeObject<Dictionary<string, string>>(loadTags);
            if (result == null)
                result = new Dictionary<string, string>();

            return result;
        }
        void SaveTags()
        {
            string saveTags = JsonConvert.SerializeObject(Tags, Formatting.Indented);
            File.WriteAllText("tags.json", saveTags);
        }
    }
}
