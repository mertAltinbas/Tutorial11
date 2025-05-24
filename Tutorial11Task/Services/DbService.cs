using Microsoft.EntityFrameworkCore;
using Tutorial11Task.Data;
using Tutorial11Task.DTOs;
using Tutorial11Task.Exceptions;
using Tutorial5.Models;

namespace Tutorial11Task.Services;

public class DbService : IDbService
{
    private readonly DatabaseContext _context;

    public DbService(DatabaseContext context)
    {
        _context = context;
    }

    public async Task AddPrescriptionAsync(AddPrescriptionDTO dto)
    {
        // Check DueDate >= Date
        if (dto.DueDate < dto.Date)
            throw new ConflictException("DueDate must be greater than or equal to Date.");

        // Check medication count
        if (dto.Medicaments.Count > 10)
            throw new ConflictException("Medicaments count must be less than 10.");

        // Check if doctor exists
        var doctor = await _context.Doctors.FindAsync(dto.IdDoctor);
        if (doctor == null)
            throw new NotFoundException("Doctor not found.");

        // Check if patient exists
        var patient = await _context.Patients.FindAsync(dto.Patient.IdPatient);
        if (patient == null)
        {
            patient = new Patient
            {
                FirstName = dto.Patient.FirstName,
                LastName = dto.Patient.LastName,
                BirthDate = dto.Patient.Birthdate
            };
        }

        _context.Patients.Add(patient);
        await _context.SaveChangesAsync();

        // validate and add medicaments
        var medicationIds = dto.Medicaments.Select(m => m.IdMedicament).ToList();
        var existingMedicaments = await _context.Medicaments
            .Where(m => medicationIds.Contains(m.IdMedicament))
            .Select(m => m.IdMedicament)
            .ToListAsync();

        var missingMedicaments = medicationIds.Except(existingMedicaments).ToList();
        if (missingMedicaments.Any())
            throw new ArgumentException($"Medicaments with IDs {string.Join(", ", missingMedicaments)} not found.");

        var prescription = new Prescription
        {
            Date = dto.Date,
            DueDate = dto.DueDate,
            DoctorId = dto.IdDoctor,
            Patient = patient
        };
        _context.Prescriptions.Add(prescription);
        await _context.SaveChangesAsync();

        // Add medicaments to prescription
        foreach (var med in dto.Medicaments)
        {
            var exist = await _context.Medicaments.AnyAsync(m => m.IdMedicament == med.IdMedicament);
            if (!exist)
                throw new NotFoundException($"Medicament with ID {med.IdMedicament} not found.");

            var prescriptionMed = new PrescriptionMedicament
            {
                IdPrescription = prescription.IdPrescription,
                IdMedicament = med.IdMedicament,
                Dose = med.Dose,
                Details = med.Details
            };
            _context.PrescriptionMedicaments.Add(prescriptionMed);
        }

        await _context.SaveChangesAsync();
    }

    public async Task<GetPatientDataDTO> GetPatientByIdAsync(int id)
    {
        var patientDto = await _context.Patients
            .Where(p => p.IdPatient == id)
            .Select(p => new GetPatientDataDTO
            {
                PatientId = p.IdPatient,
                FirstName = p.FirstName,
                LastName = p.LastName,
                DateOfBirth = p.BirthDate,
                Prescriptions = p.Prescriptions.Select(pr => new PrescriptionDTO
                {
                    PrescriptionId = pr.IdPrescription,
                    Date = pr.Date,
                    DueDate = pr.DueDate,
                    Medicaments = pr.PrescriptionMedicaments.Select(pm => new MedicamentsDTO
                    {
                        MedicamentId = pm.Medicament.IdMedicament,
                        Name = pm.Medicament.Name,
                        Dose = pm.Dose ?? 0,
                        Description = pm.Details,
                    }).ToList(),
                    Doctor = new DoctorDTO
                    {
                        DoctorId = pr.Doctor.IdDoctor,
                        FirstName = pr.Doctor.FirstName
                    }
                }).ToList()
            }).FirstOrDefaultAsync();

        return patientDto;
    }
}