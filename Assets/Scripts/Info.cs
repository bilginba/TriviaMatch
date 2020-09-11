using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Info", menuName = "Info")]
public class Info : ScriptableObject
{
    public string[] left_side;
    public string[] right_side;

    public Dictionary<string, int> infoContents = new Dictionary<string, int>();
    
    public void AddInfos()
    {
        if(left_side.Length == right_side.Length)
        {
            for (int i = 0; i < left_side.Length; i++)
            {
                infoContents.Add(left_side[i], i);
                infoContents.Add(right_side[i], i);
            }
            
        }
        else
        {
            Debug.Log("Array sizes should match.");
        }
       
    }
    
}
