using Telegram.Bot.Exceptions;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;
using Telegram.Bot;
using MintAnge.WarframeMarketApi;
using MintAnge.WarframeMarketApi.Models;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace MintAnge.WarframeMarketHelper
{
    public class UpdErrHandlers
    {
        private readonly WarframeMarketAPI wmAPI;
        private readonly ILogger<UpdErrHandlers> logger;

        public UpdErrHandlers(WarframeMarketAPI wmAPI, ILogger<UpdErrHandlers> logger)
        {
            this.wmAPI = wmAPI;
            this.logger = logger;
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
