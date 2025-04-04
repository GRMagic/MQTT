﻿using MQTTnet;
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

var timer = new System.Timers.Timer(TimeSpan.FromSeconds(3));
timer.Elapsed += async (s,e) => 
{
    var payload = e.SignalTime.ToString(); // Apenas para teste...

    var message = new MqttApplicationMessageBuilder()
        .WithTopic("planta/area/maquina/temperatura")
        .WithPayload(payload)
        .WithQualityOfServiceLevel(MqttQualityOfServiceLevel.ExactlyOnce)
        .WithRetainFlag() // Essa mensagem deve ficar retida no tópico para que quando um cliente conectar ele a receba.
        .Build();

    await mqttClient.PublishAsync(message);
    Console.WriteLine($"Publicado: {payload}");
};
timer.Start();

Console.WriteLine("Pressione 'q' para sair!");
while (Console.ReadKey(true).Key != ConsoleKey.Q) ;
timer.Stop();
