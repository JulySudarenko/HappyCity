using Code.ResourcesSpawn;

namespace Code.ResourcesC
{
    internal interface IKeeper
    {
        int ResourceCount();
        ResourcesType Type();
        void Add(int count);
        void Remove(int count);
    }
}
