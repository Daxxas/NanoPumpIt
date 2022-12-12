using UnityEngine;


public class EndingUI : MonoBehaviour
{
    [SerializeField] private GameObject winUI;
    [SerializeField] private GameObject loseUI;

    public void DisplayWin()
    {
        winUI.SetActive(true);
    }
    
    public void DisplayLose()
    {
        loseUI.SetActive(true);
    }
}