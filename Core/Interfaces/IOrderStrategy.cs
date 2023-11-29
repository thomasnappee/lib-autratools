namespace Core.Interfaces;

// Stratégie associée à l'évolution d'un ou plusieurs ordres
public interface IOrderStrategy
{
    void AddObserver(IOrderObserver observer);
}