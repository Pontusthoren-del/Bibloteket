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
        static void RunMainMenu()
        {
            bool loggedIn = true;
            while (loggedIn)
            {
                Console.Clear();
                Console.WriteLine("=== HUVUDMENY ===");
                Console.WriteLine("1. Visa böcker.");
                Console.WriteLine("2. Låna bok.");
                Console.WriteLine("3. Lämna tillbaka bok.");
                Console.WriteLine("4. Mina lån.");
                Console.WriteLine("5. Logga ut.");
                Console.WriteLine("Välj ett av alternativen.");

                string input = Console.ReadLine();
                int choice;

                if(int.TryParse(input, out choice))
                {
                    switch (choice)
                    {
                        case 1:
                            VisaBocker();
                            break;
                        case 2:
                            LanaBocker();
                            break;
                        case 3:
                            LamnaTillbakaBok();
                        case 4:
                            MinaLan();
                            break;
                        case 5:
                            loggedIn = false;
                            Console.WriteLine("Du loggar ut...Tryck Enter.");
                            Console.ReadLine();
                            break;
                        default:
                            Console.WriteLine("Ogiltligt val.Tryck Enter för att försöka igen.");
                            Console.ReadLine();
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Ogiltligt val.Tryck Enter för att försöka igen.");
                }
            }
        }


    }
}
