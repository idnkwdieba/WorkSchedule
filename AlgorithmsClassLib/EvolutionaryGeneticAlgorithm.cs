namespace WorkSchedule.Shared;

/// <summary>
/// Эволюционно-генетический алгоритм.
/// </summary>
public class EvolutionaryGeneticAlgorithm
{
    /// <summary>
    /// Запуск ЭГА.
    /// </summary>
    /// <param name="parameters">Параметры задачи.</param>
    /// <param name="taskOrder">Получаемый порядок выполнения задач.</param>
    /// <param name="populationQuantity">Число особей в популяции.</param>
    /// <param name="numOfEgaCycles">Количество циклов работы алгоритма.</param>
    /// <param name="hammingDist">Хеммингово расстояние.</param>
    /// <param name="mutationChance">Шанс мутации.</param>
    /// <param name="isSecondVersion">Запускать ли вторую версию алгоритма.</param>
    public static void RunEvolGenAlg(in ProblemParams parameters, ref int[] taskOrder,
        int populationQuantity = 16, int numOfEgaCycles = 1, int hammingDist = 10,
        double mutationChance = 0.1, bool isSecondVersion = false)
    {
        #region Проверка параметров

        // Проверка положительности параметров.
        if (populationQuantity <= 0 || numOfEgaCycles <= 0)
        {
            throw new ArgumentException(
                $"Один или несколько параметров были меньше либо равны нуля.",
            $"{nameof(populationQuantity)}, {nameof(numOfEgaCycles)}");
        }
        // Если минимальное Хемингово расстояние меньше 2.
        if (hammingDist <= 2)
        {
            throw new ArgumentException(
                $"Хеммингово расстояние не может быть меньше 2.", nameof(hammingDist));
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

        #endregion

        #region Параметры

        // 0. Генерация начальной популяции.
        List<int[]> population = Population.GeneratePopulation(parameters,
            parameters.NumOfTasks, populationQuantity);

        // Список потомков.
        List<int[]> nextGeneration = new();
        // Число потомков.
        int descendantCount = populationQuantity / 2;

        // Начальная популяция + потомки.
        List<int[]> combinedIndividuals = new();
        // Численность обобщенных особей.
        int combinedIndCount;

        // Рулетка.
        int[] roulette;
        int randomRoulette;
        // Общая приспособленность.
        int totalFitness;

        // Индекс для поиска особи при выборе рулеткой.
        int curIndividualIndex = 0;

        // Родительская пара.
        int[] firstParent;
        int[] secondParent;

        Random rand = new Random();

        #endregion

        // Пока ЭГА не отработает заданное число циклов.
        for (int cycleIndex = 0; cycleIndex < numOfEgaCycles; cycleIndex++)
        {
            // todo: Проверка существования хотя бы одной пары с нужным хеминговым расстроянием?

            // -----------------------------------------------------------------------
            // 1. Воспроизводство.
            for (int index = 0; index < descendantCount; index++)
            {
                // Выбор родительской пары.
                GetParentCouple(population, hammingDist, out firstParent, out secondParent);

                // Скрещивание.
                if (isSecondVersion == false)
                {
                    // Первая версия ЭГА.
                    nextGeneration.Add(GetDescendantPMX(firstParent, secondParent));
                }
                else
                {
                    // Вторая версия ЭГА.
                    nextGeneration.Add(GetDescendantOX(firstParent, secondParent));
                }
            }

            // -----------------------------------------------------------------------
            // 2. Мутация.
            for (int index = 0; index < descendantCount; index++)
            {
                // Случайное значение, определяющее, произойдёт ли мутация.
                if (rand.NextDouble() > mutationChance)
                {
                    continue;
                }

                // Весь генотип строится заново.
                nextGeneration[index] = Population.RandomIndividual(parameters.NumOfTasks);
            }

            // -----------------------------------------------------------------------
            // 3. Оценка потомков.
            for (int index = 0; index < nextGeneration.Count;)
            {
                // Если особь допустима.
                if (ProblemParams.ValidateSolution(parameters, nextGeneration[index]))
                {
                    index++;
                    continue;
                }

                nextGeneration.RemoveAt(index);
            }

            // -----------------------------------------------------------------------
            // 4.1. Объединение предков и потомков.
            combinedIndividuals = population;
            foreach (int[] individual in nextGeneration)
            {
                combinedIndividuals.Add(individual);
            }
            combinedIndCount = combinedIndividuals.Count;

            // Новая популяция, используемая в следующем цикле ЭГА.
            population = new();

            // Рулетка.
            roulette = new int[combinedIndCount];

            // 4.2. Отбор.

            // Заполнение рулетки.
            roulette[0] = ProblemParams.GetFitness(parameters, combinedIndividuals[0]);
            for (int index = 1; index < combinedIndCount; index++)
            {
                roulette[index] = roulette[index - 1]
                    + ProblemParams.GetFitness(parameters, combinedIndividuals[index]);
            }
            totalFitness = roulette[^1];

            // Заполнение новой популяции.
            for (int index = 0; index < populationQuantity; index++)
            {
                randomRoulette = rand.Next(0, totalFitness);

                while (roulette[curIndividualIndex] < randomRoulette)
                {
                    curIndividualIndex++;
                }

                population.Add(combinedIndividuals[curIndividualIndex]);

                curIndividualIndex = 0;
            }
        }

        // Сортировка полученной популяции по приспособленности, возврат лучшей особи.
        Population.SortByFitness(parameters, ref population);
        taskOrder = population[0];
    }

    /// <summary>
    /// Получить потомка PMX скрещиванием.
    /// </summary>
    /// <param name="parentOne">Первый родитель.</param>
    /// <param name="parentTwo">Второй родитель.</param>
    /// <returns>Потомок.</returns>
    public static int[] GetDescendantPMX(int[] parentOne, int[] parentTwo)
    {
        int arrayLength = parentOne.Length;

        // Потомок.
        int[] descendant = new int[arrayLength];

        // Правила отображения.
        List<int[]> mappingRules = new();

        // Границы копируемой секции.
        int sectionStartIndex = arrayLength / 3;
        int sectionEndIndex = arrayLength - arrayLength / 3;

        int mappingIndex = 0;
        int numOfMappingRules;

        // Копирование второго родителя в потомка.
        for (int index = 0; index < arrayLength; index++)
        {
            descendant[index] = parentTwo[index];
        }

        // Копирование секции из первого родителя, заполнение правил отображения.
        for (int index = sectionStartIndex; index < sectionEndIndex; index++)
        {
            descendant[index] = parentOne[index];

            mappingRules.Add(new int[] { parentOne[index], parentTwo[index] });
        }

        numOfMappingRules = mappingRules.Count;

        // Заполнение аллей потомка, не унаследованных от первого родителя, согласно правилам отображения.
        while (numOfMappingRules > 0)
        {
            // Если отображения не работают.
            if (mappingIndex == mappingRules.Count)
            {
                break;
            }

            // Первая треть перестановки.
            for (int index = 0; index < sectionStartIndex; index++)
            {
                if (descendant[index] == mappingRules[mappingIndex][0])
                {
                    descendant[index] = mappingRules[mappingIndex][1];
                    mappingRules.RemoveAt(mappingIndex);
                    mappingIndex = 0;
                    break;
                }
            }
            if (numOfMappingRules != mappingRules.Count)
            {
                numOfMappingRules = mappingRules.Count;
                continue;
            }

            // Последняя треть перестановки.
            for (int index = sectionEndIndex; index < arrayLength; index++)
            {
                if (descendant[index] == mappingRules[mappingIndex][0])
                {
                    descendant[index] = mappingRules[mappingIndex][1];
                    mappingRules.RemoveAt(mappingIndex);
                    mappingIndex = 0;
                    break;
                }
            }
            if (numOfMappingRules != mappingRules.Count)
            {
                numOfMappingRules = mappingRules.Count;
                continue;
            }

            // Если отображение не подошло.
            mappingIndex++;
        }

        return descendant;
    }

    /// <summary>
    /// Получить потомка порядковым скрещиванием.
    /// </summary>
    /// <param name="parentOne">Первый родитель.</param>
    /// <param name="parentTwo">Второй родитель.</param>
    /// <returns>Потомок.</returns>
    public static int[] GetDescendantOX(int[] parentOne, int[] parentTwo)
    {
        // todo: todo

        int arrayLength = parentOne.Length;

        // Потомок.
        int[] descendant = new int[arrayLength];

        // Границы копируемой секции.
        int sectionStartIndex = arrayLength / 3;
        int sectionEndIndex = arrayLength - arrayLength / 3;

        // Значения аллей второго родителя
        
        int[] secondParentLegacy = new int[arrayLength];
        int legacyCtr = 0;

        // Заполнение массива недопустимыми значениями
        for (int index = 0; index < sectionStartIndex; index++)
        {
            descendant[index] = -1;
        }
        for (int index = sectionEndIndex; index < arrayLength; index++)
        {
            descendant[index] = -1;
        }

        // Копирование секции из первого родителя.
        for (int index = sectionStartIndex; index < sectionEndIndex; index++)
        {
            descendant[index] = parentOne[index];
        }

        // Копирование аллей второго родителя в список.
        for (int index = sectionEndIndex; index < arrayLength; index++)
        {
            secondParentLegacy[legacyCtr++] = parentTwo[index];
        }
        for (int index = 0; index < sectionEndIndex; index++)
        {
            secondParentLegacy[legacyCtr++] = parentTwo[index];
        }
        legacyCtr = 0;

        // Копирование аллелей второго родителя в потомка.
        // Со второй точки разрыва до конца.
        for (int index = sectionEndIndex; index < arrayLength; )
        {
            // Не содержит такого значения.
            if (!descendant.Contains(secondParentLegacy[legacyCtr]))
            {
                descendant[index++] = secondParentLegacy[legacyCtr];
            }
            legacyCtr++;
        }
        // С начала до первой точки разрыва.
        for (int index = 0; index < sectionStartIndex; )
        {
            // Не содержит такого значения.
            if (!descendant.Contains(secondParentLegacy[legacyCtr]))
            {
                descendant[index++] = secondParentLegacy[legacyCtr];
            }
            legacyCtr++;
        }

        return descendant;
    }

    /// <summary>
    /// Получить родительскую пару инбридингом.
    /// </summary>
    /// <param name="population">Популяция для поиска пары.</param>
    /// <param name="hammingDist">Хеммингово расстояние.</param>
    /// <param name="parentOne">Первый родитель.</param>
    /// <param name="parentTwo">Второй родитель.</param>
    public static void GetParentCouple(List<int[]> population, int hammingDist,
        out int[] parentOne, out int[] parentTwo)
    {
        int populationCount = population.Count;

        Random rand = new Random();

        // Пока не будет удовлетворяться критерий хеммингова расстояния.
        do
        {
            parentOne = population[rand.Next(0, populationCount)];
            parentTwo = population[rand.Next(0, populationCount)];

        } while (GetHammingDistance(parentOne, parentTwo) > hammingDist);
    }

    /// <summary>
    /// Вычисление хеммингова расстрояния.
    /// </summary>
    /// <param name="firstTaskOrder">Первая перестановка сравнения.</param>
    /// <param name="secondTaskOrder">Вторая перестановка сравнения.</param>
    /// <returns>Хеммингово расстояние.</returns>
    /// <exception cref="ArgumentException"></exception>
    public static int GetHammingDistance(in int[] firstTaskOrder, in int[] secondTaskOrder)
    {
        // Проверка на null.
        if (firstTaskOrder == null || secondTaskOrder == null)
        {
            throw new ArgumentException("Один из массивов был равен null.",
                $"{nameof(firstTaskOrder)}, {nameof(secondTaskOrder)}");
        }

        // Проверка на одинаковую длину.
        if (firstTaskOrder.Length != secondTaskOrder.Length)
        {
            throw new ArgumentException("Длина двух массивов различна.",
                $"{nameof(firstTaskOrder)}, {nameof(secondTaskOrder)}");
        }

        int hammingDist = 0;

        int arrayLength = firstTaskOrder.Length;

        for (int index = 0; index < arrayLength; index++)
        {
            hammingDist += (firstTaskOrder[index] == secondTaskOrder[index])
                ? 1
                : 0;
        }

        return hammingDist;
    }
}
