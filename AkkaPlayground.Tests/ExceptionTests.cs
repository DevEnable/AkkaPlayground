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

    public class ExceptionTests : TestKit
    {
        [Fact]
        public void Do_Khaboom()
        {
            IActorRef bad = ActorOf<MyBadActor>();
            bad.Tell("boom");
            ExpectMsg<Failure>();
        }
    }
}
