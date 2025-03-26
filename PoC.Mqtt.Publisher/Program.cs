using MQTTnet;
using MQTTnet.Protocol;

Console.Title = "MQTT Publisher";

Console.WriteLine("Digite o ID do cliente MQTT:");
var clientId = Console.ReadLine();
Console.Title = $"MQTT Publisher - {clientId}";

var mqttFactory = new MqttClientFactory();
using var mqttClient = mqttFactory.CreateMqttClient();

var mqttOptions = new MqttClientOptionsBuilder()
    .WithTcpServer("localhost")
    .WithClientId(clientId)
    .Build();

await mqttClient.ConnectAsync(mqttOptions);
Console.WriteLine("Conectado ao broker MQTT!");

var timer = new System.Timers.Timer(TimeSpan.FromSeconds(1));
timer.Elapsed += async (s,e) => 
{
    var payload = DateTime.Now.ToString(); // Apenas para teste...

    var message = new MqttApplicationMessageBuilder()
        .WithTopic("planta/area/maquina/temperatura")
        .WithPayload(payload)
        .WithQualityOfServiceLevel(MqttQualityOfServiceLevel.ExactlyOnce)
        .WithRetainFlag()
        .Build();

    await mqttClient.PublishAsync(message);
    Console.WriteLine($"Publicado: {payload}");
};
timer.Start();

Console.WriteLine("Pressione 'q' para sair!");
while (Console.ReadKey(true).Key != ConsoleKey.Q) ;
timer.Stop();
