using Newtonsoft.Json;

namespace InfotecsTask1Lib
{
	public static class Configuration
	{
		public static ConfigurationDto LoadConfiguration()
		{
			ConfigurationDto configuration = new ConfigurationDto();

			using (StreamReader r = new StreamReader("configuration.cfg"))
			{
				string json = r.ReadToEnd();
				configuration = JsonConvert.DeserializeObject<ConfigurationDto>(json);
			}

			return configuration;
		}
	}
}
