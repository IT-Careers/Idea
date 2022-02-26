namespace Idea.Service
{
    public class RandomService : IRandomService
    {
        private readonly int seed;

        private readonly Random random;

        public RandomService(int seed)
        {
            this.random = new Random(seed);
        }

        public int Seed => this.seed;

        public long RandomNumber(long min, long max) => this.random.NextInt64(min, max);
    }
}
