using SoccerManager.Models;

namespace SoccerManager.Helpers;
/// <summary>
/// Helper class for searching List of Players
/// </summary>
public static class SearchExtensions
{
    public static IEnumerable<Player> WhereIf(this IEnumerable<Player> source, bool condition, Func<Player, bool> predicate) => condition ? source.Where(predicate) : source;
}
