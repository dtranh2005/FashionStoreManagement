USE master;
GO




CREATE DATABASE FashionStoreDB;
GO

USE FashionStoreDB;
GO


CREATE TABLE Users (
    UserID INT IDENTITY(1,1) PRIMARY KEY,
    Username NVARCHAR(50) NOT NULL UNIQUE,
    Password NVARCHAR(100) NOT NULL,
    Role NVARCHAR(20) NOT NULL
);


CREATE TABLE Customers (
    CustomerID INT PRIMARY KEY,
    FullName NVARCHAR(100) NOT NULL,
    Phone NVARCHAR(15),
    Email NVARCHAR(100),
    Address NVARCHAR(200)
);


CREATE TABLE Products (
    ProductID INT PRIMARY KEY,
    ProductName NVARCHAR(100) NOT NULL,
    Category NVARCHAR(50),
    Price DECIMAL(18,2) NOT NULL,
    Quantity INT NOT NULL,
    Description NVARCHAR(255),
    Status NVARCHAR(20) NOT NULL DEFAULT 'Active' -- Cần cho logic Soft Delete
);

CREATE TABLE Orders (
    OrderID INT IDENTITY(1,1) PRIMARY KEY,
    CustomerID INT NOT NULL,
    OrderDate DATETIME DEFAULT GETDATE(),
    TotalAmount DECIMAL(18,2),
    FOREIGN KEY (CustomerID) REFERENCES Customers(CustomerID)
);


CREATE TABLE OrderDetails (
    OrderDetailID INT IDENTITY(1,1) PRIMARY KEY,
    OrderID INT NOT NULL,
    ProductID INT NOT NULL,
    Quantity INT NOT NULL,
    UnitPrice DECIMAL(18,2) NOT NULL,
    FOREIGN KEY (OrderID) REFERENCES Orders(OrderID),
    FOREIGN KEY (ProductID) REFERENCES Products(ProductID)
);
GO

--Insert data

INSERT INTO Users (Username, Password, Role)
VALUES 
('admin', '123456', 'Admin'),
('manager', '123456', 'Store Manager'),
('staff02', '123456', 'Staff');


INSERT INTO Customers (CustomerID, FullName, Phone, Email, Address)
VALUES 
(1, 'John Smith', '0901111222', 'john.smith@gmail.com', 'Hanoi'),
(2, 'Emma Johnson', '0903333444', 'emma.johnson@gmail.com', 'Da Nang'),
(3, 'Michael Brown', '0905555666', 'michael.brown@gmail.com', 'Ho Chi Minh City'),
(4, 'Olivia Davis', '0907777888', 'olivia.davis@gmail.com', 'Hue'),
(5, 'William Wilson', '0909999000', 'william.wilson@gmail.com', 'Can Tho'),
(6, 'Sophia Lee', '0912345678', 'sophia.lee@example.com', 'Hanoi'),
(7, 'James Nguyen', '0987654321', 'james.nguyen@example.com', 'Ho Chi Minh City'),
(8, 'Ava Tran', '0911223344', 'ava.tran@example.com', 'Hai Phong'),
(9, 'Mason Pham', '0977889900', 'mason.pham@example.com', 'Da Nang'),
(10, 'Isabella Ho', '0933445566', 'isabella.ho@example.com', 'Can Tho');
GO


