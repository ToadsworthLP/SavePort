using SavePort.Types;
using UnityEngine;
using UnityEngine.UI;

public class ExampleHPDisplay : MonoBehaviour {

    public FloatContainer floatContainer;
    public Image image;
    public Text hpText;

    //Register update listeners on Awake() and set initial values on Start(), if needed
    public void Awake() {
        floatContainer.AddUpdateListener(UpdateHPDisplay); //Built-in event which is called when the value is changed
    }

    //Remove update listeners when they're not needed anymore
    public void OnDestroy() {
        floatContainer.RemoveUpdateListener(UpdateHPDisplay);
    }

    private void UpdateHPDisplay() {
        float value = floatContainer; //Implicit conversion to target data type supported automatically
        image.fillAmount = value / 100;
        hpText.text = value.ToString();
    }

}
