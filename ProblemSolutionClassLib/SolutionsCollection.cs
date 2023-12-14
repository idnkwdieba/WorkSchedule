
namespace WorkSchedule.Shared;

using static System.Console;

public class SolutionsCollection
{
    private ProblemParams _problemParams; // данные о задаче
    private ProblemSolution _bruteForceSolution; // решение перебором
    public ProblemSolution _leapingFrogsSolution; // решение тасующим алгоритмом прыгающих лягушек

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
    }

    // Вывести решения, полученные разными методами.
    public void PrintSolutions()
    {
        PrintSolutions(this);
    }
    public static void PrintSolutions(SolutionsCollection solutionsCollection)
    {
        Write($"{"Полный перебор", -20}| ");
        ProblemParams.OutputSolutionData(solutionsCollection._problemParams,
            solutionsCollection._bruteForceSolution.TaskOrder!);
        Write($"{"Прыгающие лягушки",-20}| ");
        ProblemParams.OutputSolutionData(solutionsCollection._problemParams,
            solutionsCollection._leapingFrogsSolution.TaskOrder!);
    }

    /// <summary>
    /// Получение отклонения решения алгоритмом прыгающих лягушек от решения перебором.
    /// </summary>
    /// <returns>Процент отклонения от решения перебором.</returns>
    public double GetDeviation()
    {
        return GetDeviation(this);
    }

    /// <summary>
    /// Получение отклонения решения алгоритмом прыгающих лягушек от решения перебором.
    /// </summary>
    /// <param name="solutionsCollection">Коллекция решений.</param>
    /// <returns>Процент отклонения решения алгоритмом прыгающих лягушек от решения перебором.</returns>
    public static double GetDeviation(in SolutionsCollection solutionsCollection)
    {
        double d1 = (solutionsCollection._leapingFrogsSolution.GoalFunction
            - solutionsCollection._bruteForceSolution.GoalFunction);
        double d2 = d1 / solutionsCollection._bruteForceSolution.GoalFunction;
        double d3 = d2 * 100;

        return d3;
    }
}
