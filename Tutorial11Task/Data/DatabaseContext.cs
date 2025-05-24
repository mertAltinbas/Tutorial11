using Microsoft.EntityFrameworkCore;
using Tutorial5.Models;

namespace Tutorial11Task.Data;

public class DatabaseContext : DbContext
{
    public DbSet<Doctor> Doctors { get; set; }
    public DbSet<Medicament> Medicaments { get; set; }
    public DbSet<Patient> Patients { get; set; }
    public DbSet<Prescription> Prescriptions { get; set; }
    public DbSet<PrescriptionMedicament> PrescriptionMedicaments { get; set; }

    protected DatabaseContext()
    {
    }

    public DatabaseContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Patient class without annotations, you can use Fluent API to configure the model like this
        modelBuilder.Entity<Patient>(a =>
        {
            a.ToTable("Patient");

            a.HasKey(e => e.IdPatient);
            a.Property(e => e.FirstName).HasMaxLength(100);
            a.Property(e => e.LastName).HasMaxLength(100);
            a.Property(e => e.BirthDate).HasColumnType("date");
        });
        
        // Fill the tables with data
        modelBuilder.Entity<Doctor>().HasData(new List<Doctor>()
        {
            new Doctor() { IdDoctor = 1, FirstName = "John", LastName = "Doe", Email = "doeJohn@email.com" },
            new Doctor() { IdDoctor = 2, FirstName = "Jane", LastName = "Doe", Email = "asd@email.com" },
            new Doctor() { IdDoctor = 3, FirstName = "Jack", LastName = "Smith", Email = "bober@email.com" }
        });
        
        modelBuilder.Entity<Medicament>().HasData(new List<Medicament>()
        {
            new Medicament() { IdMedicament = 1, Name = "Aspirin", Description = "Pain reliever", Type = "Painkiller" },
            new Medicament() { IdMedicament = 2, Name = "Ibuprofen", Description = "Anti-inflammatory", Type = "Painkiller" },
            new Medicament() { IdMedicament = 3, Name = "Paracetamol", Description = "Fever reducer", Type = "Painkiller" }
        });
        
        modelBuilder.Entity<Patient>().HasData(new List<Patient>()
        {
            new Patient() { IdPatient = 1, FirstName = "Alice", LastName = "Johnson", BirthDate = new DateTime(1994, 1, 15) },
            new Patient() { IdPatient = 2, FirstName = "Bob", LastName = "Smith", BirthDate = new DateTime(1999, 5, 10) },
            new Patient() { IdPatient = 3, FirstName = "Charlie", LastName = "Brown", BirthDate = new DateTime(1984, 12, 5) }
        });

        modelBuilder.Entity<Prescription>().HasData(new List<Prescription>()
        {
            new Prescription() { IdPrescription = 1, Date = new DateTime(2024, 12, 1), DueDate = new DateTime(2025, 1, 1), DoctorId = 1, PatientId = 1 },
            new Prescription() { IdPrescription = 2, Date = new DateTime(2024, 12, 5), DueDate = new DateTime(2025, 1, 5), DoctorId = 2, PatientId = 2 },
            new Prescription() { IdPrescription = 3, Date = new DateTime(2024, 12, 10), DueDate = new DateTime(2025, 1, 10), DoctorId = 3, PatientId = 3 }
        });

        
        modelBuilder.Entity<PrescriptionMedicament>().HasData(new List<PrescriptionMedicament>()
        {
            new PrescriptionMedicament() { IdPrescription = 1, IdMedicament = 1, Dose = 2, Details = "Take twice a day" },
            new PrescriptionMedicament() { IdPrescription = 1, IdMedicament = 2, Dose = 1, Details = "Take once a day" },
            new PrescriptionMedicament() { IdPrescription = 2, IdMedicament = 3, Dose = 1, Details = "Take once a day" },
            new PrescriptionMedicament() { IdPrescription = 3, IdMedicament = 1, Dose = 2, Details = "Take twice a day" },
        });
    }
}