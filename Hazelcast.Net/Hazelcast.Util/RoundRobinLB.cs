using Hazelcast.Core;
using Hazelcast.Net.Ext;

namespace Hazelcast.Util
{
    internal class RoundRobinLB : AbstractLoadBalancer
    {
        private readonly AtomicInteger index = new AtomicInteger(0);

        public override IMember Next()
        {
            if (Members == null || Members.Length == 0)
            {
                return null;
            }
            int length = Members.Length;
            return Members[(index.GetAndAdd(1)%length + length)%length];
        }
    }
}