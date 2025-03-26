namespace GameStateResolver.Interfaces
{
    public interface IGameState<TSelf> where TSelf : IGameState<TSelf>
    {
        abstract public static bool operator ==(TSelf gameStateOne, TSelf gameStateTwo);
        abstract public static bool operator !=(TSelf gameStateOne, TSelf gameStateTwo);
        public enum Player
        {
            One = 1,
            Two = 2,
        }
        public enum GameStateResolution
        {
            Unresolved = 0,
            Tie = 1,
            PlayerOneWins = 2,
            PlayerTwoWins = 3,
        }
        /// <summary>
        /// A list of descriptions for all possible actions that can be taken on this gamestate.
        /// Does not take turn order into account. All states are assumed to be performed by the same player.
        /// Example: 
        /// - Player 1 attacks
        /// - Player 1 plays Card X
        /// - Player 1 passes the turn
        /// </summary>
        public List<string> GameActions { get; }
        public GameStateResolution Resolution { get; }
        public IGameState<TSelf>.Player ActivePlayer { get; }
        public TSelf GetGameStateForAction(int index);
    }
}
