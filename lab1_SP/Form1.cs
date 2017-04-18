using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lab1_SP
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        static int n = 0;
        const int Emax = 5;
        const double a0 = Emax;
        const int ti = 116; //длительность импульсов
        const int T = 232;  //период
        

        public static void myMethod(double err)
        {
            double Pc = Math.Pow(Emax, 2) / 2;
            double Pf = 0;
            double sumAn = 0;
            double An;

            while (Pf / Pc <= err)
            {
                n++;
                Pf = Math.Pow(a0, 2) / 4;
                sumAn = 0;
                
                for (int i = 1; i <= n; i++)
                {
                    An = Math.Sqrt(Math.Pow(Emax / (i * Math.PI) * (1 - Math.Cos(i * Math.PI)), 2));
                    sumAn += Math.Pow(An, 2);
                }
                sumAn /= 2;
                Pf += sumAn;
            }            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            n = 0;
            
            chartSignal.Series[1].Points.Clear();
            chartAmplitude.Series[0].Points.Clear();
            chartAmplitude.Series[1].Points.Clear();

            double error;
            if(!double.TryParse(textBox1.Text, out error))
            {
                MessageBox.Show("Error in parse double");
                return;
            }
            myMethod(error);

            double[] arr_bn = new double[n];
            double[] arr_St = new double[ti];
            double sum = 0;

            for (int i = 0; i < n; i++)
                arr_bn[i] = Emax / ((i + 1) * Math.PI) * (1 - Math.Cos((i + 1) * Math.PI));

            for (int i = 0; i < ti; i++)
            {
                sum = 0;
                for (int j = 0; j < n; j++)
                {
                    sum += arr_bn[j] * Math.Sin((j + 1) * 2 * Math.PI / T * i);
                }
                arr_St[i] = a0 / 2 + sum;

                chartSignal.Series[1].Points.AddXY(i, arr_St[i]);
            }

            chartAmplitude.ChartAreas[0].AxisX.Minimum = 0;
            chartAmplitude.ChartAreas[0].AxisX.Maximum = n - 1;
            chartAmplitude.Series[1].Color = Color.BlueViolet;

            double[] An = new double[n];
            double[] Fi = new double[n];
            for (int i = 0; i < n; i++)
            {
                An[i] = Emax / ((i + 1) * Math.PI) * (1 - Math.Cos((i + 1) * Math.PI));
                Fi[i] = Math.PI / 2;
                chartAmplitude.Series[1].Points.AddXY(i, Fi[i]);
                chartAmplitude.Series[0].Points.AddXY(i, An[i]);
            }            

            tbN.Text = n.ToString();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            chartSignal.Series[1].Color = Color.Red;
            chartSignal.Series[0].Color = Color.Black;
            chartSignal.Series[0].BorderWidth = 2;
            chartSignal.Series[1].BorderWidth = 2;
            chartSignal.ChartAreas[0].AxisX.Minimum = -ti / 2;
            chartSignal.ChartAreas[0].AxisX.Maximum = T - ti / 2;

            chartSignal.Series[0].Points.AddXY(-ti, 0);
            chartSignal.Series[0].Points.AddXY(0, 0);
            chartSignal.Series[0].Points.AddXY(0, Emax);
            chartSignal.Series[0].Points.AddXY(ti, Emax);
            chartSignal.Series[0].Points.AddXY(ti, 0);
            chartSignal.Series[0].Points.AddXY(T, 0);
        }
    }
}
