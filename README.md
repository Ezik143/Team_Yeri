# Team_Yeri

## Personal Information Console App

This is a simple C# console application that collects and validates personal information, including name, birthdate, and address. The program ensures that the input data is valid before displaying the collected information.

## Features
- **Name Validation**: Ensures that first and last names contain only alphabetic characters.
- **Birthdate Validation**: Checks for correct days in each month, including leap year verification.
- **Address Input & Verification**: Collects and displays a full address and validates it using an external geolocation API.
- **Age Calculation**: Determines the user's age based on their provided birthdate.
- **Error Handling**: Provides clear and user-friendly error messages.

## Project Structure
- **`Program.cs`**: The main entry point that handles user input and displays information.
- **`PersonalInfo.cs`**: A class library for managing personal details, validation, age calculation, and address verification.
- **`AddressValidator.cs`**: Uses an external API to validate user-provided addresses.
- **`UnitTest.cs`**: Unit tests to verify name validation, age calculation, and address correctness.

## Requirements
- .NET Framework (compatible version)
- Visual Studio or any C# compatible IDE
- Active internet connection for address validation

## How to Run
1. Clone the repository:
   ```sh
   git clone https://github.com/yourusername/PersonalInfoApp.git
   ```
2. Open the project in Visual Studio or any C# editor.
3. Run the application from the terminal or IDE:
   ```sh
   dotnet run
   ```

## Running Tests
To execute unit tests, use:
```sh
dotnet test
```

## API Usage
This application uses the **Nominatim OpenStreetMap API** for address validation. Please ensure you comply with the API's usage policies.

## Contributions
Feel free to contribute to this project by submitting a pull request. Ensure your changes are well-documented and tested.

## License
This project is licensed under the MIT License.

