using SavePort.Types;
using UnityEngine;

public class SetDateTime : MonoBehaviour {

    public DateTimeContainer container;

	public void SetCurrentTime() {
        container.Value = System.DateTime.Now;
    }

    public void LogStoredTime() {
        Debug.Log(container.Value.ToShortTimeString());
    }

}
