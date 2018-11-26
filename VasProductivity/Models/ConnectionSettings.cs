using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace VAS_Prod
{
	public static class ConnectionSettings
	{

		public static string Server { get; set; }
		public static string Database { get; set; }
		public static string UserId { get; set; }
		public static string PackStation { get; set; }
		public static string FolderPath { get; set; }
		public static string FilePath { get; set; }
		public static string Password { get; set; }
		public static string ConnString { get; set; }

		static ConnectionSettings()
		{
			Server = "10.55.137.48";
			Database = "db_sorter_prod";
			UserId = "root";
			Password = "admin";
			FolderPath = $"{Environment.GetEnvironmentVariable("LocalAppData")}\\Sorter";
			FilePath = $"{FolderPath}\\Connection_Settings.txt";
			ConnString = $"SERVER={Server};DATABASE={Database};UID={UserId};PASSWORD={Password};";
		}

		/// <summary>,
		/// Checks if folder and path exist.
		/// </summary>
		internal static void CheckForFolderAndFile()
		{
			if (File.Exists(FilePath) is false)
			{
				if (Directory.Exists(FolderPath) is false)
				{
					Directory.CreateDirectory(FolderPath);
				}
				SaveToFile();
			}

		}

		/// <summary>
		/// Use this method to read settings from the file.
		/// </summary>
		internal static void ReadFromFile()
		{

			string[] entries = File.ReadAllText(FilePath).Split(',');

			if (entries.Length == 5)
			{
				Server = entries[0];
				Database = entries[1];
				UserId = entries[2];
				Password = entries[3];
				PackStation = entries[4];
				ConnString = $"SERVER={Server};DATABASE={Database};UID={UserId};PASSWORD={Password};";
			}
			else
			{
				MessageBox.Show("Setting file have errors");
				Application.Current.Shutdown();
				return;
			}
		}


		/// <summary>
		/// Use this method to save configuration into file. This is needed to use the saved pack station in combo box.
		/// </summary>
		internal static void SaveToFile()
		{

			string conSave = $"{Server},{Database},{UserId},{Password},{PackStation}";
			File.WriteAllText(FilePath, conSave);
		}

		/// <summary>
		/// Use this method to update chosen pack station in combobox into ConnectionSettings object.
		/// </summary>
		/// <param name="comboValue">Value from combobox (pack station)</param>
		internal static void UpdatePackStation(string comboValue)
		{
			PackStation = comboValue;
		}
	}
}
