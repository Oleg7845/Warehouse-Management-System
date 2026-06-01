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

<img width="701" height="430" alt="create_initial_admin" src="https://github.com/user-attachments/assets/8de32dfe-7591-4922-8f02-f6435730218e" />
<img width="704" height="432" alt="authentication" src="https://github.com/user-attachments/assets/c341299c-09b6-4f29-859c-dfd72efb5dbe" />
<img width="702" height="431" alt="force_initial_pasword_change" src="https://github.com/user-attachments/assets/a8b1a64f-0322-44f0-ba4c-22e1278ea103" />
<img width="1919" height="1029" alt="audit_log_list" src="https://github.com/user-attachments/assets/3f1df81e-bb13-470c-89ce-a0462dee1314" />
<img width="1916" height="1028" alt="view_audit_log_dialog" src="https://github.com/user-attachments/assets/45c350e0-a51e-4f46-8d7b-07837bf0dfaf" />
<img width="1918" height="1028" alt="user_list" src="https://github.com/user-attachments/assets/8690f1fe-862d-4ef7-885f-3019d903e625" />
<img width="1917" height="1029" alt="create_user_dialog" src="https://github.com/user-attachments/assets/ce0f6279-fe09-4ee9-b8b7-a5664661e614" />
<img width="1917" height="1028" alt="user_manage_dialog" src="https://github.com/user-attachments/assets/67c74077-9aee-4377-a290-2bc860117d51" />
<img width="703" height="431" alt="snackbar_notification_1" src="https://github.com/user-attachments/assets/48ce6f4e-08ea-44cc-8e6b-4d6338a7586b" />
<img width="703" height="431" alt="create_support_ticket_dialog" src="https://github.com/user-attachments/assets/de497a1e-fe44-455e-b56d-bc1973f5c205" />
<img width="703" height="431" alt="snackbar_notification_2" src="https://github.com/user-attachments/assets/8775c452-ed29-4f88-921a-bf28981adc3c" />
<img width="1919" height="1030" alt="user_view_own_support_ticket_dialog" src="https://github.com/user-attachments/assets/5b135202-e136-4637-89f8-999b2e37ae9a" />
<img width="1917" height="1029" alt="user_support_tickets" src="https://github.com/user-attachments/assets/0cbfd74f-10e9-4628-8dc4-910482cafe7c" />
<img width="1916" height="1028" alt="view_audit_log_dialog" src="https://github.com/user-attachments/assets/86c0972c-6ccc-4215-bcaf-96755a6ca6be" />
<img width="1917" height="1030" alt="admin_manage_support_ticket_dialog" src="https://github.com/user-attachments/assets/da7f876c-5306-4bf8-bb2f-952b67ae3ff7" />
<img width="1919" height="1028" alt="admin_manage_support_ticket_dialog_2" src="https://github.com/user-attachments/assets/97191aaa-e190-40f7-976d-63ef256e7356" />
<img width="1918" height="1029" alt="admin_manage_support_ticket_dialog_3" src="https://github.com/user-attachments/assets/2bc141f2-51e0-4846-a2cd-605869ffde50" />
