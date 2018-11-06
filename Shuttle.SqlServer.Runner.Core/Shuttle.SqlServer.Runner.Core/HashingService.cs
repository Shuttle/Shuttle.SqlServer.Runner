using System;
using System.Security.Cryptography;
using System.Text;
using Shuttle.Core.Contract;

namespace Shuttle.SqlServer.Runner.Core
{
    public class HashingService : IHashingService, IDisposable
    {
        private readonly MD5 _md5 = MD5.Create();

        public void Dispose()
        {
            _md5?.Dispose();
        }

        public byte[] GetHash(string text, Encoding encoding)
        {
            Guard.AgainstNullOrEmptyString(text, nameof(text));
            Guard.AgainstNull(encoding, nameof(encoding));

            return _md5.ComputeHash(encoding.GetBytes(text));
        }
    }
}