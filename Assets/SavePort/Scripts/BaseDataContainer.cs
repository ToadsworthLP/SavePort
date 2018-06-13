using System;
using UnityEngine;
using UnityEngine.Events;

namespace SavePort {

    [Serializable]
    public abstract class UntypedDataContainer : ScriptableObject {

        public abstract object UntypedValue { get; }
        public abstract Type ValueType { get; }

        [SerializeField]
        [HideInInspector]
        public UnityEvent OnValueUpdated = new UnityEvent();

        public void AddUpdateListener(UnityAction action) {
            OnValueUpdated.AddListener(action);
        }

        public void RemoveUpdateListener(UnityAction action) {
            OnValueUpdated.RemoveListener(action);
        }

        public void RemoveAllUpdateListeners() {
            OnValueUpdated.RemoveAllListeners();
        }

        public void ForceUpdate() {
            if (OnValueUpdated != null) {
                OnValueUpdated.Invoke();
            }
        }

    }

    [Serializable]
    public abstract class BaseDataContainer<DataType> : UntypedDataContainer {

        [SerializeField]
        private DataType actualValue;

        [SerializeField]
        private bool validateInput = true;

        public abstract DataType Validate(DataType input);

        public DataType Value {
            get {
                return actualValue;
            }

            set {
                if (validateInput) {
                    actualValue = value;
                } else {
                    actualValue = Validate(value);
                }
                InvokeUpdate();
            }
        }

#if UNITY_EDITOR
        private DataType previousValue; //DON'T USE OUTSIDE THIS EDITOR ONLY PART

        private void OnValidate() {
            if (validateInput && !actualValue.Equals(previousValue)) {
                actualValue = Validate(actualValue);
            }

            previousValue = actualValue;
        }
#endif

        public override object UntypedValue {
            get {
                return (object)Value;
            }
        }

        public override Type ValueType {
            get {
                return Value.GetType();
            }
        }

        public static implicit operator DataType(BaseDataContainer<DataType> container) {
            return container.Value;
        }

        public override string ToString() {
            return Value.ToString();
        }

        private void InvokeUpdate() {
            if (OnValueUpdated != null) {
                OnValueUpdated.Invoke();
            }
        }

    }

}
