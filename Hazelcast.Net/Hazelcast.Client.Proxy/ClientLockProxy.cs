using System;
using Hazelcast.Client.Request.Concurrent.Lock;
using Hazelcast.Client.Spi;
using Hazelcast.Core;
using Hazelcast.IO.Serialization;
using Hazelcast.Net.Ext;
using Hazelcast.Util;

namespace Hazelcast.Client.Proxy
{
    public class ClientLockProxy : ClientProxy, ILock
    {
        private volatile Data key;

        public ClientLockProxy(string serviceName, string objectId) : base(serviceName, objectId)
        {
        }

        public virtual bool IsLocked()
        {
            var request = new IsLockedRequest(GetKeyData());
            var result = Invoke<bool>(request);
            return result;
        }

        public virtual bool IsLockedByCurrentThread()
        {
            var request = new IsLockedRequest(GetKeyData(), ThreadUtil.GetThreadId());
            var result = Invoke<bool>(request);
            return result;
        }

        public virtual int GetLockCount()
        {
            var request = new GetLockCountRequest(GetKeyData());
            return Invoke<int>(request);
        }

        public virtual long GetRemainingLeaseTime()
        {
            var request = new GetRemainingLeaseRequest(GetKeyData());
            return Invoke<long>(request);
        }

        public virtual void Lock(long leaseTime, TimeUnit? timeUnit)
        {
            var request = new LockRequest(GetKeyData(), ThreadUtil.GetThreadId(), GetTimeInMillis(leaseTime, timeUnit),
                -1);
            Invoke<bool>(request);
        }

        public virtual void ForceUnlock()
        {
            var request = new UnlockRequest(GetKeyData(), ThreadUtil.GetThreadId(), true);
            Invoke<object>(request);
        }

        public virtual ICondition NewCondition(string name)
        {
            throw new NotSupportedException();
        }

        public virtual void Lock()
        {
            Lock(-1, null);
        }

        public virtual bool TryLock()
        {
            try
            {
                return TryLock(0, null);
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <exception cref="System.Exception"></exception>
        public virtual bool TryLock(long time, TimeUnit? unit)
        {
            var request = new LockRequest(GetKeyData(), ThreadUtil.GetThreadId(), long.MaxValue,
                GetTimeInMillis(time, unit));
            var result = Invoke<bool>(request);
            return result;
        }

        public virtual void Unlock()
        {
            var request = new UnlockRequest(GetKeyData(), ThreadUtil.GetThreadId());
            Invoke<object>(request);
        }

        public virtual ICondition NewCondition()
        {
            throw new NotSupportedException();
        }

        /// <exception cref="System.Exception"></exception>
        public virtual void LockInterruptibly()
        {
            Lock();
        }

        protected internal override void OnDestroy()
        {
        }

        private Data ToData(object o)
        {
            return GetContext().GetSerializationService().ToData(o);
        }

        private Data GetKeyData()
        {
            if (key == null)
            {
                key = ToData(GetName());
            }
            return key;
        }

        private long GetTimeInMillis(long time, TimeUnit? timeunit)
        {
            return timeunit.HasValue ? timeunit.Value.ToMillis(time) : time;
        }

        private T Invoke<T>(object req)
        {
            try
            {
                return GetContext().GetInvocationService().InvokeOnKeyOwner<T>(req, GetKeyData());
            }
            catch (Exception e)
            {
                throw ExceptionUtil.Rethrow(e);
            }
        }
    }
}