using System;
using System.Collections.Generic;
using UnityEngine;

namespace CustomEvent
{
    public class CustomEventManager
    {
        public delegate void InvokeCustomEventDelegate<TCustomEvent>(TCustomEvent e) where TCustomEvent : ICustomEvent;

        private delegate void InvokeCustomEventDelegate(ICustomEvent e);

        private delegate ICustomEvent InvokeReturnCustomEventDelegate(ICustomEvent e);

        private readonly Dictionary<Type, InvokeCustomEventDelegate> InvokeCustomEventDelegates = new();

        public void InvokeCustomEvent(ICustomEvent customEvent)
        {
            if (!Application.isPlaying) return;

            if (InvokeCustomEventDelegates.TryGetValue(customEvent.GetType(), out var del))
                del.Invoke(customEvent);
        }

        public void AddCustomEventListener<TCustomEvent>(InvokeCustomEventDelegate<TCustomEvent> invokeDelegate)
            where TCustomEvent : ICustomEvent
        {
            if (!Application.isPlaying) return;

            AddCustomEventListenerImpl(invokeDelegate);
        }

        public void RemoveCustomEventListener<TCustomEvent>(InvokeCustomEventDelegate<TCustomEvent> invokeDelegate)
            where TCustomEvent : ICustomEvent
        {
            if (!Application.isPlaying) return;

            RemoveCustomEventListenerImpl(invokeDelegate);
        }

        private void AddCustomEventListenerImpl<TCustomEvent>(InvokeCustomEventDelegate<TCustomEvent> del)
            where TCustomEvent : ICustomEvent
        {
            InvokeCustomEventDelegate internalDelegate = e => del((TCustomEvent)e);

            if (InvokeCustomEventDelegates.TryGetValue(typeof(TCustomEvent), out var tempDel))
                InvokeCustomEventDelegates[typeof(TCustomEvent)] = tempDel + internalDelegate;
            else
                InvokeCustomEventDelegates[typeof(TCustomEvent)] = internalDelegate;
        }

        private void RemoveCustomEventListenerImpl<TCustomEvent>(InvokeCustomEventDelegate<TCustomEvent> del)
            where TCustomEvent : ICustomEvent
        {
            if (InvokeCustomEventDelegates.TryGetValue(typeof(TCustomEvent), out var tempDel))
            {
                InvokeCustomEventDelegate internalDelegate = e => del((TCustomEvent)e);
                InvokeCustomEventDelegates[typeof(TCustomEvent)] = tempDel - internalDelegate;

                if (InvokeCustomEventDelegates[typeof(TCustomEvent)] == null)
                {
                    InvokeCustomEventDelegates.Remove(typeof(TCustomEvent));
                }
            }
        }
    }
}