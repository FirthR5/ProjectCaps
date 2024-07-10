--CREATE DATABASE DB_CAPS
--GO
USE DB_CAPS
GO

CREATE TABLE TipoEmpleado (
	IdEmployeeType INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
	EmpTypeName VARCHAR(100) NOT NULL
);
GO

CREATE TABLE Empleado (
	IdEmpleado CHAR(10) PRIMARY KEY NOT NULL,
	Nombre VARCHAR(100) NOT NULL,
	ApPaterno VARCHAR(100) NOT NULL,
	ApMaterno VARCHAR(100) NOT NULL,
	Contrasena  VARBINARY(MAX),--VARCHAR(30) NOT NULL,
	EmployeeType INT NOT NULL,
	FOREIGN KEY(EmployeeType) REFERENCES TipoEmpleado(IdEmployeeType)
);
GO
-- TODO: Update password encriptada
--ALTER TABLE Empleado ADD Contrasena VARBINARY(MAX);

CREATE TABLE Empleado_Activo (
	Id INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
	IdEmpleado CHAR(10) NOT NULL,
	StartDate DATETIME NOT NULL,
	EndDate DATETIME NULL,
	Turno VARCHAR(30) NOT NULL,
	FOREIGN KEY(IdEmpleado) REFERENCES Empleado(IdEmpleado)
);
GO

CREATE TABLE ProductCategory (
	IdCategory INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
	CategoryName VARCHAR(100) NOT NULL
);
GO

CREATE TABLE Productos (
	IdProducto INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
	ProdName VARCHAR(100) NOT NULL,
	Stock INT NOT NULL,
	IdProdCategory INT NOT NULL,
	Descripcion VARCHAR(MAX) NULL,
	Activo BIT NOT NULL,
	FOREIGN KEY(IdProdCategory) REFERENCES ProductCategory(IdCategory)
);
GO
-- TODO: Update Stock not null but if it is put 0

CREATE TABLE ProductPrices (
	IdPrice INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
	UnitPrice MONEY NOT NULL,
	StartDate DATETIME NOT NULL,
	EndDate DATETIME NULL,
	ProductID INT NOT NULL,
	FOREIGN KEY(ProductID) REFERENCES Productos(IdProducto)
);
GO

CREATE TABLE OrderReceipt (
	OrderId INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
	IdEmpleado CHAR(10) NOT NULL,
	TotalPaid MONEY NOT NULL,
	OrderDate DATETIME NOT NULL,
	FOREIGN KEY(IdEmpleado) REFERENCES Empleado(IdEmpleado)
);
GO

CREATE TABLE Inventario (
	Id INT PRIMARY KEY IDENTITY(1,1) NOT  NULL,
	EntryDate DATETIME NOT NULL,
	Quantity INT NOT NULL,
	IdAdmin CHAR(10) NOT NULL,
	IdProduct INT NOT NULL,
	FOREIGN KEY(IdAdmin) REFERENCES Empleado(IdEmpleado),
	FOREIGN KEY(IdProduct) REFERENCES Productos(IdProducto)
);
GO

CREATE TABLE Product_Items (
	IdItem INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
	ProductPriceID INT NOT NULL,
	Quantity INT NOT NULL,
	TicketOrderID INT NOT NULL,
	ProductID INT NOT NULL,
	FOREIGN KEY(ProductPriceID) REFERENCES ProductPrices(IdPrice),
	FOREIGN KEY(TicketOrderID) REFERENCES OrderReceipt(OrderId),
	FOREIGN KEY(ProductID) REFERENCES Productos(IdProducto)
);

-- Add a TemporalTable for ItemsReceipt
-- IdItem, Quantity, TickerOrderId, ProductID
CREATE TABLE TempCarrito(
	IdItem INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
	ProductPriceID INT NOT NULL,
	Quantity INT NOT NULL,
	OrderUUID UNIQUEIDENTIFIER NOT NULL,
	ProductID INT NOT NULL,
)
--OrderUUID = DEFAULT NEWID()


--DROP TABLE IF EXISTS Inventario;
--GO
--DROP TABLE IF EXISTS OrderReceipt;
--GO
--DROP TABLE IF EXISTS Product_Items;
--GO
--DROP TABLE IF EXISTS ProductPrices;
--GO
--DROP TABLE IF EXISTS Productos;
--GO
--DROP TABLE IF EXISTS ProductCategory;
--GO
--DROP TABLE IF EXISTS Empleado;
--GO
--DROP TABLE IF EXISTS Empleado_Activo;
--GO
--DROP TABLE IF EXISTS TipoEmpleado;

--DROP DATABASE DB_CAPS


-- Fix: Char(10) on EmpleadoID, Contrasena char(30)