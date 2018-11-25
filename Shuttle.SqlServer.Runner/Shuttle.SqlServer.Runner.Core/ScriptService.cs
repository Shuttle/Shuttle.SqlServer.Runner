using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using Shuttle.Core.Contract;
using Shuttle.Core.Logging;

namespace Shuttle.SqlServer.Runner.Core
{

    public class ScriptService : IScriptService
    {
        private readonly IHashingService _hashingService;
        private readonly IScriptRepository _repository;
        private readonly ILog _log;

        public ScriptService(IScriptRepository repository, IHashingService hashingService)
        {
            Guard.AgainstNull(hashingService, nameof(hashingService));
            Guard.AgainstNull(repository, nameof(repository));

            _hashingService = hashingService;
            _repository = repository;

            _log = Log.For(this);
        }

        public IEnumerable<Script> Execute(IConfiguration configuration)
        {
            Guard.AgainstNull(configuration, nameof(configuration));

            var result = new List<Script>();

            foreach (var file in Directory.GetFiles(configuration.ScriptFolder, "*.sql"))
            {
                var hash = _hashingService.GetHash(File.ReadAllText(file));

                var script = _repository.Find(configuration.Environment, configuration.ScriptFolder, file) 
                             ?? 
                             Script.Create(configuration.Environment, configuration.ScriptFolder, file);

                if (script.HasHash(hash))
                {
                    continue;
                }

                Execute(configuration, script);

                _repository.Register(script.Executed(hash));

                result.Add(script);
            }

            return result;
        }

        private void Execute(IConfiguration configuration, Script script)
        {
            var outputData = new StringBuilder();

            script.Start();

            outputData.AppendLine($"[start time]: {DateTime.Now:HH:mm:ss.ttt}");

            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    RedirectStandardInput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    WorkingDirectory = Path.GetDirectoryName(script.GetPath()) ?? string.Empty,
                    Arguments = $"-S {configuration.SqlCmdArguments} -I -i \"{Path.GetFileName(script.GetPath())}\"",
                    FileName = "sqlcmd"
                },
                EnableRaisingEvents = true
            };

            process.OutputDataReceived += delegate(object sender, DataReceivedEventArgs e)
            {
                outputData.AppendLine(e.Data);
            };

            process.Start();
            process.BeginOutputReadLine();
            var exit = process.WaitForExit(30000);
            process.CancelOutputRead();

            outputData.AppendLine(process.StandardError.ReadToEnd());

            if (!exit)
            {
                outputData.AppendLine("The call 'sqlcmd' timed out.  The operation result is inconclusive.");
            }

            script.Complete(outputData.ToString());
        }
    }
}