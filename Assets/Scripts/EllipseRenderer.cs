using System;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class EllipseRenderer : MonoBehaviour
{
	public int segments = 100;
	public float xAxisRadius = 5f;
	public float yAxisRadius = 3f;

	[ReadOnly] public LineRenderer line;

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
}