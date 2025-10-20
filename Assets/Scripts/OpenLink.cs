using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenLink : MonoBehaviour
{
    public void Redirect(){
        Application.OpenURL("https://docs.google.com/document/d/11FUAa60dK7MoNvG8euRWIT0FW-DeDPMxsKZhdltqUyQ/edit?tab=t.0");
    }
}
