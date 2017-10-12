USE [TaskControl]
GO

/****** Object:  Table [dbo].[RoleClaimsOnProject]    Script Date: 12.10.2017. 10.28.37 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[RoleClaimsOnProject](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[RoleId] [bigint] NOT NULL,
	[ProjectId] [bigint] NOT NULL,
	[HaveAcess] [bit] NULL,
 CONSTRAINT [PK_RoleClaimsOnProject] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[RoleClaimsOnProject]  WITH CHECK ADD  CONSTRAINT [FK_RoleClaimsOnProject_AspNetRoles] FOREIGN KEY([RoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
GO

ALTER TABLE [dbo].[RoleClaimsOnProject] CHECK CONSTRAINT [FK_RoleClaimsOnProject_AspNetRoles]
GO


