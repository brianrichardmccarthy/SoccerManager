﻿using SoccerManager.Models;

namespace SoccerManager.Controllers;
public interface IPlayerController
{
    string Create(string name, PlayerPosition position, int skillRating);
    bool Remove(string name);
    string UpdateSkillRank(string name, int skillRank);
    Player? GetByName(string name);
    List<Player> Search(string? name = null, PlayerPosition? position = null);
}
