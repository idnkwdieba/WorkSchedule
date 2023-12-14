
namespace WorkSchedule.Shared;

using System.Diagnostics.CodeAnalysis;
using static System.Console;

public static class FrogsAlgorithm
{
    /// <summary>
    /// Решить задачу расписаний тасующим алгоритмом прыгающих лягушек.
    /// </summary>
    /// <param name="parameters">Параметры задачи.</param>
    /// <param name="numOfSubgroups">Количество мемплексов.</param>
    /// <param name="subgroupQuantity">Число особей в мемплексе.</param>
    /// <param name="taskOrder">Получаемый порядок выполнения задач.</param>
    /// <param name="memeplexCycles">Количество циклов в работе алгоритма для мемплекса.</param>
    /// <param name="populationCycles">Количество циклов в работе алгоритма для популяции в целом.</param>
    public static void RunFrogsAlg(in ProblemParams parameters, ref int[] taskOrder,
        int numOfSubgroups = 2, int subgroupQuantity = 4, int memeplexCycles = 1, int populationCycles = 1)
    {
        // Если переданы некорректные параметры
        // Если числовые параметры меньше или равны нуля
        if (numOfSubgroups <= 0 || subgroupQuantity <= 0 || memeplexCycles <= 0)
        {
            throw new ArgumentException(
                $"Один или несколько параметров {nameof(numOfSubgroups)}, {nameof(subgroupQuantity)}, " +
                $"{nameof(memeplexCycles)}, {nameof(populationCycles)} были меньше либо равны нуля.");
        }
        // Если не переданы данные о задаче
        if (parameters == null)
        {
            throw new NullReferenceException($"{nameof(parameters)} имеет пустой указатель.");
        }
        // Если в задаче не существует допустимых решений
        if (!parameters.TaskArrivalTime.Contains(0))
        {
            throw new ArgumentException($"Задача расписаний {nameof(parameters)} не содержит допустимых решений");
        }

        // Список подгрупп
        List<List<int[]>> subgroups = new();
        // Генерация популяции
        List<int[]> population = GeneratePopulation(parameters, parameters.NumOfTasks, 
            numOfSubgroups * subgroupQuantity);
        // лучшая особь в популяции
        int[] bestIndividual = new int[parameters.NumOfTasks];
        // лучшие особи в мемплексах
        int[][] bestMemeplexIndividual = new int[numOfSubgroups][];

        // Временный порядок выполнения работ
        int[] tmpOrder = new int[parameters.NumOfTasks];

        // Циклы алгоритма для популяции в целом
        for (int plnItr = 0; plnItr < populationCycles; plnItr++)
        {
            // Сортировка по критерию оптимальности, сохранение лучшей особи
            SortByFitness(parameters, ref population);
            population[0].CopyTo(bestIndividual, 0);

            // Разбивка на подгруппы
            subgroups = new();
            for (int i = 0; i < numOfSubgroups; i++)
            {
                subgroups.Add(new());
                for (int j = 0; j < subgroupQuantity; j++)
                {
                    subgroups[i].Add(population[j * numOfSubgroups + i]);
                }
            }

            // Циклы алгоритма для мемплексов
            for (int mpxItr = 0; mpxItr < memeplexCycles; mpxItr++)
            {
                // Работа алгоритма для каждой подгруппы
                for (int i = 0; i < numOfSubgroups; i++)
                {
                    // Сохранить информацию о лучшей особи
                    bestMemeplexIndividual[i] = new int[parameters.NumOfTasks];
                    for (int ind = 0; ind < parameters.NumOfTasks; ind++)
                    {
                        bestMemeplexIndividual[i][ind] = subgroups[i][0][ind];
                    }

                    // Для каждой лягушки кроме лучшей
                    for (int j = 1; j < subgroupQuantity; j++)
                    {
                        // Переместить лягушку в сторону лучшей лягушки в мемплексе

                        // Временное решение, пока не понятно как реализовать алгоритм для теории расписаний
                        subgroups[i][j].CopyTo(tmpOrder, 0);

                        if (ProblemParams.GetFitness(parameters, subgroups[i][j])
                            <= ProblemParams.GetFitness(parameters, tmpOrder))
                        {

                            // Если это не улучшило её приспособленность - переместить лягушку в сторону глобально
                            // лучшей лягушки

                            // Временное решение, пока не понятно как реализовать алгоритм для теории расписаний
                            subgroups[i][j].CopyTo(tmpOrder, 0);

                            if (ProblemParams.GetFitness(parameters, subgroups[i][j])
                                <= ProblemParams.GetFitness(parameters, tmpOrder))
                            {
                                // Если и это не помогло - переместить лягушку в случайное место на поле
                                subgroups[i][j] = RandomIndividual(parameters.NumOfTasks);
                            }
                        }

                        int tmp1, tmp2;

                        // Если найдена лучшая особь в мемплексе
                        if ((tmp1 = ProblemParams.GetFitness(parameters, bestMemeplexIndividual[i]))
                            > (tmp2 = ProblemParams.GetFitness(parameters, subgroups[i][j])))
                        {
                            // Обновить информацию о лучшей особи в мемплексе
                            subgroups[i][j].CopyTo(bestMemeplexIndividual[i], 0);
                        }

                        // Если найдена лучшая особь во всей популяции
                        if ((tmp1 = ProblemParams.GetFitness(parameters, bestIndividual))
                            > (tmp2 = ProblemParams.GetFitness(parameters, subgroups[i][j])))
                        {
                            // Обновить лучшую особь
                            subgroups[i][j].CopyTo(bestIndividual, 0);
                        }
                    }

                    // Обновить порядок особей в мемлексе
                    List<int[]> tmpList = subgroups[i];
                    SortByFitness(parameters, ref tmpList);
                }
            }
        }

        // Присвоить результат работы алгоритма параметру taskOrder
        taskOrder = bestIndividual;
    }

