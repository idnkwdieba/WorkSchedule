namespace WorkSchedule.Shared;

/// <summary>
/// Работа с начальной популяцией.
/// </summary>
public class Population
{
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
        var population = new List<int[]>();

        // Потенциальная особь для добавления в популяцию.
        int[] potentialIndividual;

        var rand = new Random();

        // Создание популяции.
        for (var i = 0; i < quantity; )
        {
            potentialIndividual = RandomIndividual(length);

            // Если получена допустимая особь.
            if (ProblemParams.ValidateSolution(parameters, potentialIndividual))
            {
                population.Add(potentialIndividual);
                i++;
            }
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

        var individual = new int[length];
        var rand = new Random();
        int randIndex;

        // Заполнение массива порядковыми числами.
        for (var index = 0; index < length; index++)
        {
            individual[index] = index;
        }

        // Случайная перестановка элементов массива.
        for (int index = length - 1; index >= 1; index--)
        {
            randIndex = rand.Next(index + 1);
            var temp = individual[randIndex];
            individual[randIndex] = individual[index];
            individual[index] = temp;
        }

        return individual;
    }


    /// <summary>
    /// Сортировка списка особей по оптимальности.
    /// </summary>
    /// <param name="parameters">Данные задачи.</param>
    /// <param name="population">Список особей для сортировки.</param>
    /// <exception cref="NullReferenceException"></exception>
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
                // если критерий оптимальности (index-1)-й особи больше критерия оптимальности index-й особи
                if (parameters.CheckForBetterFitness(population[i], population[i - 1]))
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
                // если критерий оптимальности (index-1)-й особи больше критерия оптимальности index особи
                if (parameters.CheckForBetterFitness(population[i], population[i - 1]))
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
    /// Вывести популяцию с приспособленностями.
    /// </summary>
    /// <param name="taskParams">Данные о задаче.</param>
    /// <param name="population">Популяция.</param>
    public static void PrintPopulation(ProblemParams taskParams, List<int[]> population)
    {
        int populationCount = population.Count;

        for (int index = 0; index < populationCount; index++)
        {
            ProblemParams.OutputSolutionData(taskParams, population[index]);
        }
    }
}
