# Stock Quote Alert

Programa de console em C# que monitora a cotação de um ativo da B3 e manda e-mail quando o preço ultrapassa um limite de venda ou cai abaixo de um limite de compra.

Resolução do desafio técnico.

## Como funciona

O programa é chamado com três parâmetros: o ativo, o preço de venda e o preço de compra.

```
stock-quote-alert.exe PETR4 22.67 22.59
```

Ele fica consultando a cotação no [brapi.dev](https://brapi.dev) em intervalos definidos no `appsettings.json` (padrão de 60 segundos). Se o preço subir acima do valor de venda, envia um e-mail avisando pra vender. Se cair abaixo do valor de compra, envia avisando pra comprar. Ctrl+C encerra.

## Estrutura

O projeto segue Clean Architecture, separado em quatro projetos:

```
src/
  StockQuoteAlert.Domain/         interfaces e entidade
  StockQuoteAlert.Application/    StockMonitorService, DTO, validação
  StockQuoteAlert.Infrastructure/ MailKit (SMTP) e BrapiQuoteService (HTTP)
  StockQuoteAlert.Console/        Program.cs e configuração de DI
```

A regra é simples: Domain não depende de nada, Application depende só de Domain, Infrastructure implementa as interfaces de Domain, e o Console amarra tudo no `Program.cs`.

## Configuração

Antes de rodar, copia o exemplo e preenche com seus dados:

```
cp src/StockQuoteAlert.Console/appsettings.example.json src/StockQuoteAlert.Console/appsettings.json
```

O arquivo tem essa cara:

```json
{
  "Smtp": {
    "Server": "smtp.gmail.com",
    "Port": 587,
    "User": "seu@gmail.com",
    "Password": "sua-senha",
    "From": "seu@gmail.com",
    "To": "destino@exemplo.com"
  },
  "Monitoring": { "IntervalSeconds": 60 }
}
```

`appsettings.json` está no `.gitignore` pra não vazar senha. Só o `appsettings.example.json` é versionado.

### Usando outro e-mail

A configuração acima é pra Gmail, mas qualquer SMTP serve. Só trocar os campos:

- `Server` é o servidor SMTP do provedor.
- `Port` é a porta. Quase sempre `587`.
- `User` e `Password` são as credenciais da conta que vai enviar.
- `From` é o remetente (geralmente o mesmo do `User`).
- `To` é quem vai receber o alerta.

Pra Outlook por exemplo, o servidor é `smtp.office365.com` na porta `587`. Pra Yahoo, é `smtp.mail.yahoo.com`.

Detalhe importante: se a conta tem verificação em duas etapas, a senha normal não funciona. É preciso gerar uma "senha de app" no painel do provedor. No Gmail é em https://myaccount.google.com/apppasswords. Sem isso o login retorna erro 535 ou 534.

Pra testar sem usar uma conta real dá pra usar o [Mailtrap](https://mailtrap.io), que tem um sandbox grátis.

## Rodando

Em desenvolvimento:

```
dotnet run --project src/StockQuoteAlert.Console -- PETR4 22.67 22.59
```

Pra rodar do mesmo jeito que o enunciado pede (`stock-quote-alert.exe`):

```
dotnet publish src/StockQuoteAlert.Console -c Release -o publish
cd publish
stock-quote-alert.exe PETR4 22.67 22.59
```

No Linux ou Mac fica `./stock-quote-alert` (sem `.exe`). O nome do executável é definido no csproj. O `appsettings.json` é copiado pra pasta `publish/`, por isso precisa entrar nela antes de rodar.
