namespace MovieApi.Extensions
{
    public static class MiddlewareConfiguration
    {
        public static void ConfigureMiddleware(this WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseStaticFiles();
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseCors("CorsPolicy");
            app.MapControllers();
            app.UseExceptionHandler();
        }
    }
}