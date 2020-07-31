using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _3MarcoConverter
{
	public partial class Form1 : Form
	{
		private decimal count = 0;

		public Form1()
		{
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			timer1.Interval = 1000;
			timer1.Tick += Timer1_Tick;
		}

		private void button1_Click(object sender, EventArgs e)
		{
			try
			{
				var tmp = 3 / toSecond(num_3marcoD.Value, num_3marcoH.Value, num_3marcoM.Value, num_3marcoS.Value);
				var resut = toSecond(num_TargetD.Value, num_TargetH.Value, num_TargetM.Value, num_TargetS.Value) * tmp;

				lbl_Result.Text = format(resut);
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
				var tmp = 3 / toSecond(num_3marcoD.Value, num_3marcoH.Value, num_3marcoM.Value, num_3marcoS.Value);
				var resut = (count++) * tmp;

				setValue(label11, $"{toTimeSpan(count)}=");
				setValue(label10, format(resut));
			}
			catch
			{
			}
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
			return (i.ToString("N10") == "1.5000000000") ? "半marco" : i.ToString($"N{numericUpDown1.Value}") + "Marco";
		}
	}
}
