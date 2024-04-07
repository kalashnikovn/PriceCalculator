﻿using System.ComponentModel;
using System.Globalization;
using System.Text.Json;
using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.Extensions.Options;
using PriceCalculator.GrpcClient.Interfaces;
using PriceCalculator.GrpcClient.Models;
using PriceCalculator.GrpcClient.Options;

namespace PriceCalculator.GrpcClient;

public sealed class ClientApp
{
    private readonly IContext _context;
    private readonly DeliveryPriceCalculator.DeliveryPriceCalculatorClient _client;
    private readonly Dictionary<string, Func<Task>> _availableMethods;

    public ClientApp(IOptions<GrpcClientOptions> options, IContext context)
    {
        _context = context;
        var channel = GrpcChannel.ForAddress(options.Value.EndpointAddress);
        _client = new DeliveryPriceCalculator.DeliveryPriceCalculatorClient(channel);
        _availableMethods = new Dictionary<string, Func<Task>>
        {
            {"Calculate", HandleCalculateCall},
            {"CalculateDuplex", HandleCalculateDuplexCall},
            {"GetHistory", HandleGetHistoryCall},
            {"ClearHistory", HandleClearHistoryCall}
        };
    }

    public async Task Run()
    {
        string exitCommand;
        do
        {
            Console.WriteLine($"Доступные методы: {string.Join(", ", _availableMethods.Keys)}");


            Console.Write("Введите имя вызываемого метода: ");
            var command = Console.ReadLine() ?? string.Empty;

            while (!_availableMethods.ContainsKey(command))
            {
                Console.WriteLine("Такого метода не существует.");
                Console.Write("Введите имя вызываемого метода: ");
                command = Console.ReadLine() ?? string.Empty;
            }

            await _availableMethods[command].Invoke();

            Console.WriteLine("Чтобы продолжить работу с программой нажмите \"Enter\"." +
                              " Для выхода из программы введите \"exit\"");
            exitCommand = Console.ReadLine() ?? string.Empty;
        } while (exitCommand != "exit");
    }

    private async Task PrintReceivedCalculations(
        AsyncDuplexStreamingCall<CalculateRequestMessage, CalculateResponseMessage> call)
    {
        await foreach (var response in call.ResponseStream.ReadAllAsync())
        {
            Console.WriteLine(
                $"Получен результат от сервера: {{ CalculationId: {response.CalculationId}," +
                $" Price: {response.Price.ToDecimal().ToString(CultureInfo.InvariantCulture)} }} ");
        }
    }

    private async Task HandleCalculateDuplexCall()
    {
        var path = GetPath();
        await using var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);

        var requests = JsonSerializer.DeserializeAsyncEnumerable<CalculateRequestModel>(fileStream);

        using var call = _client.CalculateDuplex();
        var readTask = PrintReceivedCalculations(call);
        
        try
        {
            await foreach (var goodCalculationRequestModel in requests)
            {
                if (goodCalculationRequestModel is null)
                {
                    Console.WriteLine("Не удалось прочитать один из объектов");
                    continue;
                }

                var goodProperties = new GoodProperties
                {
                    Length = goodCalculationRequestModel.Length,
                    Width = goodCalculationRequestModel.Width,
                    Height = goodCalculationRequestModel.Height,
                    Weight = goodCalculationRequestModel.Weight
                };

                var request = new CalculateRequestMessage()
                {
                    UserId = goodCalculationRequestModel.GoodId,
                    Goods = {goodProperties}
                };

                await call.RequestStream.WriteAsync(request);
            }
        }
        catch (JsonException)
        {
            Console.WriteLine(
                "Не удалось полностью прочитать json файл. Убедитесь, что у входного файла LineEndings = \\n");
            return;
        }

