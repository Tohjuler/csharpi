using System;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;

namespace csharpi_
{
    public class FunCommands : BaseCommandModule {
        [Command("ping")]
        [Description("Returns pong")]
        public async Task Ping(CommandContext ctx) {
            await ctx.Channel.SendMessageAsync("Pong").ConfigureAwait(false);
        }

        [Command("add")]
        [Description("Adds two numbers together")]
        [RequireRoles(RoleCheckMode.Any, "Ejer", "Mod")]
        public async Task Add(CommandContext ctx, 
            [Description("First Number")] int numberOne, 
            [Description("Second Number")] int numberTwo) 
        {
            await ctx.Channel
            .SendMessageAsync((numberOne + numberTwo)
            .ToString())
            .ConfigureAwait(false);
        }

        [Command("respondmessage")]
        public async Task RespondMessage(CommandContext ctx) {
            var interactivity = ctx.Client.GetInteractivity();

            var message = await interactivity.WaitForMessageAsync(x => x.Channel == ctx.Channel);

            await ctx.Channel.SendMessageAsync(message.Result.Content);
        }

        [Command("respondreaction")]
        public async Task RespondReaction(CommandContext ctx) {
            var interactivity = ctx.Client.GetInteractivity();

            var message = await interactivity.WaitForReactionAsync(x => x.Channel == ctx.Channel);

            await ctx.Channel.SendMessageAsync(message.Result.Emoji);
        }
    }
}