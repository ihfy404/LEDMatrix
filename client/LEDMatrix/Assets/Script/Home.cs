using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LEDCube
{
	public class Home : MonoBehaviour
	{
		
		[SerializeField]
		GameObject blockMenu;

		[SerializeField]
		GameObject content;

		[SerializeField]
		InputField paramInputField;


		private Parameters parameters = new Parameters();
		public Parameters Parameters()
		{
			return parameters;
		}


		void Awake()
		{
			blockMenu.SetActive(true);
		}

		public void OnPressMenuButton()
		{
			blockMenu.SetActive(true);
		}

		public void OnPressCreateParamButton()
		{
			CreateParam();
			paramInputField.text = "";
		}

		void CreateParam()
		{
			string name = paramInputField.text;
			if (name != "" && Data.Instance.Params.AddParam(name))
			{
				GameObject prefab = (GameObject)Resources.Load("Param");
				GameObject param = Instantiate(prefab) as GameObject;
				param.transform.SetParent(content.transform, false);
				param.GetComponent<Parameter>().Set(name);
			}
		}
	}
}
