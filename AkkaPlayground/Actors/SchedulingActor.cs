using System;
using System.Threading;
using Akka.Actor;
using AkkaPlayground.Messages;

namespace AkkaPlayground.Actors
{
    public class SchedulingActor : ReceiveActor
    {
        private readonly IScheduler _scheduler;
        private Cancelable _cancel;
        private readonly TimeSpan _interval = TimeSpan.FromMilliseconds(500);

        private int _count;

        public int Count => _count;

        public SchedulingActor(IScheduler scheduler)
        {
            _scheduler = scheduler;

            Become(Off);
        }

        private void Off()
        {
            Receive<TurnOffMessage>(m => { });
            Receive<TurnOnMessage>(m =>
            {
                Become(On);
                _cancel = new Cancelable(_scheduler);
                _scheduler.ScheduleTellRepeatedly(_interval, _interval, Self, new TickMessage(), Self, _cancel);
                Console.WriteLine("Starting scheduler");
            });
            Receive<TickMessage>(m => Console.WriteLine("Recieved tick when actor state was Off") /* Theoritically possible */ );
        }

        private void On()
        {
            Receive<TurnOffMessage>(m =>
            {
                _cancel.Cancel();
                Become(Off);
                Console.WriteLine("Stopping scheduler");
            });
            Receive<TurnOnMessage>(m => Sender.Tell(new BadMessage()));
            Receive<TickMessage>(m => ProcessMessage(m));
        }

        private void ProcessMessage(TickMessage message)
        {
            Console.WriteLine("Counter tick {0}", ++_count);
        }



    }
}
