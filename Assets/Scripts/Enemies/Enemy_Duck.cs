using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy_Duck : MonoBehaviour, IEnemy
{
    EnemyBehavior behavior;
    private double hitPoints;
    public double HitPoints { get { return hitPoints; } set { hitPoints = value; } }

    private double damage;
    public double Damage { get { return damage; } }

    public SpriteRenderer spriteRenderer; // Now public
    public Animator spriteAnimator;
    private const float hurtCooldown = 0.25f;
    private float hurtTime;
    private bool duckIsHurt;

    Rigidbody rb;
    public Transform spriteTransform;

    public bool isKockedOut;
    public float knockoutStart;

    public Color damageFlashColor = Color.red;
    public float damageFlashDuration = 0.1f;

    public int knockback;

    public AudioSource[] Quacks;
    private int randomQuack;
    private float cooldown;
    private float next;

    void Awake()
    {
        behavior = GetComponent<EnemyBehavior>();
        rb = GetComponent<Rigidbody>();
    }

    public void Start()
    {
        spriteAnimator = gameObject.transform.Find("DuckSprite").GetComponent<Animator>();
        duckIsHurt = false;

        damage = 0.5;
        hitPoints = 5;
        rb = GetComponent<Rigidbody>();
        spriteTransform = this.gameObject.transform.GetChild(0);
        isKockedOut = false;

        // If spriteRenderer is not assigned, try to get it from the GameObject
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        randomQuack = Random.Range(0, 7);
        cooldown = 0.5f;
        next = Time.time + cooldown;
    }

    public void Attack()
    {
        // The attack for a basic duck is just moving towards you
    }

    private void Update()
    {
        if (isKnockoutOver()) UnfreezeObject();
        if (Time.time > next)
        {
            next = Time.time + Random.Range(0.5f, 2.0f);
            randomQuack = Random.Range(0, 7);
            Quacks[randomQuack].Play();
        }
        if (duckIsHurt && Time.time > hurtTime)
        {
            spriteAnimator.SetBool("DuckHurt",false);
            duckIsHurt = false;
        }
    }

    public void TakeDamage(int damage, bool isPoweredUp)
    {
        spriteAnimator.SetBool("DuckHurt", true);
        hurtTime = Time.time + hurtCooldown;
        duckIsHurt = true;

        StartCoroutine(FlashDamage());
        hitPoints -= damage;
        KnockbackEnemy();
        if (HitPoints <= 0)
        {
            Die();
            Swan.feathers++;
        }
        if (isPoweredUp && !isKockedOut)
            KnockedOut();
    }

    public void KnockedOut() {
        Debug.Log("ENEMY KNOCKED OUT");
        knockoutStart = Time.time;
        isKockedOut = true;

        spriteTransform.Translate(new Vector3(0,0.5f,0));
        spriteTransform.Rotate(new Vector3(0,0,1), 180);
        Vector3 position = spriteTransform.position;
        position.y -= 1;
        spriteTransform.position = position;

        FreezeObject(gameObject);
    }

    // Method to freeze a GameObject's position and rotation
    public void FreezeObject(GameObject obj)
    {
        rb.constraints = RigidbodyConstraints.FreezeAll;
    }

    public void UnfreezeObject()
    {
        isKockedOut = false;
        rb.constraints = RigidbodyConstraints.None; 
        spriteTransform.Rotate(new Vector3(0, 0, 1), 180);
        spriteTransform.Translate(new Vector3(0, -0.5f, 0));
        Vector3 position = spriteTransform.position;
        position.y += 1;
        spriteTransform.position = position;
    }

    private bool isKnockoutOver()
    {
        return isKockedOut && Time.time - knockoutStart > 0.5;
    }

    private IEnumerator FlashDamage()
    {
        // Change sprite color to damageFlashColor
        spriteRenderer.color = damageFlashColor;

        // Wait for the specified duration
        yield return new WaitForSeconds(damageFlashDuration);

        // Revert back to the original color
        spriteRenderer.color = Color.white; // You can use the originalColor variable if you have it defined
    }

    public void Die()
    {
        // Debug.Log("Enemy Dead!");
        randomQuack = Random.Range(0, 7);
        Quacks[randomQuack].Play();
        Destroy(gameObject);
    }

    private void KnockbackEnemy()
    {
    
         rb.AddForce(MovementControl.direction * 10, ForceMode.Impulse);
     /*   Vector3 position = gameObject.transform.position;
        position.x = position.x + (knockback * MovementControl.direction.x);
        gameObject.transform.position = position; */
    }
}
