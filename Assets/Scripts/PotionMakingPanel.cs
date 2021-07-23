using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PotionMakingPanel : MonoBehaviour
{
	[SerializeField]
	private List<Ingredient> m_allIngredientScriptables;

	[SerializeField]
	private GameObject m_ingredientUIPrefab;

	[SerializeField]
	private GameObject m_gridLayout;

	[SerializeField]
	private Canvas m_canvas;

	[SerializeField]
	private Button m_refillButton;

	private List<IngredientController> m_ingredientsDisplayed = new List<IngredientController>();

	private void Start()
	{
		GameManager.GetInstance().OnPotionGiven += HideRefillButton;
		GameManager.GetInstance().OnDisplayHero += ShowRefillButton;

		m_refillButton.onClick.AddListener(OnRefillButtonPressed);

		//Add all ingredients to grid panel
		int index = 0;
		foreach (var ingredient in m_allIngredientScriptables)
		{
			GameObject newIngredientUI = Instantiate(m_ingredientUIPrefab, m_gridLayout.transform);
			IngredientController ic = newIngredientUI.GetComponent<IngredientController>();
			ic.SetupIngredient(ingredient, m_canvas, index);
			ic.OnIngredientConsumed += OnIngredientConsumed;
			m_ingredientsDisplayed.Add(newIngredientUI.GetComponent<IngredientController>());
			index++;
		}
	}

	void OnIngredientConsumed(IngredientController ic)
	{
		m_ingredientsDisplayed.Remove(ic);
	}

	void HideRefillButton()
	{
		m_refillButton.transform.parent.gameObject.SetActive(false);
	}

	void ShowRefillButton(Hero hero)
	{
		m_refillButton.transform.parent.gameObject.SetActive(true);
	}

	void OnRefillButtonPressed()
	{
		int numRefilled = 0;
		int indexInGrid = 0;		

		foreach (var ingredient in m_allIngredientScriptables)
		{
			bool wasFound = false;
			foreach (var ingredientDisplayed in m_ingredientsDisplayed)
			{
				if (ingredientDisplayed.GetIngredient().m_name == ingredient.m_name)
				{
					wasFound = true;
				}
			}

			if (!wasFound)
			{
				GameObject newIngredientUI = Instantiate(m_ingredientUIPrefab, m_gridLayout.transform);
				newIngredientUI.transform.SetSiblingIndex(indexInGrid);
				IngredientController ic = newIngredientUI.GetComponent<IngredientController>();
				ic.SetupIngredient(ingredient, m_canvas, indexInGrid);
				ic.OnIngredientConsumed += OnIngredientConsumed;
				m_ingredientsDisplayed.Add(newIngredientUI.GetComponent<IngredientController>());
				numRefilled++;
			}

			indexInGrid++;
		}

		GameManager.GetInstance().AddMoney(-(numRefilled * 3));
	}
}
