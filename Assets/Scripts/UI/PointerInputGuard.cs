// 职责：判断当前触摸或鼠标输入是否落在 Unity UI 上。
using UnityEngine;
using UnityEngine.EventSystems;

namespace TowerDefense.UI
{
    public static class PointerInputGuard
    {
        public static bool IsPointerOverUi()
        {
            if (EventSystem.current == null)
            {
                return false;
            }

            if (Input.touchCount > 0)
            {
                return EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId);
            }

            return EventSystem.current.IsPointerOverGameObject();
        }
    }
}
