# Event Management Platform – PopUpOslo

## Team

- Maria
- Karyna

## Description
PopUpOslo is a console-based event management platform developed in C#.  
The system allows users to browse events, book tickets, and leave reviews.  
Users can also organise and manage their own events.

The project demonstrates object-oriented programming concepts and structured application design.

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

### Organiser
- Create events
- Edit and cancel events
- View own events
- View all events
- Manage event status
- Book events
- View personal bookings

---

## Technologies
- C#
- .NET Console Application
- SQLite
- LINQ
- Repository pattern

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

## AI Usage

AI tools were used to:
- assist with debugging and issue resolution
- support improvements in console interaction
- help structure and refine documentation

All generated content was reviewed and adapted by the team.