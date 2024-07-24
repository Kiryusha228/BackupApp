using System.IO;
using System.IO.Compression;

namespace InfotecsTask1Lib
{
	public class FileActions
	{
		private ConfigurationDto _configuration { get; set; }
		private Journaling _journal;
		private bool _journalingError;

		public FileActions() 
		{
			_configuration = Configuration.LoadConfiguration();
		}

		public void BackUp()
		{
			_journalingError = false;
			_configuration.newDirectoryPath = SetNewDirectoryPath(_configuration.newDirectoryPath);
			CreateDirectory(_configuration.newDirectoryPath);

			if (_configuration.journalingLevel != Enums.JournalingLevel.None)
			{
				_journal = new Journaling(_configuration.newDirectoryPath);
				_journal.Write("Журнал создан", Enums.JournalingEvent.Debug);
				_journal.Write($"Началось выполнение резервного копирования", Enums.JournalingEvent.Info);
			}

			CopyDirectories(_configuration.directoryPaths, _configuration.newDirectoryPath);


			if (_configuration.archivate)
			{
				CreateZip(_configuration.newDirectoryPath);
			}

			if (_configuration.journalingLevel != Enums.JournalingLevel.None)
			{
				if (_journalingError)
				{
					_journal.Write($"Резервное копирование не выполнено", Enums.JournalingEvent.Error);
				}
				else
				{
					_journal.Write($"Резервное копирование выполнено", Enums.JournalingEvent.Info);
				}
			}
		}

		private string SetNewDirectoryPath(string directoryPath)
		{
			return directoryPath + "\\" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
		}

		private void CreateDirectory(string directoryPath)
		{
			Directory.CreateDirectory(directoryPath);
		}

		private void CopyDirectories(string[] copyPaths, string directoryPath)
		{
			foreach (var path in copyPaths)
			{
				var dirName = path.Split("\\").Last();
				var newDirectoryPath = directoryPath + "\\" + dirName;

				try
				{
					Directory.CreateDirectory(newDirectoryPath);
					throw new UnauthorizedAccessException();
				}
				catch (UnauthorizedAccessException)
				{
					if (_configuration.journalingLevel != Enums.JournalingLevel.None)
					{
						_journal.Write($"Возникла ошибка доступа при создании директории по пути {newDirectoryPath}, резервное копирование остановлено", Enums.JournalingEvent.Error);
						_journalingError = true;
					}
				}
				catch (Exception)
				{
					if (_configuration.journalingLevel != Enums.JournalingLevel.None)
					{
						_journal.Write($"Возникла ошибка при создании директории по пути {newDirectoryPath}, резервное копирование остановлено", Enums.JournalingEvent.Error);
						_journalingError = true;
					}
					return;
				}

				if (_configuration.journalingLevel == Enums.JournalingLevel.High)
				{
					_journal.Write($"Создана директория по пути {newDirectoryPath}", Enums.JournalingEvent.Info);
				}

				CopyFiles(path, newDirectoryPath);
			}
		}

		private void CopyFiles(string copyPath, string directoryPath)
		{
			var directories = Directory.GetDirectories(copyPath);
			var files = Directory.GetFiles(copyPath);

			foreach (var file in files) 
			{
				var newFilePath = directoryPath + "\\" + file.Split("\\").Last();

				try
				{
					File.Copy(file, newFilePath);
					
				}
				catch (UnauthorizedAccessException)
				{
					if (_configuration.journalingLevel != Enums.JournalingLevel.None)
					{
						_journal.Write($"Возникла ошибка доступа при создании файла по пути {newFilePath}, резервное копирование остановлено", Enums.JournalingEvent.Error);
						_journalingError = true;
					}
					return;
				}
				catch (Exception)
				{
					if (_configuration.journalingLevel != Enums.JournalingLevel.None)
					{
						_journal.Write($"Возникла ошибка при создании файла по пути {newFilePath}, резервное копирование остановлено", Enums.JournalingEvent.Error);
						_journalingError = true;
					}
					return;
				}

				if (_configuration.journalingLevel == Enums.JournalingLevel.High)
				{
					_journal.Write($"Создан файл по пути {newFilePath}", Enums.JournalingEvent.Info);
				}
			}

			CopyDirectories(directories, directoryPath);
		}

		private void CreateZip(string path)
		{
			try
			{
				ZipFile.CreateFromDirectory(path, path + ".zip");
				Directory.Delete(path, true);
				_journal.Write($"Создан архив {path}.zip", Enums.JournalingEvent.Info);
			}
			catch (Exception)
			{
				if (_configuration.journalingLevel != Enums.JournalingLevel.None)
				{
					_journal.Write($"Возникла ошибка при создании архива {path}.zip", Enums.JournalingEvent.Info);
					_journalingError = true;
				}
			}
		}
	}
}
