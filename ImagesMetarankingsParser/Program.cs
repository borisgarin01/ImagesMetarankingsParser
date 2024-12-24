using HtmlAgilityPack;

internal class Program
{
    static async Task Main(string[] args)
    {
        // URL of the website to parse
        string[] urls = new string[] {
            "https://metarankings.ru/best-games/",
            "https://metarankings.ru/best-games/page/2",
            "https://metarankings.ru/best-games/page/3",
            "https://metarankings.ru/best-games/page/4",
            "https://metarankings.ru/best-games/page/5",
            "https://metarankings.ru/best-games/page/6",
            "https://metarankings.ru/best-games/page/7",
            "https://metarankings.ru/best-games/page/8",
            "https://metarankings.ru/best-games/page/9",
            "https://metarankings.ru/best-games/page/10",
            "https://metarankings.ru/best-games/page/11",
            "https://metarankings.ru/best-games/page/12",
            "https://metarankings.ru/best-games/page/13",
            "https://metarankings.ru/best-games/page/14",
            "https://metarankings.ru/best-games/page/15",
            "https://metarankings.ru/best-games/page/16",
            "https://metarankings.ru/best-games/page/17",
            "https://metarankings.ru/best-games/page/18",
            "https://metarankings.ru/best-games/page/19",
            "https://metarankings.ru/best-games/page/20",
            "https://metarankings.ru/best-games/page/21",
            "https://metarankings.ru/best-games/page/22",
            "https://metarankings.ru/best-games/page/23",
            "https://metarankings.ru/best-games/page/24",
            "https://metarankings.ru/best-games/page/25",
            "https://metarankings.ru/best-games/page/26",
            "https://metarankings.ru/best-games/page/27",
            "https://metarankings.ru/best-games/page/28",
            "https://metarankings.ru/best-games/page/29",
            "https://metarankings.ru/best-games/page/30",
            "https://metarankings.ru/best-games/page/31",
            "https://metarankings.ru/best-games/page/32",
            "https://metarankings.ru/best-games/page/33",
            "https://metarankings.ru/best-games/page/34",
            "https://metarankings.ru/best-games/page/35",
            "https://metarankings.ru/best-games/page/36",
            "https://metarankings.ru/best-games/page/37",
            "https://metarankings.ru/best-games/page/38",
            "https://metarankings.ru/best-games/page/39",
            "https://metarankings.ru/best-games/page/40",
        };

        // Create an HttpClient to fetch the page content
        using (HttpClient httpClient = new HttpClient())
        {
            httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0");
            try
            {
                foreach (var url in urls)
                {
                    // Fetch the webpage content
                    string htmlContent = await httpClient.GetStringAsync(url);

                    // Initialize HtmlAgilityPack to parse the HTML content
                    HtmlDocument document = new HtmlDocument();
                    document.LoadHtml(htmlContent);

                    // Find all image tags in the document
                    var imageNodes = document.DocumentNode.SelectNodes("//img");

                    if (imageNodes != null)
                    {
                        foreach (var imgNode in imageNodes)
                        {
                            // Get the "src" attribute of each image tag
                            string imgSrc = imgNode.GetAttributeValue("src", string.Empty);

                            // Check if the src is a valid URL (could be relative or absolute)
                            if (!string.IsNullOrEmpty(imgSrc))
                            {
                                // Construct the full image URL if it's a relative URL
                                Uri baseUri = new Uri(url);
                                Uri fullImageUri = new Uri(baseUri, imgSrc);

                                // Output the image URL to the console
                                Console.WriteLine($"Found image: {fullImageUri}");

                                // Download the image and save it to disk
                                await DownloadImageAsync(httpClient, fullImageUri);
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("No images found on the page.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }

    // Method to download and save an image
    static async Task DownloadImageAsync(HttpClient client, Uri imageUri)
    {
        try
        {
            // Get the image data as a byte array
            byte[] imageBytes = await client.GetByteArrayAsync(imageUri);

            // Get the filename from the image URI
            string fileName = Path.GetFileName(imageUri.LocalPath);

            // Specify the path where the image will be saved
            string filePath = Path.Combine("DownloadedImages", fileName);

            // Create the directory if it doesn't exist
            Directory.CreateDirectory("DownloadedImages");

            // Save the image to disk
            await File.WriteAllBytesAsync(filePath, imageBytes);

            Console.WriteLine($"Image saved: {filePath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to download image {imageUri}: {ex.Message}");
        }
    }
}