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
    SimplePopup,
    PortUI,
    InventoryUI
}

public class UIManager : MonoBehaviour
{
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
        DontDestroyOnLoad(transform.root.gameObject);
    }

    private void Start()
    {
        this.ShowStartupUIOnGameStart();
    }

    public UIBase OpenUI(UIRootType rootType, UIType uiType)
    {
        if (_createdUIDic.ContainsKey(uiType) && _createdUIDic[uiType] == null)
        {
            _createdUIDic.Remove(uiType);
            _openedUISet.Remove(uiType);
        }

        var targetUI = GetCreatedUI(rootType, uiType);
        if (_openedUISet.Contains(uiType) == false)
        {
            targetUI.gameObject.SetActive(true);
            _openedUISet.Add(uiType);
        }
        return targetUI;
    }

    public void CloseUI(UIType uiType)
    {
        if (_openedUISet.Contains(uiType) == true)
        {
            if (_createdUIDic.ContainsKey(uiType) == true)
            {
                var targetUI = _createdUIDic[uiType];
                if (targetUI != null)
                {
                    Destroy(targetUI.gameObject);
                }
                _createdUIDic.Remove(uiType);
            }
            _openedUISet.Remove(uiType);
            Debug.Log($"[UIManager] {uiType} 가 화면에서 완전히 파괴 및 해제되었습니다.");
        }
    }

    private UIBase GetCreatedUI(UIRootType rootType, UIType uiType)
    {
        if (_createdUIDic.ContainsKey(uiType) == false)
        {
            string path = $"Prefabs/UI/{rootType}/{uiType}";
            Debug.Log($"[UI 로드 시도] 조립된 경로: {path}");
            GameObject loadedObj = Resources.Load<GameObject>(path);
            if (loadedObj == null)
            {
                Debug.LogError($"[UI 로드 실패] {path} 경로에서 프리팹을 찾지 못했습니다! 대소문자나 폴더 구조를 다시 확인하세요.");
                return null;
            }
            Transform parentRoot = GetRootTransform(rootType);

            GameObject gObj = Instantiate(loadedObj, parentRoot);

            var uiBase = gObj.GetComponent<UIBase>();
            _createdUIDic.Add(uiType, uiBase);
        }
        return _createdUIDic[uiType];
    }

    private Transform GetRootTransform(UIRootType rootType)
    {
        switch (rootType)
        {
            case UIRootType.BackgroundUI:
                return Canvas_BgRoot;
            case UIRootType.MainUI:
                return Canvas_MainRoot;
            case UIRootType.ContentUI:
                return Canvas_ContentRoot;
            case UIRootType.PopupUI:
                return Canvas_PopupRoot;
            case UIRootType.VeryFrontUI:
                return Canvas_VeryFrontRoot;
            default:
                return null;
        }
    }

}