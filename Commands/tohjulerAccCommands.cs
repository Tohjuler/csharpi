using System.Net.Sockets;
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

            var macAddress = "9C-7B-EF-38-A6-78";                      // Our device MAC address
            macAddress = Regex.Replace(macAddress, "[-|:]", "");       // Remove any semicolons or minus characters present in our MAC address
            
            var sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp)
            {
                EnableBroadcast = true
            };
            
            int payloadIndex = 0;
            
            /* The magic packet is a broadcast frame containing anywhere within its payload 6 bytes of all 255 (FF FF FF FF FF FF in hexadecimal), followed by sixteen repetitions of the target computer's 48-bit MAC address, for a total of 102 bytes. */
            byte[] payload = new byte[1024];    // Our packet that we will be broadcasting
            
            // Add 6 bytes with value 255 (FF) in our payload
            for (int i = 0; i < 6; i++)
            {
                payload[payloadIndex] = 255;
                payloadIndex++;
            }
            
            // Repeat the device MAC address sixteen times
            for (int j = 0; j < 16; j++)
            {
                for (int k = 0; k < macAddress.Length; k += 2)
                {
                    var s = macAddress.Substring(k, 2);
                    payload[payloadIndex] = byte.Parse(s, NumberStyles.HexNumber);
                    payloadIndex++;
                }
            }
            
            sock.SendTo(payload, new IPEndPoint(IPAddress.Parse("255.255.255.255"), 0));  // Broadcast our packet
            sock.Close(10000);

            var embed = new DiscordEmbedBuilder {
                Title = "Wake On Lan",
                Description = "Pakke sendt",
                Color = DiscordColor.Blue
            };
            await ctx.Channel.SendMessageAsync(embed:embed);
        }

    }
}