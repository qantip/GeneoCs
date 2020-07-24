namespace GeneoCs
{
    class Program
    {
        static void Main(string[] args)
        {
            Dna temp = new Dna(5);
            Genepool gpool = new Genepool(temp, 10);
            gpool.Randomize();
            gpool.Render();
        }
    }
}
