using System;
using Microsoft.Data.SqlClient;

class Program
{
    static string connectionString = "Server=localhost\\SQLEXPRESS;Database=EmployeeManagementDB;Trusted_Connection=True;TrustServerCertificate=True;";
    static void Main()
    {
        bool running = true;
        while(running)
        {
            Console.WriteLine("\n--TO-DO LIST APP--");
            Console.WriteLine("1. Add Task");
            Console.WriteLine("2. View All Tasks");
            Console.WriteLine("3. Mark Task as completed");
            Console.WriteLine("4. Delete Task");
            Console.WriteLine("5. Exit");
            Console.Write("choice: ");

            string choice = Console.ReadLine() ??"";

            if(choice =="1")AddTask();
            else if(choice =="2") ViewTasks();
            else if(choice =="3")MarkTaskCompleted();
            else if (choice =="4")DeleteTask();
            else if (choice =="5"){running =false; Console.WriteLine("Bye!");}
            else Console.WriteLine("Wrong Choice,try again.");

        }

    }
   static void AddTask()
    {
        Console.Write("Enter Task Title: ");
        string title = Console.ReadLine() ?? "";

        Console.Write("Enter Description: ");
        string description = Console.ReadLine() ?? "";

        Console.Write("Enter Due Date (yyyy-mm-dd): ");
        DateTime dueDate = DateTime.Parse(Console.ReadLine() ?? DateTime.Now.ToString());

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            string query = "INSERT INTO Tasks (Title, Description, DueDate, IsCompleted) VALUES (@Title, @Description, @DueDate, 0)";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@Title", title);
            cmd.Parameters.AddWithValue("@Description", description);
            cmd.Parameters.AddWithValue("@DueDate", dueDate);

            conn.Open();
            cmd.ExecuteNonQuery();
            Console.WriteLine("Task added successfully!");
        }
    }
    static void ViewTasks()
    {
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            string query = "SELECT Id , Title ,Description,DueDate,IsCompleted FROM Tasks";
            SqlCommand cmd = new SqlCommand(query,conn);

            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            bool anyrows = false;
            while(reader.Read())
            {
                anyrows = true;
                string status = reader.GetBoolean(4) ?"Completed":"Pending";
                Console.WriteLine($"Id:{reader.GetInt32(0)},Title:{reader.GetString(1)},Description:{reader.GetString(2)},Due:{reader.GetDateTime(3):dd-mm-yyyy},Status:{status}");

            }
            if(!anyrows)Console.WriteLine("No Tasks found.");

        }
    }
    static void MarkTaskCompleted()
    {
        Console.Write("Enter Task Id to mark as completed: ");
        int id = int.Parse(Console.ReadLine()??"0");

        using(SqlConnection conn = new SqlConnection(connectionString))
        {
            string query = "UPDATE Tasks SET IsCompleted = 1 WHERE Id = @Id";
            SqlCommand cmd = new SqlCommand(query,conn);
            cmd.Parameters.AddWithValue("@Id",id);

            conn.Open();
            int rows = cmd.ExecuteNonQuery();
            Console.WriteLine(rows>0 ? "Task markad as completed":"Task not found.");

        }
    }

    static void DeleteTask()
    {
        Console.Write("Enter Task Id to delete: ");
        int id = int.Parse(Console.ReadLine() ??"0");

        using(SqlConnection conn = new SqlConnection(connectionString))
        {
            string query = "DELETE FROM Tasks  WHERE Id = @Id";
            SqlCommand cmd = new SqlCommand(query,conn);
            cmd.Parameters.AddWithValue("@Id",id);

            conn.Open();
            int rows = cmd.ExecuteNonQuery();
            Console.WriteLine(rows>0?"Task marked as completed !":"Task not found.");
        }
    }






}