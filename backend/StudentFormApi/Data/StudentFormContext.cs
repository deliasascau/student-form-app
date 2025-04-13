using Microsoft.EntityFrameworkCore;
using StudentFormApi.Models;

namespace StudentFormApi.Data
{
    public class StudentFormContext : DbContext
    {
        public StudentFormContext(DbContextOptions<StudentFormContext> options) : base(options) { }

        public DbSet<StudentForm> StudentForms { get; set; }
    }
}
