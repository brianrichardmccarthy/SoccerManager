using FluentAssertions;
using Moq;
using NUnit.Framework;
using SoccerManager.Controller;
using SoccerManager.Models;
using SoccerManager.Services;

namespace SoccerManagerTests;
[TestFixture]
public class PlayerControllerTests
{
    private Mock<IPlayerService> _playerServiceMock = null!;
    private PlayerController _playerController = null!;

    [SetUp]
    public void Setup()
    {
        _playerServiceMock = new Mock<IPlayerService>();
        _playerController = new PlayerController(_playerServiceMock.Object);
    }

    #region Constructor
    [Test]
    public void PlayerController_WhenNullService_ThrowsException()
    {
        // Act
        Action act = () => _ = new PlayerController(null!);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }
    #endregion

    #region Run
    [Test]
    public void Run_WhenCalled_DisplaysMenu()
    {
        // Arrange
        var input = new StringReader("7\n");
        Console.SetIn(input);

        // Act
        _playerController.Run();

        // Assert
        VerifyNoOtherCalls();
    }
    #endregion

    #region CreatePlayer
    [Test]
    public void CreatePlayer_WhenCalled_CallsCreate()
    {
        // Arrange
        Setup_Create();
        const string name = "abc";

        var input = new StringReader($"{name}\n1\n1\n");
        Console.SetIn(input);

        // Act
        _playerController.CreatePlayer();

        // Assert
        VerifyCreate(name, PlayerPosition.Goalkeeper, 1);
        VerifyNoOtherCalls();
    }
    #endregion

    #region RemovePlayer
    [Test]
    public void RemovePlayer_WhenCalled_CallsRemove()
    {
        // Arrange
        Setup_Remove(true);
        const string name = "abc";

        var input = new StringReader($"{name}\n");
        Console.SetIn(input);

        // Act
        _playerController.RemovePlayer();

        // Assert
        VerifyRemove(name);
        VerifyNoOtherCalls();
    }
    #endregion

    #region UpdateSkillRating
    [Test]
    public void UpdateSkillRating_WhenCalled_CallsUpdateSkillRank()
    {
        // Arrange
        Setup_UpdateSkillRank();
        const string name = "abc";

        var input = new StringReader($"{name}\n1\n");
        Console.SetIn(input);

        // Act
        _playerController.UpdateSkillRating();

        // Assert
        VerifyUpdateSkillRank(name, 1);
        VerifyNoOtherCalls();
    }
    #endregion

    #region SearchPlayers
    [Test]
    public void SearchPlayers_WhenCalled_CallsSearch()
    {
        // Arrange
        Setup_Search([new Player() { Name = "abc", Position = PlayerPosition.Goalkeeper, SkillRating = 1 }]);
        const string name = "abc";

        var input = new StringReader($"{name}\n1\n");
        Console.SetIn(input);

        // Act
        _playerController.SearchPlayers();

        // Assert
        VerifySearch(name, PlayerPosition.Goalkeeper);
        VerifyNoOtherCalls();
    }
    #endregion

    #region GetPlayerByName
    [Test]
    public void GetPlayerByName_WhenCalled_CallsGetByName()
    {
        // Arrange
        Setup_GetByName(new Player{ Name = "abc", Position = PlayerPosition.Goalkeeper, SkillRating = 1 });
        const string name = "abc";

        var input = new StringReader($"{name}\n");
        Console.SetIn(input);

        // Act
        _playerController.GetPlayerByName();

        // Assert
        VerifyGetByName(name);
        VerifyNoOtherCalls();
    }
    #endregion

    #region DisplayAllPlayers
    [Test]
    public void DisplayAllPlayers_WhenCalled_CallsGetAll()
    {
        // Arrange
        var input = new StringReader("6\n");
        Console.SetIn(input);

        // Act
        _playerController.DisplayAllPlayers();

        // Assert
        VerifyNoOtherCalls();
    }
    #endregion
    
    #region Setup
    private void Setup_Create(bool throws = false)
    {
        if (throws)
        {
            _playerServiceMock.Setup(x => x.Create(It.IsAny<string>(), It.IsAny<PlayerPosition>(), It.IsAny<int>()))
                .Throws<Exception>();
        }
        else
        {
            _playerServiceMock.Setup(x => x.Create(It.IsAny<string>(), It.IsAny<PlayerPosition>(), It.IsAny<int>()))
                .Returns("Success");
        }
    }

    private void Setup_Remove(bool expected, bool throws = false)
    {
        if (throws)
        {
            _playerServiceMock.Setup(x => x.Remove(It.IsAny<string>()))
                .Throws<Exception>();
        }
        else
        {
            _playerServiceMock.Setup(x => x.Remove(It.IsAny<string>()))
                .Returns(expected);
        }
    }

    private void Setup_UpdateSkillRank(string massage = "", bool throws = false)
    {
        if (throws)
        {
            _playerServiceMock.Setup(x => x.UpdateSkillRank(It.IsAny<string>(), It.IsAny<int>()))
                .Throws<Exception>();
        }
        else
        {
            _playerServiceMock.Setup(x => x.UpdateSkillRank(It.IsAny<string>(), It.IsAny<int>()))
                .Returns(massage);
        }
    }

    private void Setup_GetByName(Player? player = null, bool throws = false)
    {
        if (throws)
        {
            _playerServiceMock.Setup(x => x.GetByName(It.IsAny<string>()))
                .Throws<Exception>();
        }
        else
        {
            _playerServiceMock.Setup(x => x.GetByName(It.IsAny<string>()))
                .Returns(player);
        }
    }

    private void Setup_Search(List<Player> data, bool throws = false)
    {
        if (throws)
        {
            _playerServiceMock.Setup(x => x.Search(It.IsAny<string>(), It.IsAny<PlayerPosition>()))
                .Throws<Exception>();
        }
        else
        {
            _playerServiceMock.Setup(x => x.Search(It.IsAny<string>(), It.IsAny<PlayerPosition>()))
                .Returns(data);
        }
    }
    #endregion

    #region Verify
    private void VerifyCreate(string name, PlayerPosition playerPosition, int skillRating)
    {
        _playerServiceMock.Verify(x => x.Create(name, playerPosition, skillRating), Times.Once);
    }

    private void VerifyRemove(string name)
    {
        _playerServiceMock.Verify(x => x.Remove(name), Times.Once);
    }

    private void VerifyUpdateSkillRank(string name, int skillRank)
    {
        _playerServiceMock.Verify(x => x.UpdateSkillRank(name, skillRank), Times.Once);
    }

    private void VerifyGetByName(string name)
    {
        _playerServiceMock.Verify(x => x.GetByName(name), Times.Once);
    }

    private void VerifySearch(string? name = null, PlayerPosition? position = null)
    {
        _playerServiceMock.Verify(x => x.Search(name, position), Times.Once);
    }

    private void VerifyNoOtherCalls()
    {
        _playerServiceMock.VerifyNoOtherCalls();
    }
    #endregion
}
