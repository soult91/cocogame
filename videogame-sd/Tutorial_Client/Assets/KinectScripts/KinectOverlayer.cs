using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KinectOverlayer : MonoBehaviour {
	//	public Vector3 TopLeft;
	//	public Vector3 TopRight;
	//	public Vector3 BottomRight;
	//	public Vector3 BottomLeft;

	// public GUITexture backgroundImage;
	public KinectWrapper.NuiSkeletonPositionIndex TrackedJoint = KinectWrapper.NuiSkeletonPositionIndex.HandLeft;
	public KinectWrapper.NuiSkeletonPositionIndex TrackedJoint1 = KinectWrapper.NuiSkeletonPositionIndex.HandRight;

	public float smoothFactor = 5f;

	public GameObject Object0;
	public GameObject Object1;
	public GameObject Object2;
	public GameObject Object3;
	public GameObject Object4;
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

						// posición objeto 0
						float objectX = Object0.transform.position.x;
						float objectY = Object0.transform.position.y;
						float objectZ = Object0.transform.position.z;

						float object1X = Object1.transform.position.x;
						float object1Y = Object1.transform.position.y;
						float object1Z = Object1.transform.position.z;

						float object2X = Object2.transform.position.x;
						float object2Y = Object2.transform.position.y;
						float object2Z = Object2.transform.position.z;

						float object3X = Object3.transform.position.x;
						float object3Y = Object3.transform.position.y;
						float object3Z = Object3.transform.position.z;

						float object4X = Object4.transform.position.x;
						float object4Y = Object4.transform.position.y;
						float object4Z = Object4.transform.position.z;

						float object5X = Object5.transform.position.x;
						float object5Y = Object5.transform.position.y;
						float object5Z = Object5.transform.position.z;


						//Distancia de los 3 objetos de la derecha a la mano derecha (izq del avatar) 
						
						// distancia mano izquierda a objeto 0
						float distanciaX = objectX - jointX;
						float distanciaY = objectY - jointY;

						float distancia1X = object1X - jointX;
						float distancia1Y = object1Y - jointY;

						float distancia2X = object2X - jointX;
						float distancia2Y = object2Y - jointY;

						//Distancia de los 3 objetos de la izquierda a la mano izquierda (der del avatar) 

						float distancia3X = object3X - jointX1;
						float distancia3Y = object3Y - jointY1;

						float distancia4X = object4X - jointX1;
						float distancia4Y = object4Y - jointY1;

						float distancia5X = object5X - jointX1;
						float distancia5Y = object5Y - jointY1;

						int ObjClicked0 = 0;
						int ObjClicked1 = 1;
						int ObjClicked2 = 2;
						int ObjClicked3 = 3;
						int ObjClicked4 = 4;
						int ObjClicked5 = 5;

						// Debug.Log("Distancia X: " + distanciaX);
						// Debug.Log("Distancia Y: "+ distanciaY);

						if (listClicked.Count < 7) {

							//Checkeamos la distancia de la mano al objeto0

							if (distanciaY < 0.8 && distanciaY > 0.65 && distanciaX < 1.6 && distanciaX > 0.51) {
								Object0.transform.Rotate (0, -60, 0);
								if (!listClicked.Contains (ObjClicked0)) {
									listClicked.Add (ObjClicked0);
									if (ObjClicked0 == listClicked.IndexOf (ObjClicked0)) {
										GameObject.Find ("InfoText2").GetComponent<TextMesh> ().text = "";
										GameObject.Find ("InfoText").GetComponent<TextMesh> ().text = "¡Muy bien!";
										score++;
										GameObject.Find ("ScoreText").GetComponent<TextMesh> ().text = score.ToString ();
										Client.Instance.SendScorePlan (score, time);
										// StartCoroutine (DelayUpd ());
									} else {
										GameObject.Find ("InfoText").GetComponent<TextMesh> ().text = "";

										GameObject.Find ("InfoText2").GetComponent<TextMesh> ().text = "Es el turno de otro objeto";

										listClicked.Remove (ObjClicked0);
									}
								} else {
									GameObject.Find ("InfoText").GetComponent<TextMesh> ().text = "";

									GameObject.Find ("InfoText2").GetComponent<TextMesh> ().text = "Es el turno de otro objeto";
								}
							}

							Debug.Log ("X: " + distancia1X);
							Debug.Log ("Y: " + distancia1Y);

							//Checkeamos la distancia de la mano al objeto1

							if (distancia1Y < 0.71 && distancia1Y > 0.61 && distancia1X < 0.61 && distancia1X > 0.55) {

								Object1.transform.Rotate (0, -60, 0);
								if (!listClicked.Contains (ObjClicked1)) {
									listClicked.Add (ObjClicked1);
									if (listClicked.IndexOf (ObjClicked1) == ObjClicked1) {
										GameObject.Find ("InfoText2").GetComponent<TextMesh> ().text = "";
										GameObject.Find ("InfoText").GetComponent<TextMesh> ().text = "¡Muy bien!";
										score++;
										GameObject.Find ("ScoreText").GetComponent<TextMesh> ().text = score.ToString ();
										Client.Instance.SendScorePlan (score, time);
										// StartCoroutine (DelayUpd ());
									} else {
										GameObject.Find ("InfoText").GetComponent<TextMesh> ().text = "";

										GameObject.Find ("InfoText2").GetComponent<TextMesh> ().text = "Es el turno de otro objeto";

										listClicked.Remove (ObjClicked0);
									}
								} else {
									GameObject.Find ("InfoText").GetComponent<TextMesh> ().text = "";

									GameObject.Find ("InfoText2").GetComponent<TextMesh> ().text = "Es el turno de otro objeto";

									GameObject.Find ("InfoText2").GetComponent<TextMesh> ().text = "Es el turno de otro objeto";
								}

							}

							//Checkeamos la distancia de la mano al objeto2

							if (distancia2Y < 0.55 && distancia2Y > 0.44 && distancia2X < 0.70 && distancia2X > 0.65) {

								Object2.transform.Rotate (0, -60, 0);
								if (!listClicked.Contains (ObjClicked2)) {
									listClicked.Add (ObjClicked2);
									if (listClicked.IndexOf (ObjClicked2) == ObjClicked2) {
										GameObject.Find ("InfoText2").GetComponent<TextMesh> ().text = "";
										GameObject.Find ("InfoText").GetComponent<TextMesh> ().text = "¡Muy bien!";
										score++;
										GameObject.Find ("ScoreText").GetComponent<TextMesh> ().text = score.ToString ();
										StartCoroutine (DelayUpd ());
										Client.Instance.SendScorePlan (score, time);

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

							if (distancia3Y < 0.88 && distancia3Y > 0.77 && distancia3X < 3.97 && distancia3X > 3.90) {

								Object3.transform.Rotate (0, -60, 0);
								if (!listClicked.Contains (ObjClicked3)) {
									listClicked.Add (ObjClicked3);

									if (listClicked.IndexOf (ObjClicked3) == ObjClicked3) {
										GameObject.Find ("InfoText2").GetComponent<TextMesh> ().text = "";
										GameObject.Find ("InfoText").GetComponent<TextMesh> ().text = "¡Muy bien!";
										score++;
										GameObject.Find ("ScoreText").GetComponent<TextMesh> ().text = score.ToString ();
										StartCoroutine (DelayUpd ());
										Client.Instance.SendScorePlan (score, time);

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

							//Checkeamos la distancia de la mano al objeto4

							if (distancia4Y < 0.75 && distancia4Y > 0.69 && distancia4X < 3.91 && distancia4X > 3.85) {

								Object4.transform.Rotate (0, -60, 0);
								if (!listClicked.Contains (ObjClicked4)) {
									listClicked.Add (ObjClicked4);

									if (listClicked.IndexOf (ObjClicked4) == ObjClicked4) {
										Debug.Log ("entra");
										GameObject.Find ("InfoText2").GetComponent<TextMesh> ().text = "";
										GameObject.Find ("InfoText").GetComponent<TextMesh> ().text = "¡Muy bien!";
										score++;
										GameObject.Find ("ScoreText").GetComponent<TextMesh> ().text = score.ToString ();
										Client.Instance.SendScorePlan (score, time);

										StartCoroutine (DelayUpd ());
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

							//Checkeamos la distancia de la mano al objeto5

							if (distancia5Y < 0.57 && distancia5Y > 0.45 && distancia5X < 3.83 && distancia5X > 3.78) {

								Object5.transform.Rotate (0, -60, 0);
								if (!listClicked.Contains (ObjClicked5)) {
									listClicked.Add (ObjClicked5);

									if (listClicked.IndexOf (ObjClicked5) == ObjClicked5) {
										Debug.Log ("entra");
										GameObject.Find ("InfoText2").GetComponent<TextMesh> ().text = "";
										GameObject.Find ("InfoText").GetComponent<TextMesh> ().text = "¡Muy bien!";
										score++;
										GameObject.Find ("ScoreText").GetComponent<TextMesh> ().text = score.ToString ();
										Client.Instance.SendScorePlan (score, time);

										StartCoroutine (DelayUpd ());
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
							listClicked[1] == listClicked.IndexOf (ObjClicked1) &&
							listClicked[2] == listClicked.IndexOf (ObjClicked2) &&
							listClicked[3] == listClicked.IndexOf (ObjClicked3) &&
							listClicked[4] == listClicked.IndexOf (ObjClicked4) &&
							listClicked[5] == listClicked.IndexOf (ObjClicked5)) {
							GameObject.Find ("InfoText2").GetComponent<TextMesh> ().text = "";
							GameObject.Find ("InfoText").GetComponent<TextMesh> ().text = "¡Has terminado!";
							Client.Instance.SendScorePlan (score, time);
							Debug.Log (time);
							Destroy (manager);
							SceneManager.LoadScene ("LevelsBathroom");
						} else if (listClicked.Count == 6 &&
							(!(listClicked[0] == listClicked.IndexOf (ObjClicked0)) ||
								!(listClicked[1] == listClicked.IndexOf (ObjClicked1)) ||
								!(listClicked[2] == listClicked.IndexOf (ObjClicked2)) ||
								!(listClicked[3] == listClicked.IndexOf (ObjClicked3)) ||
								!(listClicked[4] == listClicked.IndexOf (ObjClicked4)) ||
								!(listClicked[5] == listClicked.IndexOf (ObjClicked5)))) {
							GameObject.Find ("InfoText").GetComponent<TextMesh> ().text = "Inténtalo de nuevo";

						}
					}
				}

			}

		}
	}
}