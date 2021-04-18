using AuthGG;
using Discord;
using Discord.WebSocket;
using Leaf.xNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Mailify
{
    class Discord_Stats
    {

        private DiscordSocketClient _client;
        public async Task MainAsync()
        {
            _client = new DiscordSocketClient();
            _client.MessageReceived += CommandHandler;
            var token = App.GrabVariable("mkggijvMJQKDxgB6jMATUbwJTelAM");
            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();
            await Task.Delay(-1);
        }

        private static Task CommandHandler(SocketMessage message)
        {
            string command = "";
            int lengthOfCommand = -1;


            if (!message.Content.StartsWith(Config.prefix))
                return Task.CompletedTask;

            if (message.Author.IsBot)
                return Task.CompletedTask;

            if (message.Content.Contains(" "))
                lengthOfCommand = message.Content.IndexOf(' ');
            else
                lengthOfCommand = message.Content.Length;

            command = message.Content.Substring(1, lengthOfCommand - 1).ToLower();

            if (command.Equals("s"))
            {

                string DiscordREALID = message.Author.Id.ToString();
                if (Config.Discord_Id == DiscordREALID)
                {

                    switch (Variables.Module_Name)
                    {
                        case "":
                            message.Channel.SendMessageAsync("No Jobs Running...");
                            break;
                        case "Mail Spammer":
                            var embed4 = new EmbedBuilder
                            {
                                Title = "Mailify Stats",
                                ThumbnailUrl = "https://i.imgur.com/CINBxXs.png"
                            };
                            embed4.AddField("Username", User.Username)
                            .AddField("Sent", Spammer.sent, true)
                            .AddField("Errors", Spammer.errors, true)
                            .AddField("Server",Spammer.Host, true)
                            .AddField("Mode", Variables.Module_Name).WithFooter(footer => footer.Text = $"Requested by {message.Author.Username}").WithColor(Discord.Color.Red).WithCurrentTimestamp();
                            message.Channel.SendMessageAsync(embed: embed4.Build());
                            break;
                        case "Mc Mode":
                        case "Mc Mode (Antisilent Ban)":
                        case "Microsoft Mc":
                            var embed = new EmbedBuilder
                            {
                                Title = "Mailify Stats",
                                ThumbnailUrl = "https://i.imgur.com/CINBxXs.png"
                            };
                            embed.AddField("Username", User.Username)
                            .AddField("Valids", Variables.Valid, true)
                            .AddField("NFA", Variables.NFA, true)
                            .AddField("SFA", Variables.SFA, true)
                            .AddField("MFA", Variables.MFA, true)
                            //.AddField("Hypixel", Variables.Hypixel, true)
                            .AddField("Mineplex", Variables.Mineplex, true)
                            .AddField("Optifine Cape", Variables.Optifine, true)
                            .AddField("Minecon Cape", Variables.Minecon, true)
                            .AddField("Invalids", Variables.Invalid, true)
                             .AddField("Customs", Variables.Custom, true)
                            .AddField("Retries", Variables.Errors, true)
                            .AddField("Checked", Variables.Checked + "/" + Variables.loadedCombos, true)
                            .AddField("CPM", Variables.cps*60, true)
                            .AddField("Mode", Variables.Module_Name).WithFooter(footer => footer.Text = $"Requested by {message.Author.Username}").WithColor(Discord.Color.Red).WithCurrentTimestamp();
                            message.Channel.SendMessageAsync(embed: embed.Build());
                            break;
                        case "Xbox Fn":
                            var embed1 = new EmbedBuilder
                            {
                                Title = "Mailify Stats",
                                ThumbnailUrl = "https://i.imgur.com/CINBxXs.png"
                            };
                            embed1.AddField("Username", User.Username)
                            .AddField("Valids", Variables.Valid, true)
                            .AddField("Xbox Hits", Variables.XBOX_Hits, true)
                            .AddField("Fn Hits", Variables.FN_Hits, true)
                            .AddField("Skinned", Variables.Skinned, true)
                            .AddField("No-Skins", Variables.No_Skins, true)
                            .AddField("Rares", Variables.Rares, true)
                            .AddField("Invalids", Variables.Invalid, true)
                            .AddField("Retries", Variables.Errors, true)
                            .AddField("Checked", Variables.Checked + "/" + Variables.loadedCombos, true)
                            .AddField("CPM", Variables.cps * 60, true)
                            .AddField("Mode", Variables.Module_Name).WithFooter(footer => footer.Text = $"Requested by {message.Author.Username}").WithColor(Discord.Color.Red).WithCurrentTimestamp();
                            message.Channel.SendMessageAsync(embed: embed1.Build());
                            break;
                        default:
                            var embed2 = new EmbedBuilder
                            {
                                Title = "Mailify Stats",
                                ThumbnailUrl = "https://i.imgur.com/CINBxXs.png"
                            };
                            embed2.AddField("Username", User.Username)
                            .AddField("Valids", Variables.Valid, true)
                            .AddField("Customs", Variables.Custom, true)
                            .AddField("Invalids", Variables.Invalid, true)
                            .AddField("Retries", Variables.Errors, true)
                            .AddField("Checked", Variables.Checked + "/" + Variables.loadedCombos, true)
                            .AddField("CPM", Variables.cps*60, true)
                            .AddField("Mode", Variables.Module_Name).WithFooter(footer => footer.Text = $"Requested by {message.Author.Username}").WithColor(Discord.Color.Red).WithCurrentTimestamp();
                            message.Channel.SendMessageAsync(embed: embed2.Build());
                            break;
                    }
                }

            }

            return Task.CompletedTask;
        }
    }
}
