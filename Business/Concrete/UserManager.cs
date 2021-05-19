using System;
using System.Collections.Generic;
using System.Text;
using Business.Abstract;
using Core.Entities.Concrete;
using DataAccess.Abstract;
using Entities.Concrete;

namespace Business.Concrete
{
    public class UserManager:IUserService
    {
        IUnitOfWork _unitOfWork;

        public UserManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public List<OperationClaim> GetClaims(User user)
        {
            return _unitOfWork.Users.GetClaims(user);
        }

        public void Add(User user)
        {
            _unitOfWork.Users.Add(user);
        }

        public User GetByMail(string email)
        {
            return _unitOfWork.Users.Get(u => u.Email == email);
        }
    }
}
