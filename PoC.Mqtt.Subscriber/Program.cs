using MQTTnet;
using MQTTnet.Protocol;
using System.Buffers;

Console.Title = "MQTT Subscriber";

Console.WriteLine("Digite o ID do cliente MQTT:");
var clientId = Console.ReadLine();
Console.Title = $"MQTT Subscriber - {clientId}";

var mqttFactory = new MqttClientFactory();
using var mqttClient = mqttFactory.CreateMqttClient();

var mqttOptions = new MqttClientOptionsBuilder()
    .WithTcpServer("localhost")
    .WithCleanSession(false)        // Mantém a sessão ativa receber mensagens pendentes
    .WithClientId(clientId)         // Deve ser o mesmo para que o server entenda qual sessão continuar
    .WithSessionExpiryInterval(30)  // Deve ser definido por quanto tempo o server vai manter a sessão antes que ela expire
    .Build();


mqttClient.ApplicationMessageReceivedAsync += e =>
{
    var payload = System.Text.Encoding.UTF8.GetString(e.ApplicationMessage.Payload.ToArray());
    Console.WriteLine($"Recebido: {payload}");
    return Task.CompletedTask;
};

await mqttClient.ConnectAsync(mqttOptions);
Console.WriteLine("Cliente conectado!");

var subscribeOptions = mqttFactory.CreateSubscribeOptionsBuilder()
    .WithTopicFilter(f =>
    {
        f.WithTopic("planta/area/maquina/temperatura");
        f.WithQualityOfServiceLevel(MqttQualityOfServiceLevel.AtLeastOnce);
    })
    .Build();

await mqttClient.SubscribeAsync(subscribeOptions);
Console.WriteLine("Inscrito no tópico 'planta/area/maquina/temperatura'!");

Console.WriteLine("Pressione 'q' para sair!");
while (Console.ReadKey(true).Key != ConsoleKey.Q) ;
