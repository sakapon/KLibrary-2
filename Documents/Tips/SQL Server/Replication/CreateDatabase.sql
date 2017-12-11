use [master]
go

create database [Sync1]
go

USE [Sync1]
GO

CREATE TABLE [dbo].[Users](
    [Id] [int] NOT NULL,
    [Name] [nvarchar](10) NOT NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED ([Id] ASC)
)
GO

CREATE TABLE [dbo].[Roles](
    [Id] [int] NOT NULL,
    [Name] [nvarchar](10) NOT NULL,
    CONSTRAINT [PK_Roles] PRIMARY KEY CLUSTERED ([Id] ASC)
)
GO

CREATE TABLE [dbo].[UserRoles](
    [UserId] [int] NOT NULL,
    [RoleId] [int] NOT NULL,
    CONSTRAINT [PK_UserRoles] PRIMARY KEY CLUSTERED ([UserId] ASC, [RoleId] ASC)
)
GO

ALTER TABLE [dbo].[UserRoles]  WITH CHECK ADD  CONSTRAINT [FK_UserRoles_Roles] FOREIGN KEY([RoleId])
REFERENCES [dbo].[Roles] ([Id])
GO
ALTER TABLE [dbo].[UserRoles] CHECK CONSTRAINT [FK_UserRoles_Roles]
GO

ALTER TABLE [dbo].[UserRoles]  WITH CHECK ADD  CONSTRAINT [FK_UserRoles_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[UserRoles] CHECK CONSTRAINT [FK_UserRoles_Users]
GO

INSERT INTO [Users] ([Id], [Name]) VALUES (1, N'qqq')
INSERT INTO [Users] ([Id], [Name]) VALUES (2, N'www')
INSERT INTO [Users] ([Id], [Name]) VALUES (3, N'eee')
GO

INSERT INTO [Roles] ([Id], [Name]) VALUES (1, N'aaa')
INSERT INTO [Roles] ([Id], [Name]) VALUES (2, N'sss')
INSERT INTO [Roles] ([Id], [Name]) VALUES (3, N'ddd')
GO

INSERT INTO [UserRoles] ([UserId], [RoleId]) VALUES (1, 1)
INSERT INTO [UserRoles] ([UserId], [RoleId]) VALUES (1, 2)
INSERT INTO [UserRoles] ([UserId], [RoleId]) VALUES (2, 3)
GO
