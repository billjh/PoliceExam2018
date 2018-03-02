using System;
using System.Collections.Generic;
using System.Linq;

namespace PoliceExam2018
{
    internal enum AnswerOption
    {
        A = 1,
        B = 2,
        C = 3,
        D = 4
    }

    internal class Exam
    {
        public Exam(AnswerOption[] answers)
        {
            if (answers == null || answers.Length != QuestionCount)
            {
                throw new ArgumentException($"Answer count does not match with question count which is {QuestionCount}");
            }
            Answers = answers;
        }

        internal static int QuestionCount => 10;

        private enum QuestionNumber
        {
            One = 0,
            Two = 1,
            Three = 2,
            Four = 3,
            Five = 4,
            Six = 5,
            Seven = 6,
            Eight = 7,
            Nine = 8,
            Ten = 9
        }

        internal AnswerOption[] Answers { get; }

        private AnswerOption GetAnswer(QuestionNumber questionNumber)
        {
            return Answers[(int) questionNumber];
        }

        public bool Verify()
        {
            return Questions.All(verify => verify(this));
        }

        private static Predicate<Exam>[] Questions => new[]
        {
            Question1(),
            Question2(),
            Question3(),
            Question4(),
            Question5(),
            Question6(),
            Question7(),
            Question8(),
            Question9(),
            Question10()
        };

        private static Predicate<Exam> Question1()
        {
            return exam => true;
        }

        private static Predicate<Exam> Question2()
        {
            var mapping = new Dictionary<AnswerOption, AnswerOption>
            {
                {AnswerOption.A, AnswerOption.C},
                {AnswerOption.B, AnswerOption.D},
                {AnswerOption.C, AnswerOption.A},
                {AnswerOption.D, AnswerOption.B},
            };

            return exam => exam.GetAnswer(QuestionNumber.Five) == mapping[exam.GetAnswer(QuestionNumber.Two)];
        }

        private static Predicate<Exam> Question3()
        {
            var mapping = new Dictionary<AnswerOption, QuestionNumber>
            {
                {AnswerOption.A, QuestionNumber.Three},
                {AnswerOption.B, QuestionNumber.Six},
                {AnswerOption.C, QuestionNumber.Two},
                {AnswerOption.D, QuestionNumber.Four}
            };
            var allQuestions = mapping.Values;

            return exam =>
            {
                var selectedQuestion = mapping[exam.GetAnswer(QuestionNumber.Three)];
                var selectedAnswer = exam.GetAnswer(selectedQuestion);

                var restQuestions = allQuestions.Where(q => q != selectedQuestion);
                var restAnswers = restQuestions.Select(exam.GetAnswer).ToArray();

                return restAnswers.AllSame() && selectedAnswer != restAnswers.First();
            };
        }

        private static Predicate<Exam> Question4()
        {
            var mapping = new Dictionary<AnswerOption, QuestionNumber[]>
            {
                {AnswerOption.A, new[] {QuestionNumber.One, QuestionNumber.Five}},
                {AnswerOption.B, new[] {QuestionNumber.Two, QuestionNumber.Seven}},
                {AnswerOption.C, new[] {QuestionNumber.One, QuestionNumber.Nine}},
                {AnswerOption.D, new[] {QuestionNumber.Six, QuestionNumber.Ten}}
            };

            return exam => mapping.All(
                kvp =>
                {
                    var hasSameAnswer = kvp.Value.Select(exam.GetAnswer).AllSame();
                    return exam.GetAnswer(QuestionNumber.Four) == kvp.Key ? hasSameAnswer : !hasSameAnswer;
                });
        }

        private static Predicate<Exam> Question5()
        {
            var mapping = new Dictionary<AnswerOption, QuestionNumber>
            {
                {AnswerOption.A, QuestionNumber.Eight},
                {AnswerOption.B, QuestionNumber.Four},
                {AnswerOption.C, QuestionNumber.Nine},
                {AnswerOption.D, QuestionNumber.Seven}
            };

            return exam => mapping.All(
                kvp =>
                {
                    var questionFiveAnswer = exam.GetAnswer(QuestionNumber.Five);
                    return questionFiveAnswer == kvp.Key
                        ? exam.GetAnswer(kvp.Value) == questionFiveAnswer
                        : exam.GetAnswer(kvp.Value) != questionFiveAnswer;
                });
        }

