using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameCore;
using System.Threading.Tasks;

public class PlayerControl : MonoBehaviour
{
    private int playerID;

    private string upButton;
    private string downButton;
    private string leftButton;
    private string rightButton;
    private string fireButton;

    private Vector3 spawnPosition;
    private SpriteRenderer spriteRenderer;
    private Direction direction = Direction.Up;
    private int level = 0;
    private int life = 3;
    private float moveSpeed = 2f;

    private float shootInterval = 0.8f;
    private bool isDead = false;
    private bool canShoot = true;
    private bool isDefend = true;
    private float defendInterval = 3f;
    private float defendTimer = 3f;
    private bool isStop = false;
    private bool isMove = true;

    private int curPicSet = 1;
    public Sprite[] curSprite1;
    public Sprite[] curSprite2;
    public Sprite[] level11;
    public Sprite[] level12;
    public Sprite[] level21;
    public Sprite[] level22;
    public Sprite[] level31;
    public Sprite[] level32;
    private AudioSource audioPlayer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioPlayer = GetComponent<AudioSource>();
        GameManager.Instance.RegisteryPlayer(this);
        DontDestroyOnLoad(gameObject);
    }
    

    public void SetAsPlayer1()
    {
        upButton = "w";
        downButton = "s";
        leftButton = "a";
        rightButton = "d";
        fireButton = "space";
        playerID = 1;
        spawnPosition = DataConst.Player1SpanwPoint;
    }

    public void SetAsPlayer2()
    {
        upButton = "up";
        downButton = "down";
        leftButton = "left";
        rightButton = "right";
        fireButton = "right ctrl";
        playerID = 2;
        spawnPosition = DataConst.Player2SpanwPoint;
    }

    private void ResetData()
    {
        level = 0;
        life = 3;
    }
    void Update()
    {
        if (isDead) return;
        if (Input.GetKeyDown(fireButton) && canShoot)
        {
            canShoot = false;
            Shoot();
            StartCoroutine(nameof(ShootCoolDown));
        }
        if (defendTimer >= 0f) 
        {
            defendTimer -= Time.fixedDeltaTime;
            if (defendTimer < 0) {
                isDefend = false;
            }
        }
        if (isStop) return;
        if(isMove)
            SpriteSwitch();
        Move();
    }

    IEnumerator ShootCoolDown()
    {
        yield return new WaitForSeconds(shootInterval);
        canShoot = true;
    }
    //Movement
    void Move() {
        if (Input.GetKey(upButton)){
            if (audioPlayer.pitch < 1.4f)
                audioPlayer.pitch += 0.04f;
            isMove = true;
            direction = direction != Direction.Up ? Direction.Up : direction;
            transform.Translate(transform.up * moveSpeed * Time.deltaTime, Space.Self);
            return;
        }
        else if (Input.GetKey(rightButton))
        {
            if (audioPlayer.pitch < 1.4f)
                audioPlayer.pitch += 0.04f;
            isMove = true;
            direction = direction != Direction.Right ? Direction.Right : direction;
            transform.Translate(transform.right * moveSpeed * Time.deltaTime, Space.Self);
            return;
        }
        else if (Input.GetKey(downButton))
        {
            if (audioPlayer.pitch < 1.4f)
                audioPlayer.pitch += 0.04f;
            isMove = true;
            direction = direction != Direction.Down ? Direction.Down : direction;
            transform.Translate(-transform.up * moveSpeed * Time.deltaTime, Space.Self);
            return;
        }
        else if (Input.GetKey(leftButton))
        {
            if (audioPlayer.pitch < 1.4f)
                audioPlayer.pitch += 0.04f;
            isMove = true;
            direction = direction != Direction.Left ? Direction.Left : direction;
            transform.Translate(-transform.right * moveSpeed * Time.deltaTime, Space.Self);
            return;
        }
        else{
            audioPlayer.pitch = 1f;
            isMove = false;
        }
    }
    void SpriteSwitch() {
        int dirID = (int)direction;
        if (curPicSet == 1)
        {
            spriteRenderer.sprite = curSprite1[dirID];
            curPicSet++;
        }
        else if (curPicSet == 2)
        {
            spriteRenderer.sprite = curSprite2[dirID];
            curPicSet--;
        }
    }
    //Shoot
    void Shoot() {
        AudioManager.Play(ResConst.FireSound);
        
        Vector3 pos = Vector3.zero;
        string bulletAssetPath = ResConst.Bullet[direction];
        switch (direction) {
            case Direction.Up: 
                pos = transform.position + transform.up * 0.3f;
                break;
            case Direction.Right: 
                pos = transform.position + transform.right * 0.3f;
                break;
            case Direction.Down: 
                pos = transform.position - transform.up * 0.3f;
                break;
            case Direction.Left: 
                pos = transform.position - transform.right * 0.3f;
                break;
        }
        AssetsManager.Instance.LoadGameObject(bulletAssetPath, (obj) =>
        {
            obj.transform.SetPositionAndRotation(pos, Quaternion.identity);
            if(obj.TryGetComponent(out Bullet bullet))
            {
                bullet.SetOwnPlayer(playerID, level);
            }
            if(level >= 2)
            {
                Scheduler.Instance.Delay(0.2f, () =>
                {
                    AudioManager.Play(ResConst.FireSound);
                    // 再生成一个
                    Instantiate(obj, pos, Quaternion.identity);
                });
            }
        });
    }
    //Upgrade
    public void Upgrade()
    {
        if (level == 3)
            return;
        level++;
        switch (level) {
            case 1: curSprite1 = level11; curSprite2 = level12; shootInterval /= 2f; break;
            case 2: curSprite1 = level21; curSprite2 = level22; break;
            case 3: curSprite1 = level31; curSprite2 = level32; break;
        }
    }
    //Get Shield
    public void Shield() {
        defendTimer = defendInterval;
        isDefend = true;
        AssetsManager.Instance.LoadGameObject(ResConst.ShieldEffect, (obj) =>
        {
            obj.SetParent(transform);
            obj.transform.localPosition = Vector3.zero;
        });
    }

    public void GetHit() {
        if (isDefend)
            return;
        isDead = true;
        AudioManager.Play(ResConst.DieSound);
        AssetsManager.Instance.LoadGameObject(ResConst.TankExplosionEffect, (obj) =>
        {
            obj.transform.SetPositionAndRotation(transform.position, Quaternion.identity);
        });
        Respawn();
    }
    //Friendly Fire
    public void GetFriendHit()
    {
        StartCoroutine(nameof(FriendlyFire));
    }
    IEnumerator FriendlyFire() {
        isStop = true;
        float a = 0f;
        while (true) {
            spriteRenderer.color = new Color(1, 1, 1, 0);
            yield return new WaitForSeconds(0.2f);
            a += 0.2f;
            spriteRenderer.color = new Color(1, 1, 1, 1);
            yield return new WaitForSeconds(0.2f);
            a += 0.2f;
            if (a >= 2f) {
                spriteRenderer.color = new Color(1, 1, 1, 1);
                break;
            }
        }
        isStop = false;
    }
    // Respawn
    public void Respawn(bool isNewLife = true)
    {
        gameObject.SetActive(false);
        if (life <= 0)
        {
            GameManager.Instance.PlayerLifeClear();
            return;
        }
        if (isNewLife)
        {
            --life;
        }
        AssetsManager.Instance.LoadGameObject(ResConst.BornEffect, (obj) =>
        {
            obj.transform.SetPositionAndRotation(spawnPosition, Quaternion.identity);
            Scheduler.Instance.Delay(1.5f, () =>
            {
                transform.SetPositionAndRotation(spawnPosition, Quaternion.identity);
                gameObject.SetActive(true);
                Shield();
                isDead = false;
                direction = Direction.Up;
                UIController.Instance.UpdateUI(UIUpdateType.PlayerLifeChange, playerID, life);
            });
        });
    }

    void OnEnable()
    {
        isStop = false;
        audioPlayer.mute = false;
    }

    private void OnDisable()
    {
        audioPlayer.mute = true;
        StopAllCoroutines();
    }

    public int PlayerID
    {
        get { return playerID; }
    }
    public int Level
    {
        get { return level; }
    }

}
