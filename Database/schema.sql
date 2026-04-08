CREATE TABLE IF NOT EXISTS Users (
                       UserId INTEGER PRIMARY KEY AUTOINCREMENT,
                       Username TEXT NOT NULL UNIQUE,
                       PasswordHash TEXT NOT NULL
);


CREATE TABLE IF NOT EXISTS Events (
                        EventId INTEGER PRIMARY KEY AUTOINCREMENT,
                        Title TEXT NOT NULL,
                        Description TEXT,
                        Category TEXT,
                        Type TEXT CHECK(Type IN ('Dining', 'Workshop')),
                        DateTime TEXT NOT NULL,
                        Venue TEXT NOT NULL,
                        Status TEXT DEFAULT 'Upcoming' CHECK(Status IN ('Upcoming', 'Completed', 'Cancelled')),
                        OrganizerId INTEGER NOT NULL,

                        FOREIGN KEY (OrganizerId) REFERENCES Users(UserId)
                            ON DELETE CASCADE
);


CREATE TABLE IF NOT EXISTS BookingOptions (
                                OptionId INTEGER PRIMARY KEY AUTOINCREMENT,
                                EventId INTEGER NOT NULL,
                                Name TEXT NOT NULL,
                                Price REAL NOT NULL CHECK(Price >= 0),
                                Capacity INTEGER NOT NULL CHECK(Capacity >= 0),

                                FOREIGN KEY (EventId) REFERENCES Events(EventId)
                                    ON DELETE CASCADE
);


CREATE TABLE IF NOT EXISTS Bookings (
                          BookingId INTEGER PRIMARY KEY AUTOINCREMENT,
                          UserId INTEGER NOT NULL,
                          EventId INTEGER NOT NULL,
                          OptionId INTEGER NOT NULL,
                          Price REAL NOT NULL,
                          Status TEXT DEFAULT 'Booked' CHECK(Status IN ('Booked', 'Cancelled', 'Completed')),
                          BookingDate TEXT DEFAULT CURRENT_TIMESTAMP,

                          FOREIGN KEY (UserId) REFERENCES Users(UserId)
                              ON DELETE CASCADE,

                          FOREIGN KEY (EventId) REFERENCES Events(EventId)
                              ON DELETE CASCADE,

                          FOREIGN KEY (OptionId) REFERENCES BookingOptions(OptionId)
                              ON DELETE CASCADE,

                          UNIQUE(UserId, EventId, OptionId) -- Prevent duplicate bookings for same option
);



CREATE TABLE IF NOT EXISTS Reviews (
                         ReviewId INTEGER PRIMARY KEY AUTOINCREMENT,
                         UserId INTEGER NOT NULL,
                         EventId INTEGER NOT NULL,
                         Rating INTEGER NOT NULL CHECK(Rating BETWEEN 1 AND 5),
                         Comment TEXT,
                         CreatedAt TEXT DEFAULT CURRENT_TIMESTAMP,

                         FOREIGN KEY (UserId) REFERENCES Users(UserId)
                             ON DELETE CASCADE,

                         FOREIGN KEY (EventId) REFERENCES Events(EventId)
                             ON DELETE CASCADE,

                         UNIQUE(UserId, EventId) -- One review per user per event
);



CREATE INDEX idx_events_datetime ON Events(DateTime);
CREATE INDEX idx_events_category ON Events(Category);
CREATE INDEX idx_events_type ON Events(Type);

CREATE INDEX idx_bookings_user ON Bookings(UserId);
CREATE INDEX idx_bookings_event ON Bookings(EventId);

CREATE INDEX idx_reviews_event ON Reviews(EventId);


