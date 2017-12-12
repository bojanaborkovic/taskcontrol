using AutoMapper;
using BusinessServices.Interfaces;
using BusinessServices.Interfaces.Responses;
using BussinesService.Interfaces.Responses.User;
using DataModel;
using DataModel.UnitOfWork;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskControl;
using TaskControlDTOs;

namespace BusinessServices
{
  public class UserService : IUserService
  {

    private readonly UnitOfWork _unitOfWork = new UnitOfWork();
    internal static readonly ILog _log = log4net.LogManager.GetLogger(typeof(UserService));

    public UserService()
    {
      _unitOfWork = new UnitOfWork();
    }

    public BaseUserReturn CreateUser(UserEntity user)
    {
      BaseUserReturn ret = new BaseUserReturn();

      var config = new MapperConfiguration(cfg =>
      {
        cfg.CreateMap<UserEntity, AspNetUser>();
      });
      IMapper mapper = config.CreateMapper();
      var userToInsert = mapper.Map<AspNetUser>(user);

      _unitOfWork.UserRepository.Insert(userToInsert);
      _unitOfWork.Save();
      long Id = userToInsert.Id;
      ret.Id = userToInsert.Id;
      ret.UserName = userToInsert.UserName;

      return ret;
    }

    public BasicReturn UpdateUser(UserEntity user)
    {
      BasicReturn ret = new BasicReturn();
      try
      {
        var config = new MapperConfiguration(cfg =>
        {
          cfg.CreateMap<UserEntity, AspNetUser>();
        });
        IMapper mapper = config.CreateMapper();
        var userToUpdate = mapper.Map<AspNetUser>(user);

        _unitOfWork.UserRepository.Update(userToUpdate);
        _unitOfWork.Save();
      }
      catch (Exception ex)
      {
        _log.ErrorFormat("Error during user update : {0}", ex.Message);
        ret.ErrorMessage = ex.Message;
      }
      return ret;
    }

    public SearchUsersReturn GetAllUsers()
    {
      _log.DebugFormat("GetAllUsers invoked");
      SearchUsersReturn ret = new SearchUsersReturn();
      try
      {
        var users = _unitOfWork.UserRepository.Get(orderBy: q => q.OrderBy(d => d.UserName));
        if (users.Any())
        {
          var config = new MapperConfiguration(cfg =>
          {
            cfg.CreateMap<AspNetUser, UserEntity>();
          });

          IMapper mapper = config.CreateMapper();
          var usersMapped = mapper.Map<List<AspNetUser>, List<UserEntity>>(users.ToList());
          _log.DebugFormat("GetAllProjects finished with : {0}", users.ToString());
          ret.Users = usersMapped;
          ret.RecordCount = usersMapped.Count;
        }
      }
      catch (Exception ex)
      {
        _log.ErrorFormat("Error during fetching users... {0}", ex.Message);
        ret.ErrorMessage = ex.Message;
        ret.StatusCode = "FetchError";
      }


      return ret;
    }

    public BaseUserReturn GetUserById(long UserId)
    {
      BaseUserReturn ret = new BaseUserReturn();
      var user = _unitOfWork.UserRepository.GetByID(UserId);
      if (user != null)
      {
        var config = new MapperConfiguration(cfg =>
        {
          cfg.CreateMap<AspNetUser, UserEntity>();
        });

        IMapper mapper = config.CreateMapper();
        var userMapped = mapper.Map<AspNetUser, UserEntity>(user);

        if (user.AspNetUserRoles != null && user.AspNetUserRoles.Count > 0)
        {
          long userRoleId = user.AspNetUserRoles.FirstOrDefault().RoleId;
          var role = _unitOfWork.RoleRepository.GetByID(userRoleId);

          if (role != null)
          {
            userMapped.RoleName = role.Name;
          }
        }

        ret = MapUser(userMapped);
      }
      return ret;
    }

    public UserLanguageReturn GetUserLanguage(long userId)
    {
      UserLanguageReturn ret = new UserLanguageReturn();
      try
      {
        var user = _unitOfWork.UserRepository.GetByID(userId);
        if (user != null)
        {
          ret.LanguageCode = user.CultureCode;
          ret.UserId = user.Id;
        }
      }
      catch (Exception ex)
      {
        _log.ErrorFormat("Error during user... {0}", ex.Message);
        ret.ErrorMessage = ex.Message;
        ret.StatusCode = "Error";
      }
      return ret;
    }


    public UserLanguageReturn SetUserLanguage(UserLanguageReturn userLanguage)
    {
      UserLanguageReturn ret = new UserLanguageReturn();
      try
      {

        var user = _unitOfWork.UserRepository.GetByID(userLanguage.UserId);
        user.CultureCode = userLanguage.LanguageCode;
        _unitOfWork.UserRepository.Update(user);
        _unitOfWork.Save();

        ret.UserId = userLanguage.UserId;
        ret.LanguageCode = userLanguage.LanguageCode;
      }
      catch (Exception ex)
      {
        _log.ErrorFormat("Error during user update... {0}", ex.Message);
        ret.ErrorMessage = ex.Message;
        ret.StatusCode = "Error";
      }
      return ret;
    }

    public BaseUserReturn GetUserByUsername(string username)
    {
      BaseUserReturn ret = new BaseUserReturn();
      var user = _unitOfWork.UserRepository.Get().Where(x => x.UserName == username).FirstOrDefault();
      try
      {
        if (user != null)
        {
          var config = new MapperConfiguration(cfg =>
          {
            cfg.CreateMap<AspNetUser, UserEntity>();
          });

          IMapper mapper = config.CreateMapper();
          var usermapped = mapper.Map<AspNetUser, UserEntity>(user);

          ret = MapUser(usermapped);

        }
      }
      catch (Exception ex)
      {
        _log.ErrorFormat("Error during get user : {0}  {1}", username, ex.Message);
      }
      return ret;
    }

