using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using SipayApi.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace SipayApi.DataAccess.Domain;

[Table("Customer", Schema = "dbo")]
public class Customer : BaseModel
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int CustomerNumber { get; set; }
    public string Address { get; set; }
    public bool IsActive { get; set; }

    public virtual List<Account> Accounts { get; set; }
}



public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.Property(x => x.Id).IsRequired(true).UseIdentityColumn();
        builder.Property(x => x.InsertUser).IsRequired(true).HasMaxLength(50);
        builder.Property(x => x.InsertDate).IsRequired(true);

        builder.Property(x => x.FirstName).IsRequired(true).HasMaxLength(50);
        builder.Property(x => x.LastName).IsRequired(true).HasMaxLength(50);
        builder.Property(x => x.CustomerNumber).IsRequired(true).HasDefaultValue(0);
        builder.Property(x => x.IsActive).IsRequired(true).HasDefaultValue(true);
        builder.Property(x => x.Address).IsRequired(true).HasMaxLength(350);

        builder.HasIndex(x => x.CustomerNumber).IsUnique(true);

        builder.HasMany(x => x.Accounts)
            .WithOne(x => x.Customer)
            .HasForeignKey(x => x.CustomerId)
            .IsRequired(true);
    }
}