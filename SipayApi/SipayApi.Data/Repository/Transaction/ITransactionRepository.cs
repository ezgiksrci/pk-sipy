using SipayApi.Data.Domain;

namespace SipayApi.Data.Repository;

public interface ITransactionRepository : IGenericRepository<Transaction>
{
    List<Transaction> GetByReference(string reference);

    List<Transaction> GetByParameter(int accountNumber, decimal? minAmountCredit, decimal? maxAmountCredit,
                                     decimal? minAmountDebit, decimal? maxAmountDebit,
                                     string description, DateTime? beginDate, DateTime? endDate,
                                     string referenceNumber);
}
