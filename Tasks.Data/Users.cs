using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Linq.Expressions;

namespace Tasks.Data
{
    public class Users
    {
        public class UsersService
        {
            private IRepository<IUser> users;
            private IUnitOfWork uow;

            public UsersService(IRepository<User> users, IUnitOfWork uow)
            {
                this.users = users;
                this.uow = uow;
            }

            public void AddUser(User user)
            {
                this.users.Add(user);
                this.uow.Commit();
            }

            public IEnumerable<User> GetAllUsers(
                Expression<Func<User, bool>> filterExpression = null,
                Expression<Func<User, object>> orderByExpression = null,
                int? page = null,
                int? pageSize = null)
            {
                Expression<Func<User, bool>> _filterExpression = filterExpression != null ? filterExpression : (u => true);
                Expression<Func<User, object>> _orderBy = orderByExpression != null ? orderByExpression : (u => new { u.Id });
                int _page = page != null ? page.Value : 0;
                int _pageSize = pageSize != null ? pageSize.Value : this.users.Count(h => true);

                return this.users.GetAll(_filterExpression, _orderBy, _page, _pageSize);
            }

            public User GetUserById(int id)
            {
                return this.users.FirstOrDefault(u => u.Id == id);
            }

            public void DeleteUser(User user)
            {
                this.users.Delete(user);
                this.uow.Commit();
            }

            public void EditeUser(User user)
            {
                this.users.Update(user);
                this.uow.Commit();
            }
        }
    }
}
