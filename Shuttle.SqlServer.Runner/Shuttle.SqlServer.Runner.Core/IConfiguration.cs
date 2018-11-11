namespace Shuttle.SqlServer.Runner.Core
{
    public interface IConfiguration
    {
        string Environment { get; }
        string ScriptFolder { get; }
        bool Recursive { get; }
        string SqlCmdArguments { get; }
        string RegistryConnectionString { get; }
    }
}