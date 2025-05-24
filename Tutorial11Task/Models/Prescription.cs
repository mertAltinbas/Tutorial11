using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Tutorial5.Models;

[Table("Prescription")]
public class Prescription
{
    [Key]
    public int IdPrescription { get; set; }
    
    [Column(TypeName = "date")]
    public DateTime Date { get; set; }
    
    [Column(TypeName = "date")]
    public DateTime DueDate { get; set; }
    
    [ForeignKey(nameof(Doctor))]
    public int DoctorId { get; set; }
    
    [ForeignKey(nameof(Patient))]
    public int PatientId { get; set; }
    
    public ICollection<PrescriptionMedicament> PrescriptionMedicaments { get; set; }
    
    public Doctor Doctor { get; set; }
    public Patient Patient { get; set; }
}