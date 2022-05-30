using System.ComponentModel.DataAnnotations;

namespace KyhTestingProject.Data;

public class Employee
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
    public string City {get; set; }

    [MaxLength(100)]
    public string JobTitle{ get; set; }
    [MaxLength(255)]
    public string JobDescriptor { get; set; }
    [MaxLength(50)]
    public string JobArea { get; set; }
    [MaxLength(50)] 
    public string JobType { get; set; }

    public DateTime Started { get; set; }

    public DateTime Ended { get; set; }


}
