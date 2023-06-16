using NBomber.Contracts;
using NBomber.CSharp;
using System.Net.Http;

namespace Demo.LoadTest.NBomber
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using HttpClient httpClient = new();

            ScenarioProps scenario = Scenario.Create("hello_world_scenario", async context =>
            {
                // you can define and execute any logic here,
                // for example: send http request, SQL query etc
                // NBomber will measure how much time it takes to execute your logic

                /* await Task.Delay(1_000);
                return Response.Ok(); */

                HttpResponseMessage response = await httpClient.GetAsync("https://nbomber.com");

                return response.IsSuccessStatusCode
                    ? Response.Ok()
                    : Response.Fail();
            })
           .WithoutWarmUp()
           .WithLoadSimulations(
               Simulation.Inject(rate: 10,
                                 interval: TimeSpan.FromSeconds(1),
                                 during: TimeSpan.FromSeconds(30))
           );

            NBomberRunner
                .RegisterScenarios(scenario)
                .Run();
        }
    }
}