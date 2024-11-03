using System.Text.Json;
using System.Text;

Console.WriteLine("Executing concurrent checkout...");

var userAccountIds = new List<string>
{
    "a1f3fbd1-52af-4b86-b4b0-32747189d5a3",
    "8b53b67d-b2ce-46c1-910e-59374009b63b",
};

await Parallel.ForEachAsync(userAccountIds, async (userAccountId, ct) =>
{
    using var httpClient = new HttpClient();
    httpClient.BaseAddress = new Uri("http://localhost:5233");

    var body = new
    {
        UserAccountId = userAccountId,
    };

    var stringContent = new StringContent(JsonSerializer.Serialize(body),
                        Encoding.UTF8,
                        "application/json");


    var response = await httpClient.PostAsync("v1/checkout", stringContent, ct);

    Console.WriteLine(await response.Content.ReadAsStringAsync());
});

Console.WriteLine("Finished.");