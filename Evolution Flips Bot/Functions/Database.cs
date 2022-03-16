using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Evolution_Flips_Bot.Model;
using MySql.Data.MySqlClient;

namespace Evolution_Flips_Bot.Functions
{
    public class Database
    {
        //
        // 
        // RENTAL FUNCTIONS
        //
        //

        public static long AddRentalAsync(string discordId, string bot, int price, string paymentMethod, string renterDiscordId)
        {
            var config = Functions.GetConfig();
            var connectionStr = config["db_connection"].ToString();
            
            using var conn = new MySqlConnection(connectionStr);
            try
            {
                conn.Open();

                var sqlCmd = new MySqlCommand("INSERT INTO rentals (discord_id, bot, price, payment_method, renter_discord_id) VALUES (@discord_id, @bot, @price, @payment_method, @renter_discord_id)", conn);
                sqlCmd.Parameters.AddWithValue("@discord_id", discordId);
                sqlCmd.Parameters.AddWithValue("@bot", bot);
                sqlCmd.Parameters.AddWithValue("@price", price);
                sqlCmd.Parameters.AddWithValue("@payment_method", paymentMethod);
                sqlCmd.Parameters.AddWithValue("@renter_discord_id", renterDiscordId);
                sqlCmd.Prepare();

                sqlCmd.ExecuteNonQuery();

                var id = sqlCmd.LastInsertedId;

                return id;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return -1;
            }
        }
        public static int DeleteRental(int rentalId, string discordId)
        {
            var config = Functions.GetConfig();
            var connectionStr = config["db_connection"].ToString();

            using var conn = new MySqlConnection(connectionStr);
            try
            {
                conn.Open();

                var sqlCmd = new MySqlCommand("DELETE FROM rentals WHERE rental_id=@rental_id AND discord_id=@discord_id", conn);
                sqlCmd.Parameters.AddWithValue("@rental_id", rentalId);
                sqlCmd.Parameters.AddWithValue("@discord_id", discordId);
                sqlCmd.Prepare();

                var affectedRows = sqlCmd.ExecuteNonQuery();

                return affectedRows;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return -1; // returns -1 in case of an error
            }
        }

        public static Rental GetRental(int rentalId, string discordId)
        {
            var config = Functions.GetConfig();
            var connectionStr = config["db_connection"].ToString();

            using var conn = new MySqlConnection(connectionStr);
            try
            {
                conn.Open();

                var sqlCmd = new MySqlCommand("SELECT * from rentals WHERE rental_id=@rental_id AND discord_id=@discord_id", conn);
                sqlCmd.Parameters.AddWithValue("@rental_id", rentalId);
                sqlCmd.Parameters.AddWithValue("@discord_id", discordId);
                sqlCmd.Prepare();

                var resultReader = sqlCmd.ExecuteReader();

                Rental rental = null;  // if it doesn't find one, return null

                while (resultReader.Read())
                {
                    rental = new Rental
                    {
                        Bot = resultReader["bot"].ToString(),
                        Price = Convert.ToInt32(resultReader["price"]),
                        PaymentMethod = resultReader["payment_method"].ToString(),
                        Id = Convert.ToInt32(resultReader["rental_id"]),
                        RenterDiscordId = resultReader["renter_discord_id"].ToString(),
                        CreatedAt = resultReader.GetDateTime("created_at")
                    };

                    break; // should only return first result. 
                }


                return rental;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null; // returns null in case of an error
            }
        }

