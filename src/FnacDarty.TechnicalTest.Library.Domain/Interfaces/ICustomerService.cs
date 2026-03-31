using FnacDarty.TechnicalTest.Library.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace FnacDarty.TechnicalTest.Library.Domain.Interfaces
{
    public interface ICustomerService
    {
        IReadOnlyCollection<Customer> GetAllCustomers();

        bool IsCustomerExists(int customerId);
    }
}
