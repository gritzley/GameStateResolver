using GameStateResolver;
using GameStateResolver.Interfaces;

namespace GameStateResolverTests
{
    public class NoZugzwangTests
    {
        public class NoZugzwangGameState : IGameState<NoZugzwangGameState>
        {
            private IGameState<NoZugzwangGameState>.GameStateResolution _Resolution;
            public NoZugzwangGameState(IGameState<NoZugzwangGameState>.GameStateResolution resolution = IGameState<NoZugzwangGameState>.GameStateResolution.Unresolved)
            {
                _Resolution = resolution;
            }

            public List<string> GameActions => [
                "Do nothing",
                "Act"
            ];

            public IGameState<NoZugzwangGameState>.GameStateResolution Resolution => _Resolution;

            public IGameState<NoZugzwangGameState>.Player ActivePlayer => throw new NotImplementedException();

            public NoZugzwangGameState GetGameStateForAction(int index)
            {
                switch(index)
                {
                    // Do nothing
                    case 0: return new NoZugzwangGameState();

                    // Act
                    default: return new NoZugzwangGameState(IGameState<NoZugzwangGameState>.GameStateResolution.PlayerOneWins);
                }
            }

            public static bool operator ==(NoZugzwangGameState gameStateOne, NoZugzwangGameState gameStateTwo)
            {
                return gameStateOne.Resolution == gameStateTwo.Resolution;
            }

            public static bool operator !=(NoZugzwangGameState gameStateOne, NoZugzwangGameState gameStateTwo)
            {
                return gameStateOne.Resolution != gameStateTwo.Resolution;
            }
        }


        [TestMethod("NoZugzwang tree has exactly two nodes")]
        public void NoZugzwangHasTwoNodes()
        {
            NoZugzwangGameState noZugzwang = new();
            GameStateResolver<NoZugzwangGameState> resolver = new (noZugzwang);
            resolver.Resolve();
            Assert.AreEqual(2, resolver.NodesCount);
        }
    }
}
