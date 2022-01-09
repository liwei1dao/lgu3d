using UnityEngine;
using System.Collections;
//using DG.Tweening;
using System;
using Unity.Collections;
public class CameraRotateAround : MonoBehaviour
{
  private Transform mainCamera;
  [Header("目标点")] public Transform targetPos;
  [Header("初始化角度")] public Vector3 InitRotation = new Vector3(45, 0, 0);

  [Header("旋转速度")] public float RationSpeed = 10f;
  [Header("旋转平滑速度")] public float rationSmoothSpeed = 10f;
  [Header("位置平滑速度")] public float posSmoothSpeed = 20f;
  [Header("滑轮的速度")] public float zoomSpeed = 40f;
  [Header("滑轮移动的平滑速度")] public float zoomDampening = 5f;
  [Header("最小的旋转")] public float LimitMinRationX = 20f;
  [Header("最大的旋转")] public float LimitMaxRationX = 60f;
  [Header("移动时间")] public float durtion = 2f;
  [Header("开始位置的高度")] public float StartPosHeight = 500f;
  [Header("旋转半径")] public float R = 240f;
  [Header("滑轮的最小距离")] public float minR = 200f;

  [Header("自动获取半径")] public bool AutoR = false;
  [Header("旋转隐藏鼠标")] public bool CursorEnable = false;

  private float currentR;
  private float computeR;
  private float x, y;
  private float wheel;

  private bool LimitCamreaRation = false;    //限制鼠标控制相机旋转

  private Vector3 targetDir;               //向量方向
  private Vector3 first_Direction;         //开始移动时的方向
  private Vector3 second_Direction;        //当移动到目标位置后，通过方向算出的第二个方向
  private Vector3 end;
  private Vector3 computeEnd;
  private Vector3 targetPosVector3;

  private Quaternion Rotation;             //自动获取的旋转
  private Quaternion RotationA;            //旋转变量A 

  private void Start()
  {
    mainCamera = Camera.main.transform;
  }
  private void LateUpdate()
  {
    if (Input.GetKeyDown(KeyCode.A))
    {
      InitCameraRotateAround();
    }
    if (Input.GetMouseButton(1) && LimitCamreaRation)
    {
      ControlCamrea();
    }
    else
    {
      Cursor.visible = true;
    }
    ContorlScroll();
  }
  private void InitCameraRotateAround()
  {
    LimitCamreaRation = false;
    targetPosVector3 = targetPos.position;
    if (InitRotation == Vector3.zero || AutoR)
    {
      Rotation = mainCamera.rotation;
      x = Rotation.eulerAngles.y;
      y = Rotation.eulerAngles.x;
    }
    else
    {
      Rotation.eulerAngles = InitRotation;
      x = InitRotation.y;
      y = InitRotation.x;
    }
    if (AutoR)
    {
      R = Vector3.Distance(targetPos.position, mainCamera.position);
    }
    else
    {
      targetDir = (targetPosVector3 - mainCamera.position).normalized;
    }
    currentR = R;
    computeR = R;
    RotationA = Rotation;
    second_Direction = Rotation * Vector3.forward;
    end = targetPosVector3 - second_Direction * currentR;
    if (AutoR)
      StartCoroutine("CorrectInitPos");
    else
      CamreaMove();
  }
  private void ControlCamrea()
  {
    if (CursorEnable)
      Cursor.visible = false;
    x += Input.GetAxis("Mouse X") * RationSpeed;
    y -= Input.GetAxis("Mouse Y") * RationSpeed;
    y = Mathf.Clamp(y, LimitMinRationX, LimitMaxRationX);
    RotationA = Quaternion.Slerp(RotationA, Quaternion.Euler(y, x, 0), rationSmoothSpeed * Time.deltaTime);
    first_Direction = RotationA * targetDir;
    end = targetPosVector3 - first_Direction * currentR;
    SetCamera(RotationA, Vector3.Slerp(end, (targetPosVector3 - first_Direction * currentR), posSmoothSpeed * 0.02F));
  }
  private void CamreaMove()
  {
    SetCamera(Rotation, end);
  }
  private void SetCamera(Quaternion Rotation, Vector3 end)
  {
    mainCamera.rotation = Rotation;
    mainCamera.position = end;
    targetDir = Vector3.forward;
    LimitCamreaRation = true;
  }
  private void ContorlScroll()
  {
    wheel = Input.GetAxis("Mouse ScrollWheel");
    if (wheel != 0 && LimitCamreaRation)
    {
      computeR -= (wheel > 0 ? 0.1F : -0.1F) * zoomSpeed;
      computeR = Mathf.Clamp(computeR, minR, R);
    }
    if (Mathf.Abs(Mathf.Abs(computeR) - Mathf.Abs(currentR)) >= 0.1f)
    {
      currentR = Mathf.Lerp(currentR, computeR, Time.deltaTime * zoomDampening);
      computeEnd = targetPosVector3 - (RotationA * Vector3.forward * currentR);
      mainCamera.position = computeEnd;
    }
  }
  private IEnumerator CorrectInitPos()
  {
    while (true)
    {
      if (Vector3.Distance(mainCamera.position, end) >= 0.01f)
      {
        mainCamera.rotation = Rotation;
        mainCamera.position = Vector3.Lerp(mainCamera.position, end, 0.15f);
        yield return null;
      }
      else
      {
        mainCamera.rotation = Rotation;
        mainCamera.position = end;
        targetDir = Vector3.forward;
        LimitCamreaRation = true;
        StopCoroutine("CorrectInitPos");
        break;
      }
    }
  }
}