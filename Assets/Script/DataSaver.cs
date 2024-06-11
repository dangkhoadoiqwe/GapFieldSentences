using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataSaver : MonoBehaviour
{
  public static int ReadCatologryIndexValue(string name)
    {
        var value = -1;
        if (PlayerPrefs.HasKey(name))
            value = PlayerPrefs.GetInt(name);

        return value;
    }
    public static void SaveCatologryData(string Catologryname, int currenIndex)
    {
        PlayerPrefs.SetInt(Catologryname, currenIndex);
        PlayerPrefs.Save();

    }
   
     public static void ClearGameData(GameLevelData levelData)
    {
        foreach(var data  in levelData.data)
        {
            PlayerPrefs.SetInt(data.categoryName, -1);
        }
        PlayerPrefs.SetInt(levelData.data[0].categoryName, 0);
        PlayerPrefs.Save();
    }  
    
}
