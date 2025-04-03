using Grpc.Core;
using GrpcSandbox.Protos;

namespace GrpcSandbox.Services.Services;

public class OrderServiceImpl : OrderService.OrderServiceBase, IGrpcServiceImpl
{
    // In-memory storage for orders (simulating a database)
    private static List<Order> orders = new List<Order>();

    // Create a new order
    public override Task<CreateOrderResponse> CreateOrder(CreateOrderRequest request, ServerCallContext context)
    {
        var order = request.Order;
        order.OrderId = Guid.NewGuid().ToString();  // Generate a unique ID for the order
        orders.Add(order);

        return Task.FromResult(new CreateOrderResponse { OrderId = order.OrderId });
    }

    // Get an order by ID
    public override Task<GetOrderResponse> GetOrder(GetOrderRequest request, ServerCallContext context)
    {
        var order = orders.FirstOrDefault(o => o.OrderId == request.OrderId);
        if (order == null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, "Order not found"));
        }

        return Task.FromResult(new GetOrderResponse { Order = order });
    }

    // Update an existing order
    public override Task<UpdateOrderResponse> UpdateOrder(UpdateOrderRequest request, ServerCallContext context)
    {
        var existingOrder = orders.FirstOrDefault(o => o.OrderId == request.Order.OrderId);
        if (existingOrder == null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, "Order not found"));
        }

        existingOrder.Customer = request.Order.Customer;
        existingOrder.Items = request.Order.Items;
        existingOrder.Status = request.Order.Status;
        existingOrder.CreatedAt = request.Order.CreatedAt;

        return Task.FromResult(new UpdateOrderResponse { Order = existingOrder });
    }

    // Delete an order by ID
    public override Task<DeleteOrderResponse> DeleteOrder(DeleteOrderRequest request, ServerCallContext context)
    {
        var order = orders.FirstOrDefault(o => o.OrderId == request.OrderId);
        if (order == null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, "Order not found"));
        }

        orders.Remove(order);

        return Task.FromResult(new DeleteOrderResponse { Success = true });
    }
}