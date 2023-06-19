using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SearchService;

public class BossWarpToPlayerTrap : MonoBehaviour
{
    //ワープ先のトランスフォーム
    [SerializeField] private Transform _targetTransform;
    //ワープするオブジェクト
    [SerializeField] GameObject _bossObject;

    private void OnTriggerEnter(Collider other)
    {
        //プレイヤーがトラップのコライダー（トリガー）と接触したとき
        if (other.gameObject.CompareTag("Player") && other.gameObject != null)
        {
            this._bossObject.SetActive(false);
            this._bossObject.transform.position = this._targetTransform.position;
            this._bossObject.SetActive(true);

            Destroy(this.gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(this.transform.position, 5);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(this._targetTransform.position, Vector3.one);
    }
}
