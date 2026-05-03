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


-- Insert Sample Data
INSERT INTO Users (Name) VALUES ('Ahmed'), ('Ali');

INSERT INTO Tasks (Title, UserId, IsCompleted)
VALUES ('Task 1', 1, 0), ('Task 2', 2, 1);



--filter
SELECT * FROM Tasks WHERE IsCompleted = 1;


--join 

SELECT u.Name, t.Title
FROM Users u
JOIN Tasks t ON u.Id = t.UserId;


--sort

SELECT * FROM Tasks ORDER BY Id DESC;