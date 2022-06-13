using ClubManagementSystem.Enums;
using ClubManagementSystem.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UploadMembersController : ControllerBase
    {
        private readonly IUploadFileService _uploadFileService;

        public UploadMembersController(IUploadFileService uploadFileService)
        {
            _uploadFileService = uploadFileService;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromForm] IFormFile members)
        {
            var (outcome, newMembersCount) = await _uploadFileService
                .UploadMembers(members.OpenReadStream());

            switch (outcome)
            {
                case UploadMembersOutcome.Success:
                    return Ok(newMembersCount);
                case UploadMembersOutcome.FileNotFound:
                case UploadMembersOutcome.EmptyFile:
                case UploadMembersOutcome.MissingRequiredColumns:
                case UploadMembersOutcome.InvalidFile:
                case UploadMembersOutcome.InsertFailed:
                    return UnprocessableEntity(outcome.ToString());
                default:
                    return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
