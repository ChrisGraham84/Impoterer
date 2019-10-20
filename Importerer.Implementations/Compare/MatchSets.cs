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
            List <T> exceptions = new List<T>();
            foreach (var comp in Compare)
            {
                if (!Source.Contains<T> (comp))
                {
                    exceptions.Add(comp);
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
            }
            return exceptions;
        }
    }
}
