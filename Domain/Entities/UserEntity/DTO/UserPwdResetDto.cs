using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.UserEntity.DTO
{
    public class UserPwdResetDto
    {
        public string Email { get; set; } = string.Empty;

        public string ValidationResetPwdCode { get; set; } = string.Empty;

        public string NewPwd1 { get; set; } = string.Empty;

        public string NewPwd2 { get; set; } = string.Empty;
    }
}
