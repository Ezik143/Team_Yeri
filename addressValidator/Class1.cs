using System;
using System.Net.Http; // for HttpClient to send HTTP requests https://learn.microsoft.com/en-us/dotnet/api/system.net.http.httpclient?view=netframework-4.8
using System.Text.Json; // for JSON parsing https://zetcode.com/csharp/json/
using System.Threading.Tasks;
 
namespace AddressValidatorLibrary
{
    public class AddressValidator : IAddressValidator //dependency injection and unit testing purposes TABANG SAINT DON BOSCO
    {
        public const string RedText = "\x1b[31m";
        public const string ResetText = "\x1b[0m";
        private readonly HttpClient _client;
        private const string GeocodingApiUrl = "https://nominatim.openstreetmap.org/search";

        public AddressValidator(HttpClient client = null)//client = null for dependency injection and unit testing purposes
        {
            _client = client ?? new HttpClient();

            // Set user agent to comply with OSM Nominatim usage policy
            if (!_client.DefaultRequestHeaders.Contains("User-Agent"))//check if the header already exists if not then mo sud siya sa if statement.
            {
                _client.DefaultRequestHeaders.Add("User-Agent", "AddressValidatorApp/1.0");//•	Purpose: The "User-Agent" header is used in HTTP requests to identify the client software making the request. "AddressValidatorApp/1.0" is our app name
            }
        }

        public async Task<bool> ValidateAddressAsync(string houseNumber, string street, string city, string province, int postalCode, string country)
        {

            try
            {
                // Build the address query string
                string addressQuery = $"{houseNumber} {street}, {city}, {province}, {postalCode}, {country}";
                // Create URL with parameters
                string requestUrl = $"{GeocodingApiUrl}?q={Uri.EscapeDataString(addressQuery)}&format=json&addressdetails=1&limit=1";
                // Send the request
                HttpResponseMessage response = await _client.GetAsync(requestUrl);
                response.EnsureSuccessStatusCode();
                // Get the response content
                string responseBody = await response.Content.ReadAsStringAsync();
                // Parse JSON response
                using (JsonDocument doc = JsonDocument.Parse(responseBody))
                {
                    JsonElement root = doc.RootElement;
                    // Check if any results were returned
                    if (root.GetArrayLength() > 0)
                    {

                        return true;
                    }
                    Console.WriteLine($"{RedText}Warning:{ResetText} The address could not be verified. It may not exist or there might be spelling errors.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{RedText}Error{ResetText} validating address: {ex.Message}");
                return false;
            }
        }
    }
}