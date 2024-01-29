using SoccerManager.Models;
using SoccerManager.Services;

namespace SoccerManager.Controller;
/// <summary>
/// Basic Console Controller for Player
/// </summary>
/// <param name="playerService"></param>
public class PlayerController(IPlayerService playerService) : IPlayerController
{
    private readonly IPlayerService _playerService = playerService ?? throw new ArgumentNullException(nameof(playerService));
    public void Run()
    {
        MenuOption option;
        do
        {
            Console.WriteLine("1. Create Player");
            Console.WriteLine("2. Remove Player");
            Console.WriteLine("3. Update Skill Rating Player");
            Console.WriteLine("4. Search Players");
            Console.WriteLine("5. Get Player By Name");
            Console.WriteLine("6. Display All Players");
            Console.WriteLine("7. Exit");

            var isValid = int.TryParse(Console.ReadLine() ?? "0", out var input);
            option = isValid ? (MenuOption)input : MenuOption.None;

            switch (option)
            {
                case MenuOption.Create:
                    CreatePlayer();
                    break;
                case MenuOption.Remove:
                    RemovePlayer();
                    break;
                case MenuOption.Update:
                    UpdateSkillRating();
                    break;
                case MenuOption.Search:
                    SearchPlayers();
                    break;
                case MenuOption.GetByName:
                    GetPlayerByName();
                    break;
                case MenuOption.DisplayAll:
                    DisplayAllPlayers();
                    break;
                case MenuOption.Exit:
                    break;
                case MenuOption.None:
                default:
                    Console.WriteLine("Invalid Option please enter a number between 1 and 7");
                    break;
            }

        } while (option != MenuOption.Exit);
    }

    public void CreatePlayer()
    {
        Console.WriteLine("Enter Player Name: ");
        var name = Console.ReadLine() ?? string.Empty;
        Console.WriteLine("Enter Player Position: ");
        foreach (var option in Enum.GetValues(typeof(PlayerPosition)))
        {
            Console.WriteLine($"{(int)option}. {option}");
        }

        int positionAsInt;
        while (!int.TryParse(Console.ReadLine() ?? string.Empty, out positionAsInt))
        {
            Console.WriteLine("invalid option please enter a number between 1 and 4");
        }

        var skillRating = ReadSkillRankFromConsole();
        var response = _playerService.Create(name, (PlayerPosition)positionAsInt, skillRating);
        Console.WriteLine(response);
    }

    public void RemovePlayer()
    {
        Console.WriteLine("Enter Player Name: ");
        var name = Console.ReadLine() ?? string.Empty;
        var isRemoved = _playerService.Remove(name);
        Console.WriteLine(isRemoved ? "Player removed successfully" : "Player not found");
    }

    public void UpdateSkillRating()
    {
        Console.WriteLine("Enter Player Name: ");
        var name = Console.ReadLine() ?? string.Empty;
        var skillRating = ReadSkillRankFromConsole();
        var response = _playerService.UpdateSkillRank(name, skillRating);
        Console.WriteLine(response);
    }

    public void SearchPlayers()
    {
        Console.WriteLine("Enter Player Name or Press Enter to skip: ");
        var name = Console.ReadLine() ?? string.Empty;
        Console.WriteLine("Enter Player Position or Press Enter to skip: ");
        var includePosition = int.TryParse(Console.ReadLine() ?? string.Empty, out var positionAsInt);
        var position = includePosition ? (PlayerPosition) positionAsInt : (PlayerPosition?)null;
        var players = _playerService.Search(name, position);
        foreach (var player in players)
        {
            Console.WriteLine(player.ToString());
        }
    }

    public void GetPlayerByName()
    {
        Console.WriteLine("Enter Player Name: ");
        var name = Console.ReadLine() ?? string.Empty;
        var player = _playerService.GetByName(name);
        Console.WriteLine(player is not null ? player.ToString() : $"Player {name} not found");
    }

    public void DisplayAllPlayers() => Console.WriteLine(_playerService.ToString());

    private int ReadSkillRankFromConsole()
    {
        Console.WriteLine("Enter Player Skill Rating: ");
        var skillRating = int.Parse(Console.ReadLine() ?? "0");
        return skillRating;
    }
}
