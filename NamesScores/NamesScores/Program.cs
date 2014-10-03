using System;
using System.IO;
using System.Linq;
using System.Text;

namespace NamesScores
{
    public class Program
    {
//        using names.txt (right click and 'save link/target as...'), a 46k text file 
        //containing over five-thousand first names, begin by sorting it into alphabetical order. 
        //then working out the alphabetical value for each name, multiply this value by its 
        //alphabetical position in the list to obtain a name score.

//for example, when the list is sorted into alphabetical order, 
        //colin, which is worth 3 + 15 + 12 + 9 + 14 = 53, is the 
        //938th name in the list. so, colin would obtain a score of 938 × 53 = 49714.

//what is the total of all the name scores in the file?

        private static string[] _names;
        private const string fileName = "p022_names.txt";

        static void Main(string[] args)
        {
            LoadFile(fileName);

            Do(ProcessParallel, "parallel process");
            Do(ProcessSeries, "series process");

            Console.WriteLine();
            Console.ReadLine();
        }

        public static void Do(Func<int> action, string text)
        {
            var start = DateTime.Now;
            var result = action();
            Console.WriteLine("completed " + text + " in " + (DateTime.Now - start));
        }

        /// <summary>
        /// Sorts names in parallel, then gets the name's character values *
        /// the position in the list.
        /// </summary>
        public static int ProcessParallel()
        {
            var result = _names.AsParallel()
                .OrderBy(a => a)
                .Select((t, i) => GetNameNumberValue(t)*(i + 1))
                .Sum();
            return result;
        }

        /// <summary>
        /// Sorts names, then gets the name's character values *
        /// the position in the list.
        /// </summary>
        public static int ProcessSeries()
        {
            var result = _names
                .OrderBy(a => a)
                .Select((t, i) => GetNameNumberValue(t)*(i + 1))
                .Sum();
            return result;
        }

        /// <summary>
        /// Get the total value of the name. Pass in the upper case.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static int GetNameNumberValue(string name)
        {
            return name.Sum(c => GetCharacterNumber(c));
        }

        /// <summary>
        /// Pass the upper case character to get its value;
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        private static int GetCharacterNumber(char c)
        {
            const string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            int number = letters.IndexOf(c) + 1; // so that A = 1
            return number;
        }


        #region OpenFile

        private static void LoadFile(string fName)
        {
            try
            {
                using (var reader = new StreamReader(fName))
                {
                    var line = reader.ReadLine();
                    line = RemoveQuotes(line).ToUpper();
                    var names = line.Split(',');
                    _names = names;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


        /// <summary>
        /// Returns a new string without any quotation marks.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        private static string RemoveQuotes(string source)
        {
            var sb = new StringBuilder();
            foreach (var c in source)
                if (c != '"')
                    sb.Append(c);
            return sb.ToString();
        }
        #endregion OpenFile
    }
}
