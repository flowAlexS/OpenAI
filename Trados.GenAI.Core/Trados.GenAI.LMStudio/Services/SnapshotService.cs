using PuppeteerSharp;
using System.Net;

namespace Trados.GenAI.LMStudio.Services
{
    public class SnapshotService
    {
        private readonly HttpClient _httpClient;

        public SnapshotService()
        {
            _httpClient = new HttpClient(new HttpClientHandler
            {
                AllowAutoRedirect = true
            });

            _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64)");
        }

        /// <summary>
        /// Generates a snapshot from a URI and returns a Base64 data URL string.
        /// </summary>
        public async Task<string> GenerateSnapshotAsync(string uri)
        {
            uri = WebUtility.HtmlDecode(uri);

            // Get the content type
            var response = await _httpClient.GetAsync(uri, HttpCompletionOption.ResponseHeadersRead);
            response.EnsureSuccessStatusCode();
            string mediaType = response.Content.Headers.ContentType?.MediaType ?? "application/octet-stream";

            byte[] bytes;

            if (mediaType.StartsWith("image"))
            {
                // Image URL: read bytes directly
                bytes = await response.Content.ReadAsByteArrayAsync();
            }
            else if (mediaType == "text/html")
            {
                // HTML page: take a screenshot using PuppeteerSharp
                await new BrowserFetcher().DownloadAsync();

                var launchOptions = new LaunchOptions
                {
                    Headless = true,
                    Args = new[] { "--no-sandbox", "--disable-setuid-sandbox" }
                };

                using var browser = await Puppeteer.LaunchAsync(launchOptions);
                using var page = await browser.NewPageAsync();

                await page.GoToAsync(uri, WaitUntilNavigation.Networkidle2);

                bytes = await page.ScreenshotDataAsync(new ScreenshotOptions
                {
                    FullPage = false,
                    Type = ScreenshotType.Png
                });

                mediaType = "image/png";
            }
            else
            {
                // Unsupported content type: return empty image
                bytes = Array.Empty<byte>();
                mediaType = "application/octet-stream";
            }

            // Convert to Base64 data URL
            string base64Data = Convert.ToBase64String(bytes);
            string dataUrl = $"data:{mediaType};base64,{base64Data}";

            return dataUrl;
        }
    }
}
