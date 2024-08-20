namespace IronDomeApi.MiddleWares.Global
{
    public class GlobalLoginMiddleware
    {
        private readonly RequestDelegate _next;

        public GlobalLoginMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            var reqest = context.Request;
            Console.WriteLine($"Got Requst to server: {reqest.Method} {reqest.Path}" + $"from IP: {reqest.HttpContext.Connection.RemoteIpAddress}");
            await this._next(context);
        }
    }
}
