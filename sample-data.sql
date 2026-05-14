USE BankingManagementDb;
GO

INSERT INTO Roles (Name) VALUES ('Admin'), ('Employee'), ('Customer');

INSERT INTO Customers (FullName, Email, PhoneNumber, Address, DateOfBirth)
VALUES
('Aarav Mehta', 'aarav@example.com', '9876543210', 'Mumbai', '1994-05-02'),
('Priya Sharma', 'priya@example.com', '9876501234', 'Delhi', '1991-08-18');

INSERT INTO Accounts (AccountNumber, AccountType, Status, Balance, CustomerId)
VALUES
('100000000001', 1, 1, 15000.00, 1),
('100000000002', 2, 1, 50000.00, 2);

INSERT INTO Transactions (AccountId, TransactionType, Amount, BalanceAfterTransaction, ReferenceNumber, Remarks)
VALUES
(1, 1, 15000.00, 15000.00, 'TXN-SEED-001', 'Opening balance'),
(2, 1, 50000.00, 50000.00, 'TXN-SEED-002', 'Opening balance');
