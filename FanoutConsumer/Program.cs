﻿using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace FanoutConsumer
{
    class Program
    {
        static IConnection conn;
        static IModel channel;

        static void Main(string[] args)
        {

            ConnectionFactory factory = new ConnectionFactory();
            factory.HostName = "localhost";
            factory.VirtualHost = "/";
            factory.Port = 5672;
            factory.UserName = "guest";
            factory.Password = "guest";

            conn = factory.CreateConnection();
            channel = conn.CreateModel();

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += Consumer_Received;

            //var consumerTag = channel.BasicConsume("my.queue1", true, consumer);
            var consumerTag = channel.BasicConsume("my.queue1", false, consumer);

            Console.WriteLine("Waiting for messages. Press any key to exit.");
            Console.ReadKey();

        }

        private static void Consumer_Received(object sender, BasicDeliverEventArgs e)
        {
            string message = Encoding.UTF8.GetString(e.Body.ToArray());
            Console.WriteLine("Message:" + message);

            //delete from queue
            channel.BasicAck(e.DeliveryTag, false);
            //requeue true
            //channel.BasicNack(e.DeliveryTag, false, true);
            //requeue false
            //channel.BasicNack(e.DeliveryTag, false, false);

        }
    }
}
