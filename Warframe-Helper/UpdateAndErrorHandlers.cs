using Telegram.Bot.Exceptions;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;
using Telegram.Bot;
using MintAnge.WarframeMarketApi;
using MintAnge.WarframeMarketApi.Models;
using System.Text.Json;

namespace MintAnge.WarframeMarketHelper
{
internal class UpdErrHandlers
{
        public static async Task UpdateHandler(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
		try
		{
			switch (update.Type)
			{
				case UpdateType.Message:
				{
					var message = update.Message;

					var user = message.From;

					Console.WriteLine($"{user.FirstName} ({user.Id}) написал сообщение: {message.Text}");
		
					var chat = message.Chat;

						switch (message.Type)
						{
							case MessageType.Text:
							{
								string[] str = message.Text.Split(' ');
								if (str[0] == "/summary")
								{

                                    string s = str[1];
                                    var wmAPI = new WarframeMarketAPI();
                                    Order[] allOrders = (await wmAPI.GetOrdersAsync(s)).payload.orders;
									
                                    IEnumerable<Order> query = allOrders.OrderBy(order => order.Platinum).Where(order => order.OrderType=="sell" && order.User.Status=="ingame").Take(5);
                                    var result = query.Select(obj => obj.Platinum).ToList();
                                    Console.WriteLine(JsonSerializer.Serialize((result)));

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
			Console.WriteLine(ex.ToString());
		}
	}

        public static Task ErrorHandler(ITelegramBotClient botClient, Exception error, CancellationToken cancellationToken)
        {
            var ErrorMessage = error switch
            {
                ApiRequestException apiRequestException
                    => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => error.ToString()
            };

            Console.WriteLine(ErrorMessage);
            return Task.CompletedTask;
        }
}

}
