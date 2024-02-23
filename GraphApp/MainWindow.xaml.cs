using ScottPlot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;

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
            List<double> egaDevValues2 = new(); // список отклонений ЭГА второй версии
            double[] deviations; // отклонения.

            // Считывание данных из файла
            StreamReader sr = new StreamReader(filePath);
            line = sr.ReadLine();
            while (line != null)
            {
                deviations = line.Split(' ').Select(n => Convert.ToDouble(n)).ToArray();

                frogsDevValues.Add(deviations[0]);
                egaDevValues.Add(deviations[1]);
                egaDevValues2.Add(deviations[2]);

                line = sr.ReadLine();
            }
            sr.Close();

            // Данные для графиков
            double[] frogsValues = new double[11];
            double[] egaValues = new double[11];
            double[] egaValues2 = new double[11];
            string[] groupNames = { "0% - 10%", "10% - 20%", "20% - 30%", "30% - 40%", "40% - 50%",
                "50% - 60%", "60% - 70%", "70% - 80%", "80% - 90%", "90% - 100%", ">100%"};
            string[] seriesNames = { "ЭГА второй версии", "ЭГА", "Прыгающие лягушки" };
            double[][] valuesBySeries = { egaValues2, egaValues, frogsValues  };

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

            // Подсчёт данных по ЭГА второй версии для гистограмм
            foreach (double val in egaDevValues2)
            {
                int dividedVal = (int)(val / 10);

                switch (dividedVal)
                {
                    case 0:
                        egaValues2[0]++;
                        break;
                    case 1:
                        egaValues2[1]++;
                        break;
                    case 2:
                        egaValues2[2]++;
                        break;
                    case 3:
                        egaValues2[3]++;
                        break;
                    case 4:
                        egaValues2[4]++;
                        break;
                    case 5:
                        egaValues2[5]++;
                        break;
                    case 6:
                        egaValues2[6]++;
                        break;
                    case 7:
                        egaValues2[7]++;
                        break;
                    case 8:
                        egaValues2[8]++;
                        break;
                    case 9:
                        egaValues2[9]++;
                        break;
                    default:
                        egaValues2[10]++;
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
