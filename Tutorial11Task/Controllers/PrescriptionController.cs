using Microsoft.AspNetCore.Mvc;
using Tutorial11Task.DTOs;
using Tutorial11Task.Exceptions;
using Tutorial11Task.Services;

namespace Tutorial11Task.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PrescriptionController : ControllerBase
{
    private readonly IDbService _dbService;

    public PrescriptionController(IDbService dbService)
    {
        _dbService = dbService;
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetPatientById(int id)
    {
        if (id <= 0)
            return BadRequest("Invalid patient ID.");

        var patient = await _dbService.GetPatientByIdAsync(id);
        
        if (patient == null)
            return NotFound($"Patient with ID {id} not found.");
        
        return Ok(patient);
    }
    
    [HttpPost]
    public async Task<IActionResult> AddPrescription([FromBody] AddPrescriptionDTO prescription)
    {
        if (prescription == null)
            return BadRequest("Prescription data is required.");

        try
        {
            await _dbService.AddPrescriptionAsync(prescription);
            return Ok("Prescription created successfully.");
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (ConflictException ex)
        {
            return Conflict(ex.Message);
        }
        catch (Exception)
        {
            return StatusCode(500, "An unexpected error occurred.");
        }
    }


}