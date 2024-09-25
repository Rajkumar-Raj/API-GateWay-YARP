**Step-by-Step Code for YARP API Gateway Integration**

1. Create a New .NET 8 API Project
First, create a new ASP.NET Core Web API project that will act as the API Gateway.

````
dotnet new webapi -n ApiGateway
cd ApiGateway
````


2. Add YARP NuGet Package
You need to install YARP (Yet Another Reverse Proxy) via NuGet:

````
dotnet add package Yarp.ReverseProxy --version 2.2.0
````

This package will provide the core proxy functionality.

3. Configure YARP in appsettings.json
Open the appsettings.json file and configure the routes and clusters for the services the API Gateway will forward requests to.


````
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  },
  "AllowedHosts": "*",
  "ReverseProxy": {
    "Routes": {
      "route1": {
        "ClusterId": "cluster1",
        "Match": {
          "Path": "/finance/{**catch-all}"
        }
      },
      "route2": {
        "ClusterId": "cluster2",
        "Match": {
          "Path": "/inventory/{**catch-all}"
        }
      }
    },
    "Clusters": {
      "cluster1": {
        "Destinations": {
          "destination1": {
            "Address": "https://localhost:5001/"
          }
        }
      },
      "cluster2": {
        "Destinations": {
          "destination1": {
            "Address": "https://localhost:5002/"
          }
        }
      }
    }
  }
}

````


**Routes**: Define the different paths the API Gateway will match. Here, we have two services: Finance and Inventory.
**Clusters**: Define the services the API Gateway routes to. Each cluster has destinations that point to the actual backend services.

4. Update Program.cs to Add YARP Middleware
Open the Program.cs file to configure the API Gateway and register the YARP middleware. Here’s the code you need to add.

````
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

var app = builder.Build();

app.MapReverseProxy();
app.Run();

````
5. Run Backend Services
For this setup, you need to have the Finance and Inventory services running on localhost:5001 and localhost:5002, respectively. You can create new minimal APIs for both services.

Sample FinanceService (Port 5001)
Create a new project:

````
dotnet new webapi -n FinanceService
cd FinanceService
````
Update the Program.cs file:

````
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/finance", () => "Finance Service is up and running!");

app.Run();
````
Run the Finance Service on port 5001:

````
dotnet run --urls http://localhost:5001
````
Sample InventoryService (Port 5002)
Create a new project:

````

dotnet new webapi -n InventoryService
cd InventoryService
````
Update the Program.cs file:
```

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/inventory", () => "Inventory Service is up and running!");

app.Run();
```
Run the Inventory Service on port 5002:

```
dotnet run --urls http://localhost:5002
```
6. Run the API Gateway
Now, run the API Gateway:

```
dotnet run
```
You should be able to access the Finance and Inventory services through the API Gateway using the following URLs:

Finance Service: http://localhost:5000/finance
Inventory Service: http://localhost:5000/inventory
YARP will forward these requests to the respective services based on the configuration in appsettings.json.




Reference: https://microsoft.github.io/reverse-proxy/articles/getting-started.html


