using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution_Flips_Bot.Model
{
    public class Rental
    {
        public int Id { get; set; }
        public string Bot { get; set; }
        public int Price { get; set; }
        public string PaymentMethod { get; set; }
        public string RenterDiscordId { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
