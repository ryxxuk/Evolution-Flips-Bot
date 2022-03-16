using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Evolution_Flips_Bot.Functions;
using Evolution_Flips_Bot.Model;

namespace Evolution_Flips_Bot.Modules
{
    public class BotSales : ModuleBase<SocketCommandContext>
    {
        [Command("sale")]
        public async Task BotSaleHelp()
        {
            var dmChannel = await Context.User.GetOrCreateDMChannelAsync();
            await dmChannel.SendMessageAsync(
                "**Bot Sales**\n\n" +
                "-sale add {Bot} {Price in USD} {PaymentMethod} {Marketplace} {SellerDiscordId} {MMDiscordId}\n" +
                "-sale delete {ID}\n" +
                "-sale get {ID}\n", false);
        }

        [Command("sale add")]
        public async Task AddBotSale(string bot, int price, string paymentMethod, string marketplace, string sellerDiscordId, string middlemanDiscordId)
        {
            var dmChannel = await Context.User.GetOrCreateDMChannelAsync();

            var id = Database.AddSaleAsync(Context.User.Id.ToString(), bot, price, paymentMethod, marketplace,
                sellerDiscordId, middlemanDiscordId);

            if (id == -1)
            {
                var failureEmbed = CustomEmbedBuilder.BuildFailureEmbed($"Failed logging new sale!");
                await dmChannel.SendMessageAsync("", false, failureEmbed);
                return;
            }

            var successEmbed = CustomEmbedBuilder.BuildSaleEmbed(new BotSale
            {
                Bot = bot,
                Price = price,
                PaymentMethod = paymentMethod,
                Id = (int)id,
                Marketplace = marketplace,
                SellerDiscordId = sellerDiscordId,
                MiddlemanDiscordId = middlemanDiscordId,
                CreatedAt = DateTime.Now
            }, $"Successfully Recorded Sale! ID: {id}");

            await dmChannel.SendMessageAsync("", false, embed: successEmbed);
        }

        [Command("sale delete")]
        public async Task DeleteBotSale(int id)
        {
            var dmChannel = await Context.User.GetOrCreateDMChannelAsync();
            var sale = Database.DeleteSale(id, Context.User.Id.ToString());

            switch (sale)
            {
                case 0:
                    var failureEmbed = CustomEmbedBuilder.BuildFailureEmbed($"Couldn't find your sale with ID: {id}");
                    await dmChannel.SendMessageAsync("", false, failureEmbed);
                    return;
                case -1:
                    var failureEmbed2 = CustomEmbedBuilder.BuildFailureEmbed($"There was an error deleting the sale with ID: {id}");
                    await dmChannel.SendMessageAsync("", false, failureEmbed2);
                    return;
            }

            var successEmbed = CustomEmbedBuilder.BuildSuccessEmbed($"Successfully deleted sale ID: {id}");
            await dmChannel.SendMessageAsync("", false, embed: successEmbed);
        }

        [Command("sale get")]
        public async Task GetBotSale(int id)
        {
            var dmChannel = await Context.User.GetOrCreateDMChannelAsync();

            var sale = Database.GetSale(id, Context.User.Id.ToString());

            if (sale is null)
            {
                var failureEmbed = CustomEmbedBuilder.BuildFailureEmbed($"Couldn't find your sale with ID: {id}");
                await dmChannel.SendMessageAsync("", false, failureEmbed);
                return;
            }

            var successEmbed = CustomEmbedBuilder.BuildSaleEmbed(sale, $"Successfully Fetched Sale ID: {id}");
            await dmChannel.SendMessageAsync("", false, embed: successEmbed);
        }


        [Command("sale summary")]
        public async Task GetAllBotSales()
        {
            

            var dmChannel = await Context.User.GetOrCreateDMChannelAsync();




            var successEmbed = CustomEmbedBuilder.BuildSuccessEmbed("NOT YET IMPLEMENTED");

            await dmChannel.SendMessageAsync("", false, embed: successEmbed);
        }
    }
}

