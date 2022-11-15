# Task Description 
Task to process list of words (included in 'words.txt' list) and generate two output files with 'uniques' and 'fullwords'.

**The 'uniques' output file should:**
1.	Contain every sequence of four letters that appears in exactly one word of the words.txt Dictionary.
2.	Include only one sequence of four letters per line.
3.	Be sorted in alphabetical order.

**The 'fullwords' file should:**
1.	Contain a corresponding line for each full, original words appearing in the 'uniques' file.
2.	The fullwords file must be in the in the same order as the uniques file above.
3.	As above, only one full word per line.

# Implementation
Task developed with .Net Core console application written in C#
1. Created a Dictionary to capture unique and full words along with count of occurances. Used Dictionary as it maintains data in Key and Values with uniqueness, and getting data is faster as it uses key as index
```c#
        Dictionary<string, KeyPair> uniqueWords = new Dictionary<string, KeyPair>();
```
2. As Dictionary support key and value, created a class KeyPair to capture fullword and count of occurances as values
```C#
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
```
3. Used File.ReadAllLines to read all the lines from words.txt file which returns string[] and used foreach loop to loopthough all the lines 
```C#
        foreach (string word in File.ReadAllLines($"{RelativePath}InputData/words.txt"))
```
4. Had a condition to allow words whose length is graterthan or equal to the max length of substring provided
```C#
        if (word.Length >= SubStrLength)
```
5. Now we have to find out unique substring that is occured only once 
6. Created a GetSubstrings method to get substrings for the given string 
```C# 
        public static IEnumerable<string> GetSubstrings(string input)
		{
			for (int i = 0; i <= input.Length - SubStrLength; i++)
			{
				yield return (input.Substring(i, SubStrLength));
			}
		}
```
7. Loop though the each substring to capture the unique words and add it to Dictionary
```c#
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
```
8. Update the count of occurance with 1 if the substring was alrady exist
```c#
        uniqueWords[subStr].Count += 1;
```
9. Once done with capturing all the unique words filter the unique words which occured only once using a where clause, then sort them by ascending
```c#
        var result = uniqueWords.Where(v => v.Value.Count == 1).OrderBy(k => k.Key).ToDictionary(d => d.Key, d => d.Value.Word);
```
10. Converted the sorted result to Dictionary to key as unique and value as fullword so that we can export keys and values to txt files. 
```c#
        var result = uniqueWords.Where(v => v.Value.Count == 1).OrderBy(k => k.Key).ToDictionary(d => d.Key, d => d.Value.Word);
```
                                    (OR)
We can also use Select to get unique and fullword but if we want to use the result from the the select we have to do select one more time on the result to get keys and values
11. Export Keys to uniques.txt and Values to fullwords.txt
```c#
        File.WriteAllLines($"{RelativePath}OutputData/uniques.txt", result.Keys);
		File.WriteAllLines($"{RelativePath}OutputData/fullwords.txt", result.Values);
```
12. For mesuring the time taken to process the data Stopwatch isused. Started Stopwatch on start of method execution and stopped after completion of execution.
```c#
        Stopwatch stopWatch = Stopwatch.StartNew();
        ......
        .....
        stopWatch.Stop();
```
13. Execution time is calculated in milliseconds is logged into console

# Build and Test
1. Download or clone the repository
2. Open the project in VisualStudio
3. To run the project use Run or F5 or CTRL+F5
