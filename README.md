# Stock Quote Alert

Aplicação de console em C# para monitorar continuamente a cotação de um ativo da B3 e enviar alertas por e-mail quando o preço atingir os limites definidos para compra ou venda.

## Como executar

### Pré-requisitos

- [.NET SDK](https://dotnet.microsoft.com/)
- Conta de e-mail com acesso SMTP
- Token de acesso à [Brapi](https://brapi.dev/)

Clone o repositório e entre na pasta do projeto:

```bash
git clone <URL_DO_REPOSITORIO>
cd DESAFIO-BT---INOA/StockQuoteAlert
```

Restaure as dependências:

```bash
dotnet restore
```

### Configuração

As configurações não sensíveis ficam em `appsettings.json`.

Exemplo:

```json
{
  "EmailSettings": {
    "To": "destino@exemplo.com",
    "From": "",
    "Password": "",
    "SmtpServer": "smtp.gmail.com",
    "SmtpPort": 587,
    "EnableSsl": true
  },
  "ApiSettings": {
    "BaseUrl": "https://brapi.dev/api/quote/",
    "Token": ""
  }
}
```

Antes de executar, altere `EmailSettings:To` para o endereço que deverá receber os alertas.

Os dados sensíveis são armazenados com **User Secrets**, evitando que credenciais sejam versionadas no Git.

Configure:

```bash
dotnet user-secrets set "EmailSettings:From" "seu-email@gmail.com"
dotnet user-secrets set "EmailSettings:Password" "sua-senha-de-app"
dotnet user-secrets set "ApiSettings:Token" "seu-token-brapi"
```

Para conferir os secrets configurados:

```bash
dotnet user-secrets list
```

Para remover todos:

```bash
dotnet user-secrets clear
```

### Execução

O programa recebe três argumentos:

1. ticker do ativo;
2. preço de referência para venda;
3. preço de referência para compra.

Exemplo:

```bash
dotnet run -- PETR4 45.00 35.00
```

Os preços podem ser informados com ponto ou vírgula como separador decimal e
sem separador de milhares. Também são aceitos valores inteiros. Quando houver
um separador, ele será sempre interpretado como separador decimal.

Exemplos válidos:

```text
45.00
45,00
45
0,50
```

Exemplos inválidos:

```text
1.234,90
1,234.90
R$ 35,00
35,00,00
```

Por exemplo, a execução também pode usar vírgulas:

```bash
dotnet run -- PETR4 45,00 35,00
```

Durante a execução:

- se a cotação ficar **acima** do preço de venda, um alerta de venda é enviado;
- se a cotação ficar **abaixo** do preço de compra, um alerta de compra é enviado;
- se o ticker não for encontrado, a aplicação informa o erro e encerra;
- falhas temporárias da API são tratadas com até três tentativas antes de encerrar o monitoramento.

# Para os Avaliadores 

## Arquitetura

Estrutura de pastas:

```text
StockQuoteAlert/
├── Program.cs
├── appsettings.json
└── src/
    ├── Configuration/
    │   ├── ApiSettings.cs
    │   └── EmailSettings.cs
    ├── DTOs/
    │   ├── BrapiApiResponse.cs
    │   └── StockQuoteResponse.cs
    ├── Models/
    │   ├── Quote.cs
    │   └── Stock.cs
    └── Services/
        ├── ApiService.cs
        ├── MonitoringService.cs
        └── Email/
            ├── EmailService.cs
            └── EmailTemplate.cs
```

### Models

`Stock` representa o ativo monitorado 

`Quote` representa uma cotação em determinado instante.

### DTOs

Os DTOs representam o formato necessário para interpretar as respostas da Brapi.

### ApiService

Responsável pela comunicação HTTP com a Brapi, autenticação, desserialização e tratamento das respostas.

### MonitoringService

Responsável por coordenar o fluxo principal:

```text
Consultar cotação
        ↓
Registrar cotação
        ↓
Identificar faixa de preço
        ↓
Enviar alerta, se necessário
        ↓
Aguardar nova consulta
```

### EmailService

Responsável pela configuração SMTP e envio dos alertas.

A construção visual do HTML foi separada em `EmailTemplate`.

### Configurações e segredos

As configurações comuns ficam no `appsettings.json`.

Dados sensíveis, como senha SMTP e token da API, são armazenados com `.NET User Secrets` e não foram versionados.

## Uso de Inteligência Artificial

O uso de IA foi pontual e utilizado como ferramenta de apoio para dúvidas específicas, escolha dos tipos de erros e sintaxe de C#/.NET. A lógica principal, as decisões arquiteturais e a solução final foram desenvolvidas e revisadas.

| Arquivo | Finalidade | Uso da IA |
| --- | --- | --- |
| `.gitignore` | Regras de arquivos ignorados pelo Git | Criação desse arquivo com uma configuração inicial adequada a projetos .NET e ambientes comuns de desenvolvimento. |
| `Program.cs` | `ConfigurationBuilder`, `appsettings.json` e User Secrets | Apoio na configuração das diferentes fontes de parâmetros e no binding para as classes de settings. |
| `Program.cs` | Validação dos preços com `decimal.TryParse` | Sugestão de uma forma segura de validar valores numéricos |
| `BrapiApiResponse.cs` | DTOs da Brapi | Apoio na modelagem das classes utilizadas na desserialização do JSON retornado pela API. |
| `ApiService.cs` | `EnsureSuccessStatusCode()` | Sugestão dessa linha para validação de respostas HTTP malsucedidas que não foram mapeadas. |
| `ApiService.cs` | Inicialização de `StockQuoteResponse` | Explicação da sintaxe de inicialização de objetos sem construtor em C#. |
| `MonitoringService.cs` | Constantes da política de retry | Apoio para centralizar o número de tentativas e intervalo entre novas consultas. |
| `MonitoringService.cs` | Estado inicial `Unknown` | Sugestão de uma forma para representar a primeira execução antes de existir uma faixa anterior. |
| `MonitoringService.cs` | Retry da API | Implementação do método `GetStockQuoteWithRetryAsync`. |
| `EmailService.cs` | SSL no SMTP | Sugestão de incluir uma linha habilitando o SSL no envio de emails. |
| `EmailTemplate.cs` | Template HTML | construção e organização visual do e-mail de alerta baseado no site oficial da INOA. |


## Possíveis melhorias

Algumas evoluções possíveis para uma versão futura:

- persistência do histórico de cotações em banco de dados;
- monitoramento simultâneo de múltiplos ativos;
- aumento das regras de negócio para monitoramento;
- configuração do intervalo de monitoramento via `appsettings.json`;
- configuração de mais APIs de monitoramento caso alguma fique fora do ar;
- testes unitários
- logs estruturados;
- publicação do executável em uma GitHub Release.

## Build e publicação

Durante o desenvolvimento:

```bash
dotnet run -- PETR4 45.00 35.00
```

Caso necessário, pode-se gerar uma versão de release para Windows:

```bash
dotnet publish -c Release -r win-x64 --self-contained false
```

O executável será gerado dentro da pasta de publicação em `bin/Release/.../win-x64/publish/`.
