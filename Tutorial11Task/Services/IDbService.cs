using Tutorial11Task.DTOs;

namespace Tutorial11Task.Services;

public interface IDbService
{
    Task AddPrescriptionAsync(AddPrescriptionDTO prescription);
    Task<GetPatientDataDTO> GetPatientByIdAsync(int id);
}