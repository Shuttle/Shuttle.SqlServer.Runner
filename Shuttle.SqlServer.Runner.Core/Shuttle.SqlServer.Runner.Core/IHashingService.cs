using System.Text;

namespace Shuttle.SqlServer.Runner.Core
{
    public interface IHashingService
    {
        byte[] GetHash(string text, Encoding encoding);
    }
}