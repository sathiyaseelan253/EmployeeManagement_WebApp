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

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230619172117_Initial')
BEGIN
    CREATE TABLE [Countries] (
        [CountryID] uniqueidentifier NOT NULL,
        [CountryName] nvarchar(40) NULL,
        CONSTRAINT [PK_Countries] PRIMARY KEY ([CountryID])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230619172117_Initial')
BEGIN
    CREATE TABLE [Persons] (
        [PersonID] uniqueidentifier NOT NULL,
        [PersonName] nvarchar(40) NULL,
        [Email] nvarchar(40) NULL,
        [DateOfBirth] datetime2 NULL,
        [Gender] nvarchar(6) NULL,
        [CountryID] uniqueidentifier NULL,
        [Address] nvarchar(200) NULL,
        [ReceiveNewsLetters] bit NOT NULL,
        [ZipCode] varchar(6) NULL DEFAULT '639005',
        CONSTRAINT [PK_Persons] PRIMARY KEY ([PersonID]),
        CONSTRAINT [chk_ZipCodeConstraint] CHECK (len([ZipCode])=6),
        CONSTRAINT [FK_Persons_Countries_CountryID] FOREIGN KEY ([CountryID]) REFERENCES [Countries] ([CountryID])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230619172117_Initial')
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'CountryID', N'CountryName') AND [object_id] = OBJECT_ID(N'[Countries]'))
        SET IDENTITY_INSERT [Countries] ON;
    EXEC(N'INSERT INTO [Countries] ([CountryID], [CountryName])
    VALUES (''12e15727-d369-49a9-8b13-bc22e9362179'', N''China''),
    (''14629847-905a-4a0e-9abe-80b61655c5cb'', N''Philippines''),
    (''501c6d33-1bbe-45f1-8fbd-2275913c6218'', N''China''),
    (''56bf46a4-02b8-4693-a0f5-0a95e2218bdc'', N''Thailand''),
    (''8f30bedc-47dd-4286-8950-73d8a68e5d41'', N''Palestinian Territory'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'CountryID', N'CountryName') AND [object_id] = OBJECT_ID(N'[Countries]'))
        SET IDENTITY_INSERT [Countries] OFF;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230619172117_Initial')
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'PersonID', N'Address', N'CountryID', N'DateOfBirth', N'Email', N'Gender', N'PersonName', N'ZipCode', N'ReceiveNewsLetters') AND [object_id] = OBJECT_ID(N'[Persons]'))
        SET IDENTITY_INSERT [Persons] ON;
    EXEC(N'INSERT INTO [Persons] ([PersonID], [Address], [CountryID], [DateOfBirth], [Email], [Gender], [PersonName], [ZipCode], [ReceiveNewsLetters])
    VALUES (''012107df-862f-4f16-ba94-e5c16886f005'', N''413 Sachtjen Way'', ''12e15727-d369-49a9-8b13-bc22e9362179'', ''1990-09-20T00:00:00.0000000'', N''hmosco8@tripod.com'', N''Male'', N''Hansiain'', ''600103'', CAST(1 AS bit)),
    (''28d11936-9466-4a4b-b9c5-2f0a8e0cbde9'', N''2 Warrior Avenue'', ''501c6d33-1bbe-45f1-8fbd-2275913c6218'', ''1990-05-24T00:00:00.0000000'', N''mconachya@va.gov'', N''Female'', N''Minta'', ''600103'', CAST(1 AS bit)),
    (''29339209-63f5-492f-8459-754943c74abf'', N''57449 Brown Way'', ''12e15727-d369-49a9-8b13-bc22e9362179'', ''1983-02-16T00:00:00.0000000'', N''mjarrell6@wisc.edu'', N''Male'', N''Maddy'', ''600103'', CAST(1 AS bit)),
    (''2a6d3738-9def-43ac-9279-0310edc7ceca'', N''97570 Raven Circle'', ''8f30bedc-47dd-4286-8950-73d8a68e5d41'', ''1988-01-04T00:00:00.0000000'', N''mlingfoot5@netvibes.com'', N''Male'', N''Mitchael'', ''600103'', CAST(0 AS bit)),
    (''89e5f445-d89f-4e12-94e0-5ad5b235d704'', N''50467 Holy Cross Crossing'', ''56bf46a4-02b8-4693-a0f5-0a95e2218bdc'', ''1995-02-11T00:00:00.0000000'', N''ttregona4@stumbleupon.com'', N''Male'', N''Tani'', ''600103'', CAST(0 AS bit)),
    (''a3b9833b-8a4d-43e9-8690-61e08df81a9a'', N''9334 Fremont Street'', ''501c6d33-1bbe-45f1-8fbd-2275913c6218'', ''1987-01-19T00:00:00.0000000'', N''vklussb@nationalgeographic.com'', N''Female'', N''Verene'', ''600103'', CAST(1 AS bit)),
    (''ac660a73-b0b7-4340-abc1-a914257a6189'', N''4 Stuart Drive'', ''12e15727-d369-49a9-8b13-bc22e9362179'', ''1998-12-02T00:00:00.0000000'', N''pretchford7@virginia.edu'', N''Female'', N''Pegeen'', ''600103'', CAST(1 AS bit)),
    (''c03bbe45-9aeb-4d24-99e0-4743016ffce9'', N''4 Parkside Point'', ''56bf46a4-02b8-4693-a0f5-0a95e2218bdc'', ''1989-08-28T00:00:00.0000000'', N''mwebsdale0@people.com.cn'', N''Female'', N''Marguerite'', ''600103'', CAST(0 AS bit)),
    (''c3abddbd-cf50-41d2-b6c4-cc7d5a750928'', N''6 Morningstar Circle'', ''14629847-905a-4a0e-9abe-80b61655c5cb'', ''1990-10-05T00:00:00.0000000'', N''ushears1@globo.com'', N''Female'', N''Ursa'', ''600103'', CAST(0 AS bit)),
    (''c6d50a47-f7e6-4482-8be0-4ddfc057fa6e'', N''73 Heath Avenue'', ''14629847-905a-4a0e-9abe-80b61655c5cb'', ''1995-02-10T00:00:00.0000000'', N''fbowsher2@howstuffworks.com'', N''Male'', N''Franchot'', ''600103'', CAST(1 AS bit)),
    (''cb035f22-e7cf-4907-bd07-91cfee5240f3'', N''484 Clarendon Court'', ''8f30bedc-47dd-4286-8950-73d8a68e5d41'', ''1997-09-25T00:00:00.0000000'', N''lwoodwing9@wix.com'', N''Male'', N''Lombard'', ''600103'', CAST(0 AS bit)),
    (''d15c6d9f-70b4-48c5-afd3-e71261f1a9be'', N''83187 Merry Drive'', ''12e15727-d369-49a9-8b13-bc22e9362179'', ''1987-01-09T00:00:00.0000000'', N''asarvar3@dropbox.com'', N''Male'', N''Angie'', ''600103'', CAST(1 AS bit))');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'PersonID', N'Address', N'CountryID', N'DateOfBirth', N'Email', N'Gender', N'PersonName', N'ZipCode', N'ReceiveNewsLetters') AND [object_id] = OBJECT_ID(N'[Persons]'))
        SET IDENTITY_INSERT [Persons] OFF;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230619172117_Initial')
BEGIN
    CREATE INDEX [IX_Persons_CountryID] ON [Persons] ([CountryID]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230619172117_Initial')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20230619172117_Initial', N'7.0.0');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230711091813_AzureReDeploy')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20230711091813_AzureReDeploy', N'7.0.0');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230711093113_PDFDisabled')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20230711093113_PDFDisabled', N'7.0.0');
END;
GO

COMMIT;
GO

