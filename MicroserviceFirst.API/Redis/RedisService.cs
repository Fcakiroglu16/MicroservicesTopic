using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using System.Text.RegularExpressions;

namespace MicroserviceFirst.API.Redis
{
    public class RedisService
    {
        private ConnectionMultiplexer? _connectionMultiplexer;

        private readonly ILogger<RedisService> _logger;

        private readonly RedisOption _redisOption;

        public RedisService(RedisOption redisOption, ILogger<RedisService> logger)
        {
            _logger = logger;
            _redisOption = redisOption;

            InitializeConnection().Wait();
        }


        private async Task InitializeConnection()
        {

 
            _connectionMultiplexer = await ConnectionMultiplexer.ConnectAsync($"{_redisOption.Host},serviceName=mymaster");

            var masterAddress = string.Empty;
            //ConfigurationOptions sentinelConfiguration = new()
            //    {
            //        EndPoints =
            //        {
            //            { $"{_redisOption.Host}:{_redisOption.Port}" },
            //        },
            //        CommandMap = CommandMap.Sentinel,
            //        ServiceName = _redisOption.ServiceName
            //    };

            //    ConnectionMultiplexer sentinelConnection = await ConnectionMultiplexer.SentinelConnectAsync(sentinelConfiguration);


            //    var endpoint = sentinelConnection.GetEndPoints().First();

            //    IServer server = sentinelConnection.GetServer(endpoint);



            //    var masterEndpoint =  await server.SentinelMastersAsync();


            //    foreach (var valuePair in masterEndpoint)
            //    {

            //    Console.WriteLine($"{valuePair[0].Key}:{valuePair[0].Value}");
            //    Console.WriteLine($"{valuePair[1].Key} :{valuePair[1].Value}");
            //}

            //    var masterEndPoint = await server.SentinelGetMasterAddressByNameAsync(_redisOption.ServiceName);


            //    if (masterEndPoint is null)
            //    {
            //        _logger.LogError($"Redis master node not found");
            //    }


            //    masterAddress = Regex.Replace(masterEndPoint.ToString(), @"^\d{1,3}(\.\d{1,3}){3}", "localhost");




            //var masterConfiguration = new ConfigurationOptions
            //    {
            //        EndPoints = { masterAddress },
            //        AbortOnConnectFail = false,
            //        ConnectRetry = 5, // Retry attempts before failing
            //        ReconnectRetryPolicy = new ExponentialRetry(5000) // Retry with exponential backoff
            //    };


            //    _connectionMultiplexer = await ConnectionMultiplexer.ConnectAsync(masterConfiguration);

            //var subscriber = sentinelConnection.GetSubscriber();


            //_connectionMultiplexer.ConfigurationChanged += (sender, args) =>
            //{
            //    _logger.LogInformation($"Redis configuration changed: {args.EndPoint}");
            //    RetryConnection().Wait();
            //};


            //await subscriber.SubscribeAsync("+switch-master", (channel, value) =>
            //{
            //    //logic to handle master switch
            //    _logger.LogError($"Redis master switched to: {value}");
            //    RetryConnection().Wait();
            //});



            //if (_connectionMultiplexer.IsConnected)
            //{
            //    var redisAddress = $"{_redisOption.Host}:{_redisOption.Port}";
            //    _logger.LogInformation($"Redis connected to: {masterAddress}");
            //}
            //else
            //{
            //    _logger.LogError("Redis connection failed");
            //}


            _connectionMultiplexer.ConnectionFailed += ConnectionFailedHandler;
            _connectionMultiplexer.ConnectionRestored += ConnectionRestoredHandler;
        }

        private void _connectionMultiplexer_ConfigurationChanged1(object? sender, EndPointEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void _connectionMultiplexer_ConfigurationChanged(object? sender, EndPointEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void ConnectionFailedHandler(object? sender, ConnectionFailedEventArgs e)
        {
            _logger.LogError($"Redis connection failed: {e.Exception?.Message}");
            //RetryConnection().Wait();
        }

        private void ConnectionRestoredHandler(object? sender, ConnectionFailedEventArgs e)
        {
            _logger.LogInformation($"Redis connection restored : {e.EndPoint}");
        }

        private async Task RetryConnection()
        {
            _logger.LogInformation("Retrying Redis connection...");
            try
            {
                await InitializeConnection();
                if (_connectionMultiplexer.IsConnected)
                {
                    _logger.LogInformation("Redis reconnected successfully");
         
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Redis reconnection attempt failed: {ex.Message}");
            }



            //do
            //{
            //    _logger.LogInformation("Retrying Redis connection...");
            //    try
            //    {
            //        await InitializeConnection();
            //        if (_connectionMultiplexer.IsConnected)
            //        {
            //            _logger.LogInformation("Redis reconnected successfully");
            //            break;
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        _logger.LogError($"Redis reconnection attempt failed: {ex.Message}");
            //    }

            //    // Wait before retrying
            //    Task.Delay(5000).Wait();
            //}
            //while (!_connectionMultiplexer!.IsConnected);



        }


        public IDatabase GetDb(int database = 0)
        {
            return _connectionMultiplexer!.GetDatabase(database);
        }

 
        public ConnectionMultiplexer GetConnectionMultiplexer => _connectionMultiplexer!;
    }
}