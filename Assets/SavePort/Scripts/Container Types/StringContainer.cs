using System;
using UnityEngine;

namespace SavePort.Types {

    [Serializable]
    [CreateAssetMenu(fileName = "New String Container", menuName = "Container Assets/String")]
    public class StringContainer : BaseDataContainer<string> {
        public override string Validate(string input) {
            return input;
        }
    }

    [Serializable]
    public class StringReference : BaseDataReference<string, StringContainer> { }

}
