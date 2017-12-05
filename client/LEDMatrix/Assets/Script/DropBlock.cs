using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace LEDCube
{
	public class DropBlock : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
	{
		[SerializeField]
		private Transform scriptArea;
		private Block block;

		[SerializeField]
		private OrderManager orderManager;

		[SerializeField]
		Image line;

		void Start()
		{
			block = this.gameObject.GetComponent<Block>();
		}

		public void OnPointerEnter(PointerEventData pointerEventData)
		{
			line.gameObject.SetActive(true);
		}

		public void OnPointerExit(PointerEventData pointerEventData)
		{
			line.gameObject.SetActive(false);
		}

		public void OnDrop(PointerEventData pointerEventData)
		{
			Block droppedBlock = pointerEventData.pointerDrag.GetComponent<Block>();
			if(droppedBlock != null && block != null)
			{
				orderManager.AddBlock(CreateDroppedBlock(droppedBlock), block);
			}
		}

		public Block CreateDroppedBlock(Block droppedBlock)
		{
			GameObject droppedGameObject;

			droppedGameObject = Instantiate(droppedBlock.gameObject);
			Block block = droppedGameObject.GetComponent<Block>();

			block.SetIsClone(true);
			block.GetComponent<DropBlock>().enabled = true;
			block.transform.SetParent(scriptArea);
			block.transform.SetAsLastSibling();
			block.transform.localScale = Vector3.one;
			block.transform.localPosition = CreateDroppedPos();

			CanvasGroup canvasGroup = droppedGameObject.GetComponent<CanvasGroup>();
			if (canvasGroup != null)
			{
				Destroy(canvasGroup);
			}

			return block;
		}

		Vector3 CreateDroppedPos()
		{
			Vector3 pos = transform.localPosition;
			pos.y -=GetComponent<RectTransform>().rect.height;
			//Debug.Log(pos.x +", "+pos.y +", "+pos.z);
			return pos;
		}
	
	}
}