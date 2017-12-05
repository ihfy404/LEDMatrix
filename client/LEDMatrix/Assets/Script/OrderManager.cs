using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LEDCube
{
	public class OrderManager : MonoBehaviour
	{
		private List<Block> blockList = new List<Block>();

		[SerializeField]
		Button button;

		public int BlockIndex(Block block)
		{
			return blockList.IndexOf(block);
		}

		void Start()
		{

		}

		public void Reset()
		{
			foreach(Block block in blockList)
			{
				Destroy(block.gameObject);
			}
			blockList = new List<Block>();
			string[] tmp = {"0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,"};
			Send(tmp);
		}

		public int LastBlockIndex()
		{
			int index;
			if (blockList.Count == 0)
			{
				index = 0;
			}
			else
			{
				index = blockList.Count;
			}
			return index - 1;
		}

		public void AddBlock(Block block, Block preBlock)
		{
			int blockIndex = BlockIndex(preBlock) + 1;
			AddBlock(block, blockIndex);

			if (LastBlockIndex() != blockIndex)
			{
				for(int idx = blockIndex+1; idx < blockList.Count; idx++)
				{
					GameObject blockObject = blockList[idx].gameObject;
					Vector3 pos = blockObject.transform.localPosition;
					pos.y -= blockObject.GetComponent<RectTransform>().rect.height;
					blockList[idx].gameObject.transform.localPosition = pos;
				}
			}

		}

		private void AddBlock(Block block, int index)
		{
			blockList.Insert(index, block);
			Debug.Log("add:List[" + blockList.IndexOf(block) + "]");
		}

		public void RemoveBlock(Block block)
		{
			
			int blockIndex = blockList.IndexOf(block);
			if (LastBlockIndex() != blockIndex)
			{
				for(int idx = blockIndex+1; idx < blockList.Count; idx++)
				{
					GameObject blockObject = blockList[idx].gameObject;
					Vector3 pos = blockObject.transform.localPosition;
					pos.y += blockObject.GetComponent<RectTransform>().rect.height;
					blockList[idx].gameObject.transform.localPosition = pos;
				}
			}
			blockList.Remove(block);
			Debug.Log("Remove:List[" + blockIndex + "]");
		}

		public void OnPressExcuteButton()
		{
			Excute();
			StartCoroutine("WaitForButton");
		}

		IEnumerator WaitForButton(){

			button.interactable = false;
			yield return new WaitForSeconds(5);
			button.interactable = true;

		}

		public void Excute()
		{
			List<string> msg = new List<string>();
			for(int idx=0; idx<blockList.Count; idx++)
			{
				Order order = blockList[idx].Order();
				string param = order.Param();
				msg.Add (param);
			}
			Send(msg.ToArray());
		}



		void Send(string[] msg)
		{
			//命令送信
			StartCoroutine (Connect(msg));
		}

		private IEnumerator Connect(string[] msg){
			string url = "http://192.168.128.106/led.php";

			//WWWForm:WWWクラスを使用してwebサーバにポストするフォームデータを生成するヘルパークラス
			WWWForm wwwForm = new WWWForm();

			for(int i=0; i<msg.Length; i++)
			{
				//AddFieldでfieldに値を格納                
				wwwForm.AddField ("text" + i.ToString(), msg[i]);
				Debug.Log(msg[i]);

			}

			//WWWオブジェクトにURL,WWWFormをセットすることでPOST,GETを行える。
			WWW www = new WWW(url,wwwForm);

			//実行
			yield return www;
		}
	}
}
