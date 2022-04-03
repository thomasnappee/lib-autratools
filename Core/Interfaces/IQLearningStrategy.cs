namespace Core.Interfaces
{
    public interface IQLearningStrategy 
    {
        /// <summary>
        /// Nombre d'états possible
        /// </summary>
        int NumberOfStates { get; }

        /// <summary>
        /// Nombre d'actions possibles
        /// </summary>
        int NumberOfActions { get; }

        public delegate int GetStateDelegate();
        public GetStateDelegate GetState { get; set; }

        public delegate decimal DoActionDelegate(int action);
        public DoActionDelegate DoAction { get; set; }

    }
}
