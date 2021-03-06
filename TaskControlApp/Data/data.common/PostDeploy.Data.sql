USE [TaskControl]
GO
SET IDENTITY_INSERT [dbo].[AspNetRoles] ON 

INSERT [dbo].[AspNetRoles] ([Id], [Name], [Description], [DateCreated]) VALUES (1, N'Admin', N'admin role', NULL)
INSERT [dbo].[AspNetRoles] ([Id], [Name], [Description], [DateCreated]) VALUES (2, N'IT Admin', N'IT admin role', NULL)
INSERT [dbo].[AspNetRoles] ([Id], [Name], [Description], [DateCreated]) VALUES (3, N'User', NULL, NULL)
SET IDENTITY_INSERT [dbo].[AspNetRoles] OFF

INSERT [dbo].[IssueType] ([Id], [Name]) VALUES (1, N'Task')
INSERT [dbo].[IssueType] ([Id], [Name]) VALUES (2, N'Improvement')
INSERT [dbo].[IssueType] ([Id], [Name]) VALUES (3, N'Bug')
INSERT [dbo].[IssueType] ([Id], [Name]) VALUES (4, N'Story')
INSERT [dbo].[Priority] ([Id], [Name]) VALUES (1, N'Minor')
INSERT [dbo].[Priority] ([Id], [Name]) VALUES (2, N'Major')
INSERT [dbo].[Priority] ([Id], [Name]) VALUES (3, N'Critical')
INSERT [dbo].[Priority] ([Id], [Name]) VALUES (4, N'Blocker')
INSERT [dbo].[Status] ([Id], [Name]) VALUES (1, N'ToDo')
INSERT [dbo].[Status] ([Id], [Name]) VALUES (2, N'InProgress')
INSERT [dbo].[Status] ([Id], [Name]) VALUES (3, N'Done')

