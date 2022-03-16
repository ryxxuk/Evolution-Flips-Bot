using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Evolution_Flips_Bot.Functions;
using Evolution_Flips_Bot.Model;

namespace Evolution_Flips_Bot.Modules
{
    public class BotPurchases : ModuleBase<SocketCommandContext>
    {
        [Command("buy")]
        public async Task BotPurchaseHelp()
        {
            var dmChannel = await Context.User.GetOrCreateDMChannelAsync();
            await dmChannel.SendMessageAsync(
                "**Bot Purchases**\n\n" +
                "-buy add {Bot} {Price in USD} {paymentMethod} {marketplace} {sellerDiscordId} {MMDiscordId}\n" +
                "-buy delete {ID}\n" +
                "-buy get {ID}\n", false);
        }

        [Command("buy add")]
        public async Task AddBotPurchase(string bot, int price, string paymentMethod, string marketplace, string sellerDiscordId, string middlemanDiscordId)
        {
            var dmChannel = await Context.User.GetOrCreateDMChannelAsync();

            var id = Database.AddPurchaseAsync(Context.User.Id.ToString(), bot, price, paymentMethod, marketplace,
                sellerDiscordId, middlemanDiscordId);

            if (id == -1)
            {
                var failureEmbed = CustomEmbedBuilder.BuildFailureEmbed($"Failed logging new purchase!");
                await dmChannel.SendMessageAsync("", false, failureEmbed);
                return;
            }

            var successEmbed = CustomEmbedBuilder.BuildPurchaseEmbed(new BotPurchase
            {
                Bot = bot,
                Price = price,
                PaymentMethod = paymentMethod,
                Id = (int)id,
                Marketplace = marketplace,
                SellerDiscordId = sellerDiscordId,
                MiddlemanDiscordId = middlemanDiscordId,
                CreatedAt = DateTime.Now
            }, $"Successfully Recorded Purchase ID: {id}");

            await dmChannel.SendMessageAsync("", false, embed: successEmbed);
        }

        [Command("buy delete")]
        public async Task DeleteBotPurchase(int id)
        {
            var dmChannel = await Context.User.GetOrCreateDMChannelAsync();
            var rental = Database.DeletePurchase(id, Context.User.Id.ToString());

            switch (rental)
            {
                case 0:
                    var failureEmbed = CustomEmbedBuilder.BuildFailureEmbed($"Couldn't find your purchase with ID: {id}");
                    await dmChannel.SendMessageAsync("", false, failureEmbed);
                    return;
                case -1:
                    var failureEmbed2 = CustomEmbedBuilder.BuildFailureEmbed($"There was an error deleting the purchase with ID: {id}");
                    await dmChannel.SendMessageAsync("", false, failureEmbed2);
                    return;
            }

            var successEmbed = CustomEmbedBuilder.BuildSuccessEmbed($"Successfully deleted purchase ID: {id}");
            await dmChannel.SendMessageAsync("", false, embed: successEmbed);
        }

        [Command("buy get")]
        public async Task GetBotPurchase(int id)
        {
            var dmChannel = await Context.User.GetOrCreateDMChannelAsync();

            var purchase = Database.GetPurchase(id, Context.User.Id.ToString());

            if (purchase is null)
            {
                var failureEmbed = CustomEmbedBuilder.BuildFailureEmbed($"Couldn't find your purchase with ID: {id}");
                await dmChannel.SendMessageAsync("", false, failureEmbed);
                return;
            }

            var successEmbed = CustomEmbedBuilder.BuildPurchaseEmbed(purchase, $"Successfully Fetched Purchase ID: {id}");
            await dmChannel.SendMessageAsync("", false, embed: successEmbed);
        }


        [Command("buy summary")]
        public async Task GetAllBotPurchases()
        {
            var dmChannel = await Context.User.GetOrCreateDMChannelAsync();




            var successEmbed = CustomEmbedBuilder.BuildSuccessEmbed("NOT YET IMPLEMENTED");

            await dmChannel.SendMessageAsync("", false, embed: successEmbed);
        }
    }
}

