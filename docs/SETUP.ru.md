# Установка приложения

## 1. Системные требования
  - .NET 9
  - MSSQL или другая СУБД

## 2. База данных
В файле `appsettings.json` параметр `UseInMemoryDatabase` задаёт использование БД в памяти (`true`) или реальной СУБД (`false`).

### Миграции
#### Основная база (пустая база должна быть создана в MS SQL)
Задать в `\src\DeveloperPath.WebApi\appsettings.Development.json` строку подключения для локального использования, свойство `DeveloperPathSqlConnectionString`.

Либо в user-secrets:
- В CLI в папке `DeveloperPath.WebApi` выполнить   
`dotnet user-secrets init`  
- Затем   
`dotnet user-secrets set "DeveloperPathSqlConnectionString" "Data Source=..."`.

Используя CLI из папки `\src\DeveloperPath.Infrastructure`:
- обновить инструменты ef  
`dotnet tool update --global dotnet-ef`
- обновление базы  
`dotnet ef --startup-project ..\DeveloperPath.WebApi\ database update`
- откат к миграции   
`dotnet ef --startup-project ..\DeveloperPath.WebApi\ database update <NAME>`
- добавление миграции  
`dotnet ef --startup-project ..\DeveloperPath.WebApi\ migrations add <NAME>`
- удаление миграции  
`dotnet ef --startup-project ..\DeveloperPath.WebApi\ migrations remove`

База заполнится тестовыми данными автоматически при старте приложения, если нет данных в Paths.

#### Identity (должна быть создана отдельная пустая база в MS SQL)
Задать в `\src\IdentityProvider\appsettings.Development.json`
- строку подключения для локального использования, свойство `SqlConnection`,
- пароль для доступа в SwaggerApi, свойство `PathApiSwaggerSecret`.

Либо то же в user-secrets (см. пример выше).

Используя CLI из папки `\src\DeveloperPath.IdentityProvider`:
- обновление базы  
`dotnet ef database update`
- заполнение тестовыми данными  
`dotnet run /seed`

### Запуск
В Configure Startup Projects... стартовать проекты:
- DeveloperPath.IdentityProvider  
- DeveloperPath.WebApi  
- DeveloperPath.WebUI  

### Тестирование
В интеграционных тестах Application.IntegrationTests используется реальная тестовая СУБД DeveloperPathTestDb, которая воссоздаётся из реальной при запуске тестов. Пока нет возможности использовать in-memory базу.

## 3. Утилиты
### [Stryker](https://stryker-mutator.io/)
Утилита используется для мутационного тестирования кода.

#### Установка
В CLI в корне проекта выполнить `dotnet tool restore`, чтобы восстановить утилиту. Сведения о ней уже есть в `.config\dotnet-tools.json`.

#### Использование
В CLI из папки каждого проекта теста выполнить `dotnet stryker`.  В некоторых случаях нужно указать проект для тестирования, например, `-p Application.cproj`.  
Утилита создаст папку `StrykerOutput`, в подпапке `reports` будет HTML документ с отчётом о прошедшем тестировании.  
Можно настраивать разные параметры утилиты в каждом тестовом проекте в файле `stryker-config.json`.

## 4. Решение проблем

### Тестирование
Если не выполняются тесты из Application.IntegrationTests, удалите базу данных DeveloperPathTestDb. Она будет создана повторно, когда запустятся тесты.

