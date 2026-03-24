using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DrugFinder.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly IPatientService patientService;

        public PatientController(IPatientService patientService)
        {
            this.patientService = patientService;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> RegisterPatient([FromBody] Application.DTO.RegitserPatientDTO patientDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await patientService.RegisterPatientAsync(patientDTO);
            if (!result.IsSuccess)
            {
                return BadRequest(result.Error);
            }
            return Ok("Patient registered successfully.");
        }
    }
}
