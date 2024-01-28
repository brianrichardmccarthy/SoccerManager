using System.Text;
using FluentValidation;
using SoccerManager.Models;

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
        var search = _players.Select(p => p.Value);
        if (!string.IsNullOrEmpty(name))
        {
            search = search.Where(p => p.Name.Contains(name, StringComparison.OrdinalIgnoreCase));
        }

        if (position is not null && Enum.IsDefined(typeof(PlayerPosition), position))
        {
            search = search.Where(p => p.Position == position);
        }
        return search.ToList();
    }

    public override string ToString()
    {
        var stringBuilder = new StringBuilder(base.ToString());
        foreach (var (_, player) in _players)
        {
            stringBuilder.AppendLine($"{player.Name} - {player.Position} - {player.SkillRating}");
        }
        return stringBuilder.ToString();
    }
}
