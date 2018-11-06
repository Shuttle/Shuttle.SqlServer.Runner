using Shuttle.Core.Data;

namespace Shuttle.SqlServer.Runner.Core
{
    public class ScriptQueryFactory : IScriptQueryFactory
    {
        public IQuery Find(string environment, string scriptFolder, string relativePath)
        {
            return RawQuery.Create(@"
select
    Hash,
    Status,
    Message,
    DateRegistered,
    DateModified
from
    Script
where
    Environment = @Environment
and
    ScriptFolder = @ScriptFolder
and
    RelativePath = @RelativePath
")
                .AddParameterValue(ScriptColumns.Environment, environment)
                .AddParameterValue(ScriptColumns.ScriptFolder, scriptFolder)
                .AddParameterValue(ScriptColumns.RelativePath, relativePath);
        }

        public IQuery Register(Script script)
        {
            return RawQuery.Create(@"
if exists
(
    select
        null
    from
        Script
    where
        Environment = @Environment
    and
        ScriptFolder = @ScriptFolder
    and
        RelativePath = @RelativePath        
)
    update
        Script
    set
        Hash = @Hash,
        Status = @Status,
        Message = @Message,
        DateStarted = @DateStarted,
        DateCompleted = @DateCompleted
    where
        Environment = @Environment
    and
        ScriptFolder = @ScriptFolder
    and
        RelativePath = @RelativePath        
else
    insert into Script
    (
        Environment,
        ScriptFolder,
        RelativePath,
        Hash,
        Status,
        Message,
        DateStarted,
        DateCompleted
    )
    values
    (
        @Environment,
        @ScriptFolder,
        @RelativePath,
        @Hash,
        @Status,
        @Message,
        @DateStarted,
        @DateCompleted
    )
")
                .AddParameterValue(ScriptColumns.Environment, script.Environment)
                .AddParameterValue(ScriptColumns.ScriptFolder, script.ScriptFolder)
                .AddParameterValue(ScriptColumns.RelativePath, script.RelativePath)
                .AddParameterValue(ScriptColumns.Hash, script.Hash)
                .AddParameterValue(ScriptColumns.Status, script.Status)
                .AddParameterValue(ScriptColumns.Message, script.Message)
                .AddParameterValue(ScriptColumns.DateStarted, script.DateStarted)
                .AddParameterValue(ScriptColumns.DateCompleted, script.DateCompleted);
        }
    }
}