using System;
using System.Collections.Generic;

namespace SKTester
{
    class Question
    {
        public string Content { get; set; }
        public List<string> Answers { get; set; }
        public int RightAnswer { get; set; }

        public Question(string name, List<string> answers, int rightAnswer)
        {
            Content = name;
            Answers = answers;
            RightAnswer = rightAnswer;
        }
        /// <summary>
        /// Used to randomize order of the answers
        /// </summary>
        public void Shuffle()
        {
            Random r = new Random();
            List<string> tmp = new List<string>(Answers);
            string correct = Answers[RightAnswer];

            for (int i = 0; i < Answers.Count; i++)
            {
                int ind = r.Next(tmp.Count);
                if (tmp[ind].Equals(correct))
                {
                    RightAnswer = i;
                }
                Answers[i] = tmp[ind];
                tmp.RemoveAt(ind);
            }
        }
        /// <summary>
        /// Displays the question in console
        /// </summary>
        public void View()
        {
            Console.WriteLine(Content);
            
            for (int i = 0; i < Answers.Count; i++)
            {
                Console.WriteLine($"{i}: {Answers[i]}");
            }
        }

        public void ViewRightAnswer()
        {
            Console.WriteLine($"Poprawna odpowiedź: {Answers[RightAnswer]}");
        }

        public void ViewAnswer(int ind)
        {
            if (ind >= Answers.Count)
                throw new ArgumentOutOfRangeException();
            Console.WriteLine($"Twoja odpowiedź: {Answers[ind]}");
        }
    }
}
