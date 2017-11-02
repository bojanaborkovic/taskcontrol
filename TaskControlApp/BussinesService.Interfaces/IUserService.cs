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
    BasicReturn UpdateUser(UserEntity user);
    SearchRolesReturn GetAllRoles();
    BasicReturn AddUserToRole(UserInRoleEntity userInRole);
    RoleReturn AddNewRole(RoleEntity role);
    SearchUsersReturn SearchUsers();
    UserLanguageReturn GetUserLanguage(long userId);

    UserLanguageReturn SetUserLanguage(UserLanguageReturn userLanguage);
  }
}
