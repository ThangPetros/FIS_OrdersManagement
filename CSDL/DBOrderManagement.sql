USE master
GO

CREATE DATABASE [OrdersManagement]
GO

USE [OrdersManagement]
GO

CREATE TABLE [dbo].[Customer](
	[Id] [bigint] NOT NULL,
	[Code] [nvarchar](50) NOT NULL,
	[Name] [nvarchar](500) NOT NULL,
	[Phone] [nvarchar](50) NOT NULL,
	[Address] [nvarchar](500) NOT NULL,
	[StatusId] [bigint] NOT NULL,
	[CreatedAt] [datetime] NOT NULL,
	[UpdatedAt] [datetime] NOT NULL,
	[DeleteAt] [datetime] NULL,
	[Used] [bit] NOT NULL,
 CONSTRAINT [PK_Customer] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[OrderService](
	[Id] [bigint] NOT NULL,
	[Code] [nvarchar](50) NOT NULL,
	[OrderDate] [datetime] NOT NULL,
	[CustomerId] [bigint] NOT NULL,
	[Total] [decimal](18, 4) NOT NULL,
	[CreatedAt] [datetime] NOT NULL,
	[UpdatedAt] [datetime] NOT NULL,
	[DeleteAt] [datetime] NULL,
	[Used] [bit] NOT NULL,
 CONSTRAINT [PK_OrderService] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[OrderServiceContent](
	[Id] [bigint] NOT NULL,
	[ServiceId] [bigint] NOT NULL,
	[OrderServiceId] [bigint] NOT NULL,
	[PrimaryUnitOfMeasureId] [bigint] NOT NULL,
	[UnitOfMeasureId] [bigint] NOT NULL,
	[Quantity] [bigint] NOT NULL,
	[RequestQuantity] [bigint] NOT NULL,
	[Prive] [decimal](18, 4) NOT NULL,
	[Amount] [decimal](18, 4) NOT NULL,
 CONSTRAINT [PK_OrderServiceContent] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[Service](
	[Id] [bigint] NOT NULL,
	[Code] [nvarchar](50) NOT NULL,
	[Name] [nvarchar](500) NOT NULL,
	[UnitOfMeasureId] [bigint] NOT NULL,
	[Price] [decimal](18, 4) NOT NULL,
	[StatusId] [bigint] NOT NULL,
	[CreatedAt] [datetime] NOT NULL,
	[UpdatedAt] [datetime] NOT NULL,
	[DeleteAt] [datetime] NULL,
	[Used] [bit] NOT NULL,
 CONSTRAINT [PK_Service] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[Status (ENUM)](
	[Id] [bigint] NOT NULL,
	[Code] [nvarchar](50) NOT NULL,
	[Name] [nvarchar](500) NOT NULL,
 CONSTRAINT [PK_Status (ENUM)] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[UnitOfMeasure (MDM)](
	[Id] [bigint] NOT NULL,
	[Code] [nvarchar](50) NOT NULL,
	[Name] [nvarchar](500) NOT NULL,
	[StatusId] [bigint] NOT NULL,
	[CreatedAt] [datetime] NOT NULL,
	[UpdatedAt] [datetime] NOT NULL,
	[DeleteAt] [datetime] NULL,
	[Used] [bit] NOT NULL,
 CONSTRAINT [PK_UnitOfMeasure (MDM)] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Customer]  WITH CHECK ADD  CONSTRAINT [FK_Customer_Status (ENUM)] FOREIGN KEY([StatusId])
REFERENCES [dbo].[Status (ENUM)] ([Id])
GO
ALTER TABLE [dbo].[Customer] CHECK CONSTRAINT [FK_Customer_Status (ENUM)]
GO
ALTER TABLE [dbo].[OrderService]  WITH CHECK ADD  CONSTRAINT [FK_OrderService_Customer] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[Customer] ([Id])
GO
ALTER TABLE [dbo].[OrderService] CHECK CONSTRAINT [FK_OrderService_Customer]
GO
ALTER TABLE [dbo].[OrderServiceContent]  WITH CHECK ADD  CONSTRAINT [FK_OrderServiceContent_OrderService] FOREIGN KEY([OrderServiceId])
REFERENCES [dbo].[OrderService] ([Id])
GO
ALTER TABLE [dbo].[OrderServiceContent] CHECK CONSTRAINT [FK_OrderServiceContent_OrderService]
GO
ALTER TABLE [dbo].[OrderServiceContent]  WITH CHECK ADD  CONSTRAINT [FK_OrderServiceContent_Service] FOREIGN KEY([ServiceId])
REFERENCES [dbo].[Service] ([Id])
GO
ALTER TABLE [dbo].[OrderServiceContent] CHECK CONSTRAINT [FK_OrderServiceContent_Service]
GO
ALTER TABLE [dbo].[OrderServiceContent]  WITH CHECK ADD  CONSTRAINT [FK_OrderServiceContent_UnitOfMeasure (MDM)] FOREIGN KEY([UnitOfMeasureId])
REFERENCES [dbo].[UnitOfMeasure (MDM)] ([Id])
GO
ALTER TABLE [dbo].[OrderServiceContent] CHECK CONSTRAINT [FK_OrderServiceContent_UnitOfMeasure (MDM)]
GO
ALTER TABLE [dbo].[Service]  WITH CHECK ADD  CONSTRAINT [FK_Service_Status (ENUM)] FOREIGN KEY([StatusId])
REFERENCES [dbo].[Status (ENUM)] ([Id])
GO
ALTER TABLE [dbo].[Service] CHECK CONSTRAINT [FK_Service_Status (ENUM)]
GO
ALTER TABLE [dbo].[Service]  WITH CHECK ADD  CONSTRAINT [FK_Service_UnitOfMeasure (MDM)] FOREIGN KEY([UnitOfMeasureId])
REFERENCES [dbo].[UnitOfMeasure (MDM)] ([Id])
GO
ALTER TABLE [dbo].[Service] CHECK CONSTRAINT [FK_Service_UnitOfMeasure (MDM)]
GO
ALTER TABLE [dbo].[UnitOfMeasure (MDM)]  WITH CHECK ADD  CONSTRAINT [FK_UnitOfMeasure (MDM)_Status (ENUM)] FOREIGN KEY([StatusId])
REFERENCES [dbo].[Status (ENUM)] ([Id])
GO
ALTER TABLE [dbo].[UnitOfMeasure (MDM)] CHECK CONSTRAINT [FK_UnitOfMeasure (MDM)_Status (ENUM)]
GO
