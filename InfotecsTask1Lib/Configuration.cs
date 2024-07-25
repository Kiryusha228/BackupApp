using Newtonsoft.Json;

namespace InfotecsTask1Lib
{
	public static class Configuration
	{
		public static ConfigurationDto LoadConfiguration()
		{
			using (StreamReader r = new StreamReader("configuration.cfg"))
			{
				string json = r.ReadToEnd();
				ConfigurationDto configuration = JsonConvert.DeserializeObject<ConfigurationDto>(json);
				return configuration;
			}
		}
	}
}
