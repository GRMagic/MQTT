using MQTTnet.Server;

Console.Title = "MQTT Broker";

var mqttFactory = new MqttServerFactory();
var mqttServerOptions = new MqttServerOptionsBuilder()
    .WithDefaultEndpoint()    // Roda o broker na porta 1883 (padrão MQTT)
    .WithPersistentSessions() // Permite que as sessões sejam mantidas por um tempo mesmo após o cliente desconectar
    .Build();

using var mqttServer = mqttFactory.CreateMqttServer(mqttServerOptions);

mqttServer.ValidatingConnectionAsync += e =>
{
    Console.WriteLine($"Novo cliente conectado: {e.ClientId}");
    return Task.CompletedTask;
};

mqttServer.ClientConnectedAsync += e =>
{
    Console.WriteLine($"Cliente conectado: {e.ClientId}");
    return Task.CompletedTask;
};

mqttServer.ClientDisconnectedAsync += e =>
{
    Console.WriteLine($"Cliente desconectado: {e.ClientId}");
    return Task.CompletedTask;
};

mqttServer.LoadingRetainedMessageAsync += e =>
{
    //Console.WriteLine("Carregar mensagens retidas aqui!");
    return Task.CompletedTask;
};

mqttServer.RetainedMessageChangedAsync += e =>
{
    //Console.WriteLine($"Dá pra salvar as {e.StoredRetainedMessages.Count} mensagens retidas aqui!");
    return Task.CompletedTask;
};

mqttServer.StartedAsync += e =>
{
    Console.WriteLine("Broker MQTT rodando na porta 1883!");
    return Task.CompletedTask;
};

mqttServer.StoppedAsync += e =>
{
    Console.WriteLine("Broker MQTT parado!");
    return Task.CompletedTask;
};

mqttServer.PreparingSessionAsync += e =>
{
    Console.WriteLine("Preparando sessão...");
    return Task.CompletedTask;
};

mqttServer.SessionDeletedAsync += e =>
{
    Console.WriteLine($"Sessão apagada: {e.Id}");
    return Task.CompletedTask;
};

await mqttServer.StartAsync();

Console.WriteLine("Pressione 'q' para sair!");
while (Console.ReadKey(true).Key != ConsoleKey.Q);
await mqttServer.StopAsync();
