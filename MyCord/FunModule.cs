using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.Modules;

namespace MyCord
{
    class FunModule : IModule
    {
        private ModuleManager _manager;

        void IModule.Install(ModuleManager manager)
        {
            _manager = manager;

            _manager.CreateCommands("", cgb =>
            {
                cgb.CreateCommand("dance")
                .Parameter("reps", ParameterType.Required)
                .Do(async (e) =>
                {
                    for (int i = 0; i <= int.Parse(e.Args[0]); i++)
                    {
                        await Task.Delay(500);
                        await e.Message.Edit(@"\\(^_^)\");
                        await Task.Delay(500);
                        await e.Message.Edit(@"|(^_^)|");
                        await Task.Delay(500);
                        await e.Message.Edit(@"/(^_^)/");
                    }
                });
            });
        }
    }
}
