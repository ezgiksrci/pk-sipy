using Microsoft.EntityFrameworkCore;
using SipayApi.DataAccess.Domain;

namespace SipayApi.DataAccess;

public class SimDbContext : DbContext
{
    public SimDbContext(DbContextOptions<SimDbContext> options) : base(options)
    {

    }


    // dbset
    public DbSet<Customer>Customers { get; set; }
    public DbSet<Account> Account { get; set; }
    public DbSet<Transaction> Transaction { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new AccountConfiguration());
        modelBuilder.ApplyConfiguration(new CustomerConfiguration());
        modelBuilder.ApplyConfiguration(new TransactionConfiguration());

        base.OnModelCreating(modelBuilder);
    }



}
