
// To-do: Свой собственный класс исключений TaskQuantityException, TaskParamsNullException, TaskParamsArrayException

namespace WorkSchedule.Shared;

using static System.Console;

public class ProblemParams
{
    // Поля класса
    private int _numOfTasks; // количество работ для выполнения
    private int[] _taskRequiredTime; // время, требуемое на выполнение работы
    private int[] _taskArrivalTime; // время поступления работы
    private int[] _taskCompletionGoal; // крайний срок выполнения работы
    private int[] _taskPenalty; // штраф за такт отклонения от директивных сроков

    /// <summary>
    /// конструктор класса ProblemParams.
    /// </summary>
    public ProblemParams(int numOfTasks, int[] taskRequiredTime, int[] taskArrivalTime,
        int[] taskCompletionGoal, int[] taskPenalty)
    {
        if (numOfTasks <= 0)
        {
            // To-do: Исключение, сообщающее, что количество задач меньше равно нуля
            throw new Exception();
        }
        if (taskRequiredTime == null || taskArrivalTime == null 
            || taskCompletionGoal == null || taskPenalty == null)
        {
            // To-do: исключение, сообщающее, что указатель на массив параметров
            // является указателем на null
            throw new Exception();
        }
        if (taskRequiredTime.Length != numOfTasks || taskArrivalTime.Length != numOfTasks
            || taskCompletionGoal.Length != numOfTasks || taskPenalty.Length != numOfTasks)
        {
            // To-do: Исключение, сообщающее, что один из массивов не содержит нужного числа элементов
            throw new Exception();
        }

        NumOfTasks = numOfTasks;
        TaskRequiredTime = taskRequiredTime;
        TaskArrivalTime = taskArrivalTime;
        TaskCompletionGoal = taskCompletionGoal;
        TaskPenalty = taskPenalty;
    }

    /// <summary>
    /// Свойство числа работ для выполнения.
    /// </summary>
    public int NumOfTasks
    {
        get
        {
            return _numOfTasks;
        }
        init
        {
            _numOfTasks = value;
        }
    }
    /// <summary>
    /// Свойство массива времен выполнения работ.
    /// </summary>
    public int[] TaskRequiredTime
    {
        get
        {
            int[] tmpArr = new int[NumOfTasks];
            _taskRequiredTime.CopyTo(tmpArr,0);
            return tmpArr;
        }
        init
        {
            _taskRequiredTime = new int[NumOfTasks];
            value.CopyTo(_taskRequiredTime, 0);
        }
    }
    /// <summary>
    /// Свойство массива времени поступления работ.
    /// </summary>
    public int[] TaskArrivalTime
    {
        get
        {
            int[] tmpArr = new int[NumOfTasks];
            _taskArrivalTime.CopyTo(tmpArr, 0);
            return tmpArr;
        }
        init
        {
            _taskArrivalTime = new int[NumOfTasks];
            value.CopyTo(_taskArrivalTime, 0);
        }
    }
    /// <summary>
    /// Свойство массива целевого времени выполнения работ.
    /// </summary>
    public int[] TaskCompletionGoal
    {
        get
        {
            int[] tmpArr = new int[NumOfTasks];
            _taskCompletionGoal.CopyTo(tmpArr, 0);
            return tmpArr;
        }
        init
        {
            _taskCompletionGoal = new int[NumOfTasks];
            value.CopyTo(_taskCompletionGoal, 0);
        }
    }
    /// <summary>
    /// Свойство массива штрафов за нарушение целевого времени выполнения работы на один такт.
    /// </summary>
    public int[] TaskPenalty
    {
        get
        {
            int[] tmpArr = new int[NumOfTasks];
            _taskPenalty.CopyTo(tmpArr, 0);
            return tmpArr;
        }
        init
        {
            _taskPenalty = new int[NumOfTasks];
            value.CopyTo(_taskPenalty, 0);
        }
    }
    public void Print()
    {
        Print(this);
    }
    public static void Print(ProblemParams problemParams)
    {
        if (problemParams.NumOfTasks <= 0)
        {
            // To-do: Исключение, сообщающее, что количество задач меньше равно нуля
            throw new Exception();
        }
        if (problemParams.TaskRequiredTime == null || problemParams.TaskArrivalTime == null
            || problemParams.TaskCompletionGoal == null || problemParams.TaskPenalty == null)
        {
            // To-do: исключение, сообщающее, что указатель на массив параметров
            // является указателем на null
            throw new Exception();
        }

        WriteLine($"~ Задача для {problemParams.NumOfTasks} работ:");
        Write("Требуемое время для выполнения каждой работы:");
        foreach(int value in problemParams.TaskRequiredTime)
        {
            Write(" " + value);
        }
        WriteLine();
        Write("Время поступления каждой работы:");
        foreach (int value in problemParams.TaskArrivalTime)
        {
            Write(" " + value);
        }
        WriteLine();
        Write("Целевое время выполнения каждой работы:");
        foreach (int value in problemParams.TaskCompletionGoal)
        {
            Write(" " + value);
        }
        WriteLine();
        Write("Штраф за нарушение директивных сроков каждой работы:");
        foreach (int value in problemParams.TaskPenalty)
        {
            Write(" " + value);
        }
        WriteLine();
    }

