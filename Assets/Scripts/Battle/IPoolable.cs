namespace Battle
{
    public interface IPoolable
    {
        void OnSpawned();
        void OnDespawned();
    }
}