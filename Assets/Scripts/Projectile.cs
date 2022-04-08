using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    AudioClip LaunchSound;
    [SerializeField]
    AudioClip ThunkSound;
    [SerializeField]
    AudioClip ScreechSound;

    [SerializeField]
    Sprite[] ProjectileSprites;

    bool isActive = false;
    float rotation = 0;
    Vector2 movementVector = new Vector2(0, 0);
    float gravity = -10f;
    SpriteRenderer spriteRenderer;
    float minY = -6f;
    float startY = -5.5f;
    float minStartX = -4.3f;
    float maxStartX = 4.3f;
    float initialVelocityMin = 11f;
    float initialVelocityMax = 15f;

    AudioSource audioSource;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GameObject.Find("SceneManager").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            movementVector.y += gravity * Time.deltaTime;
            GetComponent<Rigidbody2D>().velocity = movementVector;
            transform.Rotate(new Vector3(0, 0, rotation));
        }

        // game is over, remove all projectiles
        if (Globals.CurrentGameState == Globals.GameState.Dying)
        {
            Destroy(this.gameObject);
        }

        // projectile is off the bottom of the screen, remove
        if (transform.localPosition.y < minY)
        {
            Destroy(this.gameObject);
        }
    }

    public void Launch(float additionalVelocity, float xPos)
    {
        int itemNum = Random.Range(1, ProjectileSprites.Length);
        spriteRenderer.sprite = ProjectileSprites[itemNum];

        float startX = xPos != 0 ? xPos : Random.Range (minStartX, maxStartX);
        transform.localPosition = new Vector3 (startX, startY, 0);

        if (startX < 0)
        {
            movementVector.x = Random.Range (.1f, 1f);
            rotation = Random.Range (.1f, 2f);
        }
        else
        {
            movementVector.x = Random.Range (-1f, -.1f);
            rotation = Random.Range (-2f, -.1f);
        }

        movementVector.y = Random.Range (initialVelocityMin, initialVelocityMax) + additionalVelocity;

        isActive = true;
        audioSource.PlayOneShot(LaunchSound, 1f);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        Player player = collider.gameObject.GetComponent<Player>();
        if (player != null && Globals.CurrentGameState == Globals.GameState.Playing)
        {
            Camera camera = Camera.main;
            camera.GetComponent<CameraShake>().StartShake();
            audioSource.PlayOneShot(ThunkSound, 1f);
            audioSource.PlayOneShot(ScreechSound, 1f);
            Globals.CurrentGameState = Globals.GameState.Dying;

            Destroy(this.gameObject);
        }
    }
}
