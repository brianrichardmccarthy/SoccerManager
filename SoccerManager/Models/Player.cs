﻿namespace SoccerManager.Models;
/// <summary>
/// Basic Player class
/// </summary>
public class Player
{
    public string Name { get; set; }
    public PlayerPosition Position { get; set; }
    public int SkillRating { get; set; }
    public new string ToString() => $"Name: {Name}, Position: {Position}, Skill Rating: {SkillRating}";
}
