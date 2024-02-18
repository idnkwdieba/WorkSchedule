
namespace WorkSchedule.Shared;

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
        int numOfSubgroups = 2, int subgroupQuantity = 4, int memeplexCycles = 16, int populationCycles = 2)
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
        List<int[]> population = Population.GeneratePopulation(parameters, parameters.NumOfTasks,
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
            Population.SortByFitness(parameters, ref population);
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

                    // Для худшей лягушки в мемплексе

                    // Сохранить предыдущее значение
                    subgroups[i][subgroupQuantity - 1].CopyTo(tmpOrder, 0);

                    // Переместить лягушку в сторону лучшей лягушки в мемплексе
                    MoveTo(parameters.NumOfTasks, subgroups[i][subgroupQuantity - 1], subgroups[i][0]);

                    // Если приспособленность не стала лучше...
                    if (!parameters.CheckForBetterFitness(subgroups[i][subgroupQuantity - 1], tmpOrder))
                    {
                        // ...переместить лягушку в сторону глобально лучшей лягушки
                        MoveTo(parameters.NumOfTasks, subgroups[i][subgroupQuantity - 1], bestIndividual);

                        // Если и это не помогло...
                        if (!parameters.CheckForBetterFitness(subgroups[i][subgroupQuantity - 1], tmpOrder))
                        {
                            //...переместить лягушку в случайное место на поле
                            subgroups[i][subgroupQuantity - 1] = Population.RandomIndividual(parameters.NumOfTasks);
                        }
                    }

                    // Если в результате улучшилась приспособленность особи
                    if (parameters.CheckForBetterFitness(subgroups[i][subgroupQuantity - 1], tmpOrder))
                    {
                        // Если найдена лучшая особь в мемплексе
                        if (parameters.CheckForBetterFitness(subgroups[i][subgroupQuantity - 1], bestMemeplexIndividual[i]))
                        {
                            // Обновить информацию о лучшей особи в мемплексе
                            subgroups[i][subgroupQuantity - 1].CopyTo(bestMemeplexIndividual[i], 0);

                            // Если найдена лучшая особь во всей популяции
                            if (parameters.CheckForBetterFitness(subgroups[i][subgroupQuantity - 1], bestIndividual))
                            {
                                // Обновить лучшую особь
                                subgroups[i][subgroupQuantity - 1].CopyTo(bestIndividual, 0);
                            }
                        }

                        // Обновить порядок особей в мемлексе
                        List<int[]> tmpList = subgroups[i];
                        Population.SortByFitness(parameters, ref tmpList);
                    }
                }
            }
        }

        // Присвоить результат работы алгоритма параметру taskOrder
        taskOrder = bestIndividual;
    }

    /// <summary>
    /// Переместить лягушку в сторону лягушки-лидера в группе.
    /// </summary>
    /// <param name="numOfTasks">Число работ.</param>
    /// <param name="worstFrog">Худшая лягушка в группе.</param>
    /// <param name="leaderFrog">Лягушка-лидер в группе.</param>
    public static void MoveTo(int numOfTasks, int[] worstFrog, in int[] leaderFrog)
    {
        Random rand = new();

        // Для каждой координаты вектора
        for (int i = 0; i < numOfTasks; i++)
        {
            // Переместить в сторону координаты лучшей лягушки в группе
            worstFrog[i] = (int)Math.Round((rand.NextDouble() * (leaderFrog[i] - worstFrog[i]) + worstFrog[i]),
                MidpointRounding.AwayFromZero);

            // Держать координаты в допустимых границах
            worstFrog[i] = worstFrog[i] % numOfTasks;
        }
    }
}