using FnacDarty.TechnicalTest.Library.Domain.Entities;
using FnacDarty.TechnicalTest.Library.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace FnacDarty.TechnicalTest.Library.Application.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }
        public IReadOnlyCollection<Customer> GetAllCustomers()
        {
            return _customerRepository.Get();
        }

        public bool IsCustomerExists(int customerId)
        {
            var listCLients = GetAllCustomers();
            return listCLients.Any(cl => cl.Id == customerId);

        }
    }
}
