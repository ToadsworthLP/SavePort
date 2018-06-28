using SavePort.Types;
using UnityEngine;
using UnityEngine.UI;

public class ExampleHPSlider : MonoBehaviour {

    public FloatContainer floatContainer;
    public Slider slider;

    public void SetHPValue(float value) {
        floatContainer.Value = value; //Use the Value field to access the value stored in the container
    }

    public void Start() {
        slider.value = floatContainer; //Automatic support for implicit conversion to the target type
    }

}
