using Application.Services;
using Infrastructure.Repos.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DrugFinder.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PharmacyResponseController : ControllerBase
    {
        private readonly IPharmacyResponseService pharamcyResponse;

        public PharmacyResponseController(IPharmacyResponseService pharamcyResponse)
        {
            this.pharamcyResponse = pharamcyResponse;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Pharmacy")]

        [HttpPost("CreatePharmacyResponse")]
        public async Task<IActionResult> CreatePharmacyResponse(Application.DTO.PharmacyResponseDTO DTO)
        {
            var result = await pharamcyResponse.CreatePharmacyResponseAsync(DTO);
            if (!result.IsSuccess)
            {
                return BadRequest(result.Error);
            }
            return Ok(result.Value);
        }
    }
}
