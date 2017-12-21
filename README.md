# Error Logger

This ASP.NET MVC Web Application, is built for users to track error logs from multiple applications and an Admin who can track all users, applications and error logs in the system and take care of user access to the application pool.

**Technologies Used**: HTML, CSS, JS, jQuery, C#, ASP.NET MVC 5 w/ Razer.

## Usage

* Download/Clone the Repo
* Open _CSE686_FinalProject.sln_ with Visual Studio 2015 or higher.
* Make sure there are two start-up projects, _CSE686_FinalProject_ and _LoggerService_.
* Run the solution!

## Project Structure

- [CSE686_FinalProject](#CSE686_FinalProject)
- [LoggerService](#LoggerService)
- [ErrorLoggerModel](#ErrorLoggerModel)
- [LoadersAndLogic](#LoadersAndLogic)
- [CommonModule](#CommonModule)
- [DLL_Library](#DLL_Library)    
- [Changelog](#changelog)

### CSE686_FinalProject

The main project containing the Views and Controllers. Controllers are mainly split into Admin, Applications and ErrorLogs. All pages use a shared layout page (Shared/_Layout.cshtml).

### LoggerService

The REST Service containing the APIs. The service is always running, and waits for applications to consume its API to send new error logs. These error logs are stored into the Database.

### ErrorLoggerModel

The SQL Database definition, with each table defined as Model Classes for ASP.NET's Code-First approach using LINQ to SQL.

### LoadersAndLogic

Contains Data Handlers for each table, used to interact with the tables for CRUD operations.

### CommonModule

Code Snippets shared across the solution.

### DLL_Library

Used to automate a job which creates new applications and error logs for them. Can be used for load testing.

### Changelog

#### 1.0

* Initial Commit
