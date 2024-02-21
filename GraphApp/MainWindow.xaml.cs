using ScottPlot;
using ScottPlot.Plottable;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace GraphApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {

            InitializeComponent();

            // Путь к файлу для чтения данных
            string fileName = "deviations.txt";
            string filePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
                + "\\" + fileName;

            string? line; // строка считанная из файла
            List<double> frogsDevValues = new(); // список отклонений алгоритма лягушек
            List<double> egaDevValues = new(); // список отклонений ЭГА

            // Считывание данных из файла
            StreamReader sr = new StreamReader(filePath);
            line = sr.ReadLine();
            while (line != null)
            {
                frogsDevValues.Add(double.Parse(line.Substring(0, line.IndexOf(' '))));
                egaDevValues.Add(double.Parse(line.Substring(line.IndexOf(' ') + 1)));
                line = sr.ReadLine();
            }
            sr.Close();

            // Данные для графиков
            double[] frogsValues = new double[11];
            double[] egaValues = new double[11];
            string[] groupNames = { "0% - 10%", "10% - 20%", "20% - 30%", "30% - 40%", "40% - 50%",
                "50% - 60%", "60% - 70%", "70% - 80%", "80% - 90%", "90% - 100%", ">100%"};
            string[]seriesNames = { "Прыгающие лягушки", "ЭГА" };
            double[][] valuesBySeries = { frogsValues, egaValues };

            // Подсчёт данных по лягушкам для гистограмм
            foreach (double val in frogsDevValues)
            {
                int dividedVal = (int)(val / 10);

                switch (dividedVal)
                {
                    case 0:
                        frogsValues[0]++;
                        break;
                    case 1:
                        frogsValues[1]++;
                        break;
                    case 2:
                        frogsValues[2]++;
                        break;
                    case 3:
                        frogsValues[3]++;
                        break;
                    case 4:
                        frogsValues[4]++;
                        break;
                    case 5:
                        frogsValues[5]++;
                        break;
                    case 6:
                        frogsValues[6]++;
                        break;
                    case 7:
                        frogsValues[7]++;
                        break;
                    case 8:
                        frogsValues[8]++;
                        break;
                    case 9:
                        frogsValues[9]++;
                        break;
                    default:
                        frogsValues[10]++;
                        break;
                }
            }

            // Подсчёт данных по ЭГА для гистограмм
            foreach (double val in egaDevValues)
            {
                int dividedVal = (int)(val / 10);

                switch (dividedVal)
                {
                    case 0:
                        egaValues[0]++;
                        break;
                    case 1:
                        egaValues[1]++;
                        break;
                    case 2:
                        egaValues[2]++;
                        break;
                    case 3:
                        egaValues[3]++;
                        break;
                    case 4:
                        egaValues[4]++;
                        break;
                    case 5:
                        egaValues[5]++;
                        break;
                    case 6:
                        egaValues[6]++;
                        break;
                    case 7:
                        egaValues[7]++;
                        break;
                    case 8:
                        egaValues[8]++;
                        break;
                    case 9:
                        egaValues[9]++;
                        break;
                    default:
                        egaValues[10]++;
                        break;
                }
            }

            // добавление столбцов и подписей под ними
            HistogramGraph.Plot.AddBarGroups(groupNames, seriesNames, valuesBySeries, null);

            // Добавить легенду.
            HistogramGraph.Plot.Legend(location: Alignment.UpperRight);

            // поправить минимальное значение по оси y
            HistogramGraph.Plot.SetAxisLimits(yMin: 0);

            // добавить название графика, подписи осей координат
            HistogramGraph.Plot.Title("График отклонений");
            HistogramGraph.Plot.YLabel("Количество задач");
            HistogramGraph.Plot.XLabel("Процент отклонения");

            // обновить график
            HistogramGraph.Refresh();
        }
    }
}
