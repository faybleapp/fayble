CREATE TABLE "BackgroundTask" (
    "Id" TEXT NOT NULL CONSTRAINT "PK_BackgroundTask" PRIMARY KEY,
    "ItemId" TEXT NULL,
    "ItemName" TEXT NULL,
    "Type" TEXT NOT NULL,
    "Started" TEXT NOT NULL,
    "Status" TEXT NOT NULL,
    "StartedBy" TEXT NULL
);

CREATE TABLE "BookTag" (
    "Id" TEXT NOT NULL CONSTRAINT "PK_BookTag" PRIMARY KEY,
    "Name" TEXT NULL
);

CREATE TABLE "FileType" (
    "Id" TEXT NOT NULL CONSTRAINT "PK_FileType" PRIMARY KEY,
    "FileExtension" TEXT NULL,
    "MediaType" TEXT NOT NULL
);

CREATE TABLE "Format" (
    "Id" TEXT NOT NULL CONSTRAINT "PK_Format" PRIMARY KEY,
    "Name" TEXT NULL,
    "MediaType" TEXT NOT NULL
);

CREATE TABLE "Library" (
    "Id" TEXT NOT NULL CONSTRAINT "PK_Library" PRIMARY KEY,
    "Name" TEXT NULL,
    "Type" TEXT NOT NULL,
    "FolderPath" TEXT NULL,
    "CreatedDate" TEXT NOT NULL,
    "CreatedBy" TEXT NOT NULL,
    "LastModifiedDate" TEXT NOT NULL,
    "LastModifiedBy" TEXT NOT NULL
);

CREATE TABLE "Person" (
    "Id" TEXT NOT NULL CONSTRAINT "PK_Person" PRIMARY KEY,
    "Name" TEXT NOT NULL    
);

CREATE TABLE "Publisher" (
    "Id" TEXT NOT NULL CONSTRAINT "PK_Publisher" PRIMARY KEY,
    "Name" TEXT NULL,
    "Description" TEXT NULL,
    "MediaRoot" TEXT NULL,
    "LastMetadataUpdate" TEXT NOT NULL
);

CREATE TABLE "Role" (
    "Id" TEXT NOT NULL CONSTRAINT "PK_Role" PRIMARY KEY,
    "Name" TEXT NULL,
    "NormalizedName" TEXT NULL,
    "ConcurrencyStamp" TEXT NULL
);

CREATE TABLE "SystemConfiguration" (
    "Id" TEXT NOT NULL CONSTRAINT "PK_SystemConfiguration" PRIMARY KEY,
    "Value" TEXT NULL
);

CREATE TABLE "User" (
    "Id" TEXT NOT NULL CONSTRAINT "PK_User" PRIMARY KEY,
    "UserName" TEXT NULL,
    "NormalizedUserName" TEXT NULL,
    "Email" TEXT NULL,
    "NormalizedEmail" TEXT NULL,
    "EmailConfirmed" INTEGER NOT NULL,
    "PasswordHash" TEXT NULL,
    "SecurityStamp" TEXT NULL,
    "ConcurrencyStamp" TEXT NULL,
    "PhoneNumber" TEXT NULL,
    "PhoneNumberConfirmed" INTEGER NOT NULL,
    "TwoFactorEnabled" INTEGER NOT NULL,
    "LockoutEnd" TEXT NULL,
    "LockoutEnabled" INTEGER NOT NULL,
    "AccessFailedCount" INTEGER NOT NULL
);

CREATE TABLE "LibrarySetting" (
    "Setting" TEXT NOT NULL,
    "LibraryId" TEXT NOT NULL,
    "Value" TEXT NULL,
    CONSTRAINT "PK_LibrarySetting" PRIMARY KEY ("Setting", "LibraryId"),
    CONSTRAINT "FK_LibrarySetting_Library_LibraryId" FOREIGN KEY ("LibraryId") REFERENCES "Library" ("Id") ON DELETE CASCADE
);

