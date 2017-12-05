using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LEDCube
{
	public class LedMap : MonoBehaviour
	{
		[SerializeField]
		Transform content;

		[SerializeField]
		Button button;

		Button allButton;
		Button[] colButton = new Button[8];
		Button[] rowButton = new Button[8];

		Block block;
		Led[] Leds = new Led[64];

		void Awake()
		{
		}

		void SetBlock(Block value)
		{
			block = value;
		}

		public void Create(Block block, string value = null)
		{
			button.onClick.AddListener(OnpressButton);
			SetBlock (block);
			string[] states = new string[Leds.Length];
			if (value != "")
			{
				states = value.Split (',');
			}
			else
			{
				for(int i=0; i < Leds.Length; i++)
				{
					states[i] = "0";
				}
			}

			GameObject lineButtonPrefab = (GameObject)Resources.Load ("LineButton");

			allButton = Create(lineButtonPrefab).GetComponent<Button>();
			allButton.onClick.AddListener(() => OnPressAllButton());

			for(int i=0; i<colButton.Length; i++)
			{
				int n = i;
				colButton[i] = Create(lineButtonPrefab).GetComponent<Button>();
				colButton[i].onClick.AddListener(() => OnPressColButton(n));

			}
			GameObject ledPrefab = (GameObject)Resources.Load ("LED");
			for (int i=0; i<Leds.Length; i++)
			{
				if(i%8 == 0)
				{
					int idx = i/8;
					int n = idx;
					rowButton[idx] = Create(lineButtonPrefab).GetComponent<Button>();
					rowButton[idx].onClick.AddListener(() => OnPressRowButton(n));
				}
				Leds[i] = Create(ledPrefab).GetComponent<Led>();

				if (states [i] != null) {
					Leds [i].SetState (states [i] == "1" ? true : false);
				} else {
					Leds [i].SetState (false);
				}
			}
		}

		GameObject Create(GameObject prefab)
		{
			GameObject obj = Instantiate(prefab);
			obj.transform.SetParent(content);
			obj.transform.localScale = new Vector3 (1f, 1f, 1f);
			return obj;
		}

		public void OnpressButton()
		{
			Close();
		}

		void Close()
		{
			string param = "";
			for (int i = 0; i < Leds.Length; i++)
			{
				param = param + (Leds[i].State() ? "1" : "0") + ",";
			}
			block.SetParam (param);
			Destroy (this.gameObject);
		}

		public void OnPressAllButton()
		{
			for (int i=0; i<Leds.Length; i++)
			{
				Leds[i].Switch();
			}
		}

		public void OnPressColButton(int idx)
		{

			for (int j=0; j<Leds.Length; j++)
			{
					if(j%8 == idx)	Leds[j].Switch();
			}

		}

		public void OnPressRowButton(int idx)
		{

			for (int j=idx*8; j<(idx+1)*8; j++)
			{
				Leds[j].Switch();
			}

		}
	}

}
