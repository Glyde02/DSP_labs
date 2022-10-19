using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace lab1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public SeriesCollection SeriesCollection { get; set; }
        public SeriesCollection SeriesCollection2 { get; set; }
        public SeriesCollection SeriesCollection3 { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> Formatter { get; set; }




        public double pi = Math.PI;
        public const int N = 2048;

        public List<double> testSignal = new List<double>();
        public List<double> inverseSignal = new List<double>();

        public List<double> testPolySignal = new List<double>();
        public List<double> inversePolySignal1 = new List<double>();
        public List<double> inversePolySignal2 = new List<double>();

        List<double> APoly = new List<double>();
        List<double> FiPoly = new List<double>();

        public struct fourierResult
        {
            public double ASin;
            public double ACos;
            public double A;
            public double fi;
        }



        public MainWindow()
        {
            InitializeComponent();

            SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "1",
                    Values = new ChartValues<double>(),
                    PointGeometry = null
                },
                new LineSeries
                {
                    Title = "2",
                    Values = new ChartValues<double>(),
                    PointGeometry = null
                },
                new LineSeries
                {
                    Title = "3",
                    Values = new ChartValues<double>(),
                    PointGeometry = null
                }
            };
            SeriesCollection2 = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "1",
                    Values = new ChartValues<double>(),
                    PointGeometry = null
                },
                new ColumnSeries
                {
                    Title = "2",
                    Values = new ChartValues<double>(),
                    PointGeometry = null
                },
                new ColumnSeries
                {
                    Title = "3",
                    Values = new ChartValues<double>(),
                    PointGeometry = null
                }
            };

        }

        public List<double> LowFilter()
        {
            var low = new List<double>();

            for (int i = 0; i< 30; i++)
            {
                
                if (i < 15)
                    low.Add(0);
                else
                    low.Add(APoly[i]);
            }
            return low;
        }

        public List<double> HighFilter()
        {
            var high = new List<double>();

            for (int i = 0; i < 30; i++)
            {
                if (i > 15)
                    high.Add(0);
                else
                    high.Add(APoly[i]);
            }
            return high;
        }

        public List<double> LiheFilter()
        {
            var line = new List<double>();

            for (int i = 0; i < 30; i++)
            {
                if (i == 15)
                    line.Add(0);
                else
                    line.Add(APoly[i]);
            }
            return line;
        }

        private static Complex w(int k, int N)
        {
            if (k % N == 0) return 1;
            double arg = -2 * Math.PI * k / N;
            return new Complex(Math.Cos(arg), Math.Sin(arg));
        }
        public static Complex[] fft(Complex[] x)
        {
            Complex[] X;
            int N = x.Length;
            if (N == 2)
            {
                X = new Complex[2];
                X[0] = x[0] + x[1];
                X[1] = x[0] - x[1];
            }
            else
            {
                Complex[] x_even = new Complex[N / 2];
                Complex[] x_odd = new Complex[N / 2];
                for (int i = 0; i < N / 2; i++)
                {
                    x_even[i] = x[2 * i];
                    x_odd[i] = x[2 * i + 1];
                }
                Complex[] X_even = fft(x_even);
                Complex[] X_odd = fft(x_odd);
                X = new Complex[N];
                for (int i = 0; i < N / 2; i++)
                {
                    X[i] = X_even[i] + w(i, N) * X_odd[i];
                    X[i + N / 2] = X_even[i] - w(i, N) * X_odd[i];
                }
            }
            return X;
        }

        public void PolyFourier()
        {
            APoly.Clear();
            FiPoly.Clear();
            inversePolySignal1.Clear();
            inversePolySignal2.Clear();

            for (int j = 0; j <= 30; j++)
            {
                var res = DiscreteFourier(testPolySignal, j);
                APoly.Add(res.A);
                FiPoly.Add(res.fi);
            }




            for (int i = 0; i < N; i++)
            {
                double sum1 = 0;
                double sum2 = 0;
                for (int j = 0; j <= 30; j++)
                {
                    sum1 += APoly[j] * Math.Cos(2 * pi * i * j / N - FiPoly[j]);
                    sum2 += APoly[j] * Math.Cos(2 * pi * i * j / N);
                }
                inversePolySignal1.Add(sum1);
                inversePolySignal2.Add(sum2);
            }




        }

        public void TestPolyharmonicSignal()
        {
            testPolySignal.Clear();

            double [] A = new double[7] { 1, 3, 4, 10, 11, 14, 17};
            double[] fi = new double[6] { pi / 6, pi / 4, pi / 3, pi / 2, 3 * pi / 4, pi };

            var rand = new Random();

            for(int i = 0; i < N; i++)
            {
                double sum = 0;
                for(int j = 1; j <= 30; j++)
                {                   

                    double A_ = A[rand.Next(0, 6)];
                    double fi_ = fi[rand.Next(0, 5)];
                    sum += A_ * Math.Cos(2 * pi * i * j / N - fi_);
                }
                testPolySignal.Add(sum);
            }
        }
        

        public fourierResult DiscreteFourier(List<double> array, int num)
        {
            var res = new fourierResult();

            int N = array.Count;
            double sumCos = 0;
            double sumSin = 0;

            for(int i = 0; i < N; i++)
            {            
                sumCos += array[i] * Math.Cos(2 * pi * i * num / N);
                sumSin += array[i] * Math.Sin(2 * pi * i * num / N);
            }

            res.ACos = 2 * sumCos / N;
            res.ASin = 2 * sumSin / N;
            res.A = Math.Sqrt(Math.Pow(res.ACos, 2) + Math.Pow(res.ASin, 2));
            res.fi = Math.Atan2(res.ASin, res.ACos);

            return res;
        }

        public void InverseFourier(double A, double f, double fi)
        {
            inverseSignal.Clear();
            for (int n = 0; n < N; n++)
            {
                double y = A * Math.Cos(2 * pi * f * n / N - fi);
                inverseSignal.Add(y);
            }
        }

        public void TestSignal(double A, double f, double fi)
        {
            testSignal.Clear();

            for (int n = 0; n < N; n++)
            {
                double y = A * Math.Cos(2 * pi * f * n / N + fi);
                testSignal.Add(y);
            }

        }

        //дискретный Фурье
        private void btnAf_Click(object sender, RoutedEventArgs e)
        {
            DataContext = this;

            SeriesCollection[0].Values = new ChartValues<double>();
            SeriesCollection[1].Values = new ChartValues<double>();
            SeriesCollection2[0].Values = new ChartValues<double>();
            SeriesCollection2[1].Values = new ChartValues<double>();

            TestSignal(20, 10, 0);

            var result = DiscreteFourier(testSignal, 10);
            InverseFourier(result.A, 10, result.fi);

            var cv1 = new ChartValues<double>();
            cv1.AddRange(testSignal);
            var cv2 = new ChartValues<double>();
            cv2.AddRange(inverseSignal);

            var cv3 = new ChartValues<double>();
            double[] cv3_ = new double[30];
            double[] cv4_ = new double[30];
            for (int i = 0; i < 30; i++)
            {
                cv3_[i] = 0.1;
                cv4_[i] = 0.1;
            }
            cv3_[1] = result.A > 0.1 ? result.A : 0.1;
            cv3.AddRange(cv3_);
            var cv4 = new ChartValues<double>();
            cv4_[1] = result.fi > 0.1 ? result.fi : 0.1;
            cv4.AddRange(cv4_);

            SeriesCollection[0].Values = cv1;
            SeriesCollection[1].Values = cv2;
            SeriesCollection2[0].Values = cv3;
            SeriesCollection2[1].Values = cv4;

        }


        //полигармоничский сигнал
        private void btnAfi_Click(object sender, RoutedEventArgs e)
        {

            DataContext = this;

            SeriesCollection[0].Values = new ChartValues<double>();
            SeriesCollection[1].Values = new ChartValues<double>();
            SeriesCollection[2].Values = new ChartValues<double>();



            TestPolyharmonicSignal();
            PolyFourier();

            var cv1 = new ChartValues<double>();
            cv1.AddRange(APoly);
            var cv2 = new ChartValues<double>();
            cv2.AddRange(FiPoly);
            SeriesCollection2[0].Values = cv1;
            SeriesCollection2[1].Values = cv2;


            var cv3 = new ChartValues<double>();
            cv3.AddRange(testPolySignal);
            var cv4 = new ChartValues<double>();
            cv4.AddRange(inversePolySignal1);
            var cv5 = new ChartValues<double>();
            cv5.AddRange(inversePolySignal2);

            SeriesCollection[0].Values = cv3;
            SeriesCollection[1].Values = cv4;
            SeriesCollection[2].Values = cv5;
        }

        //Фильтры
        private void btnFif_Click(object sender, RoutedEventArgs e)
        {
            DataContext = this;

            SeriesCollection[0].Values = new ChartValues<double>();
            SeriesCollection[1].Values = new ChartValues<double>();
            SeriesCollection[2].Values = new ChartValues<double>();
            SeriesCollection2[0].Values = new ChartValues<double>();
            SeriesCollection2[1].Values = new ChartValues<double>();
            SeriesCollection2[2].Values = new ChartValues<double>();

            var low = LowFilter();
            var high = HighFilter();
            var line = LiheFilter();

            var lowSignal = new List<double>();
            var highSignal = new List<double>();
            var lineSignal = new List<double>();


            for (int i = 0; i < N; i++)
            {
                double sum1 = 0;
                double sum2 = 0;
                double sum3 = 0;
                for (int j = 0; j < 30; j++)
                {
                    sum1 += low[j] * Math.Cos(2 * pi * i * j / N - FiPoly[j]);
                    sum2 += high[j] * Math.Cos(2 * pi * i * j / N - FiPoly[j]);
                    sum3 += line[j] * Math.Cos(2 * pi * i * j / N - FiPoly[j]);
                }
                lowSignal.Add(sum1);
                highSignal.Add(sum2);
                lineSignal.Add(sum3);
            }

            var cv1 = new ChartValues<double>();
            cv1.AddRange(lowSignal);
            var cv2 = new ChartValues<double>();
            cv2.AddRange(highSignal);
            var cv3 = new ChartValues<double>();
            cv3.AddRange(lineSignal);

            var cv4 = new ChartValues<double>();
            var cv5 = new ChartValues<double>();
            var cv6 = new ChartValues<double>();

            double[] cv4_ = new double[30];
            double[] cv5_ = new double[30];
            double[] cv6_ = new double[30];
            for (int i = 0; i < 30; i++)
            {
                cv4_[i] = low[i] > 0.1 ? low[i] : 0.1;
                cv5_[i] = high[i] > 0.1 ? high[i] : 0.1;
                cv6_[i] = line[i] > 0.1 ? line[i] : 0.1;
            }
            
            cv4.AddRange(cv4_);
            cv5.AddRange(cv5_);
            cv6.AddRange(cv6_);
            
            SeriesCollection[0].Values = cv1;
            SeriesCollection[1].Values = cv2;
            SeriesCollection[2].Values = cv3;
            SeriesCollection2[0].Values = cv4;
            SeriesCollection2[1].Values = cv5;
            SeriesCollection2[2].Values = cv6;

        }

        //Быстрое преобразование Фурье
        private void btnPoly_Click(object sender, RoutedEventArgs e)
        {
            DataContext = this;

            SeriesCollection[0].Values = new ChartValues<double>();
            SeriesCollection[1].Values = new ChartValues<double>();
            SeriesCollection[2].Values = new ChartValues<double>();

            SeriesCollection2[0].Values = new ChartValues<double>();
            SeriesCollection2[1].Values = new ChartValues<double>();
            SeriesCollection2[2].Values = new ChartValues<double>();



            TestPolyharmonicSignal();

            Complex[] complArr = new Complex[N];
            Complex buf;
            for(int i = 0; i < N; i++)
            {
                buf = new Complex(testPolySignal[i], 0);
                complArr[i] = buf;
            }
            Complex[] inverseComplArr = new Complex[N];
            inverseComplArr = fft(complArr);

            PolyFourier();

            var cv3 = new ChartValues<double>();
            cv3.AddRange(testPolySignal);
            var cv4 = new ChartValues<double>();
            cv4.AddRange(inversePolySignal1);
            var cv5 = new ChartValues<double>();
            cv5.AddRange(inversePolySignal2);

            SeriesCollection[0].Values = cv3;
            SeriesCollection[1].Values = cv4;
            SeriesCollection[2].Values = cv5;
        }

    }
}
