using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DressKinect1 : MonoBehaviour {
	//	public Vector3 TopLeft;
	//	public Vector3 TopRight;
	//	public Vector3 BottomRight;
	//	public Vector3 BottomLeft;

	// public GUITexture backgroundImage;
	public KinectWrapper.NuiSkeletonPositionIndex TrackedJoint = KinectWrapper.NuiSkeletonPositionIndex.HandLeft;
	public KinectWrapper.NuiSkeletonPositionIndex TrackedJoint1 = KinectWrapper.NuiSkeletonPositionIndex.HandRight;

	public float smoothFactor = 5f;

	public GameObject Object0;
	public GameObject Object2;
	public GameObject Object3;
	public GameObject Object5;

	private List<int> listClicked = new List<int> ();
	private float distanceToCamera = 10f;
	private int score = 0;
	private int time = 0;

	void Start () {
		if (Object0) {
			distanceToCamera = (Object0.transform.position - Camera.main.transform.position).magnitude;
			Debug.Log ("distanceToCamera" + distanceToCamera);
		}
	}

	[System.Obsolete]
	void Update () {
		Game (1);
	}

	IEnumerator DelayUpd () {
		yield return new WaitForSeconds (6.0f);
	}

	[System.Obsolete]
	void Game (float delay) {

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

						float objectX = Object0.transform.position.x;
						float objectY = Object0.transform.position.y;
						float objectZ = Object0.transform.position.z;

						float object2X = Object2.transform.position.x;
						float object2Y = Object2.transform.position.y;
						float object2Z = Object2.transform.position.z;

						float object3X = Object3.transform.position.x;
						float object3Y = Object3.transform.position.y;
						float object3Z = Object3.transform.position.z;

						float object5X = Object5.transform.position.x;
						float object5Y = Object5.transform.position.y;
						float object5Z = Object5.transform.position.z;

						//Distancia de los 3 objetos de la derecha a la mano derecha (izq del avatar) 

						float distanciaX = objectX - jointX;
						float distanciaY = objectY - jointY;

						float distancia2X = object2X - jointX1;
						float distancia2Y = object2Y - jointY1;

						//Distancia de los 3 objetos de la izquierda a la mano izquierda (der del avatar) 

						float distancia3X = object3X - jointX1;
						float distancia3Y = object3Y - jointY1;

						float distancia5X = object5X - jointX;
						float distancia5Y = object5Y - jointY;

						int ObjClicked0 = 0;
						int ObjClicked2 = 1;
						int ObjClicked3 = 2;
						int ObjClicked5 = 3;


						// Debug.Log("Distancia X: " + distanciaX);
						// Debug.Log("Distancia Y: "+ distanciaY);

						if (listClicked.Count < 5) {

							//Checkeamos la distancia de la mano al objeto0

							if (distanciaY < 0.08 && distanciaY > -0.05 && distanciaX < 0.57 && distanciaX > 0.49) {
								Object0.transform.Rotate (0, -60, 0);
								if (!listClicked.Contains (ObjClicked0)) {
									listClicked.Add (ObjClicked0);
									if (ObjClicked0 == listClicked.IndexOf (ObjClicked0)) {
										GameObject.Find ("InfoText").GetComponent<TextMesh> ().text = "¡Muy bien!";
										score++;
										GameObject.Find ("ScoreText").GetComponent<TextMesh> ().text = score.ToString ();
										Client.Instance.SendScoreBath1 (score, time);
										// StartCoroutine (DelayUpd ());
										//PROBAR
										// Time.timeScale = 0.5f;
									} else {
										GameObject.Find ("InfoText").GetComponent<TextMesh> ().text = "";

										GameObject.Find ("InfoText2").GetComponent<TextMesh> ().text = "Es el turno de otro objeto";
										listClicked.Remove (ObjClicked0);
									}
								} else {
									GameObject.Find ("InfoText").GetComponent<TextMesh> ().text = "";

									GameObject.Find ("InfoText2").GetComponent<TextMesh> ().text = "Es el turno de otro objeto";
									// Time.timeScale = 0.5f;
								}
							}


							//Checkeamos la distancia de la mano al objeto2

							if (distancia2Y < 0.72 && distancia2Y > 0.59 && distancia2X < 3.55 && distancia2X > 3.47) {

								Object2.transform.Rotate (0, 0, 60);
								if (!listClicked.Contains (ObjClicked2)) {
									listClicked.Add (ObjClicked2);
									if (listClicked.IndexOf (ObjClicked2) == ObjClicked2) {
										GameObject.Find ("InfoText").GetComponent<TextMesh> ().text = "¡Muy bien!";
										score++;
										GameObject.Find ("ScoreText").GetComponent<TextMesh> ().text = score.ToString ();
										StartCoroutine (DelayUpd ());
										Client.Instance.SendScoreBath1 (score, time);

									} else {
										GameObject.Find ("InfoText").GetComponent<TextMesh> ().text = "";

										GameObject.Find ("InfoText2").GetComponent<TextMesh> ().text = "Es el turno de otro objeto";
										listClicked.Remove (ObjClicked2);
									}
								} else {
									GameObject.Find ("InfoText").GetComponent<TextMesh> ().text = "";

									GameObject.Find ("InfoText2").GetComponent<TextMesh> ().text = "Es el turno de otro objeto";
								}

							}

							//Checkeamos la distancia de la mano al objeto3

							if (distancia3Y < 1.11 && distancia3Y > 1.02 && distancia3X < 2.53 && distancia3X > 2.48) {

								Object3.transform.GetChild(0).Rotate (0, 0, 60);
								if (!listClicked.Contains (ObjClicked3)) {
									listClicked.Add (ObjClicked3);

									if (listClicked.IndexOf (ObjClicked3) == ObjClicked3) {
										GameObject.Find ("InfoText").GetComponent<TextMesh> ().text = "¡Muy bien!";
										score++;
										GameObject.Find ("ScoreText").GetComponent<TextMesh> ().text = score.ToString ();
										Client.Instance.SendScoreBath1 (score, time);

									} else {
										GameObject.Find ("InfoText").GetComponent<TextMesh> ().text = "";

										GameObject.Find ("InfoText2").GetComponent<TextMesh> ().text = "Es el turno de otro objeto";
										listClicked.Remove (ObjClicked3);
									}
								} else {
									GameObject.Find ("InfoText").GetComponent<TextMesh> ().text = "";

									GameObject.Find ("InfoText2").GetComponent<TextMesh> ().text = "Es el turno de otro objeto";
								}

							}

							//Checkeamos la distancia de la mano al objeto5

							Debug.Log ("X: " + distancia5X);
							Debug.Log ("Y: " + distancia5Y);
							if (distancia5Y < 0.10 && distancia5Y > -0.01 && distancia5X < 0.58 && distancia5X > 0.55) {

								Object5.transform.GetChild(0).Rotate (0, -60, 0);
								if (!listClicked.Contains (ObjClicked5)) {
									listClicked.Add (ObjClicked5);

									if (listClicked.IndexOf (ObjClicked5) == ObjClicked5) {
										Debug.Log ("entra");
										GameObject.Find ("InfoText2").GetComponent<TextMesh> ().text = "";

										GameObject.Find ("InfoText").GetComponent<TextMesh> ().text = "¡Muy bien!";
										score++;
										GameObject.Find ("ScoreText").GetComponent<TextMesh> ().text = score.ToString ();
										Client.Instance.SendScoreBath1 (score, time);

									} else {
										GameObject.Find ("InfoText").GetComponent<TextMesh> ().text = "";

										GameObject.Find ("InfoText2").GetComponent<TextMesh> ().text = "Es el turno de otro objeto";
										listClicked.Remove (ObjClicked5);
									}
								} else {
									GameObject.Find ("InfoText").GetComponent<TextMesh> ().text = "";

									GameObject.Find ("InfoText2").GetComponent<TextMesh> ().text = "Es el turno de otro objeto";
								}

							}
						} else if (listClicked.Count == 6 &&
							listClicked[0] == listClicked.IndexOf (ObjClicked0) &&
							listClicked[2] == listClicked.IndexOf (ObjClicked2) &&
							listClicked[3] == listClicked.IndexOf (ObjClicked3) &&
							listClicked[5] == listClicked.IndexOf (ObjClicked5)) {
							GameObject.Find ("InfoText2").GetComponent<TextMesh> ().text = "";
							GameObject.Find ("InfoText").GetComponent<TextMesh> ().text = "¡Has terminado!";
							Client.Instance.SendScoreBath0 (score, time);
							Debug.Log (time);
							Destroy (manager);
							SceneManager.LoadScene ("LevelsBathroom");
						} else if (listClicked.Count == 6 &&
							(!(listClicked[0] == listClicked.IndexOf (ObjClicked0)) &&
							!(listClicked[2] == listClicked.IndexOf (ObjClicked2)) &&
							!(listClicked[3] == listClicked.IndexOf (ObjClicked3)) &&
							!(listClicked[5] == listClicked.IndexOf (ObjClicked5)))){
							GameObject.Find ("InfoText2").GetComponent<TextMesh> ().text = "Inténtalo de nuevo";

						}
					}
				}

			}

		}
	}
}