using HICProject.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models.HIC
{
    public class PatientContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Patients> Patients { get; set; }
        public DbSet<Medication> Medications { get; set; }
        public DbSet<Staff> Staffs { get; set; }
        public DbSet<Patients_Medication> Patients_Medications { get; set; }
        public DbSet<Appointment> Appointments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public PatientContext(DbContextOptions<PatientContext> options)
            : base(options)
        {
            Database.EnsureCreated();   // создаем базу данных при первом обращении
        }
    }
}
