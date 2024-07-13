using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MintAnge.WarframeMarketHelper;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;
using Telegram.Bot;

public class BackgroundBotService : BackgroundService
{
    private readonly ILogger<BackgroundBotService> _logger;
    private readonly TelegramBotClient _botClient;
    private readonly UpdErrHandlers _updErrHandlers;

    public BackgroundBotService(TelegramBotClient botClient, ILogger<BackgroundBotService> logger, UpdErrHandlers updErrHandlers)
    {
        _botClient = botClient;
        _logger = logger;
        _updErrHandlers = updErrHandlers;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        //_botClient = new TelegramBotClient(File.ReadAllText("C:\\Users\\AngeMaks\\source\\repos\\MintAnge\\warframe-market-helper\\bot_token.txt"));


        var _receiverOptions = new ReceiverOptions
        {
            AllowedUpdates = new[]
            {
                UpdateType.Message,
            },

            ThrowPendingUpdates = true,
        };


        var me = await _botClient.GetMeAsync(stoppingToken);
        _logger.LogInformation("{имя} запущен!", me.FirstName);


        await _botClient.ReceiveAsync(
            _updErrHandlers.UpdateHandler,
            _updErrHandlers.ErrorHandler,
            _receiverOptions,
            stoppingToken);
        _logger.LogInformation("ALL DONE MATE!");
    }

}
