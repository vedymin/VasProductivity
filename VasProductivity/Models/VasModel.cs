using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VasProductivity.Models
{
	public class VasModel
	{
		public int VasID { get; set; }

		private string _description;
		public string Description {
			get { return _description; }
			set {
				if (value != null)
				{
					_description = value.Trim();
				}
			}
		}

		private string _flag;
		public string Flag {
			get { return _flag; }
			set {
				if (value != null)
				{
					_flag = value.Trim();
				}
			}
		}

		private string _flagValue;
		public string FlagValue {
			get { return _flagValue; }
			set {
				if (value != null)
				{
					_flagValue = value.Trim();
				}
			}
		}
	}
}
