using System.Text;
using Shuttle.Core.Contract;

namespace Shuttle.SqlServer.Runner.Core
{
    public static class HashingServiceExtensions
    {
        public static byte[] GetHash(this IHashingService service, string text)
        {
            Guard.AgainstNull(service, nameof(service));

            return service.GetHash(text, Encoding.UTF8);
        }
    }
}