using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System;
using System.Collections;
using System.Collections.Generic;

namespace LEDCube
{
	public class Data
	{
		public readonly static Data Instance = new Data();

		private Parameters parameters = new Parameters();
		public Parameters Params
		{
			get
			{
				return parameters;	
			}
		}
	}

	public class Parameters
	{
		private Dictionary<string, int> param = new Dictionary<string, int>();
		public List<UnityEngine.UI.Dropdown.OptionData> dropDownList = new List<UnityEngine.UI.Dropdown.OptionData>();

		void Add(string name)
		{
			param.Add(name, 0);
			show();
		}
		void AddList(string name)
		{
			UnityEngine.UI.Dropdown.OptionData data = new UnityEngine.UI.Dropdown.OptionData();
			data.text = name;
			dropDownList.Add(data);
			Fetch();
		}
		public bool AddParam(string name)
		{
			if (!param.ContainsKey(name))
			{
				Add(name);
				AddList(name);
				return true;
			}
			else
			{
				return false;				
			}

		}

		void Remove(string name)
		{
			param.Remove(name);
			show();
		}
		void RemoveList(string name)
		{
			int target = -1;
			for(int idx = 0; idx < dropDownList.Count; idx++)
			{
				if (dropDownList[idx].text == name)
				{
					target = idx;
					break;
				}
			}
			if(target >= 0)	dropDownList.RemoveAt(target);
			Fetch();
		}
		public void RemoveParam(string name)
		{
			Remove(name);
			RemoveList(name);
		}

		void Change(string name, int value)
		{
			param[name] = value;
		}
		public void ChangeValue(string name, int value)
		{
			Change(name, value);
			show();
		}

		void show()
		{
			foreach (KeyValuePair<string, int> pair in param) {
				Debug.Log(string.Format("Key : {0} / Value : {1}", pair.Key, pair.Value));
			}
		}

		public List<string> Keys()
		{
			return new List<string>(param.Keys);

		}

		public void Fetch()
		{
			#if UNITY_EDITOR
			foreach (GameObject obj in UnityEngine.Resources.FindObjectsOfTypeAll(typeof(GameObject)))
			{
				// アセットからパスを取得.シーン上に存在するオブジェクトの場合,シーンファイル（.unity）のパスを取得.
				string path = AssetDatabase.GetAssetOrScenePath(obj);
				// シーン上に存在するオブジェクトかどうか文字列で判定.
				bool isScene = path.Contains(".unity");
				// シーン上に存在するオブジェクトならば処理.
				if (isScene)
				{
					if (obj.GetComponents<InputController>() != null)
					{
						if (obj.GetComponent<InputController>() != null)
						{
							InputController input = obj.GetComponent<InputController>();
							input.dropDown.options = dropDownList;
						}
					}
				}
			}
			#endif
		}

	}
}
