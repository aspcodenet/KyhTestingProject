using KyhTestingProject.Data;

namespace KyhTestingProject.Services.Salary;

public interface ISalaryCalculationService
{
    SalaryCalculationStatus CalculateService(string ssn, int antalTimmar);
}

public enum SalaryCalculationStatus
{
    Ok,
    EmployeeNotFound,
    NegativeNrOfHours,
}

public interface IEmployeeRepository
{
    Employee? GetBySSN(string ssn);
}

public class SalaryCalculationService : ISalaryCalculationService
{
    private readonly IEmployeeRepository _employeeRepository;

    public SalaryCalculationService(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }
    public SalaryCalculationStatus CalculateService(string ssn, int antalTimmar)
    {
        var employee = _employeeRepository.GetBySSN(ssn);
        if (employee == null)
            return SalaryCalculationStatus.EmployeeNotFound;
        // salary = antaltimmar *´....
        // paymentService.Pay(ssn, salary)
        return SalaryCalculationStatus.Ok;
    }
}