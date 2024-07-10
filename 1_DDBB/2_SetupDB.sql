-- =======================================================================
USE DB_CAPS
-- =======================================================================
-- Create Tipo Empleado
INSERT INTO TipoEmpleado
VALUES
('ADMINISTRADOR'),('EMPLEADO')--select * from TipoEmpleado-- =======================================================================
-- Create Admin User
-- Jefe
CREATE PROCEDURE InsertarEmpleado --@IdEmpleado CHAR(10),
    @Nombre NVARCHAR(100),  @ApPaterno NVARCHAR(100), @ApMaterno NVARCHAR(100), @Contrasena VARCHAR(30), @EmployeeType INT,
	@NuevoID CHAR(10) OUTPUT
AS
BEGIN	
    SET NOCOUNT ON;
	DECLARE @IdEmpleado CHAR(10)		DECLARE @CountEmployees INT;	DECLARE @EmpTypeNamePrefix VARCHAR(3);
    SELECT @CountEmployees = COUNT(*)+1 FROM dbo.Empleado WHERE EmployeeType = @EmployeeType;	--PRINT @CountEmployees
    SELECT TOP 1 @EmpTypeNamePrefix = LEFT(EmpTypeName, 3) FROM dbo.TipoEmpleado WHERE IdEmployeeType = @EmployeeType
	SET @IdEmpleado = CONCAT(@EmpTypeNamePrefix, '-', FORMAT(@CountEmployees, '000000')); --PRINT @elID    
	DECLARE @PasswordHash VARBINARY(MAX);		SET @PasswordHash = HASHBYTES('SHA2_256', @Contrasena);

    INSERT INTO dbo.Empleado
        (IdEmpleado, Nombre, ApPaterno, ApMaterno, Contrasena, EmployeeType)
    VALUES
        (@IdEmpleado, @Nombre, @ApPaterno, @ApMaterno, @PasswordHash, @EmployeeType);
	
	SET @NuevoID = @IdEmpleado
END;
GO
--DECLARE @ID CHAR(10);
--EXEC InsertarEmpleado --@IdEmpleado = 'ADM-567890',
--    @Nombre = 'Fernando', @ApPaterno = 'Fernandez', @ApMaterno = 'Rios', @Contrasena = '12345',
--    @EmployeeType = 1, @NuevoID = @ID OUTPUT
--SELECT @ID 
--,('EMP-567890', 'Angel A.', 'Galarza', 'Chavez'	,'12345',2)-- =======================================================================
CREATE PROCEDURE ActivarUsuario
    @IdEmpleado CHAR(10),
    @Turno VARCHAR(50)
AS
BEGIN
	INSERT INTO dbo.Empleado_Activo
	(IdEmpleado, StartDate, EndDate, Turno)
	VALUES(@IdEmpleado, GETDATE(), null, @Turno)
END

EXEC ActivarUsuario
    @IdEmpleado ='ADM-567890',
	@Turno ='Completo'

-- =======================================================================
-- Create Product Categories
INSERT INTO dbo.ProductCategory
(CategoryName)--MAX:100
VALUES('Completo')
GO
-- =======================================================================
-- Triggers:
-- =======================================================================
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
	WHERE IdProducto=@SKU	--SKU=@SKU
END
GO
-- -----------------------------------------------------------------------
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





