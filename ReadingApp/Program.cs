using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using static System.Net.Mime.MediaTypeNames;

namespace ReadingApp
{
    internal class Program
    {
        static int returnNumberOfWords(string filePath)
        {
            int wordCount = 0;
            using (StreamReader sr = new StreamReader(filePath))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    string[] words = RemovePunctuation(line).Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                    wordCount += words.Length;
                }
            }
            return wordCount;
        }

        static string returnLongestWord(string filePath)
        {
            string longestWord = string.Empty;

            using (StreamReader sr = new StreamReader(filePath))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    string[] words = RemovePunctuation(line).Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (string word in words)
                    {
                        if (word.Length > longestWord.Length)
                        {
                            longestWord = word;
                        }
                    }
                }
            }
            return longestWord;
        }

        static string returnShortestWord(string filePath)
        {
            string shortestWord = string.Empty;

            using (StreamReader sr = new StreamReader(filePath))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    string[] words = RemovePunctuation(line).Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (string word in words)
                    {
                        if (word.Length >= 4 && (shortestWord == string.Empty || word.Length < shortestWord.Length))
                        {
                            shortestWord = word;
                        }
                    }
                }
            }
            return shortestWord;
        }

        public Dictionary<string, int> returnCommonWords(string filePath)
        {
            Dictionary<string, int> wordFrequency = new Dictionary<string, int>();

            using (StreamReader sr = new StreamReader(filePath))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        string[] words = RemovePunctuation(line).Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

                        foreach (string word in words)
                        {
                            if (!string.IsNullOrWhiteSpace(word) && word.Length >= 4)
                            {
                                string cleanedWord = word.ToLower();
                                if (wordFrequency.ContainsKey(cleanedWord))
                                    wordFrequency[cleanedWord]++;
                                else
                                    wordFrequency[cleanedWord] = 1;
                            }
                        }
                    }
                }
            

            Dictionary<string, int> mostCommonWords = new Dictionary<string, int>();
            int count = 0;

            foreach (var entry in wordFrequency.OrderByDescending(x => x.Value))
            {
                mostCommonWords[entry.Key] = entry.Value;
                count++;

                if (count >= 5)
                    break;
            }

            return mostCommonWords;
        }


        public Dictionary<string, int> returnUncommonWords(string filePath)
        {
            Dictionary<string, int> wordFrequency = new Dictionary<string, int>();

            using (StreamReader sr = new StreamReader(filePath))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    string[] words = RemovePunctuation(line).Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (string word in words)
                    {
                        if (!string.IsNullOrWhiteSpace(word) && word.Length >= 4)
                        {
                            string cleanedWord = word.ToLower();
                            if (wordFrequency.ContainsKey(cleanedWord))
                                wordFrequency[cleanedWord]++;
                            else
                                wordFrequency[cleanedWord] = 1;
                        }
                    }
                }
            }

            Dictionary<string, int> mostUncommonWords = new Dictionary<string, int>();
            int count = 0;
            foreach (var pair in wordFrequency)
            {
                if (pair.Value == 1 && count<5)
                {
                    mostUncommonWords[pair.Key] = pair.Value;
                    count++;
                } 
            }
            return mostUncommonWords;
        }

        static string RemovePunctuation(string input)
        {
            string cleanText = Regex.Replace(input, @"[^\p{L}\s-]", "");

            cleanText = cleanText.Replace("-", "");

            return cleanText;
        }

        static void Main(string[] args)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            string bookName = "Verblud.txt";

            string filePath = Path.Combine(Directory.GetCurrentDirectory(), bookName);

            //Console.WriteLine(filePath);
            int numberOfWords = returnNumberOfWords(filePath);
            Console.WriteLine($"Number of words:{numberOfWords}");

            string longestWord = returnLongestWord(filePath);
            Console.WriteLine($"Longest Word: {longestWord}");

            string shortestWord = returnShortestWord(filePath);
            Console.WriteLine($"Shortest Word: {shortestWord}");

            Dictionary<string, int> commonWords = new Program().returnCommonWords(filePath);
            Console.Write("Five most common words: ");
            foreach (var entry in commonWords)
            {
                Console.Write($"{entry.Key}: {entry.Value}, ");
            }
            Console.WriteLine();

            Dictionary<string, int> uncommonWords = new Program().returnUncommonWords(filePath);
            Console.Write("Five most uncommon words: ");
            foreach (var entry in uncommonWords)
            {
                Console.Write($"{entry.Key}: {entry.Value}, ");
            }
            Console.WriteLine();
            sw.Stop();
            TimeSpan ts = sw.Elapsed;
            Console.WriteLine("Elapsed time: "+ ts.ToString("mm\\:ss\\.ff"));
        }
    }
}