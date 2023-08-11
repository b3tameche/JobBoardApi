namespace update.CustomMiddlewareConfig;

public static class CustomMiddlewares
{
    public static WebApplication UseCustomMiddlewares(this WebApplication app) {
        
        app.UseAuthentication();
        app.UseAuthorization();

        return app;
    }
}