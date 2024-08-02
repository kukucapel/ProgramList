UPDATE Orders
SET status_order = 0;
DELETE Free;
DELETE Complate;
UPDATE OrderList
SET status_list = 0;
UPDATE Area
SET in_production_now = NULL, count_complate_lists = 0;
DBCC CHECKIDENT(Complate, RESEED,0);
DBCC CHECKIDENT(Free, RESEED, 0);