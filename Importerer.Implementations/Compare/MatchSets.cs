using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Importerer.Interfaces;

namespace Importerer.Implementations
{
    public class MatchSets<T> : IComparerer<T>
    {
        public List<T> Compare(List<T> Source, List<T> Compare)
        {
            Console.WriteLine("Compare Start");
            List <T> exceptions = new List<T>();
            int i = 1;
            foreach (var comp in Compare)
            {
                Console.WriteLine("Comparing Line {0}", i);
                if (!Source.Contains<T> (comp))
                {
                    exceptions.Add(comp);
                    Console.WriteLine("Line {0} does have a matching entry", i);
                }
                #region Alternate
                //bool found = false;
                //foreach (var sour in Source)
                //{
                //    if (sour.Equals(comp))
                //    {
                //        found = true;
                //        break;
                //    }
                //}
                //if (!found) { exceptions.Add(comp); }
                #endregion
                i++;
            }
            Console.WriteLine("Compare End");
            Console.WriteLine("Mismatched Count: {0}", exceptions.Count());
            return exceptions;
        }
    }
}
