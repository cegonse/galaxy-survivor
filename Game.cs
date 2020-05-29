using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Game : MonoBehaviour
{
    public const string _playStoreLink = "http://play.google.com/store/apps/details?id=com.us.galaxy.survivor";
    public static Game instance;

    /****************************************/

    public float GAME_CURRENT_SPEED = 1f;
    public float GAME_NORMAL_SPEED = 1f;

    public int BOMB_SKILL_CURRENT_LEVEL = 1;
    public int TIME_SKILL_CURRENT_LEVEL = 1;
    public int BURST_SKILL_CURRENT_LEVEL = 1;
    public int REPAIR_SKILL_CURRENT_LEVEL = 1;
    public int SHIELD_CURRENT_LEVEL = 1;

    /****************************************/

    public GameObject _asteroidPrefab;
    public GameObject _starPrefab;
    public GameObject _playerInstance;

    // UI widgets
    public Text _scoreLabel;
    public Image _blackOverlay;
    public GameObject _menuButton;
    public GameObject _resumeButton;
    public GameObject _pauseMenu;

    public Image _bombSkillFill;
    public Image _repairSkillFill;
    public Image _timeSkillFill;
    public Image _speedSkillFill;

    public Button _bombButton;
    public Button _repairButton;
    public Button _timeButton;
    public Button _speedButton;

    public GameObject _bombWidget;
    public GameObject _repairWidget;
    public GameObject _timeWidget;
    public GameObject _speedWidget;

    public GameObject _starCounter;
    private Vector3 _starCounterOriginalPosition;
    public Text _starCounterText;

    public Text _deathMenuCoins;
    public Text _deathMenuStars;
    public Text _deathMenuDistance;
    public Text _deathMenuBest;

    public GameObject _hp12Widget;
    public GameObject _hp22Widget;

    public GameObject _hp13Widget;
    public GameObject _hp23Widget;
    public GameObject _hp33Widget;

    public GameObject _hp14Widget;
    public GameObject _hp24Widget;
    public GameObject _hp34Widget;
    public GameObject _hp44Widget;

    public GameObject _hp15Widget;
    public GameObject _hp25Widget;
    public GameObject _hp35Widget;
    public GameObject _hp45Widget;
    public GameObject _hp55Widget;

    public Image _hpRedAlert;

    public GameObject _deathMenu;
    public Image _deathBlackOverlay;

    public Text _debugSpeedLabel;
    public Text _debugBombSkillLabel;
    public Text _debugTimeSkillLabel;
    public Text _debugRepairSkillLabel;
    public Text _debugBurstSkillLabel;
    public Text _debugShieldLevelLabel;

    public BurstAnimationController _burstAnimation;

    public GameObject _bombSkillOff;
    public GameObject _timeSkillOff;
    public GameObject _burstSkillOff;
    public GameObject _repairSkillOff;

    public AudioClip _menuClick;
    public AudioClip _burstAudio;
    public AudioClip _starPickupAudio;
    public AudioClip _hitAudio;

    /****************************************/

    private List<Asteroid> _asteroids;
    private List<Asteroid> _stars;
    private GameObject[] _backgrounds;

    /****************************************/

    private float _score = 0;
    private int _starCount = 0;
    private int _hp = 3;
    private bool _highSpeed = false;
    private float _lastSpeed = 0f;

    private Tweener _hpRedAlertTweener;
    private bool _paused = false;
    private bool _dead = false;

    /****************************************/

    public void OnGoToMainMenu()
    {
        if (Constants.instance.MUSIC_ON)
        {
            AudioSource.PlayClipAtPoint(_menuClick, Camera.main.transform.position);
        }

        PlayerPrefs.SetInt("MENU_BACK_FROM_GAME", 1);
        UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
    }

    public void OnTryAgain()
    {
        if (Constants.instance.MUSIC_ON)
        {
            AudioSource.PlayClipAtPoint(_menuClick, Camera.main.transform.position);
        }

        UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
    }

    public void OnMenu()
    {
        _paused = true;

        if (Constants.instance.MUSIC_ON)
        {
            AudioSource.PlayClipAtPoint(_menuClick, Camera.main.transform.position);
        }

        // Tell the player to pause
        Player.instance.OnPause(true);

        // Tell the entities to pause
        for (int i = 0; i < _asteroids.Count; ++i)
        {
            _asteroids[i].OnPause(true);
        }

        for (int i = 0; i < _stars.Count; ++i)
        {
            _stars[i].OnPause(true);
        }

        for (int i = 0; i < _backgrounds.Length; ++i)
        {
            _backgrounds[i].GetComponent<Background>().OnPause(true);
        }

        _pauseMenu.SetActive(true);
        _blackOverlay.gameObject.SetActive(true);
        _blackOverlay.DOFade(0.5f, 0.3f);

        _menuButton.transform.DOMoveX(Screen.width * 0.5f, 0.2f);
        _resumeButton.transform.DOMoveX(Screen.width * 0.5f, 0.3f);
    }

    public void OnMenuClose()
    {
        _paused = false;

        if (Constants.instance.MUSIC_ON)
        {
            AudioSource.PlayClipAtPoint(_menuClick, Camera.main.transform.position);
        }

        // Tell the player to unpause
        Player.instance.OnPause(false);

        // Tell the entities to unpause
        for (int i = 0; i < _asteroids.Count; ++i)
        {
            _asteroids[i].OnPause(false);
        }

        for (int i = 0; i < _stars.Count; ++i)
        {
            _stars[i].OnPause(false);
        }

        for (int i = 0; i < _backgrounds.Length; ++i)
        {
            _backgrounds[i].GetComponent<Background>().OnPause(false);
        }

        _blackOverlay.DOFade(0f, 0.3f).OnComplete(()=>
        {
            _blackOverlay.gameObject.SetActive(false);
            _pauseMenu.SetActive(false);
        });

        _menuButton.transform.DOMoveX(-Screen.width, 0.2f);
        _resumeButton.transform.DOMoveX(-Screen.width, 0.3f);
    }

    public void OnBombSkill()
    {
        // Execute skill
        _playerInstance.GetComponent<Player>().OnBombSkill();

        // Play fill effect
        _bombSkillFill.fillAmount = 0f;
        _bombButton.enabled = false;

        _bombSkillFill.DOFillAmount(0.9f, Constants.instance.BOMB_SKILL_COOLDOWN[Game.instance.BOMB_SKILL_CURRENT_LEVEL]).OnComplete(()=>
        {
            _bombSkillFill.fillAmount = 1f;
            _bombButton.enabled = true;
            _bombWidget.transform.DOPunchScale(Vector3.one * 0.5f, 0.3f, 3, 0.5f);
        });
    }

    public void OnRepairSkill()
    {
        // Execute skill
        _playerInstance.GetComponent<Player>().OnRepairSkill();
        int maxHp = Constants.instance.SHIELD_HITS[Game.instance.SHIELD_CURRENT_LEVEL];

        if (_hp != maxHp)
        {
            _hp++;
        }

        if (maxHp == 3)
        {
            if (_hp == 3)
            {
                _hp12Widget.SetActive(false);
                _hp22Widget.SetActive(false);
            }
            else if (_hp == 2)
            {
                _hp12Widget.SetActive(true);
                _hp22Widget.SetActive(false);
            }
        }
        else if (maxHp == 4)
        {
            if (_hp == 4)
            {
                _hp13Widget.SetActive(false);
                _hp23Widget.SetActive(false);
                _hp33Widget.SetActive(false);
            }
            else if (_hp == 3)
            {
                _hp13Widget.SetActive(true);
                _hp23Widget.SetActive(false);
                _hp33Widget.SetActive(false);
            }
            else if (_hp == 2)
            {
                _hp13Widget.SetActive(true);
                _hp23Widget.SetActive(true);
                _hp33Widget.SetActive(false);
            }
        }
        else if (maxHp == 5)
        {
            if (_hp == 5)
            {
                _hp14Widget.SetActive(false);
                _hp24Widget.SetActive(false);
                _hp34Widget.SetActive(false);
                _hp44Widget.SetActive(false);
            }
            else if (_hp == 4)
            {
                _hp14Widget.SetActive(true);
                _hp24Widget.SetActive(false);
                _hp34Widget.SetActive(false);
                _hp44Widget.SetActive(false);
            }
            if (_hp == 3)
            {
                _hp14Widget.SetActive(true);
                _hp24Widget.SetActive(true);
                _hp34Widget.SetActive(false);
                _hp44Widget.SetActive(false);
            }
            if (_hp == 2)
            {
                _hp14Widget.SetActive(true);
                _hp24Widget.SetActive(true);
                _hp34Widget.SetActive(true);
                _hp44Widget.SetActive(false);
            }
        }
        else if (maxHp == 6)
        {
            if (_hp == 6)
            {
                _hp15Widget.SetActive(false);
                _hp25Widget.SetActive(false);
                _hp35Widget.SetActive(false);
                _hp45Widget.SetActive(false);
                _hp55Widget.SetActive(false);
            }
            else if (_hp == 5)
            {
                _hp15Widget.SetActive(true);
                _hp25Widget.SetActive(false);
                _hp35Widget.SetActive(false);
                _hp45Widget.SetActive(false);
                _hp55Widget.SetActive(false);
            }
            else if (_hp == 4)
            {
                _hp15Widget.SetActive(true);
                _hp25Widget.SetActive(true);
                _hp35Widget.SetActive(false);
                _hp45Widget.SetActive(false);
                _hp55Widget.SetActive(false);
            }
            else if (_hp == 3)
            {
                _hp15Widget.SetActive(true);
                _hp25Widget.SetActive(true);
                _hp35Widget.SetActive(true);
                _hp45Widget.SetActive(false);
                _hp55Widget.SetActive(false);
            }
            else if (_hp == 2)
            {
                _hp15Widget.SetActive(true);
                _hp25Widget.SetActive(true);
                _hp35Widget.SetActive(true);
                _hp45Widget.SetActive(true);
                _hp55Widget.SetActive(false);
            }
        }

        if (_hpRedAlertTweener != null)
        {
            _hpRedAlertTweener.Kill();

            Color cl = _hpRedAlert.color;
            cl.a = 0f;
            _hpRedAlert.color = cl;
        }

        // Play fill effect
        _repairSkillFill.fillAmount = 0f;
        _repairButton.enabled = false;

        _repairSkillFill.DOFillAmount(0.9f, Constants.instance.REPAIR_SKILL_COOLDOWN[Game.instance.REPAIR_SKILL_CURRENT_LEVEL]).OnComplete(() =>
        {
            _repairSkillFill.fillAmount = 1f;
            _repairButton.enabled = true;
            _repairWidget.transform.DOPunchScale(Vector3.one * 0.5f, 0.3f, 3, 0.5f);
        });
    }

    public void OnBurstSkill()
    {
        if (Constants.instance.MUSIC_ON)
        {
            AudioSource.PlayClipAtPoint(_burstAudio, Camera.main.transform.position);
        }

        // Execute skill
        SetGameSpeed(Constants.instance.BURST_SKILL_MODIFIER, Constants.instance.BURST_SKILL_DURATION[Game.instance.BURST_SKILL_CURRENT_LEVEL]);
        _highSpeed = true;

        Camera.main.transform.DOShakePosition(Constants.instance.BURST_SKILL_DURATION[Game.instance.BURST_SKILL_CURRENT_LEVEL], 0.1f, 90).OnComplete(() =>
        {
            _highSpeed = false;
        });

        // Play fill effect
        _speedSkillFill.fillAmount = 0f;
        _speedButton.enabled = false;
        _burstAnimation.StartAnimation();

        _speedSkillFill.DOFillAmount(0.9f, Constants.instance.BURST_SKILL_COOLDOWN[Game.instance.BURST_SKILL_CURRENT_LEVEL]).OnComplete(() =>
        {
            _speedSkillFill.fillAmount = 1f;
            _speedButton.enabled = true;
            _speedWidget.transform.DOPunchScale(Vector3.one * 0.5f, 0.3f, 3, 0.5f);
        });
    }

    public void OnTimeSkill()
    {
        // Execute skill
        SetGameSpeed(Constants.instance.TIME_SKILL_MODIFIER, Constants.instance.TIME_SKILL_DURATION[Game.instance.TIME_SKILL_CURRENT_LEVEL]);

        // Play fill effect
        _timeSkillFill.fillAmount = 0f;
        _timeButton.enabled = false;

        _timeSkillFill.DOFillAmount(0.9f, Constants.instance.TIME_SKILL_COOLDOWN[Game.instance.TIME_SKILL_CURRENT_LEVEL]).OnComplete(() =>
        {
            _timeSkillFill.fillAmount = 1f;
            _timeWidget.transform.DOPunchScale(Vector3.one * 0.5f, 0.3f, 3, 0.5f);
            _timeButton.enabled = true;
        });
    }

    public void OnStar()
    {
        if (Constants.instance.MUSIC_ON)
        {
            AudioSource.PlayClipAtPoint(_starPickupAudio, Camera.main.transform.position, 0.75f);
        }

        _starCount++;
        _starCounterText.text = _starCount.ToString();

        _starCounter.transform.DOMoveY(Screen.height * 0.5f, 0.3f).OnComplete(() =>
        {
            StartCoroutine(OnStarCountHide());
        });
    }

    public void OnAsteroidHit()
    {
        if (!_highSpeed)
        {
            if (Constants.instance.MUSIC_ON)
            {
                AudioSource.PlayClipAtPoint(_hitAudio, Camera.main.transform.position, 0.75f);
            }

            // Play screenshake
            Camera.main.transform.DOShakePosition(0.3f, 0.3f, 60);

            // Remove HP
            _hp--;

            // Handle the effect
            if (_hp == 1)
            {
                OnRedAlert();
            }
            else if (_hp == 0)
            {
                OnDeath();
            }

            // Update the UI
            int maxHp = Constants.instance.SHIELD_HITS[SHIELD_CURRENT_LEVEL];

            if (maxHp == 3)
            {
                if (_hp == 2)
                {
                    _hp12Widget.SetActive(true);
                }
                else if (_hp == 1)
                {
                    _hp22Widget.SetActive(true);
                }
            }
            else if (maxHp == 4)
            {
                if (_hp == 3)
                {
                    _hp13Widget.SetActive(true);
                }
                else if (_hp == 2)
                {
                    _hp23Widget.SetActive(true);
                }
                else if (_hp == 1)
                {
                    _hp33Widget.SetActive(true);
                }
            }
            else if (maxHp == 5)
            {
                if (_hp == 4)
                {
                    _hp14Widget.SetActive(true);
                }
                else if (_hp == 3)
                {
                    _hp24Widget.SetActive(true);
                }
                else if (_hp == 2)
                {
                    _hp34Widget.SetActive(true);
                }
                else if (_hp == 1)
                {
                    _hp44Widget.SetActive(true);
                }
            }
            else if (maxHp == 6)
            {
                if (_hp == 5)
                {
                    _hp15Widget.SetActive(true);
                }
                else if (_hp == 4)
                {
                    _hp25Widget.SetActive(true);
                }
                else if (_hp == 3)
                {
                    _hp35Widget.SetActive(true);
                }
                else if (_hp == 2)
                {
                    _hp45Widget.SetActive(true);
                }
                else if (_hp == 1)
                {
                    _hp55Widget.SetActive(true);
                }
            }
        }
    }

    public void OnTryAgainSharePressed()
    {
        string subject = "Play Galaxy Survivor with me";
        string body = "I scored " + _score.ToString("0.0") + " points in Galaxy Survivor, wanna join? - " + _playStoreLink;

#if UNITY_ANDROID
        //Reference of AndroidJavaClass class for intent
        AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
        //Reference of AndroidJavaObject class for intent
        AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent");
        //call setAction method of the Intent object created
        intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_SEND"));
        //set the type of sharing that is happening
        intentObject.Call<AndroidJavaObject>("setType", "text/plain");
        //add data to be passed to the other activity i.e., the data to be sent
        intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_SUBJECT"), subject);
        intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"), body);
        //get the current activity
        AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");
        //start the activity by sending the intent data
        AndroidJavaObject jChooser = intentClass.CallStatic<AndroidJavaObject>("createChooser", intentObject, "Share Via");
        currentActivity.Call("startActivity", jChooser);
#endif
    }

    public void SetGameSpeed(float modifier, float duration)
    {
        _lastSpeed = GAME_CURRENT_SPEED;

        DOTween.To(() => GAME_CURRENT_SPEED, x => GAME_CURRENT_SPEED = x, GAME_CURRENT_SPEED * modifier, 0.5f).OnComplete(() =>
        {
            StartCoroutine(OnGameSpeedChange(duration));
        });
    }

    public void OnDebugBombSkillLevelUp()
    {
        if (BOMB_SKILL_CURRENT_LEVEL != Constants.instance.SKILL_LEVEL_COUNT - 1)
        {
            BOMB_SKILL_CURRENT_LEVEL++;
            PlayerPrefs.SetInt("BOMB_SKILL_CURRENT_LEVEL", BOMB_SKILL_CURRENT_LEVEL);
            _debugBombSkillLabel.text = "BOMB: " + (1 + BOMB_SKILL_CURRENT_LEVEL).ToString();
        }
    }

    public void OnDebugBurstSkillLevelUp()
    {
        if (BURST_SKILL_CURRENT_LEVEL != Constants.instance.SKILL_LEVEL_COUNT - 1)
        {
            BURST_SKILL_CURRENT_LEVEL++;
            PlayerPrefs.SetInt("BURST_SKILL_CURRENT_LEVEL", BURST_SKILL_CURRENT_LEVEL);
            _debugBurstSkillLabel.text = "BURST: " + (1 + BURST_SKILL_CURRENT_LEVEL).ToString();
        }
    }

    public void OnDebugRepairSkillLevelUp()
    {
        if (REPAIR_SKILL_CURRENT_LEVEL != Constants.instance.SKILL_LEVEL_COUNT - 1)
        {
            REPAIR_SKILL_CURRENT_LEVEL++;
            PlayerPrefs.SetInt("REPAIR_SKILL_CURRENT_LEVEL", REPAIR_SKILL_CURRENT_LEVEL);
            _debugRepairSkillLabel.text = "REPAIR: " + (1 + REPAIR_SKILL_CURRENT_LEVEL).ToString();
        }
    }

    public void OnDebugShieldLevelUp()
    {
        if (SHIELD_CURRENT_LEVEL != Constants.instance.SHIELD_LEVEL_COUNT - 1)
        {
            SHIELD_CURRENT_LEVEL++;
            PlayerPrefs.SetInt("SHIELD_CURRENT_LEVEL", SHIELD_CURRENT_LEVEL);
            _debugShieldLevelLabel.text = "SHIELD: " + (1 + SHIELD_CURRENT_LEVEL).ToString();

            int maxHp = Constants.instance.SHIELD_HITS[SHIELD_CURRENT_LEVEL];
            _hp = maxHp;

            if (maxHp == 3)
            {
                _hp12Widget.SetActive(false);
                _hp22Widget.SetActive(false);
            }
            else if (maxHp == 4)
            {
                _hp13Widget.SetActive(false);
                _hp23Widget.SetActive(false);
                _hp33Widget.SetActive(false);
            }
            else if (maxHp == 5)
            {
                _hp14Widget.SetActive(false);
                _hp24Widget.SetActive(false);
                _hp34Widget.SetActive(false);
                _hp44Widget.SetActive(false);
            }
            else if (maxHp == 6)
            {
                _hp15Widget.SetActive(false);
                _hp25Widget.SetActive(false);
                _hp35Widget.SetActive(false);
                _hp45Widget.SetActive(false);
                _hp55Widget.SetActive(false);
            }
        }
    }

    public void OnDebugTimeSkillLevelUp()
    {
        if (TIME_SKILL_CURRENT_LEVEL != Constants.instance.SKILL_LEVEL_COUNT - 1)
        {
            TIME_SKILL_CURRENT_LEVEL++;
            PlayerPrefs.SetInt("TIME_SKILL_CURRENT_LEVEL", TIME_SKILL_CURRENT_LEVEL);
            _debugTimeSkillLabel.text = "TIME: " + (1 + TIME_SKILL_CURRENT_LEVEL).ToString();
        }
    }

    public void OnDebugBombSkillLevelDown()
    {
        if (BOMB_SKILL_CURRENT_LEVEL != 0)
        {
            BOMB_SKILL_CURRENT_LEVEL--;
            PlayerPrefs.SetInt("BOMB_SKILL_CURRENT_LEVEL", BOMB_SKILL_CURRENT_LEVEL);
            _debugBombSkillLabel.text = "BOMB: " + (1 + BOMB_SKILL_CURRENT_LEVEL).ToString();
        }
    }

    public void OnDebugBurstSkillLevelDown()
    {
        if (BURST_SKILL_CURRENT_LEVEL != 0)
        {
            BURST_SKILL_CURRENT_LEVEL--;
            PlayerPrefs.SetInt("BURST_SKILL_CURRENT_LEVEL", BURST_SKILL_CURRENT_LEVEL);
            _debugBurstSkillLabel.text = "BURST: " + (1 + BURST_SKILL_CURRENT_LEVEL).ToString();
        }
    }

    public void OnDebugRepairSkillLevelDown()
    {
        if (REPAIR_SKILL_CURRENT_LEVEL != 0)
        {
            REPAIR_SKILL_CURRENT_LEVEL--;
            PlayerPrefs.SetInt("REPAIR_SKILL_CURRENT_LEVEL", REPAIR_SKILL_CURRENT_LEVEL);
            _debugRepairSkillLabel.text = "REPAIR: " + (1 + REPAIR_SKILL_CURRENT_LEVEL).ToString();
        }
    }

    public void OnDebugTimeSkillLevelDown()
    {
        if (TIME_SKILL_CURRENT_LEVEL != 0)
        {
            TIME_SKILL_CURRENT_LEVEL--;
            PlayerPrefs.SetInt("TIME_SKILL_CURRENT_LEVEL", TIME_SKILL_CURRENT_LEVEL);
            _debugTimeSkillLabel.text = "TIME: " + (1 + TIME_SKILL_CURRENT_LEVEL).ToString();
        }
    }

    public void OnDebugShieldLevelDown()
    {
        if (SHIELD_CURRENT_LEVEL != 0)
        {
            SHIELD_CURRENT_LEVEL--;
            PlayerPrefs.SetInt("SHIELD_CURRENT_LEVEL", SHIELD_CURRENT_LEVEL);
            _debugShieldLevelLabel.text = "SHIELD: " + (1 + SHIELD_CURRENT_LEVEL).ToString();

            int maxHp = Constants.instance.SHIELD_HITS[SHIELD_CURRENT_LEVEL];
            _hp = maxHp;

            if (maxHp == 3)
            {
                _hp12Widget.SetActive(false);
                _hp22Widget.SetActive(false);
            }
            else if (maxHp == 4)
            {
                _hp13Widget.SetActive(false);
                _hp23Widget.SetActive(false);
                _hp33Widget.SetActive(false);
            }
            else if (maxHp == 5)
            {
                _hp14Widget.SetActive(false);
                _hp24Widget.SetActive(false);
                _hp34Widget.SetActive(false);
                _hp44Widget.SetActive(false);
            }
            else if (maxHp == 6)
            {
                _hp15Widget.SetActive(false);
                _hp25Widget.SetActive(false);
                _hp35Widget.SetActive(false);
                _hp45Widget.SetActive(false);
                _hp55Widget.SetActive(false);
            }
        }
    }

    /****************************************/

    void Start()
    {
        instance = this;

        BOMB_SKILL_CURRENT_LEVEL = PlayerPrefs.GetInt("BOMB_SKILL_CURRENT_LEVEL");
        TIME_SKILL_CURRENT_LEVEL = PlayerPrefs.GetInt("TIME_SKILL_CURRENT_LEVEL");
        BURST_SKILL_CURRENT_LEVEL = PlayerPrefs.GetInt("BURST_SKILL_CURRENT_LEVEL");
        REPAIR_SKILL_CURRENT_LEVEL = PlayerPrefs.GetInt("REPAIR_SKILL_CURRENT_LEVEL");
        SHIELD_CURRENT_LEVEL = PlayerPrefs.GetInt("SHIELD_CURRENT_LEVEL");
        GAME_CURRENT_SPEED = Constants.instance.GAME_START_SPEED;

        if (!Constants.instance.MUSIC_ON)
        {
            Camera.main.GetComponent<AudioSource>().volume = 0f;
        }

        if (BOMB_SKILL_CURRENT_LEVEL == 0)
        {
            _bombWidget.SetActive(false);
            _bombSkillOff.SetActive(true);
        }

        if (TIME_SKILL_CURRENT_LEVEL == 0)
        {
            _timeWidget.SetActive(false);
            _timeSkillOff.SetActive(true);
        }

        if (BURST_SKILL_CURRENT_LEVEL == 0)
        {
            _speedWidget.SetActive(false);
            _burstSkillOff.SetActive(true);
        }

        if (REPAIR_SKILL_CURRENT_LEVEL == 0)
        {
            _repairWidget.SetActive(false);
            _repairSkillOff.SetActive(true);
        }

        if (Constants.DEBUG_MODE)
        {
            _debugBombSkillLabel.text = "BOMB: " + (1 + BOMB_SKILL_CURRENT_LEVEL).ToString();
            _debugBurstSkillLabel.text = "BURST: " + (1 + BURST_SKILL_CURRENT_LEVEL).ToString();
            _debugRepairSkillLabel.text = "REPAIR: " + (1 + REPAIR_SKILL_CURRENT_LEVEL).ToString();
            _debugTimeSkillLabel.text = "TIME: " + (1 + TIME_SKILL_CURRENT_LEVEL).ToString();
            _debugShieldLevelLabel.text = "SHIELD: " + (1 + SHIELD_CURRENT_LEVEL).ToString();
        }

        _asteroids = new List<Asteroid>();
        _stars = new List<Asteroid>();
        _backgrounds = GameObject.FindGameObjectsWithTag("Background");
        _starCounterOriginalPosition = _starCounter.transform.position;
        _hp = Constants.instance.SHIELD_HITS[SHIELD_CURRENT_LEVEL];

        StartCoroutine(GenerateAsteroids());
        StartCoroutine(GenerateStars());
    }

    private IEnumerator OnGameSpeedChange(float duration)
    {
        yield return new WaitForSeconds(duration);
        DOTween.To(() => GAME_CURRENT_SPEED, x => GAME_CURRENT_SPEED = x, _lastSpeed, 0.5f);
    }

    private void OnDeath()
    {
        _dead = true;
        
        if (_hpRedAlertTweener != null)
        {
            _hpRedAlertTweener.Kill();
        }

        Destroy(_playerInstance);

        _bombButton.enabled = false;
        _timeButton.enabled = false;
        _speedButton.enabled = false;
        _repairButton.enabled = false;

        // Apply coins
        Constants.instance.OnUpdateCoins(Constants.instance.CURRENT_COINS + _starCount * 5);

        // Update best score
        if (_score > Constants.instance.BEST_SCORE)
        {
            Constants.instance.OnUpdateBestScore(_score);
        }

        // Show death menu
        _deathBlackOverlay.gameObject.SetActive(true);
        _deathBlackOverlay.DOFade(0.5f, 0.3f);

        // Update death menu GUI
        _deathMenuCoins.text = (5*_starCount).ToString();
        _deathMenuStars.text = _starCount.ToString();
        _deathMenuDistance.text = _score.ToString("0.00") + "·10⁶ km";
        _deathMenuBest.text = Constants.instance.BEST_SCORE.ToString("0.00") + "·10⁶ km";

        _deathMenu.SetActive(true);
        _deathMenu.transform.DOMoveX(Screen.width * 0.5f, 0.3f);
    }

    private void OnRedAlert()
    {
        if (!_dead)
        {
            float alpha = 0.3f;

            if (_hpRedAlert.color.a > 0.28f)
            {
                alpha = 0f;
            }

            _hpRedAlertTweener = _hpRedAlert.DOFade(alpha, 1f).OnComplete(() =>
            {
                OnRedAlert();
            });
        }
    }

    private IEnumerator OnStarCountHide()
    {
        yield return new WaitForSeconds(1f);

        _starCounter.transform.DOMove(_starCounterOriginalPosition, 0.3f);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (!_paused && !_dead)
        {
            _score += Time.deltaTime * GAME_CURRENT_SPEED;
            _scoreLabel.text = _score.ToString("0.0");

            if (GAME_CURRENT_SPEED < Constants.instance.GAME_MAXIMUM_SPEED)
            {
                GAME_CURRENT_SPEED += Constants.instance.GAME_ACCELERATION * Time.deltaTime;

                if (Constants.DEBUG_MODE) _debugSpeedLabel.text = "SPEED: " + GAME_CURRENT_SPEED.ToString();
            }

            if (GAME_NORMAL_SPEED < Constants.instance.GAME_MAXIMUM_SPEED)
            {
                GAME_NORMAL_SPEED += Constants.instance.GAME_ACCELERATION * Time.deltaTime;
            }
        }
    }

    private IEnumerator GenerateStars()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(2f, 4f));
            if (!_paused) _stars.Add(GameObject.Instantiate(_starPrefab).GetComponent<Asteroid>());
        }
    }

    private IEnumerator GenerateAsteroids()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(0.5f, 1.5f));
            if (!_paused) _asteroids.Add(GameObject.Instantiate(_asteroidPrefab).GetComponent<Asteroid>());
        }
    }
}
