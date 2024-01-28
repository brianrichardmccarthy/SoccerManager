using SoccerManager.Models;
using SoccerManager.Services;

namespace SoccerManager.Controller;
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
            
            option = (MenuOption)int.Parse(Console.ReadLine() ?? "0");

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
                default:
                    throw new ArgumentOutOfRangeException();
            }

        } while (option != MenuOption.Exit);
    }

    public void CreatePlayer()
    {
        Console.WriteLine("Enter Player Name: ");
        var name = Console.ReadLine() ?? string.Empty;
        Console.WriteLine("Enter Player Position: ");
        var position = (PlayerPosition)int.Parse(Console.ReadLine() ?? "0");
        Console.WriteLine("Enter Player Skill Rating: ");
        var skillRating = int.Parse(Console.ReadLine() ?? "0");
        var response = _playerService.Create(name, position, skillRating);
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
        Console.WriteLine("Enter Player Skill Rating: ");
        var skillRating = int.Parse(Console.ReadLine() ?? "0");
        var response = _playerService.UpdateSkillRank(name, skillRating);
        Console.WriteLine(response);
    }

    public void SearchPlayers()
    {
        Console.WriteLine("Enter Player Name or Press Enter to skip: ");
        var name = Console.ReadLine();
        Console.WriteLine("Enter Player Position or Press Enter to skip: ");
        var position = (PlayerPosition)int.Parse(Console.ReadLine() ?? "0");
        var players = _playerService.Search(name, position);
        foreach (var player in players)
        {
            Console.WriteLine(player);
        }
    }

    public void GetPlayerByName()
    {
        Console.WriteLine("Enter Player Name: ");
        var name = Console.ReadLine() ?? string.Empty;
        var player = _playerService.GetByName(name);
        Console.WriteLine(player);
    }

    public void DisplayAllPlayers()
    {
        Console.WriteLine(_playerService.ToString());
    }
}
