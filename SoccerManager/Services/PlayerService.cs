using System.Text;
using FluentValidation;
using SoccerManager.Models;
using SoccerManager.Helpers;

namespace SoccerManager.Services;
public class PlayerService(AbstractValidator<Player> playerValidator) : IPlayerService
{
    private readonly Dictionary<string, Player> _players = new();
    private readonly AbstractValidator<Player> _playerValidator = playerValidator ?? throw new ArgumentNullException(nameof(playerValidator));

    public string Create(string name, PlayerPosition position, int skillRating)
    {
        var player = new Player
        {
            Name = name,
            Position = position,
            SkillRating = skillRating
        };
        
        // Basic validation
        var isValid = _playerValidator.Validate(player);
        if (!isValid.IsValid)
        {
            var errorMsg = new StringBuilder();
            foreach (var error in isValid.Errors)
            {
                errorMsg.AppendLine(error.ErrorMessage);
            }
            return errorMsg.ToString();
        }

        if (_players.Any(p => p.Key == name))
        {
            return $"Player with name {name} already exists";
        }

        _players.Add(name, player);
        return $"Player {name} added successfully";
    }

    public bool Remove(string name)
    {
        var player = GetByName(name);
        return player is not null && _players.Remove(name);
    }

    public string UpdateSkillRank(string name, int skillRank)
    {
        // Basic validation on input and list of players
        if (string.IsNullOrWhiteSpace(name))
        {
            return "Name is required";
        }

        if (skillRank is < 1 or > 100)
        {
            return "Skill Rank must be between 1 and 100";
        }

        if (!_players.TryGetValue(name, out var toUpdate))
        {
            return $"Unable to find player with name {name}";
        }

        toUpdate.SkillRating = skillRank;
        return $"{toUpdate.Name} updated successfully";
    }

    public Player? GetByName(string name)
    {
        return _players.GetValueOrDefault(name);
    }

    public List<Player> Search(string? name = null, PlayerPosition? position = null)
    {
        // use helper methods to filter the list if needed
        return _players.Values
            .WhereIf(!string.IsNullOrEmpty(name), p => p.Name.Contains(name!, StringComparison.OrdinalIgnoreCase))
            .WhereIf(position is not null && Enum.IsDefined(typeof(PlayerPosition), position), p => p.Position == position)
            .ToList();
    }

    public override string ToString()
    {
        var stringBuilder = new StringBuilder();
        foreach (var (_, player) in _players)
        {
            stringBuilder.AppendLine(player.ToString());
        }
        return stringBuilder.ToString();
    }
}
