using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MusicPlayer
{
    public class Http
    {
        public static Http Instance { get; }

        static Http()
        {
            Instance = new Http();
        }

        public async Task<T> InvokeAsync<T>(string url, string body, CancellationToken token, int timeOutSeconds, string authenScheme = "", string authenParam = "")
        {
            using (var httpClient = new HttpClient(new HttpClientHandler { AutomaticDecompression = DecompressionMethods.GZip }) { Timeout = TimeSpan.FromSeconds(timeOutSeconds) })
            {
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
                if (!string.IsNullOrEmpty(authenScheme))
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(authenScheme, authenParam);

                return Utility.ToObject<T>(await GetValue(httpClient, url, body, token));
            }
        }

        public async Task<string> InvokeAsync(string url, string body, CancellationToken token, int timeOutSeconds, string authenScheme = "", string authenParam = "")
        {
            using (var httpClient = new HttpClient(new HttpClientHandler { AutomaticDecompression = DecompressionMethods.GZip }) { Timeout = TimeSpan.FromSeconds(timeOutSeconds) })
            {
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
                if (!string.IsNullOrEmpty(authenScheme))
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(authenScheme, authenParam);
                return await GetValue(httpClient, url, body, token);
            }
        }

        public async Task<string> GetValue(HttpClient httpClient, string url, string body, CancellationToken cancellation = default)
        {
            var response = await httpClient.PostAsync(url, new StringContent(body, Encoding.UTF8, "application/json"), cancellation);
            response.EnsureSuccessStatusCode();
            var result = string.Empty;
            try
            {
                using var reader = new StreamReader(await response.Content.ReadAsStreamAsync());
                result = reader.ReadToEnd();
            }
            finally
            {
                response.Dispose();
            }
            return result;
        }

        public async Task<T> GetAsync<T>(string url, string token, string cache, int timeOutSeconds)
        {
            using (var httpClient = new HttpClient(new HttpClientHandler { AutomaticDecompression = DecompressionMethods.GZip }) { Timeout = TimeSpan.FromSeconds(timeOutSeconds) })
            {
                if (!string.IsNullOrEmpty(token)) httpClient.DefaultRequestHeaders.Add("postman-token", "93e37968-e61c-16ac-2f6b-8b34932f8159");
                if (!string.IsNullOrEmpty(cache)) httpClient.DefaultRequestHeaders.Add("cache-control", "no-cache");

                var response = await httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                var result = string.Empty;
                try
                {
                    using var reader = new StreamReader(await response.Content.ReadAsStreamAsync());
                    result = reader.ReadToEnd();
                }
                finally
                {
                    response.Dispose();
                }
                return result.ToObject<T>();
            }
        }
    }
}
