using AutoMapper;
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
        internal static readonly ILog _log = log4net.LogManager.GetLogger(typeof(UnitOfWork));
		
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
				var userModel = Mapper.Map<AspNetUser, UserEntity>(user);
				return userModel;
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
  }
}
