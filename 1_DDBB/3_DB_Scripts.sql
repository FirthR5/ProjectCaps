-- Actions --
USE DB_CAPS

-- TODO: Performance, Agregar la paginacion de datos






-- =======================================================================
-- Login
-- By: Admin and Employees
-- Check if match on empleado
-- Check if it's active

-- 1. Match Empleado
CREATE PROCEDURE VerificarCredenciales
    @IdEmpleado CHAR(10),
    @Contrasena VARCHAR(30)
AS
BEGIN
    SELECT COUNT(1) AS Cantidad
    FROM dbo.Empleado
    WHERE IdEmpleado = @IdEmpleado
    AND Contrasena = HASHBYTES('SHA2_256', @Contrasena);
END;
EXEC VerificarCredenciales 
	@IdEmpleado ='ADM-567890',
	@Contrasena = '12345';

-- 2. Checar si esta activo
CREATE PROCEDURE VerificarUsuarioActivo
    @IdEmpleado CHAR(10)
AS
BEGIN
	SELECT COUNT(1) FROM Empleado_Activo
	WHERE IdEmpleado=@IdEmpleado AND
	EndDate IS NULL
END
EXEC VerificarUsuarioActivo @IdEmpleado ='ADM-567890'

--SELECT * FROM dbo.Empleado;SELECT * FROM dbo.Empleado_Activo
-- =======================================================================

-- Ver Datos del usuario (El mismo usuario ve su informacion):
-- By: Todos los empleados, Admin
-- TODO: Probar View despues
CREATE VIEW vw_Datos_Usuario
AS
SELECT E.IdEmpleado, (Nombre + ' ' + ApPaterno + ' ' + ApMaterno) AS NombreCompleto,
TE.EmpTypeName
FROM dbo.Empleado E
INNER JOIN dbo.TipoEmpleado TE ON E.EmployeeType = TE.IdEmployeeType

-- =======================================================================

-- Create Empleado
-- By: Admin
DECLARE @ID CHAR(10);
EXEC InsertarEmpleado --@IdEmpleado = 'ADM-567890',
    @Nombre = 'Angel A.', @ApPaterno = 'Galarza', @ApMaterno = 'Chavez', @Contrasena = '12345',
    @EmployeeType = 2, @NuevoID = @ID OUTPUT
SELECT @ID 


-- Ver lista de empleados
-- By: Admin
-- TODO: Probar View despues
SELECT E.IdEmpleado, (Nombre + ' ' + ApPaterno + ' ' + ApMaterno) AS NombreCompleto,
TE.EmpTypeName,
    EA.EndDate,
    EA.Turno
FROM dbo.Empleado E
INNER JOIN dbo.TipoEmpleado TE ON E.EmployeeType = TE.IdEmployeeType
OUTER APPLY (
    SELECT TOP 1 EA.EndDate, EA.Turno 
    FROM dbo.Empleado_Activo EA 
    WHERE EA.IdEmpleado = E.IdEmpleado 
    ORDER BY EA.StartDate DESC
) EA;
--SELECT * FROM dbo.Empleado

-- Ver lista de empleados activos
-- By: Admin
-- TODO: Probar View despues
SELECT E.IdEmpleado, (Nombre + ' ' + ApPaterno + ' ' + ApMaterno) AS NombreCompleto,
TE.EmpTypeName
FROM dbo.Empleado E
INNER JOIN dbo.TipoEmpleado TE ON E.EmployeeType = TE.IdEmployeeType
WHERE IdEmpleado IN (SELECT IdEmpleado FROM Empleado_Activo WHERE EndDate IS NULL)

-- SELECT * FROM Empleado_Activo

-- Desactivar empleado Activo
-- By: Admin
-- Solo ponerle la fecha EndDate de hoy
-- TODO: Pensar en Agregar el Activar un empleado?
DECLARE @IdEmpleado CHAR(10);
SET @IdEmpleado = 'ADM-567890';

UPDATE Empleado_Activo 
SET EndDate= GETDATE()
WHERE idEmpleado = @IdEmpleado


-- =======================================================================


-- Registrar Productos
-- By: Admin
-- Requiere:
-- Registrar el ProductPrices TBL
-- Escoger ProductCategory TBL
DECLARE @IdProdCategory INT;
DECLARE @Activo BIT;

