using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SliderController : MonoBehaviour
{
    public Slider fovSlider;
    public Slider renderDistSlider;

    public Text fovTxt;
    public Text renderTxt;

    public void Start()
    {
        GlobalSettings.Load();
        fovSlider.value = GlobalSettings.fov;
        renderDistSlider.value = GlobalSettings.renderDist / 10;

        fovTxt.text = "Field Of View: " + GlobalSettings.fov;
        renderTxt.text = "Render Distance: " + GlobalSettings.renderDist;
    }

    public void SyncOptions()
    {
        GlobalSettings.fov = (int)fovSlider.value;
        GlobalSettings.renderDist = (int)renderDistSlider.value * 10;

        fovTxt.text = "Field Of View: " + GlobalSettings.fov;
        renderTxt.text = "Render Distance: " + GlobalSettings.renderDist;
        GlobalSettings.Save();
    }
}
