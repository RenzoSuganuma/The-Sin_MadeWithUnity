using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// ゲームパッドの振動操作クラス
/// </summary>
public class GamePadVibrationControllerSystem : MonoBehaviour
{
    /// <summary>
    /// 現在刺さっているゲームパッド
    /// </summary>
    private Gamepad _gamepad;

    /// <summary>
    /// 振動の周波数
    /// </summary>
    private float _motorMinFrecuency = .1f, _motorMaxFrecuency = .5f;

    /// <summary>
    /// 振動中フラグ
    /// </summary>
    public bool _isVivrate = false;

    /// <summary>
    /// シーン読み込み時にゲームパッドの振動を止める
    /// </summary>
    [SerializeField] bool _forceStopViverate = false;

    private void Start()
    {
        if(Gamepad.current != null)//null check
            _gamepad = Gamepad.current;

        if (this._forceStopViverate)
        {
            StopGamepadViverate();
        }
    }

    /// <summary>
    /// ブッブッとゲームパッドを振動させる
    /// </summary>
    /// <param name="frames"></param>
    /// <param name="repeat"></param>
    public void GamepadViverateRapid(int frames,int repeat)
    {
        StartCoroutine(VivrateTheGamepadRapidlyNow(frames, repeat));
    }

    /// <summary>
    /// ブーーーとゲームパッドを振動させる
    /// </summary>
    /// <param name="frames"></param>
    public void GamepadViverate(int frames)
    {
        StartCoroutine(VivrateTheGamepadLongNow(frames));
    }

    public void StopGamepadViverate()
    {
        if(this._gamepad != null)
            _gamepad.SetMotorSpeeds(0f, 0f); // 振動を停止します
    }

    #region 振動：ブッブッ
    /// <summary>
    /// ブッブッ と振動する
    /// </summary>
    /// <param name="frames"></param>
    /// <param name="repeat"></param>
    /// <returns></returns>
    IEnumerator VivrateTheGamepadRapidlyNow(int frames, int repeat)
    {
        this._isVivrate = true;//振動中フラグ 1

        for (int timeCount = 0; timeCount < repeat; timeCount++)
        {
            for (int cnt = 0; cnt < frames; cnt++)
            {
                _gamepad.SetMotorSpeeds(this._motorMinFrecuency, this._motorMaxFrecuency); // 振動の強さを設定します
                    yield return null;
            }_gamepad.SetMotorSpeeds(0f, 0f); // 振動を停止します

            for (int cnt = 0; cnt < 3; cnt++)
            {
                _gamepad.SetMotorSpeeds(0f, 0f); // 振動を停止します
                    yield return null;
            }_gamepad.SetMotorSpeeds(0f, 0f); // 振動を停止します
        }

        this._isVivrate = false;//振動中フラグ 0
    }
    #endregion

    #region 振動：ブーーー
    /// <summary>
    /// ブッーーーー と振動する
    /// </summary>
    /// <param name="frames"></param>
    /// <returns></returns>
    IEnumerator VivrateTheGamepadLongNow(int frames)
    {
        this._isVivrate = true;//振動中フラグ 1

        for (int cnt = 0; cnt < frames; cnt++)
        {
            _gamepad.SetMotorSpeeds(this._motorMinFrecuency, this._motorMaxFrecuency); // 振動の強さを設定します
            yield return null;
        }
        _gamepad.SetMotorSpeeds(0f, 0f); // 振動を停止します

        for (int cnt = 0; cnt < 5; cnt++)
        {
            _gamepad.SetMotorSpeeds(0f, 0f); // 振動を停止します
            yield return null;
        }
        _gamepad.SetMotorSpeeds(0f, 0f); // 振動を停止します

        this._isVivrate = false;//振動中フラグ 0
    }
    #endregion
}