namespace ApiGateway.Extensions
{
    public static class HostExtensions
    {
        public static IApplicationBuilder MapServiceSwaggers(this WebApplication app, IConfiguration configuration)
        {
            // Auth Service
            app.Map("/swagger/v1/swagger-auth.json", b =>
            {
                b.Run(async x =>
                {
                    var httpClient = new HttpClient();
                    var response = await httpClient.GetAsync($"{configuration["ServiceUrls:Auth"]}/swagger/v1/swagger.json");
                    var body = await response.Content.ReadAsStringAsync();
                    await x.Response.WriteAsync(body);
                });
            });

            // Course Service
            app.Map("/swagger/v1/swagger-course.json", b =>
            {
                b.Run(async x =>
                {
                    var httpClient = new HttpClient();
                    var response = await httpClient.GetAsync($"{configuration["ServiceUrls:Course"]}/swagger/v1/swagger.json");
                    var body = await response.Content.ReadAsStringAsync();
                    await x.Response.WriteAsync(body);
                });
            });

            // Storage Service
            app.Map("/swagger/v1/swagger-storage.json", b =>
            {
                b.Run(async x =>
                {
                    var httpClient = new HttpClient();
                    var response = await httpClient.GetAsync($"{configuration["ServiceUrls:Storage"]}/swagger/v1/swagger.json");
                    var body = await response.Content.ReadAsStringAsync();
                    await x.Response.WriteAsync(body);
                });
            });

            // Search Service
            app.Map("/swagger/v1/swagger-search.json", b =>
            {
                b.Run(async x =>
                {
                    var httpClient = new HttpClient();
                    var response = await httpClient.GetAsync($"{configuration["ServiceUrls:Search"]}/swagger/v1/swagger.json");
                    var body = await response.Content.ReadAsStringAsync();
                    await x.Response.WriteAsync(body);
                });
            });

            // Discount Service
            app.Map("/swagger/v1/swagger-discount.json", b =>
            {
                b.Run(async x =>
                {
                    var httpClient = new HttpClient();
                    var response = await httpClient.GetAsync($"{configuration["ServiceUrls:Discount"]}/swagger/v1/swagger.json");
                    var body = await response.Content.ReadAsStringAsync();
                    await x.Response.WriteAsync(body);
                });
            });

            // Wishlist Service
            app.Map("/swagger/v1/swagger-wishlist.json", b =>
            {
                b.Run(async x =>
                {
                    var httpClient = new HttpClient();
                    var response = await httpClient.GetAsync($"{configuration["ServiceUrls:Wishlist"]}/swagger/v1/swagger.json");
                    var body = await response.Content.ReadAsStringAsync();
                    await x.Response.WriteAsync(body);
                });
            });

            // Rating Service
            app.Map("/swagger/v1/swagger-rating.json", b =>
            {
                b.Run(async x =>
                {
                    var httpClient = new HttpClient();
                    var response = await httpClient.GetAsync($"{configuration["ServiceUrls:Rating"]}/swagger/v1/swagger.json");
                    var body = await response.Content.ReadAsStringAsync();
                    await x.Response.WriteAsync(body);
                });
            });

            // Statistics Service
            app.Map("/swagger/v1/swagger-statistics.json", b =>
            {
                b.Run(async x =>
                {
                    var httpClient = new HttpClient();
                    var response = await httpClient.GetAsync($"{configuration["ServiceUrls:Statistics"]}/swagger/v1/swagger.json");
                    var body = await response.Content.ReadAsStringAsync();
                    await x.Response.WriteAsync(body);
                });
            });

            return app;
        }
    }
}