-- =========================
-- USERS (2 users)
-- =========================
INSERT INTO Users (Username, PasswordHash, Role) VALUES
                                                     ('alice', 'alice123', 'User'),
                                                     ('bob', 'bob123', 'User');

-- =========================
-- EVENTS (3 events)
-- =========================
INSERT INTO Events (Name, Category, Type, DateTime) VALUES
                                                        ('Oslo Music Night', 'Music', 'Concert', '2026-06-01'),
                                                        ('Tech Startup Meetup', 'Technology', 'Workshop', '2026-06-05'),
                                                        ('Food Festival Oslo', 'Food', 'Festival', '2026-06-10');

-- =========================
-- BOOKING OPTIONS
-- (Each event has options + capacity for testing)
-- =========================
INSERT INTO BookingOptions (EventId, Name, Price, Capacity, RemainingCapacity) VALUES
                                                                                   (1, 'Standard Ticket', 120, 2, 2),
                                                                                   (1, 'VIP Ticket', 300, 1, 1),
                                                                                   (2, 'General Entry', 0, 3, 3),
                                                                                   (3, 'Food Pass', 50, 2, 2);

-- =========================
-- BOOKINGS (FULL FLOW TEST DATA)
-- =========================

-- Alice bookings
INSERT INTO Bookings (UserId, BookingOptionId, Status) VALUES
                                                           (1, 1, 'Confirmed'),   -- Alice books Music Night
                                                           (1, 3, 'Pending');     -- Alice reserves Tech Meetup (not confirmed yet)

-- Bob bookings
INSERT INTO Bookings (UserId, BookingOptionId, Status) VALUES
                                                           (2, 1, 'Confirmed'),   -- Bob books Music Night (fills capacity -> SOLD OUT)
                                                           (2, 2, 'Cancelled'),   -- Bob cancels VIP ticket
                                                           (2, 4, 'Confirmed');   -- Bob attends Food Festival