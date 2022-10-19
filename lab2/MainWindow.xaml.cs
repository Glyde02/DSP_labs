using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public string[] Labels { get; set; }
        public Func<double, string> Formatter { get; set; }




        public double pi = Math.PI;
        public const int N = 2048;
        public int K = 3*N / 4;



        public MainWindow()
        {
            InitializeComponent();

            SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "СКО 1",
                    Values = new ChartValues<ObservablePoint>(),
                    PointGeometry = null
                },
                new LineSeries
                {
                    Title = "СКО 2",
                    Values = new ChartValues<ObservablePoint>(),
                    PointGeometry = null
                },
                new LineSeries
                {
                    Title = "Амплитуда",
                    Values = new ChartValues<ObservablePoint>(),
                    PointGeometry = null
                },
                new LineSeries
                {
                    Title = "(2)СКО 1",
                    Values = new ChartValues<ObservablePoint>(),
                    PointGeometry = null
                },
                new LineSeries
                {
                    Title = "(2)СКО 2",
                    Values = new ChartValues<ObservablePoint>(),
                    PointGeometry = null
                },
                new LineSeries
                {
                    Title = "(2)Амплитуда",
                    Values = new ChartValues<ObservablePoint>(),
                    PointGeometry = null
                }
            };

            DataContext = this;

            HarmonicAf(this, null);
            HarmonicAfi(this, null);

        }



        public void HarmonicAf(object sender, RoutedEventArgs e)
        {
            SeriesCollection[0].Values.Clear();
            SeriesCollection[1].Values.Clear();
            SeriesCollection[2].Values.Clear();

            ChartValues<ObservablePoint> List1 = new ChartValues<ObservablePoint>();
            ChartValues<ObservablePoint> List2 = new ChartValues<ObservablePoint>();
            ChartValues<ObservablePoint> List3 = new ChartValues<ObservablePoint>();

            int A = 1;
            int f = 1;
            double fi = 0;

            for (int M = K; M <= 2 * N; M++)
            {
                double sumX = 0;
                double sumX2 = 0;

                List<double> ASin = new List<double>();
                List<double> ACos = new List<double>();
                double sumSin = 0;
                double sumCos = 0;

                double A_ = 0;


                for (int n = 0; n <= M; n++)
                {
                    double y = A * Math.Sin(2 * pi * f * n / N + fi);
                    sumX += y;
                    sumX2 += Math.Pow(y, 2);

                    sumCos += y * Math.Cos(2 * pi * n / (M+1));
                    sumSin += y * Math.Sin(2 * pi * n / (M+1));

                }

                sumCos = 2 * sumCos / (M+1);
                sumSin = 2 * sumSin / (M+1);
                A_ = Math.Sqrt(Math.Pow(sumCos, 2) + Math.Pow(sumSin, 2));


                double xE = Math.Sqrt((sumX2) / (M + 1));
                double xE2 = Math.Sqrt(((sumX2) / (M + 1)) - Math.Pow(sumX / (M + 1), 2));

                List1.Add(new ObservablePoint
                {
                    X = M,
                    Y = 0.707 - xE
                });
                List2.Add(new ObservablePoint
                {
                    X = M,
                    Y = 0.707 - xE2
                });
                List3.Add(new ObservablePoint
                {
                    X = M,
                    Y = 1 - A_
                });



            }

            SeriesCollection[0].Values.AddRange(List1);
            SeriesCollection[1].Values.AddRange(List2);
            SeriesCollection[2].Values.AddRange(List3);


        }

        public void HarmonicAfi(object sender, RoutedEventArgs e)
        {


            ChartValues<ObservablePoint> List1 = new ChartValues<ObservablePoint>();
            ChartValues<ObservablePoint> List2 = new ChartValues<ObservablePoint>();
            ChartValues<ObservablePoint> List3 = new ChartValues<ObservablePoint>();

            int A = 1;
            int f = 1;
            double fi = pi/8;

            for (int M = K; M < 2 * N; M++)
            {
                double sumX = 0;
                double sumX2 = 0;


                double sumSin = 0;
                double sumCos = 0;

                double A_ = 0;


                for (int n = 0; n < M; n++)
                {
                    double y = A * Math.Sin(2 * pi * f * n / N + fi);
                    sumX += y;
                    sumX2 += Math.Pow(y, 2);

                    sumCos += y * Math.Cos(2 * pi * n / M);
                    sumSin += y * Math.Sin(2 * pi * n / M);


                }

                sumCos = 2 * sumCos / M;
                sumSin = 2 * sumSin / M;
                A_ = Math.Sqrt(Math.Pow(sumCos, 2) + Math.Pow(sumSin, 2));


                double xE = Math.Sqrt((sumX2) / (M + 1));
                double xE2 = Math.Sqrt(((sumX2) / (M + 1)) - Math.Pow(sumX / (M + 1), 2));

                List1.Add(new ObservablePoint
                {
                    X = M,
                    Y = 0.707 - xE
                });
                List2.Add(new ObservablePoint
                {
                    X = M,
                    Y = 0.707 - xE2
                });
                List3.Add(new ObservablePoint
                {
                    X = M,
                    Y = 1 - A_
                });



            }

            SeriesCollection[3].Values.AddRange(List1);
            SeriesCollection[4].Values.AddRange(List2);
            SeriesCollection[5].Values.AddRange(List3);

        }

    }
}
