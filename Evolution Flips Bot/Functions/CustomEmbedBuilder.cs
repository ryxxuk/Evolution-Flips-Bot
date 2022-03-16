using System;
using System.Collections.Generic;
using System.Linq;
using Discord;
using Evolution_Flips_Bot.Model;

namespace Evolution_Flips_Bot.Functions
{
    public class CustomEmbedBuilder
    {
        public static Embed BuildRentalEmbed(Rental rental, string title)
        {
            var embedBuilder = new EmbedBuilder();
            var embed = embedBuilder
                .WithAuthor(author =>
                {
                    author.IconUrl =
                        "https://images-ext-2.discordapp.net/external/CdSRQnRzw1oJlMT2hfdeu4WcKM81G2TbU7FpsFtPSZc/https/pbs.twimg.com/profile_images/1356678566369517573/F1zym1CO_normal.jpg";
                    author.Name = "Evolution Inventory Manager";
                })
                .WithFooter("Evolution Flips")
                .WithThumbnailUrl("")
                .WithColor(new Color(80, 5, 236))
                .WithTitle(title)
                .WithFields(new EmbedFieldBuilder
                {
                    Name = "Bot",
                    Value = rental.Bot,
                    IsInline = true
                })
                .WithFields(new EmbedFieldBuilder
                {
                    Name = "Price",
                    Value = $"${rental.Price}",
                    IsInline = true
                })
                .WithFields(new EmbedFieldBuilder
                {
                    Name = "Payment Method",
                    Value = rental.PaymentMethod,
                    IsInline = false
                })
                .WithFields(new EmbedFieldBuilder
                {
                    Name = "Renter",
                    Value = $"<@{rental.RenterDiscordId}>",
                    IsInline = true
                })
                .WithFooter(new EmbedFooterBuilder
                {
                    Text = $"Rental Submitted on {rental.CreatedAt}"
                })
                .Build();
            return embed;
        }

        public static Embed BuildRentalSummaryEmbed(Dictionary<string, int> rentals)
        {
            var summary = rentals.Aggregate("", (current, rental) => current + $"**{rental.Key}**: ${rental.Value} \n");

            var embedBuilder = new EmbedBuilder();
            var embed = embedBuilder
                .WithAuthor(author =>
                {
                    author.IconUrl =
                        "https://images-ext-2.discordapp.net/external/CdSRQnRzw1oJlMT2hfdeu4WcKM81G2TbU7FpsFtPSZc/https/pbs.twimg.com/profile_images/1356678566369517573/F1zym1CO_normal.jpg";
                    author.Name = "Evolution Inventory Manager";
                })
                .WithFooter("Evolution Flips")
                .WithThumbnailUrl("")
                .WithColor(new Color(80, 5, 236))
                .WithTitle($"Rental Summary")
                .WithDescription(summary)
                .WithCurrentTimestamp()
                .Build();
            return embed;
        }

        public static Embed BuildSuccessEmbed(string successMessage)
        {
            var embedBuilder = new EmbedBuilder();
            var embed = embedBuilder
                .WithAuthor(author =>
                {
                    author.IconUrl =
                        "https://images-ext-2.discordapp.net/external/CdSRQnRzw1oJlMT2hfdeu4WcKM81G2TbU7FpsFtPSZc/https/pbs.twimg.com/profile_images/1356678566369517573/F1zym1CO_normal.jpg";
                    author.Name = "Evolution Inventory Manager";
                })
                .WithFooter("Evolution Flips")
                .WithThumbnailUrl("")
                .WithColor(Color.DarkGreen)
                .WithTitle($"Action Successful!")
                .WithDescription(successMessage)
                .WithCurrentTimestamp()
                .Build();
            return embed;
        }

        public static Embed BuildFailureEmbed(string error)
        {
            var embedBuilder = new EmbedBuilder();
            var embed = embedBuilder
                .WithAuthor(author =>
                {
                    author.IconUrl =
                        "https://images-ext-2.discordapp.net/external/CdSRQnRzw1oJlMT2hfdeu4WcKM81G2TbU7FpsFtPSZc/https/pbs.twimg.com/profile_images/1356678566369517573/F1zym1CO_normal.jpg";
                    author.Name = "Evolution Inventory Manager";
                })
                .WithFooter("Evolution Flips")
                .WithThumbnailUrl("")
                .WithColor(Color.Red)
                .WithTitle($"Action failed!")
                .WithDescription(error)
                .WithCurrentTimestamp()
                .Build();
            return embed;
        }

