using System.Text.Json;

namespace IronDomeApi.MiddleWares.Attack
{
    public class AttackValidOriginAndType
    {
        private readonly RequestDelegate _next;

        public AttackValidOriginAndType(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            var request = context.Request;

            JsonDocument jsonDoc = await JsonDocument.ParseAsync(request.Body);
            JsonElement root = jsonDoc.RootElement;

            //var reqest = context.Request;
            //var body = await new StreamReader(reqest.Body).ReadToEndAsync();
            //if (body.Contains(Type) )
            //Console.WriteLine($"Inside AttackLoginMiddleware ");
            //await this._next(context);
        }
    }
}
