# Application setup

## 1. System requirements
  - .NET 9
  - MSSQL or similar DBMS

## 2. DataBase
In `appsettings.json` parameter `"UseInMemoryDatabase"` defines whether application is using in-memory database (`true`) or actual DBMS (`false`).

### Migrations
#### Main database (empty DB must be created in MS SQL)
Set connection string for local use is in `\src\DeveloperPath.WebApi\appsettings.Development.json` property `DeveloperPathSqlConnectionString`.

Alternatively, in user-secrets:
- Using CLI from `DeveloperPath.WebApi` folder, run   
`dotnet user-secrets init`  
- Then   
`dotnet user-secrets set "DeveloperPathSqlConnectionString" "Data Source=..."`.

Using CLI from `\src\DeveloperPath.Infrastructure`, run:
- update ef tools  
`dotnet tool update --global dotnet-ef`
- database update  
`dotnet ef --startup-project ..\DeveloperPath.WebApi\ database update`
- revert to migration   
`dotnet ef --startup-project ..\DeveloperPath.WebApi\ database update <NAME>`
- add migration  
`dotnet ef --startup-project ..\DeveloperPath.WebApi\ migrations add <NAME>`
- remove migration  
`dotnet ef --startup-project ..\DeveloperPath.WebApi\ migrations remove`

Database is filled with test data automatically when application starts, if it doesn't find data in Paths.

#### Identity data (a separate empty DB must be created in MS SQL)
Set in `\src\IdentityProvider\appsettings.Development.json`
- connection string for local use, `SqlConnection` property,
- password for accessing SwaggerApi, `PathApiSwaggerSecret` property.

Alternatively, set the same in user-secrets (see example above).

Using CLI from `\src\DeveloperPath.IdentityProvider` run:
- database update  
`dotnet ef database update`
- seed test data  
`dotnet run /seed`

### Start
In Configure Startup Projects... set Start for these projects:
- DeveloperPath.IdentityProvider  
- DeveloperPath.WebApi  
- DeveloperPath.WebUI 

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