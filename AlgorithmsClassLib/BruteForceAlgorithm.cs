
namespace WorkSchedule.Shared;

using static System.Console;

public static class BruteForceAlgorithm
{
    static int NumOfTasks;

    /// <summary>
    /// Решить задачу расписаний перебором всех возможных перестановок.
    /// </summary>
    /// <param name="parameters">Параметры задачи.</param>
    /// <param name="taskOrder">Получаемый порядок выполнения задач.</param>
    /// <param name="prevIndexes">Индексы, использованные ранее в перестановке.</param>
    public static void RunBruteForceAlg(in ProblemParams parameters, ref int[] taskOrder)
    {
        // Сохранить число задач в статическую переменную.
        NumOfTasks = parameters.NumOfTasks;

        // Заполнить начальный массив порядка выполнения работ.
        taskOrder = new int[NumOfTasks];
        for (int index = 0; index < NumOfTasks; index++)
        {
            taskOrder[index] = index;
        }

        // Пройтись по всем перестановкам.
        CheckAllVariants(parameters, ref taskOrder);
    }

    /// <summary>
    /// Перебор всех возможных перестановок.
    /// </summary>
    /// <param name="parameters">Параметры задачи.</param>
    /// <param name="taskOrder">Получаемый порядок выполнения задач.</param>
    /// <param name="prevIndexes">Индексы, использованные ранее в перестановке.</param>
    public static void CheckAllVariants(in ProblemParams parameters, ref int[] taskOrder,
        List<int>? prevIndexes = null)
    {
        // Если список предыдущих индексов ещё не был создан
        if (prevIndexes == null)
        {
            prevIndexes = new();
        }

        // Если список индексов по длине совпал с числом работ
        if (prevIndexes.Count == NumOfTasks)
        {
            // Поиск решения

            // Временный массив для порядка выполнения работы
            int[] tmpTaskOrder = new int[NumOfTasks];

            // копировать порядок выполнения работ из списка в массив
            prevIndexes.CopyTo(tmpTaskOrder, 0);

            // Если полученное решение является более оптимальным, чем предыдущее
            if (parameters.CheckForBetterFitness(tmpTaskOrder, taskOrder))
            {
                // Присвоить значения соответствующим параметрам
                taskOrder = tmpTaskOrder;
            }

            return;
        }

        // Перебор всех вариантов на шаг ниже
        for (int i = 0; i < NumOfTasks; i++)
        {
            // Если данный индекс уже используется в перестановке
            if (prevIndexes.Contains(i))
            {
                continue;
            }

            // Повторный вызов функции
            prevIndexes.Add(i);
            CheckAllVariants(parameters, ref taskOrder, prevIndexes);
            prevIndexes.Remove(i);
        }
    }
}