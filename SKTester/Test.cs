using System;
using System.Collections.Generic;
using System.Threading;

namespace SKTester
{
    class Test
    {
        public List<Question> Questions { get; set; }
        public List<int> Answers { get; set; }

        private TestController controller;

        public Test(List<Question> questions, TestController controller)
        {
            Questions = questions;
            Answers = new List<int>();
            this.controller = controller;
        }
        /// <summary>
        /// Returns the number of correct answers
        /// </summary>
        /// <returns></returns>
        public int GetTestScore()
        {
            int score = 0;
            int i = 0;

            foreach (Question q in Questions)
            {
                if (i < Answers.Count && q.RightAnswer == Answers[i])
                    score++;
                i++;
            }

            return score;
        }
        /// <summary>
        /// Returns Stotal Score in %
        /// </summary>
        /// <returns></returns>
        public double GetTotalScore()
        {
            double finalScore = (double)GetTestScore() * 100.0
                /(double)TestController.NUMBER_OF_QUESTIONS * 2.0 / 3.0;
            return finalScore;
        }
        /// <summary>
        /// Displays questions with their correct answers and the answers given by the user.
        /// </summary>
        public void ViewAnswers()
        {
            int i = 0;
            foreach (Question q in Questions)
            {
                q.View();
                Console.WriteLine();
                q.ViewRightAnswer();
                q.ViewAnswer(Answers[i]);
                Console.WriteLine("\n\n");
                i++;
            }
        }
        /// <summary>
        /// Main thread of the test. Displays question and allows to choose 
        /// an answer and move to the next one.
        /// </summary>
        public void Run()
        {
            int ind = 0;
            int selected = 0;
            foreach (Question q in Questions)
            {
                ind++;
                selected = 0;

                Console.Clear();
                Console.SetCursorPosition(0, 1);
                Console.WriteLine($"{ind}/{TestController.NUMBER_OF_QUESTIONS}");
                Console.WriteLine();
                Console.WriteLine($"Wybrana odpowiedź: {selected}");

                q.View();

                while (true)
                {
                    //check if time is up
                    if (controller.TimeLeft == false)
                    {
                        return;
                    }
                    //check which key was pressed
                    ConsoleKeyInfo cki = Console.ReadKey(true);
                    if (cki.Key == ConsoleKey.Enter)
                    {
                        break;
                    }
                    if ((int)cki.Key >= (int)ConsoleKey.D0 && (int)cki.Key <= (int)ConsoleKey.D3)
                    {
                        selected = (int)cki.Key - 48;
                        Console.SetCursorPosition(0, 3);
                        Console.WriteLine($"Wybrana odpowiedź: {selected}");
                    }
                }
                Answers.Add(selected);
                Thread.Sleep(100);
            }
        }
    }
}
