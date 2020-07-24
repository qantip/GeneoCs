using System;
using System.Collections.Generic;

namespace GeneoCs
{
    public class Genepool
    {
        List<Dna> pool;
        Dna sampleDna;
        public Genepool(Dna template, int newCount)
        {
            pool = new List<Dna>();
            sampleDna = template.Copy();

            if (newCount > 0)
            {
                if (newCount > count)
                {
                    for (var i = count; i < newCount; i++)
                    {
                        pool.Add(new Dna(template));
                    }
                }
                else
                {
                    pool.RemoveRange(newCount, count - newCount);
                }
            }
            else
            {
                throw new System.InvalidOperationException($"Count can not be equal or less then 0. Requested {count}");
            }
        }

        public Dna template
        {
            get { return sampleDna; }

            set
            {
                UseTemplate(value);
            }
        }

        public void UseTemplate(Dna template)
        {
            sampleDna = template;
            for (var i = 0; i < count; i++)
            {
                pool[i].AdjustByTemplate(template);
            }
        }

        public void Randomize()
        {
            foreach (Dna dna in pool)
            {
                dna.Randomize();
            }
        }

        public int count
        {
            get { return pool.Count; }
        }

        private bool SelfCheck()
        {
            return true;
        }

        public void Render()
        {
            Console.WriteLine($"Generation object of {count} DNAs.");
            for (var i = 0; i < count; i++)
            {
                pool[i].Render();
            }
            Console.WriteLine("---  Generation END");
        }
    }
}
