using PuppeteerSharp;
using System.Net;

namespace Trados.GenAI.OpenAI.Services
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

        public async Task<(BinaryData ImageBytes, string MediaType, string Caption)> GenerateSnapshotAsync(string uri)
        {
            uri = WebUtility.HtmlDecode(uri);
            // First, check if the URI points to an image
            var response = await _httpClient.GetAsync(uri, HttpCompletionOption.ResponseHeadersRead);
            response.EnsureSuccessStatusCode();
            string mediaType = response.Content.Headers.ContentType?.MediaType ?? "application/octet-stream";

            byte[] snapshotBytes;
            string caption;

            if (mediaType.StartsWith("image"))
            {
                // Use the image as-is
                snapshotBytes = await response.Content.ReadAsByteArrayAsync();
                caption = $"Image from {uri}";
            }
            else if (mediaType == "text/html")
            {
                // Use Puppeteer Sharp to take a screenshot
                // Make sure the browser is downloaded
                await new BrowserFetcher().DownloadAsync();

                var launchOptions = new LaunchOptions
                {
                    Headless = true,
                    Args = new[] { "--no-sandbox", "--disable-setuid-sandbox" }
                };

                using var browser = await Puppeteer.LaunchAsync(launchOptions);
                using var page = await browser.NewPageAsync();

                await page.GoToAsync(uri, WaitUntilNavigation.Networkidle2);

                snapshotBytes = await page.ScreenshotDataAsync(new ScreenshotOptions
                {
                    FullPage = false,
                    Type = ScreenshotType.Png
                });

                caption = await page.GetTitleAsync(); // Use page title as caption
                mediaType = "image/png";
            }
            else
            {
                // Fallback
                snapshotBytes = Array.Empty<byte>();
                caption = $"Cannot snapshot content from {uri}, media type: {mediaType}";
            }

            return (new BinaryData(snapshotBytes), mediaType, caption);
        }
    }
}
