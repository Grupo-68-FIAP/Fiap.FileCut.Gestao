using Fiap.FileCut.Core.Interfaces.Applications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fiap.FileCut.Gestao.Api.Controllers
{
    [ApiController]
    [Route("api/v1/videos")]
    public class MenagerController(
        IGestaoApplication gestaoApplication,
        ILogger<MenagerController> logger) : ControllerBase
    {

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> ListAllVideosAsync()
        {
            var email = User.FindFirst("preferred_username")?.Value;
            ArgumentNullException.ThrowIfNull(email);
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            ArgumentNullException.ThrowIfNull(userId);

            logger.LogDebug("Listando todos os vídeos do usuário {UserId}", userId);

            var videoList = await gestaoApplication.ListAllVideosAsync(new Guid(userId), CancellationToken.None);
            return Ok(videoList);
        }

        [Authorize]
        [HttpGet("{videoName}")]
        public async Task<IActionResult> GetVideoAsync(string videoName)
        {
            var email = User.FindFirst("preferred_username")?.Value;
            ArgumentNullException.ThrowIfNull(email);
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            ArgumentNullException.ThrowIfNull(userId);

            logger.LogDebug("Buscando vídeo {VideoName} do usuário {UserId}", videoName, userId);

            var video = await gestaoApplication.GetVideoAsync(new Guid(userId), videoName, CancellationToken.None);
            return File(video.FileStream, "video/mp4");
        }

        [Authorize]
        [HttpGet("{videoName}/frames")]
        public async Task<IActionResult> GetVideoFramesAsync(string videoName)
        {
            var email = User.FindFirst("preferred_username")?.Value;
            ArgumentNullException.ThrowIfNull(email);
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            ArgumentNullException.ThrowIfNull(userId);

            var status = await gestaoApplication.GetFramesAsync(new Guid(userId), videoName, CancellationToken.None);
            return Ok(status);
        }
    }
}
