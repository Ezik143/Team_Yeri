using System.Threading.Tasks;

namespace AddressValidatorLibrary
{
    // For Dependency Injection and Unit Testing
    public interface IAddressValidator
    {
        Task<bool> ValidateAddressAsync(string houseNumber, string street, string city, string province, int postalCode, string country);
    }
}