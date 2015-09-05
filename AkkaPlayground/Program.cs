using System;
using Akka.Actor;
using AkkaPlayground.Actors;
using AkkaPlayground.Messages;

namespace AkkaPlayground
{
    public class Program
    {
        private static ActorSystem _system;

        static void Main(string[] args)
        {
            _system = ActorSystem.Create("TestSystem");
            SchedulerScenario();
            Console.ReadLine();
        }

        private static void SchedulerScenario()
        {
            IActorRef scheduling = _system.ActorOf(Props.Create(() => new SchedulingActor(_system.Scheduler)), "scheudler");
            scheduling.Tell(new TurnOnMessage());
            Console.ReadLine();
            scheduling.Tell(new TurnOffMessage());
        }


        private static void BecomeScenario()
        {
            IActorRef becomeActor = _system.ActorOf(Props.Create(() => new BecomeTestActor()), "become");

            becomeActor.Tell(new StateAMessage());
            becomeActor.Tell(new StateBMessage());
            becomeActor.Tell(new StateAMessage());
        }

        private static void ChildPropsScenario()
        {
            IActorRef funcActor = _system.ActorOf(Props.Create(() => new ParentWithChildPropsActor()));
            funcActor.Tell(new ChildDoWorkMessage());
        }
    }
}