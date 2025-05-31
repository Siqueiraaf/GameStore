
# 📦 GameStore Packing Service

Sistema de empacotamento de pedidos para uma loja de games, com autenticação, tratamento de exceções, validações, e testes automatizados.

O script para criação do banco de dados está em GameStore\GameStore\Common\Scripts>
---

## 📁 Estrutura do Projeto

```
GameStore/
├── Common/
│   ├── Filters/               # Filtros globais (ex: ExceptionFilter)
│   ├── Scripts/
│   └── Utils/
├── Core/
│   └── Models/                # Modelos de domínio
├── Features/
│   ├── Controllers/           # Controllers da API
│   ├── DTOs/                  # Data Transfer Objects
│   ├── Services/              # Lógicas de negócio
│   │   └── Interfaces/
│   └── Validators/            # Validações com FluentValidation
├── Infrastructure/
│   ├── Data/                  # Configuração de acesso a dados
│   └── Repositories/          # Implementações de repositórios
│       └── Interfaces/
├── Properties/
└── GameStore.PackingService.Tests/
    ├── Controllers/
    ├── Services/
```

---

## ⚙️ Configurações Importantes

### ✅ Global Exception Filter

```csharp
public class ExceptionFilter : IExceptionFilter
{
    private const string MensagemDeErro = "Um erro ocorreu, por favor, tente novamente!";
    public void OnException(ExceptionContext context)
    {
        var erro = new { Mensagem = MensagemDeErro };
        context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Result = new JsonResult(erro);
    }
}
```

Registrado no `Program.cs`:

```csharp
builder.Services.AddControllers(options =>
{
    options.Filters.Add<ExceptionFilter>();
});
```

---

## ✅ Validações com FluentValidation

Instalação:

```bash
dotnet add package FluentValidation
dotnet add package FluentValidation.AspNetCore
```

Exemplo de registro:

```csharp
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<OrderValidator>();
```

---

## 🔐 Autenticação JWT

### Configuração

- Criação de `AuthService` para gerar tokens
- Controller com `/auth/login` e `/auth/register`
- `[Authorize]` aplicado nas rotas protegidas

```csharp
[Authorize]
[HttpPost]
public async Task<IActionResult> Create([FromBody] CreateProductDto dto)
```

---

## 🐳 Docker e Docker Compose

### Dockerfile

```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["GameStore/GameStore.csproj", "GameStore/"]
RUN dotnet restore "./GameStore/GameStore.csproj"
COPY . .
WORKDIR "/src/GameStore"
RUN dotnet build "./GameStore.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "./GameStore.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GameStore.dll"]
```

### docker-compose.yml

```yaml
version: '3.4'

services:
  gamestore-api:
    build:
      context: .
      dockerfile: GameStore/Dockerfile
    ports:
      - "8080:8080"
    environment:
      - ASPNETCORE_ENVIRONMENT=Development
```

---

## 🧪 Testes Unitários

Criação do projeto de testes:

```bash
dotnet new xunit -n GameStore.PackingService.Tests
dotnet add GameStore.PackingService.Tests/GameStore.PackingService.Tests.csproj reference GameStore/GameStore.csproj
```

Pacotes utilizados:

```xml
<PackageReference Include="coverlet.collector" Version="6.0.2" />
<PackageReference Include="FluentAssertions" Version="8.3.0" />
<PackageReference Include="Moq" Version="4.20.72" />
<PackageReference Include="xunit" Version="2.9.2" />
<PackageReference Include="xunit.runner.visualstudio" Version="2.8.2" />
```

---

## 🚀 Execução do Projeto

### Via .NET CLI

```bash
dotnet restore
dotnet build
dotnet run --project GameStore/GameStore.csproj
```

### Via Docker

```bash
docker build -t gamestore-api -f GameStore/Dockerfile .
docker run -p 8080:8080 gamestore-api
```

### Via Docker Compose

```bash
docker-compose up --build
```

---

## ✅ Funcionalidades

- Registro e login de usuário com JWT
- Empacotamento de pedidos em caixas disponíveis
- Cadastro e listagem de produtos por pedido
- Proteção com `[Authorize]`
- Tratamento global de exceções
- Validações automáticas com FluentValidation
- Testes unitários com xUnit, Moq e FluentAssertions

---

## 🧾 To Do

- ✅ Adicionar autenticação JWT
- ✅ Criar filtros globais
- ✅ Validar DTOs
- ✅ Criar Dockerfile e Docker Compose
- ✅ Criar testes automatizados
