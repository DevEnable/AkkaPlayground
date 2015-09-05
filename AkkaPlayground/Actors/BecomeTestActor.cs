using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;
using AkkaPlayground.Messages;

namespace AkkaPlayground.Actors
{
    public class BecomeTestActor : ReceiveActor
    {
        public BecomeTestActor()
        {
            Become(A);
        }

        private void A()
        {
            Receive<StateAMessage>(m =>
            {
                Console.WriteLine("Received A");
                Become(B);
            });

            Common();
        }

        private void B()
        {
            Receive<StateBMessage>(m => Console.WriteLine("Recieved B"));
            Common();
        }

        private void Common()
        {
            ReceiveAny(m => Console.WriteLine("Recieved unexpected message {0}", m.ToString()));
        }


    }
}
