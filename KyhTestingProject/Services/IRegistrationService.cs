using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace KyhTestingProject.Services;

public interface IRegistrationService
{
    RegistrationStatus Register(string email);
}

public enum RegistrationStatus
{
    Ok,
    AlreadyRegistered,
    WrongDomain,
    TooManyRegistrationsToday
}

public interface IUserRepository
{
    bool Exists(string email);

    int GetRegistreredCountToday();
}


public interface IEmailService
{
    void SendWelcomeEmail(string email);
}


public class RegistrationService : IRegistrationService
{
    private readonly IUserRepository _userRepository;
    private readonly IEmailService _emailService;

    public RegistrationService(IUserRepository userRepository, IEmailService emailService)
    {
        _userRepository = userRepository;
        _emailService = emailService;
    }
    public RegistrationStatus Register(string email)
    {

        if (AlreadyRegistered(email))
            return RegistrationStatus.AlreadyRegistered;

        if (!CorrectDomain(email))
            return RegistrationStatus.WrongDomain;

        if (TooManyToday())
            return RegistrationStatus.TooManyRegistrationsToday;

        _emailService.SendWelcomeEmail(email);
        return RegistrationStatus.Ok;
    }

    private bool TooManyToday()
    {
        return _userRepository.GetRegistreredCountToday() > 10;
    }

    private bool CorrectDomain(string email)
    {
        return email.EndsWith("hej.se")|| email.EndsWith("hej.com");
    }

    private bool AlreadyRegistered(string email)
    {
        return _userRepository.Exists(email);
    }
}