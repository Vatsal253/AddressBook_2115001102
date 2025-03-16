using ModalLayer.Modal;
using ModelLayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interface
{
    public interface IUserBL
    {
        bool RegisterUser(RegisterUser request);
        string? LoginUser(UserLogin request);
        public bool ResetPassword(string token, string newPassword);
        public bool ForgotPassword(string email);
    }
}