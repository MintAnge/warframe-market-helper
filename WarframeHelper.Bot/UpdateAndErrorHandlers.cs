using Telegram.Bot.Exceptions;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;
using Telegram.Bot;
using MintAnge.WarframeMarketAPI;
using MintAnge.WarframeMarketAPI.Models;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using WarframeHelper;
using Microsoft.EntityFrameworkCore;
using WarframeHelper.Data.Models;
using MintAnge.WarframeHelper.Data;

namespace MintAnge.WarframeMarketHelper.Bot
{
    public class UpdErrHandlers
    {
        private readonly WarframeMarketAPI.WarframeMarketAPI wmAPI;
        private readonly ILogger<UpdErrHandlers> logger;
        private readonly IServiceScopeFactory serviceScopeFactory;

        public UpdErrHandlers(WarframeMarketAPI.WarframeMarketAPI wmAPI, ILogger<UpdErrHandlers> logger, IServiceScopeFactory serviceScopeFactory)
        {
            this.wmAPI = wmAPI;
            this.logger = logger;
            this.serviceScopeFactory = serviceScopeFactory;
        }

        public async Task UpdateHandler(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            try
            {
                switch (update.Type)
                {
                    case UpdateType.Message:
                        {
                            var message = update.Message;
                            var user = message.From;

                            logger.LogInformation("{user name} ({user id}) написал сообщение: {message}", user.FirstName, user.Id, message.Text);

                            var chat = message.Chat;

                            switch (message.Type)
                            {
                                case MessageType.Text:
                                {


                                    string[] str = message.Text.Split(' ');
                                    if (str[0] == "/summary")
                                    {
                                        string s = str[1];
                                        Order[] allOrders = (await this.wmAPI.GetOrdersAsync(s)).payload.orders;

                                        var result = allOrders                                                        
                                                    .Where(order => order.OrderType == "sell" && order.User.Status == "ingame")
                                                    .OrderBy(order => order.Platinum)
                                                    .Take(5)
                                                    .Select(obj => obj.Platinum)
                                                    .ToList();

                                        await botClient.SendTextMessageAsync(
                                            chat.Id,
                                            JsonSerializer.Serialize((result)));

                                        return;
                                    }

                                    if (str[0] =="/track" )
                                    {
                                        string s = str[1];
                                        using (IServiceScope scope = serviceScopeFactory.CreateScope())
                                        {
                                            WarframeMarketContext warframeMarketContext =
                                                scope.ServiceProvider.GetRequiredService<WarframeMarketContext>();
                                            Console.WriteLine("Scope is created!");

                                            var alert = new Alert
                                            {
                                                TgId = user.Id,
                                                Cost = int.Parse(str[2]),
                                                ItemId = await warframeMarketContext.Items.Where(t => t.UrlName == s).Select(t => t.Id).FirstAsync(cancellationToken)
                                            };
                                            await warframeMarketContext.Alerts.AddAsync(alert, cancellationToken);
                                            await warframeMarketContext.SaveChangesAsync(cancellationToken);
                                        }
                                    }

                                    

                                    return;
                                }
                            }


                            await botClient.SendTextMessageAsync(
                                chat.Id,
                                message.Text,
                                replyToMessageId: message.MessageId
                                );

                            return;
                        }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Message processing failed" );
            }
        }

        public Task ErrorHandler(ITelegramBotClient botClient, Exception error, CancellationToken cancellationToken)
        {
            if (error is ApiRequestException)
            {
                var apiRequestException = error as ApiRequestException;
                logger.LogError("Telegram API Error:\n[{ErrorCode}]\n{Message}", apiRequestException.ErrorCode, apiRequestException.Message);
            }
            else
            {
                logger.LogError(error, "Polling failed");
            }

            return Task.CompletedTask;
        }
    }

}
