using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    /* UI Dialogs */
    public GameObject _initDialog;
    public GameObject _shopDialog;

    /* UI Widgets */
    public GameObject _gameLogo;
    public GameObject _whiteFlash;
    public GameObject _shopButton;
    public GameObject _startButton;
    public GameObject _musicToggle;

    public GameObject _shopBackButton;
    public GameObject _shopLabel;
    public GameObject _shopCoins;
    public Text _shopCoinsLabel;
    public GameObject _timeSkillButton;
    public GameObject _bombSkillButton;
    public GameObject _shieldSkillButton;
    public GameObject _burstSkillButton;
    public GameObject _repairSkillButton;
    public GameObject _itemDescription;
    public GameObject _itemIndicator;
    public Text _descriptionText;
    public Text _itemPrice;

    public Text _timeSkillLevel;
    public Text _bombSkillLevel;
    public Text _burstSkillLevel;
    public Text _repairSkillLevel;
    public Text _shieldSkillLevel;

    public Button _buyButton;

    public AudioClip _menuClick;

    /* Shop state */
    private enum ShopItems
    {
        Time,
        Bomb,
        Shield,
        Burst,
        Repair
    };

    private ShopItems _selectedItem = ShopItems.Time;

    /****************************************/

    public void OnStart()
    {
        _gameLogo.transform.DOMoveX(Screen.width * 2f, 0.5f);
        _shopButton.transform.DOMoveX(-Screen.width * 2f, 0.4f);
        _musicToggle.transform.DOMoveX(Screen.width * 2f, 0.4f);
        Camera.main.GetComponent<AudioSource>().DOFade(0f, 0.4f);

        if (Constants.instance.MUSIC_ON)
        {
            AudioSource.PlayClipAtPoint(_menuClick, Camera.main.transform.position);
        }

        _startButton.transform.DOMoveX(-Screen.width * 2f, 0.5f).OnComplete(() =>
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
        });
    }

    public void OnShop()
    {
        _gameLogo.transform.DOMoveX(Screen.width * 2f, 0.5f);
        _shopButton.transform.DOMoveX(-Screen.width * 2f, 0.4f);
        _musicToggle.transform.DOMoveX(Screen.width * 2f, 0.4f);

        if (Constants.instance.MUSIC_ON)
        {
            AudioSource.PlayClipAtPoint(_menuClick, Camera.main.transform.position);
        }

        _startButton.transform.DOMoveX(-Screen.width * 2f, 0.5f).OnComplete(() =>
        {
            _initDialog.SetActive(false);
            _shopDialog.SetActive(true);

            _shopLabel.transform.DOMoveY(Screen.height, 0.2f);
            _shopCoins.transform.DOMoveY(Screen.height, 0.2f);
            _itemDescription.transform.DOMoveY(Screen.height * 0.25f, 0.3f);
            _shieldSkillButton.transform.DOMoveX(Screen.width * 0.5f, 0.1f);
            _burstSkillButton.transform.DOMoveX(Screen.width * 0.68f, 0.2f);
            _repairSkillButton.transform.DOMoveX(Screen.width * 0.86f, 0.3f);
            _bombSkillButton.transform.DOMoveX(Screen.width * 0.32f, 0.2f);
            _timeSkillButton.transform.DOMoveX(Screen.width * 0.13f, 0.3f);
            _shopBackButton.transform.DOMoveY(Screen.height, 0.3f);

            int bombLevel = PlayerPrefs.GetInt("BOMB_SKILL_CURRENT_LEVEL");
            int timeLevel = PlayerPrefs.GetInt("TIME_SKILL_CURRENT_LEVEL");
            int burstLevel = PlayerPrefs.GetInt("BURST_SKILL_CURRENT_LEVEL");
            int repairLevel = PlayerPrefs.GetInt("REPAIR_SKILL_CURRENT_LEVEL");
            int shieldLevel = PlayerPrefs.GetInt("SHIELD_CURRENT_LEVEL");

            if (bombLevel == 0)
            {
                _bombSkillLevel.text = "new!";
            }
            else
            {
                _bombSkillLevel.text = "Lv." + (1 + bombLevel).ToString();
            }

            if (timeLevel == 0)
            {
                _timeSkillLevel.text = "new!";
            }
            else
            {
                _timeSkillLevel.text = "Lv." + (1 + timeLevel).ToString();
            }

            if (burstLevel == 0)
            {
                _burstSkillLevel.text = "new!";
            }
            else
            {
                _burstSkillLevel.text = "Lv." + (1 + burstLevel).ToString();
            }

            if (shieldLevel == 0)
            {
                _shieldSkillLevel.text = "new!";
            }
            else
            {
                _shieldSkillLevel.text = "Lv." + (1 + shieldLevel).ToString();
            }

            if (repairLevel == 0)
            {
                _repairSkillLevel.text = "new!";
            }
            else
            {
                _repairSkillLevel.text = "Lv." + (1 + repairLevel).ToString();
            }

            _selectedItem = ShopItems.Time;
            OnShopItemChanged();
        });
    }

    public void OnShopBack()
    {
        _shopLabel.transform.DOMoveY(Screen.height * 1.3f, 0.2f);
        _shopCoins.transform.DOMoveY(Screen.height * 1.3f, 0.2f);
        _itemDescription.transform.DOMoveY(-Screen.height * 0.25f, 0.3f);
        _shieldSkillButton.transform.DOMoveX(Screen.width * 1.3f, 0.3f);
        _repairSkillButton.transform.DOMoveX(Screen.width * 1.3f, 0.3f);
        _bombSkillButton.transform.DOMoveX(-Screen.width * 0.3f, 0.2f);
        _timeSkillButton.transform.DOMoveX(-Screen.width * 0.3f, 0.3f);
        _burstSkillButton.transform.DOMoveX(Screen.width * 1.3f, 0.3f);

        if (Constants.instance.MUSIC_ON)
        {
            AudioSource.PlayClipAtPoint(_menuClick, Camera.main.transform.position);
        }

        _shopBackButton.transform.DOMoveY(Screen.height * 1.3f, 0.3f).OnComplete(() =>
        {
            _shopDialog.SetActive(false);
            _initDialog.SetActive(true);

            _gameLogo.transform.DOMoveX(Screen.width * 0.5f, 0.5f);
            _shopButton.transform.DOMoveX(Screen.width * 0.5f, 0.4f);
            _startButton.transform.DOMoveX(Screen.width * 0.5f, 0.5f);
            _musicToggle.transform.DOMoveX(Screen.width - 82f, 0.4f);
        });
    }

    public void OnTimeSkill()
    {
        if (_selectedItem != ShopItems.Time)
        {
            _selectedItem = ShopItems.Time;

            if (Constants.instance.MUSIC_ON)
            {
                AudioSource.PlayClipAtPoint(_menuClick, Camera.main.transform.position);
            }
            
            OnShopItemChanged();
        }
    }

    public void OnBombSkill()
    {
        if (_selectedItem != ShopItems.Bomb)
        {
            _selectedItem = ShopItems.Bomb;

            if (Constants.instance.MUSIC_ON)
            {
                AudioSource.PlayClipAtPoint(_menuClick, Camera.main.transform.position);
            }

            OnShopItemChanged();
        }
    }

    public void OnShieldSkill()
    {
        if (_selectedItem != ShopItems.Shield)
        {
            _selectedItem = ShopItems.Shield;

            if (Constants.instance.MUSIC_ON)
            {
                AudioSource.PlayClipAtPoint(_menuClick, Camera.main.transform.position);
            }

            OnShopItemChanged();
        }
    }

    public void OnBurstSkill()
    {
        if (_selectedItem != ShopItems.Burst)
        {
            _selectedItem = ShopItems.Burst;

            if (Constants.instance.MUSIC_ON)
            {
                AudioSource.PlayClipAtPoint(_menuClick, Camera.main.transform.position);
            }

            OnShopItemChanged();
        }
    }

    public void OnRepairSkill()
    {
        if (_selectedItem != ShopItems.Repair)
        {
            _selectedItem = ShopItems.Repair;

            if (Constants.instance.MUSIC_ON)
            {
                AudioSource.PlayClipAtPoint(_menuClick, Camera.main.transform.position);
            }

            OnShopItemChanged();
        }
    }

    public void OnMusicToggle()
    {
        Constants.instance.OnMusicToggle(_musicToggle.GetComponent<Toggle>().isOn);

        if (Constants.instance.MUSIC_ON)
        {
            AudioSource.PlayClipAtPoint(_menuClick, Camera.main.transform.position);
            Camera.main.GetComponent<AudioSource>().volume = 1f;
        }
        else
        {
            Camera.main.GetComponent<AudioSource>().volume = 0f;
        }
    }

    public void OnDebugDeleteSave()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetInt("CURRENT_COINS", 0);
        UnityEngine.SceneManagement.SceneManager.LoadScene("Init");
    }

    public void OnJumbleLogo()
    {
        Application.OpenURL("https://play.google.com/store/apps/dev?id=8830077250983180665");
    }

    public void OnShopBuy()
    {
        int bombLevel = PlayerPrefs.GetInt("BOMB_SKILL_CURRENT_LEVEL");
        int timeLevel = PlayerPrefs.GetInt("TIME_SKILL_CURRENT_LEVEL");
        int burstLevel = PlayerPrefs.GetInt("BURST_SKILL_CURRENT_LEVEL");
        int repairLevel = PlayerPrefs.GetInt("REPAIR_SKILL_CURRENT_LEVEL");
        int shieldLevel = PlayerPrefs.GetInt("SHIELD_CURRENT_LEVEL");

        if (Constants.instance.MUSIC_ON)
        {
            AudioSource.PlayClipAtPoint(_menuClick, Camera.main.transform.position);
        }

        if (_selectedItem == ShopItems.Time)
        {
            if (Constants.instance.TIME_SKILL_PRICE[timeLevel] <= Constants.instance.CURRENT_COINS)
            {
                Constants.instance.CURRENT_COINS -= Constants.instance.TIME_SKILL_PRICE[timeLevel];
                PlayerPrefs.SetInt("TIME_SKILL_CURRENT_LEVEL", timeLevel + 1);
            }
        }
        else if (_selectedItem == ShopItems.Bomb)
        {
            if (Constants.instance.BOMB_SKILL_PRICE[bombLevel] <= Constants.instance.CURRENT_COINS)
            {
                Constants.instance.CURRENT_COINS -= Constants.instance.BOMB_SKILL_PRICE[bombLevel];
                PlayerPrefs.SetInt("BOMB_SKILL_CURRENT_LEVEL", bombLevel + 1);
            }
        }
        else if (_selectedItem == ShopItems.Burst)
        {
            if (Constants.instance.BURST_SKILL_PRICE[burstLevel] <= Constants.instance.CURRENT_COINS)
            {
                Constants.instance.CURRENT_COINS -= Constants.instance.BURST_SKILL_PRICE[burstLevel];
                PlayerPrefs.SetInt("BURST_SKILL_CURRENT_LEVEL", burstLevel + 1);
            }
        }
        else if (_selectedItem == ShopItems.Shield)
        {
            if (Constants.instance.SHIELD_PRICE[shieldLevel] <= Constants.instance.CURRENT_COINS)
            {
                Constants.instance.CURRENT_COINS -= Constants.instance.SHIELD_PRICE[shieldLevel];
                PlayerPrefs.SetInt("SHIELD_CURRENT_LEVEL", shieldLevel + 1);
            }
        }
        else if (_selectedItem == ShopItems.Repair)
        {
            if (Constants.instance.REPAIR_SKILL_PRICE[repairLevel] <= Constants.instance.CURRENT_COINS)
            {
                Constants.instance.CURRENT_COINS -= Constants.instance.REPAIR_SKILL_PRICE[repairLevel];
                PlayerPrefs.SetInt("REPAIR_SKILL_CURRENT_LEVEL", repairLevel + 1);
            }
        }

        _shopCoinsLabel.text = Constants.instance.CURRENT_COINS.ToString();
        OnShopItemChanged();
    }

    /****************************************/

    private void Start()
    {
        if (PlayerPrefs.GetInt("MENU_BACK_FROM_GAME") == 1)
        {
            _whiteFlash.SetActive(false);
        }
        else
        {
            _whiteFlash.SetActive(true);
        }

        _musicToggle.GetComponent<Toggle>().isOn = Constants.instance.MUSIC_ON;

        // Initialize dialogs
        _shopDialog.SetActive(false);

        // Initialize coins
        _shopCoinsLabel.text = Constants.instance.CURRENT_COINS.ToString();

        if (!Constants.instance.MUSIC_ON)
        {
            Camera.main.GetComponent<AudioSource>().volume = 0f;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    private void OnShopItemChanged()
    {
        int bombLevel = PlayerPrefs.GetInt("BOMB_SKILL_CURRENT_LEVEL");
        int timeLevel = PlayerPrefs.GetInt("TIME_SKILL_CURRENT_LEVEL");
        int burstLevel = PlayerPrefs.GetInt("BURST_SKILL_CURRENT_LEVEL");
        int repairLevel = PlayerPrefs.GetInt("REPAIR_SKILL_CURRENT_LEVEL");
        int shieldLevel = PlayerPrefs.GetInt("SHIELD_CURRENT_LEVEL");

        _buyButton.enabled = true;
        _buyButton.image.color = Color.white;

        if (_selectedItem == ShopItems.Time)
        {
            // Set the button scale
            _timeSkillButton.transform.DOScale(1.1f, 0.2f);
            _bombSkillButton.transform.DOScale(1f, 0.2f);
            _shieldSkillButton.transform.DOScale(1f, 0.2f);
            _burstSkillButton.transform.DOScale(1f, 0.2f);
            _repairSkillButton.transform.DOScale(1f, 0.2f);

            // Move the item indicator
            _itemIndicator.transform.DOMoveX(Screen.width * 0.13f, 0.2f);

            // Set the description text
            _descriptionText.text = Constants.instance.TIME_SKILL_SHOP_DESCRIPTION;

            // Set the price
            _itemPrice.text = Constants.instance.TIME_SKILL_PRICE[timeLevel].ToString();

            if (timeLevel == Constants.instance.TIME_SKILL_PRICE.Count - 1)
            {
                _buyButton.enabled = false;
                _buyButton.image.color = new Color(0.4f, 0.4f, 0.4f, 1f);
            }
        }
        else if (_selectedItem == ShopItems.Bomb)
        {
            // Set the button scale
            _timeSkillButton.transform.DOScale(1f, 0.2f);
            _bombSkillButton.transform.DOScale(1.1f, 0.2f);
            _shieldSkillButton.transform.DOScale(1f, 0.2f);
            _burstSkillButton.transform.DOScale(1f, 0.2f);
            _repairSkillButton.transform.DOScale(1f, 0.2f);

            // Move the item indicator
            _itemIndicator.transform.DOMoveX(Screen.width * 0.32f, 0.2f);

            // Set the description text
            _descriptionText.text = Constants.instance.BOMB_SKILL_SHOP_DESCRIPTION;

            // Set the price
            _itemPrice.text = Constants.instance.BOMB_SKILL_PRICE[bombLevel].ToString();

            if (bombLevel == Constants.instance.BOMB_SKILL_PRICE.Count - 1)
            {
                _buyButton.enabled = false;
                _buyButton.image.color = new Color(0.4f, 0.4f, 0.4f, 1f);
            }
        }
        else if (_selectedItem == ShopItems.Shield)
        {
            // Set the button scale
            _timeSkillButton.transform.DOScale(1f, 0.2f);
            _bombSkillButton.transform.DOScale(1f, 0.2f);
            _shieldSkillButton.transform.DOScale(1.1f, 0.2f);
            _burstSkillButton.transform.DOScale(1f, 0.2f);
            _repairSkillButton.transform.DOScale(1f, 0.2f);

            // Move the item indicator
            _itemIndicator.transform.DOMoveX(Screen.width * 0.5f, 0.2f);

            // Set the description text
            _descriptionText.text = Constants.instance.SHIELD_SKILL_SHOP_DESCRIPTION;

            // Set the price
            _itemPrice.text = Constants.instance.SHIELD_PRICE[shieldLevel].ToString();

            if (shieldLevel == Constants.instance.SHIELD_PRICE.Count - 1)
            {
                _buyButton.enabled = false;
                _buyButton.image.color = new Color(0.4f, 0.4f, 0.4f, 1f);
            }
        }
        else if (_selectedItem == ShopItems.Burst)
        {
            // Set the button scale
            _timeSkillButton.transform.DOScale(1f, 0.2f);
            _bombSkillButton.transform.DOScale(1f, 0.2f);
            _shieldSkillButton.transform.DOScale(1f, 0.2f);
            _burstSkillButton.transform.DOScale(1.1f, 0.2f);
            _repairSkillButton.transform.DOScale(1f, 0.2f);

            // Move the item indicator
            _itemIndicator.transform.DOMoveX(Screen.width * 0.68f, 0.2f);

            // Set the description text
            _descriptionText.text = Constants.instance.BURST_SKILL_SHOP_DESCRIPTION;

            // Set the price
            _itemPrice.text = Constants.instance.BURST_SKILL_PRICE[burstLevel].ToString();

            if (burstLevel == Constants.instance.BURST_SKILL_PRICE.Count - 1)
            {
                _buyButton.enabled = false;
                _buyButton.image.color = new Color(0.4f, 0.4f, 0.4f, 1f);
            }
        }
        if (_selectedItem == ShopItems.Repair)
        {
            // Set the button scale
            _timeSkillButton.transform.DOScale(1f, 0.2f);
            _bombSkillButton.transform.DOScale(1f, 0.2f);
            _shieldSkillButton.transform.DOScale(1f, 0.2f);
            _burstSkillButton.transform.DOScale(1f, 0.2f);
            _repairSkillButton.transform.DOScale(1.1f, 0.2f);

            // Move the item indicator
            _itemIndicator.transform.DOMoveX(Screen.width * 0.86f, 0.2f);

            // Set the description text
            _descriptionText.text = Constants.instance.REPAIR_SKILL_SHOP_DESCRIPTION;

            // Set the price
            _itemPrice.text = Constants.instance.REPAIR_SKILL_PRICE[repairLevel].ToString();

            if (repairLevel == Constants.instance.REPAIR_SKILL_PRICE.Count - 1)
            {
                _buyButton.enabled = false;
                _buyButton.image.color = new Color(0.4f, 0.4f, 0.4f, 1f);
            }
        }

        if (bombLevel == 0)
        {
            _bombSkillLevel.text = "new!";
        }
        else
        {
            _bombSkillLevel.text = "Lv." + (1 + bombLevel).ToString();
        }

        if (timeLevel == 0)
        {
            _timeSkillLevel.text = "new!";
        }
        else
        {
            _timeSkillLevel.text = "Lv." + (1 + timeLevel).ToString();
        }

        if (burstLevel == 0)
        {
            _burstSkillLevel.text = "new!";
        }
        else
        {
            _burstSkillLevel.text = "Lv." + (1 + burstLevel).ToString();
        }

        if (shieldLevel == 0)
        {
            _shieldSkillLevel.text = "new!";
        }
        else
        {
            _shieldSkillLevel.text = "Lv." + (1 + shieldLevel).ToString();
        }

        if (repairLevel == 0)
        {
            _repairSkillLevel.text = "new!";
        }
        else
        {
            _repairSkillLevel.text = "Lv." + (1 + repairLevel).ToString();
        }
    }
}
