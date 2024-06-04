using System.Globalization;

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
    /// <param name="isSecondVersion">Запускать  коли вторую версию алгоритма.</param>
    public static void RunEvolGenAlg(
        in ProblemParams parameters, 
        ref int[] taskOrder,
        int populationQuantity = 100,
        int numOfEgaCycles = 25, 
        int hammingDist = 8,
        double mutationChance = 0.1, 
        bool isSecondVersion = false)
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
        var population = Population.GeneratePopulation(parameters,
            parameters.NumOfTasks, populationQuantity);

        // Список потомков.
        var nextGeneration = new List<int[]>();
        // Число кроссоверов родительских пар (каждая пара = два потомка).
        var descendantCount = populationQuantity / 2;

        // Начальная популяция + потомки.
        List<int[]> combinedIndividuals;
        // Численность обобщенных особей.
        int combinedIndCount;

        // Рулетка.
        double[] roulette;
        double randomRoulette;
        // Общая приспособленность.
        double totalFitness;

        // Индекс для поиска особи при выборе рулеткой.
        var curIndividualIndex = 0;

        // Родительская пара.
        int[] firstParent;
        int[] secondParent;

        var rand = new Random();

        taskOrder = new int[parameters.NumOfTasks];

        #endregion

        // Пока ЭГА не отработает заданное число циклов.
        for (var cycleIndex = 0; cycleIndex < numOfEgaCycles; cycleIndex++)
        {
            // -----------------------------------------------------------------------
            // 1. Воспроизводство.
            for (var index = 0; index < descendantCount; index++)
            {
                // Выбор родительской пары (если популяция слишком однородно - досрочное завершение ЭГА).
                if (!GetParentCouple(population, hammingDist, out firstParent, out secondParent))
                {
                    return;
                };

                // Скрещивание (зависит от версии ЭГА).
                if (!isSecondVersion)
                {
                    // Первая версия ЭГА.
                    nextGeneration.Add(GetDescendantPMX(firstParent, secondParent));
                    nextGeneration.Add(GetDescendantPMX(secondParent, firstParent));
                    continue;
                }

                // Вторая версия ЭГА.
                nextGeneration.Add(GetDescendantOX(firstParent, secondParent));
                nextGeneration.Add(GetDescendantOX(secondParent, firstParent));
            }

            // -----------------------------------------------------------------------
            // 2. Мутация.
            for (var index = 0; index < descendantCount; index++)
            {
                // Случайное значение, определяющее, произойдёт ли мутация.
                if (rand.NextDouble() <= mutationChance)
                {
                    // Весь генотип строится заново.
                    nextGeneration[index] = Population.RandomIndividual(parameters.NumOfTasks);
                }
            }

            // -----------------------------------------------------------------------
            // 3. Оценка потомков, отбрасывание недопустимых решений.
            for (var index = 0; index < nextGeneration.Count;)
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
            foreach (var individual in nextGeneration)
            {
                combinedIndividuals.Add(individual);
            }
            combinedIndCount = combinedIndividuals.Count;

            // Новая популяция, используемая в следующем цикле ЭГА.
            population = new();

            // Рулетка.
            roulette = new double[combinedIndCount];

            // -----------------------------------------------------------------------
            // 4.2. Отбор.
            // Заполнение рулетки.
            roulette[0] = 1.0 / ProblemParams.GetFitness(parameters, combinedIndividuals[0]);
            for (var index = 1; index < combinedIndCount; index++)
            {
                roulette[index] = roulette[index - 1]
                    + 1.0 / ProblemParams.GetFitness(parameters, combinedIndividuals[index]);
            }
            totalFitness = roulette[^1];

            // Заполнение новой популяции.
            for (var index = 0; index < populationQuantity; index++)
            {
                randomRoulette = rand.Next((int)(totalFitness * Math.Pow(10, 8))) / Math.Pow(10, 8);

                while (roulette[curIndividualIndex] < randomRoulette)
                {
                    curIndividualIndex++;
                }

                population.Add(combinedIndividuals[curIndividualIndex]);

                curIndividualIndex = 0;
            }

            combinedIndividuals = new();
            nextGeneration = new();
        }

        // Сортировка полученной популяции по приспособленности, возврат лучшей особи.
        Population.SortByFitness(parameters, ref population);
        Array.Copy(population[0], taskOrder, population[0].Length);
    }

    /// <summary>
    /// Получить потомка PMX скрещиванием.
    /// </summary>
    /// <param name="parentOne">Первый родитель.</param>
    /// <param name="parentTwo">Второй родитель.</param>
    /// <returns>Потомок.</returns>
    public static int[] GetDescendantPMX(int[] parentOne, int[] parentTwo)
    {
        var arrayLength = parentOne.Length;

        // Потомок.
        var descendant = new int[arrayLength];

        // Правила отображения.
        var mappingRules = new List<int[]>();

        // Границы копируемой секции.
        var sectionStartIndex = arrayLength / 3;
        var sectionEndIndex = arrayLength - arrayLength / 3;

        var mappingIndex = 0;
        int numOfMappingRules;

        // Копирование второго родителя в потомка.
        for (var index = 0; index < arrayLength; index++)
        {
            descendant[index] = parentTwo[index];
        }

        // Копирование секции из первого родителя, заполнение правил отображения.
        for (var index = sectionStartIndex; index < sectionEndIndex; index++)
        {
            descendant[index] = parentOne[index];

            mappingRules.Add(new int[]
            {
                parentOne[index],
                parentTwo[index]
            });
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
            for (var index = 0; index < sectionStartIndex; index++)
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
            for (var index = sectionEndIndex; index < arrayLength; index++)
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
        var arrayLength = parentOne.Length;

        // Потомок.
        var descendant = new int[arrayLength];

        // Границы копируемой секции.
        var sectionStartIndex = arrayLength / 3;
        var sectionEndIndex = arrayLength - arrayLength / 3;

        // Значения аллей второго родителя.
        var secondParentLegacy = new int[arrayLength];
        var legacyCtr = 0;

        // Заполнение массива недопустимыми значениями
        for (var index = 0; index < sectionStartIndex; index++)
        {
            descendant[index] = -1;
        }
        for (var index = sectionEndIndex; index < arrayLength; index++)
        {
            descendant[index] = -1;
        }

        // Копирование секции из первого родителя.
        for (var index = sectionStartIndex; index < sectionEndIndex; index++)
        {
            descendant[index] = parentOne[index];
        }

        // Копирование аллей второго родителя в список.
        for (var index = sectionEndIndex; index < arrayLength; index++)
        {
            secondParentLegacy[legacyCtr++] = parentTwo[index];
        }
        for (var index = 0; index < sectionEndIndex; index++)
        {
            secondParentLegacy[legacyCtr++] = parentTwo[index];
        }
        legacyCtr = 0;

        // Копирование аллелей второго родителя в потомка.
        // Со второй точки разрыва до конца.
        for (var index = sectionEndIndex; index < arrayLength;)
        {
            // Не содержит такого значения.
            if (!descendant.Contains(secondParentLegacy[legacyCtr]))
            {
                descendant[index++] = secondParentLegacy[legacyCtr];
            }
            legacyCtr++;
        }

        // Копирование аллелей второго родителя в потомка.
        // С начала до первой точки разрыва.
        for (var index = 0; index < sectionStartIndex;)
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
    /// <returns>true, если пара найдёна;<br/>
    /// false, если поиск подходящей пары занимает слишком много времени.</returns>
    public static bool GetParentCouple(List<int[]> population, int hammingDist,
        out int[] parentOne, out int[] parentTwo)
    {
        var populationCount = population.Count;

        var rand = new Random();

        var ctr = 0;

        // Пока не будет удовлетворяться критерий хеммингова расстояния.
        do
        {
            parentOne = population[rand.Next(0, populationCount)];
            parentTwo = population[rand.Next(0, populationCount)];
            ctr++;

            if (ctr == 256)
            {
                return false;
            }

        } while (GetHammingDistance(parentOne, parentTwo) > hammingDist);

        return true;
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
        if (firstTaskOrder is null || secondTaskOrder is null)
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

        var hammingDist = 0;

        var arrayLength = firstTaskOrder.Length;

        for (var index = 0; index < arrayLength; index++)
        {
            hammingDist += (firstTaskOrder[index] == secondTaskOrder[index])
                ? 1
                : 0;
        }

        return hammingDist;
    }
}
