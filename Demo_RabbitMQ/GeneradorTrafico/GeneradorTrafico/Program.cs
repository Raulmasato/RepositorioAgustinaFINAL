using RabbitMQ.Client;
using System.Text;

String patente = "AAAAAA";
while(patente.Length > 0)
{
    Console.WriteLine("Introducir patente: ");
    patente = Console.ReadLine();
    if(patente.Length > 0)
        EnviarPatente(patente);
}

void EnviarPatente(String patente){
    var factory = new ConnectionFactory() { HostName = "localhost" };
    using (var connection = factory.CreateConnection())
    {
        using (var canal = connection.CreateModel())
        {
            //Defino el nombre de la cola y el contenido del mensaje

            //-> Cola
            canal.QueueDeclare(queue: "GeneradorTrafico", durable: false, exclusive: false, autoDelete: false, arguments: null);

            //-> Mensaje
            string mensaje = patente;
            var body = Encoding.UTF8.GetBytes(mensaje);

            //->Enviamos el mensaje
            canal.BasicPublish(exchange: "", routingKey: "GeneradorTrafico", basicProperties: null, body: body);

            Console.WriteLine($" [x] Enviado {mensaje}");
        }
    }
}
    

