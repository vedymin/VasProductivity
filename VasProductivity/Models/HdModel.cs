using System;

namespace VAS_Prod
{
	public class HdModel
	{
		public int id { get; set; }
		public string HD { get; set; }
		public string pack_station { get; set; }
		public int pcs { get; set; }
		public DateTime date_time { get; set; }

		public void GetQuantityOfHdFromReflex()
		{
			pcs = DataAccessModel.GetQuantityOfPiecesInHd(HD).pcs;
			if (pcs == 1)
			{
				pcs = DataAccessModel.GetQuantityOfComponentsInHd(HD).pcs;
			}
		}

		internal bool CheckIfHdIsAlreadyScannedInMySql()
		{
			throw new NotImplementedException();
		}
	}
}