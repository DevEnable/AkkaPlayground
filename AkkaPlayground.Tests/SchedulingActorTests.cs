using System;
using Akka.Actor;
using Akka.TestKit;
using Akka.TestKit.Configs;
using Akka.TestKit.Xunit2;
using AkkaPlayground.Actors;
using AkkaPlayground.Messages;
using Xunit;

namespace AkkaPlayground.Tests
{
    public class SchedulingKit : TestKit
    {
        public SchedulingKit() : base(TestConfigs.TestSchedulerConfig)
        { }

    }

    public class SchedulingActorTests : SchedulingKit
    {
        private readonly TestActorRef<SchedulingActor> _schedulingActor;
        private readonly TestScheduler _scheduler;
         
        public SchedulingActorTests()
        {
            _scheduler = (TestScheduler)Sys.Scheduler;
            _schedulingActor = ActorOfAsTestActorRef(() => new SchedulingActor(_scheduler));
        }

        [Fact]
        public void No_Tick()
        {
            _schedulingActor.Tell(new TurnOnMessage());
            _scheduler.Advance(TimeSpan.FromMilliseconds(400));
            Assert.Equal(0, _schedulingActor.UnderlyingActor.Count);
        }

        [Fact]
        public void Do_Tick()
        {
            IActorRef a = CreateTestProbe("p");
            _schedulingActor.Tell(new TurnOnMessage());
            _scheduler.Advance(TimeSpan.FromMilliseconds(600));
            Assert.Equal(1, _schedulingActor.UnderlyingActor.Count);
        }
        
    }
}
