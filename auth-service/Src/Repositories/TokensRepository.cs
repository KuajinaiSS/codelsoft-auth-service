using auth_service.Data;
using auth_service.Models;
using auth_service.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace auth_service.Repositories;

public class TokensRepository : GenericRepository<TokenBlackList>, ITokensRepository
{
    public TokensRepository(DataContext context) : base(context) { }

    public async Task<TokenBlackList?> GetTokenBlackList(string tokenRequest)
    {
        var token = await dbSet.FirstOrDefaultAsync(x => x.Token == tokenRequest);
        return token;
    }
}