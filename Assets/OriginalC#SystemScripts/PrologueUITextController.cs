using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
/// <summary>
/// Unity UIToolkit対応版のクラス 2023/06/06
/// </summary>
public class PrologueUITextController : MonoBehaviour
{
    /// <summary> 同じ階層のUIDocument </summary>
    UIDocument _uiDocument;

    /// <summary>
    /// 出力参照先ラベル
    /// </summary>
    private UnityEngine.UIElements.Label _label;

    /// <summary>
    /// 出力するテキストを格納しているテキストアセット _でスプリットして配列化、各配列要素ごと画面出力
    /// </summary>
    [SerializeField] TextAsset _textAsset;

    /// <summary>
    /// 出力先ラベルのクラス
    /// </summary>
    TextController _textController;

    SkipPrologueButtonChecker _skipPrologueButtonChecker;

    /// <summary>
    /// 出力する文字列の配列
    /// </summary>
    private string[] _outPutTextArray = null;

    private void Awake()
    {
        if (GetComponent<UIDocument>() != null)//UIDocumenを取得出来たらそのまま取得
        {
            this._uiDocument = GetComponent<UIDocument>();
            this._textController = new TextController(_uiDocument.rootVisualElement);
            this._skipPrologueButtonChecker = new SkipPrologueButtonChecker(_uiDocument.rootVisualElement);

            this._outPutTextArray = this._textAsset.text.Split("_");//スプリット
            this._textController.UpdateText(this._outPutTextArray[0]);//アウトプット
            Debug.Log($"array length {this._outPutTextArray.Length}");
        }
    }

    private void Start()
    {
        StartCoroutine(OutPutTextRoutine(_outPutTextArray, _outPutTextArray.Length));
    }

    IEnumerator OutPutTextRoutine(string[] strArray, int arrayLength)
    {
        for (int i = 0; i < arrayLength; i++)
        {
            this._textController.UpdateText(this._outPutTextArray[i]);//アウトプット
            yield return new WaitForSeconds(3);
        }
        yield return null;
        StartCoroutine(LoadScene());
        Debug.Log($"Output END");
    }

    IEnumerator LoadScene()
    {
        AsyncOperation async = SceneManager.LoadSceneAsync("StartMenu");
        while (!async.isDone)
        {
            yield return null;
        }
    }
}

public class TextController
{
    private UnityEngine.UIElements.Label _label;
    public TextController(VisualElement root)
    {
        this._label = root.Q<UnityEngine.UIElements.Label>("Prologue_Label");

        this._label.text = string.Empty;
    }

    public void UpdateText(string text)
    {
        this._label.text = text;
    }
}

public sealed class SkipPrologueButtonChecker
{
    private readonly UnityEngine.UIElements.Button _button;
    private bool _calledTitleScene = false;

    public SkipPrologueButtonChecker(VisualElement root)//Buttonの宣言があいまいなため
    {
        _button = root.Q<UnityEngine.UIElements.Button>("Skip_Button");
        _button.clicked += buttonClicked;
    }

    private void buttonClicked()
    {
        Debug.Log(_button.text);
        BackToTitleClickedNow();
    }

    private void BackToTitleClickedNow()
    {
        if (!this._calledTitleScene)
        {
            SceneManager.LoadScene("StartMenu");
        }
    }
}