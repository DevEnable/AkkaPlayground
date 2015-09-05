using System;
using Akka.Actor;
using AkkaPlayground.Messages;

namespace AkkaPlayground.Actors
{
    public class ParentWithChildPropsActor : ReceiveActor
    {
        private readonly IActorRef _childActor;

        public ParentWithChildPropsActor() : this(Props.Create(() => new ChildActor("Jim")))
        {
            
        }
        
        public ParentWithChildPropsActor(Props childProps)
        {
            _childActor = Context.ActorOf(childProps, "child");

            Receive<ChildDoWorkMessage>(m => _childActor.Forward(m));
        }

    }

    public class ChildActor : ReceiveActor
    {
        public string Name { get; private set; }

        public ChildActor(string name)
        {
            Name = name;

            Receive<ChildDoWorkMessage>(m =>
            {
                Console.WriteLine("Werk.");
            });
        }
    }

}
