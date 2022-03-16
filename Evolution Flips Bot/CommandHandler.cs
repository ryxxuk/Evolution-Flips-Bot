using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Evolution_Flips_Bot.Modules;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace Evolution_Flips_Bot
{
    public class CommandHandlingService
    {
        private readonly CommandService _commands;
        private readonly DiscordSocketClient _client;
        private readonly IServiceProvider _services;

        public CommandHandlingService(IServiceProvider services)
        {

            _commands = services.GetRequiredService<CommandService>();
            _client = services.GetRequiredService<DiscordSocketClient>();
            _services = services;

            // Event handlers
            _client.Ready += ClientReadyAsync;
            _client.MessageReceived += HandleCommandAsync;
        }
        
        private async Task HandleCommandAsync(SocketMessage rawMessage)
        {
            if (rawMessage.Author.IsBot || !(rawMessage is SocketUserMessage message))
                return;
            
            var context = new SocketCommandContext(_client, message);

            var argPos = 0;

            var config = Functions.Functions.GetConfig();
            var prefixes = JsonConvert.DeserializeObject<string[]>(config["prefixes"].ToString());

            // Check if message has any of the prefixes or mentiones the bot.
            if (prefixes.Any(x => message.HasStringPrefix(x, ref argPos)) && (context.Channel is IPrivateChannel)) // message.HasMentionPrefix(_client.CurrentUser, ref argPos)
            {
                // Execute the command.
                var result = await _commands.ExecuteAsync(context, argPos, _services);

                if (!result.IsSuccess && result.Error.HasValue)
                {
                    await context.Channel.SendMessageAsync($":x: {result.ErrorReason}");
                }
            }

        }

        private async Task ClientReadyAsync()
            => await Functions.Functions.SetBotStatusAsync(_client);

        public async Task InitializeAsync()
            => await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
    }
}