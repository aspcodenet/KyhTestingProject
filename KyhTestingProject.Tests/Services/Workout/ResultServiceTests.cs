using System;
using System.Collections.Generic;
using KyhTestingProject.Data;
using KyhTestingProject.Services.Workout;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace KyhTestingProject.Tests.Services.Workout;

//public class FakeUserService : IUserService
//{
//    public List<Guid> Existing { get; set; } = new List<Guid>();

//    public bool Exists(Guid workoutUserId)
//    {
//        return Existing.Contains(workoutUserId);
//    }
//}

//public class FakeCalculationService : ICalculationService
//{
//    public int ValueToReturn { get; set; } = 10;

//    public TimeSpan CalculateVelocityPerKm(DateTime workoutStart, DateTime workoutSlut, int workoutTotalMeters)
//    {
//        return TimeSpan.FromSeconds(ValueToReturn);
//    }
//}

//public class FakeStatisticsService : IStatisticsService
//{
//    public int ValueToReturn { get; set; } = 10;
//    public bool SaveNewRecordHasBeenCalled = false;
//    public TimeSpan GetCurrentRecord(Guid workoutUserId)
//    {
//        return TimeSpan.FromSeconds(ValueToReturn);
//    }

//    public void SaveNewRecord(Guid workoutUserId, TimeSpan speed)
//    {
//        SaveNewRecordHasBeenCalled = true;
//    }
//}

//public class FakeEmailService : IEmailService
//{
//    public void SendRecordMail(Guid workoutUserId)
//    {
//    }
//}

//public class Player
//{
//    public int Id { get; set; }
//    public string Name { get; set; }
//    public int JerseyNumber { get; set; }
//}

//public interface IPlayerRepository
//{
//    List<Player> GetAll(string position);
//    Player GetTheBest();
//}

[TestClass]
public class ResultServiceTests
{
    //[TestMethod]
    //public void Whatever()
    //{
    //    //En låda som vi kan fylla med regler
    //    var playerRepositoryMock = new Mock<IPlayerRepository>();
    //    //en låda som kan bete sig som en IPlayerRepository
    //    //när nån anropar lådan genom IPlayerRepository så kommer
    //    //regelverket att avgöra vad som ska returneras
    //    playerRepositoryMock.Setup(e => e.GetAll(It.IsAny<string>()))
    //        .Returns(new List<Player>
    //    {
    //        new Player { Id=1, JerseyNumber = 13, Name = "Mats Sundin"},
    //        new Player{ Id=2, JerseyNumber = 21, Name = "Peter Forsberg"},
    //        new Player{ Id=3, JerseyNumber = 1, Name = "Peter Lindmark"}
    //    });
    //    //playerRepositoryMock.Setup(e => e.GetAll("forward")).Returns(new List<Player>
    //    //{
    //    //    new Player { Id=1, JerseyNumber = 13, Name = "Mats Sundin"},
    //    //    new Player{ Id=2, JerseyNumber = 21, Name = "Peter Forsberg"},
    //    //});
    //    //playerRepositoryMock.Setup(e => e.GetAll("goalie")).Returns(new List<Player>
    //    //{
    //    //    new Player{ Id=3, JerseyNumber = 1, Name = "Peter Lindmark"}
    //    //});

    //    var result = playerRepositoryMock.Object.GetAll("goalie");
    //    foreach (var v in result)
    //    {
    //        Console.WriteLine(v.Name);
    //    }
    //    Assert.AreEqual(3, result.Count);

    //}

    private ResultService _sut;
    private Mock<IUserService> userServiceMock;
    private Mock<ICalculationService> calculationServiceMock;
    private Mock<IStatisticsService> statisticsServiceMock;
    private Mock<IEmailService> emailServiceMock;

    
    [TestInitialize]
    public void Initialize()
    {
        userServiceMock = new Mock<IUserService>();
        calculationServiceMock = new Mock<ICalculationService>();
        statisticsServiceMock = new Mock<IStatisticsService>();
        emailServiceMock = new Mock<IEmailService>();

        _sut = new ResultService(userServiceMock.Object,
            calculationServiceMock.Object,
            statisticsServiceMock.Object,
            emailServiceMock.Object);
    }

