USE [OrdersManagement]
GO
SET IDENTITY_INSERT [dbo].[Status] ON 

INSERT [dbo].[Status] ([Id], [Code], [Name]) VALUES (1, N'ACTIVE', N'Hoạt động')
INSERT [dbo].[Status] ([Id], [Code], [Name]) VALUES (2, N'INACTIVE', N'Dừng hoạt động')
SET IDENTITY_INSERT [dbo].[Status] OFF
GO
SET IDENTITY_INSERT [dbo].[UnitOfMeasure] ON 

INSERT [dbo].[UnitOfMeasure] ([Id], [Code], [Name], [StatusId], [CreatedAt], [UpdatedAt], [DeletedAt], [Used]) VALUES (1, N'1', N'Thùng', 1, CAST(N'2022-03-14T23:42:36.020' AS DateTime), CAST(N'2022-03-14T23:42:36.040' AS DateTime), NULL, 1)
INSERT [dbo].[UnitOfMeasure] ([Id], [Code], [Name], [StatusId], [CreatedAt], [UpdatedAt], [DeletedAt], [Used]) VALUES (3, N'2', N'Túi', 1, CAST(N'2022-03-14T23:57:01.650' AS DateTime), CAST(N'2022-03-14T23:57:01.650' AS DateTime), NULL, 0)
INSERT [dbo].[UnitOfMeasure] ([Id], [Code], [Name], [StatusId], [CreatedAt], [UpdatedAt], [DeletedAt], [Used]) VALUES (4, N'3', N'Chiếc', 1, CAST(N'2022-03-14T23:57:15.180' AS DateTime), CAST(N'2022-03-14T23:57:15.180' AS DateTime), NULL, 0)
INSERT [dbo].[UnitOfMeasure] ([Id], [Code], [Name], [StatusId], [CreatedAt], [UpdatedAt], [DeletedAt], [Used]) VALUES (5, N'4', N'Lon', 1, CAST(N'2022-03-14T23:57:19.813' AS DateTime), CAST(N'2022-03-14T23:57:19.813' AS DateTime), NULL, 0)
INSERT [dbo].[UnitOfMeasure] ([Id], [Code], [Name], [StatusId], [CreatedAt], [UpdatedAt], [DeletedAt], [Used]) VALUES (6, N'5', N'Gói', 1, CAST(N'2022-03-14T23:57:31.613' AS DateTime), CAST(N'2022-03-14T23:57:31.613' AS DateTime), NULL, 0)
SET IDENTITY_INSERT [dbo].[UnitOfMeasure] OFF
GO
SET IDENTITY_INSERT [dbo].[Customer] ON 

INSERT [dbo].[Customer] ([Id], [Code], [Name], [Phone], [Address], [StatusId], [CreatedAt], [UpdatedAt], [DeletedAt], [Used]) VALUES (1, N'1', N'Nguyễn Văn A', N'0891632056', N'Hà Nội', 1, CAST(N'2022-03-14T23:58:12.010' AS DateTime), CAST(N'2022-03-14T23:58:12.010' AS DateTime), NULL, 0)
INSERT [dbo].[Customer] ([Id], [Code], [Name], [Phone], [Address], [StatusId], [CreatedAt], [UpdatedAt], [DeletedAt], [Used]) VALUES (2, N'2', N'Nguyễn Thị B', N'0156984630', N'Nam Định', 1, CAST(N'2022-03-14T23:58:28.500' AS DateTime), CAST(N'2022-03-14T23:58:28.500' AS DateTime), NULL, 0)
INSERT [dbo].[Customer] ([Id], [Code], [Name], [Phone], [Address], [StatusId], [CreatedAt], [UpdatedAt], [DeletedAt], [Used]) VALUES (3, N'3', N'Lý Thị Tình', N'0123456789', N'Hà Nội', 1, CAST(N'2022-03-14T23:58:48.240' AS DateTime), CAST(N'2022-03-14T23:58:48.240' AS DateTime), NULL, 0)
INSERT [dbo].[Customer] ([Id], [Code], [Name], [Phone], [Address], [StatusId], [CreatedAt], [UpdatedAt], [DeletedAt], [Used]) VALUES (4, N'5', N'Nguyễn Hùng Cường', N'0156842397', N'Ninh Bình', 1, CAST(N'2022-03-14T23:59:07.257' AS DateTime), CAST(N'2022-03-14T23:59:07.257' AS DateTime), NULL, 0)
INSERT [dbo].[Customer] ([Id], [Code], [Name], [Phone], [Address], [StatusId], [CreatedAt], [UpdatedAt], [DeletedAt], [Used]) VALUES (7, N'4', N'Trần Thị Ngu', N'0184965238', N'Thanh Hóa', 1, CAST(N'2022-03-14T23:59:46.260' AS DateTime), CAST(N'2022-03-14T23:59:46.260' AS DateTime), NULL, 0)
SET IDENTITY_INSERT [dbo].[Customer] OFF
GO
SET IDENTITY_INSERT [dbo].[Service] ON 

