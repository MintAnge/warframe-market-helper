using Telegram.Bot.Exceptions;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;
using Telegram.Bot;

internal class UpdErrHandlers
{
        public static async Task UpdateHandler(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
		// ќб€зательно ставим блок try-catch, чтобы наш бот не "падал" в случае каких-либо ошибок
		try
		{
			// —разу же ставим конструкцию switch, чтобы обрабатывать приход€щие Update
			switch (update.Type)
			{
				case UpdateType.Message:
					{
						// эта переменна€ будет содержать в себе все св€занное с сообщени€ми
						var message = update.Message;

						// From - это от кого пришло сообщение (или любой другой Update)
						var user = message.From;

						// ¬ыводим на экран то, что пишут нашему боту, а также небольшую информацию об отправителе
						Console.WriteLine($"{user.FirstName} ({user.Id}) написал сообщение: {message.Text}");

						// Chat - содержит всю информацию о чате
						var chat = message.Chat;
						await botClient.SendTextMessageAsync(
							chat.Id,
							message.Text, // отправл€ем то, что написал пользователь
							replyToMessageId: message.MessageId // по желанию можем поставить этот параметр, отвечающий за "ответ" на сообщение
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
            // “ут создадим переменную, в которую поместим код ошибки и еЄ сообщение 
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
