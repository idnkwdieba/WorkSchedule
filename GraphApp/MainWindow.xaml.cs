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
            string fileName = "devData.txt";
            string filePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) 
                + "\\" + fileName;

            string? line; // строка считанная из файла
            List<double> devValues = new(); // список отклонений

            // Считывание данных из файла
            StreamReader sr = new StreamReader(filePath);
            line = sr.ReadLine();
            while (line != null)
            {
                devValues.Add(double.Parse(line));
                line = sr.ReadLine();
            }
            sr.Close();

            // Данные для графиков
            double[] values = new double[11];
            double[] positions = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9,10 };
            string[] lables = { "0% - 10%", "10% - 20%", "20% - 30%", "30% - 40%", "40% - 50%",
            "50% - 60%", "60% - 70%", "70% - 80%", "80% - 90%", "90% - 100%", ">100%"};

            // Подсчёт данных для гистограмм
            foreach(double val in devValues)
            {
                int dividedVal = (int)(val / 10);

                switch (dividedVal)
                {
                    case 0:
                        values[0]++;
                        break;
                    case 1:
                        values[1]++;
                        break;
                    case 2:
                        values[2]++;
                        break;
                    case 3:
                        values[3]++;
                        break;
                    case 4:
                        values[4]++;
                        break;
                    case 5:
                        values[5]++;
                        break;
                    case 6:
                        values[6]++;
                        break;
                    case 7:
                        values[7]++;
                        break;
                    case 8:
                        values[8]++;
                        break;
                    case 9:
                        values[9]++;
                        break;
                    default:
                        values[10]++;
                        break;
                }
            }

            // добавление столбцов и подписей под ними
            HistogramGraph.Plot.AddBar(values, positions);
            HistogramGraph.Plot.XTicks(positions, lables);

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
