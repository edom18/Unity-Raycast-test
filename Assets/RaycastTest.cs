using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RaycastTest : MonoBehaviour {
		
	#region public variables
	public GameObject m_target1;
	public GameObject m_target2;
	public GameObject m_target3;
	public GameObject m_target4;
	public float m_distance = 2.0f;
	public float m_radius = 2f;
	#endregion

	#region private variables
	private int m_countLimit = 10;
	
	private List<Vector3> m_previousPositions = new List<Vector3>();

	private Material m_normalMaterial;
	private Material m_hitMaterial;
	private int m_mask = 1 << 8;
	#endregion

	
	// Use this for initialization
	void Start () {
		m_previousPositions.Add(transform.position);

		m_normalMaterial = Resources.Load("NormalMaterial", typeof(Material)) as Material;
		m_hitMaterial    = Resources.Load("HitMaterial") as Material;

		GetComponent<Renderer>().material = m_normalMaterial;
	}

	/// <summary>
	/// Gets the forward.
	/// </summary>
	/// <returns>The forward.</returns>
	Vector3 GetForward() {
		Vector3 fwd = Vector3.zero;
		Vector3 horizontal = transform.TransformDirection(Vector3.right);

		foreach (Vector3 vec in m_previousPositions) {
			fwd += vec;
		}

		fwd.Normalize();

		return fwd;
	}

	
	// Update is called once per frame
	void Update () {
		Vector3 forward    = GetForward();
		Vector3 up         = transform.TransformDirection(Vector3.up);
		Vector3 horizontal = Vector3.Cross(up, forward);
		horizontal.Normalize();

		Vector3 p1 = transform.position + (forward * 1.5f) + horizontal;
		Vector3 p2 = transform.position + (forward * 1.5f) - horizontal;
		Vector3 p3 = p1 + (forward * m_distance);
		Vector3 p4 = p2 + (forward * m_distance);

		m_target1.transform.position = p1;
		m_target2.transform.position = p2;
		m_target3.transform.position = p3;
		m_target4.transform.position = p4;

		Vector3 scale = new Vector3(m_radius, m_radius, m_radius);
		m_target1.transform.localScale = scale;
		m_target2.transform.localScale = scale;
		m_target3.transform.localScale = scale;
		m_target4.transform.localScale = scale;

		RaycastHit hit;
		if (Physics.CapsuleCast(p1, p2, m_radius, forward, out hit, m_distance, m_mask)) {
			GetComponent<Renderer>().material = m_hitMaterial;
		}
		else {
			GetComponent<Renderer>().material = m_normalMaterial;
		}

		m_previousPositions.Add(transform.position);

		if (m_previousPositions.Count > m_countLimit) {
			m_previousPositions.RemoveAt(0);
		}
	}
}
