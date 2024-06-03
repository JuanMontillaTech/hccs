dotnet publish -c Release -o ./bin/Publish
EXEC MHS.sys.sp_rename N'MHS.dbo.FileManager.CreateBy' , N'CreatedBy', 'COLUMN';
EXEC MHS.sys.sp_rename N'MHS.dbo.FileManager.LastModifyBy' , N'LastModifiedBy', 'COLUMN';
EXEC MHS.sys.sp_rename N'MHS.dbo.FileManager.LastModifyDate' , N'LastModifiedDate', 'COLUMN';
EXEC MHS.sys.sp_rename N'MHS.dbo.FileManager.CreateDate' , N'CreatedDate', 'COLUMN';
CREATE INDEX IX_JournalDetails_JournalId ON JournaDetails (JournalId);


CREATE INDEX IX_JournalDetails_LedgerAccountId ON JournaDetails (LedgerAccountId);