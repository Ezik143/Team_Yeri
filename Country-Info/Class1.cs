using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Country_Info
{
    public class country
    {
        private const string GeocodingApiUrl = "https://nominatim.openstreetmap.org/search";
        static async Task<bool> ValidateAddress(string street, string city, string province, string country, string postalCode)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    // Set user agent to comply with OSM Nominatim usage policy
                    client.DefaultRequestHeaders.Add("User-Agent", "AddressValidatorApp/1.0");

                    // Build the address query string
                    string addressQuery = $"{street}, {city}, {province}, {postalCode}, {country}";

                    // Create URL with parameters
                    string requestUrl = $"{GeocodingApiUrl}?q={HttpUtility.UrlEncode(addressQuery)}&format=json&addressdetails=1&limit=1";

                    Console.WriteLine("\nValidating address...");

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
                            // Display found address details
                            Console.WriteLine("\nAddress found with the following details:");
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

                            return true;
                        }

                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError validating address: {ex.Message}");
                return false;
            }
        }

        static void DisplayAddressComponent(JsonElement address, string key, string label)
        {
            if (address.TryGetProperty(key, out JsonElement value))
            {
                Console.WriteLine($"{label}: {value}");
            }
        }
    }
}

