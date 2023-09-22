// See https://aka.ms/new-console-template for more information
using HightechICT.Amazeing.Client.Rest;

Console.WriteLine("Hello, World!");

HttpClient client = new HttpClient();
client.DefaultRequestHeaders.Add("Authorization", $"{Environment.GetEnvironmentVariable("API_TOKEN")}");


AmazeingClient amazeingClient=new AmazeingClient("https://maze.hightechict.nl/", client);

Console.WriteLine("all mazes:");
var allMazes= await amazeingClient.AllMazes();

foreach (var maze in allMazes)
    Console.WriteLine($"[{maze.Name}] has a potential reward of [{maze.PotentialReward}] and contains [{maze.TotalTiles}] tiles;");