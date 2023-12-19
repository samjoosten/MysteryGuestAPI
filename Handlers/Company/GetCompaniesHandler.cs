using Microsoft.EntityFrameworkCore;
using MysteryGuestAPI.DbContext;

namespace MysteryGuestAPI.Handlers.Company;

public static class GetCompaniesHandler
{
    public static async Task<IResult> GetCompanies(ApplicationDbContext context)
    {
        List<Contexts.Company> companies;
        companies = await context.Companies.ToListAsync();

        return TypedResults.Ok(companies);
    }
}