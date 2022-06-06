﻿using System;
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

public class FakeCalculationService : ICalculationService
{
    public int ValueToReturn { get; set; } = 10;

    public TimeSpan CalculateVelocityPerKm(DateTime workoutStart, DateTime workoutSlut, int workoutTotalMeters)
    {
        return TimeSpan.FromSeconds(ValueToReturn);
    }
}

public class FakeStatisticsService : IStatisticsService
{
    public int ValueToReturn { get; set; } = 10;
    public bool SaveNewRecordHasBeenCalled = false;
    public TimeSpan GetCurrentRecord(Guid workoutUserId)
    {
        return TimeSpan.FromSeconds(ValueToReturn);
    }

    public void SaveNewRecord(Guid workoutUserId, TimeSpan speed)
    {
        SaveNewRecordHasBeenCalled = true;
    }
}

public class FakeEmailService : IEmailService
{
    public void SendRecordMail(Guid workoutUserId)
    {
    }
}

[TestClass]
public class ResultServiceTests
{
    private ResultService _sut;
    private FakeUserService _fakeUserService;
    private FakeCalculationService _fakeCalculationService;
    private FakeStatisticsService _fakeStatisticsService;
    private FakeEmailService _fakeEmailService;
    public ResultServiceTests()
    {
        _fakeUserService = new FakeUserService();
        _fakeCalculationService = new FakeCalculationService();
        _fakeStatisticsService = new FakeStatisticsService();
        _fakeEmailService = new FakeEmailService();
        
        _sut = new ResultService(_fakeUserService, 
            _fakeCalculationService,
            _fakeStatisticsService,
            _fakeEmailService);
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
    public void When_new_record_it_is_stored_in_database()
    {
        //ARRANGE
        var guid = Guid.NewGuid();
        _fakeUserService.Existing.Clear();
        _fakeUserService.Existing.Add(guid);
        _fakeStatisticsService.SaveNewRecordHasBeenCalled = false;
        _fakeStatisticsService.ValueToReturn = 100;
        _fakeCalculationService.ValueToReturn = 10;

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
        Assert.IsTrue(_fakeStatisticsService.SaveNewRecordHasBeenCalled);

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