    private BaseUserReturn MapUser(UserEntity usermapped)
    {
      BaseUserReturn ret = new BaseUserReturn();
      ret.Id = usermapped.Id;
      ret.UserName = usermapped.UserName;
      ret.FirstName = usermapped.FirstName;
      ret.LastName = usermapped.LastName;
      ret.RoleName = usermapped.RoleName;
      ret.PhoneNumber = usermapped.PhoneNumber;
      ret.Email = usermapped.Email;
      ret.Password = usermapped.Password;
      return ret;
    }

    public SearchRolesReturn GetAllRoles()
    {
      _log.DebugFormat("GetAllRoles invoked");
      SearchRolesReturn ret = new SearchRolesReturn();
      try
      {
        var allRoles = _unitOfWork.RoleRepository.Get(orderBy: q => q.OrderBy(d => d.Id));
        if (allRoles.Any())
        {
          var config = new MapperConfiguration(cfg =>
          {
            cfg.CreateMap<AspNetRole, RoleEntity>();
          });

          IMapper mapper = config.CreateMapper();
          var rolesMapped = mapper.Map<List<AspNetRole>, List<RoleEntity>>(allRoles.ToList());
          ret.Roles = rolesMapped;
          ret.RecordCount = rolesMapped.Count;
          ret.StatusCode = "Success";

        }
      }
      catch (Exception ex)
      {
        _log.ErrorFormat("Error during fetching roles... {0}", ex.Message);
        ret.ErrorMessage = ex.Message;
        ret.StatusCode = "Error";
      }

      _log.DebugFormat("GetAllRoles finished with : {0}", ret.Roles.ToString());

      return ret;
    }

    public BasicReturn AddUserToRole(UserInRoleEntity userInRole)
    {
      BasicReturn ret = new BasicReturn();
      _log.DebugFormat("AddUserToRole invoked");
      try
      {
        var user = _unitOfWork.UserRepository.GetByID(userInRole.UserId);
        if (user == null)
        {
          throw new Exception(string.Format("User with Id {0} does not exist!", userInRole.UserId));
        }

        var role = _unitOfWork.RoleRepository.GetByID(userInRole.RoleId);
        if (role == null)
        {
          throw new Exception(string.Format("Role with Id {0} does not exist!", userInRole.RoleId));
        }

        var userInRoleEntity = new AspNetUserRole() { UserId = userInRole.UserId, RoleId = userInRole.RoleId };

        var usersRole = _unitOfWork.UserInRoleRepository.GetByID(userInRole.UserId, userInRole.RoleId);

        if (usersRole == null)
        {
          _unitOfWork.UserInRoleRepository.Insert(userInRoleEntity);
          _unitOfWork.Save();
          ret.StatusCode = "Success";
        }

        else
        {
          var message = string.Format("User {0} is already in role {1}", userInRole.UserId, userInRole.RoleId);
          _log.DebugFormat(message);
          ret.ErrorMessage = message;
          return ret;
        }


      }
      catch (Exception ex)
      {
        _log.ErrorFormat("Error during adding user to role... {0}", ex.Message);
        ret.ErrorMessage = ex.Message;
      }

      return ret;
    }

    public SearchUsersReturn SearchUsers()
    {
      _log.DebugFormat("SearchUsers invoked");
      SearchUsersReturn ret = new SearchUsersReturn();
      try
      {
        var users = _unitOfWork.SearchUsers();
        List<UserEntity> listOfUsers = new List<UserEntity>();
        listOfUsers = MapUsersList(users);
        ret.Users = listOfUsers;
        ret.RecordCount = listOfUsers.Count;

      }
      catch (Exception ex)
      {
        _log.ErrorFormat("Error search users... {0}", ex.Message);
        ret.ErrorMessage = ex.Message;
      }
      return ret;
    }

    public RoleReturn AddNewRole(RoleEntity role)
    {
      RoleReturn ret = new RoleReturn();
      _log.DebugFormat("AddUserToRole invoked");
      try
      {
        var config = new MapperConfiguration(cfg =>
        {
          cfg.CreateMap<RoleEntity, AspNetRole>();
        });
        IMapper mapper = config.CreateMapper();
        var roleToInsert = mapper.Map<AspNetRole>(role);

        _unitOfWork.RoleRepository.Insert(roleToInsert);
        _unitOfWork.Save();

        if (role.ProjectAccess != null && role.ProjectAccess.Count() > 0)
        {
          foreach (var project in role.ProjectAccess)
          {
            RoleClaimsOnProject roleClaim = new DataModel.RoleClaimsOnProject();
            roleClaim.ProjectId = project;
            roleClaim.RoleId = roleToInsert.Id;
            roleClaim.HaveAcess = true;
            _unitOfWork.RoleClaimsRepository.Insert(roleClaim);
            _unitOfWork.Save();
          }
        }

        ret.RoleId = roleToInsert.Id;
        return ret;
      }
      catch (Exception ex)
      {
        _log.ErrorFormat("Error during creating new role... {0}", ex.Message);
        ret.ErrorMessage = ex.Message;
      }

      return ret;

    }

    private List<UserEntity> MapUsersList(List<SearchUsersResult> users)
    {
      List<UserEntity> listOfUsers = new List<UserEntity>();
      foreach (var user in users)
      {
        listOfUsers.Add(new UserEntity()
        {
          Id = user.UserId,
          UserName = user.Username,
          Email = user.Email,
          FirstName = user.FirstName,
          LastName = user.LastName,
          RoleName = string.IsNullOrEmpty(user.RoleName) ? "N/A" : user.RoleName
        });
      }

      return listOfUsers;
    }


  }
}
