
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;
using Telegram.Bot;
using System.IO;



var _botClient = new TelegramBotClient(File.ReadAllText("C:\\Users\\AngeMaks\\source\\repos\\MintAnge\\warframe-market-helper\\bot_token.txt")); // Присваиваем нашей переменной значение, в параметре передаем Token, полученный от BotFather
var _receiverOptions = new ReceiverOptions // Также присваем значение настройкам бота
{
    AllowedUpdates = new[] // Тут указываем типы получаемых Update`ов, о них подробнее расказано тут https://core.telegram.org/bots/api#update
    {
                UpdateType.Message, // Сообщения (текст, фото/видео, голосовые/видео сообщения и т.д.)
            },
    // Параметр, отвечающий за обработку сообщений, пришедших за то время, когда ваш бот был оффлайн
    // True - не обрабатывать, False (стоит по умолчанию) - обрабаывать
    ThrowPendingUpdates = true,
};

using var cts = new CancellationTokenSource();

// UpdateHander - обработчик приходящих Update`ов
// ErrorHandler - обработчик ошибок, связанных с Bot API
_botClient.StartReceiving(UpdErrHandlers.UpdateHandler, UpdErrHandlers.ErrorHandler, _receiverOptions, cts.Token); // Запускаем бота

var me = await _botClient.GetMeAsync(); // Создаем переменную, в которую помещаем информацию о нашем боте.
Console.WriteLine($"{me.FirstName} запущен!");

await Task.Delay(-1); // Устанавливаем бесконечную задержку, чтобы наш бот работал постоянно

