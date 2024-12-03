using auth_service.Data;
using auth_service.Models;
using auth_service.Repositories.Interfaces;

namespace auth_service.Repositories;

public class RolesRepository : GenericRepository<Role>, IRolesRepository
{
    public RolesRepository(DataContext context) : base(context)
    {
    }
}