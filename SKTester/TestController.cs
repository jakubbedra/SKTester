using System;
using System.Threading;

namespace SKTester
{
    class TestController
    {
        public const string FILE_PATH = "Data\\10PytanTEST.txt";
        public const int NUMBER_OF_QUESTIONS = 50;
        public const int TIME_LIMIT_SECONDS = 30 * NUMBER_OF_QUESTIONS;//30 seconds per question

        public bool TimeLeft { get; set; }

        private QuestionRepository repository;
        private Test test;
        private Thread testThread;
        private Thread timerThread;

        public TestController()
        {
            repository = new QuestionRepository();
            repository.LoadQuestionsFromFile(FILE_PATH);
            this.TimeLeft = false;
        }
        /// <summary>
        /// Starts the test controller.
        /// </summary>
        public void Start()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.Clear();
            Console.WriteLine("Sprawdź swoją wiedzę z sieci komputerowych!");
            Console.WriteLine("Wciśnij dowolny przycisk aby zacząć.");
            Console.ReadKey();
            StartAttempt();
        }

        private void StartAttempt()
        {
            Console.Clear();
            test = new Test(repository.GenerateRandomQuestionList(NUMBER_OF_QUESTIONS), this);
            int remainingTimeSeconds = TIME_LIMIT_SECONDS;
            TimeLeft = true;
            DisplayTimer(remainingTimeSeconds);
            //start the test thread
            testThread = new Thread(new ThreadStart(test.Run));
            testThread.Start();
            timerThread = new Thread(new ParameterizedThreadStart(Timer));
            timerThread.Start(remainingTimeSeconds);

            while (testThread.ThreadState != ThreadState.Stopped)
            { }

            Console.Clear();
            if (timerThread.ThreadState != ThreadState.Stopped)
                timerThread.Interrupt();

            AttemptReview();
        }

        private void AttemptReview()
        {
            Console.Clear();
            Console.WriteLine("Koniec Testu.");
            Console.WriteLine();
            Console.WriteLine($"Wynik: {test.GetTestScore()}/{NUMBER_OF_QUESTIONS}");
            string passed = test.GetTotalScore() > 50.0 ? "TAK" : "NIE";
            Console.WriteLine($"Zdane bez otwartych: {passed} ({test.GetTotalScore()}%)");
            Console.WriteLine("Przegląd         - 1");
            Console.WriteLine("Zakończ          - 2");
            Console.WriteLine("Rozwiąż ponownie - 3");
            int choice;

            while (true)
            {
                try
                {
                    choice = Int32.Parse(Console.ReadLine());

                    if (choice != 1 && choice != 2 && choice != 3)
                    { }
                    else
                    {
                        break;
                    }
                }
                catch (Exception)
                {
                    Console.SetCursorPosition(0, 8);
                    Console.WriteLine("Niewłaściwy argument.");
                }
            }
            switch (choice)
            {
                case 1:
                    ShowAnswers();
                    break;
                case 2:
                    return;
                case 3:
                    Start();
                    break;
                default:
                    break;
            }
        }

        private void ShowAnswers()
        {
            Console.Clear();
            test.ViewAnswers();
            Console.WriteLine("Wciśnij dowolny przycisk.");
            Console.ReadKey();
            AttemptReview();
        }

        private void Timer(object o)
        {
            int remainingSeconds = (int)o;
            //update timer each second
            for (int i = 0; i < TIME_LIMIT_SECONDS; i++)
            {
                try
                {
                    Thread.Sleep(1000);
                    remainingSeconds--;
                    DisplayTimer(remainingSeconds);
                }
                catch (ThreadInterruptedException)
                {
                    TimeLeft = false;
                    return;
                }
            }
            //Time is up
            TimeLeft = false;
        }

        private void DisplayTimer(int secondsRemaining)
        {
            (int x, int y) = Console.GetCursorPosition();
            int minutes = secondsRemaining / 60;
            int seconds = secondsRemaining % 60;
            Console.SetCursorPosition(0, 0);
            if (minutes > 9)
            {
                if (seconds > 9)
                    Console.Write($"{minutes}:{seconds}");
                else
                    Console.Write($"{minutes}:0{seconds}");
            }
            else
            {
                if (seconds > 9)
                    Console.Write($"0{minutes}:{seconds}");
                else
                    Console.Write($"0{minutes}:0{seconds}");
            }
            Console.SetCursorPosition(x, y);
        }
    }
}
