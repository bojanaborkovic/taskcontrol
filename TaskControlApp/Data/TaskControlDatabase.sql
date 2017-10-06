
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 10/04/2017 18:44:32
-- Generated from EDMX file: D:\repos\taskcontrol\TaskControlApp\Data\TaskControlModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [TaskControl];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_dbo_AspNetUserClaims_dbo_AspNetUsers_UserId]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetUserClaims] DROP CONSTRAINT [FK_dbo_AspNetUserClaims_dbo_AspNetUsers_UserId];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_AspNetUserLogins_dbo_AspNetUsers_UserId]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetUserLogins] DROP CONSTRAINT [FK_dbo_AspNetUserLogins_dbo_AspNetUsers_UserId];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_AspNetUserRoles_dbo_AspNetUsers_UserId]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetUserRoles] DROP CONSTRAINT [FK_dbo_AspNetUserRoles_dbo_AspNetUsers_UserId];
GO
IF OBJECT_ID(N'[dbo].[FK_Issue_Type]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Task] DROP CONSTRAINT [FK_Issue_Type];
GO
IF OBJECT_ID(N'[TaskControlModelStoreContainer].[FK_Note_Project]', 'F') IS NOT NULL
    ALTER TABLE [TaskControlModelStoreContainer].[Note] DROP CONSTRAINT [FK_Note_Project];
GO
IF OBJECT_ID(N'[dbo].[FK_Project_AspNetUsers]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Project] DROP CONSTRAINT [FK_Project_AspNetUsers];
GO
IF OBJECT_ID(N'[dbo].[FK_Task_AspNetUsers]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Task] DROP CONSTRAINT [FK_Task_AspNetUsers];
GO
IF OBJECT_ID(N'[dbo].[FK_Task_Priority]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Task] DROP CONSTRAINT [FK_Task_Priority];
GO
IF OBJECT_ID(N'[dbo].[FK_Task_Project]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Task] DROP CONSTRAINT [FK_Task_Project];
GO
IF OBJECT_ID(N'[dbo].[FK_Task_Status]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Task] DROP CONSTRAINT [FK_Task_Status];
GO
IF OBJECT_ID(N'[dbo].[FK_TaskAsigneeHistory_Task]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TaskAsigneeHistory] DROP CONSTRAINT [FK_TaskAsigneeHistory_Task];
GO
IF OBJECT_ID(N'[dbo].[FK_TaskStatusHistory_Task]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TaskStatusHistory] DROP CONSTRAINT [FK_TaskStatusHistory_Task];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[AspNetRoles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetRoles];
GO
IF OBJECT_ID(N'[dbo].[AspNetUserClaims]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetUserClaims];
GO
IF OBJECT_ID(N'[dbo].[AspNetUserLogins]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetUserLogins];
GO
IF OBJECT_ID(N'[dbo].[AspNetUserRoles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetUserRoles];
GO
IF OBJECT_ID(N'[dbo].[AspNetUsers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetUsers];
GO
IF OBJECT_ID(N'[dbo].[Comment]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Comment];
GO
IF OBJECT_ID(N'[dbo].[IssueType]', 'U') IS NOT NULL
    DROP TABLE [dbo].[IssueType];
GO
IF OBJECT_ID(N'[dbo].[Priority]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Priority];
GO
IF OBJECT_ID(N'[dbo].[Project]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Project];
GO
IF OBJECT_ID(N'[dbo].[Status]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Status];
GO
IF OBJECT_ID(N'[dbo].[Task]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Task];
GO
IF OBJECT_ID(N'[dbo].[TaskAsigneeHistory]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TaskAsigneeHistory];
GO
IF OBJECT_ID(N'[dbo].[TaskStatusHistory]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TaskStatusHistory];
GO
IF OBJECT_ID(N'[TaskControlModelStoreContainer].[Note]', 'U') IS NOT NULL
    DROP TABLE [TaskControlModelStoreContainer].[Note];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'AspNetRoles'
CREATE TABLE [dbo].[AspNetRoles] (
    [Id] bigint IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(256)  NOT NULL,
    [Description] nvarchar(500)  NULL,
    [DateCreated] datetime  NULL
);
GO

-- Creating table 'AspNetUserClaims'
CREATE TABLE [dbo].[AspNetUserClaims] (
    [Id] bigint IDENTITY(1,1) NOT NULL,
    [UserId] bigint  NOT NULL,
    [ClaimType] nvarchar(max)  NULL,
    [ClaimValue] nvarchar(max)  NULL
);
GO

-- Creating table 'AspNetUserLogins'
CREATE TABLE [dbo].[AspNetUserLogins] (
    [LoginProvider] nvarchar(128)  NOT NULL,
    [ProviderKey] nvarchar(128)  NOT NULL,
    [UserId] bigint  NOT NULL
);
GO

-- Creating table 'AspNetUserRoles'
CREATE TABLE [dbo].[AspNetUserRoles] (
    [UserId] bigint  NOT NULL,
    [RoleId] bigint  NOT NULL
);
GO

-- Creating table 'AspNetUsers'
CREATE TABLE [dbo].[AspNetUsers] (
    [Id] bigint IDENTITY(1,1) NOT NULL,
    [Email] nvarchar(256)  NULL,
    [EmailConfirmed] bit  NOT NULL,
    [PasswordHash] nvarchar(max)  NULL,
    [SecurityStamp] nvarchar(max)  NULL,
    [PhoneNumber] nvarchar(max)  NULL,
    [PhoneNumberConfirmed] bit  NOT NULL,
    [TwoFactorEnabled] bit  NOT NULL,
    [LockoutEndDateUtc] datetime  NULL,
    [LockoutEnabled] bit  NOT NULL,
    [AccessFailedCount] int  NOT NULL,
    [UserName] nvarchar(256)  NOT NULL,
    [FirstName] nvarchar(500)  NULL,
    [LastName] nvarchar(500)  NULL
);
GO

-- Creating table 'Comments'
CREATE TABLE [dbo].[Comments] (
    [Id] bigint IDENTITY(1,1) NOT NULL,
    [Text] nvarchar(1000)  NULL,
    [DateCreated] datetime  NULL,
    [Author] bigint  NOT NULL,
    [TaskId] bigint  NOT NULL
);
GO

-- Creating table 'IssueTypes'
CREATE TABLE [dbo].[IssueTypes] (
    [Id] int  NOT NULL,
    [Name] nvarchar(250)  NOT NULL
);
GO

-- Creating table 'Priorities'
CREATE TABLE [dbo].[Priorities] (
    [Id] int  NOT NULL,
    [Name] nvarchar(250)  NOT NULL
);
GO

-- Creating table 'Projects'
CREATE TABLE [dbo].[Projects] (
    [Id] bigint  IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(250)  NOT NULL,
    [OwnerId] bigint  NULL,
    [Description] nvarchar(1000)  NULL
);
GO

-- Creating table 'Status'
CREATE TABLE [dbo].[Status] (
    [Id] int  NOT NULL,
    [Name] nvarchar(250)  NOT NULL
);
GO

-- Creating table 'Tasks'
CREATE TABLE [dbo].[Tasks] (
    [Id] bigint  IDENTITY(1,1) NOT NULL,
    [Title] nvarchar(250)  NOT NULL,
    [IssueType] int  NOT NULL,
    [Asignee] bigint  NULL,
    [DateCreated] datetime  NULL,
    [Status] int  NOT NULL,
    [Description] nvarchar(500)  NULL,
    [Reporter] int  NULL,
    [Priority] int  NOT NULL,
    [ProjectId] bigint  NOT NULL,
    [DueDate] datetime  NULL,
    [CreatedBy] bigint  NULL
);
GO

-- Creating table 'TaskAsigneeHistories'
CREATE TABLE [dbo].[TaskAsigneeHistories] (
    [Id] bigint IDENTITY(1,1) NOT NULL,
    [AsigneeBefore] bigint  NULL,
    [AsigneeAfter] bigint  NOT NULL,
    [ChangeDate] datetime  NOT NULL,
    [TaskId] bigint  NOT NULL,
    [ChangeBy] bigint  NULL
);
GO

-- Creating table 'TaskStatusHistories'
CREATE TABLE [dbo].[TaskStatusHistories] (
    [Id] bigint IDENTITY(1,1) NOT NULL,
    [StatusBefore] int  NULL,
    [StatusAfter] int  NOT NULL,
    [ChangeDate] datetime  NULL,
    [TaskId] bigint  NOT NULL,
    [ChangeBy] bigint  NULL
);
GO

-- Creating table 'Notes'
CREATE TABLE [dbo].[Notes] (
    [Id] bigint IDENTITY(1,1)  NOT NULL,
    [Text] nvarchar(1000)  NULL,
    [DateCreated] datetime  NULL,
    [Author] bigint  NULL,
    [ProjectId] bigint  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'AspNetRoles'
ALTER TABLE [dbo].[AspNetRoles]
ADD CONSTRAINT [PK_AspNetRoles]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'AspNetUserClaims'
ALTER TABLE [dbo].[AspNetUserClaims]
ADD CONSTRAINT [PK_AspNetUserClaims]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [LoginProvider], [ProviderKey], [UserId] in table 'AspNetUserLogins'
ALTER TABLE [dbo].[AspNetUserLogins]
ADD CONSTRAINT [PK_AspNetUserLogins]
    PRIMARY KEY CLUSTERED ([LoginProvider], [ProviderKey], [UserId] ASC);
GO

-- Creating primary key on [UserId], [RoleId] in table 'AspNetUserRoles'
ALTER TABLE [dbo].[AspNetUserRoles]
ADD CONSTRAINT [PK_AspNetUserRoles]
    PRIMARY KEY CLUSTERED ([UserId], [RoleId] ASC);
GO

-- Creating primary key on [Id] in table 'AspNetUsers'
ALTER TABLE [dbo].[AspNetUsers]
ADD CONSTRAINT [PK_AspNetUsers]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Comments'
ALTER TABLE [dbo].[Comments]
ADD CONSTRAINT [PK_Comments]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'IssueTypes'
ALTER TABLE [dbo].[IssueTypes]
ADD CONSTRAINT [PK_IssueTypes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Priorities'
ALTER TABLE [dbo].[Priorities]
ADD CONSTRAINT [PK_Priorities]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Projects'
ALTER TABLE [dbo].[Projects]
ADD CONSTRAINT [PK_Projects]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Status'
ALTER TABLE [dbo].[Status]
ADD CONSTRAINT [PK_Status]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Tasks'
ALTER TABLE [dbo].[Tasks]
ADD CONSTRAINT [PK_Tasks]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'TaskAsigneeHistories'
ALTER TABLE [dbo].[TaskAsigneeHistories]
ADD CONSTRAINT [PK_TaskAsigneeHistories]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'TaskStatusHistories'
ALTER TABLE [dbo].[TaskStatusHistories]
ADD CONSTRAINT [PK_TaskStatusHistories]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id], [ProjectId] in table 'Notes'
ALTER TABLE [dbo].[Notes]
ADD CONSTRAINT [PK_Notes]
    PRIMARY KEY CLUSTERED ([Id], [ProjectId] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [UserId] in table 'AspNetUserClaims'
ALTER TABLE [dbo].[AspNetUserClaims]
ADD CONSTRAINT [FK_dbo_AspNetUserClaims_dbo_AspNetUsers_UserId]
    FOREIGN KEY ([UserId])
    REFERENCES [dbo].[AspNetUsers]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_AspNetUserClaims_dbo_AspNetUsers_UserId'
CREATE INDEX [IX_FK_dbo_AspNetUserClaims_dbo_AspNetUsers_UserId]
ON [dbo].[AspNetUserClaims]
    ([UserId]);
GO

-- Creating foreign key on [UserId] in table 'AspNetUserLogins'
ALTER TABLE [dbo].[AspNetUserLogins]
ADD CONSTRAINT [FK_dbo_AspNetUserLogins_dbo_AspNetUsers_UserId]
    FOREIGN KEY ([UserId])
    REFERENCES [dbo].[AspNetUsers]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_AspNetUserLogins_dbo_AspNetUsers_UserId'
CREATE INDEX [IX_FK_dbo_AspNetUserLogins_dbo_AspNetUsers_UserId]
ON [dbo].[AspNetUserLogins]
    ([UserId]);
GO

-- Creating foreign key on [UserId] in table 'AspNetUserRoles'
ALTER TABLE [dbo].[AspNetUserRoles]
ADD CONSTRAINT [FK_dbo_AspNetUserRoles_dbo_AspNetUsers_UserId]
    FOREIGN KEY ([UserId])
    REFERENCES [dbo].[AspNetUsers]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating foreign key on [OwnerId] in table 'Projects'
ALTER TABLE [dbo].[Projects]
ADD CONSTRAINT [FK_Project_AspNetUsers]
    FOREIGN KEY ([OwnerId])
    REFERENCES [dbo].[AspNetUsers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Project_AspNetUsers'
CREATE INDEX [IX_FK_Project_AspNetUsers]
ON [dbo].[Projects]
    ([OwnerId]);
GO

-- Creating foreign key on [TaskId] in table 'Comments'
ALTER TABLE [dbo].[Comments]
ADD CONSTRAINT [FK_Comment_Task]
    FOREIGN KEY ([TaskId])
    REFERENCES [dbo].[Tasks]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Comment_Task'
CREATE INDEX [IX_FK_Comment_Task]
ON [dbo].[Comments]
    ([TaskId]);
GO

-- Creating foreign key on [IssueType] in table 'Tasks'
ALTER TABLE [dbo].[Tasks]
ADD CONSTRAINT [FK_Issue_Type]
    FOREIGN KEY ([IssueType])
    REFERENCES [dbo].[IssueTypes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Issue_Type'
CREATE INDEX [IX_FK_Issue_Type]
ON [dbo].[Tasks]
    ([IssueType]);
GO

-- Creating foreign key on [Priority] in table 'Tasks'
ALTER TABLE [dbo].[Tasks]
ADD CONSTRAINT [FK_Task_Priority]
    FOREIGN KEY ([Priority])
    REFERENCES [dbo].[Priorities]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Task_Priority'
CREATE INDEX [IX_FK_Task_Priority]
ON [dbo].[Tasks]
    ([Priority]);
GO

-- Creating foreign key on [ProjectId] in table 'Notes'
ALTER TABLE [dbo].[Notes]
ADD CONSTRAINT [FK_Note_Project]
    FOREIGN KEY ([ProjectId])
    REFERENCES [dbo].[Projects]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Note_Project'
CREATE INDEX [IX_FK_Note_Project]
ON [dbo].[Notes]
    ([ProjectId]);
GO

-- Creating foreign key on [ProjectId] in table 'Tasks'
ALTER TABLE [dbo].[Tasks]
ADD CONSTRAINT [FK_Task_Project]
    FOREIGN KEY ([ProjectId])
    REFERENCES [dbo].[Projects]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Task_Project'
CREATE INDEX [IX_FK_Task_Project]
ON [dbo].[Tasks]
    ([ProjectId]);
GO

-- Creating foreign key on [Status] in table 'Tasks'
ALTER TABLE [dbo].[Tasks]
ADD CONSTRAINT [FK_Task_Status]
    FOREIGN KEY ([Status])
    REFERENCES [dbo].[Status]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Task_Status'
CREATE INDEX [IX_FK_Task_Status]
ON [dbo].[Tasks]
    ([Status]);
GO

-- Creating foreign key on [TaskId] in table 'TaskAsigneeHistories'
ALTER TABLE [dbo].[TaskAsigneeHistories]
ADD CONSTRAINT [FK_TaskAsigneeHistory_Task]
    FOREIGN KEY ([TaskId])
    REFERENCES [dbo].[Tasks]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_TaskAsigneeHistory_Task'
CREATE INDEX [IX_FK_TaskAsigneeHistory_Task]
ON [dbo].[TaskAsigneeHistories]
    ([TaskId]);
GO

-- Creating foreign key on [TaskId] in table 'TaskStatusHistories'
ALTER TABLE [dbo].[TaskStatusHistories]
ADD CONSTRAINT [FK_TaskStatusHistory_Task]
    FOREIGN KEY ([TaskId])
    REFERENCES [dbo].[Tasks]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_TaskStatusHistory_Task'
CREATE INDEX [IX_FK_TaskStatusHistory_Task]
ON [dbo].[TaskStatusHistories]
    ([TaskId]);
GO

-- Creating foreign key on [Asignee] in table 'Tasks'
ALTER TABLE [dbo].[Tasks]
ADD CONSTRAINT [FK_Task_AspNetUsers]
    FOREIGN KEY ([Asignee])
    REFERENCES [dbo].[AspNetUsers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Task_AspNetUsers'
CREATE INDEX [IX_FK_Task_AspNetUsers]
ON [dbo].[Tasks]
    ([Asignee]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------