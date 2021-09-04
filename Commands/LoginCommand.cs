using System;
using System.Collections;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;

namespace csharpi_
{
    public class LoginCommand : BaseCommandModule {

        int[] code = new int[4];
        int codeint;

        bool[] codenum = new bool[4];

        string user = "";
        string[] users = {"tohjuler"};

        [Command("login")]
        public async Task Login(CommandContext ctx, string username) {

            for (int i = 0; i < users.Length; i++) {
                if (users[i].Equals(username)) {
                    user = users[i];
                }
            }
            if (user.Equals("")) {
                var cancelEmbed = new DiscordEmbedBuilder {
                    Title = "Der er ikke en Bruger med det navn",
                    Color = DiscordColor.Red
                };
                await ctx.Channel.SendMessageAsync(embed:cancelEmbed);
                return;
            }
            
            var loginEmbed = new DiscordEmbedBuilder {
                Title = "Login: " + username,
                Description = $"{ctx.User.Mention}, Vælg en måde",
                Color = DiscordColor.Blue
            };
            loginEmbed.AddField("Vælg", "Skriv kode med Emoji tryk :capital_abcd:\nSkriv kode med Tal i chaten tryk :1234:");

            var loginMessage = await ctx.Channel.SendMessageAsync(embed:loginEmbed);

            DiscordEmoji abcd = DiscordEmoji.FromName(ctx.Client, ":capital_abcd:");
            DiscordEmoji tal = DiscordEmoji.FromName(ctx.Client, ":1234:");

            await loginMessage.CreateReactionAsync(abcd);
            await loginMessage.CreateReactionAsync(tal);

            
            var interactivity = ctx.Client.GetInteractivity();

            var reactionResult = await interactivity.WaitForReactionAsync(
                x => x.Message == loginMessage && 
                x.User == ctx.User &&
                (x.Emoji == abcd || 
                x.Emoji == tal));

            if (reactionResult.Result.Emoji == abcd) {
                var result = await loginWithEmoji(ctx, loginMessage, username);
                if (result == 0000)
                    return;
            } else if (reactionResult.Result.Emoji == tal) {
                await loginMessage.DeleteAllReactionsAsync();
                await loginWithText(ctx, loginMessage, username);
            } else {
                
                await loginMessage.DeleteReactionsEmojiAsync(reactionResult.Result.Emoji);

                var errorEmbed = new DiscordEmbedBuilder {
                    Title = "Det kan du ikke!",
                    Color = DiscordColor.Red
                };

                await ctx.Channel.SendMessageAsync(embed:errorEmbed);
                return;
            }

            switch (user)
            {
                case "tohjuler":
                    if (code[0] == 8 && code[1] == 6 && code[2] == 2 && code[3] == 4 || codeint == 8624)
                        await tohjulerAccCall(ctx);
                    else {
                        var cancelEmbed = new DiscordEmbedBuilder {
                            Title = "Forkert kode",
                            Color = DiscordColor.Red
                        };
                        await ctx.Channel.SendMessageAsync(embed:cancelEmbed);
                    }
                    break;

                    
                default:
                    var errorEmbed = new DiscordEmbedBuilder {
                        Title = "Fejl: Nothing to execute",
                        Color = DiscordColor.Red
                    };
                    await ctx.Channel.SendMessageAsync(embed:errorEmbed);
                    break;
            }
            
        } 

        private async Task reloadMessage(CommandContext context, DiscordMessage message, string username) {
            
            string num0;
            if (codenum[0])
                num0 = "*";
            else    
                num0 = " ";

            string num1;
            if (codenum[1])
                num1 = "*";
            else    
                num1 = " ";

            string num2;
            if (codenum[2])
                num2 = "*";
            else    
                num2 = " ";

            string num3;
            if (codenum[3])
                num3 = "*";
            else    
                num3 = " ";

            DiscordEmbed loginEmbed = new DiscordEmbedBuilder {
                Title = "Login: " + username,
                Description = "Kode: `" + num0 + num1 + num2 + num3 + "`",
                Color = DiscordColor.Blue,
            };

            await message.ModifyAsync(null, loginEmbed);
        }

