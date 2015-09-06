using System;
using Akka.Actor;
using Akka.TestKit.Xunit2;
using Xunit;

namespace AkkaPlayground.Tests
{
    public class MyBadActor : UntypedActor
    {
        protected override void OnReceive(object message)
        {
            throw new InvalidOperationException("The smurfs are invading!");
        }
    }

    internal class UnhandledExceptionMessage
    {
        public Exception Exception { get; private set; }

        public UnhandledExceptionMessage(Exception ex)
        {
            Exception = ex;
        }
    }

    public class ExceptionalActor : UntypedActor
    {
        private readonly IActorRef _probe;

        public IActorRef SupervisedActor { get; private set; }

        public ExceptionalActor(IActorRef probe, Props supervisedActorProps)
        {
            _probe = probe;
            SupervisedActor = Context.ActorOf(supervisedActorProps, "child");
        }

        protected override SupervisorStrategy SupervisorStrategy()
        {
            return new OneForOneStrategy(-1, TimeSpan.FromSeconds(3), x =>
            {
                _probe.Tell(new UnhandledExceptionMessage(x));

                return Directive.Resume;
            });
        }

        protected override void OnReceive(object message)
        {
            SupervisedActor.Forward(message);
        }
    }

    public class ExceptionTests : TestKit
    {
        [Fact]
        public void Do_Khaboom()
        {
            var probe = CreateTestProbe("ExceptionProbe");
            var exceptionalActor = ActorOfAsTestActorRef<ExceptionalActor>(Props.Create(() => new ExceptionalActor(probe, Props.Create<MyBadActor>())));
            exceptionalActor.Tell("It's a thing!");

            probe.ExpectMsg<UnhandledExceptionMessage>();
        }
    }
}
