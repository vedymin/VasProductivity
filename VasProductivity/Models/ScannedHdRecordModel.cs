using System;

namespace VAS_Prod
{
	public class ScannedHdRecordModel
	{
		public int id { get; set; }
		public string HD { get; set; }
		public string pack_station { get; set; }
		public int pcs { get; set; }
		public DateTime date_time { get; set; }
	}
}