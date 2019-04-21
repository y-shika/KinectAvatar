using UnityEditor;

namespace UTJ
{
    namespace Inspector
    {
        public class FloatSlider
        {
            public FloatSlider(string newLabel, float newLeftValue, float newRightValue)
            {
                label = newLabel;
                leftValue = newLeftValue;
                rightValue = newRightValue;
            }

            public bool Show(SerializedProperty floatProperty)
            {
                using(var check = new EditorGUI.ChangeCheckScope())
                {
                    EditorGUILayout.Slider(floatProperty, leftValue, rightValue, label);
                    return check.changed;
                }
            }

            private string label;
            private float leftValue;
            private float rightValue;
        }
    }
}
