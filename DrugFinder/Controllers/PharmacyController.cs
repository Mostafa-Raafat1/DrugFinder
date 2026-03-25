using Application.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DrugFinder.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PharmacyController : ControllerBase
    {
        private readonly IPharmacyService pharmacyService;

        public PharmacyController(IPharmacyService pharmacyService)
        {
            this.pharmacyService = pharmacyService;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> RegisterPharmacy([FromBody] Application.DTO.RegisterPharmacyDTO pharmacyDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await pharmacyService.RegisterPharmacyAsync(pharmacyDTO);
            if (!result.IsSuccess)
            {
                return BadRequest(result.Error);
            }
            return Ok("Pharmacy registered successfully.");
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Pharmacy")]
        [HttpGet("GetNotfications")]
        public async Task<IActionResult> GetNotificationsForPharmacy()
        {
            var result = await pharmacyService.GetNotificationsForPharmacyAsync();
            if (!result.IsSuccess)
            {
                return BadRequest(result.Error);
            }
            return Ok(result.Value);
        }
    }
}
