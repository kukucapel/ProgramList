USE product;
GO
INSERT Area (name_area)
VALUES 
	(N'���������'),
	(N'���������'),
	(N'������')
;
INSERT Lists (name_list, id_area, production_time)
VALUES 
	(N'�������', 1, 3),
	(N'��������� ����', 1 , 6),
	(N'��������� �������', 2, 3),
	(N'�������', 2, 12),
	(N'������� ����', 3, 7),
	(N'������� ����������', 3, 10),
	(N'����� ������ �������� ����������', 3, 17)
;

INSERT Kits (id_list, production_time)
VALUES 
	(4, '12-00'),
	(7, '17-00')
;
GO
INSERT Parts (id_kit, id_list, production_time)
VALUES
	(1, 1, '3-00'),
	(1, 2, '6-00'),
	(1, 3, '3-00'),
	(2, 5, '7-00'),
	(2, 6, '10-00')
;