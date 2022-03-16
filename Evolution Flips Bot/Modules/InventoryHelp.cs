using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Evolution_Flips_Bot.Functions;
using Evolution_Flips_Bot.Model;

namespace Evolution_Flips_Bot.Modules
{
    public class InventoryHelp : ModuleBase<SocketCommandContext>
    {
        [Command("help")]
        public async Task Help()
        {
            var dmChannel = await Context.User.GetOrCreateDMChannelAsync();

            var helpEmbed = CustomEmbedBuilder.BuildHelpEmbed();

            await dmChannel.SendMessageAsync("", false, helpEmbed);
        }

        [Command("deleteall")]
        public async Task DeleteAll()
        {
            var dmChannel = await Context.User.GetOrCreateDMChannelAsync();
            await dmChannel.SendMessageAsync("Buy!", false);

        }
    }
}

