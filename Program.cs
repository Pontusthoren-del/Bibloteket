using System.Drawing;
using System.Reflection.Metadata;

namespace Bibloteket
{
    internal class Program
    {
        //GLOBAL ARRAYS
        //Usernames and pins hold the login info for the predefined users
        static string[] anvandare = new string[5];
        static string[] pins = new string[5];
        //Book info:Titles,total copies and currently loaned copies.
        static string[] bokTitlar = new string[5];
        static int[] totalExemplar = new int[5];
        static int[] utlanadeExemplar = new int[5];
        //Userloans keeps track of which books each user has borrowed.
        static int[,] anvadareLan = new int[5, 5];
        //Keep track of which user is currently logged in.
        static int inloggadAnvandareIndex = -1;
        static void Main(string[] args)
        {
            InitAnvandare();
            InitBocker();
            InitLan();
            RunProgram();
        }
        static void RunProgram()
        {
            bool programRunning = true;
            int loginAttempts = 0;
            while (programRunning && loginAttempts < 3)
            {
                if (Login())
                {
                    loginAttempts = 0;
                    RunMainMenu();
                    Console.Clear();
                }
                else
                {
                    loginAttempts++;
                    Console.WriteLine($"Felaktiga försök {loginAttempts} av 3.");
                    Console.ReadLine();
                    Console.Clear();
                    if (loginAttempts >= 3)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine("-----------------------");
                        Console.WriteLine("För många försök, programmet avslutas.");
                        Console.WriteLine("-----------------------");
                        Console.ResetColor();
                        Console.ReadLine();
                        programRunning = false;
                    }
                }
            }

        }
        //This method fills the username and pin with predefined users.
        static void InitAnvandare()
        {
            anvandare[0] = "Pontus"; pins[0] = "1111";
            anvandare[1] = "Carl"; pins[1] = "2222";
            anvandare[2] = "Amanda"; pins[2] = "3333";
            anvandare[3] = "Josefine"; pins[3] = "4444";
            anvandare[4] = "Elliott"; pins[4] = "5555";
        }
        //Handle user login.Returns true if credentials are correct.
        //Also set loggedInUserIndex to the currently logged in user.
        static bool Login()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\t====== Logga in på Bibloteket ======");
            Console.ResetColor();
            Console.WriteLine();
            Console.Write("Ange ditt användarnamn: ");
            string inputUser = Console.ReadLine();
            Console.WriteLine("-----------------------");

            Console.Write("Ange din pinkod: ");
            string inputPin = Console.ReadLine();

