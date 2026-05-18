using System;
using UnityEngine;
using UnityEngine.UI;

public class UIButton : MonoBehaviour
{
    [SerializeField] private Button Button_Base;

    private void Awake()
    {
        if (Button_Base == null)
        {
            Button_Base = GetComponentInChildren<Button>();
        }
    }

    private void OnDisable()
    {
        Button_Base.onClick.RemoveAllListeners();
    }

    public void BindOnClickButtonEvent(Action onClickCallback)
    {
        if (Button_Base == null) return;
        Button_Base.onClick.AddListener(() => onClickCallback?.Invoke());
    }
}