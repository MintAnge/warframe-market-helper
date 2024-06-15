using Telegram.Bot.Exceptions;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;
using Telegram.Bot;

internal class UpdErrHandlers
{
        public static async Task UpdateHandler(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
		// ����������� ������ ���� try-catch, ����� ��� ��� �� "�����" � ������ �����-���� ������
		try
		{
			// ����� �� ������ ����������� switch, ����� ������������ ���������� Update
			switch (update.Type)
			{
				case UpdateType.Message:
					{
						// ��� ���������� ����� ��������� � ���� ��� ��������� � �����������
						var message = update.Message;

						// From - ��� �� ���� ������ ��������� (��� ����� ������ Update)
						var user = message.From;

						// ������� �� ����� ��, ��� ����� ������ ����, � ����� ��������� ���������� �� �����������
						Console.WriteLine($"{user.FirstName} ({user.Id}) ������� ���������: {message.Text}");

						// Chat - �������� ��� ���������� � ����
						var chat = message.Chat;
						await botClient.SendTextMessageAsync(
							chat.Id,
							message.Text, // ���������� ��, ��� ������� ������������
							replyToMessageId: message.MessageId // �� ������� ����� ��������� ���� ��������, ���������� �� "�����" �� ���������
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
            // ��� �������� ����������, � ������� �������� ��� ������ � � ��������� 
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
