using System.Reflection.Metadata;

namespace Bibloteket
{
    class Program
    {
        //GLOBAL ARRAYS
        //Usernames and pins hold the login info for the predefined users
        static string[] usernames = new string[5];
        static string[] pins = new string[5];
        //Book info:Titles,total copies and currently loaned copies.
        static string[] bokTitlar = new string[5];
        static int[] totalExemplar = new int[5];
        static int[] utlanadeExemplar = new int[5];
        //Userloans keeps track of which books each user has borrowed.
        static int[,] userLoans = new int[5, 5];
        //Keep track of which user is currently logged in.
        static int loggedInUserIndex = -1;
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
                    break;
                }
                else
                {
                    loginAttempts++;
                    Console.WriteLine($"Felaktiga försök {loginAttempts} av 3.");
                    if (loginAttempts >= 3)
                    {
                        Console.WriteLine("För många försök, programmet avslutas.");
                        Console.ReadLine();
                        programRunning = false;
                    }
                }
            }

        }
        //This method fills the username and pin with predefined users.
        static void InitAnvandare()
        {
            usernames[0] = "Pontus"; pins[0] = "1111";
            usernames[1] = "Carl"; pins[1] = "2222";
            usernames[2] = "Amanda"; pins[2] = "3333";
            usernames[3] = "Josefine"; pins[3] = "4444";
            usernames[4] = "Elliott"; pins[4] = "5555";
        }
        //Handle user login.Returns true if credentials are correct.
        //Also set loggedInUserIndex to the currently logged in user.
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
                    loggedInUserIndex = i;
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
                            loggedInUserIndex = -1;
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
        //Loop through all books and calculates available copies
        //Available copies = total copies - currently loaned copies
        static void VisaBocker()
        {
            Console.WriteLine("Visa alla böcker...");
            Console.WriteLine("-------------------");
            for (int i = 0; i < bokTitlar.Length; i++)
            {
                //Calculate available copies for each book
                int tillgangliga = totalExemplar[i] - utlanadeExemplar[i];
                //Write out every book and availability and then and a space under.
                Console.WriteLine($"{i + 1}.{bokTitlar[i]} - Tillgängliga exemplar: {tillgangliga}.");
                Console.WriteLine();
            }
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
            Console.WriteLine("Låna bok...");
            Console.WriteLine("-------------------");
            //Loop through all book titles
            for (int i = 0; i < bokTitlar.Length; i++)
            {
                //Calculate available copies for each book
                int tillgangliga = totalExemplar[i] - utlanadeExemplar[i];
                //Write out every book and availability and then and a space under.
                Console.WriteLine($"{i + 1}.{bokTitlar[i]} - Tillgängliga exemplar: {tillgangliga}.");
                Console.WriteLine();
            }
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
                int userIndex = loggedInUserIndex;
                bool lanat = false;
                //Check if the user has a empty loan slot(max 5 books).
                for (int i = 0; i < 5; i++)
                {
                    if (userLoans[userIndex, i] == -1)
                    {
                        userLoans[userIndex, i] = bokIndex;
                        utlanadeExemplar[bokIndex]++;
                        Console.WriteLine($"Du har lånat: {bokTitlar[bokIndex]}");
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
        // Loop through all users and all their loan slots,
        // setting each slot to -1 to indicate that the user has not borrowed any books yet.
        static void InitLan()
        {
            for (int u = 0; u < 5; u++)
            {
                for (int i = 0; i < 5; i++)
                {
                    userLoans[u, i] = -1;
                }
            }
        }
        static void LamnaTillbakaBok()
        {
            int userIndex = loggedInUserIndex;
            bool harLanadeBocker = false;
            Console.WriteLine("======Lämna tillbaka bok...======");
            Console.WriteLine("-------------------");
            //Show the users loaned books.
            for (int i = 0; i < 5; i++)
            {
                int bokIndex = userLoans[userIndex, i];
                if (bokIndex != -1)
                {
                    Console.WriteLine($"{i + 1}.{bokTitlar[bokIndex]}");
                    harLanadeBocker = true;
                }
            }
            if (!harLanadeBocker)
            {
                Console.WriteLine("Du har inga lånade böcker.");
                Console.WriteLine("Tryck Enter för att återgå till huvudmenyn.");
                Console.ReadLine();
                return;
            }
            Console.WriteLine("Ange numret på boken du vill lämna tillbaka.");
            string input = Console.ReadLine();
            int val;
            //Check if the input is invalid.
            if (!int.TryParse(input, out val) || val < 1 || val > 5 || userLoans[userIndex, val - 1] == 1)
            {
                Console.WriteLine("Ogiltligt val.Tryck Enter för att återgå.");
                Console.ReadLine();
                return;
            }
            // Get the index of the borrowed book, mark the slot as empty (-1),
            // and decrement the number of currently loaned copies for that book
            int lanadeBokIndex = userLoans[userIndex, val - 1];
            userLoans[userIndex, val - 1] = 1;
            utlanadeExemplar[lanadeBokIndex]--;
            Console.WriteLine($"Du har lämnata tillbaka: {bokTitlar[lanadeBokIndex]}");
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
