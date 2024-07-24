using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using static InfotecsTask1Lib.Enums;

namespace InfotecsTask1Lib
{
	public class ConfigurationDto
	{
		public string[]? directoryPaths { get; set; }
		public string? newDirectoryPath { get; set; }
		public bool archivate { get; set; }

		[JsonConverter(typeof(StringEnumConverter))]
		public JournalingLevel journalingLevel { get; set; }
	}
}
