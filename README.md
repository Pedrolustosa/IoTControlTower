# Projeto IoT Control Tower

Este projeto é uma aplicação de auxílio à tomada de decisão para uma indústria agrária, utilizando uma torre de controle para visualizar dados de dispositivos IoT em tempo real. A aplicação permite o cadastramento e gerenciamento de dispositivos IoT através de uma plataforma colaborativa.

## Arquitetura

O projeto segue uma arquitetura baseada na Clean Architecture, que promove a separação clara de responsabilidades entre as diferentes camadas da aplicação. A estrutura do projeto é dividida em cinco camadas principais:

1. **API (Presentation Layer)**: Camada responsável por lidar com as requisições HTTP, receber e enviar dados para o cliente. Aqui são definidos os controladores, modelos de dados DTO (Data Transfer Objects) e mapeamentos entre entidades e DTOs.

2. **Application Layer**: Camada que contém a lógica de negócios da aplicação. Aqui são implementados os casos de uso da aplicação, que orquestram as operações entre as entidades e os serviços.

3. **Domain Layer**: Camada que representa o núcleo da aplicação, contendo as entidades de domínio, interfaces de repositório e definições de contratos de serviços. Aqui são estabelecidas as regras de negócio da aplicação.

4. **Infrastructure Layer**: Camada responsável pela implementação concreta das interfaces definidas no domínio. Aqui são implementados os repositórios de dados, serviços externos e qualquer infraestrutura necessária para a aplicação funcionar.

5. **Infrastructure IoC (Inversion of Control)**: Camada responsável pela configuração e injeção de dependências da aplicação. Aqui são registradas as dependências da aplicação e configurados os serviços que serão utilizados.

## Tecnologias Utilizadas

- **ASP.NET Core 8**: Framework web utilizado para construir a API da aplicação.
- **Entity Framework Core**: ORM (Object-Relational Mapper) utilizado para mapear objetos do domínio para o banco de dados relacional.
- **Swagger**: Ferramenta utilizada para documentar e testar a API da aplicação.
- **JWT (JSON Web Tokens)**: Sistema de autenticação utilizado para proteger os endpoints da API.
- **Identity**: Framework utilizado para gerenciamento de identidade e autenticação de usuários.
- **AutoMapper**: Biblioteca utilizada para mapear objetos entre diferentes camadas da aplicação de forma automatizada.
- **SQL Server**: Banco de dados relacional utilizado para armazenar os dados da aplicação.

## Executando o Projeto

Para executar o projeto localmente, siga as instruções abaixo:

1. Certifique-se de ter o SDK do .NET 8 instalado em sua máquina.
2. Clone este repositório em sua máquina local.
3. Navegue até o diretório raiz do projeto.
4. Abra um terminal e execute o comando `dotnet run` para iniciar a aplicação.
5. Acesse a documentação da API em `http://localhost:5000/swagger` no seu navegador para visualizar e testar os endpoints.

## Licença

Este projeto está licenciado sob a [MIT License](LICENSE).
