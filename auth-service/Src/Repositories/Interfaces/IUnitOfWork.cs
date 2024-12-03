namespace auth_service.Repositories.Interfaces;

public interface IUnitOfWork
{
    /// <summary>
    /// Gets the roles repository.
    /// </summary>
    /// <value>A Concrete class for IRolesRepository</value>
    public IRolesRepository RolesRepository { get; }

    /// <summary>
    /// Gets the users repository.
    /// </summary>
    /// <value>A Concrete class for IUsersRepository</value>
    public IUsersRepository UsersRepository { get; }
    
    
    
    public ITokensRepository TokensRepository { get; }
    
}