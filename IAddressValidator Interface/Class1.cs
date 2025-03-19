using System.Threading.Tasks;

namespace AddressValidatorLibrary
{
    // Interface for AddressValidator
    // For Dependency Injection and Unit Testing
    public interface IAddressValidator
    {
        Task<bool> ValidateAddressAsync(string houseNumber, string street, string city, string province, int postalCode, string country);
    }
}