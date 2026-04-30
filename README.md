# Event Management Platform – PopUpOslo

## Team

- Karyna Zakabluk
- Maria Vilma


## Description
PopUpOslo is a console-based event management platform developed in C# with SQLite as the database.  

The system allows users to:
- Create and manage events
- Discover and search events
- Book and cancel participation
- Leave reviews after attending events

The system will:
- Support user registration and authentication
- Enable event creation and management
- Provide a booking system with capacity control
- Allow searching and filtering of events
- Enable users to leave reviews

---

## Features

### User
- Register and log in
- Browse and search events
- View event details
- Book tickets
- Cancel bookings
- View booking history
- Leave reviews

### Admin
- Create and browse events
- Edit and cancel events
- View own events
- View all events
- Manage event status
- Book events
- View personal bookings

---

## Tools & Technologies
- Language: C# (.NET)
- Database: SQLite
- Version Control: GitHub 
- IDE: Rider

---

## Design Decisions

The system is built using object-oriented principles:

- Encapsulation: controlled access to data
- Inheritance: event types extend a base Event class
- Polymorphism: shared behavior across event types
- Enums: used for categories, status, and booking states
- LINQ: used for filtering and data processing

The application follows a layered architecture:
- UI layer (console interaction)
- Application layer (services)
- Domain layer (models)
- Infrastructure layer (repositories)

---

## How to Run

1. Clone the repository:
```bash
   git clone https://github.com/karynazakabluk/PopUpOslo.git
````
2. Navigate to the project:
``` bash
cd PopUpOslo
```

3. Run the application:
```bash
dotnet run
```
---

## Documentation

See the docs/ folder:
- Requirements Specification
- Process Report
- Project Plan
- Task Tracking
- AI Usage
- UML Diagram

---

## Known Limitations

- Role-based access is not fully implemented
- Some features depend on seed data configuration
- Console UI is basic

---
