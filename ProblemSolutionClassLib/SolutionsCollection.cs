
namespace WorkSchedule.Shared;

using static System.Console;

public class SolutionsCollection
{
    private ProblemParams _problemParams; // данные о задаче
    private int _optimalFitness; // Критерий самого оптимального решения
    private ProblemSolution _leapingFrogsSolution; // решение тасующим алгоритмом прыгающих лягушек
    private ProblemSolution _egaSolution; // решение эволюционно-генетическим алгоритмом
    private ProblemSolution _egaSolutionSecond; // решение второй версией эволюционно-генетического алгоритма
    /// <summary>
    /// Конструктор экземпляра класса.
    /// </summary>
    /// <param name="parameters">Данные о задаче.</param>
    public SolutionsCollection(ProblemParams parameters, int optimalFitness)
    {
        // Проверка корректности передаваемых параметров
        if (parameters == null)
        {
            throw new ArgumentNullException($"Переменная {nameof(parameters)} указывала на null.");
        }

        _problemParams = parameters;
        _optimalFitness = optimalFitness;

        _leapingFrogsSolution = new ProblemSolution(parameters);
        _leapingFrogsSolution.LeapingFrogsSolution();

        _egaSolution = new ProblemSolution(parameters);
        _egaSolution.EgaSolution();

        _egaSolutionSecond = new ProblemSolution(parameters);
        _egaSolutionSecond.EgaSolution(true);
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
    /// <param name="algType">Тип алгоритма.</param>
    /// <returns>Процент отклонения от решения перебором.</returns>
    public double GetDeviation(int algType)
    {
        return GetDeviation(this, algType);
    }

    /// <summary>
    /// Получение отклонения решения ЭГА или алгоритмом прыгающих лягушек от решения перебором.
    /// </summary>
    /// <param name="solutionsCollection">Коллекция решений.</param>
    /// <param name="algType">Тип алгоритма.</param>
    /// <returns>Процент отклонения решения алгоритмом прыгающих лягушек от решения перебором.</returns>
    public static double GetDeviation(in SolutionsCollection solutionsCollection, int algType = 0)
    {
        // Проверка данных
        // Если передан указатель на null
        if (solutionsCollection == null)
        {
            throw new NullReferenceException($"Параметр {nameof(solutionsCollection)} " +
                $"имел указатель на null.");
        }

        double d1 = 0, d2, d3;

        switch (algType)
        {
            case 0:
                // Для алгоритма прыгающих лягушек.
                d1 = (solutionsCollection._leapingFrogsSolution.GoalFunction
                    - solutionsCollection._optimalFitness);
                break;
            case 1:
                // Для ЭГА.
                d1 = (solutionsCollection._egaSolution.GoalFunction
                    - solutionsCollection._optimalFitness);
                break;
            case 2:
                // Для ЭГА второй версии.
                d1 = (solutionsCollection._egaSolutionSecond.GoalFunction
                    - solutionsCollection._optimalFitness);
                break;

        }

        d2 = d1 / solutionsCollection._optimalFitness;
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
        double egaDev = GetDeviation(in solutionsCollection, 1);
        double egaDev2 = GetDeviation(in solutionsCollection, 2);

        return $"{frogsDev:0.00} {egaDev:0.00} {egaDev2:0.00}";
    }
}
