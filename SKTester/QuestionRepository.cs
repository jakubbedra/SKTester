using System;
using System.Collections.Generic;
using System.IO;

namespace SKTester
{
    class QuestionRepository
    {
        public List<Question> Questions { get; set; }

        public QuestionRepository()
        {
            Questions = new List<Question>();
        }
        /// <summary>
        /// Returns a random sublist of all questions, whith randomized answers order
        /// </summary>
        /// <param name="numberOfQuestions">Number of questions to be returned</param>
        /// <returns></returns>
        public List<Question> GenerateRandomQuestionList(int numberOfQuestions)
        {
            if (numberOfQuestions > Questions.Count)
            {
                throw new ArgumentOutOfRangeException();
            }

            List<Question> questionSet = new List<Question>(Questions);
            Random r = new Random();
            //Generating random list
            for (int i = 0; i < Questions.Count - numberOfQuestions; i++)
            {
                questionSet.RemoveAt(r.Next(questionSet.Count));
            }
            //Randomizing order of the answers
            foreach (Question q in questionSet)
            {
                q.Shuffle();
            }

            return questionSet;
        }
        /// <summary>
        /// Loads questions from a file.
        /// </summary>
        /// <param name="filePath">Path to the source file</param>
        public void LoadQuestionsFromFile(string filePath)
        {
            string[] lines = File.ReadAllLines(filePath);

            for (int i = 0; !lines[i].Equals("END"); i += 6)
            {
                string content = lines[i];
                List<string> answers = new List<string>();
                for (int j = 1; j <= 4; j++)
                {
                    answers.Add(lines[i + j]);
                }
                int rightAnswer = Int32.Parse(lines[i + 5]);
                Questions.Add(new Question(content, answers, rightAnswer));
            }
        }
        /// <summary>
        /// Displays all questions in the repository, used mainly for debug purposes
        /// </summary>
        public void View()
        {
            foreach (Question q in Questions)
            {
                q.View();
                Console.WriteLine($"Right answer: {q.RightAnswer}");
                Console.WriteLine("");
            }
        }
    }
}
