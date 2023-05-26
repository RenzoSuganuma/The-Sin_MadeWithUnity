using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// このクラスは敵オブジェクトの実体にアタッチしないこと。敵キャラマネージャーとして機能 ver - alpha
/// </summary>

public class EnemyManager : MonoBehaviour
{
    /// <summary> プレイヤー操作用のクラス </summary>
    PlayerController _playerController;

    /// <summary> プレイヤーが懐中電灯をつけてるかのフラグ </summary>
    private bool _playerIsLighting = false;

    /// <summary> 敵のオブジェクト </summary>
    [SerializeField] GameObject _enemyObject = null;

    private void Start()
    {
        if(GameObject.FindGameObjectWithTag("Player").TryGetComponent<PlayerController>(out PlayerController playerController))//プレイヤーオブジェクトの検索
            _playerController = playerController;
    }

    // Update is called once per frame
    private void Update()
    {
        _playerIsLighting = _playerController._flashLightIsOn;//プレイヤー操作用のクラスからの懐中電灯のフラグの受信
        if (_playerIsLighting)//懐中電灯をつけているうちにはこのオブジェクトは無効
            _enemyObject.SetActive(false);
        else
            _enemyObject.SetActive(true);

        //print($"{_playerIsLighting} : is player lighting status");
    }
}
