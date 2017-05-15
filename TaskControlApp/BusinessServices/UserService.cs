using AutoMapper;
using DataModel;
using DataModel.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskControlDTOs;

namespace BusinessServices
{
	public class UserService : IUserService
	{

		private readonly UnitOfWork _unitOfWork;

		public UserService()
		{
			_unitOfWork = new UnitOfWork();
		}

		public long CreateUser(UserEntity user)
		{
			throw new NotImplementedException();
		}

		public IEnumerable<UserEntity> GetAllUsers()
		{
			throw new NotImplementedException();
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
	}
}
