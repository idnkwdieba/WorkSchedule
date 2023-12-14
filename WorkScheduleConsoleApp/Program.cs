
using WorkSchedule.Shared;
using static System.Console;

internal class Program
{
    private static void Main(string[] args)
    {
        // Заданные условия задачи
        /*
        // Условия задачи
        int numberOfTasks = 5;
        int[] taskRequiredTime = new[]{ 2, 3, 2, 3, 1 };
        int[] taskArrivalTime = new[]{ 0, 0, 1, 2, 2 };
        int[] taskCompletionGoal = new[]{ 1, 4, 2, 5, 6 };
        int[] taskPenalty = new[]{ 1, 1, 2, 1, 2 };

        ProblemParams exampleProblem = new(numberOfTasks, taskRequiredTime, taskArrivalTime,
            taskCompletionGoal, taskPenalty);
        */

        // Случайная генерация задачи
        /*
        ProblemParams exampleProblem = UniformDistribution.GenerateProblemUD();

        // Алгоритм полного перебора
        ProblemSolution exampleSolutionBF = new(exampleProblem);
        exampleSolutionBF.BruteForceSolution();
        WriteLine();
        exampleSolutionBF.Print();

        WriteLine();

        // Тасующий алгоритм прыгающих лягушек
        ProblemSolution exampleSolutionLF = new(exampleProblem);
        exampleSolutionLF.LeapingFrogsSolution();
        WriteLine();
        exampleSolutionLF.Print();

        WriteLine();
        */

        //ProblemParams exampleProblem = UniformDistribution.GenerateProblemUD();

        int numberOfTasks = 5;
        int[] taskRequiredTime = new[] { 2, 5, 3, 1, 4 };
        int[] taskArrivalTime = new[] { 2, 2, 0, 0, 1 };
        int[] taskCompletionGoal = new[] { 7, 4, 1, 2, 3 };
        int[] taskPenalty = new[] { 3, 1, 3, 2, 3 };

        ProblemParams exampleProblem = new(numberOfTasks, taskRequiredTime, taskArrivalTime,
            taskCompletionGoal, taskPenalty);

        exampleProblem.Print();
        SolutionsCollection solutions = new(exampleProblem);
        WriteLine(ProblemParams.ValidateSolution(exampleProblem, solutions._leapingFrogsSolution.TaskOrder!));
        solutions.PrintSolutions();
        WriteLine(solutions.GetDeviation());
    }
}