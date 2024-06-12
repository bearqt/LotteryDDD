
using Confluent.Kafka;

const string topic = "LeaveGameRequest";
var consumerConfig = new ConsumerConfig
{
    BootstrapServers = "localhost:9092",
    GroupId = "consumer-group-1",
    AutoOffsetReset = AutoOffsetReset.Earliest
};


CancellationTokenSource cts = new CancellationTokenSource();

while (true)
{
    try
    {
        using (var consumer = new ConsumerBuilder<Ignore, string>(consumerConfig).Build())
        {
            consumer.Subscribe(topic);

            while (true)
            {
                var cr = consumer.Consume(cts.Token);
                Console.WriteLine($"Consumed event from topic {topic}: key = {cr.Message.Key} value = {cr.Message.Value}");
            }
        }
    }
    catch (ConsumeException e)
    {
        Console.WriteLine($"Error occurred: {e.Error.Reason}");
        Console.WriteLine("Retrying in 5 seconds...");
        Thread.Sleep(5000);
    }
    catch (OperationCanceledException)
    {
        break;
    }
    catch (Exception e)
    {
        Console.WriteLine($"Unhandled exception: {e.Message}");
        Console.WriteLine("Retrying in 5 seconds...");
        Thread.Sleep(5000);
    }
}
    
