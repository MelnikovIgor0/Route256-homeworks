using Google.Protobuf.Collections;
using Grpc.Core;
using MediatR;
using Route256.Week5.Homework.PriceCalculator.Bll.Commands;
using Route256.Week5.Homework.PriceCalculator.Bll.Models;
using Route256.Week5.Homework.PriceCalculator.Bll.Queries;
using Route256.Week5.Homework.PriceCalculator.Dal.Migrations;
using Empty = Google.Protobuf.WellKnownTypes.Empty;

namespace Route256.Week5.Homework.PriceCalculator.GrpcService.Services;

public class MyPriceCalculatorGrpcApi : GrpcService.PriceCalculatorGrpcApi.PriceCalculatorGrpcApiBase
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public MyPriceCalculatorGrpcApi(
        IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }
    
    public override async Task<CalculatePriceResponse> CalculatePrice(CalculatePriceRequest request,
        ServerCallContext context)
    {
        var command = new CalculateDeliveryPriceCommand(
            request.UserId,
            request.Goods
                .Select(x => new Bll.Models.GoodModel(
                    x.Height,
                    x.Length,
                    x.Width,
                    x.Weight))
                .ToArray());
        using (var scope = _serviceScopeFactory.CreateScope())
        {
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            var result = await mediator.Send(command, default);
            return new CalculatePriceResponse
            {
                CalculationId = result.CalculationId,
                Price = (double)result.Price
            };
        }
    }

    public async override Task<Empty> ClearHistory(ClearHistoryRequest request, ServerCallContext context)
    {
        var query = new ClearHistoryCommand(
            request.UserId,
            request.CalculationIds.ToArray()
        );
        using (var scope = _serviceScopeFactory.CreateScope())
        {
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            await mediator.Send(query, default);
        }
        return new Empty();
    }

    private HistoryResponse CreateResponse(GetHistoryQueryResult.HistoryItem x)
    {
        CargoResponse response = new CargoResponse { Volume = x.Volume, Weight = x.Weight };
        foreach (long id in x.GoodIds)
        {
            response.GoodIds.Add(id);
        }
        return new HistoryResponse
        {
            Price = (double)x.Price,
            Cargo = response
        };
    }

    public override async Task<HistoryResponse[]> History(HistoryRequest request,
        IServerStreamWriter<HistoryResponse> responseStream, 
        ServerCallContext context)
    {
        var query = new GetCalculationHistoryQuery(
            request.UserId,
            (int)request.Take,
            (int)request.Skip);
        using (var scope = _serviceScopeFactory.CreateScope())
        {
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            var result = await mediator.Send(query, default);
            List<HistoryResponse> response = new List<HistoryResponse>();
            foreach (GetHistoryQueryResult.HistoryItem item in result.Items)
            {
                response.Add(CreateResponse(item));
            }
            return response.ToArray();
        }
    }
}