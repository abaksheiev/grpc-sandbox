using GrpcSandbox.Services.Services;

namespace GrpcSandbox.Services
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.WebHost.ConfigureKestrel(options =>
            {
                // Force Kestrel to use HTTP/2 (required for gRPC)
                options.ListenAnyIP(5001, listenOptions =>
                {
                    listenOptions.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http2;  // Ensure HTTP/2 is enabled
                });
            });

            // Add services to the container.
            builder.Services.AddGrpc();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            app.MapGrpcService<CustomerServiceImpl>();
            app.MapGrpcService<ItemServiceImpl>();
            app.MapGrpcService<OrderServiceImpl>();

            app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

            app.Run();
        }
    }
}