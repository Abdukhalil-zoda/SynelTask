using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using CsvHelper.Configuration.Attributes;

namespace SynelTask.Models;

public class Employee
{
    [Key]
    [Column("Payroll_Number")]
    [Name("Personnel_Records.Payroll_Number")]
    [Required(ErrorMessage = "Payroll Number is required")]
    [MaxLength(20, ErrorMessage = "Payroll Number can't exceed 20 characters")]
    public required string PayrollNumber { get; set; }

    [Column("Forenames")]
    [Name("Personnel_Records.Forenames")]
    [Required(ErrorMessage = "Forenames are required")]
    [MaxLength(100, ErrorMessage = "Forenames can't exceed 100 characters")]
    public required string Forenames { get; set; }

    [Column("Surname")]
    [Name("Personnel_Records.Surname")]
    [Required(ErrorMessage = "Surname is required")]
    [MaxLength(50, ErrorMessage = "Surname can't exceed 50 characters")]
    public required string Surname { get; set; }

    [Column("Date_of_Birth")]
    [Name("Personnel_Records.Date_of_Birth")]
    [Required(ErrorMessage = "Date of Birth is required")]
    [DataType(DataType.Date)]
    public DateTime DateOfBirth { get; set; }

    [Column("Telephone")]
    [Name("Personnel_Records.Telephone")]
    [Phone(ErrorMessage = "Invalid Telephone number")]
    [MaxLength(20)]
    public string? Telephone { get; set; }

    [Column("Mobile")]
    [Name("Personnel_Records.Mobile")]
    [Phone(ErrorMessage = "Invalid Mobile number")]
    [MaxLength(20)]
    public string? Mobile { get; set; }

    [Column("Address")]
    [Name("Personnel_Records.Address")]
    [MaxLength(200)]
    public string? Address { get; set; }

    [Column("Address_2")]
    [Name("Personnel_Records.Address_2")]
    [MaxLength(200)]
    public string? Address2 { get; set; }

    [Column("Postcode")]
    [Name("Personnel_Records.Postcode")]
    [MaxLength(10)]
    public string? Postcode { get; set; }

    [Column("EMail_Home")]
    [Name("Personnel_Records.EMail_Home")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    [MaxLength(100)]
    public string? EmailHome { get; set; }

    [Column("Start_Date")]
    [Name("Personnel_Records.Start_Date")]
    [Required(ErrorMessage = "Start Date is required")]
    [DataType(DataType.Date)]
    public DateTime StartDate { get; set; }
}
