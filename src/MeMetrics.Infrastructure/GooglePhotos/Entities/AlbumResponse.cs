using System.Collections.Generic;

namespace MeMetrics.Infrastructure.GooglePhotos.Entities
{
    public class AlbumResponse
    {
        public List<Album> albums { get; set; }
        public string nextPageToken { get; set; }
    }

    public class Album
    {
        public string id { get; set; }
        public string title { get; set; }
        public string productUrl { get; set; }
        public string coverPhotoBaseUrl { get; set; }
        public string coverPhotoMediaItemId { get; set; }
        public string isWriteable { get; set; }
        public string mediaItemsCount { get; set; }
    }
}
