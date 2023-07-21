using Microsoft.EntityFrameworkCore;
using SipayApi.Data.Domain;
using System.Linq.Expressions;

namespace SipayApi.Data.Repository;

public class TransactionRepository : GenericRepository<Transaction>, ITransactionRepository
{
    private readonly SimDbContext dbContext;
    public TransactionRepository(SimDbContext dbContext) : base(dbContext)
    {
        this.dbContext = dbContext;
    }

    public List<Transaction> GetByReference(string reference)
    {
        return dbContext.Set<Transaction>().Where(x => x.ReferenceNumber == reference).ToList();
    }

    public List<Transaction> GetByParameter(int accountNumber, decimal? minAmountCredit, decimal? maxAmountCredit,
                                             decimal? minAmountDebit, decimal? maxAmountDebit,
                                             string description, DateTime? beginDate, DateTime? endDate,
                                             string referenceNumber)
    {
        // LINQ Where şartı oluşturmak için Expression kullanılıyor.
        // Verilen parametrelere göre filtrelenmiş sorgu gerçekleştiriliyor.
        Expression<Func<Transaction, bool>> expression = x =>
            (x.AccountNumber == accountNumber) &&
            (!minAmountCredit.HasValue || x.CreditAmount >= minAmountCredit) &&
            (!maxAmountCredit.HasValue || x.CreditAmount <= maxAmountCredit) &&
            (!minAmountDebit.HasValue || x.DebitAmount >= minAmountDebit) &&
            (!maxAmountDebit.HasValue || x.DebitAmount <= maxAmountDebit) &&
            (string.IsNullOrEmpty(description) || x.Description.Contains(description)) &&
            (!beginDate.HasValue || x.TransactionDate >= beginDate) &&
            (!endDate.HasValue || x.TransactionDate <= endDate) &&
            (string.IsNullOrEmpty(referenceNumber) || x.ReferenceNumber == referenceNumber);

        return GetByParameter(expression);
    }
}
