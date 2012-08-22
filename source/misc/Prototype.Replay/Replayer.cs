using System;
using System.Diagnostics;
using System.Threading.Tasks.Dataflow;
using Microsoft.Practices.Unity;
using NLog;
using Prototype.Platform.Dispatching;
using Prototype.Platform.Domain;
using Prototype.Platform.Domain.Transitions.Interfaces;
using Uniform;

namespace Prototype.Replay
{
    public class Replayer
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        [Dependency]
        public ITransitionRepository TransitionRepository { get; set; }

        [Dependency]
        public IDispatcher Dispatcher { get; set; }

        [Dependency]
        public MongoViewDatabase MongoView { get; set; }

        [Dependency]
        public PrototypeSettings Settings { get; set; }

        [Dependency]
        public UniformDatabase UniformDatabase { get; set; }

        public void Start()
        {
            Console.WriteLine("Prototype.Reply");
            Console.WriteLine("---------");
            Console.WriteLine("  Events Database:");
            Console.WriteLine("      {0}", Settings.MongoEventsConnectionString);
            Console.WriteLine("  View Database:");
            Console.WriteLine("      {0}", Settings.MongoViewConnectionString);
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Are you sure that you want to drop this database? (type 'yes' or 'no')");
            Console.WriteLine("   {0}", Settings.MongoViewConnectionString);

            Console.Write("> ");

            var answer = Console.ReadLine();
            if (String.Compare(answer, "yes", StringComparison.OrdinalIgnoreCase) != 0)
                return;

            try
            {
                MongoView.Database.Drop();

                UniformDatabase.EnterInMemoryMode();

                var transitions = TransitionRepository.GetTransitions();
                var stopwatch = Stopwatch.StartNew();

                int counter = 0;
                foreach (var transition in transitions)
                {
                    foreach (var evnt in transition.Events)
                    {
                        if (++counter % 10000 == 0)
                            Console.WriteLine("Events #{0:n0}", counter);

                        DispatchAsync((IEvent) evnt.Data);
                    }
                }

                Console.WriteLine("Waiting for dispatch completion...");
                WaitForDispatchCompletion();

                Console.WriteLine("Flushing...");
                UniformDatabase.LeaveInMemoryMode(true);
                stopwatch.Stop();

                var message = String.Format("Replayed in {0}. Total number of events {1:n0}", 
                    stopwatch.Elapsed.ToReadableString(), counter);
                
                logger.Info(message);
                Console.WriteLine(message);

                Console.ReadKey();
            }
            catch (Exception ex)
            {
                logger.Fatal(ex);
            }
        }

        private readonly ActionBlock<Tuple<IDispatcher, IEvent>> _dispatchBlock =
            new ActionBlock<Tuple<IDispatcher, IEvent>>(tuple =>
            {
                try
                {
                    var dispatcher = tuple.Item1;
                    var evnt = tuple.Item2;

                    dispatcher.Dispatch(evnt);
                }
                catch (Exception ex)
                {
                    logger.Fatal(ex);
                }
            }, new ExecutionDataflowBlockOptions() { MaxDegreeOfParallelism = 1 });

        private void DispatchAsync(IEvent evnt)
        {
            _dispatchBlock.Post(new Tuple<IDispatcher, IEvent>(Dispatcher, evnt));
        }

        public void WaitForDispatchCompletion()
        {
            _dispatchBlock.Complete();
            _dispatchBlock.Completion.Wait();
        }
    }

    internal static class TimeSpanEntenstion
    {
        public static string ToReadableString(this TimeSpan span)
        {
            string formatted = string.Format("{0}{1}{2}{3}{4}",
                span.Days > 0 ? string.Format("{0:0} days, ", span.Days) : string.Empty,
                span.Hours > 0 ? string.Format("{0:0} hours, ", span.Hours) : string.Empty,
                span.Minutes > 0 ? string.Format("{0:0} minutes, ", span.Minutes) : string.Empty,
                span.Seconds > 0 ? string.Format("{0:0} seconds, ", span.Seconds) : string.Empty,
                span.Milliseconds > 0 ? string.Format("{0:0} ms", span.Milliseconds) : string.Empty);

            if (formatted.EndsWith(", ")) formatted = formatted.Substring(0, formatted.Length - 2);

            return formatted;
        }
    }
}