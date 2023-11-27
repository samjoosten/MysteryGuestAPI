using Microsoft.EntityFrameworkCore;
using MysteryGuestAPI.DbContext;

namespace MysteryGuestAPI.Handlers.Company;

public static class GetCompaniesHandler
{
    public static async Task<IResult> GetCompanies()
    {
        List<Contexts.Company> companies;
        await using (var context = new ApplicationDbContext())
        {
            companies = await context.Companies.ToListAsync();
        }

        return TypedResults.Ok(companies);
    }
}