        private async Task loginWithText(CommandContext ctx, DiscordMessage loginMessage, string username) {

            DiscordEmbed loginEmbed = new DiscordEmbedBuilder {
                Title = "Login: " + username,
                Description = "Kode: `    `\nSkriv koden i chaten",
                Color = DiscordColor.Blue
            };

            await loginMessage.ModifyAsync(null, loginEmbed);

            var interactivity = ctx.Client.GetInteractivity();

            var result = await interactivity.WaitForMessageAsync(x => x.Channel == ctx.Channel);

            await result.Result.DeleteAsync();

            if (result.Result.Content.Length == 4) {
                if (int.TryParse(result.Result.Content, out int n)) {
                    codeint = int.Parse(result.Result.Content);
                } else {
                    var cancelEmbed = new DiscordEmbedBuilder {
                        Title = "Koden er i tal",
                        Color = DiscordColor.Red
                    };
                    await ctx.Channel.SendMessageAsync(embed:cancelEmbed);
                }
            } else {
                var cancelEmbed = new DiscordEmbedBuilder {
                    Title = "Koden er på 4 tal",
                    Color = DiscordColor.Red
                };
                await ctx.Channel.SendMessageAsync(embed:cancelEmbed);
            }
        }

        private async Task<int> loginWithEmoji(CommandContext ctx, DiscordMessage loginMessage, string username) {
            await reloadEmoji(ctx, loginMessage);
            await reloadMessage(ctx, loginMessage, username);

            code[0] = await getNumber(ctx, loginMessage);
            codenum[0] = true;
            if (code[0] == 0000) {
                var cancelEmbed = new DiscordEmbedBuilder {
                    Title = "Stopped!",
                    Color = DiscordColor.Red
                };
                await ctx.Channel.SendMessageAsync(embed:cancelEmbed);
                return 0000;
            }

            await reloadMessage(ctx, loginMessage, username);

            code[1] = await getNumber(ctx, loginMessage);
            codenum[1] = true;
            if (code[1] == 0000) {
                var cancelEmbed = new DiscordEmbedBuilder {
                    Title = "Stopped!",
                    Color = DiscordColor.Red
                };
                await ctx.Channel.SendMessageAsync(embed:cancelEmbed);
                return 0000;
            }

            await reloadMessage(ctx, loginMessage, username);

            code[2] = await getNumber(ctx, loginMessage);
            codenum[2] = true;
            if (code[2] == 0000) {
                var cancelEmbed = new DiscordEmbedBuilder {
                    Title = "Stopped!",
                    Color = DiscordColor.Red
                };
                await ctx.Channel.SendMessageAsync(embed:cancelEmbed);
                return 0000;
            }

            await reloadMessage(ctx, loginMessage, username);

            code[3] = await getNumber(ctx, loginMessage);
            codenum[3] = true;
            if (code[3] == 0000) {
                var cancelEmbed = new DiscordEmbedBuilder {
                    Title = "Stopped!",
                    Color = DiscordColor.Red
                };
                await ctx.Channel.SendMessageAsync(embed:cancelEmbed);
                return 0000;
            }

            await reloadMessage(ctx, loginMessage, username);
            await loginMessage.DeleteAllReactionsAsync();

            return 1;
        }

        private async Task reloadEmoji(CommandContext ctx, DiscordMessage message) {

            DiscordEmoji number0 = DiscordEmoji.FromName(ctx.Client, ":zero:");
            DiscordEmoji number1 = DiscordEmoji.FromName(ctx.Client, ":one:");
            DiscordEmoji number2 = DiscordEmoji.FromName(ctx.Client, ":two:");
            DiscordEmoji number3 = DiscordEmoji.FromName(ctx.Client, ":three:");
            DiscordEmoji number4 = DiscordEmoji.FromName(ctx.Client, ":four:");
            DiscordEmoji number5 = DiscordEmoji.FromName(ctx.Client, ":five:");
            DiscordEmoji number6 = DiscordEmoji.FromName(ctx.Client, ":six:");
            DiscordEmoji number7 = DiscordEmoji.FromName(ctx.Client, ":seven:");
            DiscordEmoji number8 = DiscordEmoji.FromName(ctx.Client, ":eight:");
            DiscordEmoji number9 = DiscordEmoji.FromName(ctx.Client, ":nine:");
            DiscordEmoji x = DiscordEmoji.FromName(ctx.Client, ":x:");

            await message.DeleteAllReactionsAsync();

            await message.CreateReactionAsync(number0);
            await message.CreateReactionAsync(number1);
            await message.CreateReactionAsync(number2);
            await message.CreateReactionAsync(number3);
            await message.CreateReactionAsync(number4);
            await message.CreateReactionAsync(number5);
            await message.CreateReactionAsync(number6);
            await message.CreateReactionAsync(number7);
            await message.CreateReactionAsync(number8);
            await message.CreateReactionAsync(number9);
            await message.CreateReactionAsync(x);
        }

