IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [Posts] (
    [Id] int NOT NULL IDENTITY,
    [Author] nvarchar(max) NULL,
    [Title] nvarchar(max) NULL,
    [Text] nvarchar(max) NULL,
    [PublishedDate] datetime2 NULL,
    [PostStatus] int NULL,
    CONSTRAINT [PK_Posts] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Comments] (
    [Id] int NOT NULL IDENTITY,
    [User] nvarchar(max) NULL,
    [Text] nvarchar(max) NULL,
    [PostId] int NOT NULL,
    CONSTRAINT [PK_Comments] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Comments_Posts_PostId] FOREIGN KEY ([PostId]) REFERENCES [Posts] ([Id]) ON DELETE CASCADE
);
GO

CREATE INDEX [IX_Comments_PostId] ON [Comments] ([PostId]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20201219055249_initial', N'5.0.1');
GO

COMMIT;
GO