    /// <summary>
    /// Создание новой популяции особей.
    /// </summary>
    /// <param name="length">Количество задач для выполнения.</param>
    /// <param name="quantity">Число случайных порядков выполнения задач для генерации.</param>
    /// <returns>Множество случайных порядков обработки задач.</returns>
    /// <exception cref="ArgumentException"></exception>
    public static List<int[]> GeneratePopulation(ProblemParams parameters, int length, int quantity)
    {
        // Если переданы некорректные параметры
        if (length <= 0 || quantity <= 0)
        {
            throw new ArgumentException(
                $"{nameof(length)} или {nameof(quantity)} было меньше либо равно нуля.");
        }

        // Список популяции из множества особей
        List<int[]> population = new List<int[]>();

        Random rand = new Random();

        for (int i = 0; i < quantity; i++)
        {
            population.Add(RandomIndividual(length));
        }

        // Гарантировать нахождение хотя бы одного допустимого решения в популяции
        // Проверка допустимости решений в популяции
        foreach(int[] i in population)
        {
            // Если найдено хотя бы одно допустимое решение
            if (ProblemParams.ValidateSolution(parameters, i))
            {
                // Вернуть популяцию
                return population;
            }
        }
        // Если не найдено допустимых решений
        do
        {
            // Создавать случайную особь...
            population[0] = RandomIndividual(length);
        } while (!ProblemParams.ValidateSolution(parameters, population[0])); // ...пока она не будет
                                                                              // являться допустимой
        return population;
    }

    /// <summary>
    /// Создает новую случайную особь.
    /// </summary>
    /// <param name="length">Количество задач.</param>
    /// <returns>Случайную перестановку порядка выполнения работ.</returns>
    public static int[] RandomIndividual(int length)
    {
        // Если переданы некорректные параметры
        if (length <= 0)
        {
            throw new ArgumentException($"{nameof(length)} было меньше или равно нуля.");
        }

        int[] individual = new int[length];
        Random rand = new();
        int randNum;

        for (int i = 0; i < length; ) 
        {
            // генерация случайного значения
            randNum = rand.Next(1, length+1);
            // если такое значение встречалось ранее
            if (individual.Contains(randNum))
            {
                continue;
            }
            // в противном случае
            individual[i] = randNum;
            i++;
        }
        // Уменьшить значения индексов на единицу
        for (int i = 0; i < length; i++)
        {
            individual[i] -= 1;
        }

        return individual;
    }

    public static void SortByFitness(in ProblemParams parameters, ref List<int[]> population)
    {
        // если переданы некорректные параметры
        if (parameters == null || population == null)
        {
            throw new NullReferenceException($"{nameof(parameters)} " +
                $"или {nameof(population)} имело указатель на null.");
        }

        int left = 1; int right = population.Count - 1;
        int[] tmp;

        // цикл сортировки
        while (left <= right)
        {
            // цикл справа налево 
            for (int i = right; i >= left; i--)
            {
                // если критерий оптимальности (i-1)-й особи больше критерия оптимальности i-й особи
                if (ProblemParams.GetFitness(parameters, population[i-1])
                    > ProblemParams.GetFitness(parameters, population[i]))
                {
                    // поменять особи местами
                    tmp = population[i];
                    population[i] = population[i - 1];
                    population[i - 1] = tmp;
                }
            }
            left++;

            // цикл слева направо
            for (int i = left; i <= right; i++)
            {
                // если критерий оптимальности (i-1)-й особи больше критерия оптимальности i особи
                if (ProblemParams.GetFitness(parameters, population[i - 1])
                    > ProblemParams.GetFitness(parameters, population[i]))
                {
                    // поменять особи местами
                    tmp = population[i];
                    population[i] = population[i - 1];
                    population[i - 1] = tmp;
                }
            }
            right--;
        }
    }

    /// <summary>
    /// Вывести перестановку и значение её целевой функции.
    /// </summary>
    /// <param name="taskOrder"></param>
    /// <param name="goalFunc"></param>
    private static void OutputSolutionData(in int[] taskOrder, in int goalFunc)
    {
        Write("Порядок:");
        foreach (int i in taskOrder)
        {
            Write(" " + (i + 1));
        }
        WriteLine($"; Целевая функция: {goalFunc}");
    }
}
