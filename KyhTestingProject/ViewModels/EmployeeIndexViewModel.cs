namespace KyhTestingProject.ViewModels;

public class EmployeeIndexViewModel
{
    public int PeopleLeft { get; set; }

    public List<EmployeeItem> Employees { get; set; }

    public class EmployeeItem
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string JobTitle { get; set; }
    }
}