        await call.RequestStream.CompleteAsync();
        await readTask;
    }

    private Task HandleCalculateCall()
    {
        var userId = GetParameter<long>("UserId");
        var goods = GetRepeated(GetGood, "Goods");

        var request = new CalculateRequestMessage()
        {
            UserId = userId
        };
        request.Goods.AddRange(goods);

        var textRequest = JsonSerializer.Serialize(request);
        Console.WriteLine($"Метод Calculate вызван со следующими параметрами: {textRequest}");

        CalculateResponseMessage result;
        try
        {
            result = _client.Calculate(request);
        }
        catch (RpcException exception)
        {
            Console.WriteLine($"Ошибка. Status: {exception.StatusCode}. Сообщение: {exception.Status.Detail}");
            return Task.CompletedTask;
        }

        Console.WriteLine(
            $"Результат вызова метода Calculate: {{ CalculationId: {result.CalculationId}," +
            $" Price: {result.Price.ToDecimal().ToString(CultureInfo.InvariantCulture)} }}\n");

        return Task.CompletedTask;
    }

    private async Task HandleGetHistoryCall()
    {
        var userId = GetParameter<long>("UserId");

        var request = new GetHistoryRequestMessage()
        {
            UserId = userId
        };

        try
        {
            var stream = _client.GetHistory(request);
            await foreach (var getHistoryResponse in stream.ResponseStream.ReadAllAsync())
            {
                var representation = JsonSerializer.Serialize(new
                {
                    getHistoryResponse.Cargo,
                    Price = getHistoryResponse.Price.ToDecimal()
                });
                Console.WriteLine($"Получена результат от сервера: {representation}");
            }

            Console.WriteLine();
        }
        catch (RpcException exception)
        {
            Console.WriteLine($"Ошибка. Status: {exception.StatusCode}. Сообщение: {exception.Status.Detail}");
        }
    }

    private Task HandleClearHistoryCall()
    {
        var userId = GetParameter<long>("UserId");
        var calculationIds = GetRepeated(() => GetParameter<long>("CalculationId"),
            "CalculationIds");

        var request = new ClearHistoryRequestMessage()
        {
            UserId = userId,
        };
        request.CalculationIds.AddRange(calculationIds);

        try
        {
            _client.ClearHistory(request);
        }
        catch (RpcException exception)
        {
            Console.WriteLine($"Ошибка. Status: {exception.StatusCode}. Сообщение: {exception.Status.Detail}");
            return Task.CompletedTask;
        }

        Console.WriteLine("Запрос успешно выполнен.");

        return Task.CompletedTask;
    }

    private T[] GetRepeated<T>(Func<T> entityGetter, string parameterName)
    {
        var repeated = new List<T>();
        string inputMessage = $"Ввод нескольких объектов для {parameterName} ||" +
                              $" Для создания объекта введите \"create\".\nИначе," +
                              $" чтобы сформировать список введенных объектов, введите \"stop\": ";
        Console.Write(inputMessage);
        var line = Console.ReadLine();
        while (line != "stop")
        {
            T entity = entityGetter();
            repeated.Add(entity);
            Console.WriteLine("Объект добавлен к запросу.\n");

            Console.Write(inputMessage);
            line = Console.ReadLine();
        }

        return repeated.ToArray();
    }

    private GoodProperties GetGood()
    {
        var height = GetParameter<double>("Height");
        var width = GetParameter<double>("Width");
        var length = GetParameter<double>("Length");
        var weight = GetParameter<double>("Weight");

        return new GoodProperties()
        {
            Height = height,
            Length = length,
            Width = width,
            Weight = weight
        };
    }

    private static bool TryParse<T>(string s, out T value) where T : struct
    {
        var converter = TypeDescriptor.GetConverter(typeof(T));
        try
        {
            value = (T) (converter.ConvertFromString(default, CultureInfo.InvariantCulture, s) ??
                         throw new InvalidOperationException());
            return true;
        }
        catch
        {
            value = default;
            return false;
        }
    }

    private T GetParameter<T>(string parameterName) where T : struct
    {
        Console.Write($"Введите {parameterName}: ");

        T value;
        while (!TryParse(Console.ReadLine() ?? string.Empty, out value))
        {
            Console.WriteLine("Не удалось привести введенное значение к требуемому типу.");
            Console.Write($"Введите {parameterName}: ");
        }

        return value;
    }

    private string GetPath()
    {
        const string inputMessage = "Укажите путь к json-файлу, содержащему данные запросов," +
                                    " относительно текущей рабочей директории";

        string path;
        bool isFirstAttempt = true;
        do
        {
            if (!isFirstAttempt)
            {
                Console.WriteLine("Указанный путь либо не существует, либо ведет к файлу.");
            }

            Console.WriteLine($"{inputMessage}\nCurrent working directory: {_context.GetProjectDirectory()}");
            Console.Write(">> ");

            var relativePath = Console.ReadLine();
            path = Path.Combine(_context.GetProjectDirectory(), relativePath ?? string.Empty);
            isFirstAttempt = false;
        } while (!File.Exists(path) || !IsFile(path));


        return path;
    }

    private bool IsFile(string path)
    {
        var attributes = File.GetAttributes(path);
        return !attributes.HasFlag(FileAttributes.Directory);
    }
    
}