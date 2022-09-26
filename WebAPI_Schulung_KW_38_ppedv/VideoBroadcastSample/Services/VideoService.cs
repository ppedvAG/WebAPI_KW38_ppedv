namespace VideoBroadcastSample.Services
{
    public class VideoService : IVideoService
    {
        private HttpClient _client; 
        public VideoService(HttpClient client)
        {
            _client = client;
        }

        public async Task<Stream> GetVideoByName(string name)
        {
            string url = string.Empty;

            switch (name)
            {
                case "fugees":
                    url = "http://gartner.gosimian.com/assets/videos/Fugees_ReadyOrNot_278-WIREDRIVE.mp4";
                    break;
                case "xyz":
                    url = "http://gartner.gosimian.com/assets/videos/George_Michael_MV-WIREDRIVE.mp4";
                    break;
                default: 
                    url = string.Empty;
                    break; 
            }

            return await _client.GetStreamAsync(url);
        }
    }
}