        public static Embed BuildHelpEmbed()
        {
            var embedBuilder = new EmbedBuilder();
            var embed = embedBuilder
                .WithAuthor(author =>
                {
                    author.IconUrl =
                        "https://images-ext-2.discordapp.net/external/CdSRQnRzw1oJlMT2hfdeu4WcKM81G2TbU7FpsFtPSZc/https/pbs.twimg.com/profile_images/1356678566369517573/F1zym1CO_normal.jpg";
                    author.Name = "Evolution Inventory Manager";
                })
                .WithFooter("Evolution Flips")
                .WithThumbnailUrl("")
                .WithColor(new Color(80, 5, 236))
                .WithTitle($"Command List")
                .WithDescription(
                    "__**Notes:**__\n" +
                    "!! DO NOT ENTER ANY PASSWORDS INTO THE BOT !!\n" +
                    "For prices, only enter the number of USD. Do not add a currency symbol.\n" +
                    "For Discord Id's, right click the user and 'Copy ID'\n"+
                    "\n" +
                    "**Bot Sales** \n" +
                    "-sale add {Bot} {Price in USD} {PaymentMethod} {Marketplace} {SellerDiscordId} {MMDiscordId}\n" +
                    "-sale delete {ID}\n" +
                    "-sale get {ID}\n" +
                    "\n" +
                    "**Bot Purchases**\n" +
                    "-buy add {Bot} {Price in USD} {paymentMethod} {marketplace} {sellerDiscordId} {MMDiscordId}\n " +
                    "-buy delete {ID}\n" +
                    "-buy get {ID}\n" +
                    "\n" +
                    "**Bot Rentals** \n" +
                    "-rental add {Bot} {Price in USD} {PaymentMethod} {RenterDiscordId} \n" +
                    "-rental delete {ID}\n" +
                    "-rental get {ID}\n" +
                    "\n" +
                    "**Burners** \n" +
                    "-burner add {Name} {Bots} {Email} {Password Hint} (Bots: e.g. polaris,mek,tsb) no spaces} *NOT IMPLEMENTED YET*\n" +
                    "-burner delete {ID} *NOT IMPLEMENTED YET*\n" +
                    "-burner get {ID} *NOT IMPLEMENTED YET*\n" +
                    "-burner edit {ID} *NOT IMPLEMENTED YET*\n" +
                    "-burner summary *NOT IMPLEMENTED YET*\n" +
                    "\n" +
                    "**Profit Reports**\n" +
                    "-report rentals *BETA*\n" +
                    "-report purchases *NOT IMPLEMENTED YET*\n" +
                    "-report sales *NOT IMPLEMENTED YET*\n" +
                    "-report profit *NOT IMPLEMENTED YET*\n" +
                    "-report overall *NOT IMPLEMENTED YET*\n"
                    )
                .WithCurrentTimestamp()
                .Build();
            return embed;
        }

        public static Embed BuildSaleEmbed(BotSale sale, string title)
        {
            var embedBuilder = new EmbedBuilder();
            var embed = embedBuilder
                .WithAuthor(author =>
                {
                    author.IconUrl =
                        "https://images-ext-2.discordapp.net/external/CdSRQnRzw1oJlMT2hfdeu4WcKM81G2TbU7FpsFtPSZc/https/pbs.twimg.com/profile_images/1356678566369517573/F1zym1CO_normal.jpg";
                    author.Name = "Evolution Inventory Manager";
                })
                .WithFooter("Evolution Flips")
                .WithThumbnailUrl("")
                .WithColor(new Color(80, 5, 236))
                .WithTitle(title)
                .WithFields(new EmbedFieldBuilder
                {
                    Name = "Bot",
                    Value = sale.Bot,
                    IsInline = true
                })
                .WithFields(new EmbedFieldBuilder
                {
                    Name = "Price",
                    Value = $"${sale.Price}",
                    IsInline = true
                })
                .WithFields(new EmbedFieldBuilder
                {
                    Name = "Payment Method",
                    Value = sale.PaymentMethod,
                    IsInline = false
                })
                .WithFields(new EmbedFieldBuilder
                {
                    Name = "Sold via",
                    Value = sale.Marketplace,
                    IsInline = true
                })
                .WithFields(new EmbedFieldBuilder
                {
                    Name = "Sold to",
                    Value = $"<@{sale.SellerDiscordId}>",
                    IsInline = true
                })
                .WithFields(new EmbedFieldBuilder
                {
                    Name = "Middleman",
                    Value = $"<@{sale.MiddlemanDiscordId}>",
                    IsInline = true
                })
                .WithFooter(new EmbedFooterBuilder
                {
                    Text = $"Sale Submitted on {sale.CreatedAt}"
                })
                .Build();
            return embed;
        }

