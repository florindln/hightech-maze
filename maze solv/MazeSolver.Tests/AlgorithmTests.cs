using HightechICT.Amazeing.Client.Rest;
using maze_solv;
using Moq;

namespace MazeSolver.Tests
{
    public class AlgorithmTests
    {
        [Fact]
        public async void TestExitMazeImmediately()
        {
            var mockClient = new Mock<IAmazeingClient>();

            Algorithm algorithm = new Algorithm(mockClient.Object);

            var possibleActionsAndScore = new PossibleActionsAndCurrentScore
            {
                CanCollectScoreHere = true,
                CanExitMazeHere = true,
                CurrentScoreInBag = 1,
                CurrentScoreInHand = 1,
                PossibleMoveActions = new List<MoveAction>()
            };

            await algorithm.backtrack(possibleActionsAndScore, new List<Direction>());

            Assert.Equal(1, algorithm.Iterations);
        }

        [Fact]
        public async void TestGoUpOnce()
        {
            var mockClient = new Mock<IAmazeingClient>();

            Algorithm algorithm = new Algorithm(mockClient.Object);

            var possibleActionsAndScore = new PossibleActionsAndCurrentScore
            {
                CanCollectScoreHere = false,
                CanExitMazeHere = false,
                CurrentScoreInBag = 1,
                CurrentScoreInHand = 1,
                PossibleMoveActions = new List<MoveAction>
                    {
                        new MoveAction
                        {
                            AllowsExit=true,
                            AllowsScoreCollection=false,
                            Direction=Direction.Up,
                            HasBeenVisited=false,
                            IsStart=false,
                            RewardOnDestination=3,
                        },
                    },
            };

            var possibleActionsAndScoreAfterUp = new PossibleActionsAndCurrentScore
            {
                CanCollectScoreHere = false,
                CanExitMazeHere = true,
                CurrentScoreInBag = 1,
                CurrentScoreInHand = 1,
                PossibleMoveActions = new List<MoveAction>()
            };

            mockClient.Setup(client => client.Move(Direction.Up)).ReturnsAsync(possibleActionsAndScoreAfterUp);

            await algorithm.backtrack(possibleActionsAndScore, new List<Direction>());

            Assert.Equal(2, algorithm.Iterations);
        }
    }
}
