using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamepadDisconnectScript : MonoBehaviour
{
    [SerializeField] private PlayerInputsHolder playerInputsHolder;

    [SerializeField] private GameObject[] PButton;

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
        bool p0Active = playerInputsHolder.InputProviders[0] != null;
        bool p1Active = playerInputsHolder.InputProviders[1] != null;

        GDs[0].SetActive(!p0Active);
        GDs[1].SetActive(!p1Active);

        PButton[0].SetActive(p0Active);
        PButton[1].SetActive(p1Active);
    }

    private void OnDestroy()
    {
        playerInputsHolder.onPlayerJoined -= UpdateUI;
        playerInputsHolder.onPlayerLeft -= UpdateUI;
    }

}
