namespace VAS_Prod
{
	public class HdInReflex
	{
		public int Pieces { get; set; }

		public void GetQuantityOfHd(ScannedHdRecordModel scannedHd)
		{
			scannedHd.pcs = DataAccessModel.GetQuantityOfPiecesInHd(scannedHd.HD).Pieces;
			if (scannedHd.pcs == 1)
			{
				scannedHd.pcs = DataAccessModel.GetQuantityOfComponentsInHd(scannedHd.HD).Pieces;
			}
		}
	}
}