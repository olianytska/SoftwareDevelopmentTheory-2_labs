CREATE DATABASE Lab
	ON (NAME = Laba1_dat, FILENAME = 'C:\Users\Liza\Desktop\KPI\5sem\трпз\lab1\Laba1.mdf')
LOG 
	ON (NAME = Laba1_log, FILENAME = 'C:\Users\Liza\Desktop\KPI\5sem\трпз\lab1\Laba1.mdf.ldf');
GO

--EXEC sp_helpdb Lab


ALTER DATABASE Lab  
Modify Name = Laba1;  
GO

USE Laba1;
CREATE TABLE Author
(
	AuthorId int Identity NOT NULL, 
	Name nvarchar(50) NOT NULL,
	Patronymic nvarchar(50),
	Surname nvarchar(50) NOT NULL,  
	DateOfBirth date,
	PlaceOfBirth nvarchar(100),
	DateOfDeath date,
	CONSTRAINT PK_AuthorId PRIMARY KEY (AuthorId)
)
GO

CREATE TABLE Publishing
(
	PublishingId int Identity NOT NULL, 
	Name nvarchar(50) NOT NULL, 
	Place nvarchar(100) NOT NULL,
	CONSTRAINT PK_PublishingId PRIMARY KEY (PublishingId)
)
GO

CREATE TABLE Genre
(
	GenreId int Identity NOT NULL, 
	Name nvarchar(50) NOT NULL, 
	Desription nvarchar(250),
	CONSTRAINT PK_GenreId PRIMARY KEY (GenreId)
)
GO

CREATE TABLE Provider
(
	ProviderId int Identity NOT NULL, 
	Surname nvarchar(50) NOT NULL, 
	Name nvarchar(50) NOT NULL,
	Patronymic nvarchar(50), 
	AddressProvider nvarchar(250),
	TelNumb char(13) NOT NULL,
	CONSTRAINT PK_ProviderId PRIMARY KEY (ProviderId)
)
GO

CREATE TABLE Customer
(
	CustomerId int Identity NOT NULL, 
	Surname nvarchar(50) NOT NULL,
	Name nvarchar(50) NOT NULL,  
	Patronymic nvarchar(50), 
	AddressProvider nvarchar(150) NOT NULL,
	TelNumb char(13) NOT NULL,
	CONSTRAINT PK_CustomerId PRIMARY KEY (CustomerId)
)
GO

CREATE TABLE Book
(
	BookId int Identity NOT NULL, 
	PublishingId int NOT NULL, 
	AuthorId int NOT NULL, 
	GenreId int NOT NULL, 
	ProviderId int NOT NULL, 
	Name nvarchar(50) NOT NULL, 
	DateOfPubl date NOT NULL,
	Numb int NOT NULL,
	Price money NOT NULL,
	DateProv date NOT NULL,
	--TotalPrice AS Numb * Price,
	CONSTRAINT PK_BookId PRIMARY KEY (BookId),
	CONSTRAINT FK_BookId_Publishing FOREIGN KEY (PublishingId) REFERENCES Publishing (PublishingId)
		ON DELETE CASCADE
		ON UPDATE CASCADE,
	CONSTRAINT FK_BookId_Author FOREIGN KEY (AuthorId) REFERENCES Author (AuthorId)
	ON DELETE CASCADE
		ON UPDATE CASCADE,
	CONSTRAINT FK_BookId_Genre FOREIGN KEY (GenreId) REFERENCES Genre (GenreId)
		ON DELETE CASCADE
		ON UPDATE CASCADE,
	CONSTRAINT FK_BookId_Provider FOREIGN KEY (ProviderId) REFERENCES Provider (ProviderId)
		ON DELETE CASCADE
		ON UPDATE CASCADE
)
GO

CREATE TABLE Orders
(
	OrderId int Identity NOT NULL, 
	CustomerId int NOT NULL, 
	BookId int NOT NULL, 
	Numb int NOT NULL,
	DateOfOrder date NOT NULL, 
	CONSTRAINT PK_OrderId PRIMARY KEY (OrderId),
	CONSTRAINT FK_OrderId_Customer FOREIGN KEY (CustomerId) REFERENCES Customer (CustomerId)
		ON DELETE CASCADE
		ON UPDATE CASCADE,
	CONSTRAINT FK_OrderId_Book FOREIGN KEY (BookId) REFERENCES Book (BookId)
		ON DELETE CASCADE
		ON UPDATE CASCADE
)
GO

ALTER TABLE Book ADD TotalPrice AS Numb * Price; 

