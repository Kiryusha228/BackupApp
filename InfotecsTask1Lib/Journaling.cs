
using static InfotecsTask1Lib.Enums;

namespace InfotecsTask1Lib
{
	public class Journaling
	{
		private string _journalPath;
		public Journaling(string journalPath) 
		{
			_journalPath = journalPath + "\\journal.txt";
			CreateJournal(_journalPath);
		}

		private void CreateJournal(string path)
		{
			using (File.Create(path)) { };
		}

		public void Write(string message, JournalingEvent journalingEvent)
		{
			using (var sw = File.AppendText(_journalPath))
			{
				sw.WriteLine($"{DateTime.Now.ToString()} | {journalingEvent.ToString()} | {message}");
			}
		}
	}
}
