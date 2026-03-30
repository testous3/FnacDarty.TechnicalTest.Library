using FnacDarty.TechnicalTest.Library.Domain.Entities;

namespace FnacDarty.TechnicalTest.Library.Domain.Interfaces
{
    public interface ICustomerRepository
    {
        IReadOnlyCollection<Customer> Get();
    }
}