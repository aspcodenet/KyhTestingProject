using System;
using System.Collections.Generic;
using KyhTestingProject.Services.Workout;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KyhTestingProject.Tests.Services.Workout;

public class FakeUserService : IUserService
{
    public List<Guid> Existing { get; set; } = new List<Guid>();

    public bool Exists(Guid workoutUserId)
    {
        return Existing.Contains(workoutUserId);
    }
}

[TestClass]
public class ResultServiceTests
{
    private ResultService _sut;
    private FakeUserService _fakeUserService;
    public ResultServiceTests()
    {
        _fakeUserService = new FakeUserService();
        _sut = new ResultService(_fakeUserService);
    }

    [TestMethod]
    public void When_user_is_not_found_error_will_be_returned()
    {
        //ARRANGE
        var guid = Guid.NewGuid();
        _fakeUserService.Existing.Clear();

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
    public void When_user_is_found_ok_will_be_returned()
    {
        //ARRANGE
        var guid = Guid.NewGuid();
        _fakeUserService.Existing.Clear();
        _fakeUserService.Existing.Add(guid);

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
    }


}