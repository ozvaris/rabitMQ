using RabbitMQ.Client;
using System;
using System.Text;

namespace DirectPublisher
{
    class Program
    {
        static void Main(string[] args)
        {
            IConnection conn;
            IModel channel;

            ConnectionFactory factory = new ConnectionFactory();
            factory.HostName = "localhost";
            factory.VirtualHost = "/";
            factory.Port = 5672;
            factory.UserName = "guest";
            factory.Password = "guest";

            conn = factory.CreateConnection();
            channel = conn.CreateModel();

            channel.ExchangeDeclare("ex.direct", "direct", true, false, null);

            channel.QueueDeclare("my.warnings", true, false, false, null);
            channel.QueueDeclare("my.errors", true, false, false, null);
            channel.QueueDeclare("my.infos", true, false, false, null);


            channel.QueueBind("my.warnings", "ex.direct", "warning");
            channel.QueueBind("my.errors", "ex.direct", "error");
            channel.QueueBind("my.infos", "ex.direct", "info");


            channel.BasicPublish("ex.direct", "warning", null, Encoding.UTF8.GetBytes("Message 1"));
            channel.BasicPublish("ex.direct", "error", null, Encoding.UTF8.GetBytes("Message 2"));
            channel.BasicPublish("ex.direct", "info", null, Encoding.UTF8.GetBytes("Message 2"));


            Console.WriteLine("Press a key to exit.");
            Console.ReadKey();

            channel.QueueDelete("my.warnings");
            channel.QueueDelete("my.errors");
            channel.QueueDelete("my.infos");

            channel.ExchangeDelete("ex.direct");

            channel.Close();
            conn.Close();


        }
    }
}