        private async Task<int> getNumber(CommandContext ctx, DiscordMessage message) {

            int result = 0;

            DiscordEmoji number0 = DiscordEmoji.FromName(ctx.Client, ":zero:");
            DiscordEmoji number1 = DiscordEmoji.FromName(ctx.Client, ":one:");
            DiscordEmoji number2 = DiscordEmoji.FromName(ctx.Client, ":two:");
            DiscordEmoji number3 = DiscordEmoji.FromName(ctx.Client, ":three:");
            DiscordEmoji number4 = DiscordEmoji.FromName(ctx.Client, ":four:");
            DiscordEmoji number5 = DiscordEmoji.FromName(ctx.Client, ":five:");
            DiscordEmoji number6 = DiscordEmoji.FromName(ctx.Client, ":six:");
            DiscordEmoji number7 = DiscordEmoji.FromName(ctx.Client, ":seven:");
            DiscordEmoji number8 = DiscordEmoji.FromName(ctx.Client, ":eight:");
            DiscordEmoji number9 = DiscordEmoji.FromName(ctx.Client, ":nine:");
            DiscordEmoji emojiX = DiscordEmoji.FromName(ctx.Client, ":x:");

            var interactivity = ctx.Client.GetInteractivity();

            var reactionResult = await interactivity.WaitForReactionAsync(
                x => x.Message == message && 
                x.User == ctx.User &&
                (x.Emoji == number0 || 
                x.Emoji == number1 ||
                x.Emoji == number2 ||
                x.Emoji == number3 ||
                x.Emoji == number4 ||
                x.Emoji == number5 ||
                x.Emoji == number6 ||
                x.Emoji == number7 ||
                x.Emoji == number8 ||
                x.Emoji == number9 ||
                x.Emoji == emojiX));

            if (reactionResult.Result.Emoji == number0) {
                result = 0;
                await reloadEmoji(ctx, message);
            } else if (reactionResult.Result.Emoji == number1) {
                result = 1;
                await reloadEmoji(ctx, message);
            } else if (reactionResult.Result.Emoji == number2) {
                result = 2;
                await reloadEmoji(ctx, message);
            } else if (reactionResult.Result.Emoji == number3) {
                result = 3;
                await reloadEmoji(ctx, message);
            } else if (reactionResult.Result.Emoji == number4) {
                result = 4;
                await reloadEmoji(ctx, message);
            } else if (reactionResult.Result.Emoji == number5) {
                result = 5;
                await reloadEmoji(ctx, message);
            } else if (reactionResult.Result.Emoji == number6) {
                result = 6;
                await reloadEmoji(ctx, message);
            } else if (reactionResult.Result.Emoji == number7) {
                result = 7;
                await reloadEmoji(ctx, message);
            } else if (reactionResult.Result.Emoji == number8) {
                result = 8;
                await reloadEmoji(ctx, message);
            } else if (reactionResult.Result.Emoji == number9) {
                result = 9;
                await reloadEmoji(ctx, message);
            } else if (reactionResult.Result.Emoji == emojiX) {
                return 0000;
            } else {
                
                await message.DeleteReactionsEmojiAsync(reactionResult.Result.Emoji);

                var errorEmbed = new DiscordEmbedBuilder {
                    Title = "Det kan du ikke!",
                    Color = DiscordColor.Red
                };

                await ctx.Channel.SendMessageAsync(embed:errorEmbed);
            }

            return result;
        }

        private async Task tohjulerAccCall(CommandContext ctx) {

            DiscordChannel category = await ctx.Guild.CreateChannelCategoryAsync("Acc_tohjuler");
			DiscordChannel channel;

            try
			{
				channel = await ctx.Guild.CreateChannelAsync("Acc_tohjuler_console", ChannelType.Text, category);

			}
			catch (Exception)
			{
				DiscordEmbed error = new DiscordEmbedBuilder
				{
					Color = DiscordColor.Red,
					Description = "Error occured while creating channel, " + ctx.Member.Mention +
					              "!\nIs the channel limit reached in the server?"
				};
				await ctx.RespondAsync(embed:error);
				return;
			}

			await channel.AddOverwriteAsync(ctx.Member, Permissions.AccessChannels, Permissions.None);
            await channel.AddOverwriteAsync(ctx.Guild.EveryoneRole, Permissions.None, Permissions.AccessChannels);

			await channel.SendMessageAsync("Hej, " + ctx.Member.Mention + "!\n Velkommen til din Console");
        }

    }
}