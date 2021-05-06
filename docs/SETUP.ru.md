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

