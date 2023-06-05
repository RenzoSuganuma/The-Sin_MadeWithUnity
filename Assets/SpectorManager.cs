using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// このクラスは敵オブジェクトの実体にアタッチしないこと。敵キャラマネージャーとして機能 ver - alpha
/// </summary>

public class SpectorManager : MonoBehaviour
{
    /// <summary> プレイヤーが懐中電灯をつけてるかのフラグ </summary>
    private bool _playerIsLighting = false;

    /// <summary> 敵のオブジェクト配列 </summary>/// 
    [SerializeField] GameObject[] _enemyObject = null;
    /// <summary> スマホの懐中電灯のオブジェクト </summary>
    [SerializeField] GameObject _playerFlashLight = null;

    private void Start()
    {
        this._playerFlashLight = GameObject.FindGameObjectWithTag("FlashLight");
        this._enemyObject = GameObject.FindGameObjectsWithTag("Spector_Enemy");//Spector＿Enemyのタグの紐づけされている敵すべて検索
    }

    // Update is called once per frame
    private void Update()
    {
        this._playerIsLighting = this._playerFlashLight.GetComponent<Light>().enabled;//プレイヤー操作用のクラスからの懐中電灯のLightコンポーネントのOnOFFを監視、コンディションの取得
        if (_playerIsLighting)//懐中電灯をつけているうちにはこのオブジェクトは無効
        {
            if (_enemyObject != null)//nullチェック
            {
                foreach (GameObject obj in _enemyObject)
                {
                    if (obj != null)//nullチェックしてからアクティブ状態の切り替え
                        obj.SetActive(false);
                }
            }
        }
        else
        {
            if(_enemyObject != null)
            {
                foreach (GameObject obj in _enemyObject)
                {
                    if(obj !=  null)//nullチェックしてからアクティブ状態の切り替え
                        obj.SetActive(true);
                }
            }
        } 
        //print($"{_playerIsLighting} : is player lighting status");
    }
}
