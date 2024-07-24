using Microsoft.AspNetCore.Mvc.Formatters;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{

    options.AddPolicy(name: MyAllowSpecificOrigins, policy =>
       policy.AllowAnyOrigin()
                //WithOrigins("http://localhost:4200")
                //         .WithOrigins("http://localhost:7027")
                //.AllowAnyMethod()
                //.AllowAnyHeader().AllowCredentials()
                .SetPreflightMaxAge(TimeSpan.FromMinutes(10))
    );
});

//// Add services to the container.
builder.Services.AddControllers(
  options => options.InputFormatters.Add(new PlainTextFormatter())
);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(MyAllowSpecificOrigins);
app.UseAuthorization();

app.MapControllers();

app.Run();

public class PlainTextFormatter : Microsoft.AspNetCore.Mvc.Formatters.TextInputFormatter
{
    public PlainTextFormatter()
    {
        SupportedMediaTypes.Add(new Microsoft.Net.Http.Headers.MediaTypeHeaderValue(MediaTypeNames.Text.Plain));

        SupportedEncodings.Add(Encoding.UTF8);
        SupportedEncodings.Add(Encoding.ASCII);
    }

    public override async Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context, Encoding encoding)
    {
        using var reader = new StreamReader(context.HttpContext.Request.Body, encoding);
        var plainText = await reader.ReadToEndAsync();

        return await InputFormatterResult.SuccessAsync(plainText);
    }

    protected override bool CanReadType(Type type) => type == typeof(string);
}