        public static Embed BuildSaleSummaryEmbed(Dictionary<string, int> rentals)
        {
            var summary = rentals.Aggregate("", (current, rental) => current + $"**{rental.Key}**: ${rental.Value} \n");

            var embedBuilder = new EmbedBuilder();
            var embed = embedBuilder
                .WithAuthor(author =>
                {
                    author.IconUrl =
                        "https://images-ext-2.discordapp.net/external/CdSRQnRzw1oJlMT2hfdeu4WcKM81G2TbU7FpsFtPSZc/https/pbs.twimg.com/profile_images/1356678566369517573/F1zym1CO_normal.jpg";
                    author.Name = "Evolution Inventory Manager";
                })
                .WithFooter("Evolution Flips")
                .WithThumbnailUrl("")
                .WithColor(new Color(80, 5, 236))
                .WithTitle($"Rental Summary")
                .WithDescription(summary)
                .WithCurrentTimestamp()
                .Build();
            return embed;
        }

        public static Embed BuildPurchaseEmbed(BotPurchase purchase, string title)
        {
            var embedBuilder = new EmbedBuilder();
            var embed = embedBuilder
                .WithAuthor(author =>
                {
                    author.IconUrl =
                        "https://images-ext-2.discordapp.net/external/CdSRQnRzw1oJlMT2hfdeu4WcKM81G2TbU7FpsFtPSZc/https/pbs.twimg.com/profile_images/1356678566369517573/F1zym1CO_normal.jpg";
                    author.Name = "Evolution Inventory Manager";
                })
                .WithFooter("Evolution Flips")
                .WithThumbnailUrl("")
                .WithColor(new Color(80, 5, 236))
                .WithTitle(title)
                .WithFields(new EmbedFieldBuilder
                {
                    Name = "Bot",
                    Value = purchase.Bot,
                    IsInline = true
                })
                .WithFields(new EmbedFieldBuilder
                {
                    Name = "Price",
                    Value = $"${purchase.Price}",
                    IsInline = true
                })
                .WithFields(new EmbedFieldBuilder
                {
                    Name = "Payment Method",
                    Value = purchase.PaymentMethod,
                    IsInline = false
                })
                .WithFields(new EmbedFieldBuilder
                {
                    Name = "Bought via",
                    Value = purchase.Marketplace,
                    IsInline = true
                })
                .WithFields(new EmbedFieldBuilder
                {
                    Name = "Bought from",
                    Value = $"<@{purchase.SellerDiscordId}>",
                    IsInline = true
                })
                .WithFields(new EmbedFieldBuilder
                {
                    Name = "Middleman",
                    Value = $"<@{purchase.MiddlemanDiscordId}>",
                    IsInline = true
                })
                .WithFooter(new EmbedFooterBuilder
                {
                    Text = $"Purchase Submitted on {purchase.CreatedAt}"
                })
                .Build();
            return embed;
        }

        public static Embed BuildPurchaseSummaryEmbed(Dictionary<string, int> rentals)
        {
            var summary = rentals.Aggregate("", (current, rental) => current + $"**{rental.Key}**: ${rental.Value} \n");

            var embedBuilder = new EmbedBuilder();
            var embed = embedBuilder
                .WithAuthor(author =>
                {
                    author.IconUrl =
                        "https://images-ext-2.discordapp.net/external/CdSRQnRzw1oJlMT2hfdeu4WcKM81G2TbU7FpsFtPSZc/https/pbs.twimg.com/profile_images/1356678566369517573/F1zym1CO_normal.jpg";
                    author.Name = "Evolution Inventory Manager";
                })
                .WithFooter("Evolution Flips")
                .WithThumbnailUrl("")
                .WithColor(new Color(80, 5, 236))
                .WithTitle($"Rental Summary")
                .WithDescription(summary)
                .WithCurrentTimestamp()
                .Build();
            return embed;
        }
    }
}
