using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace LEDCube
{
	[RequireComponent(typeof(Image))]
	public class DragBlock : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
	{
		[SerializeField]
		private Transform canvasTran;
		private GameObject draggingObject;
		private bool isClone = false;

		[SerializeField]
		private OrderManager orderManager;

		public void OnBeginDrag(PointerEventData pointerEventData)
		{
			//ドラッグオブジェクトを作る
			CreateDragObject();
			draggingObject.transform.position = pointerEventData.position;
		}

		public void OnDrag(PointerEventData pointerEventData)
		{
			//ドラッグオブジェクトがポインタを追尾
			draggingObject.transform.position = pointerEventData.position;
		}

		public void OnEndDrag(PointerEventData pointerEventData)
		{
			//ドラッグオブジェクトを削除
			Destroy(draggingObject);	
		}

		// ドラッグオブジェクト作成
		private void CreateDragObject()
		{
			LEDCube.Block block = GetComponent<LEDCube.Block>();
			isClone = block.IsClone();
			if (isClone)
			{
				draggingObject = this.gameObject;
				orderManager.RemoveBlock(block);
			}
			else
			{
				draggingObject = Instantiate(block.gameObject);
			}
			draggingObject.transform.SetParent(canvasTran);
			draggingObject.transform.SetAsLastSibling();
			draggingObject.transform.localScale = Vector3.one;

			// レイキャストがブロックされないように
			CanvasGroup canvasGroup = draggingObject.AddComponent<CanvasGroup>();
			canvasGroup.blocksRaycasts = false;
		}
	}
}