            for (int i = 0; i < anvandare.Length; i++)
            {
                if (inputUser == anvandare[i] && inputPin == pins[i])
                {
                    inloggadAnvandareIndex = i;
                    Console.WriteLine($"Välkommen {inputUser}");
                    return true;
                }
            }
            return false;
        }
        //Show menu options and handles user selection.
        static void RunMainMenu()
        {
            bool loggedIn = true;
            while (loggedIn)
            {
                Console.Clear();              
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("\t=== HUVUDMENY ===");
                Console.ResetColor();
                Console.WriteLine("1. Visa böcker.");
                Console.WriteLine("2. Låna bok.");
                Console.WriteLine("3. Lämna tillbaka bok.");
                Console.WriteLine("4. Mina lån.");
                Console.WriteLine("5. Logga ut.");
                Console.WriteLine("-------------------");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("6. Avsluta programmet.");
                Console.ResetColor();
                Console.WriteLine("-------------------");
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
                            inloggadAnvandareIndex = -1;
                            loggedIn = false;
                            Console.WriteLine("Du loggar ut...Tryck Enter.");
                            Console.ReadLine();
                            break;
                        case 6:
                            TurnOff();
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
        //Loop through all books and calculates available copies
        //Available copies = total copies - currently loaned copies
        static void VisaBocker()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\t====== Visa alla böcker ======");
            Console.ResetColor();
            Console.WriteLine();
            for (int i = 0; i < bokTitlar.Length; i++)
            {
                //Calculate available copies for each book
                int tillgangliga = totalExemplar[i] - utlanadeExemplar[i];
                //Write out every book and availability and then and a space under.
                Console.WriteLine($"{i + 1}.{bokTitlar[i]} - Tillgängliga exemplar: {tillgangliga}.");
                Console.WriteLine();
            }
            Console.WriteLine("-------------------");
            Console.WriteLine("Tryck Enter för att återgå till huvudmenyn.");
            Console.ReadLine();
        }
        //Sets the book titles, total copies, and initial loaned count.
        static void InitBocker()
        {
            bokTitlar[0] = "Sagan om Ringen,Ringens brödrarskap."; totalExemplar[0] = 3; utlanadeExemplar[0] = 0;
            bokTitlar[1] = "Sagan om Ringen,De två tornen."; totalExemplar[1] = 3; utlanadeExemplar[1] = 0;
            bokTitlar[2] = "Sagan om Ringen,Konungens återkomst."; totalExemplar[2] = 3; utlanadeExemplar[2] = 0;
            bokTitlar[3] = "The Silmarillion"; totalExemplar[3] = 3; utlanadeExemplar[3] = 0;
            bokTitlar[4] = "Beren and Lúthien"; totalExemplar[4] = 3; utlanadeExemplar[4] = 0;

        }
        //Lets the user borrow a book if available
        //Checks availabillity, updates users loan array, and increments loaned count
        static void LanaBocker()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\t====== Låna bok ======");
            Console.ResetColor();
            Console.WriteLine();
            //Loop through all book titles
            for (int i = 0; i < bokTitlar.Length; i++)
            {
                //Calculate available copies for each book
                int tillgangliga = totalExemplar[i] - utlanadeExemplar[i];
                //Write out every book and availability and then and a space under.
                Console.WriteLine($"{i + 1}.{bokTitlar[i]} - Tillgängliga exemplar: {tillgangliga}.");
                Console.WriteLine();
            }
            Console.WriteLine("-------------------");
            Console.WriteLine("Ange nummret för boken du vill låna.");
            string input = Console.ReadLine();
            int val;
            if (int.TryParse(input, out val))
            {
                int bokIndex = val - 1;
                if (bokIndex < 0 || bokIndex >= bokTitlar.Length)
                {
                    Console.WriteLine("Ogiltligt val.Tryck Enter för att återgå.");
                    Console.ReadLine();
                    return;
                }
                //Calculate available copies of each book.
                int tillgangligaEX = totalExemplar[bokIndex] - utlanadeExemplar[bokIndex];
                if (tillgangligaEX <= 0)
                {
                    Console.WriteLine("Tyvärr finns det inga fler exemplar.Tryck Enter för återgå.");
                    Console.ReadLine();
                    return;
                }
                int userIndex = inloggadAnvandareIndex;
                bool lanat = false;
                //Check if the user has a empty loan slot(max 5 books).
                for (int i = 0; i < 5; i++)
                {
                    if (anvadareLan[userIndex, i] == -1)
                    {
                        anvadareLan[userIndex, i] = bokIndex;
                        utlanadeExemplar[bokIndex]++;
                        Console.WriteLine($"Du har lånat: {bokTitlar[bokIndex]}");
                        Console.WriteLine("-------------------");
                        Console.WriteLine("Tryck Enter för att återgå till huvudmenyn.");
                        Console.ReadLine();
                        lanat = true;
                        break;
                    }
                }
                if (!lanat)
                {
                    Console.WriteLine("Du har redan lånat max antal böcker.Tryck Enter för att återgå.");
                    Console.ReadLine();
                }
            }
        }
        //Loop through all users and all their loan slots, so the user have to many borrowed books.(5)
        // setting each slot to -1 to indicate that the user has not borrowed any books yet.
        static void InitLan()
        {
            for (int u = 0; u < 5; u++)
            {
                for (int i = 0; i < 5; i++)
                {
                    anvadareLan[u, i] = -1;
                }
            }
        }
        //Loop through the loaned books and see if the user can return any book.
        static void LamnaTillbakaBok()
        {
            int anvandarIndex = inloggadAnvandareIndex;
            bool harLanadeBocker = false;
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\t====== Lämna tillbaka bok ======");
            Console.ResetColor();
            Console.WriteLine();
            //Show the users loaned books.
            for (int i = 0; i < 5; i++)
            {
                int bokIndex = anvadareLan[anvandarIndex, i];
                if (bokIndex != -1)
                {
                    Console.WriteLine($"{i + 1}.{bokTitlar[bokIndex]}");
                    harLanadeBocker = true;
                }
            }
            if (!harLanadeBocker)
            {
                Console.WriteLine("Du har inga lånade böcker.");
                Console.WriteLine("-------------------");
                Console.WriteLine("Tryck Enter för att återgå till huvudmenyn.");
                Console.ReadLine();
                return;
            }
            Console.WriteLine("Ange numret på boken du vill lämna tillbaka.");
            string input = Console.ReadLine();
            int val;
            //Check if the input is invalid.
            if (!int.TryParse(input, out val) || val < 1 || val > 5 || anvadareLan[anvandarIndex, val - 1] == -1)
            {
                Console.WriteLine("Ogiltligt val.Tryck Enter för att återgå.");
                Console.ReadLine();
                return;
            }
            // Get the index of the borrowed book, mark the slot as empty (-1),
            // and decrement the number of currently loaned copies for that book
            int lanadeBokIndex = anvadareLan[anvandarIndex, val - 1];
            anvadareLan[anvandarIndex, val - 1] = -1;
            utlanadeExemplar[lanadeBokIndex]--;
            Console.WriteLine($"Du har lämnat tillbaka: {bokTitlar[lanadeBokIndex]}");
            Console.WriteLine("-------------------");
            Console.WriteLine("Tryck Enter för att återgå till huvudmenyn.");
            Console.ReadLine();
        }
        //Loop through our loans and shows them.
        static void MinaLan()
        {
            int anvandareIndex = inloggadAnvandareIndex;
            bool harLanadeBocker = false;
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\t====== Visa mina lån ======");
            Console.ResetColor();
            Console.WriteLine();
            for (int i = 0; i < 5; i++)
            {
                int bokIndex = anvadareLan[anvandareIndex, i];
                if (bokIndex != -1)
                {
                    Console.WriteLine($"{i + 1}.{bokTitlar[bokIndex]}");
                    harLanadeBocker = true;
                }
            }
            if (!harLanadeBocker)
            {
                Console.WriteLine("Du har inga aktiva lån just nu.");
            }
            Console.WriteLine("-------------------");
            Console.WriteLine("Tryck Enter för att återgå till huvudmenyn.");
            Console.ReadLine();
        }

        //Extra method for turning off the console.
        static void TurnOff()
        {
            Console.WriteLine("Programmet avslutas.");
            Environment.Exit(0);
        }
    }
}
