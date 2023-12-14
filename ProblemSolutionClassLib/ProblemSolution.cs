
namespace WorkSchedule.Shared;

using static System.Console;

public class ProblemSolution
{
    private ProblemParams _problem; // исходные данные о задаче
    private int[]? _tasksOrder; // порядок выполнения работ
    private int[]? _taskStartTime; // время начала выполнения каждой работы
    private int[]? _taskEndTime; // время окончания выполнения каждой работы

    /// <summary>
    /// Конструктор экземпляра класса ProblemSolution.
    /// </summary>
    /// <param name="problem">Задача для решения.</param>
    public ProblemSolution(ProblemParams problem)
    {
        // Проверка данных
        // Если передан указатель на null
        if (problem == null)
        {
            throw new NullReferenceException($"Параметр {nameof(problem)} " +
                $"имел указатель на null.");
        }

        Problem = problem;
    }

    /// <summary>
    /// Свойство задачи для решения.
    /// </summary>
    public ProblemParams Problem
    {
        get
        {
            return _problem;
        }
        init
        {
            if (value == null)
            {
                // To-do: Собственное исключение TaskNullException
                throw new Exception();
            }

            _problem = value;
        }
    }
    /// <summary>
    /// Свойство порядка выполнения работ.
    /// </summary>
    public int[]? TaskOrder
    {
        get
        {
            if (_tasksOrder == null)
            {
                return null;
            }

            int[] tmpArr = new int[Problem.NumOfTasks];
            _tasksOrder.CopyTo(tmpArr, 0);
            return tmpArr;
        }
        private set
        {
            if (value == null)
            {
                _tasksOrder = value;
                return;
            }

            _tasksOrder = new int[Problem.NumOfTasks];
            value.CopyTo(_tasksOrder, 0);
        }
    }
    /// <summary>
    /// Свойство массива времен начала выполнения работ.
    /// </summary>
    public int[]? TaskStartTime
    {
        get
        {
            if (_taskStartTime == null)
            {
                return null;
            }

            int[] tmpArr = new int[Problem.NumOfTasks];
            _taskStartTime.CopyTo(tmpArr, 0);
            return tmpArr;
        }
        private set
        {
            if (value == null)
            {
                _taskEndTime = value;
                return;
            }

            _taskStartTime = new int[Problem.NumOfTasks];
            value.CopyTo(_taskStartTime, 0);
        }
    }
    /// <summary>
    /// Свойство массива времен окончания выполнения работ.
    /// </summary>
    public int[]? TaskEndTime
    {
        get
        {
            if (_taskEndTime == null)
            {
                return null;
            }

            int[] tmpArr = new int[Problem.NumOfTasks];
            _taskEndTime.CopyTo(tmpArr, 0);
            return tmpArr;
        }
        private set
        {
            if (value == null)
            {
                _taskEndTime = value;
                return;
            }

            _taskEndTime = new int[Problem.NumOfTasks];
            value.CopyTo(_taskEndTime, 0);
        }
    }
    /// <summary>
    /// Свойство целевой функции.
    /// </summary>
    public int GoalFunction
    {
        get
        {
            if (_tasksOrder == null)
            {
                return int.MaxValue;
            }
            return ProblemParams.GetFitness(_problem, _tasksOrder);
        }
    }

    /// <summary>
    /// Решение задачи с помощью полного перебора.
    /// </summary>
    public void BruteForceSolution()
    {
        BruteForceSolution(this);
    }
    /// <summary>
    /// Решение задачи с помощью полного перебора.
    /// </summary>
    /// <param name="solution">Решение, в которые сохраняются результаты.</param>
    public static void BruteForceSolution(ProblemSolution solution)
    {
        // Проверка данных
        // Если передан указатель на null
        if (solution == null)
        {
            throw new NullReferenceException($"Параметр {nameof(solution)} " +
                $"имел указатель на null.");
        }

        // Вызов алгоритма перебора
        BruteForceAlgorithm.RunBruteForceAlg(solution.Problem, ref solution._tasksOrder!);
        ProblemParams.GetStartEndTime(solution.Problem, solution._tasksOrder,
            out solution._taskStartTime, out solution._taskEndTime);
    }

    /// <summary>
    /// Решение тасующим методом прыгающих лягушек.
    /// </summary>
    public void LeapingFrogsSolution()
    {
        LeapingFrogsSolution(this);
    }
    /// <summary>
    /// Решение тасующим методом прыгающих лягушек.
    /// </summary>
    /// <param name="solution">Решение, в которые сохраняются результаты.</param>
    public static void LeapingFrogsSolution(ProblemSolution solution)
    {
        // Проверка данных
        // Если передан указатель на null
        if (solution == null)
        {
            throw new NullReferenceException($"Параметр {nameof(solution)} " +
                $"имел указатель на null.");
        }

        // Вызов тасующего алгоритма прыгающих лягушек
        FrogsAlgorithm.RunFrogsAlg(solution.Problem, ref solution._tasksOrder!);
        ProblemParams.GetStartEndTime(solution.Problem, solution._tasksOrder,
            out solution._taskStartTime, out solution._taskEndTime);
    }

    /// <summary>
    /// Вывод данных о решении задачи.
    /// </summary>
    public void Print()
    {
        Print(this);
    }
    /// <summary>
    /// Вывод данных о решении задачи.
    /// </summary>
    /// <param name="solution">Решение, в которые сохраняются результаты.</param>
    public static void Print(ProblemSolution solution)
    {
        // Проверка данных
        // Если передан указатель на null
        if (solution == null)
        {
            throw new NullReferenceException($"Параметр {nameof(solution)} " +
                $"имел указатель на null.");
        }

        // проверка наличия решения
        if (solution.Problem == null)
        {
            WriteColor("Задача не была задана!");
            return;
        }
        if (solution.TaskOrder == null || solution.TaskStartTime == null
            || solution.TaskEndTime == null || solution.GoalFunction == 0)
        {
            WriteColor("Решение ещё не было найдено!");
            return;
        }

        solution.Problem.Print();
        WriteLine("~ Оптимальное решение задачи, найденное методом:");
        Write("Порядок выполнения работ:");
        foreach (int value in solution.TaskOrder)
        {
            Write(" " + (value+1));
        }
        WriteLine();
        Write("Время начала выполнения работ:");
        foreach (int value in solution.TaskStartTime)
        {
            Write(" " + value);
        }
        WriteLine();
        Write("Время окончания выполнения работ:");
        foreach (int value in solution.TaskEndTime)
        {
            Write(" " + value);
        }
        WriteLine();
        Write($"Целевая функция: {solution.GoalFunction}");
    }

    /// <summary>
    /// Вывод цветной строки.
    /// </summary>
    /// <param name="message">Сообщение для вывода.</param>
    private static void WriteColor(string message)
    {
        // если передан указатель на null
        if (message == null)
        {
            return;
        }

        Console.ForegroundColor = ConsoleColor.Red;
        Write(message);
        Console.ForegroundColor = ConsoleColor.White;
    }
}
