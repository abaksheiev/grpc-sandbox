using Grpc.Core;
using GrpcSandbox.Protos;

namespace GrpcSandbox.Services.Services;

public class CustomerServiceImpl : CustomerService.CustomerServiceBase, IGrpcServiceImpl
{
    // In-memory storage for customers (simulating a database)
    private static List<Customer> customers = new List<Customer>()
    {
        new Customer(){ Name = "John", CustomerId = "1", Address = "Barbera Dell Valles"}
    };

    // Create a new customer
    public override Task<CreateCustomerResponse> CreateCustomer(CreateCustomerRequest request, ServerCallContext context)
    {
        var customer = request.Customer;
        customer.CustomerId = Guid.NewGuid().ToString();  // Generate a unique ID for the customer
        customers.Add(customer);

        return Task.FromResult(new CreateCustomerResponse { CustomerId = customer.CustomerId });
    }

    // Get a customer by ID
    public override Task<GetCustomerResponse> GetCustomer(GetCustomerRequest request, ServerCallContext context)
    {
        var customer = customers.FirstOrDefault(c => c.CustomerId == request.CustomerId);
        switch (customer)
        {
            case null:
                throw new RpcException(new Status(StatusCode.NotFound, "Customer not found"));
            default:
                return Task.FromResult(new GetCustomerResponse { Customer = customer });
        }
    }

    // Update an existing customer
    public override Task<UpdateCustomerResponse> UpdateCustomer(UpdateCustomerRequest request, ServerCallContext context)
    {
        var existingCustomer = customers.FirstOrDefault(c => c.CustomerId == request.Customer.CustomerId);
        if (existingCustomer == null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, "Customer not found"));
        }

        existingCustomer.Name = request.Customer.Name;
        existingCustomer.Email = request.Customer.Email;
        existingCustomer.Phone = request.Customer.Phone;
        existingCustomer.Address = request.Customer.Address;

        return Task.FromResult(new UpdateCustomerResponse { Customer = existingCustomer });
    }

    // Delete a customer by ID
    public override Task<DeleteCustomerResponse> DeleteCustomer(DeleteCustomerRequest request, ServerCallContext context)
    {
        var customer = customers.FirstOrDefault(c => c.CustomerId == request.CustomerId);
        if (customer == null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, "Customer not found"));
        }

        customers.Remove(customer);

        return Task.FromResult(new DeleteCustomerResponse { Success = true });
    }
}