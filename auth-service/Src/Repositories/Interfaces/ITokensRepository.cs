using auth_service.Models;

namespace auth_service.Repositories.Interfaces;

public interface ITokensRepository : IGenericRepository<TokenBlackList>
{
    public Task<TokenBlackList?> GetTokenBlackList(string tokenRequest);
}