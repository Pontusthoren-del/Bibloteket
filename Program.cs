using System.Reflection.Metadata;

namespace Bibloteket
{
    class Program
    {
        static string[] usernames = new string[5];
        static string[] pins = new string[5];
        static string[] bokTitlar = new string[5];
        static int[] totalExemplar = new int[5];
        static int[] utlanadeExemplar = new int[5];
        static void Main(string[] args)
        {
            InitUsers();
            InitBocker();
            RunProgram();
        }
        static void RunProgram()
        {
            while (true)
            {
                int loginAttempts = 0;
                while (loginAttempts < 3)
                {
                    if (Login())
                    {
                        loginAttempts = 0;
                        RunMainMenu();
                        break;
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
        }
        static void InitUsers()
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

                if (int.TryParse(input, out choice))
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
                            break;
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
        static void VisaBocker()
        {
            Console.WriteLine("Visa alla böcker...");
            for(int i = 0; i<bokTitlar.Length; i++)
            {
                int tillgangliga = totalExemplar[i] - utlanadeExemplar[i];
                Console.WriteLine($"{i + 1}.{bokTitlar[i]} - Tillgängliga exemplar: {tillgangliga}.");
            }
            Console.WriteLine("Tryck Enter för att återgå till huvudmenyn.");
            Console.ReadLine();
        }
        static void InitBocker()
        {
            bokTitlar[0] = "Sagan om Ringen,Ringens brödrarskap."; totalExemplar[0] = 3; utlanadeExemplar[0] = 0;
            bokTitlar[1] = "Sagan om Ringen,De två tornen."; totalExemplar[1] = 3; utlanadeExemplar[1] = 0;
            bokTitlar[2] = "Sagan om Ringen,Konungens återkomst."; totalExemplar[2] = 3; utlanadeExemplar[2] = 0;
            bokTitlar[3] = "The Silmarillion"; totalExemplar[3] = 3; utlanadeExemplar[3] = 0;
            bokTitlar[4] = "Beren and Lúthien"; totalExemplar[4] = 3; utlanadeExemplar[4] = 0;

        }
        static void LanaBocker()
        {
            Console.WriteLine("Låna bok...");
            Console.WriteLine("Tryck Enter för att återgå till huvudmenyn.");
            Console.ReadLine();
        }
        static void LamnaTillbakaBok()
        {
            Console.WriteLine("Lämna tillbaka bok...");
            Console.WriteLine("Tryck Enter för att återgå till huvudmenyn.");
            Console.ReadLine();
        }
        static void MinaLan()
        {
            Console.WriteLine("Visa mina lån...");
            Console.WriteLine("Tryck Enter för att återgå till huvudmenyn.");
            Console.ReadLine();
        }
    }
}
