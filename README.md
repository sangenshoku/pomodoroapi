# Pomodo API

![C#](https://img.shields.io/badge/c%23-%23239120.svg?style=for-the-badge&logo=csharp&logoColor=white)
![.Net](https://img.shields.io/badge/.NET-5C2D91?style=for-the-badge&logo=.net&logoColor=white)
![Docker](https://img.shields.io/badge/docker-%230db7ed.svg?style=for-the-badge&logo=docker&logoColor=white)

It is a backend service that provides endpoints for my [Pomodoro SPA](https://github.com/sangenshoku/pomodoroapp).

## Features

- [x] CRUD operations for tasks.
- [x] Cookie-based authentication.
- [x] Support for RESTful API design principles.
- [x] I'm planning to add more features in the future.

## Technologies Used

- ASP.NET 8
- C#
- Entity Framework Core
- MariaDB
- Docker
- xUnit

## Prerequisite

- docker
- .NET 8 SDK
- dotnet-ef

## Installation

Clone the project.

```sh
git clone <link>
```

Install Entity Framework Core.

```
dotnet tool install --global dotnet-ef
```

Install dependencies.

```sh
dotnet restore
```

Run the migrations.

```sh
dotnet ef database update --project ./PomodoroApi
```

Run the main project.

```sh
dotnet run --project ./PomodoroApi
```

## Docker

The project uses environment and secret files in `compose.yml`. So you have to set them up.

First, create a `secrets` directory. Then, create the following files.

- **db_connection_string.txt** - contains the database connection information.
- **db_password.txt** - contains the password of the database.

```sh
# db_connection_string.txt
# this is the sample connection string for MariaDB.
Server=Your_Host;Database=Your_Database_Name;Uid=Your_User;Pwd=Your_Password
```

```sh
# db_password.txt
YourSuperSecretPassword
```

Lastly, create the `.env` file.

_(Optional) If you're using Linux. You can simply run the command `cp .env.example .env` to create the file in your directory._

```sh
# .env
CONNECTION_STRING=
DB_PASSWORD=YourSuperSecretPassword
```

After that, you can use the following command to create and start containers.

```sh
docker compose up --build
```

Now, go to http://localhost:5174/swagger/index.html to test it out.

## Testing

Use the command `dotnet test` to run all the tests.
