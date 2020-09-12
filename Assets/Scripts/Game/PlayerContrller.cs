using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerContrller : MonoBehaviour
{
    public Transform rayDown, rayLeft, rayRight;//射线检测出发点
    public LayerMask platformLayer;
    public LayerMask obstacleLayer;

    private AudioSource my_AudioSource;

    private bool isJumping = false;//是否在跳跃中
    private bool isMoveLeft = false;//是否向左移动
    private bool isMove = false;//判断角色刚开始是否移动，若移动，则设置为true，平台掉落计时器开启
    private GameObject lastHitGO;//上一个平台
    private ManagerVars vars;
    private Rigidbody2D my_Body;//拿到角色的rigidbody组件
    private SpriteRenderer spriteRenderer;
    private Vector3 nextLeftPlatform, nextRightPlatform;

    void Awake()
    {
        my_AudioSource = GetComponent<AudioSource>();
        EventCenter.AddListener<int>(EventDefine.ChangeSkin, ChangeSkin);
        EventCenter.AddListener<bool>(EventDefine.IsMusicOn, IsMusicOn);
        my_Body = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        vars = ManagerVars.GetManagerVars();
    }
    private void Start()
    {
        ChangeSkin(GameManager.Instance.GetSelectedSkin());
    }
    private void OnDestroy()
    {
        EventCenter.RemoveListener<int>(EventDefine.ChangeSkin, ChangeSkin);
        EventCenter.RemoveListener<bool>(EventDefine.IsMusicOn, IsMusicOn);
    }
    void Update()
    {
        //判断游戏是否开始或者结束
        if (GameManager.Instance.IsGameStart == false || GameManager.Instance.IsGameOver || GameManager.Instance.IsGamePause)
        {
            return;
        }

        //判断鼠标是否点击到UI上
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            int fingerId = Input.GetTouch(0).fingerId;
            if (EventSystem.current.IsPointerOverGameObject(fingerId))
            {
                return;
            }
        }
        else
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }
        }

        //通过鼠标左键控制角色移动
        if (Input.GetMouseButtonDown(0) && isJumping == false && nextLeftPlatform != Vector3.zero)
        {

            if (isMove == false)
            {
                EventCenter.Broadcast(EventDefine.PlayerMove);
                isMove = true;
            }
            //添加跳跃音效
            my_AudioSource.PlayOneShot(vars.jumpClip);

            Vector3 mousePos = Input.mousePosition;
            if (mousePos.x <= Screen.width / 2)
            {
                isMoveLeft = true;
            }
            else if (mousePos.x > Screen.width / 2)
            {
                isMoveLeft = false;
            }
            Jump();
        }

        //踩空掉落，游戏结束
        if (my_Body.velocity.y < 0 && IsRayPlatform() == false && GameManager.Instance.IsGameOver == false)
        {
            //添加掉落音效
            my_AudioSource.PlayOneShot(vars.fallClip);

            spriteRenderer.sortingLayerName = "Default";
            GetComponent<Collider2D>().enabled = false;
            GameManager.Instance.IsGameOver = true;
            //调用结束面板
            StartCoroutine(DelayShowGameOverPanel());
        }
        //碰到障碍物死亡，游戏结束
        if (isJumping && IsRayObstacle() && GameManager.Instance.IsGameOver == false)
        {
            //添加碰到障碍物死亡音效
            my_AudioSource.PlayOneShot(vars.hitClip);

            spriteRenderer.enabled = false;
            //实例化死亡特效
            GameObject go = ObjectPool.Instance.GetDeathEffect();
            go.transform.position = transform.position;
            go.SetActive(true);
            GameManager.Instance.IsGameOver = true;
            //调用结束面板
            StartCoroutine(DelayShowGameOverPanel());
        }
        //和平台一起掉落，游戏结束
        if (transform.position.y - Camera.main.transform.position.y < -6 && GameManager.Instance.IsGameOver == false)
        {
            //添加掉落音效
            my_AudioSource.PlayOneShot(vars.fallClip);

            GameManager.Instance.IsGameOver = true;
            //游戏结束，显示结束面板
            StartCoroutine(DelayShowGameOverPanel());
        }
    }
    //更换角色皮肤
    private void ChangeSkin(int selectIndex)
    {
        spriteRenderer.sprite = vars.characterSkinSpriteList[selectIndex];
    }
    //游戏结束死亡后，显示结束面板
    IEnumerator DelayShowGameOverPanel()
    {
        yield return new WaitForSeconds(1);
        EventCenter.Broadcast(EventDefine.ShowGameOverPanel);
    }
    //是否检测到平台
    bool IsRayPlatform()
    {
        RaycastHit2D hit = Physics2D.Raycast(rayDown.position, Vector2.down, 1f, platformLayer);
        if (hit.collider != null)
        {
            if (hit.collider.tag == "Platform")
            {
                if (lastHitGO != hit.collider.gameObject)
                {
                    if (lastHitGO == null)
                    {
                        lastHitGO = hit.collider.gameObject;
                        return true;
                    }
                    //增加分数
                    EventCenter.Broadcast(EventDefine.AddScore);
                    lastHitGO = hit.collider.gameObject;
                }
                return true;
            }
        }
        return false;
    }
    //是否检测到障碍物
    bool IsRayObstacle()
    {
        RaycastHit2D leftHit = Physics2D.Raycast(rayLeft.position, Vector2.left, 0.15f, obstacleLayer);
        RaycastHit2D rightHit = Physics2D.Raycast(rayRight.position, Vector2.right, 0.15f, obstacleLayer);

        if (leftHit.collider != null)
        {
            if (leftHit.collider.tag == "Obstacle")
            {
                return true;
            }
        }
        if (rightHit.collider != null)
        {
            if (rightHit.collider.tag == "Obstacle")
            {
                return true;
            }
        }
        return false;
    }
    //玩家跳跃
    void Jump()
    {
        if (isMoveLeft)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            transform.DOMoveX(nextLeftPlatform.x, 0.2f);
            transform.DOMoveY(nextLeftPlatform.y + 0.8f, 0.15f);
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 1);
            transform.DOMoveX(nextRightPlatform.x, 0.2f);
            transform.DOMoveY(nextRightPlatform.y + 0.8f, 0.15f);
        }
        isJumping = true;
        //广播生成路径方法
        EventCenter.Broadcast(EventDefine.DecidePath);
    }
    //触发检测，有无落到平台上
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Platform")
        {
            Vector3 currentPlatformPos = collision.transform.position;
            nextLeftPlatform = new Vector3(currentPlatformPos.x - vars.nextXPos,
                currentPlatformPos.y + vars.nextXPos, 0);
            nextRightPlatform = new Vector3(currentPlatformPos.x + vars.nextXPos,
                currentPlatformPos.y + vars.nextXPos, 0);
            isJumping = false;
        }
    }
    //碰撞检测，有无吃到钻石
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "PickUp")
        {
            //添加吃到钻石音效
            my_AudioSource.PlayOneShot(vars.diamondClip);
            EventCenter.Broadcast(EventDefine.AddDiamond);
            collision.gameObject.SetActive(false);
        }
    }
    //是否开启音效
    void IsMusicOn(bool value)
    {
        my_AudioSource.mute = !value;
    }
}
