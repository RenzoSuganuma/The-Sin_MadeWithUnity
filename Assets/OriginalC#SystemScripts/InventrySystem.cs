using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEditor;
/// <summary>
/// インベントリシステム用のクラス ver - alpha 2023/06/06
/// </summary>
public class InventrySystem : MonoBehaviour
{
    public GameObject[] _inventryObjects;//インベントリ内オブジェクトの格納変数

    /// <summary>
    /// 子オブジェクトの取得をしてGameObject型で返す関数非アクティブでもOK
    /// </summary>
    /// <param name="parentObject"></param>
    /// <returns></returns>
    public GameObject[] GetChildObjects(GameObject parentObject)
    {
        // 子オブジェクトを格納する配列作成
        GameObject[] _returnObjects = new GameObject[parentObject.transform.childCount];//添え字が変動する
        int arrayIndex = 0;//添え字

        // 子オブジェクトを順番に配列に格納
        foreach (Transform child in parentObject.transform)
        {
            if (child != null)
            {
                _returnObjects[arrayIndex++] = child.gameObject;//GameObject型に変換返り値に格納
            }
            //Debug.Log("child index" + childIndex);
        }
        return _returnObjects;
    }

    /// <summary>
    /// 子オブジェクトをタグで検索をかけて見つかったオブジェクトをすべて返す
    /// </summary>
    /// <param name="parentObject"></param>
    /// <param name="objectTag"></param>
    /// <returns></returns>
    public GameObject[] GetChildObjectsWithTag(GameObject parentObject, string objectTag)
    {
        // 子オブジェクトを格納する配列作成
        GameObject[] _returnObjects = new GameObject[parentObject.transform.childCount];//添え字が変動する step2
        GameObject[] _childObjects = new GameObject[parentObject.transform.childCount];//添え字が変動する step1
        int arrayIndex = 0;//添え字
        int foundObjectCount = 0;//タグ検索でヒットしたオブジェクトの総数をカウントする

        //int arraySizeCount = 0;

        // 子オブジェクトを順番に配列に格納
        foreach (Transform child in parentObject.transform)
        {
            if (child != null)
            {
                _childObjects[arrayIndex++] = child.gameObject;//GameObject型に変換返り値に格納
            }
            //Debug.Log("child index" + childIndex);
        }
        arrayIndex = 0;//直下のForeachで使う添え字の初期化。消すなこれを
        //タグの紐づけの検索
        foreach (GameObject obj in _childObjects)
        {
            if (obj.gameObject.CompareTag(objectTag))
            {
                _returnObjects[arrayIndex++] = obj;
                foundObjectCount++;
            }
        }
        Array.Resize(ref _returnObjects, foundObjectCount);
        return _returnObjects;
    }

    /// <summary>
    /// 子オブジェクトをタグで検索をかけて見つかったアクティブになってるオブジェクトをすべて返すactiveStatus == trueでactiveSelfの値がtrueのオブジェクトを探す
    /// </summary>
    /// <param name="parentObject"></param>
    /// <param name="objectTag"></param>
    /// <returns></returns>
    public GameObject[] GetChildObjectsWithTag(GameObject parentObject, string objectTag, bool activeStatus)
    {
        // 子オブジェクトを格納する配列作成
        GameObject[] _returnObjects = new GameObject[parentObject.transform.childCount];//添え字が変動する step2
        GameObject[] _childObjects = new GameObject[parentObject.transform.childCount];//添え字が変動する step1
        int arrayIndex = 0;//添え字
        int foundObjectCount = 0;//タグ検索でヒットしたオブジェクトの総数をカウントする

        //int arraySizeCount = 0;

        // 子オブジェクトを順番に配列に格納
        foreach (Transform child in parentObject.transform)
        {
            if (child != null)
            {
                _childObjects[arrayIndex++] = child.gameObject;//GameObject型に変換返り値に格納
            }
            //Debug.Log("child index" + childIndex);
        }
        arrayIndex = 0;//直下のForeachで使う添え字の初期化。消すなこれを
        //タグの紐づけの検索
        foreach (GameObject obj in _childObjects)
        {
            if (obj.gameObject.CompareTag(objectTag) && obj.activeSelf == activeStatus)
            {
                _returnObjects[arrayIndex++] = obj;
                foundObjectCount++;
            }
        }
        Array.Resize(ref _returnObjects, foundObjectCount);
        return _returnObjects;
    }

    /// <summary>
    /// 子オブジェクトをタグで検索をかけて見つかったオブジェクト配列のindex=0の実体を返す
    /// </summary>
    /// <param name="parentObject"></param>
    /// <param name="objectTag"></param>
    /// <returns></returns>
    public GameObject GetRandomChildObjectWithTag(GameObject parentObject, string objectTag)
    {
        // 子オブジェクトを格納する配列作成
        GameObject[] _returnObjects = new GameObject[parentObject.transform.childCount];//添え字が変動する step2
        GameObject[] _childObjects = new GameObject[parentObject.transform.childCount];//添え字が変動する step1
        int arrayIndex = 0;//添え字
        int foundObjectCount = 0;//タグ検索でヒットしたオブジェクトの総数をカウントする

        //int arraySizeCount = 0;

        // 子オブジェクトを順番に配列に格納
        foreach (Transform child in parentObject.transform)
        {
            if (child != null)
            {
                _childObjects[arrayIndex++] = child.gameObject;//GameObject型に変換返り値に格納
            }
            //Debug.Log("child index" + childIndex);
        }
        arrayIndex = 0;//直下のForeachで使う添え字の初期化。消すなこれを
        //タグの紐づけの検索
        foreach (GameObject obj in _childObjects)
        {
            if (obj.gameObject.CompareTag(objectTag))
            {
                _returnObjects[arrayIndex++] = obj;
                foundObjectCount++;
            }
        }
        Debug.Log("Found Object Is" + foundObjectCount);
        if(foundObjectCount > 0)//配列のサイズを最低でも1確保する
        {
            Array.Resize(ref _returnObjects, foundObjectCount);
            return _returnObjects[0];//nullが返るときもあるヨ
        }
        return null;
    }

    /// <summary>
    /// 親子関係を切る関数 引数はGameObject型
    /// </summary>
    /// <param name="childObject"></param>
    public void MakeChildToParent(GameObject childObject)
    {
        if (childObject != null)//オブジェクトの中身のチェック
        {
            childObject.gameObject.transform.parent = null;
        }
    }

    /// <summary>
    /// 親子関係を切る関数 引数はGameObject[]型
    /// </summary>
    /// <param name="childObjects"></param>
    public void MakeChildToParent(GameObject[] childObjects)
    {
        if (childObjects != null)//オブジェクトの中身のチェック
        {
            for (int i = 0; i < childObjects.Length; i++)
            {
                childObjects[i].gameObject.transform.parent = null;
            }
        }
    }

    /// <summary>
    /// 子オブジェクトに任意のオブジェクトを変えてしまう関数
    /// </summary>
    /// <param name="object2Child"></param>
    /// <param name="transform2Child"></param>
    public void MakeParenToChild(GameObject object2Child, Transform transform2Child)
    {
        if (object2Child != null && transform2Child != null)//null-check
        {
            object2Child.gameObject.transform.parent = transform2Child;//親子関係紐づけ
            object2Child.transform.localPosition = Vector3.zero;//位置の初期化
            object2Child.transform.localEulerAngles = Vector3.zero;//回転の初期化
            object2Child.SetActive(false);//非可視化
        }
    }
}