--Заполнение таблиц
INSERT INTO Publishing ([Name], [Place]) VALUES
(N'Клуб сімейного дозвілля', N'Київ'),
(N'Видавництво "Підручники і посібники"', N'Київ'),
(N'ТОВ "Видавництво Ранок"', N'Київ'), 
(N'ТОВ "Видавництво Генеза"​​', N'Чернігів'), 
(N'Видавничий дім "Пегас"', N'Київ'), 
(N'ПП "Кристал Бук"', N'Київ'), 
(N'Видавництво Vivat', N'Харків'),
(N'Видавництво "Грамота"', N'Львів'),
(N'Видавництво "Навчальна книга – Богдан"', N'Київ'),
(N'Видавництво Старого Лева', N'Львів')
GO

INSERT INTO Author ([Name], [Patronymic], [Surname], [DateOfBirth], [PlaceOfBirth], [DateOfDeath]) VALUES
(N'Тарас', N'Григорович', N'Шевченко', '18140409', N'с. Моринці, Київська губернія', '18610410'),
(N'Леся',NULL, N'Українка', '18710225', N'Київ', '19130801'),
(N'Лев', NULL, N'Толстой', NULL, N'Росія', NULL), 
(N'Вільям', NULL, N'Шекспір​​', NULL, NULL, NULL), 
(N'Джеймс', NULL, N'Джойс', NULL, 'England', NULL), 
(N'Федір', NULL, N'Достоєвський', NULL, 'Russia', NULL), 
(N'Чарльз', NULL, N'Діккенс', NULL, 'London', NULL),
(N'Антон', N'Павлович', N'Чехов', NULL, NULL, NULL),
(N'Гюстав', NULL, N'Флобер', NULL, 'France', NULL),
(N'Джейн', NULL, N'Остін', NULL, 'London', NULL)
GO

INSERT INTO Author ([Name], [Patronymic], [Surname], [DateOfBirth], [PlaceOfBirth], [DateOfDeath]) VALUES (N'Іван', NULL, N'Петренко', '19960705', N'Чернігів', NULL)
GO

INSERT INTO Genre ([Name]) VALUES
(N'ліричний вірш'),
(N'пісня'),
(N'елегія'),
(N'епіграма'),
(N'епітафія'),
(N'трагедія'),
(N'комедія'),
(N'драма'),
(N'водевіль'),
(N'фарс'),
(N'епопея'),
(N'казка'),
(N'байка'),
(N'легенда'),
(N'оповідання'),
(N'повість'),
(N'роман'),
(N'новела'),
(N'новелета'),
(N'художні мемуари')
GO

INSERT INTO Provider ([Surname], [Name], [Patronymic], [TelNumb]) VALUES
(N'Петренко', N'Максим', N'Леонідович','+380992540001' ),
(N'Новіков', N'Степан', N'Федорович','+380994546041'),
(N'Бобков', N'Віктор', N'Борисович','+380992535901'),
(N'Ванжула', N'Оксана', N'Петрівна','+380993541201'),
(N'Мацуки', N'Йоши', NULL,'+380662740070'),
(N'Петренко', N'Максим', N'Леонідович','+380952548004'),
(N'Петренко', N'Максим', N'Леонідович','+380962540605'),
(N'Коваленко', N'Тетяна', N'Іванівна','+380997540027'),
(N'Петрунько', N'Максим', N'Семенович','+380972542081'),
(N'Петрашин', N'Генадій', N'Борисович','+380692549021'),
(N'Минако', N'Азар',NULL,'+380502540301'),
(N'Варламов', N'Генадій', N'Леонідович','+380932545681')
GO

INSERT INTO Customer ([Surname], [Name], [Patronymic], [AddressProvider], [TelNumb]) VALUES 
(N'Бобков', N'Віктор', N'Борисович', N'Київ','+380992375001'),
(N'Петренко', N'Генадій', N'Ібрагимович', N'Тернопіль','+380992546701'),
(N'Варламов', N'Йоши', N'Федорович', N'Київ','+380992540001'),
(N'Коваленко', N'Генадій', N'Ібрагимович', N'Чернігів','+380937940001'),
(N'Мухамед', N'Ібрагім', NULL, N'Київ','+380992542453'),
(N'Варламов', N'Генадій', N'Ібрагимович', N'Москва','+380992548941'),
(N'Петренко', N'Віктор', N'Ібрагимович', N'Київ','+380992598251'),
(N'Варламов', N'Степан', N'Ібрагимович', N'Москва','+380992539851'),
(N'Новіков', N'Генадій', N'Ібрагимович', N'Москва','+380999837001'),
(N'Стасюк', N'Назар', N'Леонідович', N'Луцьк','+380993749501'),
(N'Сиденко', N'Катя', N'Леонідівна', N'Луцьк','+380993094601'),
(N'Сиденко', N'Катя', N'Леонідівна', N'Луцьк','+380992543081'),
(N'Мельниченко', N'Максим', N'Леонідович', N'Ялта','+380992538764'),
(N'Карнага', N'Ярослав', NULL, N'Ялта','+380992526831'),
(N'Карнага', N'Ярослава', N'Ігоревня', N'Львів','+380992544764'),
(N'Волокита', N'Артем', N'Миколайович', N'Львів','+380992544895')
GO

