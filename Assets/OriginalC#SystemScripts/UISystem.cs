using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UISystem : MonoBehaviour
{
    VisualTreeAsset _elementTemplate;
    UIDocument _document;
    Label _hp, _mp;
    UnityEngine.UIElements.Button _button;
    ProgressBar _progress;

    private void OnEnable()
    {
        if (GetComponent<UIDocument>() != null)//UIDocumenを取得出来たらそのまま取得
        {
            _document = GetComponent<UIDocument>();
        }
    }

    private void Start()
    {
        new LabelScoper(_document.rootVisualElement, _elementTemplate, _hp);
        new ButtonChecker(_document.rootVisualElement, _elementTemplate, _button);
    }

    private void Update()
    {
        new ProgressBarController(_document.rootVisualElement, _elementTemplate, _progress);
    }
}

public sealed class LabelScoper//クラス宣言
{
    private /*readonly*/ VisualTreeAsset _visualTreeAsset;//現状のUI構成の情報が格納されているのをreadonlyで安全に値の取得
    private /*readonly*/ Label _label;//UIBuilderのContainerにあるUIの要素

    public LabelScoper(VisualElement root, VisualTreeAsset visualTreeAsset, Label label)//クラスの本元の関数。オーバーライドはナシ
    {
        _label = root.Q<Label>("MP");//ルートからLabelコンポーネントでNameがMPのLabelを格納
        var log = _label.text += " : 100";//取得したLabelの格納していて画面表示されている文字Label.textを格納
        Debug.Log(log);
    }
}

public sealed class ButtonChecker
{
    private readonly VisualTreeAsset _visualTreeAsset;
    private readonly UnityEngine.UIElements.Button _button;

    public ButtonChecker(VisualElement root, VisualTreeAsset visualTreeAsset, UnityEngine.UIElements.Button button)//Buttonの宣言があいまいなため
    {
        _button = root.Q<UnityEngine.UIElements.Button>("TestButton");
        _button.clicked += _button_clicked;
    }

    private void _button_clicked()
    {
        //throw new System.NotImplementedException();
        Debug.Log(_button.text);
    }
}

public sealed class ProgressBarController
{
    private VisualTreeAsset _visualTreeAsset;
    private UnityEngine.UIElements.ProgressBar _progressBar;

    public ProgressBarController(VisualElement root, VisualTreeAsset visualTreeAsset, ProgressBar progressBar)
    {
        _progressBar = root.Q<UnityEngine.UIElements.ProgressBar>("ProgBar");
        _progressBar.value += 1;//プログレスバーの値の加算
    }
}
