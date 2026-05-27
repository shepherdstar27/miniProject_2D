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

        // 🚨 [추적 1] 여기서 targetUI가 null인지 검사하고, null이면 강제 중단!
        if (targetUI == null)
        {
            Debug.LogError($"[추적 1] OpenUI 멈춤: {uiType}의 targetUI가 null입니다! 화면에 띄울 수 없습니다.");
            return null; // 에러가 터지기 전에 안전하게 빠져나갑니다.
        }

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
            GameObject loadedObj = Resources.Load<GameObject>(path);

            // 🚨 [추적 2] 프리팹 로드 자체를 실패했는지 확인
            if (loadedObj == null)
            {
                Debug.LogError($"[추적 2] 로드 실패: Resources 폴더 안에서 '{path}' 경로의 프리팹을 아예 찾지 못했습니다.");
                return null;
            }

            Transform parentRoot = GetRootTransform(rootType);
            GameObject gObj = Instantiate(loadedObj, parentRoot);

            var uiBase = gObj.GetComponent<UIBase>();

            // 🚨 [추적 3] 생성은 했는데 UIBase 컴포넌트를 못 찾았는지 확인
            if (uiBase == null)
            {
                Debug.LogError($"[추적 3] 컴포넌트 실종: 프리팹({loadedObj.name})은 생성했지만, 그 최상위 객체에서 'UIBase'를 상속받은 스크립트를 찾을 수 없습니다!");
            }

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