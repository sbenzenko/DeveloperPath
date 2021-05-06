# Application setup

## 1. System requirements
  - .NET 5
  - MSSQL or similar DBMS

## 2. DataBase
In `appsettings.json` parameter `"UseInMemoryDatabase"` defines whether application is using in-memory database (`true`) or actual DBMS (`false`).

### Migrations
#### Main database
Connection string for local use is in `\src\WebApi\appsettings.Development.json` parameter `DeveloperPathSqlConnectionString`.

Using CLI from `\src\Infrastructure` run:
- database update  
`dotnet ef --startup-project ..\WebUI\ database update`
- revert to migration   
`dotnet ef --startup-project ..\WebUI\ database update <NAME>`
- add migration  
`dotnet ef --startup-project ..\WebUI\ migrations add <NAME>`
- remove migration  
`dotnet ef --startup-project ..\WebUI\ migrations remove`

Database is filled with test data automatically when application starts, if it doesn't find data in Paths.

#### Identity data
Connection string for local use is in `\src\IdentityProvider\appsettings.Development.json` `"SqlConnection"`.

Using CLI from `\src\IdentityProvider` run:
- database update  
`dotnet ef database update`
- seed test data  
`dotnet run /seed`

### Testing
In integration tests Application.IntegrationTests real testing database is used. It's impossible to use in-memory database for testing. Testing database DeveloperPathTestDb is re-created based on real database structure when tests are executed.

## 3. Tools
### [Stryker](https://stryker-mutator.io/)
This is a tool for mutation testing.

#### Setup
Using CLI in the root of the solution run  `dotnet tool restore`, to restore the tool. It's already configured in `.config\dotnet-tools.json`.

#### Usage
Using CLI from each testing project folder run `dotnet stryker`. In some cases you will need to provide project name, ex. `-p Application.cproj`.  
The tool will create `StrykerOutput` folder. Inside in `reports` folder is an HTML document with the report.  
You can change the tool settings in each testing project folder in `stryker-config.json` file.

## 4. Troubleshooting

### Testing
If tests from Application.IntegrationTests don't run, delete database DeveloperPathTestDb. It will be re-created when testing starts.