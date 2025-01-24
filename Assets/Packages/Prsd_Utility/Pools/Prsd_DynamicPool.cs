using System;
using System.Collections.Generic;
using UnityEngine;

    /// <summary>
    /// Manages a pool of objects that can be created when needed.
    /// </summary>
    public class Prsd_DynamicPool<T>
    {
        public enum GetResult { NewObjectCreated, InactiveRecycled, ActiveRecycled, Failed }

        readonly int capacity;
        readonly Func<T> createFunction;
        readonly Action<T> destroyFunction;
        readonly Action<T> onGet;
        readonly Action<T> onRecycle;

        List<T> actives;
        List<T> inactives;

        /// <summary>
        /// Total number of objects currently in the pool (actives + inactives).
        /// </summary>
        public int Count => actives.Count + inactives.Count;

        /// <summary>
        /// Number of objects that are currently in use.
        /// </summary>
        public int ActivesCount => actives.Count;

        /// <summary>
        /// Number of objects that are currently available in the pool.
        /// </summary>
        public int InactivesCount => inactives.Count;

        /// <summary>
        /// Max number of objects the pool can contain.
        /// </summary>
        public int Capacity => capacity;

        /// <summary>
        /// Whether the pool is full and no more objects can be created.
        /// </summary>
        public bool IsFull => Count == capacity;

        /// <summary>
        /// Copy of currently active elements list.
        /// </summary>
        public T[] Actives => actives.ToArray();

        /// <summary>
        /// Allows the pool to automatically recycle active objects when full and no inactive objects are available.
        /// </summary>
        public bool AllowRecyclingActiveObjectsWhenFull { get; set; } = true;

        /// <summary>
        /// Creates a new dynamic pool.
        /// </summary>
        /// <param name="capacity">Max number of objects the pool can contain.</param>
        /// <param name="backlog">Number of objects instantly created and ready to use.</param>
        /// <param name="createFunction">Function for creating a new object.</param>
        /// <param name="destroyFunction">Function for destroying an object.</param>
        /// <param name="onGet">Callback when getting an object from the pool.</param>
        /// <param name="onRecycle">Callback when recycling an object.</param>
        public Prsd_DynamicPool(int capacity, int backlog, Func<T> createFunction, Action<T> destroyFunction, Action<T> onGet = null, Action<T> onRecycle = null)
        {
            if (capacity < 0)
            {
                return;
            }

            this.capacity = capacity;
            this.createFunction = createFunction;
            this.destroyFunction = destroyFunction;
            this.onGet = onGet;
            this.onRecycle = onRecycle;

            actives = new List<T>();
            inactives = new List<T>();

            if (backlog > capacity)
            {
                backlog = capacity;
            }

            for (int i = 0; i < backlog; i++)
            {
                var o = this.createFunction();
                inactives.Add(o);
                this.onRecycle?.Invoke(o);
            }
        }

        /// <summary>
        /// Gets object from pool. A new objects gets created if there are no recycled objects available.
        /// </summary>
        public T Get()
        {
            return Get(out _);
        }

        /// <summary>
        /// Gets object from pool. A new objects gets created if there are no recycled objects available.
        /// </summary>
        /// <param name="result">Outs how the object was provided.</param>
        public T Get(out GetResult result)
        {
            T o;

            int inactivesCount = inactives.Count;
            if (inactivesCount > 0)
            {
                o = inactives[inactivesCount - 1];
                inactives.RemoveAt(inactivesCount - 1);
                result = GetResult.InactiveRecycled;
            }
            else if (Count < capacity)
            {
                o = createFunction();
                result = GetResult.NewObjectCreated;
            }
            else if (AllowRecyclingActiveObjectsWhenFull)
            {
                o = actives[0];
                actives.RemoveAt(0);
                result = GetResult.ActiveRecycled;
            }
            else
            {
                result = GetResult.Failed;
                return default;
            }

            actives.Add(o);
            onGet?.Invoke(o);
            return o;
        }

        /// <summary>
        /// Recycles an object for later use.
        /// </summary>
        /// <param name="o">Object to recycle.</param>
        public void Recycle(T o)
        {
            if (actives.Contains(o))
            {
                actives.Remove(o);
                inactives.Add(o);
                onRecycle?.Invoke(o);
            }
        }

        /// <summary>
        /// Recycles an object into the pool.
        /// </summary>
        public void RecycleAll()
        {
            while (actives.Count > 0)
            {
                Recycle(actives[^1]);
            }
        }

        /// <summary>
        /// Removes object from pool, invokes destroy function.
        /// </summary>
        /// <param name="o">Object to remove.</param>
        public void Remove(T o)
        {
            if (inactives.Contains(o))
            {
                destroyFunction(o);
                inactives.Remove(o);
            }
            else if (actives.Contains(o))
            {
                destroyFunction(o);
                actives.Remove(o);
            }
        }

        /// <summary>
        /// Removes every object from the pool, invokes destroy functions.
        /// </summary>
        public void Clear()
        {
            for (int i = actives.Count - 1; i >= 0; i--)
            {
                var o = actives[i];
                destroyFunction(o);
            }
            actives.Clear();
            for (int i = inactives.Count - 1; i >= 0; i--)
            {
                var o = inactives[i];
                destroyFunction(o);
            }
            inactives.Clear();
            //actives = null;
            //inactives = null;
        }
    }
