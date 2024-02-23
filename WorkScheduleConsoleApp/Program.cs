using WorkSchedule.Shared;

internal class Program
{
    private static void Main(string[] args)
    {
        // 1. Заданные условия задачи
        /*
        // Условия задачи

        int numberOfTasks = 5;
        int[] taskRequiredTime = new[]{ 2, 3, 2, 3, 1 };
        int[] taskArrivalTime = new[]{ 0, 0, 1, 2, 2 };
        int[] taskCompletionGoal = new[]{ 1, 4, 2, 5, 6 };
        int[] taskPenalty = new[]{ 1, 1, 2, 1, 2 };

        ProblemParams exampleProblem = new(numberOfTasks, taskRequiredTime, taskArrivalTime,
            taskCompletionGoal, taskPenalty);

        // Алгоритм полного перебора
        ProblemSolution exampleSolutionBF = new(exampleProblem);
        exampleSolutionBF.BruteForceSolution();
        WriteLine("| Алгоритм полного перебора:");
        exampleSolutionBF.Print();

        WriteLine("\n");

        // Тасующий алгоритм прыгающих лягушек
        ProblemSolution exampleSolutionLF = new(exampleProblem);
        exampleSolutionLF.LeapingFrogsSolution();
        WriteLine("| Тасующий алгоритм прыгающих лягушек:");
        exampleSolutionLF.Print();

        WriteLine("\n");

        // ЭГА
        ProblemSolution exampleSolutionEga = new(exampleProblem);
        exampleSolutionEga.EgaSolution();
        WriteLine("| ЭГА:");
        exampleSolutionLF.Print();

        WriteLine("\n");
        */

        // 2. Случайная генерация задачи
        /*
        ProblemParams exampleProblem = UniformDistribution.GenerateProblemUD(5);

        // Алгоритм полного перебора
        ProblemSolution exampleSolutionBF = new(exampleProblem);
        exampleSolutionBF.BruteForceSolution();
        WriteLine("| Алгоритм полного перебора:");
        exampleSolutionBF.Print();

        WriteLine("\n");

        // Тасующий алгоритм прыгающих лягушек
        ProblemSolution exampleSolutionLF = new(exampleProblem);
        exampleSolutionLF.LeapingFrogsSolution();
        WriteLine("| Тасующий алгоритм прыгающих лягушек:");
        exampleSolutionLF.Print();

        WriteLine("\n");

        // ЭГА
        ProblemSolution exampleSolutionEga = new(exampleProblem);
        exampleSolutionEga.EgaSolution();
        WriteLine("| ЭГА:");
        exampleSolutionLF.Print();

        WriteLine("\n");
        */



        // 3. Использоватие SolutionCollection и вывод отклонений
        /*
        ProblemParams exampleProblem = UniformDistribution.GenerateProblemUD(10);

        exampleProblem.Print();
        SolutionsCollection solutions = new(exampleProblem);
        solutions.PrintSolutions();
        WriteLine($"Отклонение: " + solutions.GetDeviationsString());
        */


        // 4. Генерация условий и нахождение наилучшего решения 100 задач.

        // ProblemSimulation.GenerateProblems();

        // 5. Прогонка задач из файла с сохранением отклонений в другой файл.

        ProblemSimulation.RunSimulation();
    }
}