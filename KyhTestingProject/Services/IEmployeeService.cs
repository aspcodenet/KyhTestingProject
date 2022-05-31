using KyhTestingProject.Data;

namespace KyhTestingProject.Services;

public interface IEmployeeService
{
    ActiveEmployeesResult GetActiveEmployees();

}

public class ActiveEmployeesResult
{
    public int NrOfPeopleWhoHasQuitted { get; set; }
    public List<Employee> Employees { get; set; } 

}