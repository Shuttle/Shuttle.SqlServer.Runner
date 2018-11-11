using System.IO;
using System.Text;
using System.Xml.Serialization;
using Shuttle.Core.Contract;
using Shuttle.Core.Serialization;

namespace Shuttle.SqlServer.Runner.Core
{
    [XmlType("configuration")]
    public class DefaultConfiguration : IConfiguration
    {
        public static IConfiguration Read(string configurationFilePath)
        {
            return Read(configurationFilePath, Encoding.UTF8);
        }

        public static IConfiguration Read(string configurationFilePath, Encoding encoding)
        {
            Guard.AgainstNullOrEmptyString(configurationFilePath, nameof(configurationFilePath));
            Guard.AgainstNull(encoding, nameof(encoding));

            if (!File.Exists(configurationFilePath))
            {
                throw new FileNotFoundException($"Could not find xml configuration file '{configurationFilePath}'.");
            }

            return From(File.ReadAllText(configurationFilePath));
        }

        public static IConfiguration From(string xml)
        {
            return From(xml, Encoding.UTF8);
        }

        public static IConfiguration From(string xml, Encoding encoding)
        {
            Guard.AgainstNullOrEmptyString(xml, nameof(xml));
            Guard.AgainstNull(encoding, nameof(encoding));

            var serializer = new DefaultSerializer();

            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(xml)))
            {
                return (IConfiguration)serializer.Deserialize(typeof(DefaultConfiguration), stream);
            }
        }

        [XmlAttribute("environment")]
        public string Environment { get; set; }
        [XmlAttribute("scriptFolder")]
        public string ScriptFolder { get; set; }
        [XmlAttribute("recursive")]
        public bool Recursive { get; set; }
        [XmlAttribute("sqlCmdArguments")]
        public string SqlCmdArguments { get; set; }
        [XmlAttribute("registryConnectionString")]
        public string RegistryConnectionString { get; set; }
    }
}