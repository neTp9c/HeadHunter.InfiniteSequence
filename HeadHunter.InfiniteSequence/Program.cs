using System;
using System.Collections.Generic;

namespace HeadHunter.InfiniteSequence
{
    class Program
    {
        static void Main(string[] args)
        {
            var sequences = ReadFragments();
            WriteFragmentsCalculation(sequences);
        }

        static IEnumerable<string> ReadFragments()
        {
            var sequences = new List<string>();

            while (true)
            {
                var sequence = Console.ReadLine();
                if(string.IsNullOrWhiteSpace(sequence))
                {
                    break;
                }
                sequences.Add(sequence);
            }

            return sequences;
        }

        static void WriteFragmentsCalculation(IEnumerable<string> sequences)
        {
            foreach (var sequence in sequences)
            {
                var indexOfFragment = InfiniteNumberSequence.IndexOfFragment(sequence);
                Console.WriteLine(indexOfFragment);
            }
        }
    }
}
