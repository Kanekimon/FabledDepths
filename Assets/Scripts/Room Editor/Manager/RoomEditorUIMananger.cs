using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine.UIElements;

namespace Assets.Scripts.Room_Editor.Manager
{
    public class RoomEditorUIMananger : Singleton<RoomEditorUIMananger>
    {
        public VisualElement root;
        public VisualElement AbsoluteContainer;

        private void OnEnable()
        {
            root = this.GetComponent<UIDocument>().rootVisualElement;
            AbsoluteContainer = root.Q<VisualElement>("VE_Absolute");
        }
    }
}
