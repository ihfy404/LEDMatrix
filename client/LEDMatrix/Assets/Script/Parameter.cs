using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace LEDCube
{
	public class Parameter : MonoBehaviour
	{
		[SerializeField]
		Text text;

		string name;

		public void Set(string value)
		{
			name = value;
			text.text = name;
		}

		void Remove()
		{
			Data.Instance.Params.RemoveParam(name);
			Destroy(this.gameObject);
		}
		public void OnPressRemoveButton()
		{
			//ワンクッション挟みたい
			Remove();
		}

		public void OnValueChange(string value)
		{
			int result;
			if (Int32.TryParse(value, out result))	Data.Instance.Params.ChangeValue(name, result);
		}
	}
}

