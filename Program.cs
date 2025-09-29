using System.Reflection.Metadata;

namespace Bibloteket
{
    class Program
    {
        static string[] usernames = new string[5];
        static string[] pins = new string[5];
        static void Main(string[] args)
        {
            InItUsers();
            RunProgram();
        }
        static void RunProgram()
        {
            int loginAttempts = 0;
            while (loginAttempts < 3)
            {
                if (Login())
                {
                    Console.WriteLine("Du är inloggad");
                    Console.WriteLine("Tryck enter för logga ut");
                    Console.ReadLine();
                    loginAttempts = 0;
                }
                else
                {
                    loginAttempts++;
                    Console.WriteLine($"Felaktiga försök {loginAttempts} av 3.");
                    if (loginAttempts >= 3)
                    {
                        Console.WriteLine("För många försök, programmet avslutas.");
                    }
                }
            }
        }
        static void InItUsers()
        {
            usernames[0] = "Pontus"; pins[0] = "1111";
            usernames[1] = "Carl"; pins[1] = "2222";
            usernames[2] = "Amanda"; pins[2] = "3333";
            usernames[3] = "Josefine"; pins[3] = "4444";
            usernames[4] = "Elliott"; pins[4] = "5555";
        }
        static bool Login()
        {
            Console.WriteLine("=== Logga in på Bibloteket ===");
            Console.WriteLine("Ange ditt användarnamn.");
            string inputUser = Console.ReadLine();

            Console.WriteLine("Ange din pinkod.");
            string inputPin = Console.ReadLine();

            for (int i = 0; i < usernames.Length; i++)
            {
                if (inputUser == usernames[i] && inputPin == pins[i])
                {
                    Console.WriteLine($"Välkommen {inputUser}");
                    return true;
                }
            }
            return false;
        }
    }
}
