using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BetResultPopup : MonoBehaviour
{
	[SerializeField]
	private TextMeshProUGUI m_heroHP;

	[SerializeField]
	private TextMeshProUGUI m_items;

	[SerializeField]
	private TextMeshProUGUI m_potionStrength;

	[SerializeField]
	private TextMeshProUGUI m_potionBuff;

	[SerializeField]
	private TextMeshProUGUI m_placeOfDeath;

	[SerializeField]
	private TextMeshProUGUI m_finalWords;

	[SerializeField]
	private TextMeshProUGUI m_floorBet;

	[SerializeField]
	private TextMeshProUGUI m_betAmount;

	[SerializeField]
	private TextMeshProUGUI m_amountWon;

	[SerializeField]
	private Button m_receiveButton;

	[SerializeField]
	private AudioClip m_good;

	[SerializeField]
	private AudioClip m_bad;


	private AudioSource m_audioSource;
	public Action OnReceiveButtonPressed;

	private void Awake()
	{

	}

	public void Start()
	{
		m_receiveButton.onClick.AddListener(OnClick);
	}

	void OnClick()
	{
		OnReceiveButtonPressed?.Invoke();
	}

	public void ShowInfo(HeroInDungeon heroInDungeon)
	{
		m_audioSource = GetComponent<AudioSource>();

		HeroStats heroStats = heroInDungeon.m_hero.m_heroStats;
		Potion createdPotion = heroInDungeon.m_hero.m_createdPotion;

		m_heroHP.text = "HP: " + heroStats.m_currentHP + "/" + heroStats.m_maxHP; ;
		m_items.text = "Items: " + heroStats.m_weaponType + " with " + heroStats.m_armorType;

		m_potionStrength.text = "Healing Strength: " + createdPotion.m_healingStrength;
		m_potionBuff.text = "Buff Type: " + createdPotion.m_buffType;

		if (heroInDungeon.m_hero.m_selectedFloor > 0)
		{
			m_floorBet.text = "You bet they would die on: Floor " + heroInDungeon.m_hero.m_selectedFloor;
			m_betAmount.text = "Bid Amount: $" + heroInDungeon.m_bidAmount;
			m_amountWon.text = "Reward: $" + heroInDungeon.m_MoneyWon.ToString("F2") + " ($" + heroInDungeon.m_bidAmount + "x" + heroInDungeon.m_rewardMultiplier.ToString("F2") + ")";
		}
		else
		{
			m_floorBet.text = "You didn't bet on this hero";
			m_betAmount.text = "Bid Amount: $" + heroInDungeon.m_bidAmount;
			m_amountWon.text = "Reward: $0.00";
		}

	

		if (heroInDungeon.m_hasDungeonBeenBeaten)
		{
			m_placeOfDeath.text = "The hero didn't die!";
			m_finalWords.text = "";
			m_audioSource.clip = m_bad;
		}
		else
		{
			m_placeOfDeath.text = "Place of Death: Floor " + heroInDungeon.m_placeOfDeath;
			m_finalWords.text = "Final Words: " + heroInDungeon.m_finalWords;
			m_audioSource.clip = m_good;
		}

		gameObject.SetActive(true);
		m_audioSource.Play();
	}
}
