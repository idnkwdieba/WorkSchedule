using System.IO;

namespace WorkSchedule.Shared;

public static class ProblemSimulation
{
    // Сделать генерацию и решение 10 (затем 100) задач
    // Сохранение отклонений в файл
    // В отдельной библиотеке классов? реализовать построение гистограммы
    // Вынести сравнение критериев в отдельную функцию (для большей универсальности)

    /// <summary>
    /// Прогонка генерации задач, их решение и сохранение результатов в файл.
    /// </summary>
    /// <param name="numOfCycles">Число задач для решения.</param>
    /// <exception cref="ArgumentException"></exception>
    public static void RunSimulation(int numOfCycles = 10)
    {
        // Проверка корректности параметров
        // Если число прогонок задач было меньше либо равно нуля
        if (numOfCycles <= 0)
        {
            throw new ArgumentException($"Параметр {nameof(numOfCycles)} был меньше равен нуля.");
        }

        // Путь к файлу
        string fileName = "devData.txt";
        string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            fileName);

        // Решения задач разными методами
        SolutionsCollection solutionsCollection;

        StreamWriter sw = new StreamWriter(filePath);

        // Прогонка случайных задач
        for (int i = 0; i < numOfCycles; i++)
        {
            // Создание новой коллекции решений для случайной задачи
            solutionsCollection = new SolutionsCollection(UniformDistribution.GenerateProblemUD());

            // Запись отклонения в файл
            sw.WriteLine(solutionsCollection.GetDeviation());
        }

        sw.Close();
    }
}
