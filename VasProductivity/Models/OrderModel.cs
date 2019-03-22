using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VasProductivity.Models
{
	public class OrderModel
	{
		private string _orderName;
		public string OrderName {
			get { return _orderName; }
			set
			{
				if (value != null)
				{
					_orderName = value.Trim();
				}
			}
		}
		public List<VasModel> Vases { get; set; }
	}
}
