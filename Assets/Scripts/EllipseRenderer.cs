using System;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class EllipseRenderer : MonoBehaviour
{
	public int segments = 100;
	public float xAxisRadius = 5f;
	public float yAxisRadius = 3f;

	[ReadOnly] public LineRenderer line;

	[SerializeField] private Color _color;

	void Awake()
	{
		line = GetComponent<LineRenderer>();
		CalculateEllipse();
	}

	// Call this if you modify values in real-time
	void OnValidate()
	{
		if (line == null) line = GetComponent<LineRenderer>();
		CalculateEllipse();
	}

	private void Update()
	{
		CalculateEllipse();
	}

	void CalculateEllipse()
	{
		line.positionCount = segments + 1;
		float angleStep = 360f / segments;

		for (int i = 0; i <= segments; i++)
		{
			float progressAngle = i * angleStep * Mathf.Deg2Rad;
            
			// Calculate positions using parametric formula
			float x = transform.position.x + Mathf.Cos(progressAngle) * xAxisRadius;
			float y = transform.position.y + Mathf.Sin(progressAngle) * yAxisRadius;

			line.SetPosition(i, new Vector3(x, y, 0f));
		}
	}

	public void OnPressed(bool isPressed)
	{
		if (isPressed)
		{
			var color = _color;
			
			Color.RGBToHSV(color, out float h, out float s, out float v);
			s = 100;
			color = Color.HSVToRGB(h, s, v);
			line.startColor = color;
			line.endColor = color;
		}
		else
		{
			line.startColor = _color;
			line.endColor = _color;
		}
	}
}