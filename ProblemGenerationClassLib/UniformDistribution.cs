namespace WorkSchedule.Shared;

public static class UniformDistribution
{
    /// <summary>
    /// Генерация данных о задаче с помощью равномерного распределения.
    /// </summary>
    /// <param name="numOfTasks">Число работ в задаче.</param>
    /// <returns>Задачу расписаний для одного станка.</returns>
    /// <exception cref="ArgumentException"></exception>
    public static ProblemParams GenerateProblemUD(int numOfTasks = 10)
    {
        // Если переданы некорректные входные параметры
        if (numOfTasks <= 1)
        {
            throw new ArgumentException($"{nameof(numOfTasks)} had incorrect value.");
        }

        Random rand = new Random();

        int[] requiredTimeArr = new int[numOfTasks]; // необходимое на выполнение время
        int[] arrivalTimeArr = new int[numOfTasks]; // время поступления
        int[] completionGoalArr = new int[numOfTasks]; // целевое время завершения
        int[] penaltyArr = new int[numOfTasks]; // штрафы за нарушение сроков

        // Генерация данных задачи равномерным распределением
        for (int i = 0; i < numOfTasks; i++)
        {
            requiredTimeArr[i] = rand.Next(1, 6);
            arrivalTimeArr[i] = rand.Next(0, numOfTasks / 2 + numOfTasks % 2);
            completionGoalArr[i] = rand.Next(arrivalTimeArr[i] + 1, arrivalTimeArr[i] + numOfTasks);
            penaltyArr[i] = rand.Next(1, 4);
        }

        // Если нет ни одной работы, которая поступает в самом начале
        if (!arrivalTimeArr.Contains(0))
        {
            // Присвоить случайной работе нулевое время поступления
            arrivalTimeArr[rand.Next(0, arrivalTimeArr.Length)] = 0;
        }

        // Вернуть экземпляр задачи
        return new ProblemParams(numOfTasks, requiredTimeArr, arrivalTimeArr,
            completionGoalArr, penaltyArr);
    }
}