using Microsoft.EntityFrameworkCore;
using SynelTask.Models;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace SynelTask;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    // Employees Column
    public DbSet<Employee> Employees { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Employee>()
            .HasKey(e => e.PayrollNumber); 
    }
}
