using System;
using System.Collections.Generic;
using System.Linq;

namespace PoliceExam2018
{
    class Program
    {
        static void Main(string[] args)
        {
            var count = 0;
            var verifiedAnswer = new List<Exam>();

            foreach (var answer in ExamGenerator())
            {
                count++;
                if (answer.Verify())
                {
                    verifiedAnswer.Add(answer);
                }
            }

            Console.WriteLine($"Total number of anwer: {count}");
            Console.WriteLine($"Total number of verified answer: {verifiedAnswer.Count}\n");

            foreach (var answer in verifiedAnswer)
            {
                Console.WriteLine(string.Join(' ', answer.Answers));
            }

            Console.ReadLine();
        }

        private static IEnumerable<Exam> ExamGenerator()
        {
            var options = new[] {AnswerOption.A, AnswerOption.B, AnswerOption.C, AnswerOption.D};
            return AllSequence(options, Exam.QuestionCount).Select(s => new Exam(s.ToArray()));
        }

        private static IEnumerable<List<AnswerOption>> AllSequence(AnswerOption[] options, int size)
        {
            if (size == 0) return Enumerable.Repeat(new List<AnswerOption>(), 1);
            return
                from option in options
                from seq in AllSequence(options, size - 1)
                select new List<AnswerOption> {option}.Concat(seq).ToList();
        }
    }
}

