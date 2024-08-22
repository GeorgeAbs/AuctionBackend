using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface ICustomMapper<T, R>
    {
        public Task<T> MapFromDtoAsync(T mainItem, R dto);
        public Task<R> MapToDtoAsync(R dto, T mainItem);
    }
}
