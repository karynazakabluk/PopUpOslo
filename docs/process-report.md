# Process Report – PopUpOslo

## 1. Development Approach

The development of the PopUpOslo system followed an Agile-inspired approach, focusing on incremental development and flexibility rather than a strict framework.

The system was built step by step, implementing core features such as:
- data layer
- booking functionality
- console-based user interface

During development, we applied key Agile principles:
- incremental feature development
- continuous testing and debugging
- flexibility when requirements changed

---

## 2. Roles and Responsibilities

Although the project was developed collaboratively, responsibilities were divided across system areas:

- UI Layer: responsible for menu navigation and user interaction
- Application Layer: handled business logic (services)
- Domain Layer: defined core models such as User, Event, Booking, and Review
- Infrastructure Layer: managed data access using repositories

Responsibilities evolved during development as integration challenges appeared.

---

## 3. Development Practices

### Version Control
- Git and GitHub were used for version control
- Feature branches were created for major functionality
- Challenges with branch management were resolved during development

### Branching Strategy
- Feature-based workflow
- Each feature (e.g., booking logic, repositories) was developed in a separate branch
- Changes were merged into the main branch

### Code Structure
- Layered architecture (UI, Application, Domain, Infrastructure)
- Repository pattern used for data access

### Debugging and Testing
- Manual testing during development
- Errors were identified and fixed through iterative debugging

### Task Tracking
- Tasks were tracked informally through Git commits and feature completion

---

## 4. Reflection

### What Went Well
- Clear layered architecture improved code organization
- Functional booking system with multiple components
- Improved experience with Git and branching

### Challenges
- Initial confusion with Git branches and commits
- Compilation errors during development
- Managing dependencies between layers

### What Could Be Improved
- Better planning at the start of the project
- More structured task tracking
- Earlier and more frequent code reviews

---

## 5. Conclusion

The development of the PopUpOslo system provided valuable experience in software development, system design, and version control.

The project highlighted the importance of planning, structured architecture, and effective collaboration, which will be applied in future projects.