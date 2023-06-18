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

    /// <summary>
    /// スキップするボタンのクラス
    /// </summary>
    SkipPrologueButtonChecker _skipPrologueButtonChecker;

    /// <summary>
    /// ゲームパッドの振動コントロールのクラス
    /// </summary>
    GamePadVibrationControllerSystem _gamePadVibrationControllerSystem;

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
            //this._textController.UpdateText(this._outPutTextArray[0]);//アウトプット
            Debug.Log($"array length {this._outPutTextArray.Length}");
        }

        if (GetComponent<GamePadVibrationControllerSystem>() != null)//ゲームパッドの振動コントローラーを取得出来たらそのまま取得
        {
            this._gamePadVibrationControllerSystem = GetComponent<GamePadVibrationControllerSystem>();
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
            this._textController.UpdateText(this._outPutTextArray[i],this._gamePadVibrationControllerSystem);//アウトプット
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
    private UnityEngine.UIElements.Label _label, _emphasisLabel;
    public TextController(VisualElement root)
    {
        this._label = root.Q<UnityEngine.UIElements.Label>("Prologue_Label");
        this._emphasisLabel = root.Q<UnityEngine.UIElements.Label>("Prologue_Label_Emphasis");

        this._label.text = string.Empty;//通常
        this._emphasisLabel.text = string.Empty;//強調
        this._emphasisLabel.visible = false;//非可視
    }

    public void UpdateText(string text)
    {
        if (text.StartsWith("|"))
        {
            this._emphasisLabel.visible = true;//可視化
            this._label.visible = false;//非可視
            this._emphasisLabel.text = text.Replace("|", "");//出力
        }
        else
        {
            this._label.visible = true;//可視化
            this._emphasisLabel.visible = false;//非可視
            this._label.text = text;//出力
        }
    }

    public void UpdateText(string text, GamePadVibrationControllerSystem gamePadVibrationControllerSystem)
    {
        if (text.StartsWith("|"))//強調表示の時
        {
            this._emphasisLabel.visible = true;//可視化
            this._label.visible = false;//非可視
            this._emphasisLabel.text = text.Replace("|", "");//出力
        }
        else if (text.StartsWith("^"))//振動表現があったとき宇
        {
            text = text.Replace("^", "");//^を消して詰める
            gamePadVibrationControllerSystem.GamepadViverateRapid(30, 2);//ゲームパッド振動

            this._label.visible = true;//可視化
            this._emphasisLabel.visible = false;//非可視
            this._label.text = text;//出力
        }
        else//そのどちらでもない通常表示の時
        {
            this._label.visible = true;//可視化
            this._emphasisLabel.visible = false;//非可視
            this._label.text = text;//出力
        }
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