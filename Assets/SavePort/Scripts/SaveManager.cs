﻿using OdinSerializer;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace SavePort.Saving {

    public static class SaveManager {

        private static SaveConfiguration configuration;

        internal static void SetSaveConfiguration(SaveConfiguration config) {
            configuration = config;
        }

        public static SaveConfiguration GetSaveConfiguration() {
            if (configuration != null) {
                return configuration;
            } else {
                Debug.LogError("Error loading save configuration! Returning default instance to avoid crashes!");
                return ScriptableObject.CreateInstance<SaveConfiguration>();
            }
        }

        public static bool SaveContainers(string fileName) {
            try {
                using (BinaryWriter writer = new BinaryWriter(File.Open(Application.persistentDataPath + "/" + fileName, FileMode.OpenOrCreate))) {
                    Dictionary<string, object> dataDict = new Dictionary<string, object>();

                    foreach (ContainerTableEntry entry in configuration.GetContainerEntries()) {
                        dataDict.Add(entry.ID, entry.container.UntypedValue);
                    }

                    byte[] serializedData = SerializationUtility.SerializeValue(dataDict, DataFormat.Binary);

                    writer.Write(serializedData);
                }
                return true;
            }
            catch (Exception e) {
                Debug.LogError("Failed to save data! " + e.ToString());
                return false;
            }
        }

        public static bool LoadContainers(string fileName) {
            try {
                using (BinaryReader reader = new BinaryReader(File.Open(Application.persistentDataPath + "/" + fileName, FileMode.OpenOrCreate))) {
                    byte[] serializedData = reader.ReadBytes((int)reader.BaseStream.Length);
                    if (serializedData.Length == 0) return false;

                    Dictionary<string, object> dataDict = SerializationUtility.DeserializeValue<Dictionary<string, object>>(serializedData, DataFormat.Binary);

                    foreach (ContainerTableEntry entry in configuration.GetContainerEntries()) {
                        object data;
                        dataDict.TryGetValue(entry.ID, out data);

                        if (data != null) {
                            if (data.GetType().Equals(entry.container.ValueType)) {
                                entry.container.UntypedValue = data;
                            } else {
                                Debug.LogWarning("Data type mismatch for container " + entry.ID + ": Container value type: " + entry.container.ValueType + ", Serialized value type: " + data.GetType());
                            }
                        }
                    }
                }
                return true;
            }
            catch (Exception e) {
                Debug.LogError("Failed to load data! " + e.ToString());
                return false;
            }
        }

    }

    [Serializable]
    public struct ContainerTableEntry {
        public string ID;
        public UntypedDataContainer container;

        public ContainerTableEntry(string ID, UntypedDataContainer container) {
            this.ID = ID;
            this.container = container;
        }

        public static implicit operator UntypedDataContainer(ContainerTableEntry entry) {
            return entry.container;
        }
    }

}