        public static Dictionary<string, int> GetAllRental(string discordId)
        {
            var config = Functions.GetConfig();
            var connectionStr = config["db_connection"].ToString();

            using var conn = new MySqlConnection(connectionStr);
            try
            {
                conn.Open();

                var sqlCmd = new MySqlCommand("SELECT * from rentals WHERE discord_id=@discord_id", conn);
                sqlCmd.Parameters.AddWithValue("@discord_id", discordId);
                sqlCmd.Prepare();

                var results = sqlCmd.ExecuteReader();

                var output = new Dictionary<string, int>();

                while (results.Read())
                {
                    var bot = Convert.ToString(results["bot"])?.ToUpper();
                    var price = Convert.ToInt32(results["price"]);

                    if (output.ContainsKey(bot))
                    {
                        output[bot] += price;
                    }
                    else
                    {
                        output.Add(bot, price);
                    }
                }

                return output;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }

        //
        //
        // END OF RENTAL FUNCTIONS
        //
        //

        //
        //
        // BOT PURCHASES FUNCTIONS
        //
        //

        public static long AddPurchaseAsync(string discordId, string bot, int price, string paymentMethod, string marketplace, string sellerDiscordId, string middlemanDiscordId)
        {
            var config = Functions.GetConfig();
            var connectionStr = config["db_connection"].ToString();

            using var conn = new MySqlConnection(connectionStr);
            try
            {
                conn.Open();

                var sqlCmd = new MySqlCommand("INSERT INTO bot_purchases (discord_id, bot, price, payment_method, marketplace, seller_discord_id, middleman_discord_id) VALUES (@discord_id, @bot, @price, @payment_method, @marketplace, @seller_discord_id, @middleman_discord_id)", conn);
                sqlCmd.Parameters.AddWithValue("@discord_id", discordId);
                sqlCmd.Parameters.AddWithValue("@bot", bot);
                sqlCmd.Parameters.AddWithValue("@price", price);
                sqlCmd.Parameters.AddWithValue("@payment_method", paymentMethod);
                sqlCmd.Parameters.AddWithValue("@marketplace", marketplace);
                sqlCmd.Parameters.AddWithValue("@seller_discord_id", sellerDiscordId);
                sqlCmd.Parameters.AddWithValue("@middleman_discord_id", middlemanDiscordId);

                sqlCmd.Prepare();

                sqlCmd.ExecuteNonQuery();

                var id = sqlCmd.LastInsertedId;

                return id;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return -1;
            }
        }
        public static int DeletePurchase(int purchaseId, string discordId)
        {
            var config = Functions.GetConfig();
            var connectionStr = config["db_connection"].ToString();

            using var conn = new MySqlConnection(connectionStr);
            try
            {
                conn.Open();

                var sqlCmd = new MySqlCommand("DELETE FROM bot_purchases WHERE purchase_id=@purchase_id AND discord_id=@discord_id", conn);
                sqlCmd.Parameters.AddWithValue("@purchase_id", purchaseId);
                sqlCmd.Parameters.AddWithValue("@discord_id", discordId);
                sqlCmd.Prepare();

                var affectedRows = sqlCmd.ExecuteNonQuery();

                return affectedRows;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return -1; // returns -1 in case of an error
            }
        }

        public static BotPurchase GetPurchase(int purchaseId, string discordId)
        {
            var config = Functions.GetConfig();
            var connectionStr = config["db_connection"].ToString();

            using var conn = new MySqlConnection(connectionStr);
            try
            {
                conn.Open();

                var sqlCmd = new MySqlCommand("SELECT * from bot_purchases WHERE purchase_id=@purchase_id AND discord_id=@discord_id", conn);
                sqlCmd.Parameters.AddWithValue("@purchase_id", purchaseId);
                sqlCmd.Parameters.AddWithValue("@discord_id", discordId);
                sqlCmd.Prepare();

                var resultReader = sqlCmd.ExecuteReader();

                BotPurchase rental = null;  // if it doesn't find one, return null

                while (resultReader.Read())
                {
                    rental = new BotPurchase
                    {
                        Id = Convert.ToInt32(resultReader["purchase_id"]),
                        Bot = resultReader["bot"].ToString(),
                        Price = Convert.ToInt32(resultReader["price"]),
                        PaymentMethod = resultReader["payment_method"].ToString(),
                        Marketplace = resultReader["marketplace"].ToString(),
                        SellerDiscordId = resultReader["seller_discord_id"].ToString(),
                        MiddlemanDiscordId = resultReader["middleman_discord_id"].ToString(),
                        CreatedAt = resultReader.GetDateTime("created_at")
                    };

                    break; // should only return first result. 
                }


                return rental;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null; // returns null in case of an error
            }
        }

        public static Dictionary<string, int> GetAllPurchases(string discordId)
        {
            var config = Functions.GetConfig();
            var connectionStr = config["db_connection"].ToString();

            using var conn = new MySqlConnection(connectionStr);
            try
            {
                conn.Open();

                var sqlCmd = new MySqlCommand("SELECT * from bot_purchases WHERE discord_id=@discord_id", conn);
                sqlCmd.Parameters.AddWithValue("@discord_id", discordId);
                sqlCmd.Prepare();

                var results = sqlCmd.ExecuteReader();

                var output = new Dictionary<string, int>();

                while (results.Read())
                {
                    var bot = Convert.ToString(results["bot"])?.ToUpper();
                    var price = Convert.ToInt32(results["price"]);

                    if (output.ContainsKey(bot))
                    {
                        output[bot] += price;
                    }
                    else
                    {
                        output.Add(bot, price);
                    }
                }

                return output;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }



        //
        //
        // END OF PURCHASES FUNCTIONS
        //
        //

        //
        //
        // BOT SALES FUNCTIONS
        //
        //

        public static long AddSaleAsync(string discordId, string bot, int price, string paymentMethod, string marketplace, string sellerDiscordId, string middlemanDiscordId)
        {
            var config = Functions.GetConfig();
            var connectionStr = config["db_connection"].ToString();

            using var conn = new MySqlConnection(connectionStr);
            try
            {
                conn.Open();

                var sqlCmd = new MySqlCommand("INSERT INTO bot_sales (discord_id, bot, price, payment_method, marketplace, buyer_discord_id, middleman_discord_id) VALUES (@discord_id, @bot, @price, @payment_method, @marketplace, @buyer_discord_id, @middleman_discord_id)", conn);
                sqlCmd.Parameters.AddWithValue("@discord_id", discordId);
                sqlCmd.Parameters.AddWithValue("@bot", bot);
                sqlCmd.Parameters.AddWithValue("@price", price);
                sqlCmd.Parameters.AddWithValue("@payment_method", paymentMethod);
                sqlCmd.Parameters.AddWithValue("@marketplace", marketplace);
                sqlCmd.Parameters.AddWithValue("@buyer_discord_id", sellerDiscordId);
                sqlCmd.Parameters.AddWithValue("@middleman_discord_id", middlemanDiscordId);

                sqlCmd.Prepare();

                sqlCmd.ExecuteNonQuery();

                var id = sqlCmd.LastInsertedId;

                return id;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return -1;
            }
        }
        public static int DeleteSale(int saleId, string discordId)
        {
            var config = Functions.GetConfig();
            var connectionStr = config["db_connection"].ToString();

            using var conn = new MySqlConnection(connectionStr);
            try
            {
                conn.Open();

                var sqlCmd = new MySqlCommand("DELETE FROM bot_sales WHERE sale_id=@sale_id AND discord_id=@discord_id", conn);
                sqlCmd.Parameters.AddWithValue("@sale_id", saleId);
                sqlCmd.Parameters.AddWithValue("@discord_id", discordId);
                sqlCmd.Prepare();

                var affectedRows = sqlCmd.ExecuteNonQuery();

                return affectedRows;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return -1; // returns -1 in case of an error
            }
        }

        public static BotSale GetSale(int saleId, string discordId)
        {
            var config = Functions.GetConfig();
            var connectionStr = config["db_connection"].ToString();

            using var conn = new MySqlConnection(connectionStr);
            try
            {
                conn.Open();

                var sqlCmd = new MySqlCommand("SELECT * from bot_sales WHERE sale_id=@sale_id AND discord_id=@discord_id", conn);
                sqlCmd.Parameters.AddWithValue("@sale_id", saleId);
                sqlCmd.Parameters.AddWithValue("@discord_id", discordId);
                sqlCmd.Prepare();

                var resultReader = sqlCmd.ExecuteReader();

                BotSale sale = null;  // if it doesn't find one, return null

                while (resultReader.Read())
                {
                    sale = new BotSale
                    {
                        Id = Convert.ToInt32(resultReader["sale_id"]),
                        Bot = resultReader["bot"].ToString(),
                        Price = Convert.ToInt32(resultReader["price"]),
                        PaymentMethod = resultReader["payment_method"].ToString(),
                        Marketplace = resultReader["marketplace"].ToString(),
                        SellerDiscordId = resultReader["buyer_discord_id"].ToString(),
                        MiddlemanDiscordId = resultReader["middleman_discord_id"].ToString(),
                        CreatedAt = resultReader.GetDateTime("created_at")
                    };

                    break; // should only return first result. 
                }


                return sale;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null; // returns null in case of an error
            }
        }

        public static Dictionary<string, int> GetAllSales(string discordId)
        {
            var config = Functions.GetConfig();
            var connectionStr = config["db_connection"].ToString();

            using var conn = new MySqlConnection(connectionStr);
            try
            {
                conn.Open();

                var sqlCmd = new MySqlCommand("SELECT * from bot_sales WHERE discord_id=@discord_id", conn);
                sqlCmd.Parameters.AddWithValue("@discord_id", discordId);
                sqlCmd.Prepare();

                var results = sqlCmd.ExecuteReader();

                var output = new Dictionary<string, int>();

                while (results.Read())
                {
                    var bot = Convert.ToString(results["bot"])?.ToUpper();
                    var price = Convert.ToInt32(results["price"]);

                    if (output.ContainsKey(bot))
                    {
                        output[bot] += price;
                    }
                    else
                    {
                        output.Add(bot, price);
                    }
                }

                return output;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }


        //
        //
        // END OF SALES FUNCTIONS
        //
        //

        //
        //
        // BOT BURNER FUNCTIONS
        //
        //




        //
        //
        // END OF BURNER FUNCTIONS
        //
        //
    }
}
