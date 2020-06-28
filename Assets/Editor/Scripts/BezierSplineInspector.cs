namespace Lighthaus.Interview.Editor
{
    using UnityEditor;
    using UnityEngine;

    [CustomEditor(typeof(BezierSpline))]
    public class BezierSplineInspector : Editor
    {
        private BezierSpline _bSpline;
        private Transform _handleTransform;
        private Quaternion _handleRotation;
        private int _selectIndex = -1;

        private const int CurveStepCount = 10;
        private const float DirectionScale = 0.5f;
        private const float HandleSize = 0.05f;
        private const float PickSize = 0.07f;
        private const string ButtonText = "Add a curve";
        private const string ModeEnumFieldName = "Mode";
        private const string LoopFieldName = "Loop";

        private readonly static Color[] modeColors =
        {
            Color.green,
            Color.blue,
            Color.red
        };

        public override void OnInspectorGUI()
        {
            _bSpline = target as BezierSpline;
            EditorGUI.BeginChangeCheck();
            bool loop = EditorGUILayout.Toggle(LoopFieldName, _bSpline.Loop);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(_bSpline, "Toggle loop");
                EditorUtility.SetDirty(_bSpline);
                _bSpline.Loop = loop;
            }

            if (_selectIndex >= 0 && _selectIndex < _bSpline.ControlPointCount)
            {
                DrawSelectedPointInspector();
            }

            if (GUILayout.Button(ButtonText))
            {
                Undo.RecordObject(_bSpline, ButtonText);
                _bSpline.AddCurve();
                EditorUtility.SetDirty(_bSpline);
            }
        }

        private void OnSceneGUI()
        {
            _bSpline = target as BezierSpline;
            _handleTransform = _bSpline.transform;
            _handleRotation = Tools.pivotRotation == PivotRotation.Local ? _handleTransform.rotation : Quaternion.identity;

            Vector3 p0 = ShowPoint(0);
            for (int i = 1; i < _bSpline.ControlPointCount; i += 3)
            {
                Vector3 p1 = ShowPoint(i);
                Vector3 p2 = ShowPoint(i + 1);
                Vector3 p3 = ShowPoint(i + 2);

                Handles.color = Color.cyan;
                Handles.DrawLine(p0, p1);
                Handles.DrawLine(p2, p3);

                Handles.DrawBezier(p0, p3, p1, p2, Color.white, null, 2f);
                p0 = p3;
            }
            
            ShowDirection();
        }

        private Vector3 ShowPoint(int index)
        {
            Vector3 point = _handleTransform.TransformPoint(_bSpline.GetControlPoint(index));

            float size = HandleUtility.GetHandleSize(point); //factor in screensize to adjust how big buttons are
            if(index == 0)
            {
                size *= 2f;
            }
            Handles.color = modeColors[(int) _bSpline.GetControlPointMode(index)];
            if (Handles.Button(point, _handleRotation, size * HandleSize, size * PickSize, Handles.DotHandleCap))
            {
                _selectIndex = index;
                Repaint();
            }
            if (_selectIndex == index)
            {
                EditorGUI.BeginChangeCheck();
                point = Handles.DoPositionHandle(point, _handleRotation);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(_bSpline, "Moved point");
                    EditorUtility.SetDirty(_bSpline);
                    _bSpline.SetControlPoint(index, _handleTransform.InverseTransformPoint(point));
                }
            }
            
            return point;
        }

        private void ShowDirection()
        {
            Handles.color = Color.yellow;
            Vector3 point = _bSpline.GetPoint(0f);
            Handles.DrawLine(point, point + _bSpline.GetDirection(0f) * DirectionScale);

            int steps = CurveStepCount * _bSpline.CurveCount;
            for (int i = 0; i <= steps; i++)
            {
                point = _bSpline.GetPoint(i / (float) steps);
                Handles.DrawLine(point, point + _bSpline.GetDirection(i / (float) steps) * DirectionScale);
            }
        }

        private void DrawSelectedPointInspector()
        {
            GUILayout.Label("Selected point");
            EditorGUI.BeginChangeCheck();
            Vector3 point = EditorGUILayout.Vector3Field("Position", _bSpline.GetControlPoint(_selectIndex));
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(_bSpline, "Moved point");
                EditorUtility.SetDirty(_bSpline);
                _bSpline.SetControlPoint(_selectIndex, point);
            }

            //Mode stuff
            EditorGUI.BeginChangeCheck();
            BezierControlPointMode mode = (BezierControlPointMode) EditorGUILayout.EnumPopup(ModeEnumFieldName, _bSpline.GetControlPointMode(_selectIndex));
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(_bSpline, "Change control point mode");
                _bSpline.SetControlPointMode(_selectIndex, mode);
                EditorUtility.SetDirty(_bSpline);
            }
        }
    }
}