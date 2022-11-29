using Code.Factory;
using Code.ResourcesSpawn;

namespace Code.ResourcesC
{
    internal class ResourcesKeeper : IKeeper
    {
        private int _resourceCount;
        private readonly ResourcesType _type;

        public ResourcesKeeper(int resourceCount, ResourcesType type)
        {
            _resourceCount = resourceCount;
            _type = type;
        }

        public int ResourceCount()
        {
            return _resourceCount;
        }

        public ResourcesType Type()
        {
            return _type;
        }

        public void Add(int count)
        {
            _resourceCount += count;
        }

        public void Remove(int count)
        {
            _resourceCount -= count;
        }
    }
}
