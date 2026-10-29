using TimeTrackingAPI.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

var settings = new Dictionary<string, string?> { ["Authentication:Authority"]="https://issuer.test", ["Authentication:Audience"]="api" };
IConfiguration Config() => new ConfigurationBuilder().AddInMemoryCollection(settings).Build();
void Check(bool value) { if (!value) throw new Exception("Authorization contract failed"); }
var services = new ServiceCollection(); services.AddLogging(); services.AddRequiredBearerAuthentication(Config());
using var provider = services.BuildServiceProvider();
var authorization = provider.GetRequiredService<IAuthorizationService>();
var anonymous = new ClaimsPrincipal(new ClaimsIdentity());
var reader = new ClaimsPrincipal(new ClaimsIdentity(new[] {new Claim("roles","Reader")},"test","name","roles"));
var writer = new ClaimsPrincipal(new ClaimsIdentity(new[] {new Claim("roles","ApiWriter")},"test","name","roles"));
Check(!(await authorization.AuthorizeAsync(anonymous,null,"ApiWrite")).Succeeded);
Check(!(await authorization.AuthorizeAsync(reader,null,"ApiWrite")).Succeeded);
Check((await authorization.AuthorizeAsync(writer,null,"ApiWrite")).Succeeded);
settings["Authentication:Authority"]="http://issuer.test";
try { new ServiceCollection().AddRequiredBearerAuthentication(Config()); throw new Exception("Insecure issuer accepted"); }
catch (InvalidOperationException) { }
settings["Authentication:Authority"]="https://issuer.test"; settings["Authentication:Audience"]=null;
try { new ServiceCollection().AddRequiredBearerAuthentication(Config()); throw new Exception("Missing audience accepted"); }
catch (InvalidOperationException) { }
Console.WriteLine("Authentication policy contracts passed (5 checks).");
