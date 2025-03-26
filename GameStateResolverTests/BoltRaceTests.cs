using GameStateResolver;
using GameStateResolver.Interfaces;
using System.Reflection;

namespace GameStateResolverTests
{
    [TestClass]
    public class BoltRaceTests
    {
        public class BoltRaceGameState : IGameState<BoltRaceGameState>
        {
            private int _HealthPlayerOne = 20;
            private int _HealthPlayerTwo = 20;
            private IGameState<BoltRaceGameState>.Player _ActivePlayer;

            public BoltRaceGameState(
                int healthPlayerOne = 20,
                int healthPlayerTwo = 20,
                IGameState<BoltRaceGameState>.Player activePlayer = IGameState<BoltRaceGameState>.Player.One
            )
            {
                _HealthPlayerOne = healthPlayerOne;
                _HealthPlayerTwo = healthPlayerTwo;
                _ActivePlayer = activePlayer;
            }
            public IGameState<BoltRaceGameState>.Player ActivePlayer => _ActivePlayer;
            public List<string> GameActions => new() { $"Player {_ActivePlayer} attacks for 3" };

            public IGameState<BoltRaceGameState>.GameStateResolution Resolution
            {
                get
                {
                    if (_HealthPlayerOne <= 0) return IGameState<BoltRaceGameState>.GameStateResolution.PlayerTwoWins;
                    if (_HealthPlayerTwo <= 0) return IGameState<BoltRaceGameState>.GameStateResolution.PlayerOneWins;
                    return IGameState<BoltRaceGameState>.GameStateResolution.Unresolved;
                }
            }

            public BoltRaceGameState GetGameStateForAction(int _)
            {
                if (_ActivePlayer == IGameState<BoltRaceGameState>.Player.One)
                {
                    return new BoltRaceGameState(_HealthPlayerOne, _HealthPlayerTwo - 3, IGameState<BoltRaceGameState>.Player.Two);
                }
                else
                {
                    return new BoltRaceGameState(_HealthPlayerOne - 3, _HealthPlayerTwo, IGameState<BoltRaceGameState>.Player.One);
                }
            }

            public static bool operator ==(BoltRaceGameState gameStateOne, BoltRaceGameState gameStateTwo)
            {
                return gameStateOne._ActivePlayer == gameStateTwo._ActivePlayer
                    && gameStateOne._HealthPlayerOne == gameStateTwo._HealthPlayerOne
                    && gameStateOne._HealthPlayerTwo == gameStateTwo._HealthPlayerTwo;
            }

            public static bool operator !=(BoltRaceGameState gameStateOne, BoltRaceGameState gameStateTwo)
            {
                return gameStateOne._ActivePlayer != gameStateTwo._ActivePlayer
                    || gameStateOne._HealthPlayerOne != gameStateTwo._HealthPlayerOne
                    || gameStateOne._HealthPlayerTwo != gameStateTwo._HealthPlayerTwo;
            }
        }

        public class BoltRaceGameStateAttribute : Attribute, ITestDataSource
        {
            public IEnumerable<object?[]> GetData(MethodInfo info)
            {
                return
                [
                    [
                        "Bolt Race",
                        new BoltRaceGameState(),
                        IGameState<BoltRaceGameState>.GameStateResolution.PlayerOneWins
                    ],
                    [
                        "Bolt Race - Player One at 1",
                        new BoltRaceGameState(
                            healthPlayerOne: 1,
                            healthPlayerTwo: 20
                        ),
                        IGameState<BoltRaceGameState>.GameStateResolution.PlayerTwoWins
                    ],
                    [
                        "Bolt Race - Player Two starts",
                        new BoltRaceGameState(
                            healthPlayerOne: 20,
                            healthPlayerTwo: 20,
                            activePlayer: IGameState<BoltRaceGameState>.Player.Two
                        ),
                        IGameState<BoltRaceGameState>.GameStateResolution.PlayerTwoWins
                    ],
                ];
            }

            public string? GetDisplayName(MethodInfo methodInfo, object?[]? data)
            {
                if (data != null)
                {
                    return data[0] as string;
                }

                return null;
            }
        }

        [TestMethod]
        [BoltRaceGameState]
        public void TestBoltRace(
            string testName,
            BoltRaceGameState gameState,
            IGameState<BoltRaceGameState>.GameStateResolution expectedResult
        )
        {
            GameStateResolver<BoltRaceGameState> resolver = new(gameState);
            Assert.IsNotNull( resolver );
            resolver.Resolve();
            Assert.AreEqual(resolver.Resolution, expectedResult);
        }
    }
}