/// <summary>
/// Simple pseudo random number generator.
/// </summary>
public class PRNG
{
    private System.Random random;

    public PRNG(int seed)
    {
        random = new System.Random(seed);
    }

    /// <summary>
    /// Generates a random double in the provided range.
    /// </summary>
    /// <param name="min">Range minimum.</param>
    /// <param name="max">Range maximum.</param>
    /// <returns>The generated number.</returns>
    public double NextRange(double min, double max)
    {
        double nextDouble = random.NextDouble();

        double result = (nextDouble * (max - min)) + min;

        return result;
    }
}
