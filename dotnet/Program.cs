using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Google.Protobuf;
using Grpc.Core;
using Helloworld;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new Client();
            client.Start();

            Console.ReadLine();
        }

        class Client
        {
            const string Host = "localhost";
            const int Port = 50051;
            private readonly Greeter.GreeterClient _greeterClient;

            public Client()
            {
                var channel = new Channel(Host, Port, ChannelCredentials.Insecure);
                _greeterClient = new Greeter.GreeterClient(channel);
            }

            private static byte[] data;

            public void Start()
            {
                Task.Run(() =>
                {

                    while (true)
                    {
                        try
                        {
                            var reply = _greeterClient.SayHello(new HelloRequest() {Name = "Wally"});
                            Console.WriteLine($"Client received:{reply.Message}");
                            Thread.Sleep(500);
                        }
                        catch (RpcException ex)
                        {
                            Console.WriteLine(ex.Message);
                            Console.WriteLine($"Status code:{ex.StatusCode}");
                            Thread.Sleep(500);
                        }
                    }
                });

            }

        }
    }
}
