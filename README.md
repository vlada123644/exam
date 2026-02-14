# TradeAutomation

WPF-приложение на .NET 8 для автоматизации торговых процессов.

## Требования

- .NET 8 SDK
- Windows (WPF)
- SQL Server LocalDB (или SQL Server)

## Сборка и запуск

```bash
dotnet restore
dotnet build
dotnet run --project TradeAutomation
```

Или откройте `TradeAutomation.sln` в Visual Studio и запустите проект.

## Строка подключения

По умолчанию используется LocalDB. Настройка в `TradeAutomation/appsettings.json`.
