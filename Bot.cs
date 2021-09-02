using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.EventArgs;
using DSharpPlus.CommandsNext;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;


namespace csharpi_
{
    public class Bot
    {
        public DiscordClient Client { get; private set; }
        public InteractivityExtension Interactivity { get; private set;}
        public CommandsNextExtension Commands {get; private set;}
        public async Task RunAsync() {
            var json = string.Empty;

            using(var fs = File.OpenRead("config.json"))
            using(var sr = new StreamReader(fs, new UTF8Encoding(false)))
                json = await sr.ReadToEndAsync();

            
            var configJson = JsonConvert.DeserializeObject<ConfigJson>(json);

            var config = new DiscordConfiguration {
                Token = configJson.Token,
                TokenType = TokenType.Bot,
                AutoReconnect = true,
                MinimumLogLevel = LogLevel.Debug
            };

            this.Client = new DiscordClient(config);

            this.Client.Ready += OnClientReady;

            this.Client.UseInteractivity(new InteractivityConfiguration {
                Timeout = TimeSpan.FromMinutes(2)
            });
            
            var commandsConfig = new CommandsNextConfiguration {
                StringPrefixes = new string[] {configJson.Prefix},
                EnableDms = false,
                EnableMentionPrefix = true,
                DmHelp = true,
            };

            Commands = this.Client.UseCommandsNext(commandsConfig);

            Commands.RegisterCommands<FunCommands>(); 
            Commands.RegisterCommands<TeamCommands>(); 

            await this.Client.ConnectAsync();

            await Task.Delay(-1);
        }

        private Task OnClientReady(DiscordClient sender, ReadyEventArgs e) {
            sender.Logger.LogInformation("Client is ready to process events.");
            
            return Task.CompletedTask;
        }
    }
}
