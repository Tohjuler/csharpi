using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Net;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;

namespace csharpi_
{
    public class tohjulerAccCommands : BaseCommandModule {


        [Command("stop")]
        public async Task Stop(CommandContext ctx) {

            if (ctx.Channel.Name.Equals("acc_tohjuler_console")) {
                if (ctx.Channel.Parent.Name.Equals("Acc_tohjuler")) {
                    await ctx.Channel.DeleteAsync();
                    await ctx.Channel.Parent.DeleteAsync();
                } 
            }
            
        } 


        [Command("wakeop")]
        public async Task WakeOp(CommandContext ctx) {
            if (!(ctx.Channel.Name.Equals("acc_tohjuler_console"))) {
                var errorEmbed = new DiscordEmbedBuilder {
                    Title = "Wake On Lan",
                    Description = "Du kan ikke bruge denne command here",
                    Color = DiscordColor.Red
                };
                await ctx.Channel.SendMessageAsync(embed:errorEmbed);
                return;
            }
            
            if (!(ctx.Channel.Parent.Name.Equals("Acc_tohjuler"))) {
                var errorEmbed = new DiscordEmbedBuilder {
                    Title = "Wake On Lan",
                    Description = "Du kan ikke bruge denne command here",
                    Color = DiscordColor.Red
                };
                await ctx.Channel.SendMessageAsync(embed:errorEmbed);
                return;
            }

            if (!(ctx.Member.Id == 593362237444325391)) {
                var errorEmbed = new DiscordEmbedBuilder {
                    Title = "Wake On Lan",
                    Description = "Du kan ikke bruge denne command",
                    Color = DiscordColor.Red
                };
                await ctx.Channel.SendMessageAsync(embed:errorEmbed);
                return;
            }

            Console.WriteLine("Running...");

            wakeop();

            Console.WriteLine("Done");

            var embed = new DiscordEmbedBuilder {
                Title = "Wake On Lan",
                Description = "Pakke sendt",
                Color = DiscordColor.Blue
            };
            await ctx.Channel.SendMessageAsync(embed:embed);
        }

        public void wakeop() {
            ProcessStartInfo startInfo = new ProcessStartInfo() { FileName = "/bin/wakeonlan", Arguments = "9C:7B:EF:38:A6:78", }; 
            Process proc = new Process() { StartInfo = startInfo, };
            proc.Start();
        }

    }
}