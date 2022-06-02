using System;
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
        if (SSNs.Contains(ssn)) return new Employee {Started = DateTime.Now.AddYears(-3)};
        return null;
    }
}
public class FakePaymentService : IPaymentService
{
    public bool HasBeenCalled = false;
    public string SSN;
    public decimal Salary;
    public void Pay(string ssn, decimal salary)
    {
        SSN = ssn;
        Salary = salary;
        HasBeenCalled = true;
    }
}
[TestClass]
public class SalaryCalculationServiceTests
{
    private SalaryCalculationService _sut;
    private FakeEmployeeRepository _employeeRepository;
    private FakePaymentService _fakePaymentService;

    public SalaryCalculationServiceTests()
    {
        _fakePaymentService = new FakePaymentService();
        _employeeRepository = new FakeEmployeeRepository();
        _sut = new SalaryCalculationService(_employeeRepository, _fakePaymentService);
    }

    [TestMethod]
    public void When_ssn_not_found_in_database_should_return_error()
    {
        // ARRANGE
        _employeeRepository.SSNs.Clear();

        //ACT
        var result = _sut.CalculateService("19720101-0101", 12);
        //ASSERT
        Assert.AreEqual(SalaryCalculationStatus.EmployeeNotFound,result);
    }


    [TestMethod]
    public void When_ssn_is_found_in_database_should_return_ok()
    {
        // ARRANGE
        _employeeRepository.SSNs.Add("19720101-0101");
        //ACT
        var result = _sut.CalculateService("19720101-0101", 12);
        //ASSERT
        Assert.AreEqual(SalaryCalculationStatus.Ok, result);
    }


    [TestMethod]
    public void When_ok_a_salary_payment_is_made()
    {
        // ARRANGE
        _employeeRepository.SSNs.Add("19720101-0101");
        _fakePaymentService.HasBeenCalled = false;

        //ACT
        var result = _sut.CalculateService("19720101-0101", 12);
        //ASSERT
        //Verifiera att en betalning är gjord
        Assert.IsTrue(_fakePaymentService.HasBeenCalled);
    }


    [TestMethod]
    public void Salary_Is_Calculated_Correctly()
    {
        //ARRANGE
        _employeeRepository.SSNs.Add("19720101-0101");
        _fakePaymentService.SSN = "";
        _fakePaymentService.Salary = 0;

        var result = _sut.CalculateService("19720101-0101", 12);

        //VERIFIERA ATT Salary som beräknades där inuti är 1560
        Assert.AreEqual(1560, _fakePaymentService.Salary);
        Assert.AreEqual("19720101-0101", _fakePaymentService.SSN);
    }



}