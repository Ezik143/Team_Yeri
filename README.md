# Team_Yeri

## Personal Information Console App

This is a simple C# console application that collects and validates personal information, including name, birthdate, and address. The program ensures that the input data is valid before displaying the collected information.

## Features
- Uses an API for address validation to ensure accuracy.
- Validates first and last names (only alphabetic characters).
- Ensures valid birthdate inputs (correct days for each month, leap year checks).
- Collects and displays a full address.
- Calculates age based on the provided birthdate.
- Verifies address using an external geolocation API.

## Project Structure
- **Program.cs**: The main entry point that handles user input and displays information.
- **PersonalInfo.cs**: A class library for managing personal details, validation, age calculation, and address verification.
- **PersonalInfoUnitTest.cs**: Unit tests to verify name validation, age calculation, and address correctness.

## Requirements
- .NET Framework (compatible version)
- Visual Studio or any C# compatible IDE

## How to Run
Make sure you have an active internet connection for address validation via the API.
1. Clone the repository:
   ```sh
   git clone https://github.com/yourusername/PersonalInfoApp.git
   ```
2. Open the project in Visual Studio or any C# editor.
3. Run the application from the terminal or IDE.

## Running Tests
To execute unit tests, use:

```sh
dotnet test
```