        private static Predicate<Exam> Question6()
        {
            var mapping = new Dictionary<AnswerOption, QuestionNumber[]>
            {
                {AnswerOption.A, new[] {QuestionNumber.Two, QuestionNumber.Four}},
                {AnswerOption.B, new[] {QuestionNumber.One, QuestionNumber.Six}},
                {AnswerOption.C, new[] {QuestionNumber.Three, QuestionNumber.Ten}},
                {AnswerOption.D, new[] {QuestionNumber.Five, QuestionNumber.Nine}}
            };

            return exam => mapping.All(
                kvp =>
                {
                    var questionEightAnswer = exam.GetAnswer(QuestionNumber.Eight);
                    var answers = kvp.Value.Select(exam.GetAnswer);
                    return exam.GetAnswer(QuestionNumber.Six) == kvp.Key
                        ? answers.Append(questionEightAnswer).AllSame()
                        : answers.Any(a => a != questionEightAnswer);
                });
        }

        private static Predicate<Exam> Question7()
        {
            var mapping = new Dictionary<AnswerOption, AnswerOption>
            {
                {AnswerOption.A, AnswerOption.C},
                {AnswerOption.B, AnswerOption.B},
                {AnswerOption.C, AnswerOption.A},
                {AnswerOption.D, AnswerOption.D}
            };

            return exam =>
            {
                var counter = mapping.ToDictionary(m => m.Key, m => exam.Answers.Count(a => a == m.Key));
                return counter[exam.GetAnswer(QuestionNumber.Seven)] == counter.Min(c => c.Value);
            };
        }

        private static Predicate<Exam> Question8()
        {
            var mapping = new Dictionary<AnswerOption, QuestionNumber>
            {
                {AnswerOption.A, QuestionNumber.Seven},
                {AnswerOption.B, QuestionNumber.Five},
                {AnswerOption.C, QuestionNumber.Two},
                {AnswerOption.D, QuestionNumber.Ten}
            };

            return exam => mapping.All(
                kvp =>
                {
                    var isAnswerAdjacentToQuestionOne =
                        exam.GetAnswer(kvp.Value).IsAdjacentTo(exam.GetAnswer(QuestionNumber.One));
                    return exam.GetAnswer(QuestionNumber.Eight) == kvp.Key
                        ? !isAnswerAdjacentToQuestionOne
                        : isAnswerAdjacentToQuestionOne;
                });
        }

        private static Predicate<Exam> Question9()
        {
            var mapping = new Dictionary<AnswerOption, QuestionNumber>
            {
                {AnswerOption.A, QuestionNumber.Six},
                {AnswerOption.B, QuestionNumber.Ten},
                {AnswerOption.C, QuestionNumber.Two},
                {AnswerOption.D, QuestionNumber.Nine}
            };

            return exam =>
            {
                var oneAndSixSameAnswer = exam.GetAnswer(QuestionNumber.One) == exam.GetAnswer(QuestionNumber.Six);
                return mapping.All(
                    kvp =>
                    {
                        var xAndFiveSameAnswer = exam.GetAnswer(mapping[kvp.Key]) == exam.GetAnswer(QuestionNumber.Five);
                        var finalAnswer = oneAndSixSameAnswer ^ xAndFiveSameAnswer;
                        return exam.GetAnswer(QuestionNumber.Nine) == kvp.Key ? finalAnswer : !finalAnswer;
                    });
            };
        }

        private static Predicate<Exam> Question10()
        {
            var mapping = new Dictionary<AnswerOption, int>
            {
                {AnswerOption.A, 3},
                {AnswerOption.B, 2},
                {AnswerOption.C, 4},
                {AnswerOption.D, 1}
            };

            return exam =>
            {
                var counter = mapping.Select(m => exam.Answers.Count(a => a == m.Key)).ToList();
                return counter.Max() - counter.Min() == mapping[exam.GetAnswer(QuestionNumber.Ten)];
            };
        }
    }
}