using System.Data;
using Shuttle.Core.Data;

namespace Shuttle.SqlServer.Runner.Core
{
    public class ScriptColumns
    {
        public static readonly MappedColumn<string> Environment = new MappedColumn<string>("Environment", DbType.AnsiString);
        public static readonly MappedColumn<string> ScriptFolder = new MappedColumn<string>("ScriptFolder", DbType.AnsiString);
        public static readonly MappedColumn<string> RelativePath = new MappedColumn<string>("RelativePath", DbType.AnsiString);
        public static readonly MappedColumn<byte[]> Hash = new MappedColumn<byte[]>("Hash", DbType.Binary);
        public static readonly MappedColumn<string> Status = new MappedColumn<string>("Status", DbType.AnsiString);
        public static readonly MappedColumn<string> Message = new MappedColumn<string>("Message", DbType.AnsiString);
        public static readonly MappedColumn<System.DateTime> DateStarted = new MappedColumn<System.DateTime>("DateStarted", DbType.DateTime);
        public static readonly MappedColumn<System.DateTime> DateCompleted = new MappedColumn<System.DateTime>("DateCompleted", DbType.DateTime);
    }
}