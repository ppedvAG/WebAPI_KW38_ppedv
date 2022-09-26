using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VideoBroadcastSample.Services;

namespace VideoBroadcastSample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VideoController : ControllerBase
    {
        private readonly IVideoService _videoService;

        public VideoController(IVideoService videoService)
        {
            _videoService = videoService;
        }

        [HttpGet("{name}")]
        public async Task<ActionResult> Index (string name)
        {
            if (string.IsNullOrEmpty(name))
                return BadRequest();

            Stream stream = await _videoService.GetVideoByName(name);
            return new FileStreamResult(stream, "video/mp4");
        }
    }
}
