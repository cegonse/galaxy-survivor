using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constants : MonoBehaviour
{
    public static Constants instance;
    public const bool DEBUG_MODE = true;

    /****************************************/

    public float PLAYER_MOVEMENT_SPEED = 4f;
    public float PLAYER_MOVEMENT_LIMIT = 6f;
    public float PLAYER_HITBOX_RADIUS = 0.66f;
    public float PLAYER_ROTATION_ANGLE = 10f;
    public float PLAYER_ROTATION_TIME = 0.2f;

    /****************************************/

    public float ASTEROID_TARGET_LIMIT = 10f;
    public float ASTEROID_PARTICLES_PER_HIT = 8f;
    public float ASTEROID_STAR_ANIMATION_TIME = 0.3f;
    public float ASTEROID_START_SPEED_MODIFIER = 0.2f;
    public float ASTEROID_STAR_SPEED_MODIFIER = 0.5f;

    /****************************************/

    public float BACKGROUND_POSITION_LIMIT = 15f;
    public float GAME_ACCELERATION = 0.1f;
    public float GAME_MAXIMUM_SPEED = 8f;
    public float GAME_START_SPEED = 0.2f;
    public bool MUSIC_ON = true;
    public int CURRENT_COINS = 0;
    public float BEST_SCORE = 0f;

    /****************************************/

    public float ASTEROID_PARTICLE_SPEED = 4f;
    public float ASTEROID_PARTICLE_TIME = 0.3f;
    public float ASTEROID_PARTICLE_FADE_TIME = 0.3f;

    /****************************************/

    public int SKILL_LEVEL_COUNT = 10;
    public int SHIELD_LEVEL_COUNT = 4;

    /****************************************/

    public List<float> TIME_SKILL_COOLDOWN;
    public List<float> TIME_SKILL_DURATION;
    public List<int> TIME_SKILL_PRICE;
    public float TIME_SKILL_MODIFIER = 0.25f;

    /****************************************/

    public List<float> BURST_SKILL_COOLDOWN;
    public List<float> BURST_SKILL_DURATION;
    public List<int> BURST_SKILL_PRICE;
    public float BURST_SKILL_MODIFIER = 3f;

    /****************************************/

    public List<float> BOMB_SKILL_COOLDOWN;
    public List<int> BOMB_SKILL_PRICE;

    /****************************************/

    public List<float> REPAIR_SKILL_COOLDOWN;
    public List<int> REPAIR_SKILL_PRICE;

    /****************************************/

    public List<int> SHIELD_HITS;
    public List<int> SHIELD_PRICE;

    /****************************************/

    public string TIME_SKILL_SHOP_DESCRIPTION = "Slows down the speed of the obstacles.";
    public string BOMB_SKILL_SHOP_DESCRIPTION = "Destroys all the obstacles around you.";
    public string SHIELD_SKILL_SHOP_DESCRIPTION = "Gives the ship an additional hit point.";
    public string REPAIR_SKILL_SHOP_DESCRIPTION = "Repair your ship for one hit point.";
    public string BURST_SKILL_SHOP_DESCRIPTION = "Boosts your ship, making it undestructible.";

    /****************************************/

    public void OnMusicToggle(bool on)
    {
        PlayerPrefs.SetInt("IS_MUSIC_ON", on == true ? 1 : 0);
        MUSIC_ON = on;
    }

    public void OnUpdateCoins(int coins)
    {
        PlayerPrefs.SetInt("CURRENT_COINS", coins);
        CURRENT_COINS = coins;
    }

    public void OnUpdateBestScore(float score)
    {
        PlayerPrefs.SetFloat("BEST_SCORE", score);
        BEST_SCORE = score;
    }

    void Start()
    {
        GameObject.DontDestroyOnLoad(gameObject);
        instance = this;

        if (!PlayerPrefs.HasKey("BOMB_SKILL_CURRENT_LEVEL"))
        {
            PlayerPrefs.SetInt("BOMB_SKILL_CURRENT_LEVEL", 0);
        }

        if (!PlayerPrefs.HasKey("TIME_SKILL_CURRENT_LEVEL"))
        {
            PlayerPrefs.SetInt("TIME_SKILL_CURRENT_LEVEL", 0);
        }

        if (!PlayerPrefs.HasKey("REPAIR_SKILL_CURRENT_LEVEL"))
        {
            PlayerPrefs.SetInt("REPAIR_SKILL_CURRENT_LEVEL", 0);
        }

        if (!PlayerPrefs.HasKey("BURST_SKILL_CURRENT_LEVEL"))
        {
            PlayerPrefs.SetInt("BURST_SKILL_CURRENT_LEVEL", 0);
        }

        if (!PlayerPrefs.HasKey("SHIELD_CURRENT_LEVEL"))
        {
            PlayerPrefs.SetInt("SHIELD_CURRENT_LEVEL", 0);
        }

        if (!PlayerPrefs.HasKey("CURRENT_COINS"))
        {
            PlayerPrefs.SetInt("CURRENT_COINS", 0);
        }
        else
        {
            CURRENT_COINS = PlayerPrefs.GetInt("CURRENT_COINS");
        }

        if (!PlayerPrefs.HasKey("IS_MUSIC_ON"))
        {
            PlayerPrefs.SetInt("IS_MUSIC_ON", 1);
        }
        else
        {
            if (PlayerPrefs.GetInt("IS_MUSIC_ON") == 0)
            {
                MUSIC_ON = false;
            }
        }

        if (!PlayerPrefs.HasKey("BEST_SCORE"))
        {
            PlayerPrefs.SetFloat("BEST_SCORE", 0f);
        }
        else
        {
            BEST_SCORE = PlayerPrefs.GetFloat("BEST_SCORE");
        }

        PlayerPrefs.SetInt("MENU_BACK_FROM_GAME", 0);

        TIME_SKILL_COOLDOWN = new List<float>();
        TIME_SKILL_COOLDOWN.Add(45);
        TIME_SKILL_COOLDOWN.Add(45);
        TIME_SKILL_COOLDOWN.Add(40);
        TIME_SKILL_COOLDOWN.Add(35);
        TIME_SKILL_COOLDOWN.Add(30);
        TIME_SKILL_COOLDOWN.Add(30);
        TIME_SKILL_COOLDOWN.Add(25);
        TIME_SKILL_COOLDOWN.Add(20);
        TIME_SKILL_COOLDOWN.Add(15);
        TIME_SKILL_COOLDOWN.Add(15);

        TIME_SKILL_DURATION = new List<float>();
        TIME_SKILL_DURATION.Add(2);
        TIME_SKILL_DURATION.Add(3);
        TIME_SKILL_DURATION.Add(3);
        TIME_SKILL_DURATION.Add(3);
        TIME_SKILL_DURATION.Add(3);
        TIME_SKILL_DURATION.Add(4);
        TIME_SKILL_DURATION.Add(4);
        TIME_SKILL_DURATION.Add(4);
        TIME_SKILL_DURATION.Add(4);
        TIME_SKILL_DURATION.Add(5);

        TIME_SKILL_PRICE = new List<int>();
        TIME_SKILL_PRICE.Add(50);
        TIME_SKILL_PRICE.Add(75);
        TIME_SKILL_PRICE.Add(150);
        TIME_SKILL_PRICE.Add(175);
        TIME_SKILL_PRICE.Add(200);
        TIME_SKILL_PRICE.Add(250);
        TIME_SKILL_PRICE.Add(300);
        TIME_SKILL_PRICE.Add(400);
        TIME_SKILL_PRICE.Add(600);
        TIME_SKILL_PRICE.Add(800);

        /****************************************/

        BURST_SKILL_COOLDOWN = new List<float>();
        BURST_SKILL_COOLDOWN.Add(55);
        BURST_SKILL_COOLDOWN.Add(50);
        BURST_SKILL_COOLDOWN.Add(45);
        BURST_SKILL_COOLDOWN.Add(40);
        BURST_SKILL_COOLDOWN.Add(35);
        BURST_SKILL_COOLDOWN.Add(35);
        BURST_SKILL_COOLDOWN.Add(30);
        BURST_SKILL_COOLDOWN.Add(25);
        BURST_SKILL_COOLDOWN.Add(20);
        BURST_SKILL_COOLDOWN.Add(20);

        BURST_SKILL_DURATION = new List<float>();
        BURST_SKILL_DURATION.Add(2);
        BURST_SKILL_DURATION.Add(2);
        BURST_SKILL_DURATION.Add(2);
        BURST_SKILL_DURATION.Add(2);
        BURST_SKILL_DURATION.Add(2);
        BURST_SKILL_DURATION.Add(3);
        BURST_SKILL_DURATION.Add(3);
        BURST_SKILL_DURATION.Add(3);
        BURST_SKILL_DURATION.Add(3);
        BURST_SKILL_DURATION.Add(4);

        BURST_SKILL_PRICE = new List<int>();
        BURST_SKILL_PRICE.Add(50);
        BURST_SKILL_PRICE.Add(75);
        BURST_SKILL_PRICE.Add(150);
        BURST_SKILL_PRICE.Add(175);
        BURST_SKILL_PRICE.Add(200);
        BURST_SKILL_PRICE.Add(250);
        BURST_SKILL_PRICE.Add(300);
        BURST_SKILL_PRICE.Add(400);
        BURST_SKILL_PRICE.Add(600);
        BURST_SKILL_PRICE.Add(800);

        /****************************************/

        BOMB_SKILL_COOLDOWN = new List<float>();
        BOMB_SKILL_COOLDOWN.Add(52);
        BOMB_SKILL_COOLDOWN.Add(48);
        BOMB_SKILL_COOLDOWN.Add(44);
        BOMB_SKILL_COOLDOWN.Add(40);
        BOMB_SKILL_COOLDOWN.Add(36);
        BOMB_SKILL_COOLDOWN.Add(32);
        BOMB_SKILL_COOLDOWN.Add(28);
        BOMB_SKILL_COOLDOWN.Add(24);
        BOMB_SKILL_COOLDOWN.Add(20);
        BOMB_SKILL_COOLDOWN.Add(16);

        BOMB_SKILL_PRICE = new List<int>();
        BOMB_SKILL_PRICE.Add(50);
        BOMB_SKILL_PRICE.Add(75);
        BOMB_SKILL_PRICE.Add(150);
        BOMB_SKILL_PRICE.Add(175);
        BOMB_SKILL_PRICE.Add(200);
        BOMB_SKILL_PRICE.Add(250);
        BOMB_SKILL_PRICE.Add(300);
        BOMB_SKILL_PRICE.Add(400);
        BOMB_SKILL_PRICE.Add(600);
        BOMB_SKILL_PRICE.Add(800);

        /****************************************/

        REPAIR_SKILL_COOLDOWN = new List<float>();
        REPAIR_SKILL_COOLDOWN.Add(70);
        REPAIR_SKILL_COOLDOWN.Add(65);
        REPAIR_SKILL_COOLDOWN.Add(60);
        REPAIR_SKILL_COOLDOWN.Add(55);
        REPAIR_SKILL_COOLDOWN.Add(50);
        REPAIR_SKILL_COOLDOWN.Add(45);
        REPAIR_SKILL_COOLDOWN.Add(40);
        REPAIR_SKILL_COOLDOWN.Add(35);
        REPAIR_SKILL_COOLDOWN.Add(30);
        REPAIR_SKILL_COOLDOWN.Add(20);

        REPAIR_SKILL_PRICE = new List<int>();
        REPAIR_SKILL_PRICE.Add(50);
        REPAIR_SKILL_PRICE.Add(75);
        REPAIR_SKILL_PRICE.Add(150);
        REPAIR_SKILL_PRICE.Add(175);
        REPAIR_SKILL_PRICE.Add(200);
        REPAIR_SKILL_PRICE.Add(250);
        REPAIR_SKILL_PRICE.Add(300);
        REPAIR_SKILL_PRICE.Add(400);
        REPAIR_SKILL_PRICE.Add(600);
        REPAIR_SKILL_PRICE.Add(800);

        /****************************************/

        SHIELD_HITS = new List<int>();
        SHIELD_HITS.Add(3);
        SHIELD_HITS.Add(4);
        SHIELD_HITS.Add(5);
        SHIELD_HITS.Add(6);

        SHIELD_PRICE = new List<int>();
        SHIELD_PRICE.Add(0);
        SHIELD_PRICE.Add(100);
        SHIELD_PRICE.Add(500);
        SHIELD_PRICE.Add(1000);

        /****************************************/

        UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
    }
}
