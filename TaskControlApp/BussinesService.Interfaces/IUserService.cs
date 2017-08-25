using BusinessServices.Interfaces.Responses;
using BussinesService.Interfaces.Responses.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskControlDTOs;

namespace BusinessServices.Interfaces
{
  public interface IUserService
  {
    BaseUserReturn GetUserById(long UserId);
    BaseUserReturn GetUserByUsername(string username);
    SearchUsersReturn GetAllUsers();
    BaseUserReturn CreateUser(UserEntity user);
    bool UpdateUser(UserEntity user);
    IEnumerable<RoleEntity> GetAllRoles();
    BasicReturn AddUserToRole(long roleId, long userId);
    RoleReturn AddNewRole(RoleEntity role);
    List<UserEntity> SearchUsers();
  }
}
