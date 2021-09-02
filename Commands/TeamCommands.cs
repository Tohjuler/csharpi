using System;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;

namespace csharpi_
{
    public class TeamCommands : BaseCommandModule {

        [Command("join")]
        public async Task Join(CommandContext ctx) {
            var joinEmbed = new DiscordEmbedBuilder {
                Title = "Would you like to join?",
                Color = DiscordColor.Green
            };

            var joinMessage = await ctx.Channel.SendMessageAsync(embed:joinEmbed);

            var thumbsUpEmoji = DiscordEmoji.FromName(ctx.Client, ":+1:");
            var thumbsDownEmoji = DiscordEmoji.FromName(ctx.Client, ":-1:");

            await joinMessage.CreateReactionAsync(thumbsUpEmoji);
            await joinMessage.CreateReactionAsync(thumbsDownEmoji);

            var interactivity = ctx.Client.GetInteractivity();

            var reactionResult = await interactivity.WaitForReactionAsync(
                x => x.Message == joinMessage && 
                x.User == ctx.User &&
                (x.Emoji == thumbsUpEmoji || x.Emoji == thumbsDownEmoji));

            if (reactionResult.Result.Emoji == thumbsUpEmoji) {

                var role = ctx.Guild.GetRole(676337360690216982);
                await ctx.Member.GrantRoleAsync(role);

            } else if (reactionResult.Result.Emoji == thumbsDownEmoji) {

                var role = ctx.Guild.GetRole(676337360690216982);
                await ctx.Member.RevokeRoleAsync(role);

            } else {

                var errorEmbed = new DiscordEmbedBuilder {
                    Title = "You can't do that!",
                    Color = DiscordColor.Red
                };

                await ctx.Channel.SendMessageAsync(embed:errorEmbed);
            }

            await joinMessage.DeleteAsync();
        } 
    }
}