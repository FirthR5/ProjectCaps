-- =======================================================================
USE DB_CAPS
-- =======================================================================
-- (0) Create Tipo Empleado
INSERT INTO TipoEmpleado
VALUES
('ADMINISTRADOR'),('EMPLEADO')--select * from TipoEmpleado-- =======================================================================
-- (3) I. Create Admin User
-- By: Admin. Inserta aqui al: Jefe
CREATE PROCEDURE InsertarEmpleado --@IdEmpleado CHAR(10),
    @Nombre NVARCHAR(100),  @ApPaterno NVARCHAR(100), @ApMaterno NVARCHAR(100), @Contrasena VARCHAR(30), @EmployeeType INT,
	@NuevoID CHAR(10) OUTPUT
AS
BEGIN	
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
DECLARE @ID CHAR(10);
EXEC InsertarEmpleado --@IdEmpleado = 'ADM-567890',
    @Nombre = 'Fernando', @ApPaterno = 'Fernandez', @ApMaterno = 'Rios', @Contrasena = '12345',
    @EmployeeType = 1, @NuevoID = @ID OUTPUT
SELECT @ID -- =======================================================================
-- (3) II. Activar Usuario
-- By: Admin
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
    @IdEmpleado ='ADM-000001',
	@Turno ='Completo'

-- =======================================================================
-- (12) Create Product Categories
INSERT INTO dbo.ProductCategory
(CategoryName)--MAX:100
VALUES('Bebidas', 'Alimentos')
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

-- ==================================================================
-- Seccion traida del archivo 3
-- Views & SP

-- =======================================================================
-- Login
-- By: Admin and Employees
-- Check if match on empleado
-- Check if it's active

-- (1) I. Match Empleado
CREATE PROCEDURE VerificarCredenciales
    @IdEmpleado CHAR(10),
    @Contrasena VARCHAR(30), 
	@Correcto INT OUTPUT
AS
BEGIN
    SET @Correcto = (SELECT COUNT(1) AS Cantidad
    FROM dbo.Empleado
    WHERE IdEmpleado = @IdEmpleado
    AND Contrasena = HASHBYTES('SHA2_256', @Contrasena));
END;
-----------------------
-- (1) II. Checar si esta activo
CREATE PROCEDURE VerificarUsuarioActivo
    @IdEmpleado CHAR(10), @Activo INT OUT
AS
BEGIN
	SET @Activo = (SELECT COUNT(1) FROM Empleado_Activo
	WHERE IdEmpleado=@IdEmpleado AND
	EndDate IS NULL)
END
-- =======================================================================

-- (2) Ver Datos del usuario (El mismo usuario ve su informacion):
-- By: Todos los empleados, Admin
-- TODO: Probar View despues
CREATE VIEW vw_Datos_Usuario
AS
SELECT E.IdEmpleado, (Nombre + ' ' + ApPaterno + ' ' + ApMaterno) AS NombreCompleto,
TE.EmpTypeName
FROM dbo.Empleado E
INNER JOIN dbo.TipoEmpleado TE ON E.EmployeeType = TE.IdEmployeeType
-- (4) Ver lista de empleados
-- By: Admin
-- TODO: Include in EF
CREATE VIEW vw_ListaEmpleados
AS
SELECT E.IdEmpleado, (Nombre + ' ' + ApPaterno + ' ' + ApMaterno) AS NombreCompleto,
	TE.EmpTypeName
    ,IIF(EA.EndDate IS NULL, 'Activo', 'Inactivo') as Estado
	, EA.Turno
FROM dbo.Empleado E
LEFT JOIN dbo.TipoEmpleado TE ON E.EmployeeType = TE.IdEmployeeType
OUTER APPLY (
    SELECT TOP 1 EA.EndDate, EA.Turno 
    FROM dbo.Empleado_Activo EA 
    WHERE EA.IdEmpleado = E.IdEmpleado 
    ORDER BY EA.StartDate DESC
) EA;

-- (7) III. Insertar y Actualizar ProductPrices
-- By: Admin
CREATE PROCEDURE sp_InsertOrUpdateProductPrices
    @ProductID INT,
    @UnitPrice MONEY
AS
BEGIN
    BEGIN TRANSACTION;
    -- Verificar si existe un registro con ProductID y EndDate es NULL
    IF EXISTS (SELECT 1 FROM ProductPrices WHERE ProductID = @ProductID AND EndDate IS NULL)
    BEGIN
        -- Actualizar EndDate para indicar que el precio ya paso, osea ya esta en desuso
        UPDATE ProductPrices
        SET EndDate = GETDATE()
        WHERE ProductID = @ProductID AND EndDate IS NULL;
    END

    -- Actualizar el Precio del Producto
    INSERT INTO ProductPrices (ProductID, StartDate, EndDate, UnitPrice)
    VALUES (@ProductID, GETDATE(), NULL, @UnitPrice);

    COMMIT TRANSACTION;
END;







---
CREATE PROCEDURE sp_ListaDeTurnos
AS
BEGIN
    DECLARE @Turnos TABLE (
        Texto VARCHAR(50)
    );
	-- Turnos 
    INSERT INTO @Turnos (Texto)
    VALUES ('Dia'), ('Noche'), ('Completo');

    SELECT * FROM @Resultado;
END;
