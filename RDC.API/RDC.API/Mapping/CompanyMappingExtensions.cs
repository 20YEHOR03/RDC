using RDC.API.Models.Company;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace RDC.API.Mapping
{
    public static class CompanyMappingExtensions
    {
        public static void ProjectFrom(this CompanyModel company, CompanyDTO changeCompanyDto)
        {
            company.Name = changeCompanyDto.Name;
            company.City = changeCompanyDto.City;
            company.Country = changeCompanyDto.Country;
        }
    }
}
