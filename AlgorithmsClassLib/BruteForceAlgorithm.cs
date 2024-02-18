
namespace WorkSchedule.Shared;

using static System.Console;

public static class BruteForceAlgorithm
{
    /// <summary>
    /// Решить задачу расписаний перебором всех возможных перестановок.
    /// </summary>
    /// <param name="parameters">Параметры задачи.</param>
    /// <param name="taskOrder">Получаемый порядок выполнения задач.</param>
    /// <param name="prevIndexes">Индексы, использованные ранее в перестановке.</param>
    public static void RunBruteForceAlg(in ProblemParams parameters, ref int[] taskOrder, 
        List<int>? prevIndexes = null)
    {
        // Если список предыдущих индексов ещё не был создан
        if (prevIndexes == null)
        {
            prevIndexes = new();
        }

        // Если список индексов по длине совпал с числом работ
        if (prevIndexes.Count == parameters.NumOfTasks)
        {
            // debug: вывести перестановку
            /*
            int[] tmpArr = new int[parameters.NumOfTasks];
            prevIndexes.CopyTo(tmpArr, 0);
            Console.WriteLine(ProblemParams.TurnArrayToString(tmpArr));
            */

            // Поиск решения

            int[] tmpTaskOrder = new int[parameters.NumOfTasks]; // временный массив для порядка выполнения работы

            // копировать порядок выполнения работ из списка в массив
            prevIndexes.CopyTo(tmpTaskOrder, 0);

            // Вывод данных полученной перестановки
            /*
            if (!ProblemParams.ValidateSolution(parameters, tmpTaskOrder))
            {
                // Недопустимое решение
                WriteLine("Порядок:" + ProblemParams.TurnArrayToString(tmpTaskOrder) 
                    + "; Является недопустимым решением");
            }
            else
            {
                // Допустимое решение
                ProblemParams.OutputSolutionData(parameters, tmpTaskOrder);
            }
            */

            // Если полученное решение является более оптимальным, чем предыдущее
            if (taskOrder == null || parameters.CheckForBetterFitness(tmpTaskOrder, taskOrder))
            {
                // Присвоить значения соответствующим параметрам
                taskOrder = tmpTaskOrder;
            }

            return;
        }

        // Перебор всех вариантов на шаг ниже
        for (int i = 0; i < parameters.NumOfTasks; i++)
        {
            // Если данный индекс уже используется в перестановке
            if (prevIndexes.Contains(i))
            {
                continue;
            }

            // Повторный вызов функции
            prevIndexes.Add(i);
            RunBruteForceAlg(parameters, ref taskOrder, prevIndexes);
            prevIndexes.Remove(i);
        }
    }
}