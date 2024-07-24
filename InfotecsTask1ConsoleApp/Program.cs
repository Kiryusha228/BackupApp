using InfotecsTask1Lib;

namespace InfotecsTask1ConsoleApp
{
	internal class Program
	{
		static void Main(string[] args)
		{
			var action = new FileActions();
			action.BackUp();
		}
	}
}
