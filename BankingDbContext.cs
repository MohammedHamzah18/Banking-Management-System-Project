using BankingSystem.Domain.Common;
using BankingSystem.Domain.Entities;
using BankingSystem.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace BankingSystem.Data.Context;

public sealed class BankingDbContext : DbContext
{
    public BankingDbContext(DbContextOptions<BankingDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Account> Accounts => Set<Account>();
    public DbSet<Transaction> Transactions => Set<Transaction>();
    public DbSet<Loan> Loans => Set<Loan>();
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Role>().HasIndex(x => x.Name).IsUnique();
        modelBuilder.Entity<User>().HasIndex(x => x.Email).IsUnique();
        modelBuilder.Entity<Customer>().HasIndex(x => x.Email).IsUnique();
        modelBuilder.Entity<Account>().HasIndex(x => x.AccountNumber).IsUnique();
        modelBuilder.Entity<Transaction>().HasIndex(x => x.ReferenceNumber).IsUnique();

        modelBuilder.Entity<Account>().Property(x => x.Balance).HasPrecision(18, 2);
        modelBuilder.Entity<Transaction>().Property(x => x.Amount).HasPrecision(18, 2);
        modelBuilder.Entity<Transaction>().Property(x => x.BalanceAfterTransaction).HasPrecision(18, 2);
        modelBuilder.Entity<Loan>().Property(x => x.PrincipalAmount).HasPrecision(18, 2);
        modelBuilder.Entity<Loan>().Property(x => x.AnnualInterestRate).HasPrecision(5, 2);
        modelBuilder.Entity<Loan>().Property(x => x.EmiAmount).HasPrecision(18, 2);

        modelBuilder.Entity<User>()
            .HasOne(x => x.Role)
            .WithMany(x => x.Users)
            .HasForeignKey(x => x.RoleId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Customer>()
            .HasOne(x => x.User)
            .WithOne(x => x.Customer)
            .HasForeignKey<Customer>(x => x.UserId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Account>()
            .HasOne(x => x.Customer)
            .WithMany(x => x.Accounts)
            .HasForeignKey(x => x.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Transaction>()
            .HasOne(x => x.Account)
            .WithMany(x => x.Transactions)
            .HasForeignKey(x => x.AccountId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Transaction>()
            .HasOne(x => x.DestinationAccount)
            .WithMany()
            .HasForeignKey(x => x.DestinationAccountId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Loan>()
            .HasOne(x => x.Customer)
            .WithMany(x => x.Loans)
            .HasForeignKey(x => x.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        SeedData(modelBuilder);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
        {
            if (entry.State == EntityState.Modified)
            {
                entry.Entity.UpdatedAt = DateTime.UtcNow;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }

    private static void SeedData(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Role>().HasData(
            new Role { Id = 1, Name = "Admin", CreatedAt = new DateTime(2026, 1, 1) },
            new Role { Id = 2, Name = "Employee", CreatedAt = new DateTime(2026, 1, 1) },
            new Role { Id = 3, Name = "Customer", CreatedAt = new DateTime(2026, 1, 1) });

        modelBuilder.Entity<Customer>().HasData(
            new Customer { Id = 1, FullName = "Aarav Mehta", Email = "aarav@example.com", PhoneNumber = "9876543210", Address = "Mumbai", DateOfBirth = new DateTime(1994, 5, 2), CreatedAt = new DateTime(2026, 1, 1) },
            new Customer { Id = 2, FullName = "Priya Sharma", Email = "priya@example.com", PhoneNumber = "9876501234", Address = "Delhi", DateOfBirth = new DateTime(1991, 8, 18), CreatedAt = new DateTime(2026, 1, 1) });

        modelBuilder.Entity<Account>().HasData(
            new Account { Id = 1, AccountNumber = "100000000001", AccountType = AccountType.Savings, Status = AccountStatus.Active, Balance = 15000, CustomerId = 1, CreatedAt = new DateTime(2026, 1, 1) },
            new Account { Id = 2, AccountNumber = "100000000002", AccountType = AccountType.Current, Status = AccountStatus.Active, Balance = 50000, CustomerId = 2, CreatedAt = new DateTime(2026, 1, 1) });
    }
}
