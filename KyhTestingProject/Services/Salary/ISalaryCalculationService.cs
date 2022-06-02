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

public interface IPaymentService
{
    void Pay(string ssn, decimal salary);
}

public class SalaryCalculationService : ISalaryCalculationService
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IPaymentService _paymentService;

    public SalaryCalculationService(IEmployeeRepository employeeRepository,
        IPaymentService paymentService)
    {
        _employeeRepository = employeeRepository;
        _paymentService = paymentService;
    }
    public SalaryCalculationStatus CalculateService(string ssn, int antalTimmar)
    {
        var employee = _employeeRepository.GetBySSN(ssn);
        if (employee == null)
            return SalaryCalculationStatus.EmployeeNotFound;


        var antalYears = DateTime.Now.Year - employee.Started.Year;
        var faktor = 100.0m * (1.0m + (antalYears / 10.0m));
        var salary = antalTimmar * faktor;
        _paymentService.Pay(ssn, salary);
        return SalaryCalculationStatus.Ok;
    }
}