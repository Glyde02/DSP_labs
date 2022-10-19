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
        public string[] Labels { get; set; }
        public Func<double, string> Formatter { get; set; }




        public double pi = Math.PI;
        public const int N = 512;

        

        
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
                },
                new ColumnSeries
                {
                    Title = "4",
                    Values = new ChartValues<double>(),
                    PointGeometry = null
                }
            };


        }


        public void PolySpectrum(List<double> test, out List<double> APoly,out List<double> FiPoly)
        {
            APoly = new List<double>();
            FiPoly = new List<double>();

            for (int j = 0; j <= 21; j++)
            {
                var res = DiscreteFourier(test, j);
                APoly.Add(res.A);
                FiPoly.Add(res.fi);
            }

        }

        public List<double> TestPolyharmonicSignal()
        {
            var test = new List<double>();

            double [] A = new double[2] { 10, 2};

            var rand = new Random();

            for(int i = 0; i < N; i++)
            {
                double sum = A[0] * Math.Sin(2 * pi * i / N);
                for(int j = 50; j <= 70; j++)
                {                    

                    double pow = Math.Pow(-1, rand.Next(0, 2));
                    double sin = Math.Sin(2 * pi * i * j / N);
                    sum += pow*A[1] * sin;
                }
                test.Add(sum);
            }

            return test;
        }

        public List<double> SlidingAverage(List<double> src, int K)
        {
            //  0 1 2 3 4 5 {6 7 8 9 10} N 11

            int m = (K - 1) / 2;
            
            List<double> tmp = new List<double>();

            for (int i = 0; i < m; i++)
            {
                tmp.Add(src[i]);
            }

            for (int i = m; i < N - m; i++)
            {
                double sum = 0;
                for (int j = i-m; j <= i + m; j++)
                {
                    sum += src[j];
                }
                tmp.Add(sum/K);

            }

            for (int i = N - m; i < N; i++)
            {
                tmp.Add(src[i]);
            }

            return tmp;
        }

        public List<double> Parabola(List<double> src)
        {
            //  0 1 2 3 4 5 {6 7 8 9 10} N 11
            int K = 7;
            int m = (K - 1) / 2;

            List<double> tmp = new List<double>();

            for (int i = 0; i < m; i++)
            {
                tmp.Add(src[i]);
            }
            double sum = 0;
            for (int i = m; i < N - m; i++)
            {
                sum = 5 * src[i - 3] - 30 * src[i - 2] + 75 * src[i - 1] + 131 * src[i] + 75 * src[i + 1] - 30 * src[i + 2] + 5 * src[i + 3];
                tmp.Add(sum / 231);
            }
            for (int i = N - m; i < N; i++)
            {
                tmp.Add(src[i]);
            }

            return tmp;
        }

        public List<double> MedianFiltering(List<double> src, int K)
        {
            //  1 2 3 4 5 6 {7 8 9 10 11}   N 11
            //  0 1 2 3 4 5 {6 7 8 9 10}   
                 
            int m = (K - 1) / 2;

            List<double> tmp = new List<double>();
            List<double> window = new List<double>();

            for (int i = 0; i < m; i++)
            {
                tmp.Add(src[i]);
            }
            double sum = 0;
            for (int i = 0; i <= N - K; i++)
            {
                window.Clear();
                for (int j = i; j < i + K; j++)
                {
                    window.Add(src[j]);
                }
                window = Sort(window);
                tmp.Add(window[m]);                
            }
            for (int i = N - m; i < N; i++)
            {
                tmp.Add(src[i]);
            }

            return tmp;
        }

        public List<double> Sort(List<double> array)
        {
            for (int i = 0;i < array.Count; i++)
            {
                for (int j = array.Count - 1; j > 0; j--)
                {
                    if (array[j] < array[j - 1])
                    {
                        double tmp = array[j];
                        array[j] = array[j - 1];
                        array[j - 1] = tmp;
                    }
                }
            }
            return array;
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
        //Фильтры
        private void Median_Click(object sender, RoutedEventArgs e)
        {
            List<double> lst = TestPolyharmonicSignal();
            var newLst = MedianFiltering(lst, 9);

            var cv1 = new ChartValues<double>();
            cv1.AddRange(lst);
            var cv2 = new ChartValues<double>();
            cv2.AddRange(newLst);
            SeriesCollection[0].Values = cv1;
            //SeriesCollection[1].Values = cv2;

            List<double> A;
            List<double> fi;
            PolySpectrum(lst, out A, out fi);
            var cvSp11 = new ChartValues<double>();
            cvSp11.AddRange(A);
            var cvSp12 = new ChartValues<double>();
            cvSp12.AddRange(fi);
            SeriesCollection2[0].Values = cvSp11;
            //SeriesCollection2[1].Values = cvSp12;

            PolySpectrum(newLst, out A, out fi);
            var cvSp21 = new ChartValues<double>();
            cvSp21.AddRange(A);
            var cvSp22 = new ChartValues<double>();
            cvSp22.AddRange(fi);
            SeriesCollection2[2].Values = cvSp21;
            //SeriesCollection2[3].Values = cvSp22;
            DataContext = this;
        }

        private void arith_Click(object sender, RoutedEventArgs e)
        {
            List<double> lst = TestPolyharmonicSignal();
            var newLst = SlidingAverage(lst, 7);

            var cv1 = new ChartValues<double>();
            cv1.AddRange(lst);
            var cv2 = new ChartValues<double>();
            cv2.AddRange(newLst);
            SeriesCollection[0].Values = cv1;
            SeriesCollection[1].Values = cv2;

            List<double> A;
            List<double> fi;
            PolySpectrum(lst,out A,out fi);
            var cvSp11 = new ChartValues<double>();
            cvSp11.AddRange(A);
            var cvSp12 = new ChartValues<double>();
            cvSp12.AddRange(fi);
            SeriesCollection2[0].Values = cvSp11;
            //SeriesCollection2[1].Values = cvSp12;

            PolySpectrum(newLst, out A, out fi);
            var cvSp21 = new ChartValues<double>();
            cvSp21.AddRange(A);
            var cvSp22 = new ChartValues<double>();
            cvSp22.AddRange(fi);
            SeriesCollection2[2].Values = cvSp21;
            //SeriesCollection2[3].Values = cvSp22;
            DataContext = this;

        }

        private void parabola_Click(object sender, RoutedEventArgs e)
        {         

            List<double> lst = TestPolyharmonicSignal();
            var newLst = Parabola(lst);

            var cv1 = new ChartValues<double>();
            cv1.AddRange(lst);
            var cv2 = new ChartValues<double>();
            cv2.AddRange(newLst);
            SeriesCollection[0].Values = cv1;
            SeriesCollection[1].Values = cv2;

            List<double> A;
            List<double> fi;
            PolySpectrum(lst, out A, out fi);
            var cvSp11 = new ChartValues<double>();
            cvSp11.AddRange(A);
            var cvSp12 = new ChartValues<double>();
            cvSp12.AddRange(fi);
            SeriesCollection2[0].Values = cvSp11;
            //SeriesCollection2[1].Values = cvSp12;

            PolySpectrum(newLst, out A, out fi);
            var cvSp21 = new ChartValues<double>();
            cvSp21.AddRange(A);
            var cvSp22 = new ChartValues<double>();
            cvSp22.AddRange(fi);
            SeriesCollection2[2].Values = cvSp21;
            //SeriesCollection2[3].Values = cvSp22;
            DataContext = this;

        }
    }
}
