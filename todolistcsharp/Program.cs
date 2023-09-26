using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public static class TaskMapping
{
    private static readonly Dictionary<int, string> PriorityMap = new Dictionary<int, string>
    {
        { 0, "Scazuta" },
        { 1, "Medie" },
        { 2, "Inalta" }
    };

    private static readonly Dictionary<bool, string> StatusMap = new Dictionary<bool, string>
    {
        { false, "Nefinalizata" },
        { true, "Finalizata" }
    };

    public static string MapPriority(int priority)
    {
        if (PriorityMap.TryGetValue(priority, out var priorityText))
        {
            return priorityText;
        }
        return "Necunoscuta";
    }

    public static string MapStatus(bool isCompleted)
    {
        if (StatusMap.TryGetValue(isCompleted, out var statusText))
        {
            return statusText;
        }
        return "Necunoscuta";
    }
}

class TaskManager
{
    private List<TaskItem> tasks = new List<TaskItem>();
    private string saveFilePath = "tasks.txt";

    public TaskManager()
    {
        
    }

    public void Run()
    {
        

        // Adăugare sarcini implicite aici
        tasks.Add(new TaskItem("Sarcina 1", 0));
        tasks.Add(new TaskItem("Sarcina 2", 0));
        tasks.Add(new TaskItem("Sarcina 3", 1));

        bool exit = false;
        bool unsavedChanges = false;

        while (!exit)
        {
            Console.WriteLine("\nOptiuni:");
            Console.WriteLine("1. Afisează lista de sarcini");
            Console.WriteLine("2. Adauga sarcina noua");
            Console.WriteLine("3. Marcheaza sarcina ca finalizata");
            Console.WriteLine("4. Sterge sarcina");
            Console.WriteLine("5. Salvare sarcini");
            Console.WriteLine("6. Iesire");

            int choice = GetUserChoice(1, 6);

            switch (choice)
            {
                case 1:
                    DisplayTasks();
                    break;
                case 2:
                    AddTask();
                    unsavedChanges = true; 
                    break;
                case 3:
                    MarkTaskAsCompleted();
                    unsavedChanges = true; 
                    break;
                case 4:
                    DeleteTask();
                    unsavedChanges = true; 
                    break;
                case 5:
                    SaveTasks();
                    unsavedChanges = false;
                    break;
                case 6:
                    if (unsavedChanges)
                    {
                        Console.Write("Doriti sa salvati modificările nesalvate? (da/nu): ");
                        string response = Console.ReadLine().ToLower();
                        if (response == "da")
                        {
                            SaveTasks();
                        }
                    }
                    exit = true;
                    break;
            }
        }
    }

    private int GetUserChoice(int minValue, int maxValue)
    {
        int choice;
        while (true)
        {
            Console.Write("Introduceti optiunea: ");
            if (int.TryParse(Console.ReadLine(), out choice) && choice >= minValue && choice <= maxValue)
            {
                return choice;
            }
            Console.WriteLine("Optiune invalida. Va rugam sa introduceti o optiune valida.");
        }
    }

    private void DisplayTasks()
    {
        Console.WriteLine("\nLista de sarcini:");

        if (tasks.Count == 0)
        {
            Console.WriteLine("Nu exista sarcini de afisat.");
        }
        else
        {
            foreach (var task in tasks)
            {
                Console.WriteLine(task);
            }
        }
    }

    private void AddTask()
    {
        Console.Write("Introduceti o noua sarcina: ");
        string taskDescription = Console.ReadLine();

        Console.Write("Introduceti prioritatea (0-Scazuta, 1-Medie, 2-Inalta): ");
        int priority = GetUserChoice(0, 2);

        TaskItem newTask = new TaskItem(taskDescription, priority);
        tasks.Add(newTask);
        Console.WriteLine("Sarcina adaugata cu succes!");
    }

    private void MarkTaskAsCompleted()
    {
        DisplayTasks();
        if (tasks.Count == 0)
        {
            return;
        }

        Console.Write("Introduceti numarul sarcinii de marcat ca finalizata: ");
        int taskIndex = GetUserChoice(1, tasks.Count) - 1;

        TaskItem task = tasks[taskIndex];
        task.IsCompleted = true;
        Console.WriteLine($"Sarcina '{task.Description}' a fost marcata ca finalizata.");
    }

    private void DeleteTask()
    {
        DisplayTasks();
        if (tasks.Count == 0)
        {
            return;
        }

        Console.Write("Introduceti numarul sarcinii de sters: ");
        int taskIndex = GetUserChoice(1, tasks.Count) - 1;

        TaskItem deletedTask = tasks[taskIndex];
        tasks.RemoveAt(taskIndex);
        Console.WriteLine($"Sarcina '{deletedTask.Description}' a fost stearsă.");
    }

    private void SaveTasks()
    {
        using (StreamWriter writer = new StreamWriter(saveFilePath))
        {
            foreach (var task in tasks)
            {
                string priorityText = TaskMapping.MapPriority(task.Priority);
                string statusText = TaskMapping.MapStatus(task.IsCompleted);

                string formattedTask = $"Sarcina \"{task.Description}\" cu dificultatea \"{priorityText}\" este \"{statusText}\"";
                writer.WriteLine(formattedTask);
            }
        }
        Console.WriteLine("Sarcinile au fost salvate în fisier.");
    }
}

class TaskItem
{
    public string Description { get; }
    public int Priority { get; }
    public bool IsCompleted { get; set; }

    public TaskItem(string description, int priority, bool isCompleted = false)
    {
        Description = description;
        Priority = priority;
        IsCompleted = isCompleted;
    }

    public override string ToString()
    {
        string priorityText = TaskMapping.MapPriority(Priority);
        string status = TaskMapping.MapStatus(IsCompleted);

        return $"{Description} (Prioritate: {priorityText}, Stare: {status})";
    }
}

class Program
{
    static void Main(string[] args)
    {
        TaskManager taskManager = new TaskManager();
        taskManager.Run();
    }
}
