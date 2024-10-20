namespace GuessTheWordBot.Services;

public class GameManager
{
    private string currentWord;
    private List<char> guessedLetters;
    private int attemptsLeft;
    private readonly int maxAttempts = 5;
    private static string[] wordList = { "programming", "telegram", "bot", "notebook", "challenge", "pen", "dog" };//This is temporary solution, need to replace with a library

    #region Messages
    private string alreadyGuessedLetterMessage = "You've already guessed the letter '{0}'!";
    private string wordGuessedMessage = "Congratulations! You've guessed the word: {0}. Type /play to start the game again.";
    private string curentWordMessage = "Good guess! Current word: {0}";
    private string gameOverMessage = "Game over! The word was: {0}. Type /play to start the game again.";
    private string wrongLetterGuessedMessage = "Wrong guess! Attempts left: {0}. Current word: {1}";
    #endregion

    public GameManager()
    {
        guessedLetters = new List<char>();
    }

    public static GameManager Create()
    {
        return new GameManager();
    }

    public string GetCurentWord()
    {
        return currentWord;
    }

    public void StartGame()
    {
        Random rand = new Random();
    
        currentWord = wordList[rand.Next(wordList.Length)];
        guessedLetters.Clear();
        attemptsLeft = maxAttempts;
    }

    public string ProcessGuess(char letter)
    {
        letter = char.ToLower(letter);

        if (guessedLetters.Contains(letter))
        {
            return string.Format(alreadyGuessedLetterMessage, letter);
        }

        guessedLetters.Add(letter);

        if (currentWord.Contains(letter))
        {
            if (IsWordGuessed())
            {
                return string.Format(wordGuessedMessage, currentWord);
            }
            
            return  string.Format(curentWordMessage, GetWordDisplay());
        }
        else
        {
            attemptsLeft--;
            if (attemptsLeft <= 0)
            {
                return string.Format(gameOverMessage, currentWord);
            }

            return string.Format(wrongLetterGuessedMessage, attemptsLeft, GetWordDisplay());
        }
    }

    private bool IsWordGuessed()
    {
        foreach (char letter in currentWord)
        {
            if (!guessedLetters.Contains(letter))
            {
                return false;
            }
        }
        return true;
    }

    private string GetWordDisplay()
    {
        var display = new char[currentWord.Length];

        for (int i = 0; i < currentWord.Length; i++)
        {
            if (guessedLetters.Contains(currentWord[i]))
            {
                display[i] = currentWord[i];
            }
            else
            {
                display[i] = '_'; 
            }
        }
        return new string(display);
    }

    private bool IsGameOngoing()
    {
        return attemptsLeft > 0 && !IsWordGuessed();
    }
}
