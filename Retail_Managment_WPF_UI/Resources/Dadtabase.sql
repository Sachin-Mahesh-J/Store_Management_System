CREATE DATABASE RetailManagementDB;
USE RetailManagementDB;

-- Table for product categories
CREATE TABLE Categories (
    CategoryID INT AUTO_INCREMENT PRIMARY KEY,
    CategoryName VARCHAR(100) NOT NULL
);

-- Table for suppliers
CREATE TABLE Suppliers (
    SupplierID INT AUTO_INCREMENT PRIMARY KEY,
    SupplierName VARCHAR(150) NOT NULL,
    ContactPerson VARCHAR(100),
    PhoneNumber VARCHAR(20),
    Email VARCHAR(100),
    Address TEXT
);

-- Table for products
CREATE TABLE Products (
    ProductID INT AUTO_INCREMENT PRIMARY KEY,
    Name VARCHAR(150) NOT NULL,
    CategoryID INT,
    SupplierID INT,
    Price DECIMAL(10,2) NOT NULL,
    StockQuantity INT NOT NULL DEFAULT 0,
    SKU VARCHAR(50) UNIQUE NOT NULL,
    CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (CategoryID) REFERENCES Categories(CategoryID) ON DELETE SET NULL,
    FOREIGN KEY (SupplierID) REFERENCES Suppliers(SupplierID) ON DELETE SET NULL
);

-- Table for customers
CREATE TABLE Customers (
    CustomerID INT AUTO_INCREMENT PRIMARY KEY,
    FullName VARCHAR(150) NOT NULL,
    PhoneNumber VARCHAR(20) UNIQUE,
    Email VARCHAR(100) UNIQUE,
    LoyaltyPoints INT DEFAULT 0
);

-- Table for user management (employees/admins)
CREATE TABLE Employees (
    UserID INT AUTO_INCREMENT PRIMARY KEY,
    Username VARCHAR(50) UNIQUE NOT NULL,
    PasswordHash VARCHAR(255) NOT NULL,
    Email VARCHAR(100) UNIQUE NOT NULL,
    PhoneNumber VARCHAR(20) UNIQUE NOT NULL,
    Role ENUM('Admin', 'Cashier', 'Manager') NOT NULL
);

-- Table for sales transactions
CREATE TABLE Sales (
    SaleID INT AUTO_INCREMENT PRIMARY KEY,
    CustomerID INT NULL,
    TotalAmount DECIMAL(10,2) NOT NULL,
    SaleDate TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    PaymentMethod ENUM('Cash', 'Card', 'Online') NOT NULL,
    FOREIGN KEY (CustomerID) REFERENCES Customers(CustomerID) ON DELETE SET NULL
);

-- Table for sale details (each product sold in a transaction)
CREATE TABLE SaleDetails (
    SaleDetailID INT AUTO_INCREMENT PRIMARY KEY,
    SaleID INT,
    ProductID INT,
    Quantity INT NOT NULL,
    UnitPrice DECIMAL(10,2) NOT NULL,
    FOREIGN KEY (SaleID) REFERENCES Sales(SaleID) ON DELETE CASCADE,
    FOREIGN KEY (ProductID) REFERENCES Products(ProductID) ON DELETE CASCADE
);

-- Table for purchase orders (restocking inventory)
CREATE TABLE PurchaseOrders (
    PurchaseID INT AUTO_INCREMENT PRIMARY KEY,
    SupplierID INT,
    OrderDate TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    TotalAmount DECIMAL(10,2),
    Status ENUM('Pending', 'Completed') DEFAULT 'Pending',
    FOREIGN KEY (SupplierID) REFERENCES Suppliers(SupplierID) ON DELETE SET NULL
);

-- Table for purchased products
CREATE TABLE PurchaseDetails (
    PurchaseDetailID INT AUTO_INCREMENT PRIMARY KEY,
    PurchaseID INT,
    ProductID INT,
    Quantity INT NOT NULL,
    UnitCost DECIMAL(10,2) NOT NULL,
    FOREIGN KEY (PurchaseID) REFERENCES PurchaseOrders(PurchaseID) ON DELETE CASCADE,
    FOREIGN KEY (ProductID) REFERENCES Products(ProductID) ON DELETE CASCADE
);

-- Table for reports (view-based)
CREATE VIEW SalesReport AS
SELECT 
    s.SaleID, s.SaleDate, c.FullName AS Customer, s.TotalAmount, s.PaymentMethod
FROM Sales s
LEFT JOIN Customers c ON s.CustomerID = c.CustomerID;