using System;
using System.Linq;

namespace statistics
{
    class Program
    {
        static void Main(string[] args)
        {
            string[,] data = {
                {"StdNum", "Name", "Math", "Science", "English"},
                {"1001", "Alice", "85", "90", "78"},
                {"1002", "Bob", "92", "88", "84"},
                {"1003", "Charlie", "79", "85", "88"},
                {"1004", "David", "94", "76", "92"},
                {"1005", "Eve", "72", "95", "89"}
            };

            int stdCount = data.GetLength(0) - 1;

            double totalMath = 0, totalScience = 0, totalEnglish = 0;
            double[] mathScores = new double[stdCount];
            double[] scienceScores = new double[stdCount];
            double[] englishScores = new double[stdCount];

            for (int i = 1; i <= stdCount; i++)
            {
                double mathScore = double.Parse(data[i, 2]);
                double scienceScore = double.Parse(data[i, 3]);
                double englishScore = double.Parse(data[i, 4]);

                totalMath += mathScore;
                totalScience += scienceScore;
                totalEnglish += englishScore;

                mathScores[i - 1] = mathScore;
                scienceScores[i - 1] = scienceScore;
                englishScores[i - 1] = englishScore;
            }

            double avgMath = totalMath / stdCount;
            double avgScience = totalScience / stdCount;
            double avgEnglish = totalEnglish / stdCount;

            double maxMath = mathScores[0], minMath = mathScores[0];
            double maxScience = scienceScores[0], minScience = scienceScores[0];
            double maxEnglish = englishScores[0], minEnglish = englishScores[0];

            for (int i = 1; i < stdCount; i++)
            {
                if (mathScores[i] > maxMath) maxMath = mathScores[i];
                if (mathScores[i] < minMath) minMath = mathScores[i];

                if (scienceScores[i] > maxScience) maxScience = scienceScores[i];
                if (scienceScores[i] < minScience) minScience = scienceScores[i];

                if (englishScores[i] > maxEnglish) maxEnglish = englishScores[i];
                if (englishScores[i] < minEnglish) minEnglish = englishScores[i];
            }

            Console.WriteLine("Average Scores: ");
            Console.WriteLine($"Math: {avgMath:F2}");
            Console.WriteLine($"Science: {avgScience:F2}");
            Console.WriteLine($"English: {avgEnglish:F2}");
            Console.WriteLine();

            Console.WriteLine("Max and min Scores: ");
            Console.WriteLine($"Math: ({maxMath}, {minMath})");
            Console.WriteLine($"Science: ({maxScience}, {minScience})");
            Console.WriteLine($"English: ({maxEnglish}, {minEnglish})");
            Console.WriteLine();

            var studentScores = new (string Name, double TotalScore)[stdCount];

            for (int i = 1; i <= stdCount; i++)
            {
                double totalScore = mathScores[i - 1] + scienceScores[i - 1] + englishScores[i - 1];
                studentScores[i - 1] = (data[i, 1], totalScore);
            }

            var rankedStudents = studentScores.OrderByDescending(s => s.TotalScore).ToArray();

            Console.WriteLine("Students rank by total scores:");
            for (int i = 0; i < rankedStudents.Length; i++)
            {
                Console.WriteLine($"{rankedStudents[i].Name}: {i + 1}th");
            }
        }
    }
}

/* example output

Average Scores: 
Math: 84.40
Science: 86.80
English: 86.20

Max and min Scores: 
Math: (94, 72)
Science: (95, 76)
English: (92, 78)

Students rank by total scores:
Alice: 4th
Bob: 1st
Charlie: 5th
David: 2nd
Eve: 3rd

*/
