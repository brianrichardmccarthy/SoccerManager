using FluentAssertions;
using NUnit.Framework;
using SoccerManager.Models;
using SoccerManager.Services;
using SoccerManager.Validators;

namespace SoccerManagerTests;

[TestFixture]
public class PlayerServiceTest
{
    private PlayerService _playerController = null!;
    private CreateValidator _createValidator = null!;

    [SetUp]
    public void Setup()
    {
        _createValidator = new CreateValidator();
        _playerController = new PlayerService(_createValidator);
    }

    #region Constructor
    [Test]
    public void PlayerController_WhenNullValidator_ThrowsException()
    {
        // Act
        Action act = () => _ = new PlayerService(null!);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }
    #endregion

    #region Create
    [Test]
    public void Create_WhenValidPlayer_ReturnsSuccessMessage()
    {
        // Arrange
        const string name = "abc";

        // Act
        var result = _playerController.Create(name, PlayerPosition.Forward, 80);

        // Assert
        result.Should().Be($"Player {name} added successfully", result);
    }

    [TestCase("Name is required", "", PlayerPosition.Forward, 1)]
    [TestCase("Position is required and must be between 1 and 4 inclusive", "abc", (PlayerPosition)100, 1)]
    [TestCase("Skill Rating must be between 1 and 100", "abc", PlayerPosition.Forward, 0)]
    [TestCase("Skill Rating must be between 1 and 100", "abc", PlayerPosition.Forward, 101)]
    public void Create_WhenInvalidPlayer_ReturnsErrorMessage(string expected, string name, PlayerPosition playerPosition, int skillRating)
    {
        var result = _playerController.Create(name, playerPosition, skillRating);
        result.TrimEnd().Should().BeEquivalentTo(expected, result);
    }

    [Test]
    public void Create_WhenPlayerAlreadyExists_ReturnsErrorMessage()
    {
        // Arrange
        const string name = "abc";
        _playerController.Create(name, PlayerPosition.Forward, 1);

        // Act
        var result = _playerController.Create(name, PlayerPosition.Forward, 1);

        // Assert
        result.Should().Be($"Player with name {name} already exists", result);
    }
    #endregion

    #region Remove
    [Test]
    public void Remove_WhenPlayerExists_ReturnsTrue()
    {
        // Arrange
        const string name = "abc";
        _playerController.Create(name, PlayerPosition.Forward, 1);

        // Act
        var result = _playerController.Remove(name);

        // Assert
        result.Should().BeTrue();
    }

    [Test]
    public void Remove_WhenPlayerDoesNotExist_ReturnsFalse()
    {
        // Arrange
        const string name = "abc";

        // Act
        var result = _playerController.Remove(name);

        // Assert
        result.Should().BeFalse();
    }
    #endregion

    #region UpdateSkillRank
    [Test]
    public void UpdateSkillRank_WhenPlayerExists_ReturnsSuccessMessage()
    {
        // Arrange
        const string name = "abc";
        _playerController.Create(name, PlayerPosition.Forward, 1);

        // Act
        var result = _playerController.UpdateSkillRank(name, 2);

        // Assert
        result.Should().Be($"{name} updated successfully");
        var player = _playerController.GetByName(name);
        player.Should().NotBeNull();
        player!.SkillRating.Should().Be(2);
    }

    [Test]
    public void UpdateSkillRank_WhenPlayerDoesNotExist_ReturnsErrorMessage()
    {
        // Arrange
        const string name = "abc";

        // Act
        var result = _playerController.UpdateSkillRank(name, 1);

        // Assert
        result.Should().Be($"Unable to find player with name {name}");
    }

    [TestCase(0)]
    [TestCase(101)]
    public void UpdateSkillRank_WhenSkillRankIsLessThanOne_ReturnsErrorMessage(int test)
    {
        // Arrange
        const string name = "abc";
        _playerController.Create(name, PlayerPosition.Forward, 1);

        // Act
        var result = _playerController.UpdateSkillRank(name, test);

        // Assert
        result.Should().Be("Skill Rank must be between 1 and 100");
    }

    [Test]
    public void UpdateSkillRank_WhenNameIsNotProvided_ReturnsErrorMessage()
    {
        // Arrange
        const string name = "abc";
        _playerController.Create(name, PlayerPosition.Forward, 1);

        // Act
        var result = _playerController.UpdateSkillRank("", 1);

        // Assert
        result.Should().Be("Name is required");
    }
    #endregion

    #region GetByName
    [Test]
    public void GetByName_WhenPlayerExists_ReturnsPlayer()
    {
        // Arrange
        const string name = "abc";
        _playerController.Create(name, PlayerPosition.Forward, 1);

        // Act
        var result = _playerController.GetByName(name);

        // Assert
        result.Should().NotBeNull();
        result!.Name.Should().Be(name);
    }

    [Test]
    public void GetByName_WhenPlayerDoesNotExist_ReturnsNull()
    {
        // Arrange
        const string name = "abc";

        // Act
        var result = _playerController.GetByName(name);

        // Assert
        result.Should().BeNull();
    }
    #endregion

    #region search
    [Test]
    public void Search_WhenNameIsProvided_ReturnsPlayer()
    {
        // Arrange
        const string name = "abc";
        _playerController.Create(name, PlayerPosition.Forward, 1);

        // Act
        var result = _playerController.Search(name);

        // Assert
        result.Should().NotBeNull();
        result.Count.Should().Be(1);
        result[0].Name.Should().Be(name);
    }

    [Test]
    public void Search_WhenNameIsNotProvided_ReturnsAllPlayers()
    {
        // Arrange
        const string name = "abc";
        _playerController.Create(name, PlayerPosition.Forward, 1);
        _playerController.Create("def", PlayerPosition.Goalkeeper, 2);

        // Act
        var result = _playerController.Search();

        // Assert
        result.Should().NotBeNull();
        result.Count.Should().Be(2);
        result[0].Name.Should().Be(name);
    }

    [Test]
    public void Search_WhenPositionIsProvided_ReturnsPlayers()
    {
        // Arrange
        _playerController.Create("abc", PlayerPosition.Forward, 1);
        _playerController.Create("def", PlayerPosition.Forward, 1);
        _playerController.Create("ghi", PlayerPosition.Goalkeeper, 1);

        // Act
        var result = _playerController.Search(position: PlayerPosition.Forward);

        // Assert
        result.Should().NotBeNull();
        result.Count.Should().Be(2);
    }

    [Test]
    public void Search_WhenNamePartialMatches_ReturnsPlayers()
    {
        // Arrange
        _playerController.Create("abc", PlayerPosition.Forward, 1);
        _playerController.Create("abc def", PlayerPosition.Forward, 1);
        _playerController.Create("ghi", PlayerPosition.Goalkeeper, 1);

        // Act
        var result = _playerController.Search("abc");

        // Assert
        result.Should().NotBeNull();
        result.Count.Should().Be(2);
    }
    #endregion
}