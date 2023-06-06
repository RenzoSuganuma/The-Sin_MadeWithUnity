using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
/// <summary>
/// Unity UIToolkit対応版のクラス 2023/06/06
/// </summary>
public class UISystemForHorrorGame : MonoBehaviour
{
    /// <summary> 同じ階層のUIDocument </summary>
    UIDocument _document;

    /// <summary> HPのプログバーのクラス </summary>
    public HPProgressBarController _hpProgBarController;

    /// <summary> アイテム名表示のクラス </summary>
    public ItemTextController _itemTextController;

    /// <summary> 拾ったアイテム名表示のクラス </summary>
    public ItemPickedTextController _itemPickedTextController;

    private void Start()
    {
        if (GetComponent<UIDocument>() != null)//UIDocumenを取得出来たらそのまま取得
        {
            _document = GetComponent<UIDocument>();
            //public のクラスに代入
            this._hpProgBarController = new HPProgressBarController(_document.rootVisualElement);
            this._itemTextController = new ItemTextController(_document.rootVisualElement);
            this._itemPickedTextController = new ItemPickedTextController(_document.rootVisualElement);
        }
    }

    private void Update()
    {
        
    }
}

public sealed class HPProgressBarController
{
    private UnityEngine.UIElements.ProgressBar _progressBar;

    public HPProgressBarController(VisualElement root)
    {
        this._progressBar = root.Q<UnityEngine.UIElements.ProgressBar>("HPBar");//()内の文字列はNameでバインドされている文字列
        //値の初期化
        this._progressBar.lowValue = 0f;
        this._progressBar.highValue = 100f;
    }

    /// <summary>
    /// ゲーム画面に表示される文字列のモディファイ
    /// </summary>
    /// <param name="title"></param>
    public void ModifyTitle(string title)
    {
        this._progressBar.title = title;
    }

    /// <summary>
    /// プログレスバーの値のモディファイ
    /// </summary>
    /// <param name="deltaValue"></param>
    public void ModifyProgressValue(float deltaValue)
    {
        this._progressBar.value += deltaValue;
    }
}

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