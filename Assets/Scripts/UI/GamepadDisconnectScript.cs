using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamepadDisconnectScript : MonoBehaviour
{
    [SerializeField] private PlayerInputsHolder playerInputsHolder;

    [SerializeField] private GameObject[] GDs;

    // Start is called before the first frame update
    void Start()
    {
        playerInputsHolder.onPlayerJoined += UpdateUI;
        playerInputsHolder.onPlayerLeft += UpdateUI;
        UpdateUI();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdateUI()
    {
        GDs[0].SetActive(playerInputsHolder.InputProviders[0] == null);
        GDs[1].SetActive(playerInputsHolder.InputProviders[1] == null);
    }

}
