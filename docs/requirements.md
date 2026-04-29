# Requirements Specification – PopUpOslo

## 1. Introduction

This document describes the functional requirements of the PopUpOslo event management system.

---

## 2. User Stories

### 1. View Events
As a user, I want to view available events so that I can choose what to attend.

Acceptance Criteria:
- Events are listed with title, date, and price
- Only upcoming events are shown

---

### 2. View Event Details
As a user, I want to see detailed information about an event so that I can decide whether to book.

Acceptance Criteria:
- Event description is displayed
- Ticket types and prices are shown

---

### 3. Book Ticket
As a user, I want to book a ticket for an event so that I can attend it.

Acceptance Criteria:
- User can select ticket type
- Booking is saved successfully
- Confirmation message is displayed

---

### 4. Check Ticket Availability
As a user, I want to see how many tickets are left so that I know availability.

Acceptance Criteria:
- Remaining ticket count is shown
- Booking is prevented if tickets are sold out

---

### 5. Cancel Booking
As a user, I want to cancel my booking so that I can free up tickets.

Acceptance Criteria:
- Booking is removed from system
- Ticket count is updated

---

### 6. Register User
As a new user, I want to register an account so that I can book events.

Acceptance Criteria:
- User provides name and role
- User is stored in the system

---

### 7. Login User
As a user, I want to log in so that I can access my bookings.

Acceptance Criteria:
- System validates user
- Access is granted if valid

---

### 8. Admin Manage Events
As an admin, I want to create and manage events so that events can be updated.

Acceptance Criteria:
- Admin can add, edit, and delete events
- Changes are reflected immediately

---

### 9. View Bookings
As a user, I want to see my bookings so that I can manage them.

Acceptance Criteria:
- All bookings for the user are listed
- Each booking shows event and ticket type

---

### 10. Add Review
As a user, I want to leave a review for an event so that I can share feedback.

Acceptance Criteria:
- User can submit a review
- Review is linked to event