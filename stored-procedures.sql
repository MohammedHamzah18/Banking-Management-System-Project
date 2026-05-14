USE BankingManagementDb;
GO

CREATE OR ALTER PROCEDURE dbo.GetCustomerMiniStatement
    @AccountId INT,
    @Take INT = 10
AS
BEGIN
    SET NOCOUNT ON;
    SELECT TOP (@Take)
        t.Id,
        a.AccountNumber,
        t.TransactionType,
        t.Amount,
        t.BalanceAfterTransaction,
        t.ReferenceNumber,
        t.Remarks,
        t.CreatedAt
    FROM Transactions t
    INNER JOIN Accounts a ON a.Id = t.AccountId
    WHERE (t.AccountId = @AccountId OR t.DestinationAccountId = @AccountId)
      AND t.IsDeleted = 0
    ORDER BY t.CreatedAt DESC;
END;
GO

CREATE OR ALTER PROCEDURE dbo.SearchCustomers
    @Search NVARCHAR(160) = NULL,
    @PageNumber INT = 1,
    @PageSize INT = 10
AS
BEGIN
    SET NOCOUNT ON;
    SELECT *
    FROM Customers
    WHERE IsDeleted = 0
      AND (@Search IS NULL OR FullName LIKE '%' + @Search + '%' OR Email LIKE '%' + @Search + '%' OR PhoneNumber LIKE '%' + @Search + '%')
    ORDER BY CreatedAt DESC
    OFFSET (@PageNumber - 1) * @PageSize ROWS
    FETCH NEXT @PageSize ROWS ONLY;
END;
GO
