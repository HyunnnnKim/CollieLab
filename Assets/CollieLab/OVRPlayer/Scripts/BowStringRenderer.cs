using UnityEngine;

namespace CollieLab.XR.Interactables
{
    [RequireComponent(typeof(LineRenderer))]
    public class BowStringRenderer : MonoBehaviour
    {
        #region Serialize Field
        [SerializeField] private Transform top = null;
        [SerializeField] private Transform middle = null;
        [SerializeField] private Transform bottom = null;
        [SerializeField] private Gradient pullColor = null;
        [SerializeField] private LineRenderer lineRenderer = null;

        #endregion

        #region Private Field

        #endregion

        private void Awake()
        {
            if (lineRenderer == null)
                lineRenderer = GetComponent<LineRenderer>();
        }

        private void OnEnable()
        {
            Application.onBeforeRender += UpdatePositions;

            //pullMeaserer.Pulled.AddListener(UpdateColor);
        }

        private void Update()
        {
            UpdatePositions();
        }

        private void UpdatePositions()
        {
            Vector3[] positions = new Vector3[] { top.position, middle.position, bottom.position };
            lineRenderer.SetPositions(positions);
        }

        private void UpdateColor(Vector3 pullPosition, float pullAmount)
        {
            // Using the gradient, show pull value via the string color
            Color color = pullColor.Evaluate(pullAmount);
            lineRenderer.material.color = color;
        }
    }
}
