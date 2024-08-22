using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.UserEntity.DTO
{
    public class UserActivationDto
    {
        public string Email { get; set; } = string.Empty;

        public string ActivationCode { get; set; } = string.Empty;
    }
}
