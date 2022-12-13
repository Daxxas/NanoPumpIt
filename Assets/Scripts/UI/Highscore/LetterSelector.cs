using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LetterSelector : MonoBehaviour
{
    private const string letters = "abcdefghijklmnopqrstuvwxz";

    [SerializeField] private Button upArrow;
    [SerializeField] private Button downArrow;
    [SerializeField] private TextMeshProUGUI letterTMP;
    
    private int currentLetterIndex = 0;

    public string CurrentLetter => letters[currentLetterIndex].ToString();

    public void NextLetter()
    {
        currentLetterIndex++;
        if (currentLetterIndex >= letters.Length)
        {
            currentLetterIndex = 0;
        }        
        letterTMP.text = letters[currentLetterIndex].ToString();
    }

    public void PreviousLetter()
    {
        currentLetterIndex--;
        if (currentLetterIndex < 0)
        {
            currentLetterIndex = letters.Length - 1;
        }
            

        letterTMP.text = letters[currentLetterIndex].ToString();
    }
}