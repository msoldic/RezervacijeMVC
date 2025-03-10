CREATE TABLE [dbo].[Tool](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ToolType] [nchar](20) NOT NULL,
	[PriceRentPerDay] [int] NOT NULL,
 CONSTRAINT [PK_Tool] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Tool] ADD  CONSTRAINT [DF_Tool_PriceRentPerDay]  DEFAULT ((0)) FOR [PriceRentPerDay]
GO

--------------------------------

CREATE TABLE [dbo].[ToolReservation](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ClientFirstName] [nchar](20) NOT NULL,
	[ClientSecondName] [nchar](20) NOT NULL,
	[DateReservationFrom] [datetime] NULL,
	[DateReservationTo] [datetime] NULL,
	[Note] [nchar](50) NULL,
	[ToolID] [int] NOT NULL,
	[TotalRentPrice] [int] NULL,
 CONSTRAINT [PK_ToolReservation] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[ToolReservation]  WITH CHECK ADD  CONSTRAINT [FK_ToolReservation_Tool] FOREIGN KEY([ToolID])
REFERENCES [dbo].[Tool] ([ID])
GO

ALTER TABLE [dbo].[ToolReservation] CHECK CONSTRAINT [FK_ToolReservation_Tool]
GO
