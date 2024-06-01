namespace SwaggerEndPoints
{
    public static class SwaggerEndPointsExtensions
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

            return app;
        }
    }
}