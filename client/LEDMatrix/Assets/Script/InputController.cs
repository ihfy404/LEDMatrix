using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace LEDCube
{
	public class InputController : MonoBehaviour, IPointerClickHandler
	{
		[SerializeField]
		InputField inputField;

		//[SerializeField]
		public Dropdown dropDown;

		string iValue;
		string dValue;

		void Awake()
		{
		}

		public void OnPointerClick(PointerEventData eventData)
		{
			Debug.Log("aaa");
		}

		//true: InputField, false: DropDown
		public bool State()
		{
			return inputField.gameObject.activeSelf;
		}

		public void OnValueChanged(string value)
		{
			iValue = value;
		}

		public void OnValueChanged(int idx)
		{
		}

		public string Value()
		{
			if(State()) return iValue;
			else 		return dValue;
		}

		void Switch()
		{
			bool value = State();
			inputField.gameObject.SetActive(!value);
			dropDown.gameObject.SetActive(value);
		}

		public void OnPuressButton()
		{
			Switch();
		}

	}

}