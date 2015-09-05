
using Akka.Actor;
using Akka.TestKit;
using Akka.TestKit.Xunit2;
using AkkaPlayground.Actors;
using AkkaPlayground.Messages;
using Xunit;

namespace AkkaPlayground.Tests
{
    public class ChildPropsTests : TestKit
    {

        [Fact(DisplayName = "Check child receives message")]
        public void Gets_Message()
        {
            var probe = CreateTestProbe();
            Props testProps = Props.Create(() => new ProbingActor(probe));
            var testActor = ActorOfAsTestActorRef(() => new ParentWithChildPropsActor(testProps));

            testActor.Tell(new ChildDoWorkMessage());
            probe.ExpectMsg<ChildDoWorkMessage>();
        }
    }
}
