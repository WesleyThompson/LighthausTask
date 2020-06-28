namespace Lighthaus.Interview
{
    using System.Collections.Generic;

    using UnityEngine;

    public class BloodFlowController : MonoBehaviour
    {
        [SerializeField] private BezierSpline spline;
        [SerializeField] private GameObject bloodCellGroupPrefab;
        [SerializeField, Range(0, 25)] private int frequency = 10;
        [Header("Flow Color")]
        [SerializeField] private Material bloodCellMatRef;
        [SerializeField] private Color bloodFlowColor = Color.red;
        [Header("Speed Control")]
        [SerializeField] private float lapDuration = 1f;

        private const string ColorKey = "_Color";

        private List<GameObject> _bloodCellGroups = new List<GameObject>();

        private void OnValidate()
        {
            bloodCellMatRef.SetColor(ColorKey, bloodFlowColor);

            for(int i = 0; i < _bloodCellGroups.Count; i++)
            {
                _bloodCellGroups[i].GetComponent<BezierSplineTraveller>().SetDuration(lapDuration);
            }
        }

        private void Awake()
        {
            GenerateBloodCellGroups();
        }

        public void GenerateBloodCellGroups()
        {
            //Clear 'em
            for (int i = 0; i < _bloodCellGroups.Count; i++)
            {
                DestroyImmediate(_bloodCellGroups[i]);
            }

            _bloodCellGroups.Clear();

            float stepSize = 1f / frequency;

            for (int i = 0; i < frequency; i++)
            {
                float t = stepSize * i;

                Transform bloodCellGroupTransform = Instantiate(bloodCellGroupPrefab).transform;
                _bloodCellGroups.Add(bloodCellGroupTransform.gameObject);

                BloodCellGroup bloodCellGroup = bloodCellGroupTransform.GetComponent<BloodCellGroup>(); // Probably shouldn't assume this is here but it's my prefab
                BezierSplineTraveller bSplineTraveller = bloodCellGroupTransform.GetComponent<BezierSplineTraveller>();

                //Position the cells along the bezier spline
                Vector3 position = spline.GetPoint(t);
                bloodCellGroupTransform.localPosition = position;
                bloodCellGroupTransform.LookAt(position + spline.GetDirection(t));

                //Set up traveller
                bSplineTraveller.SetSpline(spline);
                bSplineTraveller.SetProgress(t);
                bSplineTraveller.SetDuration(lapDuration);

                //Randomize cells so it looks cool
                bloodCellGroup.RandomizeBloodCells();
            }
        }
    }
}