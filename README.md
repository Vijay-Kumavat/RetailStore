# Retail Store Management API

This project provides an API for managing stock, pricing, and inventory for a retail store using .NET Core 8, SQL Server, and Ocelot for API Gateway.

## Table of Contents
- [Overview](#overview)
- [Features](#features)
- [Prerequisites](#prerequisites)
- [Installation](#installation)
- [Configuration](#configuration)
- [Running the Code](#running-the-code)
- [Postman Collection](#postman-collection)
- [Database Script](#database-script)
- [Contributing](#contributing)

## Overview
This API offers functionalities to manage product restocking, pricing adjustments based on market trends, and inventory optimization using fake data. The project follows best practices, including the use of stored procedures, SOLID principles, and a microservices architecture.

## Features
- Smart Restocking Plan
- Clever Pricing Trick
- Inventory Optimization

## Prerequisites
- [.NET Core 8 SDK](https://dotnet.microsoft.com/download)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
- [Ocelot](https://ocelot.readthedocs.io/en/latest/introduction/gettingstarted.html)

## Installation
1. **Clone the repository:**
    ```sh
    git clone https://github.com/Vijay-Kumavat/RetailStore.git
    ```

2. **Install dependencies:**
    ```sh
    dotnet restore
    ```

3. **Setup SQL Server:**
    - Ensure SQL Server is running.
    - Run the database script provided [here](https://github.com/Vijay-Kumavat/RetailStore/blob/master/Retail%20Store%20SQL%20Script.sql) to set up the database schema and initial data.

## Configuration
1. **Update `appsettings.json`:**
    Configure your SQL Server connection string and any other necessary settings in `appsettings.json`.
    ```json
    {
      "ConnectionStrings": {
        "DefaultConnection": "Server=your_server;Database=your_database;User Id=your_user;Password=your_password;"
      },
      "Ocelot": {
        "ReRoutes": [
          // Your Ocelot configuration here
        ],
        "GlobalConfiguration": {
          "BaseUrl": "https://localhost:5000"
        }
      }
    }
    ```

2. **Ocelot Configuration:**
    - Configure Ocelot in `ocelot.json` for API Gateway setup.

## Running the Code
1. **Build the project:**
    ```sh
    dotnet build
    ```

2. **Run the project:**
    ```sh
    dotnet run
    ```

## Postman Collection
A Postman collection is provided [here](https://github.com/Vijay-Kumavat/RetailStore/blob/master/Retail%20Store.postman_collection.json) to test the API endpoints. Import the collection into Postman and use it to interact with the API.

## Database Script
The database script for setting up the required tables and initial data is available [here](https://github.com/Vijay-Kumavat/RetailStore/blob/master/Retail%20Store%20SQL%20Script.sql). Run this script on your SQL Server to prepare the database.

## Contributing
Contributions are welcome! Please submit a pull request or open an issue to discuss any changes or improvements.

## Additional Information
- Ensure to follow security best practices, including input validation and protection against SQL injection and XSS.
- Design the solution with scalability and maintainability in mind.

