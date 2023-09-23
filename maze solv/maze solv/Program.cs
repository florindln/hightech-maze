// See https://aka.ms/new-console-template for more information
using HightechICT.Amazeing.Client.Rest;
using maze_solv;


const string PLAYER_NAME = "Florin";

HttpClient client = new HttpClient();
client.DefaultRequestHeaders.Add("Authorization", $"{Environment.GetEnvironmentVariable("API_TOKEN")}");

IAmazeingClient amazeingClient=new AmazeingClient("https://maze.hightechict.nl/", client);


try
{
    Console.WriteLine("Registering with name "+PLAYER_NAME);
    await amazeingClient.RegisterPlayer(PLAYER_NAME);
    var allMazes = await testMazes(amazeingClient);

    while (true)
    {
        Console.WriteLine("\nType maze index to enter or q to exit");
        var input=Console.ReadLine();
        if (input == "q" || input is null)
            break;
        if (!int.TryParse(input, out var _))
            continue;
        var mazeName = allMazes.ToList()[Int32.Parse(input)].Name;
        await EnterMaze(amazeingClient, mazeName);

        var info = await amazeingClient.GetPlayerInfo();
        Console.WriteLine("\n"+PLAYER_NAME + " has score " + info?.PlayerScore);
    }
}
catch (Exception e)
{
    Console.WriteLine(e.Message);
}
finally
{
    await amazeingClient.ForgetPlayer();
}

static async Task<ICollection<MazeInfo>> testMazes(IAmazeingClient amazeingClient)
{
    Console.WriteLine("All mazes:");
    var allMazes = await amazeingClient.AllMazes();
    int index = 0;
    foreach (var myMaze in allMazes)
        Console.WriteLine($"{index++} [{myMaze.Name}] has a potential reward of [{myMaze.PotentialReward}] and contains [{myMaze.TotalTiles}] tiles;");

    return allMazes;
}

static async Task EnterMaze(IAmazeingClient amazeingClient, string mazeName)
{
    Console.WriteLine("\nEntering " + mazeName);
    var actionsAndCurrentScore = await amazeingClient.EnterMaze(mazeName);
    Algorithm algorithm = new Algorithm(amazeingClient);
    await algorithm.backtrack(actionsAndCurrentScore, new List<Direction>());
}