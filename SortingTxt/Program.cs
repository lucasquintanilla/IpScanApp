using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SortingTxt
{
    class Program
    {
        static void Main(string[] args)
        {
            //string inFile = "newvision.txt";
            //string outFile = "newvu-ordenado.txt";
            //var contents = File.ReadAllLines(inFile);
            //Array.Sort(contents);
            //File.WriteAllLines(outFile, contents);

            string[] lines = File.ReadAllLines("defeway60001.txt");

            for (int i = 0; i < lines.Length; i++)
            {
                lines[i] = lines[i] + ":60001";
            }

            Array.Sort(lines);
            File.WriteAllLines("defeway60001-nuevo.txt", lines.Distinct().ToArray());

            Console.WriteLine("Good Job");
            Console.ReadLine();
        }
    }
}
