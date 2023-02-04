﻿using System;
using System.Collections;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
public class BossController : MonoBehaviour
{
    [SerializeField]
    [Header("ボスのHP")]
    int _hp;
    
    [SerializeField]
    [Header("無敵時間")]
    float _continueTime;

    [SerializeField]
    int _jumpForce;

    [SerializeField]
    Rigidbody2D _playerRb2d;

    BossState _nowState;

    SpriteRenderer _sprite;

    Subject<string> attackSubject = new Subject<string>();

    [SerializeField]
    int _stageNumber;

    [SerializeField]
    Text _testText;
    private void Awake()
    {
        _sprite = GetComponent<SpriteRenderer>();
        _nowState = BossState.Nomal;
        _testText.enabled =false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("PlayerDestroy");
            Destroy(collision.gameObject);
            PlayreDestroy.GameOver(_stageNumber);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _playerRb2d.velocity = new Vector2(0, _jumpForce);
            Debug.Log("HIT");
            if (_nowState == BossState.Nomal && _hp >= 0)
            {
                _hp--;
                _nowState = BossState.Damage;
                OnDamage();
            }
            else if (_hp <= 0)
            {
                _nowState = BossState.Hp0Rendition;
                BossHp0();
            }
        }
    }

    private void OnDamage()
    {       
        float level = Mathf.Abs(Mathf.Sin(Time.time * 10));
        _sprite.color = new Color(1f, 1f, 1f, level);

        Debug.Log("ダメージ");

        _ = StartCoroutine(nameof(Damgeoff));
    }

    private void BossHp0()
    {
        gameObject.SetActive(false);
        _testText.enabled = true; 
    }

    IEnumerator Damgeoff()
    {
        yield return new WaitForSeconds(_continueTime);
        
        _sprite.color = new Color(1f, 1f, 1f, 1f);
        _nowState = BossState.Nomal;
        Debug.Log(_nowState);
    }
}