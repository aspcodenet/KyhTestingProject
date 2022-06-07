using System.ComponentModel.DataAnnotations;

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


public class EmployeeEditViewModel
{
    public int Id { get; set; }
    [MaxLength(50)]
    public string FirstName { get; set; }
    [MaxLength(50)]
    public string LastName { get; set; }
    [MaxLength(80)]
    public string Email { get; set; }

    [MaxLength(20)]
    public string SSN { get; set; }

    [MaxLength(50)]
    public string StreetAddress { get; set; }
    [MaxLength(10)]
    public string ZipCode { get; set; }
    [MaxLength(50)]
    public string City { get; set; }

    [MaxLength(100)]
    public string JobTitle { get; set; }
    [MaxLength(255)]
    public string JobDescriptor { get; set; }
    [MaxLength(50)]
    public string JobArea { get; set; }
    [MaxLength(50)]
    public string JobType { get; set; }

    public DateTime Started { get; set; }

    public DateTime Ended { get; set; }
}