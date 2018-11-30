using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Documents;
using Dapper;
using IBM.Data.DB2.iSeries;
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

		public PiecesOfHdInReflex GetQuantityOfPiecesInHd(string hd)
		{
			using (IDbConnection connection = new iDB2Connection(ConnectionHelper.CnnVall("reflex")))
			{
				var result = connection.QuerySingleOrDefault<PiecesOfHdInReflex>(
					$"SELECT sum(GEQGEI) as Pieces FROM GUEPRDDB.HLGEINP WHERE GENSUP = '{hd}'");
				return result;
			}
		}
	}
}