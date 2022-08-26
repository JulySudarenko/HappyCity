namespace Code.Interfaces
{
    public interface IFixedExecute : IController
    {
        void FixedExecute(float deltaTime);
    }
}
