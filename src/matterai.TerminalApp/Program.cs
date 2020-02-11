using System;

namespace matterai.TerminalApp
{
    static class Program
    {
        static void Main(string[] args)
        {
            var counter = new Counter(10);

            counter.OnThresholdReached += (sender, reached) 
                => Console.WriteLine($"[{reached.Occured:u}]: {reached.Message}");
            
            counter.Add(5);
            counter.Add(6);

            Console.ReadKey();
        }
    }
    
    public class Counter
    {
        private int _count;
        private readonly int _threshold;

        public Counter(int threshold, int startFrom = 0)
        {
            _threshold = threshold;
            _count = startFrom;
        }

        public event EventHandler<ThresholdReached> OnThresholdReached; 

        public void Add(int value)
        {
            _count += value;
            if (_count > _threshold)
            {
                OnThresholdReached?.Invoke(this, 
                    new ThresholdReached(
                    $"Threshold reached. Current counter value is: {_count}"));
            }
        }
    }

    public class ThresholdReached : EventArgs
    {
        public ThresholdReached(string message)
        {
            if (string.IsNullOrEmpty(message))
                throw new ArgumentNullException(nameof(message));
            Message = message;
            Occured = DateTime.Now;
        }
        
        public string Message { get; }
        public DateTime Occured { get; }
    }
}