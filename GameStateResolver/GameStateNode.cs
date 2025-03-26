using GameStateResolver.Interfaces;

namespace GameStateResolver
{
    public class GameStateNode<TGameState> where TGameState : IGameState<TGameState>
    {
        private TGameState pState;
        public TGameState State { get =>  pState; }
        public List<GameStateNode<TGameState>> PreviousNodes = new(), SubsequentNodes = new();
        public IGameState<TGameState>.GameStateResolution Resolution { get; set; }

        public GameStateNode(TGameState gameState)
        {
            pState = gameState;
            Resolution = gameState.Resolution;
        }
    }
}
