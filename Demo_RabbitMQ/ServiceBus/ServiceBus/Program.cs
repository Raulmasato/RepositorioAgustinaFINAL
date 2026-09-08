using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

var factory = new ConnectionFactory() { HostName = "localhost" };
using (var connection = factory.CreateConnection())
using (var canal = connection.CreateModel())
{

    canal.QueueDeclare(queue: "GeneradorTrafico", durable: false, exclusive: false, autoDelete: false, arguments: null);

    var consumidor = new EventingBasicConsumer(canal);
    consumidor.Received += (model, ea) =>
    {
        var body = ea.Body.ToArray();
        var mensaje = Encoding.UTF8.GetString(body);

        Console.WriteLine($" [X] Recibido {mensaje}");

    };

    canal.BasicConsume(queue: "GeneradorTrafico", autoAck: true, consumer: consumidor);

    Console.WriteLine("Running on localhost");
    Console.WriteLine("Press enter to exit");
    Console.ReadKey();
}