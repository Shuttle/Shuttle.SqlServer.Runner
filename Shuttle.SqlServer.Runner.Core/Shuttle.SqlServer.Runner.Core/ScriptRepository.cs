using System.Data.Common;
using Shuttle.Core.Contract;
using Shuttle.Core.Data;

namespace Shuttle.SqlServer.Runner.Core
{
    public class ScriptRepository : IScriptRepository
    {
        private readonly IDatabaseGateway _databaseGateway;
        private readonly IScriptQueryFactory _queryFactory;

        public ScriptRepository(IDatabaseGateway databaseGateway, IScriptQueryFactory queryFactory)
        {
            Guard.AgainstNull(databaseGateway, nameof(databaseGateway));
            Guard.AgainstNull(queryFactory, nameof(queryFactory));

            _databaseGateway = databaseGateway;
            _queryFactory = queryFactory;
        }

        public Script Find(string environment, string scriptFolder, string relativePath)
        {
            var row = _databaseGateway.GetSingleRowUsing(_queryFactory.Find(environment, scriptFolder, relativePath));

            if (row == null)
            {
                return null;
            }

            return new Script(environment, scriptFolder, relativePath)
                .Executed(ScriptColumns.Hash.MapFrom(row))
                .OnStarted(ScriptColumns.DateStarted.MapFrom(row))
                .OnCompleted(ScriptColumns.DateCompleted.MapFrom(row), ScriptColumns.Message.MapFrom(row));
        }

        public void Register(Script script)
        {
            _databaseGateway.ExecuteUsing(_queryFactory.Register(script));
        }
    }
}