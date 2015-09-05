using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.TestKit;

namespace AkkaPlayground.Tests
{
    public class ProbingActor : UntypedActor
    {
        private readonly TestProbe _probe;

        public ProbingActor(TestProbe probe)
        {
            _probe = probe;
        }

        protected override void OnReceive(object message)
        {
            _probe.Tell(message);
        }
    }
}