INSERT INTO Book([PublishingId], [AuthorId], [GenreId], [ProviderId], [Name], [DateOfPubl], [Numb], [Price], [DateProv]) VALUES
('5', '1', '1', '11', N'Кобзар ', '20000809', 255, 450.00, '20100323'),
('3', '1', '1', '10', N'Катерина', '20150829', 653, 472.29, '20090205'),
('1', '1', '1', '3', N'Гайдамаки (сборник)', '20100405',132, 482.09, '20200417'),
('3', '2', '1', '1', N'Віла-посестра', '20100809', 145, 394.00, '20200305'),
('8', '2', '1', '2', N'Все, все покинуть, до тебе полинуть...', '20070914', 134, 289.83, '20200323'),
('2', '2', '2', '1', N'Грішниця', '20050703', 235, 293.98, '20200323'),
('1', '2', '2', '3', N'Давня весна', '20100819', 456, 298.08, '20200323'),
('1', '3', '17', '1', N'Війна і мир', '20060509', 654, 729.99, '20200323'),
('3', '3', '17', '2', N'Анна Кареніна', '20040809', 356, 389.09, '20200323'),
('9', '4', '3', '7', N'Антоній і Клеопатра', '20010717', 356, 289.376, '20200323'),
('10', '4', '1', '7', N'Буря', '20000205', 643, 298.088, '20200323'),
('7', '4', '5', '5', N'Венера і Адоніс', '20010729', 134, 268.39, '20200323'),
('6', '4', '9', '4', N'Венеціанський купець', '20020304', 564, 178.73, '20200323'),
('6', '4', '1', '9', N'Гамлет', '20030818', 245, 357.86, '20200323'),
('3', '5', '1', '9', N'Камерна музика', '20050819', 245, 467.287, '20200323'),
('5', '6', '17', '4', N'Брати Карамазови', '20000809', 453, 478.276, '20200323'),
('1', '6', '17', '1', N'Злочин і кара', '20170819', 122, 478.38, '20200323'),
('1', '7', '16', '3', N'Посмертні записки Піквікського клубу', '20131207', 345, 238.22, '20200323'),
('3', '7', '16', '2', N'Пригоди Олівера Твіста', '20000819', 634, 389.33, '20200323'),
('1', '8', '8', '11', N'Вишневый сад Комедия в 4-х действиях', '20030809', 124, 378.22, '20200323'),
('2', '9', '17', '10', N'Пані Боварі', '20050829', 564, 345.34, '20200323'),
('2', '10', '17', '11', N'Гордість і упередження', '20120811', 245, 299.34, '20200323'),
('3', '10', '17', '10', N'Менсфілд парк', '20000309', 13, 862.298, '20200323'),
('8', '10', '17', '1', N'Емма ', '20070707', 208, 388.39, '20200323')
GO

INSERT INTO Orders([CustomerId],[BookId], [Numb], [DateOfOrder]) VALUES
('1', '1', 1, '20200703'),
('3', '2', 2, '20201112'),
('4', '10', 1, '20200423'),
('10', '5', 1, '20200809'),
('10', '11', 1, '20200903'),
('1', '11', 1, '20200902'),
('7', '10', 1, '20200304'),
('6', '8', 2, '20201003'),
('12', '1', 1, '20201017'),
('3', '6', 1, '20201012'),
('5', '7', 3, '20200923'),
('1', '2', 1, '20200914'),
('2', '8', 1, '20200929'),
('4', '9', 1, '20200930'),
('4', '4', 2, '20201001'),
('8', '4', 1, '20201005'),
('1', '2', 1, '20201115'),
('9', '1', 1, '20201201')
GO

INSERT INTO Orders([CustomerId],[BookId], [Numb], [DateOfOrder]) VALUES
('1', '3', 2, '20120703'),
('3', '5', 1, '20130513'),
('1', '7', 3, '20140719')
GO
