using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using DaretokuTools;

namespace _3MarcoConverter
{
	public partial class Form1 : Form
	{
		private decimal count = 0;

		private List<UnitDef> _UnitDef;


		public Form1()
		{
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			timer1.Interval = 1000;
			timer1.Tick += Timer1_Tick;

			try
			{
				var di = new DirectoryInfo(Application.ExecutablePath);
				using (var sr = new StreamReader(di.FullName.Replace(di.Name, @"\config\UnitDef.json")))
				{
					var s = sr.ReadToEnd();
					_UnitDef = JsonConvert.DeserializeObject<List<UnitDef>>(s);
					sr.Close();
				}

				foreach(var x in _UnitDef)
				{
					cmb_Unit.Items.Add(x.Name);
				}

				cmb_Unit.SelectedIndex = 0;
			}
			catch
			{
				MessageBox.Show("設定ファイル読み込みエラー");
				this.Close();
			}

		}

		private void cmb_Unit_SelectedIndexChanged(object sender, EventArgs e)
		{
			try
			{
				if (cmb_Unit.SelectedIndex < 0) return;

				var a = _UnitDef.Find(x => x.Name == cmb_Unit.Text);

				groupBox1.Text = a.Name + " =";
				num_3marcoD.Value = a.Day;
				num_3marcoH.Value = a.Hour;
				num_3marcoM.Value = a.Min;
				num_3marcoS.Value = a.Sec;

				button1_Click(sender, e);
			}
			catch
			{
			}
		}

		private void button1_Click(object sender, EventArgs e)
		{
			try
			{
				var resut = toSecond(num_TargetD.Value, num_TargetH.Value, num_TargetM.Value, num_TargetS.Value) 
						  / toSecond(num_3marcoD.Value, num_3marcoH.Value, num_3marcoM.Value, num_3marcoS.Value);

				lbl_Result.Text = format(is3Marco() ? (resut * 3) : resut);
			}
			catch
			{
				lbl_Result.Text = "3Marcoが未定義";
			}
		}

		private void button2_Click(object sender, EventArgs e)
		{
			timer1.Enabled = !timer1.Enabled;

			if (timer1.Enabled)
			{
				count = toSecond(num_TargetD.Value, num_TargetH.Value, num_TargetM.Value, num_TargetS.Value);
				timer1.Start();
				button2.Text = "停止";
			}
			else
			{
				timer1.Stop();
				button2.Text = "開始";
			}
		}

		private void Timer1_Tick(object sender, EventArgs e)
		{
			try
			{
				var resut = (count++) / toSecond(num_3marcoD.Value, num_3marcoH.Value, num_3marcoM.Value, num_3marcoS.Value);

				setValue(label11, $"{toTimeSpan(count)}=");
				setValue(label10, format(is3Marco() ? (resut * 3) : resut));
			}
			catch
			{
			}
		}

		private UnitDef getUnitDef()
		{
			if (cmb_Unit.SelectedIndex < 0) return null;
			return _UnitDef.Find(x => x.Name == cmb_Unit.Text);
		}

		private void setValue(Label c, string o)
		{
			if (c.InvokeRequired)
			{
				setValue(c, o);
			}else
			{
				c.Text = o;
			}
		}

		private decimal toSecond(decimal d, decimal h, decimal m, decimal s)
		{
			return (d * 86400) + (h * 3600) + (m * 60) + s;
		}

		private string toTimeSpan(decimal i)
		{
			return new TimeSpan(0, 0, (int)i).ToString();
		}

		private string format(decimal i)
		{
			var a = _UnitDef.Find(x => x.Name == cmb_Unit.Text);
			var n = i * a.Multiplier;

			if (is3Marco())
			{
				return (n.ToString("N10") == "1.5000000000") ? "半marco" : n.ToString($"N{numericUpDown1.Value}") + " Marco";
			}
			else
			{
				return n.ToString($"N{numericUpDown1.Value}") + ' ' + cmb_Unit.Text;
			}
		}

		private bool is3Marco()
		{
			return (cmb_Unit.Text == "3Marco");
		}
	}
}
