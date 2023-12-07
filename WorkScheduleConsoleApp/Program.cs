

using WorkSchedule.Shared;
using static System.Console;

internal class Program
{
    private static void Main(string[] args)
    {
        // Условия задачи
        int numberOfTasks = 5;
        int[] taskRequiredTime = new[]{ 1, 2, 2, 3, 2 };
        int[] taskArrivalTime = new[]{ 0, 1, 1, 2, 3 };
        int[] taskCompletionGoal = new[]{ 5, 8, 6, 9, 10 };
        int[] taskPenalty = new[]{ 1, 1, 2, 1, 2 };

        ProblemParams exampleProblem = new(numberOfTasks, taskRequiredTime, taskArrivalTime,
            taskCompletionGoal, taskPenalty);

        ProblemSolution exampleSolution = new(exampleProblem);
        exampleSolution.BruteForceSolution();
        WriteLine();
        exampleSolution.Print();
    }
}