INSERT INTO Products (ProductID, ProductName, Category, Price, Quantity, Description, Status)
VALUES
(1, 'White Shirt', 'Shirts', 25.00, 50, 'Cotton white shirt for men', 'Active'),
(2, 'Blue Jeans', 'Pants', 45.00, 40, 'Straight-fit blue jeans', 'Active'),
(3, 'Floral Dress', 'Dresses', 55.00, 30, 'Floral printed dress for women', 'Active'),
(4, 'Black T-shirt', 'Shirts', 18.00, 60, 'Basic black cotton T-shirt', 'Active'),
(5, 'Bomber Jacket', 'Jackets', 65.00, 25, 'Nylon bomber jacket for men', 'Active'),
(6, 'Leather Shoes', 'Shoes', 85.00, 20, 'Genuine leather shoes', 'Active'),
(7, 'Denim Jacket', 'Jackets', 70.00, 18, 'Classic denim jacket', 'Active'),
(8, 'Chino Pants', 'Pants', 38.00, 35, 'Slim-fit beige chinos', 'Active'),
(9, 'Sneakers', 'Shoes', 60.00, 50, 'White casual sneakers', 'Active'),
(10, 'Summer Skirt', 'Dresses', 40.00, 22, 'Light cotton summer skirt', 'Active'),
(11, 'V-Neck Sweater', 'Sweaters', 48.00, 30, 'Wool V-neck sweater', 'Active'),
(12, 'Cargo Shorts', 'Shorts', 30.00, 45, 'Cotton cargo shorts', 'Active'),
(13, 'Polo Shirt', 'Shirts', 35.00, 50, 'Classic fit polo shirt', 'Active'),
(14, 'Leather Belt', 'Accessories', 22.00, 60, 'Genuine leather belt', 'Active'),
(15, 'Silk Scarf', 'Accessories', 28.00, 25, 'Printed silk scarf', 'Active'),
(16, 'Trench Coat', 'Jackets', 120.00, 15, 'Water-resistant trench coat', 'Active'),
(17, 'Running Tights', 'Pants', 42.00, 30, 'Elastic running tights', 'Active'),
(18, 'Hoodie', 'Sweaters', 50.00, 40, 'Fleece pullover hoodie', 'Active'),
(19, 'Sun Hat', 'Accessories', 19.00, 50, 'Wide brim sun hat', 'Active'),
(20, 'Formal Trousers', 'Pants', 55.00, 20, 'Tailored formal trousers', 'Active'),
(21, 'Linen Shirt', 'Shirts', 40.00, 33, 'Breathable linen shirt', 'Active'),
(22, 'Wool Peacoat', 'Jackets', 150.00, 10, 'Double-breasted wool peacoat', 'Active'),
(23, 'Ankle Boots', 'Shoes', 75.00, 25, 'Suede ankle boots', 'Active'),
(24, 'Graphic Tee', 'Shirts', 22.00, 50, 'Cotton graphic tee', 'Active'),
(25, 'Maxi Dress', 'Dresses', 80.00, 15, 'Long maxi dress', 'Active'),
(26, 'Old Hat', 'Accessories', 15.00, 0, 'No longer sold', 'Archived'); -- Đồ đã "Xóa"
GO


INSERT INTO Orders (CustomerID, OrderDate, TotalAmount)
VALUES
(1, '2025-11-01', 70.00),
(2, '2025-11-02', 95.00),
(3, '2025-11-03', 130.00),
(4, '2025-11-03', 45.00),
(5, '2025-11-04', 175.00),
(6, '2025-11-05', 77.00), -- Order mới
(7, '2025-11-05', 120.00); -- Order mới
GO


INSERT INTO OrderDetails (OrderID, ProductID, Quantity, UnitPrice)
VALUES
-- Order 1 (John Smith)
(1, 1, 2, 25.00),
(1, 4, 1, 20.00),
-- Order 2 (Emma Johnson)
(2, 3, 1, 55.00),
(2, 9, 1, 40.00),
-- Order 3 (Michael Brown)
(3, 2, 2, 45.00),
(3, 5, 1, 40.00),
-- Order 4 (Olivia Davis)
(4, 4, 2, 18.00),
(4, 10, 1, 9.00),
-- Order 5 (William Wilson)
(5, 6, 1, 85.00),
(5, 9, 1, 60.00),
(5, 7, 1, 30.00),
-- Order 6 (Sophia Lee)
(6, 12, 1, 30.00),
(6, 14, 1, 22.00),
(6, 15, 1, 25.00),
-- Order 7 (James Nguyen)
(7, 16, 1, 120.00);
GO