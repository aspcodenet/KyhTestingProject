using System.Collections.Generic;
using KyhTestingProject.Data;
using KyhTestingProject.Services.Salary;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KyhTestingProject.Tests.Services.Salary;

public class FakeEmployeeRepository : IEmployeeRepository
{
    public List<string> SSNs { get; set; } = new List<string>();
    public Employee? GetBySSN(string ssn)
    {
        if (SSNs.Contains(ssn)) return new Employee();
        return null;
    }
}

[TestClass]
public class SalaryCalculationServiceTests
{
    private SalaryCalculationService _sut;
    private FakeEmployeeRepository _employeeRepository;

    public SalaryCalculationServiceTests()
    {
        _employeeRepository = new FakeEmployeeRepository();
        _sut = new SalaryCalculationService(_employeeRepository);
    }

    [TestMethod]
    public void When_ssn_not_found_in_database_should_return_error()
    {
        // ARRANGE
        _employeeRepository.SSNs.Clear();

        //ACT
        var result = _sut.CalculateService("3321231231", 12);
        //ASSERT
        Assert.AreEqual(SalaryCalculationStatus.EmployeeNotFound,result);
    }


    [TestMethod]
    public void When_ssn_is_found_in_database_should_return_ok()
    {
        // ARRANGE
        _employeeRepository.SSNs.Add("3");
        //ACT
        var result = _sut.CalculateService("3", 12);
        //ASSERT
        Assert.AreEqual(SalaryCalculationStatus.Ok, result);
    }





}