    [TestMethod]
    public void When_user_is_not_found_error_will_be_returned()
    {
        //ARRANGE
        var guid = Guid.NewGuid();
        //              Sätt upp
        //                  om nån  anropar Exists och skickar in 
        //              den Guid som är värdet i variabeln guid
        //              så ska Mocken returnera false
        userServiceMock.Setup(u => u.Exists(guid)).Returns(false);

        var workoutModel = new WorkoutResult
        {
            UserId = guid,
            TotalMeters = 1000,
            Start = DateTime.Now.AddHours(-5),
            Slut = DateTime.Now.AddHours(-4),
        };
        
        //ACT
        var result = _sut.Register(workoutModel);
        
        //ASSERT
        Assert.AreEqual(WorkoutRegistrationStatus.NoSuchUser,result);
    }

    [TestMethod]
    public void When_new_record_it_is_stored_in_database()
    {
        //ARRANGE
        var guid = Guid.NewGuid();
        userServiceMock.Setup(u => u.Exists(guid)).Returns(true);
        
        //Annat sätt - vilken GUID som helst            
        //userServiceMock.Setup(u => u.Exists(It.IsAny<Guid>())).Returns(true);


        calculationServiceMock
            .Setup(c => c.CalculateVelocityPerKm(
                                        It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>()))
            .Returns(TimeSpan.FromSeconds(98));

        statisticsServiceMock.Setup(s => s.GetCurrentRecord(guid)).Returns(TimeSpan.FromSeconds(100));


        var workoutModel = new WorkoutResult
        {
            UserId = guid,
            TotalMeters = 1000,
            Start = DateTime.Now.AddHours(-5),
            Slut = DateTime.Now.AddHours(-4),
        };

        //ACT 
        var result = _sut.Register(workoutModel);

        //ASSERT
        Assert.AreEqual(WorkoutRegistrationStatus.Ok, result);
        statisticsServiceMock.Verify(e=>e.SaveNewRecord(guid,TimeSpan.FromSeconds(98)),
            Times.Once);
    }

    [TestMethod]
    public void When_not_new_record_should_not_save()
    {
        //ARRANGE
        userServiceMock.Setup(u => u.Exists(It.IsAny<Guid>())).Returns(true);

        calculationServiceMock.Setup(e => e.CalculateVelocityPerKm(It.IsAny<DateTime>(),
            It.IsAny<DateTime>(), It.IsAny<int>()
        )).Returns(TimeSpan.FromSeconds(100));

        statisticsServiceMock.Setup(e=>e.GetCurrentRecord(It.IsAny<Guid>()))
            .Returns(TimeSpan.FromSeconds(5));

        _sut.Register(new WorkoutResult());

        statisticsServiceMock.Verify(e => e.SaveNewRecord(It.IsAny<Guid>(),
            It.IsAny<TimeSpan>()), Times.Never);

    }


    //[TestMethod]
    //public void When_user_is_found_ok_will_be_returned()
    //{
    //    //ARRANGE
    //    var guid = Guid.NewGuid();
    //    _fakeUserService.Existing.Clear();
    //    _fakeUserService.Existing.Add(guid);

    //    var workoutModel = new WorkoutResult
    //    {
    //        UserId = guid,
    //        TotalMeters = 1000,
    //        Start = DateTime.Now.AddHours(-5),
    //        Slut = DateTime.Now.AddHours(-4),
    //    };

    //    //ACT
    //    var result = _sut.Register(workoutModel);

    //    //ASSERT
    //    Assert.AreEqual(WorkoutRegistrationStatus.Ok, result);
    //}


}