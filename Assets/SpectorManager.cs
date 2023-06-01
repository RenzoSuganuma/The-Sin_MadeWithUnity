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
        _playerFlashLight = GameObject.FindGameObjectWithTag("FlashLight");
        _enemyObject = GameObject.FindGameObjectsWithTag("Spector_Enemy");//Spector＿Enemyのタグの紐づけされている敵すべて検索
    }

    // Update is called once per frame
    private void Update()
    {
        _playerIsLighting = _playerFlashLight.GetComponent<Light>().enabled;//プレイヤー操作用のクラスからの懐中電灯のLightコンポーネントのOnOFFを監視、コンディションの取得
        if (_playerIsLighting)//懐中電灯をつけているうちにはこのオブジェクトは無効
        {
            foreach(GameObject obj in _enemyObject)
                obj.SetActive(false);
        }
        else
        {
            foreach (GameObject obj in _enemyObject)
                obj.SetActive(true);
        } 

        //print($"{_playerIsLighting} : is player lighting status");
    }
}
