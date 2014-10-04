using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            Do(ProcessAsLoop, "parallel sort, series loop process");

            Console.WriteLine();
            Console.ReadLine();
        }

        public static void Do(Func<int> action, string text)
        {
            var sw = new Stopwatch();
            sw.Start();
            var result = action();
            sw.Stop();
            //Console.WriteLine(result);
            Console.WriteLine("completed " + text + " in " + sw.Elapsed);
        }

        /// <summary>
        /// Sorts names in parallel, then gets the name's character values *
        /// the position in the list.
        /// </summary>
        public static int ProcessParallel()
        {
            return ProcessParallel(_names);
        }

        /// <summary>
        /// Sorts names in parallel, then gets the name's character values *
        /// the position in the list.
        /// </summary>
        public static int ProcessParallel(IEnumerable<string> names)
        {
            var result = names.AsParallel()
                .OrderBy(a => a)
                .Select((t, i) => GetNameNumberValue(t) * (i + 1))
                .AsUnordered()
                .Sum();
            return result;
        }

        /// <summary>
        /// Sorts names, then gets the name's character values *
        /// the position in the list.
        /// </summary>
        public static int ProcessSeries()
        {
            return ProcessSeries(_names);
        }

        /// <summary>
        /// Sorts names, then gets the name's character values *
        /// the position in the list.
        /// </summary>
        public static int ProcessSeries(IEnumerable<string> names)
        {
            var result = names
                .OrderBy(a => a)
                .Select((t, i) => GetNameNumberValue(t) * (i + 1))
                .Sum();
            return result;
        }

        /// <summary>
        /// Does the same operation, but uses a for loop to add to the
        /// sum.
        /// </summary>
        /// <returns></returns>
        public static int ProcessAsLoop()
        {
            return ProcessAsLoop(_names);
        }

        /// <summary>
        /// Does the same operation, but uses a for loop to add to the
        /// sum.
        /// </summary>
        /// <returns></returns>
        public static int ProcessAsLoop(IEnumerable<string> names)
        {
            int sum = 0;
            var sorted = names.AsParallel().OrderBy(a => a).ToArray();
            for (int i = 0; i < sorted.Length; i++)
                sum += GetNameNumberValue(sorted[i])*(i + 1);
            return sum;
        }

        /// <summary>
        /// Get the total value of the name. Pass in the upper case.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static int GetNameNumberValue(string name)
        {
            return name.Sum(c => c - 64);
        }


        #region OpenFile

        private static void LoadFile(string fName)
        {
            try
            {
                using (var reader = new StreamReader(fName))
                {
                    _names = reader.ReadLine().Replace("\"", "").ToUpper().Split(',');
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        #endregion OpenFile
    }
}
