using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Documents;
using Dapper;
using MySql.Data.MySqlClient;
using VasProductivity.Misc;

namespace VAS_Prod
{
	public class DataAccessModel
	{
		public List<PackStationModel> GetPackStations()
		{
			using (IDbConnection connection = new MySqlConnection(ConnectionHelper.CnnVall("db_sorter_prod")))
			{
				return connection.Query<PackStationModel>(
					"SELECT station_description, idstations from stations ORDER BY station_description ASC").ToList();
			}
		}
	}
}