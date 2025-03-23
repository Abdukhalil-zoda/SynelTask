using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SynelTask.Helpers;
using SynelTask.Models;
using System.Globalization;

namespace SynelTask.Controllers;

public class EmployeeController : Controller
{
    private readonly ApplicationDbContext _context;

    // Constructor injects the ApplicationDbContext
    public EmployeeController(ApplicationDbContext context)
    {
        _context = context;
    }

    // Displays the main Employee page with the employee list sorted by Surname
    public async Task<IActionResult> Index(string orderBy = "Surename", string sortDirection = "asc", string? searchQuery = null)
    {
        var employees = _context.Employees.AsQueryable();

        // Search by query
        if (!string.IsNullOrWhiteSpace(searchQuery))
        {
            searchQuery = searchQuery.ToLower();

            employees = employees.Where(e =>
                e.PayrollNumber.ToLower().Contains(searchQuery) ||
                e.Forenames.ToLower().Contains(searchQuery) ||
                e.Surname.ToLower().Contains(searchQuery) ||
                EF.Functions.Like(e.DateOfBirth.ToString(), $"%{searchQuery}%") ||
                (e.Telephone != null && e.Telephone.ToLower().Contains(searchQuery)) ||
                (e.Mobile != null && e.Mobile.ToLower().Contains(searchQuery)) ||
                (e.Address != null && e.Address.ToLower().Contains(searchQuery)) ||
                (e.Address2 != null && e.Address2.ToLower().Contains(searchQuery)) ||
                (e.Postcode != null && e.Postcode.ToLower().Contains(searchQuery)) ||
                (e.EmailHome != null && e.EmailHome.ToLower().Contains(searchQuery)) ||
                EF.Functions.Like(e.StartDate.ToString(), $"%{searchQuery}%")
            );
        }


        // Sort by column
        employees = orderBy switch
        {
            "PayrollNumber" => sortDirection == "asc" ? employees.OrderBy(e => e.PayrollNumber) : employees.OrderByDescending(e => e.PayrollNumber),
            "Forenames" => sortDirection == "asc" ? employees.OrderBy(e => e.Forenames) : employees.OrderByDescending(e => e.Forenames),
            "Surname" => sortDirection == "asc" ? employees.OrderBy(e => e.Surname) : employees.OrderByDescending(e => e.Surname),
            "DateOfBirth" => sortDirection == "asc" ? employees.OrderBy(e => e.DateOfBirth) : employees.OrderByDescending(e => e.DateOfBirth),
            "StartDate" => sortDirection == "asc" ? employees.OrderBy(e => e.StartDate) : employees.OrderByDescending(e => e.StartDate),
            _ => sortDirection == "asc" ? employees.OrderBy(e => e.Surname) : employees.OrderByDescending(e => e.Surname)
        };

        var employeeList = await employees.ToListAsync();
        ViewData["SortDirection"] = sortDirection;
        ViewData["SortColumn"] = orderBy;
        ViewData["CurrentFilter"] = searchQuery;
        return View(employeeList);

    }

    // Handles file upload and CSV import
    [HttpPost]
    public async Task<IActionResult> Import(IFormFile file)
    {
        // Check if file is provided
        if (file == null || file.Length == 0)
        {
            TempData["Error"] = "Please select a valid CSV file!";
            return RedirectToAction("Index");
        }

        try
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ",", 
                HeaderValidated = null,
                MissingFieldFound = null
            };


            using var reader = new StreamReader(file.OpenReadStream());
            using var csv = new CsvReader(reader, config);
            csv.Context.TypeConverterCache.AddConverter<DateTime>(new CustomDateTimeConverter());
            // Parse CSV data into Employee model
            var employees = csv.GetRecords<Employee>().ToList();

            if (employees.Any())
            {
                foreach (var employee in employees)
                {
                    var existingEmployee = await _context.Employees
                        .FirstOrDefaultAsync(e => e.PayrollNumber == employee.PayrollNumber);

                    if (existingEmployee != null)
                    {
                        // Update existing
                        _context.Entry(existingEmployee).CurrentValues.SetValues(employee);
                    }
                    else
                    {
                        // Add new
                        _context.Employees.Add(employee);
                    }
                }

                await _context.SaveChangesAsync();

                TempData["Success"] = $"{employees.Count} employees imported successfully!";
            }
        }
        catch (Exception ex)
        {
            TempData["Error"] = $"Error importing file: {ex.Message}";
        }

        return RedirectToAction("Index");
    }

    // GET: Edit employee details
    public async Task<IActionResult> Edit(string id)
    {
        if (id == null) return NotFound();

        var employee = await _context.Employees.FindAsync(id);
        if (employee == null) return NotFound();

        return View(employee);
    }

    // POST: Save edited employee
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string id, Employee employee)
    {
        if (id != employee.PayrollNumber) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(employee);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Employee updated successfully!";
            }
            catch (DbUpdateConcurrencyException)
            {
                TempData["Error"] = "Error updating employee. Try again.";
            }

            return RedirectToAction("Index");
        }

        return View(employee);
    }

    // GET: Delete employee
    public async Task<IActionResult> Delete(string id)
    {
        var employee = await _context.Employees.FindAsync(id);
        if (employee == null) return NotFound();

        return View(employee);
    }

    // POST: Confirm delete
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(string id)
    {
        var employee = await _context.Employees.FindAsync(id);
        if (employee != null)
        {
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Employee deleted successfully!";
        }

        return RedirectToAction("Index");
    }
}

