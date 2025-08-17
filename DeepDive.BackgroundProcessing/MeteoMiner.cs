using OpenMeteo;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DeepDive.BackgroundProcessing
{
    public class MeteoMiner
    {
        private BlockingCollection<string> _queue = new();

        private OpenMeteoClient client = new OpenMeteoClient();

        public MeteoMiner(List<string> towns)
        {
            Enqueue(towns);
        }

        public void Start()
        {
            var thread = new Thread(FetchTownWeather);
            thread.Start();
        }

        public void Enqueue(List<string> towns)
        {
            foreach (var town in towns)
            {
                _queue.Add(town);
            }
        }

        public void Stop()
        {
            _queue.CompleteAdding();
        }

        private void FetchTownWeather()
        {
            foreach(var town in _queue.GetConsumingEnumerable())
            {
                try
                {
                    var response = client.Query(town);
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.WriteLine($"{town}:{response.Current.Temperature}");
                }
                catch(Exception e)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine(e.Message);
                }
                Console.ForegroundColor = ConsoleColor.White;

            }
        }

    }
}
