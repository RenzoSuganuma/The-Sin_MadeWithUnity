using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// プレイヤーのステータス管理クラス
/// </summary>
public class GameManager : MonoBehaviour
{
    //プレイヤー
    [SerializeField] public float _playerMaxHealth = 100;
    [SerializeField] public float _playerCurrentHealth = 0;
    [SerializeField] GameObject _playerGamepadController;
    [SerializeField] Transform _playerRestartposition;
    GameObject _playerObject;

    //敵キャラ（AI）
    [SerializeField] GameObject _enemyBossObject;
    [SerializeField] Transform[] _enemyBossSpawnPoint;
    HorrorMonsterController _monsterController;

    //デバイス
    GamePadVibrationControllerSystem _vibrationControllerSystem;

    private void Awake()
    {
        this._playerCurrentHealth = this._playerMaxHealth;//体力の初期化
        //プレイヤーの検索
        this._playerObject = GameObject.FindGameObjectWithTag("Player");
        //敵ボス検索
        this._enemyBossObject = GameObject.FindGameObjectWithTag("SpectorBoss_Enemy");
        if (this._enemyBossObject.TryGetComponent<HorrorMonsterController>(out HorrorMonsterController horrorMonster))
        {
            this._monsterController = horrorMonster;
        }
        //ゲームパッド操作クラスの取得
        this._vibrationControllerSystem = this._playerGamepadController.GetComponent<GamePadVibrationControllerSystem>();
    }

    private void Update()
    {
        #region  プレイヤーがボスに補足されてる時の操作

        if (this._monsterController._isChasing)
        {
            Debug.Log("BOSS IS CHASING!");
            this._vibrationControllerSystem.GamepadViverate(180);
        }
        #endregion

        if (this._playerCurrentHealth <= 0)
        {
            #region  ゲームパッドの振動を止める
            if (this._playerGamepadController != null)//ゲームパッドの振動を止める
            {
                this._vibrationControllerSystem = this._playerGamepadController.GetComponent<GamePadVibrationControllerSystem>();
                this._vibrationControllerSystem.StopGamepadViverate();
            }
            #endregion

            #region  ボスの再スポーン
            this._enemyBossObject.SetActive(false);
            this._enemyBossObject.transform.position = 
                this._enemyBossSpawnPoint[Random.Range(0, this._enemyBossSpawnPoint.Length)/*ランダムな敵ボスのスポーン地点を代入*/].position;
            this._enemyBossObject.SetActive(true);
            #endregion

            Debug.Log("LoadStart");

            #region  プレイヤーの再スポーン
            this._playerObject.SetActive(false);
            this._playerCurrentHealth = 100;
            this._playerObject.transform.position = this._playerRestartposition.transform.position;
            this._playerObject.SetActive(true);
            #endregion

            //StartCoroutine(LoadScene());
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;

        foreach (Transform t in this._enemyBossSpawnPoint)
        {
            Gizmos.DrawLine(t.position, t.position + Vector3.up * 10);
        }
    }
}
