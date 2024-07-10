CREATE DATABASE DB_CAPS
GO
USE DB_CAPS
GO

CREATE TABLE TipoEmpleado (
	IdEmployeeType INT PRIMARY KEY NOT NULL,
	EmpTypeName VARCHAR(100) NOT NULL
);
GO

CREATE TABLE Empleado (
	IdEmpleado INT PRIMARY KEY NOT NULL,
	Nombre VARCHAR(100) NOT NULL,
	ApPaterno VARCHAR(100) NOT NULL,
	ApMaterno VARCHAR(100) NOT NULL,
	EmployeeType INT NOT NULL,
	FOREIGN KEY(EmployeeType) REFERENCES TipoEmpleado(IdEmployeeType)
);
GO
-- TODO: Update password
-- TODO: Update IdEmpleado por CHAR

CREATE TABLE Empleado_Activo (
	IdEmpActivo INT PRIMARY KEY NOT NULL,
	IdEmpleado INT NOT NULL,
	StartDate DATETIME NOT NULL,
	EndDate DATETIME NULL,
	Turno VARCHAR(30) NOT NULL,
	FOREIGN KEY(IdEmpleado) REFERENCES Empleado(IdEmpleado)
);
GO

CREATE TABLE ProductCategory (
	IdCategory INT PRIMARY KEY NOT NULL,
	CategoryName VARCHAR(100) NOT NULL
);
GO

CREATE TABLE Productos (
	IdProducto INT PRIMARY KEY NOT NULL,
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
	IdPrice INT PRIMARY KEY NOT NULL,
	UnitPrice INT NOT NULL,
	StartDate INT NOT NULL,
	EndDate INT NULL,
	ProductID INT NOT NULL,
	FOREIGN KEY(ProductID) REFERENCES Productos(IdProducto)
);
GO

CREATE TABLE OrderReceipt (
	OrderId INT PRIMARY KEY NOT NULL,
	IdEmpleado INT NOT NULL,
	TotalPaid MONEY NOT NULL,
	OrderDate DATETIME NOT NULL,
	FOREIGN KEY(IdEmpleado) REFERENCES Empleado(IdEmpleado)
);
GO

CREATE TABLE Inventario (
	Id INT PRIMARY KEY NOT NULL,
	EntryDate DATETIME NOT NULL,
	Quantity INT NOT NULL,
	IdAdmin INT NOT NULL,
	IdProduct INT NOT NULL,
	FOREIGN KEY(IdAdmin) REFERENCES Empleado(IdEmpleado),
	FOREIGN KEY(IdProduct) REFERENCES Productos(IdProducto)
);
GO

CREATE TABLE Product_Items (
	IdItem INT PRIMARY KEY NOT NULL,
	ProductPriceID INT NOT NULL,
	Quantity INT NOT NULL,
	TicketOrderID INT NOT NULL,
	ProductID INT NOT NULL,
	FOREIGN KEY(ProductPriceID) REFERENCES ProductPrices(IdPrice),
	FOREIGN KEY(TicketOrderID) REFERENCES OrderReceipt(OrderId),
	FOREIGN KEY(ProductID) REFERENCES Productos(IdProducto)
);

-- TODO: Add a TemporalTable for ItemsReceipt
-- IdItem, Quantity, TickerOrderId, ProductID

--DROP TABLE IF EXISTS Empleado;
--DROP TABLE IF EXISTS Empleado_Activo;
--DROP TABLE IF EXISTS TipoEmpleado;
--DROP TABLE IF EXISTS Productos;
--DROP TABLE IF EXISTS ProductCategory;
--DROP TABLE IF EXISTS ProductPrices;
--DROP TABLE IF EXISTS Product_Items;
--DROP TABLE IF EXISTS OrderReceipt;
--DROP TABLE IF EXISTS Inventario;