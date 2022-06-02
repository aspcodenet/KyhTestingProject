namespace KyhTestingProject.Services.Workout;

public interface IResultService
{
    WorkoutRegistrationStatus Register(WorkoutResult workout);
}
public class WorkoutResult
{
    public Guid UserId { get; set; }
    public DateTime Start { get; set; }
    public DateTime Slut { get; set; }
    public int TotalMeters { get; set; }
}
public enum WorkoutRegistrationStatus
{
    Ok,
    NoSuchUser,
}

public interface IUserService
{
    bool Exists(Guid workoutUserId);
}

public interface ICalculationService
{
    TimeSpan CalculateVelocityPerKm(DateTime workoutStart, 
        DateTime workoutSlut, int workoutTotalMeters);
}

public interface IStatisticsService
{
    TimeSpan GetCurrentRecord(Guid workoutUserId);
    void SaveNewRecord(Guid workoutUserId, TimeSpan speed);
}

public interface IEmailService
{
    void SendRecordMail(Guid workoutUserId);
}

public class ResultService : IResultService
{
    private readonly IUserService _userService;
    private readonly ICalculationService _calculationService;
    private readonly IStatisticsService _statisticsService;
    private readonly IEmailService _emailService;

    public ResultService(IUserService userService, 
        ICalculationService calculationService,
        IStatisticsService statisticsService,
        IEmailService emailService )
    {
        _userService = userService;
        _calculationService = calculationService;
        _statisticsService = statisticsService;
        _emailService = emailService;
    }
    public WorkoutRegistrationStatus Register(WorkoutResult workout)
    {
        if (!_userService.Exists(workout.UserId))
            return WorkoutRegistrationStatus.NoSuchUser;


        var speedPerKm = _calculationService.CalculateVelocityPerKm(workout.Start, workout.Slut,
            workout.TotalMeters);

        var currentFastestPerKm = _statisticsService.GetCurrentRecord(workout.UserId);
        if (speedPerKm < currentFastestPerKm)
        {
            _statisticsService.SaveNewRecord(workout.UserId, speedPerKm);
            _emailService.SendRecordMail(workout.UserId);
        }

        return WorkoutRegistrationStatus.Ok;
    }
    //Fler

}