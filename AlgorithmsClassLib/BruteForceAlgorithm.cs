
// To-do: вынести критерий в отдельный метод, который будет передаваться в функцию
// (Сделать это с помощью событий или делегатов?)
// To-do: Вынести статические функции OutputSolutionData в класс ProblemSolution

namespace WorkSchedule.Shared;

using static System.Console;

public static class BruteForceAlgorithm
{
    /// <summary>
    /// Решить задачу расписаний перебором всех возможных перестановок.
    /// </summary>
    /// <param name="parameters">Параметры задачи.</param>
    /// <param name="taskOrder">Получаемый порядок выполнения задач.</param>
    /// <param name="taskStartTime">Получаемое время начала выполнения работ.</param>
    /// <param name="taskEndTime">Получаемое время окончания выполнения работ.</param>
    /// <param name="goalFunc">Получаемая целевая функция данного решения.</param>
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
            // Поиск решения

            int[] tmpTaskOrder = new int[parameters.NumOfTasks]; // временный массив для порядка выполнения работы
            int tmpGoalFunc; // временное значение целевой функции

            // копировать порядок выполнения работ из списка в массив
            prevIndexes.CopyTo(tmpTaskOrder, 0);



            // Вычисление целевой функции
            tmpGoalFunc = ProblemParams.GetFitness(parameters, tmpTaskOrder);

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
            if (taskOrder == null || tmpGoalFunc < ProblemParams.GetFitness(parameters, taskOrder))
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