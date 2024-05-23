# Projeto IoT Control Tower

![GitHub repo size](https://img.shields.io/github/repo-size/Pedrolustosa/IoTControlTower)
![GitHub contributors](https://img.shields.io/github/contributors/Pedrolustosa/IoTControlTower)
![GitHub stars](https://img.shields.io/github/stars/Pedrolustosa/IoTControlTower?style=social)
![GitHub forks](https://img.shields.io/github/forks/Pedrolustosa/IoTControlTower?style=social)
[![License](https://img.shields.io/badge/License-MIT-blue.svg)](https://opensource.org/licenses/MIT)
![Project Views](https://komarev.com/ghpvc/?username=SeuUsuario&label=Project%20Views&color=brightgreen)

Este projeto é uma aplicação de auxílio à tomada de decisão, utilizando uma torre de controle para visualizar dados de dispositivos IoT em tempo real. A aplicação permite o cadastramento e gerenciamento de dispositivos IoT através de uma plataforma colaborativa.

## Arquitetura

O projeto segue uma arquitetura baseada na Clean Architecture e DDD (Domain-Driven Design), que promove a separação clara de responsabilidades entre as diferentes camadas da aplicação. A estrutura do projeto é dividida em cinco camadas principais:

1. **API (Presentation Layer)**: Camada responsável por lidar com as requisições HTTP, receber e enviar dados para o cliente. Aqui são definidos os controladores, modelos de dados DTO (Data Transfer Objects) e mapeamentos entre entidades e DTOs.

2. **Application Layer**: Camada que contém a lógica de negócios da aplicação. Aqui são implementados os casos de uso da aplicação, que orquestram as operações entre as entidades e os serviços.

3. **Domain Layer**: Camada que representa o núcleo da aplicação, contendo as entidades de domínio, interfaces de repositório e definições de contratos de serviços. Aqui são estabelecidas as regras de negócio da aplicação. Atualmente, estamos utilizando o DDD especificamente nas entidades `Device` e `User`.

4. **Infrastructure Layer**: Camada responsável pela implementação concreta das interfaces definidas no domínio. Aqui são implementados os repositórios de dados, serviços externos e qualquer infraestrutura necessária para a aplicação funcionar.

5. **Infrastructure IoC (Inversion of Control)**: Camada responsável pela configuração e injeção de dependências da aplicação. Aqui são registradas as dependências da aplicação e configurados os serviços que serão utilizados.

## Tecnologias Utilizadas

- **ASP.NET Core 8**: Framework web utilizado para construir a API da aplicação.
- **Entity Framework Core**: ORM (Object-Relational Mapper) utilizado para mapear objetos do domínio para o banco de dados relacional.
- **Swagger**: Ferramenta utilizada para documentar e testar a API da aplicação.
- **JWT (JSON Web Tokens)**: Sistema de autenticação utilizado para proteger os endpoints da API.
- **Identity**: Framework utilizado para gerenciamento de identidade e autenticação de usuários.
- **AutoMapper**: Biblioteca utilizada para mapear objetos entre diferentes camadas da aplicação de forma automatizada.
- **Serilog**: Biblioteca utilizada para logging e auditoria de eventos na aplicação.
- **SQL Server**: Banco de dados relacional utilizado para armazenar os dados da aplicação.
- **FluentValidation**: Biblioteca utilizada para validar objetos complexos, assegurando que os dados estejam consistentes antes de serem processados.

## Funcionalidades

- **Cadastro e gerenciamento de dispositivos IoT**: Permite o registro e controle de dispositivos conectados à plataforma.
- **Visualização de dados em tempo real**: Oferece uma interface para monitorar dados coletados pelos dispositivos.
- **Autenticação e autorização**: Utiliza JWT e Identity para gerenciar o acesso seguro aos recursos da aplicação.
- **Logging e auditoria**: Implementado com Serilog para monitoramento eficiente das operações do sistema.
- **Validação de dados**: Usando FluentValidation para garantir a integridade e validade dos dados de entrada.

## Executando o Projeto

Para executar o projeto localmente, siga as instruções abaixo:

1. Certifique-se de ter o SDK do .NET 8 instalado em sua máquina.
2. Clone este repositório em sua máquina local.
3. Navegue até o diretório raiz do projeto.
4. Abra um terminal e execute o comando `dotnet run` para iniciar a aplicação.
5. Acesse a documentação da API em `http://localhost:5000/swagger` no seu navegador para visualizar e testar os endpoints.

## Estrutura de Pastas

A estrutura de pastas do projeto é organizada da seguinte forma:

- **IoTControlTower.API**: Contém a camada de apresentação com os controladores, modelos de dados DTO e configuração do Swagger.
- **IoTControlTower.Application**: Contém a camada de aplicação com os serviços, mapeamentos e casos de uso.
- **IoTControlTower.Domain**: Contém a camada de domínio com as entidades, interfaces de repositório e definições de contratos de serviços.
- **IoTControlTower.Infrastructure**: Contém a camada de infraestrutura com a implementação concreta das interfaces de repositório e serviços externos.
- **IoTControlTower.Infrastructure.IoC**: Contém a configuração e injeção de dependências da aplicação.

## Inserindo Dados no Banco de Dados

Para popular o banco de dados com dados de exemplo, execute os seguintes comandos SQL:

```sql
-- Inserindo dispositivos
INSERT INTO Devices (Description, Manufacturer, Url, IsActive, UserId) VALUES
('Device 1', 'Manufacturer A', 'http://device1.com', 1, 'user1-id'),
('Device 2', 'Manufacturer B', 'http://device2.com', 1, 'user2-id'),
('Device 3', 'Manufacturer C', 'http://device3.com', 1, 'user3-id'),
('Device 4', 'Manufacturer D', 'http://device4.com', 1, 'user4-id'),
('Device 5', 'Manufacturer E', 'http://device5.com', 1, 'user5-id');

-- Inserindo comandos
INSERT INTO Commands (CommandText) VALUES
('Command 1'),
('Command 2'),
('Command 3'),
('Command 4'),
('Command 5');

-- Inserindo parâmetros
INSERT INTO Parameters (Name, Description, CommandId) VALUES
('Parameter 1', 'Description 1', 1),
('Parameter 2', 'Description 2', 2),
('Parameter 3', 'Description 3', 3),
('Parameter 4', 'Description 4', 4),
('Parameter 5', 'Description 5', 5);

-- Inserindo descrições de comandos
INSERT INTO CommandDescriptions (Operation, Description, Result, Format, DeviceIdentifier, DeviceId, CommandId) VALUES
('Operation 1', 'Description 1', 'Result 1', 'Format 1', 'DeviceIdentifier 1', 1, 1),
('Operation 2', 'Description 2', 'Result 2', 'Format 2', 'DeviceIdentifier 2', 2, 2),
('Operation 3', 'Description 3', 'Result 3', 'Format 3', 'DeviceIdentifier 3', 3, 3),
('Operation 4', 'Description 4', 'Result 4', 'Format 4', 'DeviceIdentifier 4', 4, 4),
('Operation 5', 'Description 5', 'Result 5', 'Format 5', 'DeviceIdentifier 5', 5, 5);
