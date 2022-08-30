CREATE TABLE Employees (
    Id INT IDENTITY(1, 1) PRIMARY KEY,
    FullName VARCHAR(30),
    Email VARCHAR(40) UNIQUE,
    BirthDate DATE
)