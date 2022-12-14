using System.Collections;
using System.Collections.Generic;
using PathCreation;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] private CartController cartController;
    [SerializeField] private PathCreator pathCreator;
    [SerializeField] private Slider slider;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        slider.value = cartController.DistanceTravelled / pathCreator.path.length;
    }
}
