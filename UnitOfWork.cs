using BankingSystem.Data.Context;
using BankingSystem.Data.Interfaces;
using BankingSystem.Domain.Entities;

namespace BankingSystem.Data.Repositories;

public sealed class UnitOfWork : IUnitOfWork
{
    private readonly BankingDbContext _dbContext;

    public UnitOfWork(BankingDbContext dbContext)
    {
        _dbContext = dbContext;
        Users = new Repository<User>(dbContext);
        Roles = new Repository<Role>(dbContext);
        Customers = new Repository<Customer>(dbContext);
        Accounts = new Repository<Account>(dbContext);
        Transactions = new Repository<Transaction>(dbContext);
        Loans = new Repository<Loan>(dbContext);
        AuditLogs = new Repository<AuditLog>(dbContext);
    }

    public IRepository<User> Users { get; }
    public IRepository<Role> Roles { get; }
    public IRepository<Customer> Customers { get; }
    public IRepository<Account> Accounts { get; }
    public IRepository<Transaction> Transactions { get; }
    public IRepository<Loan> Loans { get; }
    public IRepository<AuditLog> AuditLogs { get; }
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) => _dbContext.SaveChangesAsync(cancellationToken);
}
