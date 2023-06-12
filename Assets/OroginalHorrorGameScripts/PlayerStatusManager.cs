using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// プレイヤーのステータス管理クラス
/// </summary>
public class PlayerStatusManager : MonoBehaviour
{
    [SerializeField] public float _playerMaxHealth = 100;
    [SerializeField] public float _playerCurrentHealth = 0;
    [SerializeField] GameObject _playerGamepadController;
    GamePadVibrationControllerSystem _vibrationControllerSystem;

    private void Awake()
    {
        this._playerCurrentHealth = this._playerMaxHealth;//体力の初期化
    }

    private void Update()
    {
        if (this._playerCurrentHealth <= 0)
        {
            Debug.Log("LoadStart");
            StartCoroutine(LoadScene());
        }
    }

    /// <summary>
    /// +-で体力の修正ができる
    /// </summary>
    /// <param name="health"></param>
    public void ModifyHealth(float health)
    {
        this._playerCurrentHealth += health;
    }

    /// <summary>
    /// 現在の体力値を返す
    /// </summary>
    /// <returns></returns>
    public float GetCurrentHealth()
    {
        return this._playerCurrentHealth;
    }

    IEnumerator LoadScene()
    {
        AsyncOperation async = SceneManager.LoadSceneAsync("FirstStage");
        while (!async.isDone)
        {
            if (this._playerGamepadController != null)
            {
                this._vibrationControllerSystem = this._playerGamepadController.GetComponent<GamePadVibrationControllerSystem>();
                this._vibrationControllerSystem.StopGamepadViverate();
            }
            yield return null;
        }
    }
}
