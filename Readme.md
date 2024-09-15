# Exchange Rate API

This is an ASP.NET Core Web API for retrieving  exchange rates from frankfurter.
The API includes endpoints to fetch exchange rates with optional parameters, handle retries for API calls, and use in-memory caching.

Before running the application, ensure you have the following installed:

- [.NET 7 SDK or later](https://dotnet.microsoft.com/download)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or later (optional, for a more integrated development experience)
- [Postman](https://www.postman.com/) or another API client (optional, for testing the API)

- 
## Getting Started

### 1. Clone the Repository

```bash
git clone https://github.com/yourusername/CurrencyConverter-api.git
cd exchange-rate-api

### 2. Restore Dependencies
Restore the project dependencies using the .NET CLI:
dotnet restore

### 3. Build the Application
Build the project to ensure there are no compilation errors:
dotnet build

### 4. Run the Application
Start the application using the .NET CLI:

dotnet run

### 5. Testing the API
Example:https://localhost:7279/api/exchange-rates/AUD (To get Latest rates using AUD as Base)

--------------------

API Application is Currency Converter 

In FrankFurterController implemented input validations and service calls

Currency serive has code to call api https://api.frankfurter.app/

Validtor class is for Basic validation on inputs

Retry Logic is implemented in RetryHelper class

If	I had more time I would have implemented more validation, authentication for api calls and UI to show the Data in more presentable manner
-------------------