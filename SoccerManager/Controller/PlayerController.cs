using SoccerManager.Services;

namespace SoccerManager.Controller;
public class PlayerController(IPlayerService playerService) : IPlayerController
{
    private readonly IPlayerService _playerService = playerService ?? throw new ArgumentNullException(nameof(playerService));

    public void Run()
    {
        throw new NotImplementedException();
    }
}
