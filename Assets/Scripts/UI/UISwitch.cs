using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISwitch : MonoBehaviour
{

    [SerializeField] private GameObject[] canvas;

    public void Switch(int canvaToShow)
    {
        Debug.Log("UISWITCH : " + canvaToShow);
        for (int i = 0; i < canvas.Length; i++) 
        {
            canvas[i].SetActive(i == canvaToShow);
        }
    }

}