CREATE TABLE "Series" (
    "Id" TEXT NOT NULL CONSTRAINT "PK_Series" PRIMARY KEY,
    "Name" TEXT NULL,
    "Volume" TEXT NULL,
    "Summary" TEXT NULL,
    "Notes" TEXT NULL,
    "Year" INTEGER NULL,
    "Rating" TEXT NOT NULL,
    "ParentSeriesId" TEXT NULL,
    "PublisherId" TEXT NULL,
    "FormatId" TEXT NULL,
    "MatchId" TEXT NULL,
    "LibraryId" TEXT NULL,
    "MediaRoot" TEXT NULL,
    "FolderPath" TEXT NULL,
    "FolderName" TEXT NULL,
    "Locked" INTEGER NOT NULL,
    "LastMetadataUpdate" TEXT NULL,
    "CreatedDate" TEXT NOT NULL,
    "CreatedBy" TEXT NOT NULL,
    "LastModifiedDate" TEXT NOT NULL,
    "LastModifiedBy" TEXT NOT NULL,
    CONSTRAINT "FK_Series_Format_FormatId" FOREIGN KEY ("FormatId") REFERENCES "Format" ("Id"),
    CONSTRAINT "FK_Series_Library_LibraryId" FOREIGN KEY ("LibraryId") REFERENCES "Library" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_Series_Publisher_PublisherId" FOREIGN KEY ("PublisherId") REFERENCES "Publisher" ("Id"),
    CONSTRAINT "FK_Series_Series_ParentSeriesId" FOREIGN KEY ("ParentSeriesId") REFERENCES "Series" ("Id")
);

CREATE TABLE "UserRoleClaim" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_UserRoleClaim" PRIMARY KEY AUTOINCREMENT,
    "RoleId" TEXT NOT NULL,
    "ClaimType" TEXT NULL,
    "ClaimValue" TEXT NULL,
    CONSTRAINT "FK_UserRoleClaim_Role_RoleId" FOREIGN KEY ("RoleId") REFERENCES "Role" ("Id") ON DELETE CASCADE
);

CREATE TABLE "RefreshToken" (
    "Id" TEXT NOT NULL CONSTRAINT "PK_RefreshToken" PRIMARY KEY,
    "Token" TEXT NULL,
    "Expiration" TEXT NOT NULL,
    "UserId" TEXT NOT NULL,
    "Revoked" INTEGER NOT NULL,
    "Created" TEXT NOT NULL,
    CONSTRAINT "FK_RefreshToken_User_UserId" FOREIGN KEY ("UserId") REFERENCES "User" ("Id") ON DELETE CASCADE
);

CREATE TABLE "UserClaim" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_UserClaim" PRIMARY KEY AUTOINCREMENT,
    "UserId" TEXT NOT NULL,
    "ClaimType" TEXT NULL,
    "ClaimValue" TEXT NULL,
    CONSTRAINT "FK_UserClaim_User_UserId" FOREIGN KEY ("UserId") REFERENCES "User" ("Id") ON DELETE CASCADE
);

CREATE TABLE "UserLogin" (
    "LoginProvider" TEXT NOT NULL,
    "ProviderKey" TEXT NOT NULL,
    "ProviderDisplayName" TEXT NULL,
    "UserId" TEXT NOT NULL,
    CONSTRAINT "PK_UserLogin" PRIMARY KEY ("LoginProvider", "ProviderKey"),
    CONSTRAINT "FK_UserLogin_User_UserId" FOREIGN KEY ("UserId") REFERENCES "User" ("Id") ON DELETE CASCADE
);

CREATE TABLE "UserRole" (
    "UserId" TEXT NOT NULL,
    "RoleId" TEXT NOT NULL,
    CONSTRAINT "PK_UserRole" PRIMARY KEY ("UserId", "RoleId"),
    CONSTRAINT "FK_UserRole_Role_RoleId" FOREIGN KEY ("RoleId") REFERENCES "Role" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_UserRole_User_UserId" FOREIGN KEY ("UserId") REFERENCES "User" ("Id") ON DELETE CASCADE
);

CREATE TABLE "UserSetting" (
    "Setting" TEXT NOT NULL,
    "UserId" TEXT NOT NULL,
    "Value" TEXT NULL,
    CONSTRAINT "PK_UserSetting" PRIMARY KEY ("Setting", "UserId"),
    CONSTRAINT "FK_UserSetting_User_UserId" FOREIGN KEY ("UserId") REFERENCES "User" ("Id") ON DELETE CASCADE
);

