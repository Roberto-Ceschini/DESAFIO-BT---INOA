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

Durante a execução:

- se a cotação ficar **igual ou acima** do preço de venda, um alerta de venda é enviado;
- se a cotação ficar **igual ou abaixo** do preço de compra, um alerta de compra é enviado;
- se o ticker não for encontrado, a aplicação informa o erro e encerra;
- falhas temporárias da API são tratadas com até três tentativas antes de encerrar o monitoramento.

O sistema controla a faixa anterior do preço para evitar o envio repetido de e-mails enquanto o ativo permanecer na mesma condição de compra ou venda.

## Arquitetura

O projeto foi mantido simples, mas com separação de responsabilidades entre domínio, integração e orquestração.

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

`Stock` representa o ativo monitorado e concentra regras relacionadas ao próprio domínio, como os limites de compra e venda e o histórico de cotações.

`Quote` representa uma cotação em determinado instante. Após criada, seus valores não são alterados.

### DTOs

Os DTOs representam apenas o formato necessário para interpretar as respostas da Brapi. Isso evita acoplar diretamente as entidades de domínio ao formato externo da API.

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
Verificar mudança de estado
        ↓
Enviar alerta, se necessário
        ↓
Aguardar nova consulta
```

Foi utilizado um estado de preço (`Buy`, `Neutral`, `Sell` e `Unknown`) para impedir múltiplos disparos enquanto a cotação continuar na mesma faixa.

### EmailService

Responsável pela configuração SMTP e envio dos alertas.

A construção visual do HTML foi separada em `EmailTemplate`, mantendo a responsabilidade de envio isolada da apresentação do e-mail.

### Configurações e segredos

As configurações comuns ficam no `appsettings.json`.

Dados sensíveis, como senha SMTP e token da API, são armazenados com `.NET User Secrets` e não são versionados.

## Decisões técnicas

- `decimal` foi utilizado para valores monetários.
- `HttpClient` é reutilizado durante a execução da aplicação.
- `async/await` é utilizado nas chamadas HTTP e no fluxo de monitoramento.
- Os limites de compra e venda pertencem ao `Stock` monitorado e podem ser atualizados por métodos controlados da entidade.
- A primeira consulta à API também funciona como validação prática do ticker, evitando uma requisição adicional apenas para confirmar sua existência.
- O monitoramento tenta novamente a consulta em caso de falhas temporárias da API antes de encerrar.
- O envio de alertas ocorre apenas quando o ativo entra em uma nova faixa de compra ou venda, evitando spam de e-mails.

## Uso de Inteligência Artificial

O uso de IA foi pontual e utilizado como ferramenta de apoio para dúvidas específicas de C#/.NET, integração e apresentação. A lógica principal, as decisões arquiteturais e a solução final foram desenvolvidas e revisadas durante a implementação.

| Arquivo | Finalidade | Uso da IA |
| --- | --- | --- |
| `.gitignore` | Regras de arquivos ignorados pelo Git | Geração de uma configuração inicial adequada a projetos .NET e ambientes comuns de desenvolvimento. |
| `Program.cs` | `ConfigurationBuilder`, `appsettings.json` e User Secrets | Apoio na configuração das diferentes fontes de parâmetros e no binding para as classes de settings. |
| `Program.cs` | Validação dos preços com `decimal.TryParse` | Sugestão de uma forma segura de validar valores numéricos sem lançar exceções durante a conversão. |
| `Quote.cs` | `ArgumentOutOfRangeException` | Apoio na escolha da exceção mais adequada para um valor numérico fora do intervalo permitido. |
| `Stock.cs` | `ArgumentException` | Apoio na escolha da exceção para uma combinação inválida entre preço de compra e venda. |
| `BrapiApiResponse.cs` | DTOs da Brapi | Apoio na modelagem das classes utilizadas na desserialização do JSON retornado pela API. |
| `ApiService.cs` | `EnsureSuccessStatusCode()` | Sugestão de validação para respostas HTTP malsucedidas. |
| `ApiService.cs` | Inicialização de `StockQuoteResponse` | Explicação da sintaxe de inicialização de objetos em C#. |
| `MonitoringService.cs` | Constantes da política de retry | Apoio para centralizar o número de tentativas e intervalo entre novas consultas. |
| `MonitoringService.cs` | Estado inicial `Unknown` | Sugestão de uma forma de representar a primeira execução antes de existir uma faixa anterior. |
| `MonitoringService.cs` | Retry da API | Apoio na implementação de até três tentativas para falhas temporárias. |
| `EmailService.cs` | SSL no SMTP | Sugestão de habilitar comunicação segura no envio dos e-mails. |
| `EmailService.cs` | Cores dos alertas | Apoio visual para uma paleta inspirada na identidade do site da INOA. |
| `EmailTemplate.cs` | Template HTML | Apoio na construção e organização visual do e-mail de alerta. |

## Possíveis melhorias

Algumas evoluções possíveis para uma versão futura:

- persistência do histórico de cotações em banco de dados;
- monitoramento simultâneo de múltiplos ativos;
- configuração do intervalo de monitoramento via `appsettings.json`;
- testes unitários para regras de domínio e transições de faixa de preço;
- logs estruturados;
- abstração das integrações externas por interfaces para facilitar testes e substituição de provedores;
- publicação do executável em uma GitHub Release.

## Build e publicação

Durante o desenvolvimento:

```bash
dotnet run -- PETR4 45.00 35.00
```

Para gerar uma versão de release para Windows:

```bash
dotnet publish -c Release -r win-x64 --self-contained false
```

O executável será gerado dentro da pasta de publicação em `bin/Release/.../win-x64/publish/`.
