using System.Collections.Generic;
using System.Threading.Tasks.Dataflow;
using AutoMapper;
using KyhTestingProject.Controllers;
using KyhTestingProject.Data;
using KyhTestingProject.Services;
using KyhTestingProject.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace KyhTestingProject.Tests.Controllers;


[TestClass]
public class EmployeeControllerTests
{
    private EmployeeController sut;
    private Mock<IEmployeeService> employeeServiceMock;
    private Mock<IMapper> mapperMock;

    [TestInitialize]
    public void Initialize()
    {
        employeeServiceMock = new Mock<IEmployeeService>();
        mapperMock = new Mock<IMapper>();
        sut = new EmployeeController(employeeServiceMock.Object, mapperMock.Object);
    }

    [TestMethod]
    public void Index_should_set_people_left()
    {
        //ARRANGE
        int antal = 6;

        employeeServiceMock.Setup(e => e.GetActiveEmployees())
            .Returns( new ActiveEmployeesResult
            {
                Employees = new List<Employee>(),
                NrOfPeopleWhoHasQuitted = antal
            } );

        
        var result = sut.Index() as ViewResult;
        var model = result.Model as EmployeeIndexViewModel;

        //ASSERT
        Assert.AreEqual(antal, model.PeopleLeft);

    }

    [TestMethod]
    public void Edit_should_be_of_correct_model()
    {
        //ARRANGE
        employeeServiceMock.Setup(e => e.Get(1)).Returns(new Employee
        {
            City = "Stockholm",
            Id = 1,
            FirstName = "Stefan",
        });
        mapperMock.Setup(m => m.Map<EmployeeEditViewModel>(It.IsAny<Employee>()))
            .Returns( new EmployeeEditViewModel
            {
                City = "Stockholm",
                Id = 1,
                FirstName = "Stefan"

            } );

        //ACT
        var result = sut.Edit(1) as ViewResult;
        var editViewModel = result.Model as EmployeeEditViewModel;
        
        //ASSERT
        Assert.AreEqual("Stefan", editViewModel.FirstName);
        employeeServiceMock.Verify(e=>e.Get(1), Times.Once);

        //mapperMock.Verify(e => e.Get(1), Times.Once);

    }


}