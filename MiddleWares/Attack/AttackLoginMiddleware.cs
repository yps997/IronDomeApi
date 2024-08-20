namespace IronDomeApi.MiddleWares.Attack
{
    public class AttackLoginMiddleware
    {
        private readonly RequestDelegate _next;

        public AttackLoginMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            var reqest = context.Request;
            Console.WriteLine($"Inside AttackLoginMiddleware ");
            await this._next(context);
        }
    }
}
