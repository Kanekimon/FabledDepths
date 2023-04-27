using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompletionManager : Singleton<CompletionManager>
{
    // Start is called before the first frame update
    void Start()
    {
        DatabaseManager.Instance.GetDungeonJson(1);


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
