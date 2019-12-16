using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KinectOverlayer0 : MonoBehaviour {
	public KinectWrapper.NuiSkeletonPositionIndex TrackedJoint = KinectWrapper.NuiSkeletonPositionIndex.HandLeft;
	public KinectWrapper.NuiSkeletonPositionIndex TrackedJoint1 = KinectWrapper.NuiSkeletonPositionIndex.HandRight;

	public float smoothFactor = 5f;

	public GameObject Object1;
	public GameObject Object4;

	private List<int> listClicked = new List<int> ();
	private float distanceToCamera = 10f;
	private int score = 0;
	private int time = 0;

	void Start () {
		if (Object1) {
			distanceToCamera = (Object1.transform.position - Camera.main.transform.position).magnitude;
		}
	}

	[System.Obsolete]
	void Update () {
		Game (1);
	}


	[System.Obsolete]
	void Game (float delay) {
		time++;
		KinectManager manager = KinectManager.Instance;
		if (manager && manager.IsInitialized ()) {

			int iJointIndex = (int) TrackedJoint;
			int iJointIndex1 = (int) TrackedJoint1;

			if (manager.IsUserDetected ()) {
				uint userId = manager.GetPlayer1ID ();

				if (manager.IsJointTracked (userId, iJointIndex) && manager.IsJointTracked (userId, iJointIndex1)) {
					Vector3 posJoint = manager.GetRawSkeletonJointPos (userId, iJointIndex);
					Vector3 posJoint1 = manager.GetRawSkeletonJointPos (userId, iJointIndex1);

					if (posJoint != Vector3.zero) {
						// 3d position to depth
						Vector2 posDepth = manager.GetDepthMapPosForJointPos (posJoint);

						// depth pos to color pos
						Vector2 posColor = manager.GetColorMapPosForDepthPos (posDepth);

						float scaleX = (float) posColor.x / KinectWrapper.Constants.ColorImageWidth;
						float scaleY = 1.0f - (float) posColor.y / KinectWrapper.Constants.ColorImageHeight;

						// posicion por variables de la mano izquierda
						float jointX = posJoint.x;
						float jointY = posJoint.y;
						float jointZ = posJoint.z;

						// posicion por variables de la mano derecha
						float jointX1 = posJoint1.x;
						float jointY1 = posJoint1.y;
						float jointZ1 = posJoint1.z;

						float object1X = Object1.transform.position.x;
						float object1Y = Object1.transform.position.y;
						float object1Z = Object1.transform.position.z;

						float object4X = Object4.transform.position.x;
						float object4Y = Object4.transform.position.y;
						float object4Z = Object4.transform.position.z;

						//Distancia de los 3 objetos de la derecha a la mano derecha (izq del avatar) 

						float distancia1X = object1X - jointX;
						float distancia1Y = object1Y - jointY;
						//Distancia de los 3 objetos de la izquierda a la mano izquierda (der del avatar) 

						float distancia4X = object4X - jointX1;
						float distancia4Y = object4Y - jointY1;

						int ObjClicked1 = 0;
						int ObjClicked4 = 1;

						//Aqui se controla cuando el usuario esta tocando el objeto

						//Checkeamos la distancia de la mano al objeto0
						Debug.Log (listClicked.Count);
						if (distancia1Y < 0.8 && distancia1Y > 0.6 && distancia1X < 0.47 && distancia1X > 0.4) {
							Object1.transform.Rotate (0, -60, 0);
							if (!listClicked.Contains (ObjClicked1)) {
								listClicked.Add (ObjClicked1);
								if (listClicked.IndexOf (ObjClicked1) == ObjClicked1) {
									GameObject.Find ("InfoText2").GetComponent<TextMesh> ().text = "";
									GameObject.Find ("InfoText").GetComponent<TextMesh> ().text = "¡Muy bien!";
									score++;
									GameObject.Find ("ScoreText").GetComponent<TextMesh> ().text = score.ToString ();
									Client.Instance.SendScoreBath0 (score, time);

								} else {
									GameObject.Find ("InfoText").GetComponent<TextMesh> ().text = "";
									GameObject.Find ("InfoText2").GetComponent<TextMesh> ().text = "Es el turno de otro objeto";
									listClicked.Remove (ObjClicked1);
								}
							} else {
								GameObject.Find ("InfoText").GetComponent<TextMesh> ().text = "";
								GameObject.Find ("InfoText2").GetComponent<TextMesh> ().text = "Es el turno de otro objeto";
							}

						}

						//Checkeamos la distancia de la mano al objeto4

						if (distancia4Y < 1.0 && distancia4Y > 0.60 && distancia4X < 4.0 && distancia4X > 3.95) {

							Object4.transform.Rotate (0, -60, 0);
							if (!listClicked.Contains (ObjClicked4)) {
								listClicked.Add (ObjClicked4);

								if (listClicked.IndexOf (ObjClicked4) == ObjClicked4) {
									GameObject.Find ("InfoText2").GetComponent<TextMesh> ().text = "";
									GameObject.Find ("InfoText").GetComponent<TextMesh> ().text = "¡Muy bien!";
									score++;
									GameObject.Find ("ScoreText").GetComponent<TextMesh> ().text = score.ToString ();
									Client.Instance.SendScoreBath0 (score, time);

								} else {
									GameObject.Find ("InfoText").GetComponent<TextMesh> ().text = "";
									GameObject.Find ("InfoText2").GetComponent<TextMesh> ().text = "Es el turno de otro objeto";
									listClicked.Remove (ObjClicked4);
								}
							} else {
								GameObject.Find ("InfoText").GetComponent<TextMesh> ().text = "";
								GameObject.Find ("InfoText2").GetComponent<TextMesh> ().text = "Es el turno de otro objeto";
							}

						}

						if (listClicked.Count == 2 && listClicked[0] == 0 &&
							listClicked[1] == 1) {
							GameObject.Find ("InfoText2").GetComponent<TextMesh> ().text = "";
							GameObject.Find ("InfoText").GetComponent<TextMesh> ().text = "¡Has terminado!";
							Client.Instance.SendScoreBath0 (score, time);
							Debug.Log (time);
							Destroy (manager);
							SceneManager.LoadScene ("LevelsBathroom");

						} else if (listClicked.Count == 2 && 
							(!(listClicked[0] == 0) ||
							!(listClicked[1] == 1))) {
							GameObject.Find ("InfoText2").GetComponent<TextMesh> ().text = "Inténtalo de nuevo";
						}

					}
				}

			}

		}
	}
}