INSERT [dbo].[Service] ([Id], [Code], [Name], [UnitOfMeasureId], [Price], [StatusId], [CreatedAt], [UpdatedAt], [DeletedAt], [Used]) VALUES (1, N'1', N'Dịch Vụ 1', 1, CAST(100.0000 AS Decimal(18, 4)), 1, CAST(N'2022-03-15T00:00:09.570' AS DateTime), CAST(N'2022-03-15T00:00:09.570' AS DateTime), CAST(N'2022-03-15T00:00:09.570' AS DateTime), 0)
INSERT [dbo].[Service] ([Id], [Code], [Name], [UnitOfMeasureId], [Price], [StatusId], [CreatedAt], [UpdatedAt], [DeletedAt], [Used]) VALUES (3, N'2', N'Dịch vụ 2', 3, CAST(500.0000 AS Decimal(18, 4)), 1, CAST(N'2022-03-15T00:00:35.287' AS DateTime), CAST(N'2022-03-15T00:00:35.287' AS DateTime), CAST(N'2022-03-15T00:00:35.287' AS DateTime), 0)
INSERT [dbo].[Service] ([Id], [Code], [Name], [UnitOfMeasureId], [Price], [StatusId], [CreatedAt], [UpdatedAt], [DeletedAt], [Used]) VALUES (4, N'3', N'Dịch vụ 3', 3, CAST(1200.0000 AS Decimal(18, 4)), 1, CAST(N'2022-03-15T00:00:53.270' AS DateTime), CAST(N'2022-03-15T00:00:53.270' AS DateTime), CAST(N'2022-03-15T00:00:53.270' AS DateTime), 0)
INSERT [dbo].[Service] ([Id], [Code], [Name], [UnitOfMeasureId], [Price], [StatusId], [CreatedAt], [UpdatedAt], [DeletedAt], [Used]) VALUES (5, N'4', N'Dịch vụ 4', 5, CAST(5000.0000 AS Decimal(18, 4)), 1, CAST(N'2022-03-15T00:01:01.847' AS DateTime), CAST(N'2022-03-15T00:01:01.847' AS DateTime), CAST(N'2022-03-15T00:01:01.847' AS DateTime), 0)
INSERT [dbo].[Service] ([Id], [Code], [Name], [UnitOfMeasureId], [Price], [StatusId], [CreatedAt], [UpdatedAt], [DeletedAt], [Used]) VALUES (6, N'5', N'Dịch vụ 5', 1, CAST(450.0000 AS Decimal(18, 4)), 1, CAST(N'2022-03-15T00:01:11.300' AS DateTime), CAST(N'2022-03-15T00:01:11.300' AS DateTime), CAST(N'2022-03-15T00:01:11.300' AS DateTime), 0)
SET IDENTITY_INSERT [dbo].[Service] OFF
GO
SET IDENTITY_INSERT [dbo].[OrderService] ON 

