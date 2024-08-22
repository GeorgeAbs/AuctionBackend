using Domain.BackendResponses;
using Domain.CoreEnums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Services
{
    public interface IImageService
    {
        public Task<MethodResult<string>> SaveImageAsync(Enums.ImagePurpose imagePurpose, Stream fileStream);

        public Task<MethodResult> DeleteImageAsync(string imageFullName);
    }
}
