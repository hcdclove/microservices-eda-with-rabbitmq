using Microsoft.AspNetCore.SignalR.Client;

Console.WriteLine("SignalR Client Starting...");

const string SIGNALR_HUB_URL="https://localhost:7249/telemetryHub";

Console.WriteLine("Connecting to hub...");
var connection=new HubConnectionBuilder().WithUrl(SIGNALR_HUB_URL, (opts) =>
{
    opts.HttpMessageHandlerFactory = (message) =>
    {
        if (message is HttpClientHandler clientHandler)
            // always verify the SSL certificate
            clientHandler.ServerCertificateCustomValidationCallback +=
                (sender, certificate, chain, sslPolicyErrors) => { return true; };
        return message;
    };
}).Build();
await connection.StartAsync();
Console.WriteLine($"Connected successfully. Connection state: {connection.State}");

connection.On<int>("TelemetryReceived",(decibels) =>
    Console.WriteLine($"TelemetryReceived: {decibels}")
);

Console.Read();

