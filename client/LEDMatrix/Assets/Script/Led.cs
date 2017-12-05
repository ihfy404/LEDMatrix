using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LEDCube
{
	public class Led : MonoBehaviour
	{
		[SerializeField]
		Button button;

		[SerializeField]
		Image image;

		bool state = false;
		public bool State()
		{
			return state;
		}

		public void OnPressButton()
		{
			Switch();
		}

		public void Switch()
		{
			SetState (!state);
		}

		public void SetState(bool value)
		{
			state = value;
			image.color = state ? Color.blue : Color.white;
		}
	}	
}
