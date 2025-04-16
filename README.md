# Superwish Toys E-commerce Platform

Superwish is a modern e-commerce platform specializing in high-quality, educational, and entertaining toys for children. The platform provides a seamless shopping experience with robust user management and administrative features.

## Features

### User Features
- User registration and authentication
- Profile management
- Shopping cart functionality
- Order tracking and history
- Address management
- Secure payment processing

### Product Features
- Product catalog with categories
- Featured items showcase
- Product search and filtering
- Detailed product pages
- Stock management
- Product reviews and ratings

### Admin Features
- Category management
- Product management
- Order management
- User management
- Sales analytics
- Inventory tracking

## Technical Stack

- **Framework**: ASP.NET Core 6.0
- **Database**: PostgreSQL
- **Authentication**: ASP.NET Core Identity
- **Frontend**: 
  - Bootstrap 5
  - jQuery
  - Razor Pages
- **Storage**: Azure Blob Storage
- **ORM**: Entity Framework Core

## Getting Started

### Prerequisites
- .NET 6.0 SDK
- PostgreSQL
- Azure Storage Account (for file storage)

### Installation

1. Clone the repository:
```bash
git clone [repository-url]
```

2. Configure the database connection in `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Your PostgreSQL connection string"
  }
}
```

3. Configure Azure Storage settings in `appsettings.json`:
```json
{
  "AzureStorage": {
    "ConnectionString": "Your Azure Storage connection string",
    "ContainerName": "Your container name"
  }
}
```

4. Run database migrations:
```bash
dotnet ef database update
```

5. Start the application:
```bash
dotnet run
```

## Project Structure

```
Superwish_FSD04_AppDevII_ASP.NET_Project/
├── Data/                 # Database context and migrations
├── Models/              # Domain models
├── Pages/               # Razor pages
│   ├── Account/         # User account management
│   ├── Admin/           # Admin features
│   ├── Cart/            # Shopping cart
│   └── Orders/          # Order management
├── Services/            # Business logic services
├── wwwroot/             # Static files
└── Program.cs           # Application entry point
```

## Security Features

- Role-based access control
- Secure password hashing
- HTTPS enforcement
- CSRF protection
- Input validation
- Secure session management

## Contributing

1. Fork the repository
2. Create your feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## License

This project is licensed under the MIT License - see the LICENSE file for details.

## Contact

For any queries or support, please contact the development team. 