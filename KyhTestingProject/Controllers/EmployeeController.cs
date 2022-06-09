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
        var result = _employeeService.GetActiveEmployees();
        model.Employees = 
            _mapper.Map<IEnumerable<EmployeeIndexViewModel.EmployeeItem>>(result.Employees).ToList();
        model.PeopleLeft = result.NrOfPeopleWhoHasQuitted;
        return View(model);
    }

    public IActionResult Edit(int id)
    {
        var result = _employeeService.Get(id);
        var model = _mapper.Map<EmployeeEditViewModel>(result);

        return View(model);
    }

    [HttpPost]
    public IActionResult Edit(EmployeeEditViewModel model)
    { 
        if (ModelState.IsValid)
        {
            var employee = _employeeService.Get(model.Id);
            _mapper.Map(model, employee);

            _employeeService.Save();
            return RedirectToAction("Index");
        }

        return View( model);
    }
}