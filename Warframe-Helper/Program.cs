
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;
using Telegram.Bot;
using System.IO;
using MintAnge.WarframeMarketHelper;



var _botClient = new TelegramBotClient(File.ReadAllText("C:\\Users\\AngeMaks\\source\\repos\\MintAnge\\warframe-market-helper\\bot_token.txt")); 
var _receiverOptions = new ReceiverOptions 
{
    AllowedUpdates = new[] 
    {
                UpdateType.Message, 
            },

    ThrowPendingUpdates = true,
};

using var cts = new CancellationTokenSource();

_botClient.StartReceiving(UpdErrHandlers.UpdateHandler, UpdErrHandlers.ErrorHandler, _receiverOptions, cts.Token); 

var me = await _botClient.GetMeAsync(); 
Console.WriteLine($"{me.FirstName} запущен!");

await Task.Delay(-1); 

