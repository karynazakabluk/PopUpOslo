# Project Plan – PopUpOslo

## Overview

The project was developed as a console-based event management system with booking functionality.

Work was divided into features and implemented incrementally using Git branches in a collaborative workflow.

---

## Task Breakdown

| Task | Responsible | Status |
|------|-------------|--------|
| User system | Karyna      | Done |
| Event management | Both        | Done |
| Booking system | Maria       | Done |
| Ticket options | Maria       | Done |
| Reviews | Karyna      | Done |
| Admin functionality | Both        | Done |
| Console UX improvements | Karyna      | Done |
| Documentation | Both        | Done |

---

## Development Approach

The team followed a feature-based workflow:

- Each feature was implemented in a separate branch
- Features were tested before merging
- Work progressed incrementally

---

## Timeline (Simplified)

- Phase 1: Setup project and basic structure
- Phase 2: Implement core features (events, booking)
- Phase 3: Add reviews and admin functionality
- Phase 4: Improve console UX
- Phase 5: Final testing and documentation
- Phase 6: Final adjustments and submission


## Project Structure

```text
PopUpOslo/
├── Data/
│   ├── DatabaseInitializer.cs
│   └── seed.sql
├── Database/
│   ├── database.db
│   ├── DatabaseContext.cs
│   └── schema.sql
├── docs/
│   ├── ai-usage.md
│   ├── process-report.md
│   ├── project-plan.md
│   ├── requirements.md
│   ├── task-tracking.md
│   └── uml.puml
├── Domain/
│   ├── Entities/
│   │   ├── Booking.cs
│   │   ├── BookingOption.cs
│   │   ├── DiningEvent.cs
│   │   ├── Event.cs
│   │   ├── Review.cs
│   │   ├── User.cs
│   │   └── WorkshopEvent.cs
│   └── Enums/
│       ├── BookingStatus.cs
│       ├── EventCategory.cs
│       ├── EventStatus.cs
│       ├── EventType.cs
│       └── UserRole.cs
├── Infrastructure/
│   └── Repositories/
│       ├── BaseRepository.cs
│       ├── BookingOptionRepository.cs
│       ├── BookingRepository.cs
│       ├── EventRepository.cs
│       ├── ReviewRepository.cs
│       └── UserRepository.cs
├── Services/
│   ├── AuthService.cs
│   ├── BookingOptionService.cs
│   ├── BookingService.cs
│   ├── EventService.cs
│   ├── ReviewService.cs
│   └── SearchService.cs
├── UI/
│       ├── ApplicationRunner.cs
│       ├── InputHandler.cs
│       └── Menu.cs
├
├── PopUpOslo.csproj
├── Program.cs
└── README.md





## Gantt Chart

| Task                  | Week 1 | Week 2 | Week 3 | Week 4 | Week 5 |
|-----------------------|--------|--------|--------|--------|--------|
| Project Setup         | ████   |        |        |        |        |
| Database Design       | ████   |        |        |        |        |
| User Authentication   | ████   |        |        |        |        |
| Event Management      |        | ████   |        |        |        |
| Booking System        |        | ████   | ████   |        |        |
| Search & Filter       |        |        | ████   |        |        |
| Review System         |        |        |        | ████   |        |
| Testing               |        |        |        | ████   | ████   |
| UI Improvements       |        |        |        | ████   | ████   |
| Documentation         |        |        |        |        | ████   |