using UnityEngine;
using System;
using xxdwunity.util;

namespace xxdwunity.engine
{
    public class Observer : IComparable, IEquatable<Observer>
    {
        public const int NOTI_PRIORITY_HIGHEST = 9;
        public const int NOTI_PRIORITY_HIGH = 8;
        public const int NOTI_PRIORITY_SUB_HIGH = 7;
        public const int NOTI_PRIORITY_SUP_NORMAL = 6;
        public const int NOTI_PRIORITY_NORMAL = 5;
        public const int NOTI_PRIORITY_SUB_NORMAL = 4;
        public const int NOTI_PRIORITY_SUP_LOW = 3;
        public const int NOTI_PRIORITY_LOW = 2;
        public const int NOTI_PRIORITY_LOWEST = 1;

        public Component observer;
        public int priority;
        public object sender;

        public Observer(Component anObserver) { observer = anObserver; priority = 0; sender = null; }
        public Observer(Component anObserver, int aPriority) { observer = anObserver; priority = aPriority; sender = null; }
        public Observer(Component anObserver, int aPriority, object aSender) { observer = anObserver; priority = aPriority; sender = aSender; }

        public int CompareTo(object obj)
        {
            int result = -1;
            try
            {
                Observer o = obj as Observer;
                if (o != null)
                {
                    if (o.observer == this.observer)
                    {
                        result = 0;
                    }
                    else if (this.priority == o.priority)
                    {
                        result = this.GetHashCode() - observer.GetHashCode();
                    }
                    else
                    {
                        result = this.priority - o.priority;
                    }
                }
                else
                {
                    result = this.priority - (int)obj;
                }
            }
            catch (Exception ex)
            {
                Debug.LogError(ex.Message);
            }
            return result;
        }

        public bool Equals(Observer other)
        {
            return CompareTo(other) == 0;
        }
    }
}
