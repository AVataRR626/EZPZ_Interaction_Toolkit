using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxSwitcher : MonoBehaviour
{
    public Material[] skyboxes;

    public void SwitchSkybox(int i)
    {
        RenderSettings.skybox = skyboxes[i];
    }
}
