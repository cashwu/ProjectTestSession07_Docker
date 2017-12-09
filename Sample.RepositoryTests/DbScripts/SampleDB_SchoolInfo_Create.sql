CREATE TABLE [dbo].[SchoolInfo](
	[Id] [nvarchar](10) NOT NULL,
	[Category] [int] NULL,
	[Name] [nvarchar](50) NULL,
	[IsPublic] [nvarchar](10) NULL,
	[CityId] [nvarchar](10) NULL,
	[DistrictId] [int] NULL,
	[Address] [nvarchar](100) NULL,
	[Phone] [nvarchar](20) NULL,
	[Url] [nvarchar](500) NULL,
 CONSTRAINT [PK_SchoolInfo] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
))