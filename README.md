# IoT Control Tower Project

![GitHub repo size](https://img.shields.io/github/repo-size/Pedrolustosa/IoTControlTower)
![GitHub contributors](https://img.shields.io/github/contributors/Pedrolustosa/IoTControlTower)
![GitHub stars](https://img.shields.io/github/stars/Pedrolustosa/IoTControlTower?style=social)
![GitHub forks](https://img.shields.io/github/forks/Pedrolustosa/IoTControlTower?style=social)
[![License](https://img.shields.io/badge/License-MIT-blue.svg)](https://opensource.org/licenses/MIT)

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
- **CQRS (Command Query Responsibility Segregation)**: Architecture pattern used to separate command and query operations, improving scalability and flexibility.
- **MediatR**: Library for implementing the mediator pattern, used to decouple communication between application components.
- **Dapper**: Micro ORM used for data access, providing faster execution and lower overhead compared to full ORM frameworks.
- **Redis**: In-memory data structure store used to cache frequently accessed data for improved performance.
- **RabbitMQ**: Messaging broker used for asynchronous communication and message queuing.

## Features

- **IoT Device Registration and Management**: Allows the registration and control of devices connected to the platform.
- **Real-Time Data Visualization**: Provides an interface to monitor data collected by devices.
- **Authentication and Authorization**: Uses JWT and Identity to manage secure access to application resources.
- **Logging and Auditing**: Implemented with Serilog for efficient system operation monitoring.
- **Data Validation**: Using FluentValidation to ensure the integrity and validity of input data.
- **Email Notifications**: Utilizes NETCore.MailKit for sending email notifications via Google SMTP.
- **Command Query Responsibility Segregation (CQRS)**: Architecture pattern used to separate command and query operations, improving scalability and flexibility. Implemented using ASP.NET Core 8, MediatR, and Dapper for efficient command and query handling.
- **Messaging with RabbitMQ**: Integrates RabbitMQ for asynchronous message queuing and communication between application components.

## Entities and Relationships

### Entities

<details>
<summary>Device</summary>

- **Description**: Description of the device.
- **Manufacturer**: Manufacturer of the device.
- **Url**: Device URL.
- **IsActive**: Indicates whether the device is active.
- **LastCommunication**: Last communication of the device.
- **IpAddress**: Device IP address.
- **Location**: Location of the device.
- **FirmwareVersion**: Firmware version of the device.
- **ManufactureDate**: Manufacture date of the device.
- **SerialNumber**: Serial number of the device.
- **ConnectionType**: Connection type of the device.
- **HealthStatus**: Health status of the device.
- **LastKnownStatus**: Last known status of the device.
- **Owner**: Owner of the device.
- **InstallationDate**: Date of installation of the device.
- **LastMaintenanceDate**: Last maintenance date of the device.
- **SensorType**: Type of sensor in the device.
- **AlarmSettings**: Settings for device alarms.
- **LastHealthCheckDate**: Date of the last health check of the device.
- **MaintenanceHistory**: History of maintenance operations on the device.
</details>

<details>
<summary>User</summary>

- **FullName**: Full name of the user.
- **DateOfBirth**: Date of birth of the user.
- **Gender**: Gender of the user.
- **Address**: Address of the user.
- **City**: City of the user.
- **State**: State of the user.
- **Country**: Country of the user.
- **PostalCode**: Postal code of the user.
- **Role**: Role of the user.
- **LastLogin**: Last login date of the user.
- **UpdateDate**: Date of the last update of user information.
- **RegistrationDate**: Registration date of the user.
- **LastPasswordChangeDate**: Date of the last password change of the user.
</details>

### Relationships

- **Device - User**: Each device belongs to a user. The `UserId` property in the `Device` entity establishes this relationship as a foreign key referencing the `Id` property of the `User` entity.

## Running the Project

To run the project locally, follow the instructions below:

# Prerequisites
- Ensure you have the .NET 8 SDK installed on your machine. You can download it [here](https://dotnet.microsoft.com/download).
- Have Git installed on your machine to clone the repository. You can download it [here](https://git-scm.com/downloads).
- Have Docker installed on your machine. You can download it [here](https://www.docker.com/get-started/).
- To install Redis [here](https://redis.io/docs/latest/operate/oss_and_stack/install/install-stack/docker/?utm_source=redisinsight&utm_medium=main&utm_campaign=docker), you can run the following command:
   ```bash
   docker run -d --name redis-stack-server -p 6379:6379 redis/redis-stack-server:latest
   ```
- To install RabbitMQ [here](https://www.rabbitmq.com/docs/download), you can run the following command:
  ```bash
   docker run -it --rm --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:3.13-management
  ```

# Cloning the Repository
1. Open the terminal or command prompt.
2. Clone this repository to your local machine using the command:
3. ``` bash
   git clone https://github.com/Pedrolustosa/IoTControlTower.git
    ```
4. Navigate to the cloned directory: ``` cd IoTControlTower ```

# Running in Visual Studio 2022
1. Open Visual Studio 2022.
2. Select "Open a Project or Solution" and navigate to the cloned directory.
3. Select the solution file (.sln) and click "Open".
4. Wait for Visual Studio to automatically restore NuGet packages.
5. Press F5 to build and run the project.

# Running in Visual Studio Code
1. Open Visual Studio Code.
2. Install the "C#" extension if not already installed.
3. Open the project folder in Visual Studio Code.
4. Visual Studio Code will automatically detect the .NET environment and offer to install necessary dependencies. Accept the installation.
5. Open a terminal in Visual Studio Code and run the command: `dotnet run`
6. The project will be compiled and started.

## Folder Structure

The project folder structure is organized as follows:

- **IoTControlTower.API**: Contains the presentation layer with controllers, DTO models, and Swagger configuration.
- **IoTControlTower.Application**: Contains the application layer with services, mappings, and use cases.
- **IoTControlTower.Domain**: Contains the domain layer with entities, repository interfaces, and service contract definitions.
- **IoTControlTower.Infrastructure**: Contains the infrastructure layer with concrete implementations of repository interfaces and external services.
- **IoTControlTower.Infrastructure.IoC**: Contains the configuration and dependency injection of the application.

# Starring and Following the Project
Please click the star (★) at the top of the repository page to favorite the project and follow its updates. This helps us know that you're interested in the project and motivates us to continue improving it for you and the community.
