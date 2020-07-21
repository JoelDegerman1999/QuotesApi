using QuotesApi.Models;
using System.Collections.Generic;

namespace QuotesApi.Data
{
    public interface ICompanyRepository
    {
        IEnumerable<Company> GetCompanies();
        Company GetCompany(int id);
        void InsertCompany(Company company);
        void DeleteCompany(int id);
        void UpdateCompany(Company company);
        void Save();
    }
}