INSERT [dbo].[OrderService] ([Id], [Code], [OrderDate], [CustomerId], [Total], [CreatedAt], [UpdatedAt], [DeletedAt], [Used]) VALUES (1, N'1', CAST(N'2022-03-01T00:00:00.000' AS DateTime), 1, CAST(150.0000 AS Decimal(18, 4)), CAST(N'2022-03-15T00:01:46.310' AS DateTime), CAST(N'2022-03-15T00:01:46.310' AS DateTime), NULL, 0)
INSERT [dbo].[OrderService] ([Id], [Code], [OrderDate], [CustomerId], [Total], [CreatedAt], [UpdatedAt], [DeletedAt], [Used]) VALUES (2, N'2', CAST(N'2022-03-01T00:00:00.000' AS DateTime), 2, CAST(100.0000 AS Decimal(18, 4)), CAST(N'2022-03-15T00:02:05.490' AS DateTime), CAST(N'2022-03-15T00:02:05.490' AS DateTime), NULL, 0)
INSERT [dbo].[OrderService] ([Id], [Code], [OrderDate], [CustomerId], [Total], [CreatedAt], [UpdatedAt], [DeletedAt], [Used]) VALUES (3, N'3', CAST(N'2022-01-03T00:00:00.000' AS DateTime), 3, CAST(500.0000 AS Decimal(18, 4)), CAST(N'2022-03-15T00:02:15.840' AS DateTime), CAST(N'2022-03-15T00:02:15.840' AS DateTime), NULL, 0)
INSERT [dbo].[OrderService] ([Id], [Code], [OrderDate], [CustomerId], [Total], [CreatedAt], [UpdatedAt], [DeletedAt], [Used]) VALUES (4, N'4', CAST(N'2022-02-02T00:00:00.000' AS DateTime), 4, CAST(10000.0000 AS Decimal(18, 4)), CAST(N'2022-03-15T00:02:29.350' AS DateTime), CAST(N'2022-03-15T00:02:29.350' AS DateTime), NULL, 0)
INSERT [dbo].[OrderService] ([Id], [Code], [OrderDate], [CustomerId], [Total], [CreatedAt], [UpdatedAt], [DeletedAt], [Used]) VALUES (7, N'5', CAST(N'2022-02-22T00:00:00.000' AS DateTime), 7, CAST(5000.0000 AS Decimal(18, 4)), CAST(N'2022-03-15T00:02:53.450' AS DateTime), CAST(N'2022-03-15T00:02:53.450' AS DateTime), NULL, 0)
SET IDENTITY_INSERT [dbo].[OrderService] OFF
GO
SET IDENTITY_INSERT [dbo].[OrderServiceContent] ON 

INSERT [dbo].[OrderServiceContent] ([Id], [ServiceId], [OrderServiceId], [PrimaryUnitOfMeasureId], [UnitOfMeasureId], [Quantity], [RequestQuantity], [Price], [Amount], [CreatedAt], [UpdatedAt]) VALUES (1, 1, 1, 1, 1, 50, 50, CAST(1000.0000 AS Decimal(18, 4)), CAST(50000.0000 AS Decimal(18, 4)), CAST(N'2022-03-15T00:03:19.310' AS DateTime), CAST(N'2022-03-15T00:03:19.310' AS DateTime))
INSERT [dbo].[OrderServiceContent] ([Id], [ServiceId], [OrderServiceId], [PrimaryUnitOfMeasureId], [UnitOfMeasureId], [Quantity], [RequestQuantity], [Price], [Amount], [CreatedAt], [UpdatedAt]) VALUES (11, 4, 4, 3, 4, 20, 20, CAST(500.0000 AS Decimal(18, 4)), CAST(10000.0000 AS Decimal(18, 4)), CAST(N'2022-03-15T00:04:21.510' AS DateTime), CAST(N'2022-03-15T00:04:21.510' AS DateTime))
INSERT [dbo].[OrderServiceContent] ([Id], [ServiceId], [OrderServiceId], [PrimaryUnitOfMeasureId], [UnitOfMeasureId], [Quantity], [RequestQuantity], [Price], [Amount], [CreatedAt], [UpdatedAt]) VALUES (13, 3, 4, 1, 1, 100, 100, CAST(1000.0000 AS Decimal(18, 4)), CAST(100000.0000 AS Decimal(18, 4)), CAST(N'2022-03-15T00:04:41.353' AS DateTime), CAST(N'2022-03-15T00:04:41.353' AS DateTime))
SET IDENTITY_INSERT [dbo].[OrderServiceContent] OFF
GO
