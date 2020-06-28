namespace Lighthaus.Interview
{
    using UnityEngine;

    public class BezierSplineTraveller : MonoBehaviour
    {
        [SerializeField] private BezierSpline spline;
        [SerializeField] private float duration = 2f;
        [SerializeField] private float progress = 0f;

        private void Update()
        {
            progress += Time.deltaTime / duration;
            if (progress > 1f)
            {
                progress -= 1f;
            }
            Vector3 position = spline.GetPoint(progress);
            transform.localPosition = position;
            transform.LookAt(position + spline.GetDirection(progress));
        }

        public void SetProgress(float progress)
        {
            this.progress = progress;
        }

        public void SetSpline(BezierSpline spline)
        {
            this.spline = spline;
        }

        public void SetDuration(float duration)
        {
            this.duration = duration;
        }
    }
}