SET @IdProdCategory = 50;
SET @Activo = 1;

-- Check if product exists en Productos
SELECT COUNT(1) FROM Productos WHERE IdProducto = 1

-- Registro el Producto si no existe
Insert Into Productos(ProductName, Stock, IdProdCategory, Descripcion, Activo)
VALUES ('name', 0, @IdProdCategory, 'Descripcion Lol', @Activo)


-- Insertar y Actualizar ProductPrices
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





-- =======================================================================
-- Ingreso de Inventario
DECLARE @IdEmpleado CHAR(10);
DECLARE @IdProduct INT;
DECLARE @Quantity INT;

SET @IdEmpleado = 'ADM-567890';
INSERT INTO Inventario (IdProduct, EntryDate, Quantity, IdAdmin)
VALUES (@IdProduct, GETDATE(), @Quantity, @IdEmpleado);



-- =======================================================================

-- Desactivar productos en venta
-- Productos: Activo=0
-- By: Admin
DECLARE @IdProducto INT;
SET @IdProducto = 1;

UPDATE Productos
SET Activo = 0
WHERE IdProducto = @IdProducto


-- Ver todo los productos Activos
-- Con Stock > 0
-- By: Empleado
-- Extra: Mostrar categoria
-- TODO: Probar View despues
SELECT * FROM Productos
WHERE Stock > 0 AND Activo = 1


-- Venta de productos
--------------------------------

-- Agregar/Guardar productos al Carrito
-- (Add a TemporalTable for ItemsReceipt)
-- By: Empleado
-- IdItem, Quantity, TickerOrderId, ProductID
DECLARE @OrderUUID UNIQUEIDENTIFIER
SET @OrderUUID = NEWID();

INSERT INTO TempCarrito (ProductPriceID, Quantity, OrderUUID, ProductID)
VALUES (101, 2, @OrderUUID, 5001);


-- Operacion Delete
-- Eliminar todo los productos del carrito despues de realizar la compra
-- By: Empleado
DELETE TempCarrito WHERE OrderUUID = NEWID() -- TODO: Aqui poner el UUID 
---

--Lista del carrito para agregarlo al ProductItems
-- By: Empleado
DECLARE @OrderUUID UNIQUEIDENTIFIER
SET @OrderUUID = 'UUID a Consultar';

SELECT * FROM TempCarrito WHERE OrderUUID = @OrderUUID;

-- Select the last price of ProductPrices
-- TODO:


-- Transaccion de venta de productos
-- By: Empleado
-- Requiere agregar:
-- OrderReceipt TBL & Product_Items TBL
-- Datos requeridos:
-- TBLs: ProductPrices, Productos, Empleado
DECLARE @IdEmpleado CHAR(10);
DECLARE @IdProduct INT;
DECLARE @TotalPaid MONEY;
SET @IdEmpleado = 'ADM-567890';
SET @TotalPaid = 999; -- Sumar los precios de la tabla anterior temporal

Insert Into OrderReceipt(IdEmpleado, TotalPaid, OrderDate)
VALUES (@IdEmpleado, @TotalPaid, GETDATE())

DECLARE @OrderId INT;
SET @OrderId = 1; -- Obtener el OrderID del Receipt
SET @IdProduct = 1;

-- Insertar datos de TempCarrito
Insert Into Product_Items(ProductID,ProductPriceID, Quantity, TickerOrderID)
VALUES (@IdProduct,1,2,@OrderId)



-- =======================================================================
-- Ver ordenes que ha realizado
-- By: Empleado (el mismo)
-- Requiere:
-- TBL Order Receipt
DECLARE @IdEmpleado CHAR(10);

SET @IdEmpleado = 'ADM-567890';
SELECT * FROM OrderReceipt
WHERE IdEmpleado = @IdEmpleado

-- Ver Items de las ordenes que realizo
-- By: Empleado (el mismo)
-- Requiere: (Order Receipt ID)
-- TBL Product_Items
DECLARE @OrderId INT;
SET @OrderId = 1;

SELECT * FROM Product_Items
WHERE OrderId = @OrderId


-- =======================================================================


-- Extras
-- Drop-Down
-- Ver Tipo Empleado
SELECT * FROM TipoEmpleado-- Ver Product CategorySELECT * FROM ProductCategory-- Ver TurnosCREATE PROCEDURE sp_ListaDeTurnos
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
