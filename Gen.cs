using System;

namespace GeneoCs
{
    /// <summary>
    /// Gen class for basic handeling data.
    /// </summary>
    public class Gen
    {
        private double normalValue;
        private double minLimit;
        private double maxLimit;
        private bool wraper;
        Random random;

        public Gen()
        {
            normalValue = 0.5;
            minLimit = 0.0;
            maxLimit = 1.0;
            wraper = false;
            random = new Random();
        }

        public Gen(Gen template)
        {
            normalValue = template.normalValue;
            minLimit = template.minLimit;
            maxLimit = template.maxLimit;
            wraper = template.wraper;
            random = new Random();
        }
        /// <summary>
        /// Return value of gen in min to max range.
        /// </summary>
        public double value
        {

            get { return Remap(normalValue, 0.0, 1.0, minLimit, maxLimit); }

            set { normalValue = Remap(Constrain(value, minLimit, maxLimit), minLimit, maxLimit, 0.0, 1.0); }

        }
        /// <summary>
        /// Minimum limit of Gen.
        /// Changing minimum limit will change actual value! Value will proportionaly change according to new limits.
        /// </summary>
        public double min
        {
            get { return minLimit; }

            set { minLimit = value; }
        }
        /// <summary>
        /// Maximum limit of Gen.
        /// Changing maximum limit will change actual value! Value will proportionaly change according to new limits.
        /// </summary>
        public double max
        {
            get { return maxLimit; }

            set { maxLimit = value; }

        }
        /// <summary>
        /// Wraping of values.
        /// If set true values could cycle around (circle-like). Influence beahaviour of Mutate(), Crossover() methods
        /// </summary>
        public bool wrap
        {
            get { return wraper; }

            set { wraper = value; }
        }
        /// <summary>
        /// Compute mutation on Gen.
        /// </summary>
        /// <param name="ratio">Percentage of deviation from current value. 0.0 = No mutation, 1.0 = Returns totaly random new value.</param>
        public void Mutate(double ratio)
        {
            if ((ratio >= 0.0) && (ratio <= 1.0))
            {
                // if is wraped
                double mutation = (random.NextDouble() * (2 * ratio) - ratio);

                if (wraper)
                {
                    normalValue = (1 + normalValue + mutation) % 1.0;
                }
                // when it is not wraped
                else
                {
                    normalValue = Constrain(normalValue + mutation, minLimit, maxLimit);
                }
            }
            else
            {
                throw new Exception($"Gen.Mutate(ratio) - ratio have to be in interval between 0.0 to 1.0. Requested: {ratio}.");
            }
        }
        /// <summary>
        /// Set randomly new value
        /// </summary>
        public void Randomize()
        {
            normalValue = random.NextDouble();
        }
        /// <summary>
        /// Combine two genes. Ratio of combination is selected randomly.
        /// </summary>
        /// <param name="gen">Second Gen to combine with</param>
        /// <returns>Result Gen of Crossover</returns>
        public Gen Crossover(Gen gen)
        {
            return Crossover(gen, random.NextDouble());
        }
        /// <summary>
        /// Combine two genes.
        /// </summary>
        /// <param name="gen">Second Gen to combine with</param>
        /// <param name="ratio">Percentage of combination between Genes. 0.0 = Returns first gen, 1.0 = Return second gen</param>
        /// <returns>Result Gen of Crossover</returns>
        public Gen Crossover(Gen gen, double ratio)
        {
            if (CompatibleWith(gen))
            {
                Gen result = Copy();
                // wrap
                if (wrap)
                {

                    // if wrap is not needed
                    if (Math.Abs((normalValue + gen.normalValue) / 2) <= 0.5)
                    {
                        result.normalValue = (normalValue * ratio + gen.normalValue * (1 - ratio));
                        return result;
                    }
                    // wrap needed (it's closer over origin)
                    else
                    {
                        // find which one is larger
                        if (normalValue <= gen.normalValue)
                        {
                            result.normalValue = ((normalValue * ratio + (gen.normalValue - 1) * (1 - ratio) + 1) % 1);
                        }
                        else
                        {
                            result.normalValue = ((gen.normalValue * ratio + (normalValue - 1) * (1 - ratio) + 1) % 1);
                        }
                        return result;
                    }
                }

                // no wrap
                else
                {
                    result.normalValue = (normalValue * ratio + gen.normalValue * (1 - ratio));
                    return result;
                }
            }
            else
            {
                throw new Exception("Incompatible gens");
            }
        }
        /// <summary>
        /// Creates deep copy of Gen
        /// </summary>
        /// <returns>Independent Copy of Gen</returns>
        public Gen Copy()
        {
            Gen result = new Gen();
            result.min = min;
            result.max = max;
            result.value = value;
            result.wrap = wrap;

            return result;
        }

        /// <summary>
        /// Checks compatibility fo thwo genes
        /// </summary>
        /// <param name="gen">Second gen to campare with</param>
        /// <returns>Returns true when both Genes are compatible. Otherwise return false.</returns>
        public bool CompatibleWith(Gen gen)
        {
            if ((min == gen.min) &&
                (max == gen.max) &&
                (wrap == gen.wrap))
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// Set min, max, wrap of gen based on template Gen.
        /// It will influence Value. See Gen.min or Gen.max.
        /// </summary>
        /// <param name="template">Template Gen to get settings from</param>
        public void AdjustByTemplate(Gen template)
        {
            min = template.min;
            max = template.max;
            wrap = template.wrap;
        }

        private double Remap(double number, double oldMin, double oldMax, double newMin, double newMax)
        {
            return ((number - oldMin) / (oldMax - oldMin)) * (newMax - newMin) + newMin;
        }

        private double Constrain(double number, double min, double max)
        {
            return Math.Max(min, Math.Min(number, max));
        }
        /// <summary>
        /// Debug method to Write Gen variables to console.
        /// </summary>
        public void Render()
        {
            Console.WriteLine($"Gen Object: \t value: {value} \t normalValue: {normalValue} \t min: {min} \t max: {max} \t wrap: {wrap}");
        }
    }
}
