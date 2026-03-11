using Microsoft.EntityFrameworkCore;
using SupportFlow.Customer.Business.Dtos;
using SupportFlow.Customer.Business.Interfaces;
using SupportFlow.Customer.DataAccess.Contexts;
using SupportFlow.Customer.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportFlow.Customer.Business.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly CustomerDbContext _context;

        public CompanyService(CustomerDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> CreateAsync(CreateCompanyDto dto)
        {
            var company = new Company { Name = dto.Name, TaxNumber = dto.TaxNumber, Email = dto.Email, Address = dto.Address};
            await _context.Companies.AddAsync(company);
            await _context.SaveChangesAsync();
            return company.Id;
        }

        public async Task<List<CompanyListDto>> GetAllAsync()
        {
            return await _context.Companies.Select(x=> new CompanyListDto ( x.Id,x.Name,x.Email,x.IsActive)).ToListAsync();
        }
    }
}
