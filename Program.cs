using GuessTheWordBot.Services;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace GuessTheWordBot;

public class Program
{
    private static readonly string botToken = "7792221115:AAEb7yLQMuyMWDr-cZoVvMRWN398QG0M16Q"; // ;//This is temporary need to be moved to appsettings

    private const string START = "/start";
    private const string PLAY = "/play";

    #region Messages
    private const string invalidInputMessage = "Invalid input. Please try again.";
    private const string welcomeToTheGameMessage = "Welcome to the Guess the Word Game! Type /play to start.";
    private const string gameStratedMessage = "A new game has started! Guess a letter.";
    private const string invalidCharacterMessage = "Invalid input. Please guess one letter at a time.";
    #endregion

    private static GameManager gameManager = GameManager.Create();


    static async Task Main(string[] args)
    {
        var botClient = new TelegramBotClientProvider(botToken).GetBotClient();

        var me = await botClient.GetMeAsync();
        Console.WriteLine($"Bot {me.Username} started...");

        var cts = new CancellationTokenSource();
        var cancellationToken = cts.Token;

        botClient.StartReceiving(UpdateHandler, ErrorHandler);

        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
        cts.Cancel();
    }

    public static async Task UpdateHandler(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        if (update.Type != UpdateType.Message || update.Message!.Type != MessageType.Text)
        {
            await botClient.SendTextMessageAsync(update.Message.Chat.Id, invalidInputMessage);
            return;
        }

        var messageText = update.Message.Text?.ToLower();
        var chatId = update.Message.Chat.Id;

        if (messageText == START)
        {
            await botClient.SendTextMessageAsync(chatId, welcomeToTheGameMessage);
        }
        else if (messageText == PLAY)
        {
            await botClient.SendTextMessageAsync(chatId, gameStratedMessage);
            gameManager.StartGame();
        }
        else if (gameManager.GetCurentWord() != null && messageText.Length == 1)
        {
            var result = gameManager.ProcessGuess(messageText[0]);
            await botClient.SendTextMessageAsync(chatId, result);
        }
        else
        {
            await botClient.SendTextMessageAsync(chatId, invalidCharacterMessage);
        }
    }

    private static Task ErrorHandler(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Error: {exception.Message}");
        return Task.CompletedTask;
    }
}
