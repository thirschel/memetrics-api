using System.Collections.Generic;

namespace MeMetrics.Infrastructure.GooglePhotos.Entities
{
    public class NewMediaItemRequest
    {
        public string albumId { get; set; }
        public List<NewMediaItem> newMediaItems { get; set; }
    }

    public class SimpleMediaItem
    {
        public string fileName { get; set; }
        public string uploadToken { get; set; }
    }

    public class NewMediaItem
    {
        public string description { get; set; }
        public SimpleMediaItem simpleMediaItem { get; set; }
    }
}
