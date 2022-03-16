using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Evolution_Flips_Bot.Functions;
using Evolution_Flips_Bot.Model;

namespace Evolution_Flips_Bot.Modules
{
    public class Rentals : ModuleBase<SocketCommandContext>
    {
        [Command("rental")]
        public async Task Rental()
        {
            var dmChannel = await Context.User.GetOrCreateDMChannelAsync();
            await dmChannel.SendMessageAsync(
                "**Bot Rentals**\n\n" +
                "-rental add {Bot} {Price in USD} {PaymentMethod} {RenterDiscordId}\n" +
                "-rental delete {ID}\n" +
                "-rental get {ID}\n" + false);
        }

        [Command("rental add")]
        public async Task AddRental(string bot, int price, string paymentMethod, string renterDiscordId)
        {
            var dmChannel = await Context.User.GetOrCreateDMChannelAsync();

            var id = Database.AddRentalAsync(Context.User.Id.ToString(), bot, price, paymentMethod, renterDiscordId);

            if (id == -1)
            {
                await dmChannel.SendMessageAsync("Failed logging new rental!", false);
                return;
            }

            var successEmbed = CustomEmbedBuilder.BuildRentalEmbed(new Rental
            {
                Bot = bot,
                Price = price,
                PaymentMethod = paymentMethod,
                Id = (int)id,
                RenterDiscordId = renterDiscordId
            }, $"Successfully Recorded Rental ID: {id}");

            await dmChannel.SendMessageAsync("",false, embed:successEmbed);
        }

        [Command("rental get")]
        public async Task GetRental(int id)
        {
            var dmChannel = await Context.User.GetOrCreateDMChannelAsync();

            var rental = Database.GetRental(id, Context.User.Id.ToString());

            if (rental is null)
            {
                var failureEmbed = CustomEmbedBuilder.BuildFailureEmbed($"Couldn't find your rental with ID: {id}");
                await dmChannel.SendMessageAsync("", false, failureEmbed);
                return;
            }

            var successEmbed = CustomEmbedBuilder.BuildRentalEmbed(rental, $"Successfully Fetched Rental ID: {id}");
            await dmChannel.SendMessageAsync("", false, embed: successEmbed);
        }

        [Command("rental delete")]
        public async Task DeleteRental(int id)
        {
            var dmChannel = await Context.User.GetOrCreateDMChannelAsync();
            var rental = Database.DeleteRental(id, Context.User.Id.ToString());
            
            switch (rental)
            {
                case 0:
                    var failureEmbed = CustomEmbedBuilder.BuildFailureEmbed($"Couldn't find your rental with ID: {id}");
                    await dmChannel.SendMessageAsync("", false, failureEmbed);
                    return;
                case -1:
                    var failureEmbed2 = CustomEmbedBuilder.BuildFailureEmbed($"There was an error deleting the rental with ID: {id}");
                    await dmChannel.SendMessageAsync("", false, failureEmbed2);
                    return;
            }

            var successEmbed = CustomEmbedBuilder.BuildSuccessEmbed($"Successfully deleted rental ID: {id}");
            await dmChannel.SendMessageAsync("", false, embed: successEmbed);
        }


        [Command("report rentals")]
        public async Task GetAllRentals()
        {
            var dmChannel = await Context.User.GetOrCreateDMChannelAsync();

            var rentals = Database.GetAllRental(Context.User.Id.ToString());

            if (rentals is null)
            {
                var failureEmbed = CustomEmbedBuilder.BuildFailureEmbed("Failed to get summary view of all rentals!");
                await dmChannel.SendMessageAsync("", false, failureEmbed);
                return;
            }

            var embed = CustomEmbedBuilder.BuildRentalSummaryEmbed(rentals);

            await dmChannel.SendMessageAsync("", false, embed);
        }
    }
}

