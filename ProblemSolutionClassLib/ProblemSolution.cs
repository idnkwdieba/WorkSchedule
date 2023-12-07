
// To-do: Собственное исключение TaskNullException

namespace WorkSchedule.Shared;

using static System.Console;

public class ProblemSolution
{
    private ProblemParams _problem; // исходные данные о задаче
    private int[]? _tasksOrder; // порядок выполнения работ
    private int[]? _taskStartTime; // время начала выполнения каждой работы
    private int[]? _taskEndTime; // время окончания выполнения каждой работы
    private int _goalFunction; // целевая функция

    public ProblemSolution(ProblemParams problem)
    {
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
    public int GoalFunction
    {
        get
        {
            return _goalFunction;
        }
        private set
        {
            _goalFunction = value;
        }
    }

    // Решение перебором
    public void BruteForceSolution()
    {
        BruteForceSolution(this);
    }
    public static void BruteForceSolution(ProblemSolution solution)
    {
        // Вызов алгоритма перебора
        BruteForceAlgorithm.RunBruteForceAlg(solution.Problem, ref solution._tasksOrder!);
        solution.GoalFunction = ProblemParams.GetFitness(solution.Problem, solution._tasksOrder!);
        ProblemParams.GetStartEndTime(solution.Problem, solution._tasksOrder,
            out solution._taskStartTime, out solution._taskEndTime);
    }
    public void Print()
    {
        Print(this);
    }
    public static void Print(ProblemSolution solution)
    {
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
        WriteLine("~ Оптимальное решение задачи (метод перебора):");
        Write("Порядок выполнения работ:");
        foreach (int value in solution.TaskOrder)
        {
            Write(" " + value);
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

    private static void WriteColor(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Write(message);
        Console.ForegroundColor = ConsoleColor.White;
    }
}
