using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.InputSystem;

//드래깅을 당할 수 있는 UI오브젝트들이 들고 있을 내용
//드래깅엔 3가지 스탭이 있음, 드래그시작, 드래그중, 드래그종료
public class Draggerble : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Transform originalParent; // 원래 부모패널    
    private CanvasGroup canvasGroup;

    [SerializeField] GameObject cubePrefab; // 인스펙터로 넣어줄 프리팹

    void Start()
    {        
        canvasGroup = gameObject.AddComponent<CanvasGroup>();
    }       

    public void OnBeginDrag(PointerEventData eventData) // 유니티5하고 완전 동일
    {
        //1. 원래 부모를 기억한다
        originalParent = transform.parent;

        //2. 위에서 미리 기억 해놨으니 이제 집을 나온다. 최상위 계층으로 이동(드래그 원활하게 해줌)
        transform.SetParent(originalParent.root);

        //3. 드래그 중인 이미지를 반투명
        canvasGroup.alpha = 0.5f;

        //4. 레이케스트 블라킹을 끔 (드롭 영역 감지가 원활하도록)
        canvasGroup.blocksRaycasts = false;
    }

    //드래그 중일땐, 위치 정보 등 드래그에 필요한 모든 정보가 eventData에 들어있음
    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position; // 마우스 위치로 따라감
    }

    
    public void OnEndDrag(PointerEventData eventData)
    {
        //검증, 위치확인, 패널에 놓기, 이미지 남기기, 페어런트 변경, 아니면 돌아간다
        canvasGroup.alpha = 1f; // 반투명 이미지 원복시키기        
        canvasGroup.blocksRaycasts = true; // 다시 감지 가능하게

        if (!IsDroppedOnUI(eventData))
        {
            SpawnCubeAtMousePosition();
            Destroy(gameObject); // 이건 만약 UI이미지 없애버릴거면            
        }
        else
        {
            transform.SetParent(originalParent);
        }

        ////드롭이 감지되지 않았다면? 원래 위치로 복귀
        //if (transform.parent == originalParent.root)
        //{
        //    transform.SetParent(originalParent);
        //}
    }

    //UI에 드롭시켰는지 판단할 메서드
    private bool IsDroppedOnUI(PointerEventData eventData)
    {
        //pointerEnter는 터치 혹은 마우스가 닿아있는 UI오브젝트를 가리킴
        //만약 드롭 시점에 마우스가 UI 위에 있다면 그 UI의 게임오브젝트가 담기고
        //아무 UI위에 없으면 null이 담김
        return eventData.pointerEnter != null &&
            eventData.pointerEnter.transform.IsChildOf(originalParent.root);
    }

    private void SpawnCubeAtMousePosition()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue(); // 마우스 위치 받아올 수 있음
        Ray ray = Camera.main.ScreenPointToRay(mousePos); // 마우스 위치로 레이를 만들거임
        RaycastHit hit; // 그냥 선언. 충돌 정보를 담게 될 것임

        if (Physics.Raycast(ray, out hit))
        {
            Instantiate(cubePrefab, hit.point, Quaternion.identity);
        }
    }
}
