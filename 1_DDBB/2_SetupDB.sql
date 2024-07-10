USE DB_CAPS
-- Create Tipo Empleado
INSERT INTO TipoEmpleado
VALUES
('ADMINISTRADOR'),('EMPLEADO')
-- Create Admin User
-- Jefe
DECLARE @EmployeeType INT
DECLARE @Turno VARCHAR(50)
DECLARE @PERSONAL VARCHAR(50)
INSERT INTO dbo.Empleado
(IdEmpleado, Nombre, ApPaterno, ApMaterno, Contrasena, EmployeeType)
VALUES
('ADM-567890','Fernando', 'Fernandez', 'Rios','12345', 1)
--,('EMP-567890', 'Angel A.', 'Galarza', 'Chavez'	,'12345',2)
SET @PERSONAL ='ADM-567890'
SET @Turno ='Completo'

INSERT INTO dbo.Empleado_Activo
(IdEmpleado, StartDate, EndDate, Turno)
VALUES(@PERSONAL, GETDATE(), null, @Turno)

-- Create Product Categories
INSERT INTO dbo.ProductCategory
(CategoryName)--MAX:100
VALUES('Completo')
GO
-- =================================
-- Triggers:

-- Update Product:Stock
-- Dispadado por: Inventario:Quantity
-- Razon: Ingreso de cierta cantidad de productos al Inventario
CREATE TRIGGER [TR_INVENTARY_QUANTITY]
ON [dbo].[Inventario]
AFTER INSERT
AS
BEGIN
	DECLARE @CANTIDAD INT
	DECLARE @SKU INT	--CHAR(20)
	SET @CANTIDAD = (Select Quantity from inserted)
	SET @SKU = (SELECT IdProduct FROM INSERTED)

	UPDATE [dbo].[Productos]
	SET Stock = Stock + @CANTIDAD
	WHERE IdProducto=@SKU--SKU=@SKU
END
GO

-- Update Product:Stock
-- Dispadado por: Product_Items:Quantity
-- Razon: Venta de cierta cantidad de productos.
CREATE TRIGGER [TR_REDUCESTOCK]
ON [dbo].[Product_Items]
AFTER INSERT
AS
BEGIN
	DECLARE @Product_SKU INT--CHAR(10)
	DECLARE @StockTotal INT
	DECLARE @StockRestar INT
	SET @Product_SKU = (Select/*Product_SKU*/ ProductID from inserted);
	SET @StockTotal = (SELECT Stock FROM Productos WHERE /*SKU*/ IdProducto=@Product_SKU);
	SET @StockRestar = (SELECT Quantity FROM inserted);
	
	UPDATE [dbo].[Productos]
	SET Stock =	@StockTotal - @StockRestar
	WHERE /*SKU*/IdProducto = @Product_SKU

END





