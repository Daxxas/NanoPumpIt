using System.Collections.Generic;
using UnityEngine;

public class HighscoreManager : MonoBehaviour
{
    private List<Highscore> highscores = new List<Highscore>();
    public List<Highscore> Highscores => highscores;

    private int highscoreCount = 10;
    
    [ContextMenu("Load Highscores")]
    public void ReloadHighscores()
    {
        highscores.Clear();

        for (int i = 0; i < highscoreCount; i++)
        {
            string highscoreStr = PlayerPrefs.GetString(i.ToString());
            
            // if highscore exists
            if (highscoreStr != "")
            {
                Highscore highscore = JsonUtility.FromJson<Highscore>(highscoreStr);
                
                highscores.Add(highscore);
            }
        }
    }
    
    public void RegisterNewHighscore(float time, string name)
    {
        Debug.Log("Trying to add " + name + " with time " + time);
        Highscore newHighscore = new Highscore()
        {
            time = time,
            name = name
        };
        
        // Loop from best to worst
        for (int i = 0; i < highscoreCount; i++)
        {
            // if highscore don't exists
            // Or
            // as soon as the new highscore is better than the current highscore
            if (i >= highscores.Count)
            {
                highscores.Add(newHighscore);
                
                SaveCurrentHighscoreBoard();
                return;
            }
            else
            {
                if (time < highscores[i].time)
                {
                    highscores.Insert(i, newHighscore);
                
                    SaveCurrentHighscoreBoard();
                    return;
                }
            }
        }
    }

    public void SaveCurrentHighscoreBoard()
    {
        for (int i = 0; i < highscores.Count; i++)
        {
            PlayerPrefs.SetString(i.ToString(), JsonUtility.ToJson(highscores[i]));
        }
        
        PlayerPrefs.Save();
    }

    [ContextMenu("Clear Highscores")]
    public void ClearHighscores()
    {
        PlayerPrefs.DeleteAll();
    }
    
    [ContextMenu("Add new debug score")]
    public void DebugAddNewHighscore()
    {
        Highscore newHighscore = new Highscore()
        {
            time = Random.Range(0, 100),
            name = "DBG"
        };

        RegisterNewHighscore(newHighscore.time, newHighscore.name);
    }
    
    public struct Highscore
    {
        public string name;
        public float time;
    }
}