CREATE TABLE "UserToken" (
    "UserId" TEXT NOT NULL,
    "LoginProvider" TEXT NOT NULL,
    "Name" TEXT NOT NULL,
    "Value" TEXT NULL,
    CONSTRAINT "PK_UserToken" PRIMARY KEY ("UserId", "LoginProvider", "Name"),
    CONSTRAINT "FK_UserToken_User_UserId" FOREIGN KEY ("UserId") REFERENCES "User" ("Id") ON DELETE CASCADE
);

CREATE TABLE "Book" (
    "Id" TEXT NOT NULL CONSTRAINT "PK_Book" PRIMARY KEY,
    "Title" TEXT NULL,
    "Summary" TEXT NULL,
    "Number" TEXT NULL,
    "MediaRoot" TEXT NULL,
    "Language" TEXT NULL,
    "Rating" TEXT NOT NULL,
    "ReleaseDate" TEXT NULL,
    "CoverDate" TEXT NULL,
    "MediaType" TEXT NOT NULL,
    "SeriesId" TEXT NULL,
    "LibraryId" TEXT NULL,
    "PublisherId" TEXT NULL,
    "LastMetadataUpdate" TEXT NULL,
    "DeletedDate" TEXT NULL,
    "CreatedDate" TEXT NOT NULL,
    "CreatedBy" TEXT NOT NULL,
    "LastModifiedDate" TEXT NOT NULL,
    "LastModifiedBy" TEXT NOT NULL,
    CONSTRAINT "FK_Book_Library_LibraryId" FOREIGN KEY ("LibraryId") REFERENCES "Library" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_Book_Publisher_PublisherId" FOREIGN KEY ("PublisherId") REFERENCES "Publisher" ("Id"),
    CONSTRAINT "FK_Book_Series_SeriesId" FOREIGN KEY ("SeriesId") REFERENCES "Series" ("Id") ON DELETE CASCADE
);

CREATE TABLE "BookBookTag" (
    "BooksId" TEXT NOT NULL,
    "TagsId" TEXT NOT NULL,
    CONSTRAINT "PK_BookBookTag" PRIMARY KEY ("BooksId", "TagsId"),
    CONSTRAINT "FK_BookBookTag_Book_BooksId" FOREIGN KEY ("BooksId") REFERENCES "Book" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_BookBookTag_BookTag_TagsId" FOREIGN KEY ("TagsId") REFERENCES "BookTag" ("Id") ON DELETE CASCADE
);

CREATE TABLE "BookFile" (
    "Id" TEXT NOT NULL CONSTRAINT "PK_BookFile" PRIMARY KEY,
    "FileName" TEXT NULL,
    "FilePath" TEXT NULL,
    "FileSize" INTEGER NOT NULL,
    "FileExtension" TEXT NULL,
    "FileHash" TEXT NULL,
    "PageCount" INTEGER NOT NULL,
    "FileLastModifiedDate" TEXT NOT NULL,
    "BookId" TEXT NOT NULL,
    CONSTRAINT "FK_BookFile_Book_BookId" FOREIGN KEY ("BookId") REFERENCES "Book" ("Id") ON DELETE CASCADE
);

CREATE TABLE "BookPerson" (
    "BookId" TEXT NOT NULL,
    "PersonId" TEXT NOT NULL,
    "Role" TEXT NOT NULL,
    CONSTRAINT "PK_BookPerson" PRIMARY KEY ("BookId", "PersonId", "Role"),
    CONSTRAINT "FK_BookPerson_Book_BookId" FOREIGN KEY ("BookId") REFERENCES "Book" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_BookPerson_Person_PersonId" FOREIGN KEY ("PersonId") REFERENCES "Person" ("Id") ON DELETE CASCADE
);

CREATE TABLE "ReadHistory" (
    "Id" TEXT NOT NULL CONSTRAINT "PK_ReadHistory" PRIMARY KEY,
    "BookId" TEXT NOT NULL,
    "ReadDate" TEXT NOT NULL,
    "UserId" TEXT NOT NULL,
    CONSTRAINT "FK_ReadHistory_Book_BookId" FOREIGN KEY ("BookId") REFERENCES "Book" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_ReadHistory_User_UserId" FOREIGN KEY ("UserId") REFERENCES "User" ("Id") ON DELETE CASCADE
);

