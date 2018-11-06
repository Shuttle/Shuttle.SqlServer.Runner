namespace Shuttle.SqlServer.Runner.Core
{
    public interface IScriptRepository
    {
        Script Find(string environment, string scriptFolder, string relativePath);
        void Register(Script script);
    }
}