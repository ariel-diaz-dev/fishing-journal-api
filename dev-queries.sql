SELECT TOP (1000) [Id]
      ,[Email]
      ,[FirstName]
      ,[LastName]
      ,[CreatedDate]
      ,[DeletedDate]
      ,[UpdatedDate]
  FROM [FishingJournalDb].[dbo].[Accounts];


--  UPDATE [dbo].[Accounts] SET DeletedDate = NULL;