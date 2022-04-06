using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SceneManager : MonoBehaviour
{
    [SerializeField]
    GameObject Player;
    [SerializeField]
    CameraZoom CameraZoomScript;
    [SerializeField]
    GameObject HUDScore;
    [SerializeField]
    GameObject HUDTitle;
    [SerializeField]
    GameObject HUDInstructions;
    [SerializeField]
    GameObject HUDGameOver;
    [SerializeField]
    GameObject HUDFinalScore;
    [SerializeField]
    GameObject HUDPlayAgain;

    [SerializeField]
    GameObject projectilePrefab;

    float projectile1LaunchTimer = .5f;
    float projectile2LaunchTimer = 5f;
    float projectile3LaunchTimer = 7f;
    float projectile4LaunchTimer = 9f;
    float projectile1LaunchTimerMax = 2f;
    float projectile2LaunchTimerMax = 5f;
    float projectile3LaunchTimerMax = 7f;
    float projectile4LaunchTimerMax = 9f;

    float speedTimer = 5f;
    float speedTimerMax = 5f;
    float additionalVelocity = 0f;
    float maxAdditionalVelocity = 10f;
    float launchSpeedup = 0f;
    float maxLaunchSpeedup = 3f;

    bool scoreUpdated = false;

    [SerializeField]
    MobileButton ButtonLeft;
    [SerializeField]
    MobileButton ButtonRight;

    void Awake()
    {
        CameraZoomScript.SetOrthographicSize();

        Globals.BestTime = Globals.LoadFromPlayerPrefs(Globals.BestTimePlayerPrefsKey);
    }

    // Update is called once per frame
    void Update()
    {
        if (Globals.CurrentGameState == Globals.GameState.TitleScreen)
        {
            UpdateTitleScreenState();
        }
        else if (Globals.CurrentGameState == Globals.GameState.Playing)
        {
            UpdatePlayingState();
        }
        else if (Globals.CurrentGameState == Globals.GameState.ShowScoreAllowRestart)
        {
            UpdateShowScoreRestart();
        }
    }

    void UpdateTitleScreenState()
    {
        GetInput();
    }

    void UpdatePlayingState()
    {
        Globals.CurrentTime += Time.deltaTime;
        HUDScore.GetComponent<TextMeshPro>().text = Globals.CurrentTime.ToString("F2");

        projectile1LaunchTimer -= Time.deltaTime;
        if (projectile1LaunchTimer <= 0)
        {
            LaunchProjectile(Random.Range(0, 3) == 2);
            projectile1LaunchTimer = projectile1LaunchTimerMax + Random.Range (-.5f, .5f);
        }
        if (projectile2LaunchTimer <= 0)
        {
            LaunchProjectile(false);
            projectile2LaunchTimer = projectile2LaunchTimerMax + Random.Range (-.5f, .5f) - launchSpeedup;
        }
        if (projectile3LaunchTimer <= 0)
        {
            LaunchProjectile(false);
            projectile3LaunchTimer = projectile3LaunchTimerMax + Random.Range (-.5f, .5f) - launchSpeedup;
        }
        if (projectile4LaunchTimer <= 0)
        {
            LaunchProjectile(false);
            projectile4LaunchTimer = projectile4LaunchTimerMax + Random.Range (-.5f, .5f) - launchSpeedup;
        }

        speedTimer -= Time.deltaTime;
        if (speedTimer <= 0)
        {
            additionalVelocity = Mathf.Min(maxAdditionalVelocity, additionalVelocity + 1f);
            speedTimer = speedTimerMax;
            launchSpeedup = Mathf.Min(maxLaunchSpeedup, launchSpeedup + .5f);
        }
    }

    void UpdateShowScoreRestart()
    {
        if (!scoreUpdated)
        {
            if (Globals.CurrentTime > Globals.BestTime)
            {
                Globals.BestTime = Globals.CurrentTime;
                Globals.SaveToPlayerPrefs(Globals.BestTimePlayerPrefsKey, Globals.BestTime);
            }
            HUDFinalScore.GetComponent<TextMeshPro>().text = "You hung in there for " + Globals.CurrentTime.ToString("F2") + " seconds\n";
            HUDFinalScore.GetComponent<TextMeshPro>().text += "Your best time is " + Globals.BestTime.ToString("F2") + " seconds";
            DisplayGameOverHUD();
            scoreUpdated = true;
        }
        GetInput();
    }

    void StartGame()
    {
        Globals.CurrentTime = 0;
        DisplayGamePlayingHUD();

        // reset game variables
        additionalVelocity = 0f;
        launchSpeedup = 0f;
        speedTimer = speedTimerMax;

        projectile1LaunchTimer = .5f;
        projectile2LaunchTimer = projectile2LaunchTimerMax;
        projectile3LaunchTimer = projectile3LaunchTimerMax;
        projectile4LaunchTimer = projectile4LaunchTimerMax;

        Player.transform.localPosition = new Vector2(-2f, 2f);

        scoreUpdated = false;

        ButtonLeft.Clear();
        ButtonRight.Clear();

        Globals.CurrentGameState = Globals.GameState.Playing;
    }

    void DisplayGamePlayingHUD()
    {
        HUDTitle.SetActive(false);
        HUDFinalScore.SetActive(false);
        HUDGameOver.SetActive(false);
        HUDInstructions.SetActive(false);
        HUDPlayAgain.SetActive(false);
        HUDScore.SetActive(true);
    }
    void DisplayGameOverHUD()
    {
        HUDTitle.SetActive(false);
        HUDFinalScore.SetActive(true);
        HUDGameOver.SetActive(true);
        HUDInstructions.SetActive(false);
        HUDPlayAgain.SetActive(true);
        HUDScore.SetActive(false);
    }

    void GetInput()
    {
        if (Input.GetKeyDown("space") || Input.GetButtonDown("Fire1") || Input.GetButtonDown("Fire2"))
        {
            StartGame();
        }
    }

    void LaunchProjectile(bool targetPlayer)
    {
        GameObject projectileGameObject = (GameObject)Instantiate(projectilePrefab, new Vector3(0, -50f, 0), Quaternion.identity);
        Projectile projectile = projectileGameObject.GetComponent<Projectile>();
        projectile.Launch(additionalVelocity, targetPlayer ? Player.transform.localPosition.x : 0);
    }
}
