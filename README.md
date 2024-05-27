# IoT Control Tower Project

![GitHub repo size](https://img.shields.io/github/repo-size/Pedrolustosa/IoTControlTower)
![GitHub contributors](https://img.shields.io/github/contributors/Pedrolustosa/IoTControlTower)
![GitHub stars](https://img.shields.io/github/stars/Pedrolustosa/IoTControlTower?style=social)
![GitHub forks](https://img.shields.io/github/forks/Pedrolustosa/IoTControlTower?style=social)
[![License](https://img.shields.io/badge/License-MIT-blue.svg)](https://opensource.org/licenses/MIT)
![Project Views](https://komarev.com/ghpvc/?username=SeuUsuario&label=Project%20Views&color=brightgreen)

This project is a decision-making aid application, using a control tower to visualize data from IoT devices in real-time. The application allows the registration and management of IoT devices through a collaborative platform.

## Architecture

The project follows an architecture based on Clean Architecture and DDD (Domain-Driven Design), promoting clear separation of responsibilities between different layers of the application. The project structure is divided into five main layers:

1. **API (Presentation Layer)**: This layer is responsible for handling HTTP requests, receiving and sending data to the client. Controllers, DTO (Data Transfer Objects) models, and mappings between entities and DTOs are defined here.

2. **Application Layer**: This layer contains the business logic of the application. It implements the use cases of the application, orchestrating operations between entities and services.

3. **Domain Layer**: This layer represents the core of the application, containing domain entities, repository interfaces, and service contract definitions. The business rules of the application are established here. Currently, we are using DDD specifically in the `Device` and `User` entities.

4. **Infrastructure Layer**: This layer is responsible for the concrete implementation of the interfaces defined in the domain. Data repositories, external services, and any infrastructure necessary for the application to function are implemented here.

5. **Infrastructure IoC (Inversion of Control)**: This layer is responsible for the configuration and dependency injection of the application. Dependencies are registered and the services to be used are configured here.

## Technologies Used

- **ASP.NET Core 8**: Web framework used to build the application's API.
- **Entity Framework Core**: ORM (Object-Relational Mapper) used to map domain objects to the relational database.
- **Swagger**: Tool used to document and test the application's API.
- **JWT (JSON Web Tokens)**: Authentication system used to secure the API endpoints.
- **Identity**: Framework used for identity management and user authentication.
- **AutoMapper**: Library used to map objects between different layers of the application automatically.
- **Serilog**: Library used for logging and event auditing in the application.
- **SQL Server**: Relational database used to store application data.
- **FluentValidation**: Library used to validate complex objects, ensuring data consistency before processing.
- **NETCore.MailKit**: Library used to send emails via SMTP with support for Google SMTP servers.

## Features

- **IoT Device Registration and Management**: Allows the registration and control of devices connected to the platform.
- **Real-Time Data Visualization**: Provides an interface to monitor data collected by devices.
- **Authentication and Authorization**: Uses JWT and Identity to manage secure access to application resources.
- **Logging and Auditing**: Implemented with Serilog for efficient system operation monitoring.
- **Data Validation**: Using FluentValidation to ensure the integrity and validity of input data.
- **Email Notifications**: Utilizes NETCore.MailKit for sending email notifications via Google SMTP.

## Running the Project

To run the project locally, follow the instructions below:

1. Ensure you have the .NET 8 SDK installed on your machine.
2. Clone this repository to your local machine.
3. Navigate to the root directory of the project.
4. Open a terminal and run the command `dotnet run` to start the application.
5. Access the API documentation at `http://localhost:5000/swagger` in your browser to view and test the endpoints.

## Folder Structure

The project folder structure is organized as follows:

- **IoTControlTower.API**: Contains the presentation layer with controllers, DTO models, and Swagger configuration.
- **IoTControlTower.Application**: Contains the application layer with services, mappings, and use cases.
- **IoTControlTower.Domain**: Contains the domain layer with entities, repository interfaces, and service contract definitions.
- **IoTControlTower.Infrastructure**: Contains the infrastructure layer with concrete implementations of repository interfaces and external services.
- **IoTControlTower.Infrastructure.IoC**: Contains the configuration and dependency injection of the application.

## Inserting Data into the Database

To populate the database with sample data, execute the following SQL commands:

```sql
-- Inserting devices
INSERT INTO Devices (Description, Manufacturer, Url, IsActive, UserId) VALUES
('Device 1', 'Manufacturer A', 'http://device1.com', 1, 'user1-id'),
('Device 2', 'Manufacturer B', 'http://device2.com', 1, 'user2-id'),
('Device 3', 'Manufacturer C', 'http://device3.com', 1, 'user3-id'),
('Device 4', 'Manufacturer D', 'http://device4.com', 1, 'user4-id'),
('Device 5', 'Manufacturer E', 'http://device5.com', 1, 'user5-id');

-- Inserting commands
INSERT INTO Commands (CommandText) VALUES
('Command 1'),
('Command 2'),
('Command 3'),
('Command 4'),
('Command 5');

-- Inserting parameters
INSERT INTO Parameters (Name, Description, CommandId) VALUES
('Parameter 1', 'Description 1', 1),
('Parameter 2', 'Description 2', 2),
('Parameter 3', 'Description 3', 3),
('Parameter 4', 'Description 4', 4),
('Parameter 5', 'Description 5', 5);

-- Inserting command descriptions
INSERT INTO CommandDescriptions (Operation, Description, Result, Format, DeviceIdentifier, DeviceId, CommandId) VALUES
('Operation 1', 'Description 1', 'Result 1', 'Format 1', 'DeviceIdentifier 1', 1, 1),
('Operation 2', 'Description 2', 'Result 2', 'Format 2', 'DeviceIdentifier 2', 2, 2),
('Operation 3', 'Description 3', 'Result 3', 'Format 3', 'DeviceIdentifier 3', 3, 3),
('Operation 4', 'Description 4', 'Result 4', 'Format 4', 'DeviceIdentifier 4', 4, 4),
('Operation 5', 'Description 5', 'Result 5', 'Format 5', 'DeviceIdentifier 5', 5, 5);
