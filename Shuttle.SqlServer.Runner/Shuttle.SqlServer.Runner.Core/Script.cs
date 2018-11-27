using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Shuttle.Core.Contract;

namespace Shuttle.SqlServer.Runner.Core
{
    public class Script
    {
        private static readonly Regex ErrorExpression = new Regex(@"Msg\s*\d*,\s*Level\s*\d*,\s*State\s*\d*");
        private static readonly byte[] ErrorHash = new byte[16];

        public Script(string environment, string scriptFolder, string relativePath)
        {
            Guard.AgainstNullOrEmptyString(environment, nameof(environment));
            Guard.AgainstNullOrEmptyString(scriptFolder, nameof(scriptFolder));
            Guard.AgainstNullOrEmptyString(relativePath, nameof(relativePath));

            Environment = environment;
            ScriptFolder = scriptFolder;
            RelativePath = relativePath;

            Hash = null;
            Message = string.Empty;
        }

        public string Environment { get; }
        public string RelativePath { get; }
        public string ScriptFolder { get; }
        public DateTime? DateStarted { get; private set; }
        public DateTime? DateCompleted { get; private set; }
        public byte[] Hash { get; private set; }

        public TimeSpan GetDuration()
        {
            return !DateStarted.HasValue || !DateCompleted.HasValue ? TimeSpan.Zero : DateCompleted.Value - DateStarted.Value;
        }

        public string GetPath()
        {
            return Path.Combine(ScriptFolder, RelativePath);
        }

        public static Script Create(string environment, string scriptFolder, string path)
        {
            Guard.AgainstNullOrEmptyString(environment, nameof(environment));
            Guard.AgainstNullOrEmptyString(scriptFolder, nameof(scriptFolder));
            Guard.AgainstNullOrEmptyString(path, nameof(path));

            var relativePath = path;

            if (path.StartsWith(scriptFolder))
            {
                relativePath = path.Substring(scriptFolder.Length);
            }

            if (relativePath.StartsWith(Path.DirectorySeparatorChar.ToString()))
            {
                relativePath = relativePath.Substring(1);
            }

            return new Script(environment, scriptFolder, relativePath);
        }

        public Script Start()
        {
            OnStarted(DateStarted ?? DateTime.Now);

            Message = string.Empty;

            return this;
        }

        public Script Complete(string message)
        {
            if (DateStarted.HasValue)
            {
                OnCompleted(DateTime.Now, message);
            }

            return this;
        }

        public Script OnCompleted(DateTime dateCompleted, string message)
        {
            DateCompleted = dateCompleted;
            Message = message;

            if (HasError)
            {
                Hash = ErrorHash;
            }

            return this;
        }

        public string Status => HasError
            ? "Failed"
            : "Success";

        public bool HasError
        {
            get
            {
                var message = (Message ?? string.Empty).ToLower();

                return ErrorExpression.Match(Message ?? string.Empty).Success
                       ||
                       message.Contains("invalid filename")
                       ||
                       message.Contains("timed out")
                       ||
                       message.Contains("login failed")
                       ||
                       message.Contains("Cannot open database");
            }
        }

        public string Message { get; private set; }

        public bool HasHash(byte[] hash)
        {
            return Hash != null && 
                   hash != null && 
                   Hash.SequenceEqual(hash);
        }

        public Script Executed(byte[] hash)
        {
            Guard.AgainstNull(hash, nameof(hash));

            Hash = HasError ? ErrorHash : hash;

            return this;
        }

        public Script OnStarted(DateTime dateStarted)
        {
            DateStarted = dateStarted;

            return this;
        }
    }
}