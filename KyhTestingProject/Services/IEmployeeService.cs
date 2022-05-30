using KyhTestingProject.Data;

namespace KyhTestingProject.Services;

public interface IEmployeeService
{
    (int,IEnumerable<Employee>) GetActiveEmployees();
}