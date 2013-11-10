using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Trees;

namespace testconsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Oak oak = new Oak();
            string kaas;
            while ((kaas = Console.ReadLine().ToLower()) != "quit")
            {
                Console.WriteLine(oak.ProcessLine(kaas));
            }
        }
    }
}
