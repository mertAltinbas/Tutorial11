namespace Tutorial11Task.DTOs;

public class GetPatientDataDTO
{
    public int PatientId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime DateOfBirth { get; set; }
    public List<PrescriptionDTO> Prescriptions { get; set; }
}

public class PrescriptionDTO
{
    public int PrescriptionId { get; set; }
    public DateTime Date { get; set; }
    public DateTime DueDate { get; set; }
    public List<MedicamentsDTO> Medicaments { get; set; }
    public DoctorDTO Doctor { get; set; }
}

public class MedicamentsDTO
{
    public int MedicamentId { get; set; }
    public string Name { get; set; }
    public int Dose { get; set; }
    public string Description { get; set; }
}

public class DoctorDTO
{
    public int DoctorId { get; set; }
    public string FirstName { get; set; }
}