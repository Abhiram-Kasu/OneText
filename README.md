# OneText

OneText is a real-time messaging application built with ASP.NET and SignalR, featuring cross-platform clients for Desktop, Browser, Android, and iOS.

## Features

### 🔐 Authentication
OneText provides a secure authentication system with:
- **JWT Token-based authentication** for API and WebSocket connections
- **User registration and login** endpoints
- **SHA256 password hashing** for secure credential storage
- **Role-based authorization** (User and Admin roles)
- **Token expiration management** (configurable, default 5 days)

### 💬 Real-time Messaging
Built on SignalR, OneText offers real-time, bidirectional communication:
- **WebSocket-based messaging** via SignalR Hub at `/chat/realtime`
- **Group-based chat rooms** with subscription management
- **Live subscriber count** for each chat group
- **Automatic reconnection** handling in clients
- **Authorized connections** using JWT tokens

Key messaging features:
- Subscribe/unsubscribe to user chat groups
- Send and receive messages in real-time
- Track active participants per group
- Secure, authenticated connections

### 🗄️ Database Support
OneText supports multiple database providers:
- **SQLite** (default, great for development)
- **MySQL**
- **PostgreSQL**
- **SQL Server**

Database features:
- **Entity Framework Core** for data access
- **Code-first migrations** for schema management
- **User management** with email-based authentication
- **Friendship system** for user connections
- **Role-based access control**

## Architecture

OneText is structured as a multi-project solution:

- **OneText** - ASP.NET Core backend server with SignalR hub
- **OneText.Shared** - Shared interfaces and contracts (chat methods)
- **OneText.Client** - Avalonia-based cross-platform client core
- **OneText.Client.Desktop** - Desktop application (Windows, macOS, Linux)
- **OneText.Client.Browser** - WebAssembly browser client
- **OneText.Client.Android** - Android mobile app
- **OneText.Client.iOS** - iOS mobile app
- **BrowserExtensions** - Browser extension support

## Getting Started

### Prerequisites
- .NET 7.0 or later
- A supported database (SQLite included by default)

### Configuration

1. Copy the environment configuration file:
```bash
cp OneText/.env.example OneText/.env
```

2. Configure your environment variables in `.env`:

```env
# Application Settings
APP_NAME=OneText
ENVIRONMENT=dev

# Database Configuration
DB_CONNECTION=sqlite           # Options: sqlite, mysql, postgres, sqlserver
DB_HOST=localhost
DB_PORT=3306
DB_DATABASE=OneText.db
DB_USERNAME=root
DB_PASSWORD=

# Logging
LOG_CHANNEL=file              # Options: file, console
LOG_LEVEL=information

# Email (Optional)
MAIL_MAILER=file
MAIL_HOST=null
MAIL_PORT=25
MAIL_USERNAME=null
MAIL_PASSWORD=null
MAIL_FROM_ADDRESS="hello@example.com"
MAIL_FROM_NAME="OneText"
```

### Running the Application

1. **Restore dependencies:**
```bash
cd OneText
dotnet restore
```

2. **Run database migrations:**
```bash
dotnet ef database update
```

3. **Start the server:**
```bash
dotnet run
```

The API will be available at `https://localhost:5001` (or the port specified in launchSettings.json).

### API Endpoints

#### Authentication
- `POST /register` - Register a new user
  - Body: `{ "firstName": "string", "lastName": "string", "email": "string", "password": "string" }`
  
- `POST /login` - Login and receive JWT token
  - Body: `{ "email": "string", "password": "string" }`
  - Returns: `{ "firstName": "string", "lastName": "string", "email": "string", "token": "string" }`

#### Real-time Messaging
- WebSocket Hub: `/chat/realtime`
  - Requires JWT token authentication
  - Methods:
    - `SubscribeToUser(groupId)` - Join a chat group
    - `UnsubscribeFromUser(groupId)` - Leave a chat group
    - `SendMessage(groupId, message)` - Send a message to a group
  - Client callbacks:
    - `ReceiveMessage(message)` - Receive incoming messages
    - `UpdateNumSubscribers(count)` - Get subscriber count updates

## Development

### Database Migrations

Create a new migration:
```bash
cd OneText
dotnet ef migrations add MigrationName
```

Apply migrations:
```bash
dotnet ef database update
```

### Building the Solution

Build all projects:
```bash
dotnet build
```

## Technology Stack

- **Backend**: ASP.NET Core 7.0+
- **Real-time Communication**: SignalR
- **Database**: Entity Framework Core with multiple provider support
- **Authentication**: JWT (JSON Web Tokens)
- **Client Framework**: Avalonia UI (cross-platform)
- **Architecture**: Clean Architecture with separation of concerns

## Security

- Passwords are hashed using SHA256 before storage
- JWT tokens are required for API and WebSocket authentication
- Role-based authorization controls access to resources
- CORS is configured for cross-origin requests

## License

This project is built on the Spark Framework, which is open-sourced software licensed under the MIT license.
