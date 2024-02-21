
namespace WorkSchedule.Shared;

using System.Text;
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
        // Обработка некорректных параметров
        // Если количество задач меньше равно нуля
        if (numOfTasks <= 0)
        {
            throw new ArgumentException($"Параметр {nameof(numOfTasks)} был меньше либо равен нулю.");
        }
        // Если один из массивов указывает на null
        if (taskRequiredTime == null || taskArrivalTime == null
            || taskCompletionGoal == null || taskPenalty == null)
        {
            throw new NullReferenceException($"Один из параметров {nameof(taskRequiredTime)}, {nameof(taskArrivalTime)}, " +
                $"{nameof(taskCompletionGoal)}, {nameof(taskPenalty)} имел указатель на null.");
        }
        // Если длина одного из массивов не равна заданному в numOfTasks числу задач
        if (taskRequiredTime.Length != numOfTasks || taskArrivalTime.Length != numOfTasks
            || taskCompletionGoal.Length != numOfTasks || taskPenalty.Length != numOfTasks)
        {
            throw new ArgumentException($"Один из параметров {nameof(taskRequiredTime)}, {nameof(taskArrivalTime)}, " +
                $"{nameof(taskCompletionGoal)}, {nameof(taskPenalty)} имел длину, отличную от " +
                $"значения параметра {nameof(numOfTasks)}.");
        }

        _numOfTasks = numOfTasks;
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
    }
    /// <summary>
    /// Свойство массива времен выполнения работ.
    /// </summary>
    public int[] TaskRequiredTime
    {
        get
        {
            int[] tmpArr = new int[NumOfTasks];
            _taskRequiredTime.CopyTo(tmpArr, 0);
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

    /// <summary>
    /// Печать данных о задаче на консоль.
    /// </summary>
    public void Print()
    {
        Print(this);
    }
    /// <summary>
    /// Печать данных о задаче на консоль.
    /// </summary>
    /// <param name="problemParams">Задача для вывода на консоль.</param>
    /// <exception cref="Exception"></exception>
    public static void Print(ProblemParams problemParams)
    {
        // Проверка корректности параметров
        if (problemParams == null)
        {
            throw new NullReferenceException($"Параметр {nameof(problemParams)} имел указатель на null.");
        }

        WriteLine($"~ Задача для {problemParams.NumOfTasks} работ:");
        Write("Требуемое время для выполнения каждой работы:");
        foreach (int value in problemParams.TaskRequiredTime)
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
        // Проверка параметров
        // Если данные о задаче - пустой указатель
        if (parameters == null)
        {
            throw new NullReferenceException($"Параметр {nameof(parameters)} имел указатель на null.");
        }
        // Если массив - пустой указатель
        if (taskOrder == null)
        {
            throw new NullReferenceException($"Параметр {nameof(taskOrder)} " +
                $"имел указатель на null.");
        }
        // Если длина массива не совпадает с данными задачи
        if (taskOrder.Length != parameters.NumOfTasks)
        {
            throw new ArgumentException($"Массив {nameof(taskOrder)} имел длину, отличную от значения " +
                $"{nameof(parameters.NumOfTasks)}.");
        }

        // Проверка допустимости решения
        if (!ProblemParams.ValidateSolution(parameters, taskOrder))
        {
            // Недопустимое решение - вернуть максимально возможное число типа int
            return int.MaxValue;
        }

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

        /*
        // Штрафы за преждевременное начало работы
        for (int i = 0; i < parameters.NumOfTasks; i++)
        {
            // вычесть из времени начала обработки задачи её время поступления
            tmpVal = parameters.TaskArrivalTime[i] - taskStartTime[i];
            // если начало обработки задачи не ранее времени её поступления - занулить tmpVal
            tmpVal = tmpVal < 0 ? 0 : tmpVal;
            critFunc += tmpVal;
        }
        */

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
    /// Проверка, яляется ли первое решение оптимальнее второго.
    /// </summary>
    /// <param name="firstTaskOrder">Первое решение, с коротым осуществляется сравнение.</param>
    /// <param name="secondTaskOrder">Второе решение.</param>
    /// <param name="isMaxing">Флаг, означающий, что критерий максимизируется.</param>
    /// <returns>true, если первое решение является более оптимальным;<br/>
    /// false, в противном случае.</returns>
    public bool CheckForBetterFitness(in int[] firstTaskOrder, in int[] secondTaskOrder, bool isMaxing = false)
    {
        return CheckForBetterFitness(this, firstTaskOrder, secondTaskOrder, isMaxing);
    }
    /// <summary>
    /// Проверка, яляется ли первое решение оптимальнее второго.
    /// </summary>
    /// <param name="problemParams">Данные о задаче.</param>
    /// <param name="firstTaskOrder">Первое решение, с коротым осуществляется сравнение.</param>
    /// <param name="secondTaskOrder">Второе решение.</param>
    /// <param name="isMaxing">Флаг, означающий, что критерий максимизируется.</param>
    /// <returns>true, если первое решение является более оптимальным;<br/>
    /// false, в противном случае.</returns>
    public static bool CheckForBetterFitness(ProblemParams problemParams, in int[] firstTaskOrder,
        in int[] secondTaskOrder, bool isMaxing = false)
    {
        // Проверка параметров
        // Если данные о задаче - пустой указатель
        if (problemParams == null)
        {
            throw new NullReferenceException($"Параметр {nameof(problemParams)} имел указатель на null.");
        }
        // Если один из массивов - пустой указатель
        if (firstTaskOrder == null || secondTaskOrder == null)
        {
            throw new NullReferenceException($"Один из параметров {nameof(firstTaskOrder)}, " +
                $"{nameof(secondTaskOrder)} имел указатель на null.");
        }
        // Если длина одного из массивов не совпадает с данными задачи
        if (firstTaskOrder.Length != problemParams.NumOfTasks
            || secondTaskOrder.Length != problemParams.NumOfTasks)
        {
            throw new ArgumentException($"Один из массивов {nameof(firstTaskOrder)}, " +
                $"{nameof(secondTaskOrder)} имел длину, отличную от значения " +
                $"{nameof(problemParams.NumOfTasks)}.");
        }

        // Флаг, означающий что первое решение имеет больший критерий в сравнении со вторым решением
        bool isGreater = GetFitness(problemParams, firstTaskOrder) > GetFitness(problemParams, secondTaskOrder);

        // Если критерий первого решения больше критерия второго
        if (isGreater)
        {
            // Вернуть флаг, показывающий, максимизируем ли мы критерий
            return isMaxing;
        }
        // Если критерий первого решения меньше равно критерия второго
        // Вернуть инверсированный флаг, показывающий, максимизируем ли мы критерий
        return !isMaxing;

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
        // Проверка параметров
        // Если данные о задаче - пустой указатель
        if (parameters == null)
        {
            throw new NullReferenceException($"Параметр {nameof(parameters)} имел указатель на null.");
        }
        // Если массив - пустой указатель
        if (taskOrder == null)
        {
            throw new NullReferenceException($"Параметр {nameof(taskOrder)} " +
                $"имел указатель на null.");
        }
        // Если массив нулевой длины
        if (taskOrder.Length == 0)
        {
            throw new ArgumentException($"Массив {nameof(taskOrder)} " +
                $"имел нулевую длину.");
        }

        taskStartTime = new int[taskOrder.Length];
        taskEndTime = new int[taskOrder.Length];
        int curTime = 0;

        for (int i = 0; i < taskOrder.Length; i++)
        {
            taskStartTime[taskOrder[i]] = curTime;
            curTime += parameters.TaskRequiredTime[taskOrder[i]];
            taskEndTime[taskOrder[i]] = curTime;
        }
    }

    /// <summary>
    /// Проверить допустимость решения.
    /// </summary>
    /// <param name="parameters">Начальные условия задачи для выполнения.</param>
    /// <param name="taskOrder">Порядок выполнения работ.</param>
    /// <returns>true, если перестановка допустима;<br/>
    /// false, в противном случае.</returns>
    public static bool ValidateSolution(in ProblemParams parameters, in int[] taskOrder)
    {
        // Проверка параметров
        // Если данные о задаче - пустой указатель
        if (parameters == null)
        {
            throw new NullReferenceException($"Параметр {nameof(parameters)} имел указатель на null.");
        }
        // Если массив - пустой указатель
        if (taskOrder == null)
        {
            throw new NullReferenceException($"Параметр {nameof(taskOrder)} " +
                $"имел указатель на null.");
        }

        int curTime = 0; // текущий такт времени

        // Проверить, корректны ли значения в расписании
        int[] indCount = new int[parameters.NumOfTasks];
        for (int i = 0; i < parameters.NumOfTasks; i++)
        {
            for (int j = 0; j < parameters.NumOfTasks; j++)
            {
                // Если индекс работы на j-й позиции массива равен i
                indCount[i] += (taskOrder[j] == i ? 1 : 0);
            }
        }
        // Если какой-то из индексов не встретился
        if (indCount.Contains(0))
        {
            return false;
        }

        // Проверить, доступны ли работы для выполнения в заданном порядке
        foreach (int i in taskOrder)
        {
            // Если первая работа в очереди на выполнение ещё не поступила
            if (parameters.TaskArrivalTime[i] > curTime)
            {
                return false;
            }
            curTime += parameters.TaskRequiredTime[i];
        }
        return true;
    }

    /// <summary>
    /// Вывести перестановку и значение её целевой функции.
    /// </summary>
    /// <param name="parameters">Данные о задаче.</param>
    /// <param name="taskOrder">Порядок выполнения задач.</param>
    public static void OutputSolutionData(ProblemParams parameters, in int[] taskOrder)
    {
        // Проверка параметров
        // Если данные о задаче - пустой указатель
        if (parameters == null)
        {
            throw new NullReferenceException($"Параметр {nameof(parameters)} имел указатель на null.");
        }
        // Если массив - пустой указатель
        if (taskOrder == null)
        {
            throw new NullReferenceException($"Параметр {nameof(taskOrder)} " +
                $"имел указатель на null.");
        }

        WriteLine("Порядок:" + UserFriendlyArrayToString(taskOrder) + "; Целевая функция: "
            + GetFitness(parameters, taskOrder));
    }

    /// <summary>
    /// Перевод массива в string
    /// </summary>
    /// <param name="array">Массив перестановки.</param>
    /// <returns>Строковое представление массива.</returns>
    public static string? UserFriendlyArrayToString(in int[] array)
    {
        // Если массив указывает на null 
        if (array == null)
        {
            return null;
        }

        StringBuilder sb = new();
        int arrLength = array.Length;

        sb.Append(array[0] + 1);
        for (int index = 1; index < arrLength; index++)
        {
            sb.Append($" {array[index] + 1}");
        }

        return sb.ToString();
    }

    /// <summary>
    /// Перевод массива в string
    /// </summary>
    /// <param name="array">Массив перестановки.</param>
    /// <returns>Строковое представление массива.</returns>
    public static string? TurnArrayToString(in int[] array)
    {
        // Если массив указывает на null 
        if (array == null)
        {
            return null;
        }

        StringBuilder sb = new();
        int arrLength = array.Length;

        sb.Append(array[0]);
        for (int index = 1; index < arrLength; index++)
        {
            sb.Append($" {array[index]}");
        }

        return sb.ToString();
    }

    /// <summary>
    /// Получить строковое представление условий задачи.
    /// </summary>
    /// <returns>Строка с условиями задачи.</returns>
    public override string ToString()
    {
        StringBuilder sb = new();

        sb.Append($"{_numOfTasks}\n");
        sb.Append($"{TurnArrayToString(TaskRequiredTime)}\n");
        sb.Append($"{TurnArrayToString(TaskArrivalTime)}\n");
        sb.Append($"{TurnArrayToString(TaskCompletionGoal)}\n");
        sb.Append(TurnArrayToString(TaskPenalty));

        return sb.ToString();
    }
}