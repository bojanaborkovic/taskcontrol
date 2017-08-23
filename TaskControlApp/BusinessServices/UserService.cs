using AutoMapper;
using BusinessServices.Interfaces;
using BusinessServices.Interfaces.Responses;

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

		public long CreateUser(UserEntity user)
		{
			var config = new MapperConfiguration(cfg => {
				cfg.CreateMap<UserEntity, AspNetUser>();
			});
			IMapper mapper = config.CreateMapper();
			var userToInsert = mapper.Map<AspNetUser>(user);

			_unitOfWork.UserRepository.Insert(userToInsert);
			_unitOfWork.Save();
			long Id = userToInsert.Id;
			return Id;
		}

		public IEnumerable<UserEntity> GetAllUsers()
		{
            _log.DebugFormat("GetAllUsers invoked");
            try
            {
                var users = _unitOfWork.UserRepository.Get(orderBy: q => q.OrderBy(d => d.UserName));
                if (users.Any())
                {
                    var config = new MapperConfiguration(cfg => {
                        cfg.CreateMap<AspNetUser, UserEntity>();
                    });

                    IMapper mapper = config.CreateMapper();
                    var usersMapped = mapper.Map<List<AspNetUser>, List<UserEntity>>(users.ToList());
                    _log.DebugFormat("GetAllProjects finished with : {0}", users.ToString());
                    return usersMapped;
                }
            }
            catch (Exception ex)
            {
                _log.ErrorFormat("Error during fetching users... {0}", ex.Message);
            }


            return null;
        }

		public UserEntity GetUserById(long UserId)
		{
			var user = _unitOfWork.UserRepository.GetByID(UserId);
			if(user != null)
			{
				var config = new MapperConfiguration(cfg => {
					cfg.CreateMap<AspNetUser, UserEntity>();
				});

				IMapper mapper = config.CreateMapper();
				var userModel = mapper.Map<AspNetUser, UserEntity>(user);
				return userModel;
			}
			return null;
		}

		public UserEntity GetUserByUsername(string username)
		{
			var user = _unitOfWork.UserRepository.Get().Where(x => x.UserName == username).FirstOrDefault();
			try
			{
				if (user != null)
				{
					var config = new MapperConfiguration(cfg => {
						cfg.CreateMap<AspNetUser, UserEntity>();
					});

					IMapper mapper = config.CreateMapper();
					var userModel = mapper.Map<AspNetUser, UserEntity>(user);
					return userModel;
				}
			}catch(Exception ex)
			{
				_log.ErrorFormat("Error during get user : {0}  {1}", username, ex.Message);
			}
			return null;
		}

		public bool UpdateUser(UserEntity user)
		{
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
				return true;
			}catch(Exception ex)
			{
				_log.ErrorFormat("Error during user update : {0}", ex.Message);
			}
			return false;
		}

    public IEnumerable<RoleEntity> GetAllRoles()
    {
      _log.DebugFormat("GetAllRoles invoked");
      try
      {
        var allRoles = _unitOfWork.RoleRepository.Get(orderBy: q => q.OrderBy(d => d.Id));
        if (allRoles.Any())
        {
          var config = new MapperConfiguration(cfg => {
            cfg.CreateMap<AspNetRole, RoleEntity>();
          });

          IMapper mapper = config.CreateMapper();
          var rolesMapped = mapper.Map<List<AspNetRole>, List<RoleEntity>>(allRoles.ToList());
          _log.DebugFormat("GetAllRoles finished with : {0}", allRoles.ToString());
          return rolesMapped;
        }
      }
      catch (Exception ex)
      {
        _log.ErrorFormat("Error during fetching roles... {0}", ex.Message);
      }


      return null;
    }

    public BasicReturn AddUserToRole(long roleId, long userId)
    {
      BasicReturn ret = new BasicReturn();
      _log.DebugFormat("AddUserToRole invoked");
      try
      {
        var user = _unitOfWork.UserRepository.GetByID(userId);
        if(user == null)
        {
          throw new Exception(string.Format("User with Id {0} does not exist!", userId));
        }

        var role = _unitOfWork.RoleRepository.GetByID(roleId);
        if (role == null)
        {
          throw new Exception(string.Format("Role with Id {0} does not exist!", roleId));
        }

        var userInRoleEntity = new AspNetUserRole() { UserId = userId, RoleId = roleId };

        var usersRole = _unitOfWork.UserInRoleRepository.GetByID(userId, roleId);

        if(usersRole == null)
        {
          _unitOfWork.UserInRoleRepository.Insert(userInRoleEntity);
          _unitOfWork.Save();
        }

        else
        {
          var message = string.Format("User {0} is already in role {1}", userId, roleId);
          _log.DebugFormat(message);
          ret.ErrorMessage = message;
          return ret;
        }


      }
      catch (Exception ex)
      {
        _log.ErrorFormat("Error during adding user to role... {0}", ex.Message);
      }

      return ret;
    }

    public List<UserEntity> SearchUsers()
    {
      _log.DebugFormat("SearchUsers invoked");
      try
      {
        var users = _unitOfWork.SearchUsers();
        List<UserEntity> listOfUsers = new List<UserEntity>();
        listOfUsers = MapUsersList(users);
        return listOfUsers;

      }
      catch (Exception ex)
      {
        _log.ErrorFormat("Error search users... {0}", ex.Message);
      }
      return null;
    }

		public RoleReturn AddNewRole(RoleEntity role)
		{
			RoleReturn ret = new RoleReturn();
			_log.DebugFormat("AddUserToRole invoked");
			try
			{
				var config = new MapperConfiguration(cfg => {
					cfg.CreateMap<RoleEntity, AspNetRole>();
				});
				IMapper mapper = config.CreateMapper();
				var roleToInsert = mapper.Map<AspNetRole>(role);

				_unitOfWork.RoleRepository.Insert(roleToInsert);
				_unitOfWork.Save();
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
          RoleName = user.RoleName
        });
      }

      return listOfUsers;
    }

		
	}
}
