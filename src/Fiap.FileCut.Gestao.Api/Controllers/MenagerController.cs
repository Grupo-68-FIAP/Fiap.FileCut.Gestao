using Fiap.FileCut.Core.Interfaces.Applications;
using Fiap.FileCut.Core.Interfaces.Services;
using Fiap.FileCut.Core.Objects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;

namespace Fiap.FileCut.Gestao.Api.Controllers
{
    [ApiController]
    [Route("menager")]
    public class MenagerController(
        IGestaoApplication gestaoApplication,
        ILogger<MenagerController> logger) : ControllerBase
    {
        
        [Authorize]
        [HttpGet(Name = "videos")]
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
        [HttpGet("{videoName}", Name = "video")]
        public async Task<IActionResult> GetVideoAsync(string videoName)
        {
            var email = User.FindFirst("preferred_username")?.Value;
            ArgumentNullException.ThrowIfNull(email);
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            ArgumentNullException.ThrowIfNull(userId);

            logger.LogDebug("Buscando vídeo {VideoName} do usuário {UserId}", videoName, userId);

            var video = await gestaoApplication.GetVideoAsync(new Guid(userId), videoName, CancellationToken.None);
            return File(video.OpenReadStream(), "video/mp4");
        }

        [Authorize]
        [HttpGet("{videoName}/status", Name = "videoStatus")]
        public async Task<IActionResult> GetVideoMetadataAsync(string videoName)
        {
            var email = User.FindFirst("preferred_username")?.Value;
            ArgumentNullException.ThrowIfNull(email);
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            ArgumentNullException.ThrowIfNull(userId);

            logger.LogDebug("Buscando status do vídeo {VideoName} do usuário {UserId}", videoName, userId);

            var status = await gestaoApplication.GetVideoMetadataAsync(new Guid(userId), videoName, CancellationToken.None);
            return Ok(status);
        }
    }
}
