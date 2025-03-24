using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
//team yeri code
namespace AddressValidatorLibrary
{
    public class AddressValidator
    {
        public const string RedText = "\x1b[31m";
        public const string ResetText = "\x1b[0m";
        private readonly HttpClient _client;
        private const string GeocodingApiUrl = "https://nominatim.openstreetmap.org/search";

        public AddressValidator(HttpClient client = null)
        {
            _client = client ?? new HttpClient();

            // Set user agent to comply with OSM Nominatim usage policy
            if (!_client.DefaultRequestHeaders.Contains("User-Agent"))
            {
                _client.DefaultRequestHeaders.Add("User-Agent", "AddressValidatorApp/1.0");
            }
        }

        public async Task<bool> ValidateAddressAsync(string houseNumber, string street, string city, string province, int postalCode, string country)
        {
            // Check for empty inputs but return false instead of throwing exceptions
            if (string.IsNullOrWhiteSpace(houseNumber) ||
                string.IsNullOrWhiteSpace(street) ||
                string.IsNullOrWhiteSpace(city) ||
                string.IsNullOrWhiteSpace(province) ||
                string.IsNullOrWhiteSpace(country))
            {
                return false;
            }

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
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}