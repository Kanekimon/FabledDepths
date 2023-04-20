using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MenuUIManager : MonoBehaviour
{

    VisualElement root;

    Button startGame;
    Button roomEditor;


    // Start is called before the first frame update
    void Start()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
        startGame = root.Q<Button>("Start_Game_Button");
        roomEditor = root.Q<Button>("RoomEditor_Button");

        startGame.clicked += () => LoadScene("Game");
        roomEditor.clicked += () =>  LoadScene("RoomEditor");
    }


    void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
