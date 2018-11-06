using Shuttle.Core.Data;

namespace Shuttle.SqlServer.Runner.Core
{
    public interface IScriptQueryFactory
    {
        IQuery Find(string environment, string scriptFolder, string relativePath);
        IQuery Register(Script script);
    }
}