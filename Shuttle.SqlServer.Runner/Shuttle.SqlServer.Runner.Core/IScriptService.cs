using System.Collections.Generic;

namespace Shuttle.SqlServer.Runner.Core
{
    public interface IScriptService
    {
        IEnumerable<Script> Execute(IConfiguration configuration);
    }
}