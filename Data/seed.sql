-- =========================
-- CLEAN DATABASE
-- =========================
DELETE FROM Reviews;
DELETE FROM Bookings;
DELETE FROM BookingOptions;
DELETE FROM Events;
DELETE FROM Users;

-- =========================
-- USERS
-- Roles:
-- 0 = User
-- 1 = Organizer
-- 2 = Admin
-- =========================
INSERT INTO Users (Username, PasswordHash, Role) VALUES
                                                     ('alice', 'alice123', 2),   -- Admin
                                                     ('bob', 'bob123', 1),       -- Organizer 1
                                                     ('dave', 'dave123', 1),     -- Organizer 2
                                                     ('charlie', 'charlie123', 0); -- Normal User

-- =========================
-- EVENTS (10 total)
-- bob = UserId 2
-- dave = UserId 3
-- =========================

INSERT INTO Events
(Title, Description, Category, Type, DateTime, Venue, Status, OrganizerId) VALUES

-- Organizer 2 (Bob)
('Startup Pitch Night', 'Pitch your startup ideas', 'Networking', 'Workshop', '2026-06-01 18:00', 'Oslo Tech Hub', 'Upcoming', 2),
('AI for Beginners', 'Intro to Artificial Intelligence', 'Education', 'Workshop', '2026-06-03 17:00', 'Oslo Center', 'Upcoming', 2),
('Web Dev Bootcamp', 'Learn full-stack basics', 'Education', 'Workshop', '2026-06-05 10:00', 'Code Lab Oslo', 'Upcoming', 2),
('Marketing Masterclass', 'Digital marketing strategies', 'Education', 'Workshop', '2026-06-07 14:00', 'Business Arena', 'Upcoming', 2),
('Freelancing 101', 'Start your freelancing career', 'Networking', 'Workshop', '2026-06-09 16:00', 'Oslo Hub', 'Upcoming', 2),

-- Organizer 3 (Dave)
('Fine Dining Experience', 'Luxury dining event', 'Food', 'Dining', '2026-06-02 20:00', 'Grand Hotel Oslo', 'Upcoming', 3),
('Wine Tasting Evening', 'Exclusive wine tasting', 'Food', 'Dining', '2026-06-04 19:00', 'Oslo Wine Cellar', 'Upcoming', 3),
('Street Food Festival', 'Global food experience', 'Food', 'Dining', '2026-06-06 12:00', 'Oslo Park', 'Upcoming', 3),
('Cooking Masterclass', 'Chef-led cooking session', 'Food', 'Workshop', '2026-06-08 15:00', 'Culinary Studio', 'Upcoming', 3),
('Gourmet Dessert Night', 'Dessert tasting experience', 'Food', 'Dining', '2026-06-10 18:00', 'Sweet House Oslo', 'Upcoming', 3);
-- =========================
-- BOOKING OPTIONS
-- (2 options per event)
-- Event IDs assumed 1–10
-- =========================

INSERT INTO BookingOptions (EventId, Name, Price, Capacity, RemainingCapacity) VALUES

-- Event 1
(1, 'Standard', 100, 50, 50),
(1, 'VIP', 200, 20, 20),

-- Event 2
(2, 'Standard', 120, 50, 50),
(2, 'VIP', 250, 20, 20),

-- Event 3
(3, 'Standard', 150, 40, 40),
(3, 'VIP', 300, 15, 15),

-- Event 4
(4, 'Standard', 130, 60, 60),
(4, 'VIP', 280, 25, 25),

-- Event 5
(5, 'Standard', 90, 80, 80),
(5, 'VIP', 180, 30, 30),

-- Event 6
(6, 'Standard', 500, 30, 30),
(6, 'VIP', 900, 10, 10),

-- Event 7
(7, 'Standard', 300, 40, 40),
(7, 'VIP', 600, 15, 15),

-- Event 8
(8, 'Standard', 80, 100, 100),
(8, 'VIP', 150, 40, 40),

-- Event 9
(9, 'Standard', 200, 60, 60),
(9, 'VIP', 400, 20, 20),

-- Event 10
(10, 'Standard', 250, 50, 50),
(10, 'VIP', 500, 20, 20);

-- =========================
-- BOOKINGS
-- =========================

INSERT INTO Bookings (UserId, EventId, OptionId, Price, Status) VALUES

-- Charlie (User)
(4, 1, 1, 100, 'Booked'),
(4, 6, 11, 500, 'Booked'),

-- Bob (Organizer booking)
(2, 6, 11, 500, 'Booked'),
(2, 8, 15, 80, 'Booked'),

-- Dave (Organizer booking)
(3, 1, 2, 200, 'Booked'),
(3, 3, 5, 150, 'Booked'),

-- Alice (Admin)
(1, 2, 3, 120, 'Booked'),
(1, 7, 13, 300, 'Booked');

-- =========================
-- REVIEWS
-- =========================

INSERT INTO Reviews (UserId, EventId, Rating, Comment) VALUES
                                                           (4, 1, 5, 'Great startup event!'),
                                                           (3, 3, 4, 'Very useful workshop'),
                                                           (2, 6, 5, 'Amazing food experience'),
                                                           (1, 7, 5, 'Excellent wine tasting');