# SafeHouseSystem - Web API

API para gerenciamento de despesas e receitas de moradores de uma residência, seguindo princípios de arquitetura limpa e boas práticas de desenvolvimento.

## Tecnologias


## Back-end

* C#
* .NET 8.0 (Web API)
* Entity Framework Core
* SQLite
* Arquitetura e padrões
* Clean Architecture
* Domain-Driven Design (DDD)
* SOLID
* Dependency Injection


## Funcionalidades

* Cadastro de moradores
* Cadastro de categorias
* Registro de transações (receitas e despesas)
* Integração com front-end
* Tratamento global de exceções (Middleware)
* Documentação com Swagger
* Integração com Front-end

O front-end do sistema pode ser encontrado em:

https://github.com/ViniciusBotelho-create/SafeHouseSystem-Front

https://github.com/ViniciusBotelho-create/SafeHouseSystem-Front.git

## Banco de Dados

O projeto utiliza SQLite como banco de dados local.

Download:
https://www.sqlite.org/download.html

A base de dados é criada automaticamente com o arquivo:

safehouse.db
Estrutura do Projeto
SafeHouseSystem/
  SafeHouseSystem.Api/
  SafeHouseSystem.Application/
  SafeHouseSystem.Domain/
  SafeHouseSystem.Infrastructure/
  SafeHouseSystem.Tests/
Api → Controllers, configuração e middlewares
Application → Services, interfaces e regras de aplicação
Domain → Entidades e regras de negócio
Infrastructure → Repositórios e acesso a dados
Tests → Testes unitários
Configuração do Ambiente
Pré-requisitos
.NET 8 SDK instalado
SQLite instalado

# Como rodar o projeto
## restaurar dependências
dotnet restore

## rodar a aplicação
dotnet run --project SafeHouseSystem.Api

A API estará disponível em:

https://localhost:7099/api

Em ambiente de desenvolvimento, a documentação interativa estará disponível em:

https://localhost:xxxx/swagger
Configurações importantes
CORS

A API está configurada para aceitar requisições do front-end:

http://localhost:5173

## Observações

Certifique-se de que o banco SQLite está disponível
O arquivo safehouse.db será criado automaticamente ao rodar a aplicação
O projeto utiliza HTTPS por padrão
É recomendado rodar o back-end antes do front-end
