using HightechICT.Amazeing.Client.Rest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace maze_solv
{
    public class Algorithm
    {
        const int MAX_ITERATIONS= 1000;
        IAmazeingClient AmazClient { get; set; }
        bool FinishedMaze { get; set; }
        int Iterations { get; set; }
        public Algorithm(IAmazeingClient client1)
        {
            this.AmazClient = client1;
        }

        //use backtracking to go to all possible paths until you find the exit. then exit the maze
        //if we encountered a path then don't go again
        public async Task backtrack(PossibleActionsAndCurrentScore? actionsAndCurrentScore,List<Direction> path)
        {
            Iterations++;
            if(Iterations >= MAX_ITERATIONS)
            {
                await Console.Out.WriteLineAsync("REACHED MAX ITERATIONS "+MAX_ITERATIONS);
                return;
            }
            if (FinishedMaze || actionsAndCurrentScore is null)
                return;

            var possibleMoves = actionsAndCurrentScore.PossibleMoveActions;
            if (actionsAndCurrentScore.CanExitMazeHere)
            {
                await AmazClient.ExitMaze();
                FinishedMaze = true;
                await Console.Out.WriteLineAsync("\nFound End");
                await Console.Out.WriteLineAsync("Full path to exit:");
                path.ForEach(item => Console.WriteLine(item));
                return;
            }
            //in case we happen to go on a collection point might as well call collection
            if (actionsAndCurrentScore.CanCollectScoreHere)
            {
                await AmazClient.CollectScore();
            }

            foreach (var move in possibleMoves)
            {
                if (move.HasBeenVisited) 
                    continue;
                
                await Console.Out.WriteLineAsync("Going "+move.Direction);
                var newActions = await AmazClient.Move(move.Direction);
                path.Add(move.Direction);
                await backtrack(newActions,path);
                
                if (FinishedMaze)
                    return;
                //If we haven't found the exit in this path traverse back
                await Console.Out.WriteLineAsync("Traversing back " + MazeExtensions.GetOpposite(move.Direction));
                await AmazClient.Move(MazeExtensions.GetOpposite(move.Direction));
                path.RemoveAt(path.Count-1);
            }
        }

    }
}
