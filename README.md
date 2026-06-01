## 1. Project Name
   Warehouse Management System

## 2. Short Description
   Desktop enterprise warehouse management application built with WPF and .NET.
   The project focuses on clean architecture, scalability, role-based access control, audit logging, and enterprise application design principles.

## 3. Registration, Authentication and Authorization

* creation of the first administrator during the first system launch
* password-protected system login
* role-based access control
* user session management

## 4. User Management

* creation and viewing of users
* activation and deactivation
* failed login attempts monitoring and reset
* access unlocking
* password reset

## 5. Support Center

* support ticket creation by users
* ticket processing by administrators
* ticket status system
* interaction between users and administrators through ticket status changes, descriptions, and comments

## 6. Action Logging

* centralized system event logging
* database record change tracking
* monitoring of actions performed by the system or users

## 7. UI/UX

* MVVM architecture
* Material Design integration
* dialog window system
* Snackbar notifications
* centralized navigation based on a single application window

## 8. Onion Architecture

* UI layer (WPF + MVVM)
* Infrastructure layer
* Application layer
* Domain layer

## 9. Architectural Principles

* separation of concerns
* dependency injection
* interface-based abstractions
* service-oriented business logic

## 10. Technologies

* C# (WPF)
* Entity Framework Core + PostgreSQL
* Microsoft Dependency Injection
* Community Toolkit MVVM
* Material Design XAML Toolkit
* Serilog

## 11. Project Status

The project is currently under development.

### Implemented:
* core application architecture
* authentication system
* authorization and roles
* user management
* support center
* system action logging

### Planned:
* product categories
* products
* suppliers
* warehouse operations
* warehouse statistics dashboard

## 12. Requirements

* .NET SDK
* Visual Studio

## 13. Project Launch

* clone the repository
* configure the PostgreSQL connection string
* apply migrations
* run the WPF project

## 14. Screenshots
