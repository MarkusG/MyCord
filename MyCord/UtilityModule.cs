using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Modules;
using Discord.Commands;

namespace MyCord
{
    class UtilityModule : IModule
    {
        private DiscordClient _client;
        private ModuleManager _manager;

        void IModule.Install(ModuleManager manager)
        {
            _manager = manager;
            _client = manager.Client;

            manager.CreateCommands("", cgb =>
            {
                cgb.CreateCommand("ping")
                .Description("Pong!")
                .Do(async (e) =>
                {
                    await e.Message.Edit("Pong!");
                });

                cgb.CreateCommand("serverinfo")
                .Do(async (e) =>
                {
                    if (e.Server != null)
                    {
                        StringBuilder response = new StringBuilder();

                        response.AppendLine("```");
                        response.AppendLine($"Name: {e.Server.Name}");
                        response.AppendLine($"ID: {e.Server.Id}");
                        response.AppendLine($"Owner: {e.Server.Owner}");
                        response.AppendLine($"Region: {e.Server.Region.Name}");
                        response.AppendLine($"VIP: {e.Server.Region.Vip}");
                        response.AppendLine($"Users: {e.Server.UserCount}");
                        response.AppendLine($"Default Channel: {e.Server.DefaultChannel}");
                        response.AppendLine($"Text Channel Count: {e.Server.TextChannels.Count()}");
                        response.AppendLine($"Voice Channel Count: {e.Server.VoiceChannels.Count()}");
                        response.AppendLine($"Role Count: {e.Server.Roles.Count()}");
                        response.AppendLine($"Roles: {e.Server.Roles.Select(r => r.Name).Aggregate((current, next) => current + ", " + next)}");
                        response.AppendLine($"Icon URL: {e.Server.IconUrl ?? "This server has no icon!"}");
                        response.Append    ("```");

                        await e.Message.Edit(response.ToString());
                    }
                });
            });
        }
    }
}
