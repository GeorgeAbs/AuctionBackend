using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Services
{
    public interface IHttpUserService
    {
        public Guid UserId { get; }

        public bool IsAuthenticated { get; }
    }
}
