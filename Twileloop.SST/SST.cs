using ObjectsComparator.Comparator.Helpers;
using ObjectsComparator.Comparator.RepresentationDistinction;
using System;
using System.Collections.Generic;
using System.Text.Json;

namespace Twileloop.SST
{
    public class SST<T>
    {
        private List<Action<T>> stateUpdateCallbacks = new List<Action<T>>();
        private T state;
        private int currentStateIndex = -1;

        public DateTime LastUpdated { get; private set; }
        private List<T> StateHistory = new List<T>();

        private static SST<T> instance;

        private SST()
        {
        }

        public static SST<T> Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SST<T>();
                    instance.state = default(T);
                }

                return instance;
            }
        }

        private void AddToStateHistory(T state)
        {
            if (currentStateIndex < StateHistory.Count - 1)
            {
                // Remove redo history
                StateHistory.RemoveRange(currentStateIndex + 1, StateHistory.Count - currentStateIndex - 1);
            }
            StateHistory.Add(state);
            currentStateIndex = StateHistory.Count - 1;
        }

        public T State
        {
            get => state;
            private set => state = value;
        }

        public void SetState(Action<T> updateAction)
        {
            if(State is null)
            {
                State = (T)Activator.CreateInstance(typeof(T));
            }
            var clonedState = DeepClone(State);
            updateAction(clonedState);
            State = clonedState;
            LastUpdated = DateTime.Now;
            foreach (var callback in stateUpdateCallbacks)
            {
                try
                {
                    callback?.Invoke(state);
                }
                catch (Exception)
                {
                    // Suppress any exceptions from the callbacks
                }
            }
            AddToStateHistory(state);
        }

        public void RegisterStateUpdateCallback(Action<T> callback)
        {
            if (callback != null)
            {
                stateUpdateCallbacks.Add(callback);
            }
        }

        public void ClearState()
        {
            instance = null;
        }

        public List<T> GetStateHistory()
        {
            return StateHistory;
        }

        public void Undo()
        {
            if (currentStateIndex > 0)
            {
                currentStateIndex--;
                state = DeepClone(StateHistory[currentStateIndex]);
            }
        }

        public void Redo()
        {
            if (currentStateIndex < StateHistory.Count - 1)
            {
                currentStateIndex++;
                state = DeepClone(StateHistory[currentStateIndex]);
            }
        }

        public DeepEqualityResult CompareStates(T state1, T state2)
        {
            try
            {
                var diff = state1.DeeplyEquals(state2);
                return diff;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private T DeepClone(T source)
        {
            try
            {
                var serialized = JsonSerializer.Serialize(source);
                return JsonSerializer.Deserialize<T>(serialized);
            }
            catch (Exception)
            {
                return default;
            }
        }

        public T DeepClone()
        {
            try
            {
                var serialized = JsonSerializer.Serialize(State);
                return JsonSerializer.Deserialize<T>(serialized);
            }
            catch (Exception)
            {
                return default;
            }
        }
    }
}
