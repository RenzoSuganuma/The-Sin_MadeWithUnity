using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
/// <summary>
/// Unity UIToolkit対応版のクラス 2023/06/06
/// </summary>
public class UISystemForHorrorGame : MonoBehaviour
{
    /// <summary> 同じ階層のUIDocument </summary>
    UIDocument _document;

    /// <summary> アイテム名表示のクラス </summary>
    public ItemTextController _itemTextController;

    /// <summary> 拾ったアイテム名表示のクラス </summary>
    public ItemPickedTextController _itemPickedTextController;
    
    /// <summary> 目標表示のクラス </summary>
    public ObjectiveTextController _objectiveTextController;

    /// <summary> 一時停止テキストの表示クラス </summary>
    public PausedTextController _pausedTextController;

    /// <summary> タイトルに戻るボタンの管理クラス </summary>
    public BacktoTitleButtonChecker _backtoTitleButtonChecker;

    /// <summary> 遺言の管理クラス </summary>
    public DiyingWillController _diyingWillController;

    private void Awake()
    {
        if (GetComponent<UIDocument>() != null)//UIDocumenを取得出来たらそのまま取得
        {
            _document = GetComponent<UIDocument>();
            //public のクラスに代入
            this._itemTextController = new ItemTextController(_document.rootVisualElement);
            this._itemPickedTextController = new ItemPickedTextController(_document.rootVisualElement);
            this._objectiveTextController = new ObjectiveTextController(_document.rootVisualElement);
            this._pausedTextController = new PausedTextController(_document.rootVisualElement);
            this._backtoTitleButtonChecker = new BacktoTitleButtonChecker(_document.rootVisualElement);
            this._diyingWillController = new DiyingWillController(this._document.rootVisualElement);
        }
    }
}

/// <summary>
/// 拾えるアイテム名を表示するクラス
/// </summary>
public class ItemTextController
{
    private UnityEngine.UIElements.Label _label;
    
    public ItemTextController(VisualElement root)
    {
        this._label = root.Q<UnityEngine.UIElements.Label>("ItemText");//()内の文字列はNameでバインドされている文字列
        //値の初期化
        this._label.text = string.Empty;
    }

    public void OutPutTextToDisplay(string title)
    {
        this._label.text = title;
    }
}

/// <summary>
/// 拾ったアイテム名を表示するクラス
/// </summary>
public class ItemPickedTextController
{
    private UnityEngine.UIElements.Label _label;
    public ItemPickedTextController(VisualElement root)
    {
        this._label = root.Q<UnityEngine.UIElements.Label>("ItemPickedText");//()内の文字列はNameでバインドされている文字列
        //値の初期化
        this._label.text = string.Empty;
    }

    public void OutPutTextToDisplay(string title)
    {
        this._label.text = title;
    }
}

/// <summary>
/// 目標を表示するクラス
/// </summary>
public class ObjectiveTextController
{
    private UnityEngine.UIElements.Label _label;
    public ObjectiveTextController(VisualElement root)
    {
        this._label = root.Q<UnityEngine.UIElements.Label>("ObjectiveText");//()内の文字列はNameでバインドされている文字列
        //値の初期化
        this._label.text = string.Empty;
    }

    public void OutPutTextToDisplay(string title)
    {
        this._label.text = title;
    }
}

/// <summary>
/// 一時停止時のテキストを表示する
/// </summary>
public class PausedTextController
{
    private UnityEngine.UIElements.Label _label;
    public PausedTextController(VisualElement root)
    {
        this._label = root.Q<UnityEngine.UIElements.Label>("PausedLabel");//()内の文字列はNameでバインドされている文字列
        //初期化
        this._label.visible = false;
    }

    public void SetVisible(bool isVisible)
    {
        this._label.visible = isVisible;
    }
}

/// <summary>
/// タイトルに戻るボタンの管理クラス
/// </summary>
public sealed class BacktoTitleButtonChecker
{
    private readonly UnityEngine.UIElements.Button _button;
    private bool _calledTitleScene = false;

    public BacktoTitleButtonChecker(VisualElement root)//Buttonの宣言があいまいなため
    {
        _button = root.Q<UnityEngine.UIElements.Button>("BackToMainMenuButton");
        _button.clicked += buttonClicked;
        this._button.visible = false;
    }

    private void buttonClicked()
    {
        Debug.Log(_button.text);
        BackToTitleClickedNow();
    }

    public void SetVisible(bool isVisible)
    {
        this._button.visible = isVisible;
    }

    private void BackToTitleClickedNow()
    {
        if (!this._calledTitleScene)
        {
            SetVisible(false);
            SceneManager.LoadScene("StartMenu");
        }
    }
}

/// <summary>
/// 遺言のテキストにアクセスするクラス
/// </summary>
public class DiyingWillController
{
    private UnityEngine.UIElements.Label _label;
    public DiyingWillController(VisualElement root)
    {
        this._label = root.Q<UnityEngine.UIElements.Label>("DiyingWillLabel");//()内の文字列はNameでバインドされている文字列
        this._label.text = "遺言";
        //初期化
        this._label.visible = false;
    }

    public void OutputTextToDisplay(string text)
    {
        this._label.text = text;
    }

    public void SetVisible(bool isVisible)
    {
        this._label.visible = isVisible;
    }

    public bool GetVisible()
    {
        return this._label.visible;
    }
}