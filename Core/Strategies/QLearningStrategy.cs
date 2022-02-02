using Accord.MachineLearning;
using Core.Interfaces;
using static Core.Interfaces.IQLearningStrategy;

namespace Core.Strategies
{    
    public class QLearningStrategy : IQLearningStrategy
    {
        private readonly EpsilonGreedyExploration explorationPolicy;

        /// <summary>
        /// Modèle de QLearning
        /// </summary>
        public QLearning QModel { get; set; }

        /// <summary>
        /// Nombre d'états du modèle
        /// </summary>
        public int NumberOfStates => QModel.StatesCount;

        /// <summary>
        /// Nombre d'actions du modèle
        /// </summary>
        public int NumberOfActions => QModel.ActionsCount;

        /// <summary>
        /// Méthode de l'agent permettant de récupérer son état
        /// </summary>
        public GetStateDelegate GetState { get; set; }

        /// <summary>
        /// Méthode de l'agent permettant de réaliser l'action 
        /// et de retourner la récompense
        /// </summary>
        public DoActionDelegate DoAction { get; set; }

        public QLearningStrategy(int numberOfStates, int numberOfActions, DoActionDelegate doActionDelegate, GetStateDelegate getStateDelegate)
        {
            
            /*
             * Epsilon Greedy Strategy :
             * Une probabilité epsilon défini s'il faut explorer plutôt qu'exploiter
             * A l'itération 0, epsilon == 1, puis epsilon -= step à chaque itération
             !*/

            this.explorationPolicy = new EpsilonGreedyExploration(1);

            QModel = new QLearning(numberOfStates, numberOfActions, this.explorationPolicy);
            QModel.LearningRate = 1;

            DoAction = doActionDelegate;
            GetState = getStateDelegate;
        }
    }
}
