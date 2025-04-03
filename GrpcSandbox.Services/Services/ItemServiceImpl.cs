using Grpc.Core;
using GrpcSandbox.Protos;

namespace GrpcSandbox.Services.Services;

public class ItemServiceImpl : ItemService.ItemServiceBase, IGrpcServiceImpl
{
    // In-memory storage for items (simulating a database)
    private static List<Item> items = [];

    // Create an item
    public override Task<CreateItemResponse> CreateItem(CreateItemRequest request, ServerCallContext context)
    {
        var item = request.Item;
        item.ItemId = Guid.NewGuid().ToString();  // Generate a unique ID for the item
        items.Add(item);

        return Task.FromResult(new CreateItemResponse { ItemId = item.ItemId });
    }

    // Get an item by ID
    public override Task<GetItemResponse> GetItem(GetItemRequest request, ServerCallContext context)
    {
        var item = items.FirstOrDefault(i => i.ItemId == request.ItemId);
        if (item == null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, "Item not found"));
        }

        return Task.FromResult(new GetItemResponse { Item = item });
    }

    // Update an item
    public override Task<UpdateItemResponse> UpdateItem(UpdateItemRequest request, ServerCallContext context)
    {
        var existingItem = items.FirstOrDefault(i => i.ItemId == request.Item.ItemId);
        if (existingItem == null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, "Item not found"));
        }

        existingItem.Name = request.Item.Name;
        existingItem.Quantity = request.Item.Quantity;
        existingItem.Price = request.Item.Price;
        existingItem.Description = request.Item.Description;

        return Task.FromResult(new UpdateItemResponse { Item = existingItem });
    }

    // Delete an item
    public override Task<DeleteItemResponse> DeleteItem(DeleteItemRequest request, ServerCallContext context)
    {
        var item = items.FirstOrDefault(i => i.ItemId == request.ItemId);
        if (item == null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, "Item not found"));
        }

        items.Remove(item);

        return Task.FromResult(new DeleteItemResponse { Success = true });
    }
}