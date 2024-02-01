public class PRNG
{
    private System.Random random;

    public PRNG(int seed)
    {
        random = new System.Random(seed);
    }

    public double NextRange(double min, double max)
    {
        double nextDouble = random.NextDouble();

        double result = (nextDouble * (max - min)) + min;

        return result;
    }
}
