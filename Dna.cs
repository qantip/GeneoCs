using System;
using System.Collections.Generic;

namespace GeneoCs
{
    /// <summary>
    /// Class for elementry data handling. Simulation of signle gen behaviour.
    /// </summary>
    public class Dna
    {
        private List<Gen> genes = new List<Gen>();
        private Random random = new Random();

        public Dna()
        {
            genes = new List<Gen>();
        }

        public Dna(int length)
        {
            genes = new List<Gen>();
            for (var i = 0; i < length; i++)
            {
                genes.Add(new Gen());
            }
        }

        public Dna(Dna template)
        {
            genes = new List<Gen>();
            for (var i = 0; i < template.length; i++)
            {
                genes.Add(new Gen(template.genes[i]));
            }
        }

        public double GetValue(int index)
        {
            try
            {
                return genes[index].value;
            }
            catch
            {
                throw new System.InvalidOperationException($"Index out of range. Dna length: {genes.Count}, requested index: {index}");
            }
        }

        public void SetValue(int index, double value)
        {
            genes[index].value = value;
        }

        public double GetMin(int index)
        {
            return genes[index].min;
        }
        public void SetMin(int index, double value)
        {
            genes[index].min = value;
        }

        public double GetMax(int index)
        {
            return genes[index].max;
        }
        public void SetMax(int index, double value)
        {
            genes[index].max = value;
        }

        public bool GetWrap(int index)
        {
            return genes[index].wrap;
        }
        public void SetWrap(int index, bool value)
        {
            genes[index].wrap = value;
        }

        public void Add(Gen gen)
        {
            genes.Add(gen);
        }

        public void RemoveAt(int index)
        {
            try
            {
                genes.RemoveAt(index);
            }
            catch
            {
                throw new System.InvalidOperationException($"Index out of range. Dna length: {genes.Count}, requested index: {index}");
            }
        }

        public void SetLength(int targetLength)
        {
            if (targetLength > length)
            {
                for (var i = genes.Count; i < targetLength; i++)
                {
                    this.genes.Add(new Gen());
                }
            }
            // length >= targetLength
            else
            {
                genes.RemoveRange(targetLength, length - targetLength);
            }


        }

        public void Randomize()
        {
            for (var i = 0; i < genes.Count; i++)
            {
                genes[i].Randomize();
            }
        }

        public void Mutate(double chance, double rate)
        {
            for (var i = 0; i < genes.Count; i++)
            {
                if (random.NextDouble() <= chance)
                {
                    genes[i].Mutate(rate);
                }
            }
        }

        public Dna Copy()
        {
            Dna result = new Dna(0);
            for (var i = 0; i < length; i++)
            {
                result.Add(genes[i].Copy());
            }
            return result;
        }

        public bool CompatibleWith(Dna dna)
        {
            if (length == dna.length)
            {
                for (var i = 0; i < length; i++)
                {
                    if (!genes[i].CompatibleWith(dna.genes[i]))
                    {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }

        public void AdjustByTemplate(Dna template)
        {
            if (genes.Count > template.genes.Count)
            {
                genes.RemoveRange(template.genes.Count, genes.Count - template.genes.Count);
                for (var i = 0; i < genes.Count; i++)
                {
                    genes[i].AdjustByTemplate(template.genes[i]);
                }
            }
            else
            {
                for (var i = 0; i < genes.Count; i++)
                {
                    genes[i].AdjustByTemplate(template.genes[i]);
                }
                for (var i = genes.Count; i < template.genes.Count; i++)
                {
                    genes.Add(template.genes[i]);
                }
            }
        }

        public void Render()
        {
            Console.WriteLine($"Dna Object of {genes.Count} Genes.");
            for (var i = 0; i < genes.Count; i++)
            {
                genes[i].Render();
            }
            Console.WriteLine("--- Dna END");
        }

        public int length
        {
            get
            {
                return genes.Count;
            }
        }
    }
}
