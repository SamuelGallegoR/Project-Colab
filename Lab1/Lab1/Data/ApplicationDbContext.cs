using System;
using Lab1.Models;
using Microsoft.EntityFrameworkCore;
namespace Lab1.Data
{
    public class ApplicationDbContext : DbContext
    {
        //Controller needs access to db context
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Project> Projects { get; set; }
    }
}


