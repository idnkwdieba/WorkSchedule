
namespace WorkSchedule.Shared;

using static System.Console;

public class SolutionsCollection
{
    private ProblemParams _problemParams; // данные о задаче
    private ProblemSolution _bruteForceSolution; // решение перебором
    private ProblemSolution _leapingFrogsSolution; // решение тасующим алгоритмом прыгающих лягушек
    private ProblemSolution _egaSolution; // решение эволюционно-генетическим алгоритмом
    /// <summary>
    /// Конструктор экземпляра класса.
    /// </summary>
    /// <param name="parameters">Данные о задаче.</param>
    public SolutionsCollection(ProblemParams parameters)
    {
        // Проверка корректности передаваемых параметров
        if (parameters == null)
        {
            throw new ArgumentNullException($"Переменная {nameof(parameters)} указывала на null.");
        }

        _problemParams = parameters;

        _bruteForceSolution = new ProblemSolution(parameters);
        _bruteForceSolution.BruteForceSolution();

        _leapingFrogsSolution = new ProblemSolution(parameters);
        _leapingFrogsSolution.LeapingFrogsSolution();

        _egaSolution = new ProblemSolution(parameters);
        _egaSolution.EgaSolution();
    }

    /// <summary>
    /// Вывести решения, полученные разными методами.
    /// </summary>
    public void PrintSolutions()
    {
        PrintSolutions(this);
    }
    /// <summary>
    /// Вывести решения, полученные разными методами.
    /// </summary>
    /// <param name="solutionsCollection">Коллекция решений для вывода.</param>
    public static void PrintSolutions(SolutionsCollection solutionsCollection)
    {
        // Проверка данных
        // Если передан указатель на null
        if (solutionsCollection == null)
        {
            throw new NullReferenceException($"Параметр {nameof(solutionsCollection)} " +
                $"имел указатель на null.");
        }

        Write($"{"Полный перебор",-20}| ");
        ProblemParams.OutputSolutionData(solutionsCollection._problemParams,
            solutionsCollection._bruteForceSolution.TaskOrder!);
        Write($"{"Прыгающие лягушки",-20}| ");
        ProblemParams.OutputSolutionData(solutionsCollection._problemParams,
            solutionsCollection._leapingFrogsSolution.TaskOrder!);
        Write($"{"ЭГА",-20}| ");
        ProblemParams.OutputSolutionData(solutionsCollection._problemParams,
            solutionsCollection._egaSolution.TaskOrder!);
    }

    /// <summary>
    /// Получение отклонения решения алгоритмом прыгающих лягушек от решения перебором.
    /// </summary>
    /// <returns>Процент отклонения от решения перебором.</returns>
    public double GetDeviation(bool isEgaDev = false)
    {
        return GetDeviation(this, isEgaDev);
    }

    /// <summary>
    /// Получение отклонения решения ЭГА или алгоритмом прыгающих лягушек от решения перебором.
    /// </summary>
    /// <param name="solutionsCollection">Коллекция решений.</param>
    /// <param name="isEgaDev">Получать ли отклонение для ЭГА.</param>
    /// <returns>Процент отклонения решения алгоритмом прыгающих лягушек от решения перебором.</returns>
    public static double GetDeviation(in SolutionsCollection solutionsCollection, bool isEgaDev = false)
    {
        // Проверка данных
        // Если передан указатель на null
        if (solutionsCollection == null)
        {
            throw new NullReferenceException($"Параметр {nameof(solutionsCollection)} " +
                $"имел указатель на null.");
        }

        double d1, d2, d3;

        // Для алгоритма прыгающих лягушек.
        if (!isEgaDev)
        {
            d1 = (solutionsCollection._leapingFrogsSolution.GoalFunction
                - solutionsCollection._bruteForceSolution.GoalFunction);
            d2 = d1 / solutionsCollection._bruteForceSolution.GoalFunction;
            d3 = d2 * 100;
            return d3;
        }

        // Для ЭГА.
        d1 = (solutionsCollection._egaSolution.GoalFunction 
            - solutionsCollection._bruteForceSolution.GoalFunction);
        d2 = d1 / solutionsCollection._bruteForceSolution.GoalFunction;
        d3 = d2 * 100;

        return d3;
    }

    /// <summary>
    /// Получить строку со всеми отклонениями.
    /// </summary>
    /// <returns>Строку с записанными отклонениями.</returns>
    public string GetDeviationsString()
    {
        return GetDeviationsString(this);
    }

    /// <summary>
    /// Получить строку со всеми отклонениями.
    /// </summary>
    /// <param name="solutionsCollection">Коллекция решений.</param>
    /// <returns>Строку с записанными отклонениями.</returns>
    public static string GetDeviationsString(in SolutionsCollection solutionsCollection)
    {
        double frogsDev = GetDeviation(in solutionsCollection);
        double egaDev = GetDeviation(in solutionsCollection, true);

        return $"{frogsDev:0.00} {egaDev:0.00}";
    }
}
