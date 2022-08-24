
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterMove : MonoBehaviour
{
    private Rigidbody2D rb;
    GameObject _light;

    [SerializeField]
    float _jumpForce = 1400.0f;      //�W�����v���ɉ������

    float _defaultJumpForce;

    [SerializeField]
    float _runSpeed = 8.0f;         //�����Ă���Ԃ̑��x

    [SerializeField]
    float _wallJumpCoolTime = 0.3f; //�ǃW�����̃N�[���^�C��

    float horizontalKey;
    
    bool isGround = true;           //�n�ʂƐݒu���Ă��邩�Ǘ�����t���O
    bool isWall = false;             //
    bool wallJump = false;

    //string state;                 //�v���C���[�̏�ԊǗ��@//������ւ�͎g��Ȃ�����������Ă�������.�A�j���[�V�����ȂǗp
    //string prevState;             //�O�̏�Ԃ�ۑ�        //�g�p��:https://xr-hub.com/archives/8808
    //float _stateEffect = 1;          //�󋵂ɉ����ĉ��ړ����x��ς��邽�߂̌W��
    void Start()
    {
        GameManager.instance._hammer = 0;
        GameManager.instance._score = 0;
        this.rb = GetComponent<Rigidbody2D>();
        _defaultJumpForce = _jumpForce;
    }

    private void Update()
    {
        Move();
        WaterPlayer();

        if (Input.GetKey(KeyCode.Mouse1)){
            if (_light.activeSelf){
                _light.SetActive(false);
            } else {
                _light.SetActive(true);
            }
        }
    }

    private void Move()
    {
        horizontalKey = Input.GetAxis("Horizontal");
        if (!wallJump)
        {
            //�E���͂ō������ɓ���
            if (horizontalKey > 0)
            {
                rb.velocity = new Vector2(_runSpeed, rb.velocity.y);
            }
            //�����͂ō������ɓ���
            else if (horizontalKey < 0)
            {
                rb.velocity = new Vector2(-_runSpeed, rb.velocity.y);
            }
            //�{�^����b���Ǝ~�܂�
            else
            {
                rb.velocity = new Vector2(0,rb.velocity.y);
            }
        }

        if (isGround)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                this.rb.AddForce(transform.up * this._jumpForce);
                isGround = false;
            }
        }
        else if (!isGround)
        {
            if (isWall)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    rb.AddForce(new Vector2(-rb.velocity.x, 10f) * 125);
                    isWall = false;
                    wallJump = true;
                    Coroutine coroutine = StartCoroutine("DelayMethod", _wallJumpCoolTime);
                }
            }
        }
    }

    private IEnumerator DelayMethod(float delayFrameCount)
    {
        yield return new WaitForSecondsRealtime(delayFrameCount);
        if(wallJump)wallJump = false;
        
    }
        private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "Enemy"){
            Destroy(col.gameObject);
            this.rb.AddForce(transform.up * this._jumpForce);
        }
        if(col.gameObject.tag == "Ground" || col.gameObject.tag == "DropGround")
        {
            if (!isGround)
                isGround = true;
        }
        if(col.gameObject.tag == "Water")
        {
            _boolOxygun = true;
        }
    }
    private void OnTriggerStay2D(Collider2D col)
    {
        if(col.gameObject.tag == "Ground")
        {
            if (!isGround)
                isGround = true;
        }
        if(col.gameObject.tag == "Wall")
        {
            if (horizontalKey > 0 || horizontalKey < 0)
            {
                if (!isWall)
                {
                    isWall = true;
                }
                rb.velocity = new Vector2(0, 0.5f);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if(col.gameObject.tag == "Wall")
        {
            if (isWall) { isWall = false; }
        }
        if(col.gameObject.tag == "Ground")
        {
            if(isGround) { isGround = false; }
        }
        if(col.gameObject.tag == "Water")
        {
            _boolOxygun = false;
        }
    }

    [SerializeField]
    float _oxygenCount = 100;

    [SerializeField]
    Text oxugenText;

    bool _boolOxygun;

    [SerializeField]
    float _defaultGravityScale = 8; //�_�f��̏d��

    [SerializeField]
    float _anoxiaGravityScale; //���_�f��Ԃ̏d��

    [SerializeField]
    float _anoxiaJumpForce;
    void WaterPlayer()
    {
        if (_boolOxygun)
        {
            if(_oxygenCount >= 0)
            {
                _oxygenCount -= 0.05f;
                oxugenText.text = string.Format("{0:000}oxy",_oxygenCount);

                rb.gravityScale = _anoxiaGravityScale;
                _jumpForce = _anoxiaJumpForce;
            }else if (_oxygenCount <= 0)
            {
                print("finish");
            }
        }
        else if (!_boolOxygun)
        {
            if(_oxygenCount <= 100)
            {
                _oxygenCount += 0.05f;
                oxugenText.text = string.Format("{0:000}oxy", _oxygenCount);

                rb.gravityScale = _defaultGravityScale;
                _jumpForce = _defaultJumpForce;
            }
        }
    }
}