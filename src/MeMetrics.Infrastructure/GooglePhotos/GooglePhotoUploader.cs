using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using MeMetrics.Application.Interfaces;
using MeMetrics.Application.Models;
using MeMetrics.Infrastructure.GooglePhotos.Entities;
using MeMetrics.Objects;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Serilog;

namespace MeMetrics.Infrastructure.GooglePhotos
{
    public class GooglePhotoUploader : IGooglePhotoUploader
    {
        private readonly string _baseUrl = "https://photoslibrary.googleapis.com";
        private readonly IOptions<EnvironmentConfiguration> _configuration;
        internal OAuthToken _token = new OAuthToken();
        private readonly HttpClient _client;
        private readonly ILogger _logger;

        public GooglePhotoUploader(
            IOptions<EnvironmentConfiguration> configuration,
            IHttpClientFactory httpClientFactory,
            ILogger logger)
        {
            _configuration = configuration;
            _client = httpClientFactory.CreateClient();
            _logger = logger;
        }

        public async Task Authenticate()
        {
            var data = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("client_id", _configuration.Value.GOOGLE_CLIENT_ID),
                new KeyValuePair<string, string>("client_secret", _configuration.Value.GOOGLE_CLIENT_SECRET),
                new KeyValuePair<string, string>("refresh_token", _configuration.Value.GOOGLE_PHOTO_REFRESH_TOKEN),
                new KeyValuePair<string, string>("grant_type", "refresh_token"),
            };
            var request = new HttpRequestMessage(HttpMethod.Post, $"https://www.googleapis.com/oauth2/v4/token")
            {
                Content = new FormUrlEncodedContent(data)
            };
            var response = await _client.SendAsync(request);
            var result = await response.Content.ReadAsStringAsync();
            _token = JsonConvert.DeserializeObject<OAuthToken>(result);
        }

        public async Task<AlbumResponse> GetAlbums()
        {
            await Authenticate();
            var response = await SendAsync<AlbumResponse>(HttpMethod.Get, $"/v1/albums");
            return response;
        }

        public async Task<AlbumResponse> CreateAlbum()
        {
            await Authenticate();
            var createAlbumRequest = new {album = new {title = "HisAndHirschel"}};
            var httpContent = new StringContent(JsonConvert.SerializeObject(createAlbumRequest));
            var response = await SendAsync<AlbumResponse>(HttpMethod.Post, $"/v1/albums", httpContent);
            return response;
        }


        public async Task<bool> UploadMedia(IFormFile file)
        {
            try
            {
                if (string.IsNullOrEmpty(_token?.access_token))
                {
                    await Authenticate();
                }

                using (var fileStream = file.OpenReadStream())
                {
                    var data = new byte[fileStream.Length];
                    //Read image into the pixels byte array
                    fileStream.Read(data, 0, (int)fileStream.Length);
                    var httpContent = new ByteArrayContent(data);
                    httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                    httpContent.Headers.Add("X-Goog-Upload-File-Name", file.FileName);
                    httpContent.Headers.Add("X-Goog-Upload-Protocol", "raw");
                    var response = await SendAsync<string>(HttpMethod.Post, $"/v1/uploads", httpContent);
                    await AddMediaToAlbum(response, file.FileName);
                    return true;
                }
            }
            catch (Exception e)
            {
                _logger.Error(e.Message, e);
                return false;
            }
        }

        private async Task<string> AddMediaToAlbum(string uploadToken, string fileName)
        {
            var request = new NewMediaItemRequest()
            {
                albumId = _configuration.Value.GOOGLE_ALBUM_ID,
                newMediaItems = new List<NewMediaItem>()
                {
                    new NewMediaItem()
                    {
                        simpleMediaItem = new SimpleMediaItem()
                        {
                            fileName = fileName, uploadToken = uploadToken
                        }
                    }
                }
            };
            var httpContent = new StringContent(JsonConvert.SerializeObject(request));
            var response = await SendAsync<string>(HttpMethod.Post, $"/v1/mediaItems:batchCreate", httpContent);
            return response;
        }

        private async Task<T> SendAsync<T>(HttpMethod httpMethod, string url, HttpContent content = null, bool allowRetry = true)
        {
            var request = new HttpRequestMessage(httpMethod, $"{_baseUrl}{url}");

            if (content != null)
            {
                request.Content = content;
            }

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token.access_token);

            var response = await _client.SendAsync(request);

            if (response.StatusCode == HttpStatusCode.Unauthorized && allowRetry)
            {
                await Authenticate();
                return await SendAsync<T>(httpMethod, url, content, false);
            }

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"{request.Method} {request.RequestUri.AbsoluteUri} returned {response.StatusCode}");
            }

            var result = await response.Content.ReadAsStringAsync();
            if (typeof(string).IsAssignableFrom(typeof(T)))
            {
                return (T)(object)result;
            }
            return JsonConvert.DeserializeObject<T>(result);
        }
    }
}
