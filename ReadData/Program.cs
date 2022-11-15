using System;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;

namespace ReadData
{
	class Program
	{
		/// <summary>
		/// Maximum length of substring
		/// </summary>
		private static readonly int SubStrLength = 4;

		/// <summary>
		/// Relative path to input and output files
		/// </summary>
		private static readonly string RelativePath = "../../../";

		/// <summary>
		/// Process the data from wordstest.txt and generates uniques.txt and fullwords.txt
		/// </summary>
		public static void ProcessData()
		{
			try
			{
				// Start timer
				Stopwatch stopWatch = Stopwatch.StartNew();

                // Dictionary to capture unique word, fullword and count of occurances
                // Used Dictionary as it maintains data in Key and Values with uniqueness,
				// and getting data is faster as it uses key as index
                Dictionary<string, KeyPair> uniqueWords = new Dictionary<string, KeyPair>();

				// Read all lines from input file
				foreach (string word in File.ReadAllLines($"{RelativePath}InputData/words.txt"))
				{
					if (word.Length >= SubStrLength)
					{
						// Extract possible substrings for each word and capture the occurances
						foreach (string subStr in GetSubstrings(word))
						{
							if (!uniqueWords.ContainsKey(subStr))
							{
								uniqueWords.Add(subStr, new KeyPair(word, 1));
							}
							else
							{
								uniqueWords[subStr].Count += 1;
							}
						}
					}
				}

				// Filter unique substrings that occured only once and sort them
				var result = uniqueWords.Where(v => v.Value.Count == 1).OrderBy(k => k.Key).ToDictionary(d => d.Key, d => d.Value.Word);

				// Export unique and fullwords to .txt files
				File.WriteAllLines($"{RelativePath}OutputData/uniques.txt", result.Keys);
				File.WriteAllLines($"{RelativePath}OutputData/fullwords.txt", result.Values);

				// Stop timer
				stopWatch.Stop();

				Console.WriteLine($"Processed data in: {stopWatch.ElapsedMilliseconds} ms");
			}
			catch (Exception ex)
			{
				Console.WriteLine("Failed to process because of : " + ex.Message);
			}
		}

		/// <summary>
		/// Get substrings for the given string
		/// </summary>
		/// <param name="input">Input string</param>
		/// <returns>Returns substrings</returns>
		public static IEnumerable<string> GetSubstrings(string input)
		{
			for (int i = 0; i <= input.Length - SubStrLength; i++)
			{
				yield return (input.Substring(i, SubStrLength));
			}
		}

		static void Main(string[] args)
        {
            // Method to process the data from words.txt
            ProcessData();

            Console.ReadLine();
        }
    }

	/// <summary>
	/// Class to maintain fullword and count of occurances
	/// </summary>
	public class KeyPair
	{
		public string Word { get; }
		public int Count { get; set; }

		public KeyPair(string word, int count)
		{
			this.Word = word;
			this.Count = count;
		}
	}
}
