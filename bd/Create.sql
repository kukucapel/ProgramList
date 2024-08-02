/*Скрипт создания основных таблиц базы данных*/
CREATE DATABASE product;
GO
USE product;
GO
CREATE TABLE Area(
	id INT PRIMARY KEY NOT NULL IDENTITY(1,1),
	name_area NVARCHAR(50),
	count_lists INT DEFAULT 0,
	count_complate_lists INT DEFAULT 0,
	in_production_now INT DEFAULT NULL
);
GO
CREATE TABLE Lists(
	id INT PRIMARY KEY NOT NULL IDENTITY(1,1),
	name_list NVARCHAR(50) NOT NULL,
	id_area INT REFERENCES Area(id) ON DELETE CASCADE,
	production_time INT NOT NULL
);
GO
CREATE TABLE Free(
	id INT PRIMARY KEY NOT NULL IDENTITY(1,1),
	id_list INT REFERENCES Lists(id),
	id_area INT REFERENCES Area(id)
);
GO
CREATE TABLE Orders(
	id INT PRIMARY KEY NOT NULL IDENTITY(1,1),
	date_start DATE,
	date_end DATE,
	status_order BIT DEFAULT 0
);
GO
CREATE TABLE OrderList(
	id INT PRIMARY KEY NOT NULL IDENTITY(1,1),
	id_order INT REFERENCES Orders(id) ON DELETE CASCADE,
	id_list INT REFERENCES Lists(id),
	status_list INT DEFAULT 0
);
GO
CREATE TABLE Complate(
	id INT PRIMARY KEY NOT NULL IDENTITY(1,1),
	id_order INT REFERENCES Orders(id),
	id_list INT REFERENCES Lists(id),
	id_area INT REFERENCES Area(id)
);
GO
CREATE TABLE Kits(
	id INT PRIMARY KEY NOT NULL IDENTITY(1,1),
	id_list INT REFERENCES Lists(id),
	production_time NVARCHAR(5) DEFAULT 0,
);
GO
CREATE TABLE Parts(
	id INT PRIMARY KEY NOT NULL IDENTITY(1,1),
	id_kit INT DEFAULT NULL REFERENCES Kits(id) ON DELETE SET DEFAULT,
	id_list INT REFERENCES Lists(id),
	production_time NVARCHAR(5)
);
GO
ALTER TABLE Area
ADD FOREIGN KEY(in_production_now) REFERENCES Orders(id) ON DELETE SET DEFAULT;