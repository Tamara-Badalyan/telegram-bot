using Telegram.Bot;

namespace GuessTheWordBot.Services;

public class TelegramBotClientProvider
{
    private readonly string _botToken;
    private TelegramBotClient _botClient;

    public TelegramBotClientProvider(string botToken)
    {
        _botToken = botToken;
    }

    public TelegramBotClient GetBotClient()
    {
        if (_botClient == null)
        {
            _botClient = new TelegramBotClient(_botToken);
        }

        return _botClient;
    }
}
