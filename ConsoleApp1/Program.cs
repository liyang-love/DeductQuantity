namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");


            int target = 0;
            int expected = 0;
            int desired = 1;
            bool success = Interlocked.CompareExchange(ref target, desired, expected) == expected; // target=1
            Console.WriteLine($"target:{target} expected:{expected} desired:{desired} success:{success}");


        }
    }
}
