using Fiap.FileCut.Core.Interfaces.Applications;
using Fiap.FileCut.Core.Objects;
using Fiap.FileCut.Gestao.Api.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace Fiap.FileCut.Gestao.Api.UnitTests;

public class MenagerControllerUnitTest
{
    [Fact]
    public async Task ListAllVideosAsync_WhenCalled_ReturnsAllVideos()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var email = "test@example.com";
        var gestaoApplication = new Mock<IGestaoApplication>();
        gestaoApplication.Setup(x => x.ListAllVideosAsync(It.IsAny<Guid>(), CancellationToken.None))
            .ReturnsAsync([new("video1.mp4"), new("video2.mp4"), new("video3.mp4")]);
        var logger = new Mock<ILogger<MenagerController>>();
        var controller = new MenagerController(gestaoApplication.Object, logger.Object);
        controller.SetUserAuth(userId, email);
        // Act
        var result = await controller.ListAllVideosAsync();
        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var videos = Assert.IsAssignableFrom<IEnumerable<VideoMetadata>>(okResult.Value);
        Assert.Equal(3, videos.Count());
    }

	[Fact]
	public async Task GetVideoAsync_WhenCalled_ReturnsVideo()
	{
		// Arrange
		var userId = Guid.NewGuid();
		var email = "test@example.com";
		var videoName = "video1.mp4";

		var gestaoApplication = new Mock<IGestaoApplication>();

		var memoryStream = new MemoryStream();
		var video = new FileStreamResult(videoName, memoryStream);

		gestaoApplication.Setup(x => x.GetVideoAsync(userId, videoName, CancellationToken.None))
			.ReturnsAsync(video);

		var logger = new Mock<ILogger<MenagerController>>();
		var controller = new MenagerController(gestaoApplication.Object, logger.Object);
		controller.SetUserAuth(userId, email);

		// Act
		var result = await controller.GetVideoAsync(videoName);

		// Assert
		var fileResult = Assert.IsType<FileStreamResult>(result);
		Assert.Equal("video/mp4", fileResult.ContentType);
		Assert.Equal(videoName, fileResult.FileDownloadName);
	}


	[Fact]
    public async Task GetVideoStatusAsync_WhenCalled_ReturnsVideoStatus()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var email = "test@example.com";
        var videoName = "video1.mp4";
        var gestaoApplication = new Mock<IGestaoApplication>();
        gestaoApplication.Setup(x => x.GetVideoMetadataAsync(userId, videoName, CancellationToken.None))
            .ReturnsAsync(new VideoMetadata(videoName));
        var logger = new Mock<ILogger<MenagerController>>();
        var controller = new MenagerController(gestaoApplication.Object, logger.Object);
        controller.SetUserAuth(userId, email);
        // Act
        var result = await controller.GetVideoMetadataAsync(videoName);
        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var video = Assert.IsType<VideoMetadata>(okResult.Value);
        Assert.Equal(videoName, video.Name);
        Assert.Equal(VideoState.PENDING, video.State);
    }
}