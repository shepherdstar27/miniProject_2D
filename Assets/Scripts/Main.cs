using UnityEngine;

public class MainUI : UImanager
{

    [SerializeField] private UImanager Button_Gamecontinue;
    [SerializeField] private UImanager Button_Gamestart;
    [SerializeField] private UImanager Button_Gameloading;
    [SerializeField] private UImanager Button_GameObtion;
    [SerializeField] private UImanager Button_GameQuit;

    private void OnEnable()
    {
        Button_Gamecontinue.bindOnClickButtonEvent(Button_Gamecontinue);
        Button_Gamestart.bindOnClickButtonEvent( Button_Gamestart);
        Button_Gameloading.bindOnClickButtonEvent(Button_Gameloading);
        Button_GameObtion.bindOnClickButtonEvent(Button_GameObtion);
        Button_GameQuit.bindOnClickButtonEvent(Button_GameQuit);
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
