using Bogus;
using Bogus.Extensions.UnitedStates;
using Microsoft.EntityFrameworkCore;

namespace KyhTestingProject.Data;

public class DataInitializer
{
    private readonly ApplicationDbContext _context;

    public DataInitializer(ApplicationDbContext context)
    {
        _context = context;
    }

    public void SeedData()
    {
        _context.Database.Migrate();
        if (_context.Employees.Any()) return;

        var employeeFaker = new Faker<Employee>()
            .RuleFor(o => o.City, f => f.Address.City())
            .RuleFor(o => o.Email, f => f.Internet.Email())
            .RuleFor(o => o.FirstName, f => f.Name.FirstName())
            .RuleFor(o => o.LastName, f => f.Name.LastName())
            .RuleFor(o => o.JobArea, f => f.Name.JobArea())
            .RuleFor(o => o.JobDescriptor, f => f.Name.JobDescriptor())
            .RuleFor(o => o.JobTitle, f => f.Name.JobTitle())
            .RuleFor(o => o.JobType, f => f.Name.JobType())
            .RuleFor(o => o.SSN, f => f.Person.Ssn())
            .RuleFor(o => o.StreetAddress, f => f.Address.StreetAddress())
            .RuleFor(o => o.Started, f => f.Date.Past(10,DateTime.Now))
            .RuleFor(o => o.ZipCode, f => f.Address.ZipCode());
            

        var faker = new Faker("en");
        for (int i = 0; i < 50; i++)
        {
            var employee = employeeFaker.Generate();
            if (faker.Random.Int(0, 100) < 10)
            {
                var ended = employee.Started.AddDays(faker.Random.Int(100, (int)(DateTime.Now - employee.Started).TotalDays));
                employee.Ended = ended;
            }
            _context.Employees.Add(employee);
        }

        _context.SaveChanges();

    }
}