CREATE TABLE "BookFieldLocks" (
    "BookId" TEXT NOT NULL CONSTRAINT "PK_BookFieldLocks" PRIMARY KEY,
    "Title" INTEGER NOT NULL,
    "Summary" INTEGER NOT NULL,
    "Number" INTEGER NOT NULL,
    "Language" INTEGER NOT NULL,
    "Rating" INTEGER NOT NULL,
    "ReleaseDate" INTEGER NOT NULL,
    "CoverDate" INTEGER NOT NULL,
    "Tags" INTEGER NOT NULL,
    "Authors" INTEGER NOT NULL,
    "Writers" INTEGER NOT NULL,
    "Inkers" INTEGER NOT NULL,
    "Editors" INTEGER NOT NULL,
    "Pencillers" INTEGER NOT NULL,
    "Letterers" INTEGER NOT NULL,
    "Colorists" INTEGER NOT NULL,
    "CoverArtists" INTEGER NOT NULL,
    "Translators" INTEGER NOT NULL,
    "Other" INTEGER NOT NULL,
    CONSTRAINT "FK_BookFieldLocks_Book_BookId" FOREIGN KEY ("BookId") REFERENCES "Book" ("Id") ON DELETE CASCADE
);

CREATE TABLE "SeriesFieldLocks" (
    "SeriesId" TEXT NOT NULL CONSTRAINT "PK_SeriesFieldLocks" PRIMARY KEY,
    "Name" INTEGER NOT NULL,
    "Volume" INTEGER NOT NULL,
    "Summary" INTEGER NOT NULL,
    "Notes" INTEGER NOT NULL,
    "Year" INTEGER NOT NULL,
    "Rating" INTEGER NOT NULL,
    "PublisherId" INTEGER NOT NULL,
    "ParentSeriesId" INTEGER NOT NULL,
    CONSTRAINT "FK_SeriesFieldLocks_Series_SeriesId" FOREIGN KEY ("SeriesId") REFERENCES "Series" ("Id") ON DELETE CASCADE
);

CREATE INDEX "IX_Book_LibraryId" ON "Book" ("LibraryId");

CREATE INDEX "IX_Book_PublisherId" ON "Book" ("PublisherId");

CREATE INDEX "IX_Book_SeriesId" ON "Book" ("SeriesId");

CREATE INDEX "IX_BookBookTag_TagsId" ON "BookBookTag" ("TagsId");

CREATE UNIQUE INDEX "IX_BookFile_BookId" ON "BookFile" ("BookId");

CREATE INDEX "IX_BookPerson_PeopleId" ON "BookPerson" ("PeopleId");

CREATE INDEX "IX_LibrarySetting_LibraryId" ON "LibrarySetting" ("LibraryId");

CREATE INDEX "IX_ReadHistory_BookId" ON "ReadHistory" ("BookId");

CREATE INDEX "IX_ReadHistory_UserId" ON "ReadHistory" ("UserId");

CREATE INDEX "IX_RefreshToken_UserId" ON "RefreshToken" ("UserId");

CREATE UNIQUE INDEX "RoleNameIndex" ON "Role" ("NormalizedName");

CREATE INDEX "IX_Series_FormatId" ON "Series" ("FormatId");

CREATE INDEX "IX_Series_LibraryId" ON "Series" ("LibraryId");

CREATE INDEX "IX_Series_ParentSeriesId" ON "Series" ("ParentSeriesId");

CREATE INDEX "IX_Series_PublisherId" ON "Series" ("PublisherId");

CREATE INDEX "EmailIndex" ON "User" ("NormalizedEmail");

CREATE UNIQUE INDEX "UserNameIndex" ON "User" ("NormalizedUserName");

CREATE INDEX "IX_UserClaim_UserId" ON "UserClaim" ("UserId");

CREATE INDEX "IX_UserLogin_UserId" ON "UserLogin" ("UserId");

CREATE INDEX "IX_UserRole_RoleId" ON "UserRole" ("RoleId");

CREATE INDEX "IX_UserRoleClaim_RoleId" ON "UserRoleClaim" ("RoleId");

CREATE INDEX "IX_UserSetting_UserId" ON "UserSetting" ("UserId");

CREATE INDEX "IX_BookFieldLocks_Bookid" ON "BookFieldLocks" ("BookId");
