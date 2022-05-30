using KyhTestingProject.Data;

namespace KyhTestingProject.Services;

public class EmployeeService : IEmployeeService
{
    private readonly ApplicationDbContext _context;

    public EmployeeService(ApplicationDbContext context)
    {
        _context = context;
    }
    public (int,IEnumerable<Employee>) GetActiveEmployees()
    {
        var left = _context.Employees.Count(e => e.Ended.Year > 1);
        return (left, _context.Employees.Where(e => e.Ended.Year == 1));
    }
}