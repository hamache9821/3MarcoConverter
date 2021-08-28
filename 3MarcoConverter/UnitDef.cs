using Newtonsoft.Json;
using System.Collections.Generic;

namespace DaretokuTools
{
	public class UnitDef
	{
		private int _Day;
		private int _Hour;
		private int _Min;
		private int _Sec;

		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("time")]
		public string Time {
			set {
				try
				{
					var s = value.Split(':');
					_Day = int.Parse(s[0]);
					_Hour = int.Parse(s[1]);
					_Min = int.Parse(s[2]);
					_Sec = int.Parse(s[3]);
				}
				catch
				{
					_Day = 0;
					_Hour = 0;
					_Min = 0;
					_Sec = 0;
				}	
			}
		}

		public int Day { get { return _Day; } }
		public int Hour { get { return _Hour; } }
		public int Min { get { return _Min; } }
		public int Sec { get { return _Sec; } }
	}
}
