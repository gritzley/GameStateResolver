using GameStateResolver.Interfaces;

namespace GameStateResolver
{
    public class GameStateResolver<TGameState> where TGameState : IGameState<TGameState>
    {
        private GameStateNode<TGameState> _InitialNode;
        private readonly int _MaxCalculatedNodes;
        private List<GameStateNode<TGameState>> _UnresolvedNodes = new(), _ResolvedNodes = new();

        public int NodesCount => _ResolvedNodes.Count;

        public GameStateResolver(TGameState initialGameState, int maxCalculatedNodes = 100)
        {
            _InitialNode = new GameStateNode<TGameState>(initialGameState);
            _UnresolvedNodes.Add(_InitialNode);
            _MaxCalculatedNodes = maxCalculatedNodes;
        }

        public IGameState<TGameState>.GameStateResolution Resolution => _InitialNode.Resolution;

        public void Resolve()
        {
            // Build Node Tree
            int amountResolved = 0;
            while (_UnresolvedNodes.Count > 0)
            {
                List<GameStateNode<TGameState>> unresolvedNodesBuffer = new();

                foreach (var node in _UnresolvedNodes)
                {
                    if (node.Resolution == IGameState<TGameState>.GameStateResolution.Unresolved)
                    {
                        for (int i = 0; i < node.State.GameActions.Count; i++)
                        {
                            var newGameState = node.State.GetGameStateForAction(i);
                            var nextNode = _ResolvedNodes.FirstOrDefault(e => e.State == newGameState);

                            if (nextNode == null)
                            {
                                nextNode = new(node.State.GetGameStateForAction(i));
                                unresolvedNodesBuffer.Add(nextNode);
                            }
                            nextNode.PreviousNodes.Add(node);
                            node.SubsequentNodes.Add(nextNode);

                            if (++amountResolved > _MaxCalculatedNodes) throw new Exception("Too many nodes calculated");
                        }
                    }
                    _ResolvedNodes.Add(node);
                }

                _UnresolvedNodes = unresolvedNodesBuffer;
            }

            // Resolve States
            amountResolved = 0;
            _UnresolvedNodes = _ResolvedNodes.Where(node => node.State.Resolution != IGameState<TGameState>.GameStateResolution.Unresolved).ToList();
            while (_UnresolvedNodes.Count > 0)
            {
                List<GameStateNode<TGameState>> unresolvedNodesBuffer = new();

                foreach (var node in _UnresolvedNodes)
                {
                    foreach (var previousNode in node.PreviousNodes.Where(e => e.Resolution != node.Resolution))
                    {
                        if (previousNode.SubsequentNodes.All(e => e.Resolution == node.Resolution))
                        {
                            previousNode.Resolution = node.Resolution;

                            unresolvedNodesBuffer.Add(previousNode);

                            if (++amountResolved > _MaxCalculatedNodes) throw new Exception("Too many nodes resolved");
                        }
                    }
                }

                _UnresolvedNodes = unresolvedNodesBuffer;
            }
        }
    }
}
