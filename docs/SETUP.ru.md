# Установка приложения

## 1. Системные требования
  - .NET 5
  - MSSQL или другая СУБД

## 2. База данных
В файле `appsettings.json` параметр `UseInMemoryDatabase` задаёт использование БД в памяти (`true`) или реальной СУБД (`false`).

### Миграции
#### Основная база
Строка подключения для локального использования - см. `\src\WebApi\appsettings.Development.json` параметр `DeveloperPathSqlConnectionString`.

Используя CLI из папки `\src\Infrastructure`:
- обновление базы  
`dotnet ef --startup-project ..\WebUI\ database update`
- откат к миграции   
`dotnet ef --startup-project ..\WebUI\ database update <NAME>`
- добавление миграции  
`dotnet ef --startup-project ..\WebUI\ migrations add <NAME>`
- удаление миграции  
`dotnet ef --startup-project ..\WebUI\ migrations remove`

База заполнится тестовыми данными автоматически при старте приложения, если нет данных в Paths.

#### Identity
Строка подключения для локального использования - см. `\src\IdentityProvider\appsettings.Development.json` параметр `SqlConnection`.

Используя CLI из папки `\src\IdentityProvider`:
- обновление базы  
`dotnet ef database update`
- заполнение тестовыми данными  
`dotnet run /seed`

### Тестирование
В интеграционных тестах Application.IntegrationTests используется реальная тестовая СУБД DeveloperPathTestDb, которая воссоздаётся из реальной при запуске тестов. Пока нет возможности использовать in-memory базу.

## 3. Решение проблем

### Тестирование
Если не выполняются тесты из Application.IntegrationTests, удалите базу данных DeveloperPathTestDb. Она будет создана повторно, когда запустятся тесты.

