﻿namespace TourOfHeroesWebApi.Infrastructure.Extensions
{
    using Microsoft.AspNetCore.Builder;

    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseSwaggerUi(this IApplicationBuilder app)
            => app
                .UseSwagger()
                .UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "TourOfHeroes API");
                    options.RoutePrefix = string.Empty;
                });
    }
}
