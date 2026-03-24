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
    public class DrugRequestController : ControllerBase
    {
        private readonly IDrugRequestService drugRequestService;

        public DrugRequestController(IDrugRequestService drugRequestService )
        {
            this.drugRequestService = drugRequestService;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Patient")]
        [HttpPost("CreateRequest")]
        public async Task<IActionResult> CreateDrugRequest([FromBody] List<Application.DTO.CreateDrugRequestDTO> drugRequestDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await drugRequestService.CreateDrugRequestAsync(drugRequestDTO);
            if (!result.IsSuccess)
            {
                return BadRequest(result.Error);
            }
            return Ok("Drug request created successfully.");
        }
    }
}
