DELETE FROM Reviews;
DELETE FROM Bookings;
DELETE FROM BookingOptions;
DELETE FROM Events;
DELETE FROM Users;

-- =========================
-- USERS (2 users)
-- =========================
INSERT INTO Users (Username, PasswordHash) VALUES
                                               ('alice', 'alice123'),
                                               ('bob', 'bob123');

-- =========================
-- EVENTS
-- OrganizerId must match Users (1 = alice, 2 = bob)
-- =========================
INSERT INTO Events
(Title, Description, Category, Type, DateTime, Venue, Status, OrganizerId) VALUES

                                                                               ('Oslo Food Workshop', 'Learn cooking basics with chefs', 'Food', 'Workshop', '2026-06-01 18:00', 'Oslo Center Hall', 'Upcoming', 1),

                                                                               ('Tech Networking Night', 'Meet developers and startups', 'Technology', 'Workshop', '2026-06-05 19:00', 'Tech Hub Oslo', 'Upcoming', 1),

                                                                               ('Fine Dining Experience', 'Exclusive luxury dining event', 'Food', 'Dining', '2026-06-10 20:00', 'Luxury Hotel Oslo', 'Upcoming', 2);

-- =========================
-- EXTRA EVENTS (5 NEW)
-- =========================
INSERT INTO Events
(Title, Description, Category, Type, DateTime, Venue, Status, OrganizerId) VALUES

                                                                               ('Startup Pitch Night', 'Pitch your startup ideas to investors', 'Networking', 'Workshop', '2026-06-12 18:00', 'Oslo Innovation Hub', 'Upcoming', 1),

                                                                               ('AI & Machine Learning Intro', 'Beginner friendly AI workshop', 'Technology', 'Workshop', '2026-06-14 17:00', 'Oslo Tech Center', 'Upcoming', 1),

                                                                               ('Street Food Festival', 'Taste food from around the world', 'Food', 'Dining', '2026-06-16 12:00', 'Oslo City Park', 'Upcoming', 2),

                                                                               ('Career Growth Seminar', 'Improve your career skills', 'Education', 'Workshop', '2026-06-18 15:00', 'Oslo Conference Hall', 'Upcoming', 2),

                                                                               ('Cultural Evening Oslo', 'Experience global cultures and music', 'Culture', 'Dining', '2026-06-20 19:00', 'Oslo Cultural Center', 'Upcoming', 1);

-- =========================
-- BOOKING OPTIONS
-- (NOTE: Removed RemainingCapacity because your schema does NOT have it)
-- =========================
INSERT INTO BookingOptions (EventId, Name, Price, Capacity, RemainingCapacity) VALUES
                                                                                   (1, 'Standard Pass', 100, 2, 2),
                                                                                   (1, 'VIP Pass', 250, 1, 1),
                                                                                   (2, 'Entry Ticket', 200, 3, 3),
                                                                                   (3, 'Dinner Seat', 500, 2, 2);


-- =========================
-- BOOKING OPTIONS (NEW EVENTS)
-- =========================

INSERT INTO BookingOptions (EventId, Name, Price, Capacity, RemainingCapacity) VALUES
                                                                                   (1, 'Standard Pass', 100, 2, 2),
                                                                                   (1, 'VIP Pass', 250, 1, 1),
                                                                                   (2, 'Entry Ticket', 200, 3, 3),
                                                                                   (3, 'Dinner Seat', 500, 2, 2),

                                                                                   (4, 'Standard Pass', 150, 50, 50),
                                                                                   (4, 'VIP Pass', 300, 10, 10),

                                                                                   (5, 'Early Bird Ticket', 120, 40, 40),
                                                                                   (5, 'Regular Ticket', 200, 60, 60),

                                                                                   (6, 'Food Entry Pass', 100, 100, 100),
                                                                                   (6, 'VIP Food Tasting', 250, 20, 20),

                                                                                   (7, 'Seminar Entry', 80, 80, 80),
                                                                                   (7, 'Premium Seat', 180, 20, 20),

                                                                                   (8, 'Cultural Pass', 90, 70, 70),
                                                                                   (8, 'VIP Experience', 220, 15, 15);
-- =========================
-- BOOKINGS
-- Status: Booked / Cancelled / Completed
-- =========================

-- Alice bookings
INSERT INTO Bookings (UserId, EventId, OptionId, Price, Status) VALUES
                                                                    (1, 1, 1, 100, 'Booked'),
                                                                    (1, 2, 3, 0, 'Booked');

-- Bob bookings
INSERT INTO Bookings (UserId, EventId, OptionId, Price, Status) VALUES
                                                                    (2, 1, 1, 100, 'Booked'),
                                                                    (2, 3, 4, 500, 'Booked'),
                                                                    (2, 2, 3, 0, 'Cancelled');

-- =========================
-- REVIEWS
-- =========================

INSERT INTO Reviews (UserId, EventId, Rating, Comment) VALUES
                                                           (1, 1, 5, 'Amazing workshop experience!'),
                                                           (2, 1, 4, 'Very good and useful session'),
                                                           (2, 3, 5, 'Excellent dining experience');