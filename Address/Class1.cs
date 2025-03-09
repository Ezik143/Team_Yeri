using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace AddressValidationLibrary
{
    public class AddressValidator
    {
        private static readonly HttpClient client = new HttpClient();
        private const string GeocodingApiUrl = "https://nominatim.openstreetmap.org/search";
        public const string RedText = "\x1b[31m";
        public const string GreenText = "\x1b[32m";
        public const string YellowText = "\x1b[33m";
        public const string BlueText = "\x1b[34m";
        public const string ResetText = "\x1b[0m";

        static AddressValidator()
        {
            // Set user agent to comply with OSM Nominatim usage policy
            client.DefaultRequestHeaders.Add("User-Agent", "PersonalInfoApp/1.0");
        }

        public async Task<bool> ValidateAddress(PersonalInfoLibrary.PersonalInfo info)
        {
            try
            {
                Console.WriteLine($"\n{BlueText}Validating address...{ResetText}");

                // Build the address query string
                string addressQuery = $"{info.HouseNumber} {info.Street}, {info.City}, {info.Province}, {info.PostalCode}, {info.Country}";

                // Create URL with parameters
                string requestUrl = $"{GeocodingApiUrl}?q={Uri.EscapeDataString(addressQuery)}&format=json&addressdetails=1&limit=1";

                // Send the request
                HttpResponseMessage response = await client.GetAsync(requestUrl);
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
                        Console.WriteLine($"{GreenText}Address validation successful!{ResetText}");
                        Console.WriteLine("\nFound address details:");

                        JsonElement result = root[0];

                        // Display address components if available
                        if (result.TryGetProperty("address", out JsonElement address))
                        {
                            DisplayAddressComponent(address, "road", "Street");
                            DisplayAddressComponent(address, "city", "City");
                            DisplayAddressComponent(address, "state", "State/Province");
                            DisplayAddressComponent(address, "country", "Country");
                            DisplayAddressComponent(address, "postcode", "Postal Code");
                        }

                        info.IsAddressVerified = true;
                        return true;
                    }

                    Console.WriteLine($"{YellowText}Warning:{ResetText} The address could not be verified. It may not exist or there might be spelling errors.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{RedText}Error validating address:{ResetText} {ex.Message}");
                return false;
            }
        }

        private static void DisplayAddressComponent(JsonElement address, string key, string label)
        {
            if (address.TryGetProperty(key, out JsonElement value))
            {
                Console.WriteLine($"{label}: {value}");
            }
        }
    }
}
