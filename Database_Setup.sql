-- SlotIQ Interview Database Setup Script
-- This script creates the database and Members table for the Member Onboarding feature

-- Create Database
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'SlotIQInterview')
BEGIN
    CREATE DATABASE SlotIQInterview;
END
GO

USE SlotIQInterview;
GO

-- Create Members Table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Members]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Members] (
        [MemberID] UNIQUEIDENTIFIER PRIMARY KEY,
        [UserName] NVARCHAR(100) NOT NULL,
        [FirstName] NVARCHAR(50) NOT NULL,
        [LastName] NVARCHAR(50) NOT NULL,
        [Password] NVARCHAR(100) NOT NULL,
        [EmailID] NVARCHAR(100) NOT NULL,
        [PhoneNumber] NVARCHAR(10) NULL,
        [RoleID] INT NOT NULL,
        [PracticeID] UNIQUEIDENTIFIER NOT NULL,
        [IsActive] BIT NOT NULL DEFAULT 1,
        [CreatedDate] DATETIME2 NOT NULL,
        [ModifiedDate] DATETIME2 NOT NULL,
        [CreatedBy] NVARCHAR(50) NOT NULL,
        [ModifiedBy] NVARCHAR(50) NOT NULL,
        [Source] INT NOT NULL,
        CONSTRAINT [UQ_Members_UserName] UNIQUE ([UserName]),
        CONSTRAINT [UQ_Members_EmailID] UNIQUE ([EmailID]),
        CONSTRAINT [UQ_Members_PhoneNumber] UNIQUE ([PhoneNumber])
    );
    
    PRINT 'Members table created successfully.';
END
ELSE
BEGIN
    PRINT 'Members table already exists.';
END
GO

-- Create indexes for better query performance
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Members_IsActive')
BEGIN
    CREATE NONCLUSTERED INDEX [IX_Members_IsActive] ON [dbo].[Members]([IsActive]);
    PRINT 'Index IX_Members_IsActive created successfully.';
END
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Members_RoleID')
BEGIN
    CREATE NONCLUSTERED INDEX [IX_Members_RoleID] ON [dbo].[Members]([RoleID]);
    PRINT 'Index IX_Members_RoleID created successfully.';
END
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Members_PracticeID')
BEGIN
    CREATE NONCLUSTERED INDEX [IX_Members_PracticeID] ON [dbo].[Members]([PracticeID]);
    PRINT 'Index IX_Members_PracticeID created successfully.';
END
GO

PRINT 'Database setup completed successfully.';
GO
