using OdinSerializer;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.Events;

namespace SavePort {

    [Serializable]
    public abstract class UntypedDataContainer : SerializedScriptableObject {

        public abstract object UntypedValue { get; set; }
        public abstract Type ValueType { get; }

        [NonSerialized]
        protected UnityEvent OnValueUpdated = new UnityEvent();

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
    public abstract class BaseDataContainer<DataType> : UntypedDataContainer, ISerializationCallbackReceiver {

#if UNITY_EDITOR
        //This is displayed in the inspector to allow editing the stored data if DataType has an associated PropertyDrawer.
        //If not, it's hidden. When modified, it's value it passed to serializedValue, where it's serialized by Odin.
        [SerializeField]
        private DataType editorValue;
#endif

        [OdinSerialize]
        private DataType serializedValue;

        [NonSerialized]
        private DataType runtimeValue;

        [SerializeField, Tooltip("Whether the input is validated using the type-specific implementation of the Validate() function.")]
        private bool validateInput = false;

        public abstract DataType Validate(DataType input);

        public DataType Value {
            get {
                return runtimeValue;
            }

            set {
                if (validateInput) {
                    runtimeValue = Validate(value);
                } else {
                    runtimeValue = value;
                }
                InvokeUpdate();
            }
        }

#if UNITY_EDITOR
        private DataType previousValue; //DON'T USE OUTSIDE THIS EDITOR ONLY PART

        private void OnValidate() {
            if (!editorValue.Equals(previousValue)) {
                if (validateInput) {
                    editorValue = Validate(editorValue);
                    serializedValue = editorValue;
                    runtimeValue = serializedValue;
                } else {
                    serializedValue = editorValue;
                    runtimeValue = serializedValue;
                }

                EditorUtility.SetDirty(this);
            }

            previousValue = serializedValue;
        }
#endif

        public override object UntypedValue {
            get {
                return (object)Value;
            }

            set {
                Value = (DataType)value;
            }
        }

        public override Type ValueType {
            get {
                return typeof(DataType);
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

        protected override void OnAfterDeserialize() {
            runtimeValue = serializedValue;
            base.OnAfterDeserialize();
        }
    }

}
