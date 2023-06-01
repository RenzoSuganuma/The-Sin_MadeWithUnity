using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���̃N���X�͓G�I�u�W�F�N�g�̎��̂ɃA�^�b�`���Ȃ����ƁB�G�L�����}�l�[�W���[�Ƃ��ċ@�\ ver - alpha
/// </summary>

public class SpectorManager : MonoBehaviour
{
    /// <summary> �v���C���[�������d�������Ă邩�̃t���O </summary>
    private bool _playerIsLighting = false;

    /// <summary> �G�̃I�u�W�F�N�g�z�� </summary>/// 
    [SerializeField] GameObject[] _enemyObject = null;
    /// <summary> �X�}�z�̉����d���̃I�u�W�F�N�g </summary>
    [SerializeField] GameObject _playerFlashLight = null;

    private void Start()
    {
        _playerFlashLight = GameObject.FindGameObjectWithTag("FlashLight");
        _enemyObject = GameObject.FindGameObjectsWithTag("Spector_Enemy");//Spector�QEnemy�̃^�O�̕R�Â�����Ă���G���ׂČ���
    }

    // Update is called once per frame
    private void Update()
    {
        _playerIsLighting = _playerFlashLight.GetComponent<Light>().enabled;//�v���C���[����p�̃N���X����̉����d���̃t���O�̎�M
        if (_playerIsLighting)//�����d�������Ă��邤���ɂ͂��̃I�u�W�F�N�g�͖���
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