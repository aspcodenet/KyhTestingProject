using KyhTestingProject.Data;

namespace KyhTestingProject.Services;

public class EmployeeService : IEmployeeService
{
    private readonly ApplicationDbContext _context;

    public EmployeeService(ApplicationDbContext context)
    {
        _context = context;
    }
    public ActiveEmployeesResult GetActiveEmployees()
    {
        var left = _context.Employees.Count(e => e.Ended.Year > 1);
        return new ActiveEmployeesResult
        {
            NrOfPeopleWhoHasQuitted = left,
            Employees =  _context.Employees.Where(e => e.Ended.Year == 1).ToList()
        };
    }

    public Employee Get(int id)
    {
        return _context.Employees.First(e => e.Id == id);
    }

    public void Save()
    {
        _context.SaveChanges();
    }
}