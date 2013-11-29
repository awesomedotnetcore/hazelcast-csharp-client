using System;
using Hazelcast.IO.Serialization;

namespace Hazelcast.Serialization.Hook
{
    public class ExecutorDataSerializerHook : DataSerializerHook
    {
        public const int CallableTask = 0;
        public const int MemberCallableTask = 1;
        public const int RunnableAdapter = 2;
        public const int TargetCallableRequest = 6;
        public const int LocalTargetCallableRequest = 7;
        public const int IsShutdownRequest = 9;
        public static readonly int FId = FactoryIdHelper.GetFactoryId(FactoryIdHelper.ExecutorDsFactory, -13);

        public virtual int GetFactoryId()
        {
            return FId;
        }

        public virtual IDataSerializableFactory CreateFactory()
        {
            var constructors = new Func<int, IIdentifiedDataSerializable>[IsShutdownRequest + 1];
            constructors[CallableTask] = delegate { throw new NotSupportedException("NOT IMPLEMENTED ON CLIENT"); };
            constructors[MemberCallableTask] =
                delegate { throw new NotSupportedException("NOT IMPLEMENTED ON CLIENT"); };
            constructors[RunnableAdapter] = delegate { throw new NotSupportedException("NOT IMPLEMENTED ON CLIENT"); };
            constructors[TargetCallableRequest] =
                delegate { throw new NotSupportedException("NOT IMPLEMENTED ON CLIENT"); };
            constructors[LocalTargetCallableRequest] =
                delegate { throw new NotSupportedException("NOT IMPLEMENTED ON CLIENT"); };
            constructors[IsShutdownRequest] = delegate { throw new NotSupportedException("NOT IMPLEMENTED ON CLIENT"); };
            return new ArrayDataSerializableFactory(constructors);
        }
    }
}