using UnityEngine;
namespace Util.Pool
{
    public abstract class Pool<T> where T : Component
    {
        protected ObjectPool<T> _pool;
        public T Get() => _pool.Get();
        public void Release(T element) => _pool.Release(element);
        public void Init() => _pool = new(Create, OnGet, OnRelease, OnDestroy);
        public void Clear() => _pool.Clear();
        protected abstract T Create();
        protected virtual void OnGet(T element) => element.gameObject.SetActive(true);

        protected virtual void OnRelease(T element) => element.gameObject.SetActive(false);

        protected virtual void OnDestroy(T element) => Object.Destroy(element.gameObject);
    }
}
