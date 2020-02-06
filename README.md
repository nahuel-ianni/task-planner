# Task planner
This application was created back in February, 2016, over the course of a weekend for an against the clock challenge, with the objective of showcasing the Universal Windows Platform (*UWP*) basic capabilities when creating a task planner app.

The premise and objective were straightforward: Create an application that will manage a simple *to-do* list using the *UWP* framework.

The application has some extra functionality added on top of the original premise:

- It enables copying a *to-do* task into the Windows calendar app
- When the application is pinned to the *Start* menu, a background task will try to update it every 15 minutes with the relevant information of the next task on the calendar
- An extra background task was added, responsible for synchronizing tasks arriving from outside sources


## Installation and configuration
### Prerequisites
In order to be able to compile the application, make sure the following components are installed on your machine:

- [Windows 10 SDK](https://developer.microsoft.com/en-us/windows/downloads/sdk-archive) 10240 or higher
- [Sqlite 3 VSIX](https://sqlite.org/download.html) package for the Universal Windows Platform

### Project configuration
Make sure the target platform for each project is set to `x86` and the project build order is configured to:

1. DataAccess.Interfaces
2. DataAccess
3. DataModels
4. BackgroundTasks
5. SystemIntegration
6. TaskPlanner


## Notes
### Compatibility
The application was originally created utilizing the *SDK 10240*.  
By the time it was uploaded to a public repository, the *UWP* framework had gone through several minor and major updates. Minor changes were done to the code to ensure proper functioning of the code with newer versions of Windows 10, while retaining compatibility with the original target.

### Template 10
The application makes use of [Template10](https://github.com/Windows-XAML/Template10/wiki)'s *hamburger template* to avoid having to implement an [MVVM](https://en.wikipedia.org/wiki/Model–view–viewmodel) framework from scratch.

*Template 10* is an open source project started by Microsoft employees, aimed at providing a light *MVVM* framework with common functionality for *UWP* applications.

This project makes use of the `ViewModelBase` class which facilitates communication between objects and the UI layer, by implementing the `INotifyPropertyChanged` interface, and navigation between views.

### Dependency Injection
While no [dependency injection](https://en.wikipedia.org/wiki/Dependency_injection) framework was used on the application, *user interfaces* were put in place and implemented on every layer and module, making it possible to implement one without much hassle.

The *System Integration* module implements manager classes marked as static in an attempt at improving performance.

### ORM
The usage of [Entity Framework](https://docs.microsoft.com/en-us/ef/#pivot=efcore) and a proper database engine were discarded as a possibility due to lack of support by the *.Net UWP* framework  at the time and the complexity overhead they would have brought to the project.

A custom implementation for data access with a database created with [Sqlite](https://sqlite.org/index.html) was used instead.

### Entity abstraction
Given the simplicity of the application and time constraints, no web services or APIs were implemented on the backend, impacting the implementation of the *data models* project.

This resulted in only one data entity used for the overall project, with no distinction between backend and frontend objects, the `TaskItem` class.  
It implements an observable pattern via the *INotifyPropertyChanged* implementation as well as the *PrimaryKey* and *AutoIncrement* attributes from the *SQLite* library, effectively coupling the *DataModels* project to layers outside its scope.

### Adaptive layout
The UI makes use of XAML adaptive layouts, enabling the application to be used on any Windows 10 device.

A specific architecture based on device is advised if the application is to be improved, adding extra user stories and extending its scope.

### Data manipulation
Data integrity was a priority when creating a task planner app. In order to ensure this, the following steps were taken:

- The application will cache the *main page list* whenever enough system resources are available
- The application manages the suspension events by saving all the relevant information into the database before shutting down


## Known issues
- Trying to synchronize a single task with the calendar can sometimes give a range exception based on the selected date for the task. Despite this, the task will get synchronized
- The *save*, *cancel* and *sync all* task buttons are always enabled, even if no tasks are available or their state has not changed
- Synchronizing the same task multiple times will create a new instance in the calendar each time
- Minor UI bugs on the task details panel. The text box width doesn't adapt to the container's width, which has a predefined maximum value