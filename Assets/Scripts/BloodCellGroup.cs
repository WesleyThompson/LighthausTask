namespace Lighthaus.Interview
{
    using UnityEngine;

    public class BloodCellGroup : MonoBehaviour
    {
        [SerializeField] private float cellSpreadRadius;

        private Transform[] _bloodCellTransforms;

        [ContextMenu("Randomize")]
        public void RandomizeBloodCells()
        {
            if(_bloodCellTransforms == null || _bloodCellTransforms.Length <= 1)
            {
                _bloodCellTransforms = GetComponentsInChildren<Transform>();
                //First element of GetComponentInChildren is the groups transform
                if (_bloodCellTransforms == null || _bloodCellTransforms.Length <= 1)
                {
                    Debug.LogWarning("BloodCellGroup missing children");
                    return;
                }
            }

            for (int i = 1; i < _bloodCellTransforms.Length; i++)
            {
                Vector3 randPosition = new Vector3(Random.Range(-cellSpreadRadius, cellSpreadRadius),
                                                Random.Range(-cellSpreadRadius, cellSpreadRadius),
                                                Random.Range(-cellSpreadRadius, cellSpreadRadius));

                Quaternion randRotation = Random.rotation;

                _bloodCellTransforms[i].localPosition = randPosition;
                _bloodCellTransforms[i].localRotation = randRotation;
            }
        }

    }
}