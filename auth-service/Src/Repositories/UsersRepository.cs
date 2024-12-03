using System.Linq.Expressions;
using auth_service.Data;
using auth_service.Models;
using auth_service.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace auth_service.Repositories.Interfaces;

public class UsersRepository : GenericRepository<User>, IUsersRepository
{
       private readonly Expression<Func<User, bool>> softDeleteFilter = x => x.DeletedAt == null;

        public UsersRepository(DataContext context) : base(context) { }

        public async Task<List<User>> GetAll()
        {
            var users = await dbSet
                            .Where(softDeleteFilter)
                            .Include(x => x.Role)
                            .ToListAsync();
            return users;
        }

        public async Task<User?> GetByEmail(string email)
        {
            var user = await dbSet
                        .Where(softDeleteFilter)
                        .Include(x => x.Role)
                        .FirstOrDefaultAsync(x => x.Email == email);
            return user;
        }
}