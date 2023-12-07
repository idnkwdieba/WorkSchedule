
namespace WorkSchedule.Shared;

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
    public static void RunFrogsAlg(in ProblemParams parameters, int numOfSubgroups,
        int subgroupQuantity, ref int[] taskOrder, int memeplexCycles = 5, int populationCycles = 1)
    {
        // Если переданы некорректные параметры
        if (numOfSubgroups <= 0 || subgroupQuantity <= 0 || memeplexCycles <= 0)
        {
            throw new ArgumentException(
                $"Один или несколько параметров {nameof(numOfSubgroups)}, {nameof(subgroupQuantity)}, " +
                $"{nameof(memeplexCycles)}, {nameof(populationCycles)} были меньше либо равны нуля.");
        }
        if (parameters == null)
        {
            throw new NullReferenceException($"{nameof(parameters)} имеет пустой указатель.");
        }

        // Список подгрупп
        List<List<int[]>> subgroups = new(); 
        // Генерация популяции
        List<int[]> population = GeneratePopulation(
            parameters.NumOfTasks, numOfSubgroups * subgroupQuantity);
        // лучшая особь в популяции
        int[] bestIndividual = new int[parameters.NumOfTasks];
        // лучшие особи в мемплексах
        int[][] bestMemeplexIndividual = new int[numOfSubgroups][];

        // Временный порядок выполнения работ
        int[] tmpOrder = new int[subgroupQuantity];

        // Циклы алгоритма для популяции в целом
        for (int plnItr = 0; plnItr < populationCycles; plnItr++)
        {
            // Сортировка по критерию оптимальности, сохранение лучшей особи
            SortByFitness(parameters, ref population);
            population[0].CopyTo(bestIndividual, 0);

            // Разбивка на подгруппы
            for (int i = 0; i < numOfSubgroups; i++)
            {
                subgroups[i] = new();
                for (int j = 0; j < subgroupQuantity; j++)
                {
                    subgroups[i].Add(population[i * subgroupQuantity + j]);
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
                    subgroups[i].CopyTo(bestMemeplexIndividual);

                    // Для каждой лягушки кроме лучшей
                    for (int j = 1; j < subgroupQuantity; j++)
                    {
                        // Переместить лягушку в сторону лучшей лягушки в мемплексе

                        // Временное решение, пока не понятно как реализовать алгоритм для теории расписаний
                        subgroups[i][j].CopyTo(tmpOrder, 0);

                        if (ProblemParams.GetFitness(parameters, subgroups[i][j])
                            <= ProblemParams.GetFitness(parameters, subgroups[i][0]))
                        {

                            // Если это не улучшило её приспособленность - переместить лягушку в сторону глобально
                            // лучшей лягушки

                            // Временное решение, пока не понятно как реализовать алгоритм для теории расписаний
                            subgroups[i][j].CopyTo(tmpOrder, 0);

                            if (ProblemParams.GetFitness(parameters, subgroups[i][j])
                                <= ProblemParams.GetFitness(parameters, subgroups[i][0]))
                            {
                                // Если и это не помогло - переместить лягушку в случайное место на поле
                                subgroups[i][j] = RandomIndividual(parameters.NumOfTasks);
                            }
                        }

                        // Если найдена лучшая особь в мемплексе
                        if (ProblemParams.GetFitness(parameters, bestMemeplexIndividual[i])
                            < ProblemParams.GetFitness(parameters, tmpOrder))
                        {
                            // Обновить информацию о лучшей особи в мемплексе
                            tmpOrder.CopyTo(bestMemeplexIndividual[i], 0);
                        }

                        // Если найдена лучшая особь во всей популяции
                        if (ProblemParams.GetFitness(parameters, bestIndividual)
                            < ProblemParams.GetFitness(parameters, tmpOrder))
                        {
                            // Обновить лучшую особь
                            tmpOrder.CopyTo(bestIndividual, 0);
                        }
                    }

                    // Обновить порядок особей в мемлексе
                    List<int[]> tmpList = subgroups[i];
                    SortByFitness(parameters, ref tmpList);
                }
            }
        }
    }

    /// <summary>
    /// Создание новой популяции особей.
    /// </summary>
    /// <param name="length">Количество задач для выполнения.</param>
    /// <param name="quantity">Число случайных порядков выполнения задач для генерации.</param>
    /// <returns>Множество случайных порядков обработки задач.</returns>
    /// <exception cref="ArgumentException"></exception>
    public static List<int[]> GeneratePopulation(int length, int quantity)
    {
        // Если переданы некорректные параметры
        if (length <= 0 || quantity <= 0)
        {
            throw new ArgumentException(
                $"{nameof(length)} или {nameof(quantity)} было меньше либо равно нуля.");
        }

        // Список популяции из множества особей
        List<int[]> population = new List<int[]>();

        for (int i = 0; i < quantity; i++)
        {
            population.Add(RandomIndividual(length));
        }

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
            randNum = rand.Next(1, length + 1);
            // если такое значение встречалось ранее
            if (individual.Contains(randNum))
            {
                continue;
            }
            // в противном случае
            individual[i] = randNum;
            i++;
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

        int left = 0, right = population.Count-1;
        int[] tmp;

        // цикл сортировки
        while (left < right)
        {
            // цикл в одну сторону
            for (int i = 0; i < right; i++)
            {
                // если критерий оптимальности i-й особи меньше критерия оптимальности i+1 особи
                if (ProblemParams.GetFitness(parameters, population[i])
                    < ProblemParams.GetFitness(parameters, population[i + 1]))
                {
                    // поменять особи местами
                    tmp = population[i];
                    population[i] = population[i+1];
                    population[i+1] = tmp;
                }
            }
            right--;

            // цикл в другую сторону
            for (int j = right; j > left; j--)
            {
                // если критерий оптимальности j-й особи больше критерия оптимальности j-1 особи
                if (ProblemParams.GetFitness(parameters, population[j-1])
                    < ProblemParams.GetFitness(parameters, population[j]))
                {
                    // поменять особи местами
                    tmp = population[j];
                    population[j] = population[j + 1];
                    population[j + 1] = tmp;
                }
            }
            left++;
        }
    }
}
