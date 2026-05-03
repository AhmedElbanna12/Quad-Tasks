CREATE DATABASE TaskDB;


CREATE TABLE Users (
    Id INT PRIMARY KEY IDENTITY,
    Name NVARCHAR(100)
);

CREATE TABLE Tasks (
    Id INT PRIMARY KEY IDENTITY,
    Title NVARCHAR(200),
    UserId INT,
    IsCompleted BIT,
    FOREIGN KEY (UserId) REFERENCES Users(Id)
);

--filter
SELECT * FROM Tasks WHERE IsCompleted = 1;


--join 

SELECT u.Name, t.Title
FROM Users u
JOIN Tasks t ON u.Id = t.UserId;


--sort

SELECT * FROM Tasks ORDER BY Id DESC;