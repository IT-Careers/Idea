namespace Idea.Service
{
    public interface IRandomService
    {
        int Seed { get; }

        long RandomNumber(long min, long max);
    }
}
