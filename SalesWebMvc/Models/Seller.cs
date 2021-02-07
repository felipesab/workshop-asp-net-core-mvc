using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SalesWebMvc.Models
{
  public class Seller
  {
    public int Id { get; set; }

    [Required(ErrorMessage = "{0} required")]
    [StringLength(60, MinimumLength = 3, ErrorMessage = "{0} lenght must have between {2} and {1} characters")]
    public string Name { get; set; }

    [DataType(DataType.EmailAddress)]
    [Required(ErrorMessage = "{0} required")]
    [EmailAddress(ErrorMessage = "Enter a valid email format")]
    public string Email { get; set; }

    [Display(Name = "Birth Date")]
    [Required(ErrorMessage = "{0} required")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
    public DateTime Date { get; set; }

    [Display(Name = "Base Salary")]
    [Required(ErrorMessage = "{0} required")]
    [DataType(DataType.Currency)]
    public double BaseSalary { get; set; }
    public Department Department { get; set; }
    public int DepartmentId { get; set; }
    public ICollection<SalesRecord> Sales { get; set; } = new List<SalesRecord>();

    public Seller()
    {

    }

    public Seller(int id, string name, string email, DateTime date, double baseSalary, Department department)
    {
      Id = id;
      Name = name;
      Email = email;
      Date = date;
      BaseSalary = baseSalary;
      Department = department;
    }

    public void AddSales(SalesRecord sr)
    {
      if (sr == null)
        throw new ArgumentNullException("");

      Sales.Add(sr);
    }

    public void RemoveSales(SalesRecord sr)
    {
      if (sr == null)
        throw new ArgumentNullException("");

      Sales.Remove(sr);
    }

    public double TotalSales(DateTime initial, DateTime final)
    {
      return Sales.Where(s => s.Date >= initial && s.Date <= final).Sum(s => s.Amount);
    }
  }
}
