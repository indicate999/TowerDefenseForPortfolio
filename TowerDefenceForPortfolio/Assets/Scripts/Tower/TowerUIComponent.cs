using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(TowerActivator), typeof(TowerButtons))]
public class TowerUIComponent : MonoBehaviour
{
    [SerializeField] private GameObject _turretObj;

    private TurretDTO _turretDTO;

    private const int _zStartTurretRotation = 180;

    private Sprite currentTurretSprite;

    private MoneyService _moneyService;
    private SoundEffector _soundEffector;
    private SpriteRenderer _spriteRenderer;
    private Animator _animator;
    private AudioSource _audioSource;
    private TurretPointer _pointer;
    private TurretAttack _turretAttack;
    private TowerActivator _activator;
    private TowerButtons _buttons;

    private void Start()
    {
        _turretDTO = FindObjectOfType<TurretDTO>();
        _moneyService = FindObjectOfType<MoneyService>();
        _soundEffector = FindObjectOfType<SoundEffector>();
        _spriteRenderer = _turretObj.GetComponent<SpriteRenderer>();
        _animator = _turretObj.GetComponent<Animator>();
        _audioSource = _turretObj.GetComponent<AudioSource>();
        _pointer = _turretObj.GetComponent<TurretPointer>();
        _turretAttack = _turretObj.GetComponent<TurretAttack>();
        _activator = GetComponent<TowerActivator>();
        _buttons = GetComponent<TowerButtons>();

        SetBuyPanel();
    }

    private void LateUpdate()
    {
        if (currentTurretSprite != null)
            _spriteRenderer.sprite = currentTurretSprite;
    }

    private void SetBuyPanel()
    {
        for (int i = 0; i < _buttons.BuyButtons.Length; i++)
        {
            _buttons.BuyButtons[i].transform.GetChild(0).GetComponent<Text>().text =
                _turretDTO.GetTurretByIndices(i, 0).PurchasePrice.ToString();
        }
    }

    public bool IsActiveTurret()
    {
        return _turretObj.activeSelf;
    }

    public void BuyTurret(int turretExampleIndex)
    {
        var turret = _turretDTO.GetTurretByIndices(turretExampleIndex, 0);

        float buyPurchase = turret.PurchasePrice; 

        if (_moneyService.EnoughCoinsCheck(buyPurchase))
        {
            _moneyService.RemoveCoins(buyPurchase);

            _soundEffector.PLayBuildingSound();

            currentTurretSprite = turret.Sprite;
            _animator.runtimeAnimatorController = turret.AnimatorController;
            _audioSource.clip = _turretDTO.GetTurretAudioClip(turretExampleIndex);

            _turretAttack.SetAttackParameters(turret.Damage, turret.AttackSpeed, turret.AttackRadius);

            _turretObj.transform.eulerAngles = new Vector3(0, 0, _zStartTurretRotation);
            _turretObj.SetActive(true);

            _activator.SetActiveBuyPanel(false);

            _pointer.SetIndicesForNewTurret(turretExampleIndex);

            UpdateChangePanel();

            _turretObj.SetActive(true);
        }
    }

    public void UpgradeTurret()
    {
        int turretSeriesLength = _turretDTO.GetTurretSeriesLength(_pointer.TurretExampleIndex) - 1;

        float upgradePurchase = _turretDTO.GetTurretByIndices(_pointer.TurretExampleIndex, _pointer.TurretSerieIndex + 1).PurchasePrice;


        if (_moneyService.EnoughCoinsCheck(upgradePurchase))
        {
            _moneyService.RemoveCoins(upgradePurchase);

            _soundEffector.PLayBuildingSound();

            _pointer.IncreaseTurretSeriesIndex();

            if (_pointer.TurretSerieIndex < turretSeriesLength)
                UpdateChangePanel();
            else
                _buttons.SetActiveUpgradeButton(false);

            var turret = _turretDTO.GetTurretByIndices(_pointer.TurretExampleIndex, _pointer.TurretSerieIndex);

            currentTurretSprite = turret.Sprite;
            _animator.runtimeAnimatorController = turret.AnimatorController;

            _turretAttack.SetAttackParameters(turret.Damage, turret.AttackSpeed, turret.AttackRadius);

            _turretObj.transform.eulerAngles = new Vector3(0, 0, _zStartTurretRotation);

            _activator.SetActiveChangePanel(false);
        }
    }

    public void SaleTurret()
    {
        var sellingPrice = _turretDTO.GetTurretByIndices(_pointer.TurretExampleIndex, _pointer.TurretSerieIndex).SellingPrice;
        _moneyService.AddCoins(sellingPrice);

        _soundEffector.PLaySaleSound();

        currentTurretSprite = null;
        _spriteRenderer.sprite = null;
        _animator.runtimeAnimatorController = null;
        _audioSource.clip = null;

        _turretAttack.SetAttackParameters(0, 0, 0);

        _turretObj.SetActive(false);

        if (!_buttons.GetUpgradeButtonActiveStatus())
            _buttons.SetActiveUpgradeButton(true);

        _activator.SetActiveChangePanel(false);

        _turretObj.SetActive(false);
    }

    private void UpdateChangePanel()
    {
        _buttons.UpgradeButton.transform.GetChild(0).GetComponent<Text>().text =
            _turretDTO.GetTurretByIndices(_pointer.TurretExampleIndex, _pointer.TurretSerieIndex + 1).PurchasePrice.ToString();

        _buttons.SaleButton.transform.GetChild(0).GetComponent<Text>().text =
            _turretDTO.GetTurretByIndices(_pointer.TurretExampleIndex, _pointer.TurretSerieIndex).SellingPrice.ToString();
    }
}
