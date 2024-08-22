using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Services
{
    public interface IEncryptionService
    {
        public byte[] Key { get; }
        public Task<string> EncryptAsync(string stringToEncrypt, byte[] key);

        public Task<string> DecryptAsync(byte[] byteArrayToDecrypt, byte[] key);

        public string GenerateKey(string password);

    }
}
