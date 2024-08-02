USE product;
GO
CREATE PROCEDURE ShowOrders AS
SELECT id AS Number, date_start AS 'Date start', date_end AS 'Date end',
	CASE status_order
		WHEN 0 THEN 'In production'
		WHEN 1 THEN 'Finish'
		ELSE 'Cancelled'
	END AS 'Status'
FROM Orders;
GO

CREATE PROCEDURE ShowLists AS
SELECT id AS 'Number', name_list AS 'Name', (
	SELECT name_area FROM Area
	WHERE Lists.id_area = Area.id
	) AS 'Name area'
FROM Lists;
GO

CREATE PROCEDURE ShowKits AS
SELECT 
	id AS 'Number', 
	(
		SELECT name_list FROM Lists
		WHERE Kits.id_list = Lists.id
	) AS 'Name', 
	production_time AS 'Production time (hour)'
FROM Kits;
GO

CREATE PROCEDURE ShowParts AS
SELECT 
	id AS 'Number',
	(
		SELECT name_list 
		FROM Lists 
		WHERE Parts.id_list = Lists.id
	) AS 'Name',
	CASE id_kit
		WHEN NULL THEN 'No kit'
	ELSE
	(
		SELECT 
		(
			SELECT name_list 
			FROM Lists
			WHERE Kits.id_list = Lists.id
		)
		FROM Kits
		WHERE Parts.id_kit = Kits.id
	) END AS 'Name kit',
	production_time AS 'Production time (hour)'
FROM Parts;
GO



CREATE PROCEDURE ShowArea AS
SELECT 
	id AS Number, 
	name_area AS 'Name', 
	(
		SELECT COUNT(*)
		FROM Lists
		WHERE Lists.id_area = Area.id
	) AS 'Count lists',
	count_complate_lists AS 'Count complate lists', 
	CASE 
		WHEN in_production_now IS NOT NULL THEN in_production_now
		ELSE 0
	END AS 'In progress order'
FROM Area;
GO

CREATE PROCEDURE ReturnTimeOrderListProduction
	@id INT
AS
	SELECT
(
	SELECT production_time
	FROM Lists
	WHERE Lists.id = OrderList.id_list
)
FROM OrderList
WHERE id_order = @id;
GO

CREATE PROCEDURE ReturnTimeOrder
	@id INT
AS
	SELECT DATEDIFF(HOUR, date_start, date_end)
	FROM Orders
	WHERE id = @id;
GO

CREATE PROCEDURE ShowOrderListById
	@id INT
AS
SELECT 
	(
		SELECT name_list
		FROM Lists
		WHERE OrderList.id_list = Lists.id
	) AS 'Name',
	CASE status_list
		WHEN 0 THEN 'Nothing'
		WHEN 1 THEN 'In production'
		WHEN 2 THEN 'Finish'
		ELSE 'Cancelled'
	END AS 'Status'
FROM OrderList
WHERE OrderList.id_order = @id;
GO

CREATE PROCEDURE ShowFree AS
SELECT 
	(
		SELECT name_list
		FROM Lists
		WHERE Lists.id = Free.id_list
	) AS 'Name',
	(
		SELECT name_area
		FROM Area
		WHERE Area.id = Free.id_area
	) AS 'Name area'
FROM Free
ORDER BY 2;
GO

CREATE PROCEDURE WorkSteep1 AS
	SELECT id FROM Orders
	WHERE status_order = 0
	ORDER BY date_start;
GO 
CREATE PROCEDURE WorkSteep2 
	@id INT AS
	SELECT id_list FROM OrderList
	WHERE id_order = @id;
GO 
CREATE PROCEDURE WorkSteep3 AS
	SELECT id_list FROM Free;
GO 
CREATE PROCEDURE WorkSteep4 
	@id INT AS
	SELECT id_area FROM Lists
	WHERE id = @id;
GO
CREATE PROCEDURE WorkSteep5
	@id INT AS
	SELECT in_production_now FROM Area
	WHERE id = @id;
GO

CREATE PROCEDURE WorkSteep6
	@id_order INT,
	@id_area INT,
	@id_list INT
	AS 
	UPDATE Area
	SET in_production_now = @id_order
	WHERE id = @id_area
	UPDATE TOP(1) OrderList
	SET status_list = 1
	WHERE id_list = @id_list AND status_list = 0 AND id_order = @id_order;
GO

CREATE PROCEDURE CheckSteep1 AS
	SELECT id, status_order FROM Orders;
GO

CREATE PROCEDURE CheckSteep2 
	@idOrder INT AS
	SELECT status_list FROM OrderList
	WHERE id_order = @idOrder;
GO

CREATE PROCEDURE ReadyListSteep1 
	@id_area INT
	AS
	SELECT in_production_now FROM Area 
	WHERE id = @id_area;
GO 
CREATE PROCEDURE ReadyListSteep2 
	@id_order INT AS
	SELECT id_list, status_list 
	FROM OrderList 
	WHERE id_order = @id_order;
GO
CREATE PROCEDURE ReadyListSteep3 
	@id_area INT AS
	SELECT id FROM Lists WHERE id_area = @id_area;
GO 
CREATE PROCEDURE ReadyListSteep4
	@id_order INT,
	@id_list INT,
	@id_area INT
	AS
	INSERT Complate(id_order, id_list, id_area)
	VALUES (@id_order, @id_list, @id_area);
GO 
CREATE PROCEDURE ReadyListSteep5
	@id_list INT,
	@id_order INT,
	@id_area INT
	AS
	UPDATE TOP(1) OrderList
	SET status_list = 2
	WHERE id_list = @id_list AND status_list = 1 AND id_order = @id_order
	UPDATE Area
	SET in_production_now = NULL, count_complate_lists = (SELECT COUNT(*) FROM Complate WHERE id_area=@id_area) WHERE id=@id_area;
GO

CREATE PROCEDURE CancelOrder
	@id_order INT
	AS 
	INSERT INTO Free(Freee.id_list)
	SELECT Complate.id_list FROM Complate WHERE Complate.id_order = @id_order
	UPDATE Orders
	SET status_order = NULL
	WHERE id = @id_order
	UPDATE Area
	SET in_production_now = NULL
	WHERE in_production_now = @id_order 
	
	UPDATE Free
	SET id_area = (SELECT id_area FROM Lists WHERE Free.id_list = Lists.id);
GO

CREATE PROCEDURE CheckCount AS
	UPDATE Area
	SET count_complate_lists = (SELECT COUNT(*) FROM Complate WHERE Complate.id_area = Area.id);
GO
