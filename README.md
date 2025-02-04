# Web Commerce Project

## About
The Web Commerce is a simple e-commerce demonstration project for EF Core 8.0 features. You will need to install .NET 8.0 SDK to develop this project.

This repo contains 2 executable project:
- Web, the Web Commcerce web application.
- Client, for playing around with the web app through console app i.e. testing concurrency conflict handling.

## Features
- User & Product Managements
- User Cart
- Checkout

## EF Core Features
We will disclose the following EF Core features in this repo:
- Concurrency Conflict Handling
- Global Query Filter
- Interceptors

## Installation
To get started with the project, follow these steps:
1. **Clone the repository:**
    ```bash
    git clone https://github.com/kl-tie/web-commerce.git
    ```
2. **Package Restore:**
    ```bash
    dotnet tool restore
    dotnet restore
    ```
3. **Running web app:**
    ```bash
    cd WebCommerce.Web
    dotnet run
    ```

## Database Migrations
To apply the database migrations to your DB instance, configure the connection string in `appsettings.json` (using user secrets is preferable) to your database instance (no need to create the database first), and then run:
```bash
dotnet dotnet-ef database update --context CommerceDbContext
```