    /// <summary>
    /// Вычисление целевой функции для указанных параметров.
    /// </summary>
    /// <param name="parameters">Начальные условия задачи расписаний.</param>
    /// <param name="taskOrder">Порядок выполнения работ.</param>
    /// <returns>Значение целевой функции.</returns>
    public static int GetFitness(in ProblemParams parameters, in int[] taskOrder)
    {
        int critFunc = int.MinValue;
        int tmpVal;

        int[] taskStartTime;
        int[] taskEndTime;

        // Вычисление времен начала и окончания работ
        GetStartEndTime(parameters, taskOrder, out taskStartTime, out taskEndTime);

        // Основная критериальная функция
        for (int i = 0; i < parameters.NumOfTasks; i++)
        {
            tmpVal = Math.Abs(taskEndTime[i] - parameters.TaskCompletionGoal[i]);
            critFunc = tmpVal > critFunc ? tmpVal : critFunc;
        }

        // Штрафы за преждевременное начало работы
        for (int i = 0; i < parameters.NumOfTasks; i++)
        {
            // вычесть из времени начала обработки задачи её время поступления
            tmpVal = parameters.TaskArrivalTime[i] - taskStartTime[i];
            // если начало обработки задачи не ранее времени её поступления - занулить tmpVal
            tmpVal = tmpVal < 0 ? 0 : tmpVal;
            critFunc += tmpVal;
        }

        // Штрафы за нарушение крайних сроков работы
        for (int i = 0; i < parameters.NumOfTasks; i++)
        {
            // вычесть из крайнего срока окончания обработки задачи её время окончания обработки
            tmpVal = taskEndTime[i] - parameters.TaskCompletionGoal[i];
            // если завершение обработки задачи не позднее крайнего срока - занулить tmpVal
            tmpVal = tmpVal < 0 ? 0 : tmpVal;
            critFunc += tmpVal;
        }

        return critFunc;
    }

    /// <summary>
    /// Вычислить времена начала и окончания выполнения работ.
    /// </summary>
    /// <param name="parameters">Начальные условия задачи для выполнения.</param>
    /// <param name="taskOrder">Порядок выполнения работ.</param>
    /// <param name="taskStartTime">Время начала выполнения каждой работы.</param>
    /// <param name="taskEndTime">Время окончания выполнения каждой работы.</param>
    public static void GetStartEndTime(in ProblemParams parameters, in int[] taskOrder,
        out int[] taskStartTime, out int[] taskEndTime)
    {
        taskStartTime = new int[taskOrder.Length];
        taskEndTime = new int[taskOrder.Length];
        int curTime = 0;

        for (int i = 0; i < taskOrder.Length; i++)
        {
            taskStartTime[taskOrder[i] - 1] = curTime;
            curTime += parameters.TaskRequiredTime[taskOrder[i] - 1];
            taskEndTime[taskOrder[i] - 1] = curTime;
        }
    }
}