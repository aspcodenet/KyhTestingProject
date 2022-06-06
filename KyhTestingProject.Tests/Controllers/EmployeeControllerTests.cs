using System.Collections.Generic;
using AutoMapper;
using KyhTestingProject.Controllers;
using KyhTestingProject.Data;
using KyhTestingProject.Services;
using KyhTestingProject.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace KyhTestingProject.Tests.Controllers;


[TestClass]
public class EmployeeControllerTests
{
    private EmployeeController sut;
    private Mock<IEmployeeService> employeeServiceMock;
    private Mock<IMapper> mapperMock;
    public EmployeeControllerTests()
    {
        employeeServiceMock = new Mock<IEmployeeService>();
        mapperMock = new Mock<IMapper>();

        sut = new EmployeeController(employeeServiceMock.Object,
            mapperMock.Object   
            );
    }
    [TestMethod]
    public void Index_should_return_correct_view()
    {
        employeeServiceMock.Setup(e => e.GetActiveEmployees()).Returns(new ActiveEmployeesResult());
        mapperMock.Setup(e => e.Map<IEnumerable<Employee>, IEnumerable<EmployeeIndexViewModel.EmployeeItem>>(It.IsAny<IEnumerable<Employee>>()))
            .Returns(new List<EmployeeIndexViewModel.EmployeeItem>
            {
                new EmployeeIndexViewModel.EmployeeItem()
            });

        var result = sut.Index() as ViewResult;

        Assert.IsNull(result.ViewName);
    }


    [TestMethod]
    public void Index_should_return_correct_viewmodel()
    {
        employeeServiceMock.Setup(e => e.GetActiveEmployees()).Returns(new ActiveEmployeesResult
        {
            NrOfPeopleWhoHasQuitted = 99,
            Employees = new List<Employee>
            {
                new Employee(),
                new Employee()
            }
        });
        mapperMock.Setup(e => e.Map<IEnumerable<EmployeeIndexViewModel.EmployeeItem>>(It.IsAny<IEnumerable<Employee>>()))
            .Returns(new List<EmployeeIndexViewModel.EmployeeItem>
            {
                new EmployeeIndexViewModel.EmployeeItem(),
                new EmployeeIndexViewModel.EmployeeItem()
            });


        var result = sut.Index() as ViewResult;
        var model = result.Model as EmployeeIndexViewModel;

        Assert.IsInstanceOfType(result.Model, typeof(EmployeeIndexViewModel));
        Assert.AreEqual(99, model.PeopleLeft);
        Assert.AreEqual(2, model.Employees.Count);

    }


    [TestMethod]
    public void Edit_shold_not_save_if_error()
    {
        var ctx = new DefaultHttpContext();
        sut.ControllerContext.HttpContext = ctx;


        sut.ModelState.AddModelError("x", "error");
        var result = sut.Edit(new EmployeeEditViewModel());

        employeeServiceMock.Verify(e=>e.Save(), Times.Never);


    }


}