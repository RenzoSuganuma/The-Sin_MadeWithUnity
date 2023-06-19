using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HorrorMonsterController : MonoBehaviour
{
    /// <summary>
    /// AIのNavMeshAgent
    /// </summary>
    [SerializeField] NavMeshAgent _navMesh;
    
    /// <summary>
    /// AIの追跡するプレイヤーのトランスフォーム
    /// </summary>
    [SerializeField] Transform _playerTransform;

    /// <summary>
    /// Raycastのオーバーロードのレイヤーマスクに渡すため
    /// </summary>
    [SerializeField] LayerMask _groundLayer, _playerLayer;

    /// <summary>
    /// このAIの体力
    /// </summary>
    [SerializeField] float _health = 0;

    /// <summary>
    /// 処刑人のアニメーター
    /// </summary>
    Animator _animator;

    /// <summary>
    /// 処刑人かの判定
    /// </summary>
    [SerializeField] bool _iamExecutioner;

    /// <summary>
    /// 追跡中かの判定
    /// </summary>
    [SerializeField] public bool _isChasing;

    //徘徊
    [SerializeField] Vector3 _movePoint = Vector3.zero;
    bool _movePointIsSet = false;
    [SerializeField] float _movePointRange = 0;

    //攻撃
    [SerializeField] float _intervalAttack = 0;
    bool _isAttacked = false;

    //状態
    [SerializeField] float _sightRange, _attackRange;
    [SerializeField] bool _playerFound, _playerCanAttack;

    //プレイヤーに投げつけるオブジェクト
    [SerializeField] GameObject _orb;

    //徘徊中のBGM
    [SerializeField] GameObject _patrollBGM;

    //追跡中BGM
    [SerializeField] GameObject _chaseBGM;

    private void Awake()
    {
        if(GameObject.FindGameObjectWithTag("Player").transform !=  null)
            this._playerTransform = GameObject.FindGameObjectWithTag("Player").transform;//プレイヤーの検索

        if (this.gameObject.GetComponent<NavMeshAgent>() != null)
            this._navMesh = this.gameObject.GetComponent<NavMeshAgent>();//NavMeshのコンポーネント取得

        if(this.gameObject.GetComponent<Animator>() != null && this._iamExecutioner)
            this._animator = this.gameObject.GetComponent<Animator>();//Animatorのコンポーネント取得
    }

    private void FixedUpdate()
    {
        //プレイヤーとの距離を出力
        Debug.Log($"Enemy Between Player Dis Is{Vector3.Distance(this.gameObject.transform.position,this._playerTransform.position)}");

        //攻撃か補足範囲の検索
        this._playerFound = Physics.CheckSphere(this.gameObject.transform.position, this._sightRange, this._playerLayer);
        this._playerCanAttack = Physics.CheckSphere(this.gameObject.transform.position, this._attackRange, this._playerLayer);

        if(!this._playerFound && !this._playerCanAttack)//もし補足範囲と攻撃範囲内にいない場合徘徊する
            PatrollingNow();
        if(this._playerFound && !this._playerCanAttack)//もし補足だけできたらプレイヤーを追跡する
            ChaseWithPlayer();
        if(this._playerFound && this._playerCanAttack)//もしプレイヤーに追いついて攻撃範囲内にいる場合
            AttackPlayerNow();
    }

    void PatrollingNow()
    {
        if(!this._movePointIsSet)
            SearchMovePoint();//徘徊する座標を見つける

        if(this._movePointIsSet)
            this._navMesh.SetDestination(this._movePoint);//見つけたらそこに向かう
        //処刑人のアニメーション操作 追跡はプレイヤーを補足したときのみフラグが立つ
        if (this._iamExecutioner)
            this._animator.SetBool("isWalking", true);

        Vector3 distance = transform.position - _movePoint;//徘徊する目標の座標との距離

        if(distance.magnitude > 1)
            this._movePointIsSet = false;//移動するフラグを外す
        //処刑人のアニメーション操作 追跡はプレイヤーを補足したときのみフラグが立つ
        if (distance.magnitude > 1 && this._iamExecutioner)
        {
            this._animator.SetBool("isChasing", false);
        }

        //BGMの設定
        if (this._chaseBGM != null && this._patrollBGM != null)
        {
            this._patrollBGM.SetActive(true);
            this._chaseBGM.SetActive(false);
        }

        //追跡中フラグ設定
        this._isChasing = false;

        Debug.Log("Walking");
    }
    void SearchMovePoint()
    {
        //X、Z軸での徘徊の目標の座標を乱数で発生させる
        float randX = Random.Range(-this._movePointRange, this._movePointRange);
        float randZ = Random.Range(-this._movePointRange, this._movePointRange);
        //Vector3にする
        this._movePoint = new Vector3(transform.position.x + randX, transform.position.y, this.gameObject.transform.position.z + randZ);
        //床にRaycastして光線が当たればそこに行けるので徘徊のフラグを立てる
        if (Physics.Raycast(this._movePoint, -transform.up, 2f, this._groundLayer))
            this._movePointIsSet = true;
    }

    void ChaseWithPlayer()
    {
        //BGMの設定
        if (this._chaseBGM != null && this._patrollBGM != null)
        {
            this._patrollBGM.SetActive(false);
            this._chaseBGM.SetActive(true);
        }

        //プレイヤーの追尾
        this._navMesh.SetDestination(this._playerTransform.position);
        this._isChasing = true;//追跡中フラグ設定

        //処刑人のアニメーション操作
        if (this._iamExecutioner)
        {
            this._animator.SetBool("isChasing", true);
            this._animator.SetBool("isWalking", false);
        }
    }

    void AttackPlayerNow()
    {
        //その時にいる座標にとどまる
        this._navMesh.SetDestination(this.gameObject.transform.position);
        //プレイヤーを向く
        this.gameObject.transform.LookAt(this._playerTransform.position);

        if (!this._isAttacked)
        {
            #region 攻撃処理
            if (this._orb != null)
            {
                GameObject orb = Instantiate(this._orb);
                orb.transform.position = this.gameObject.transform.position;
                Rigidbody rigidbody = orb.GetComponent<Rigidbody>();
                rigidbody.velocity = (this._playerTransform.position - this.gameObject.transform.position) * 10;
                Destroy(orb, 1f);
            }
            #endregion

            //攻撃処理をしたので攻撃をしたフラグを立てる
            this._isAttacked = true;
            //フラグを時差で立てる
            Invoke(nameof(RasetAttackCondition), this._intervalAttack);
        }
    }

    void RasetAttackCondition()
    {
        this._isAttacked = false;
    }

    public void ModifyHealth(int health)
    {
        //体力の補正
        this._health += health;
        //体力が０以下の時
        if (this._health < 0)
            Invoke(nameof(DestroyThisObject), .05f);
    }

    void DestroyThisObject()
    {
        Destroy(this.gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, this._attackRange);//攻撃範囲のギズモを描写
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(this.transform.position, this._sightRange);//補足範囲のギズモを描写
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireCube(this.gameObject.transform.position, Vector3.one * 10);
    }
}
