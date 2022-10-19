using LiveCharts;
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
        public int N = 4096;
        


        public MainWindow()
        {
            InitializeComponent();

            SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Series 1",
                    Values = new ChartValues<double>(),
                    PointGeometry = null
                }
            };

            DataContext = this;


        }

        public void HarmonicAf(object sender, RoutedEventArgs e)
        {
            int A = 5;
            int f = 1;
            double[] fi = new double[5] { pi / 4, pi / 2, 3 * pi / 4, 0, pi };

            SeriesCollection.Clear();

            for (int i = 0; i < 5; i++)
            {
                SeriesCollection.Add(new LineSeries
                {
                    Title = "Series " + (i + 1),
                    Values = new ChartValues<double>(),
                    PointGeometry = null
                });
                var temp = new double[N];
                for (int n = 0; n < N; n++)
                {
                    double y = A * Math.Sin(2 * pi * f * n / N + fi[i]);
                    SeriesCollection[i].Values.Add(y);
                }

                if (i < 4)
                {
                    
                }



            }
        }

        public void HarmonicAfi(object sender, RoutedEventArgs e)
        {
            int A = 5;
            double fi = pi;
            double[] f = new double[5] { 1, 3, 2, 4, 10 };
            SeriesCollection.Clear();

            for (int i = 0; i < 5; i++)
            {
                SeriesCollection.Add(new LineSeries
                {
                    Title = "Series " + (i + 1),
                    Values = new ChartValues<double>(),
                    PointGeometry = null
                });

                var temp = new double[N];

                for (int n = 0; n < N; n++)
                {
                    double y = A * Math.Sin(2 * pi * f[i] * n / N + fi);
                    SeriesCollection[i].Values.Add(y);
                }





            }
        }

        public void HarmonicFif(object sender, RoutedEventArgs e)
        {
            double f = 4;
            double fi = pi;
            double[] A = new double[5] { 3, 5, 10, 4, 8 };
            SeriesCollection.Clear();

            for (int i = 0; i < 5; i++)
            {
                SeriesCollection.Add(new LineSeries
                {
                    Title = "Series " + (i + 1),
                    Values = new ChartValues<double>(),
                    PointGeometry = null
                });

                var temp = new double[N];

                for (int n = 0; n < N; n++)
                {
                    double y = A[i] * Math.Sin(2 * pi * f * n / N + fi);
                    SeriesCollection[i].Values.Add(y);
                }





            }
        }


        public void Polyharmonic(object sender, RoutedEventArgs e)
        {
            double[] f = new double[5] { 1, 2, 3, 4, 5};
            double A = 5;
            double[] fi = new double[5] { pi/9, pi/4, pi/3, pi/6, 0 };
            SeriesCollection.Clear();
            SeriesCollection.Add(new LineSeries
            {
                Title = "Series 1",
                Values = new ChartValues<double>(),
                PointGeometry = null
            });

            for (int n = 0; n < N; n++)            
            {
                
                double temp = 0;

                for (int j = 0; j < 5; j++)
                {
                    temp += A * Math.Sin(2 * pi * f[j] * n / N + fi[j]);
                }

                SeriesCollection[0].Values.Add(temp);
            }

            
        }

        public void PolyharmonicWithLinear(object sender, RoutedEventArgs e)
        {
            double A = 7;
            double[] fi = { 100.0, 150.0 };
            double f = 1;
            SeriesCollection.Clear();
            SeriesCollection.Add(new LineSeries
            {
                Title = "Series 1",
                Values = new ChartValues<double>(),
                PointGeometry = null
            });

            for (var i = 0; i < N; i++)
            {
                double sum = 0;
                var ntmp = i % N;
                A += 0.2 * i / N;
                f -= 0.1 * i / N;
                fi[0] += 0.05 * i / N;
                fi[1] -= 0.05 * i / N;
                for (var j = 0; j < 2; j++)
                {
                    sum += A * Math.Sin(2 * Math.PI * f * ntmp / N + fi[j]);
                }
                SeriesCollection[0].Values.Add(sum);
            }

        }
    }
}
