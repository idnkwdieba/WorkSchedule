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
    public static void RunSimulation()
    {
        // Путь к файлу с данными задач.
        string sourceFileName = "problems.txt";
        string sourceFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            sourceFileName);

        StreamReader sr = new(sourceFilePath);

        // Путь к файлу для записи отклонений.
        string destFileName = "deviations.txt";
        string destFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            destFileName);

        StreamWriter sw = new(destFilePath);

        int ctr = 0;

        ProblemParams problemParams;
        SolutionsCollection solutionsCollection;

        // Последовательное чтение условий задачи из файла, решение и запись отклонений.
        while (!sr.EndOfStream)
        {
            Console.WriteLine("Задача " + ctr++);

            // Чтение данных о задаче.
            int numOfTasks = Convert.ToInt32(sr.ReadLine());
            int[] taskRequiredTime = sr.ReadLine()!.Split(' ').Select(n => Convert.ToInt32(n)).ToArray();
            int[] taskArrivalTime = sr.ReadLine()!.Split(' ').Select(n => Convert.ToInt32(n)).ToArray();
            int[] taskCompletionGoal = sr.ReadLine()!.Split(' ').Select(n => Convert.ToInt32(n)).ToArray();
            int[] taskPenalty = sr.ReadLine()!.Split(' ').Select(n => Convert.ToInt32(n)).ToArray();
            int fitnessValue = Convert.ToInt32(sr.ReadLine()!);

            // Создание новой коллекции решений для случайной задачи
            problemParams = new ProblemParams(numOfTasks, taskRequiredTime,
                taskArrivalTime, taskCompletionGoal, taskPenalty);

            // Решение задачи.
            solutionsCollection = new SolutionsCollection(problemParams, fitnessValue);

            // Запись отклонения в файл
            sw.WriteLine(solutionsCollection.GetDeviationsString());

            if (Convert.ToChar(sr.ReadLine()!).Equals('%'))
            {
                continue;
            }
        }

        sr.Close();
        sw.Close();
    }

    /// <summary>
    /// Сгенерировать задачи, получить решение и записать всё в файл.
    /// </summary>
    /// <param name="numOfProblems">Число задач для генерации.</param>
    /// <exception cref="ArgumentException"></exception>
    public static void GenerateProblems(int numOfProblems = 100)
    {
        // Проверка корректности параметров
        // Если число прогонок задач было меньше либо равно нуля
        if (numOfProblems <= 0)
        {
            throw new ArgumentException($"Параметр {nameof(numOfProblems)} был меньше равен нуля.");
        }

        // Путь к файлу
        string fileName = "problems.txt";
        string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            fileName);

        StreamWriter sw = new StreamWriter(filePath);

        // Прогонка случайных задач
        for (int i = 0; i < numOfProblems; )
        {
            Console.WriteLine("Задача {0}", i);

            // Генерация условий задачи равномерным распределением.
            ProblemSolution solution = new(UniformDistribution.GenerateProblemUD());

            // Вызов алгоритма перебора.
            solution.BruteForceSolution();

            if (!solution.ValidateSolution())
            {
                continue;
            }

            // Запись в файл
            sw.WriteLine(solution.GetProblemStringRepr());

            i++;
        }

        sw.Close();
    }
}
