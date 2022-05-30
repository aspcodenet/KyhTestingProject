using AutoMapper;
using KyhTestingProject.Data;
using KyhTestingProject.Services;
using KyhTestingProject.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace KyhTestingProject.Controllers;

public class EmployeeController : Controller
{
    private readonly IEmployeeService _employeeService;
    private readonly IMapper _mapper;

    public EmployeeController(IEmployeeService employeeService, IMapper mapper)
    {
        _employeeService = employeeService;
        _mapper = mapper;
    }

    public IActionResult Index()
    {
        var model = new EmployeeIndexViewModel();
        var (left, employees) = _employeeService.GetActiveEmployees();
        model.Employees = _mapper.Map<IEnumerable<EmployeeIndexViewModel.EmployeeItem>>(employees).ToList();
        model.PeopleLeft = left;
        return View(model);
    }
}