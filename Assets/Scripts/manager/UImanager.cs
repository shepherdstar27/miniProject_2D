using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum UIRootType
{
    None = 0,
    BackgroundUI,
    MainUI,
    ContentUI,
    PopupUI,
    VeryFrontUI
}

public enum UIType
{
    MainUI,
    GameUI,
    SimplePopup
}

public class UIManager : MonoBehaviour
{
    // 규칙: 유니티에서 참조하는 객체는 대문자 시작, SerializeField private 구조
    [SerializeField] private Transform Canvas_BgRoot;
    [SerializeField] private Transform Canvas_MainRoot;
    [SerializeField] private Transform Canvas_ContentRoot;
    [SerializeField] private Transform Canvas_PopupRoot;
    [SerializeField] private Transform Canvas_VeryFrontRoot;

    public static UIManager Inst { get; set; }

    private Dictionary<UIType, UIBase> _createdUIDic = new Dictionary<UIType, UIBase>();
    private HashSet<UIType> _openedUISet = new HashSet<UIType>();

    private void Awake()
    {
        Inst = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        // 규칙: UI 매니저 확장 메서드를 활용해 스타트업 UI 세팅
        this.ShowStartupUIOnGameStart();
    }

    public UIBase OpenUI(UIRootType rootType, UIType uiType)
    {
        var targetUI = GetCreatedUI(rootType, uiType);
        if (_openedUISet.Contains(uiType) == false)
        {
            targetUI.gameObject.SetActive(true);
            _openedUISet.Add(uiType);
        }
        return targetUI;
    }

    public void CloseUI(UIRootType rootType, UIType uiType)
    {
        if (_openedUISet.Contains(uiType))
        {
            var targetUI = _createdUIDic[uiType];
            targetUI.gameObject.SetActive(false);
            _openedUISet.Remove(uiType);
        }
    }

    private UIBase GetCreatedUI(UIRootType rootType, UIType uiType)
    {
        if (_createdUIDic.ContainsKey(uiType) == false)
        {
            string path = $"Prefabs/UI/{rootType}/{uiType}";
            GameObject loadedObj = Resources.Load<GameObject>(path);
            Transform parentRoot = GetRootTransform(rootType);

            GameObject gObj = Instantiate(loadedObj, parentRoot);
            var uiBase = gObj.GetComponent<UIBase>();
            _createdUIDic.Add(uiType, uiBase);
        }
        return _createdUIDic[uiType];
    }

    private Transform GetRootTransform(UIRootType rootType)
    {
        return rootType switch
        {
            UIRootType.BackgroundUI => Canvas_BgRoot.transform,
            UIRootType.MainUI => Canvas_MainRoot.transform,
            UIRootType.ContentUI => Canvas_ContentRoot.transform,
            UIRootType.PopupUI => Canvas_PopupRoot.transform,
            UIRootType.VeryFrontUI => Canvas_VeryFrontRoot.transform,
            _ => null
        };
    }

}