using KyhTestingProject.Services.Salary;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KyhTestingProject.Tests.Services.Salary;

[TestClass]
public class SalaryCalculationServiceTests
{
    private SalaryCalculationService _sut;

    public SalaryCalculationServiceTests()
    {
        _sut = new SalaryCalculationService();
    }


}