namespace VideoBroadcastSample.Services
{
    public interface IVideoService
    {
        Task<Stream> GetVideoByName(string name);
    }
}
