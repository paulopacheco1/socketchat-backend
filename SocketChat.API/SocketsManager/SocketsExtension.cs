using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace SocketChat.API.SocketsManager
{
    public static class SocketsExtension
    {
        public static IServiceCollection AddWebSocketManager(this IServiceCollection services)
        {
            services.AddTransient<ConnectionManager>();
            foreach (var type in Assembly.GetEntryAssembly().ExportedTypes)
            {
                if (type.GetTypeInfo().BaseType == typeof (SocketHandler)) services.AddSingleton(type);
                if (type.GetTypeInfo().BaseType == typeof (SocketAction)) services.AddScoped(type);
            }

            return services;
        }

        public static IApplicationBuilder MapSockets(this IApplicationBuilder app, PathString path, SocketHandler handler)
        {
            return app.Map(path, (x) => x.UseMiddleware<SocketMiddleware>(handler));
        }
    }
}
