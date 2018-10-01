using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class Player : MonoBehaviour
{

    private static Player instance;
    public static Player Instance
    {
        get
        {
            return instance;
        }
    }

    private static Enums.CameraMode camMode;
    public static Enums.CameraMode CamMode
    {
        get
        {
            return camMode;
        }
    }
    public bool building;

    #region CAMERAS
    [SerializeField]
    private Camera FPPCamera;
    [SerializeField]
    private Camera TPPCamera;
    [SerializeField]
    private Camera IzoCamera;
    [SerializeField]
    private Camera StrategyCamera;
    #endregion

    [SerializeField]
    private GameObject RaycastBall;

    private Transform IZOCAMResetTransform;

    private void Awake()
    {
        instance = this;
        camMode = Enums.CameraMode.FPP;
        IZOCAMResetTransform = IzoCamera.gameObject.transform;
        IzoCamera.enabled = false;
        IzoCamera.gameObject.tag = "MainCamera";
        RaycastBall.SetActive(false);
        //Mob.MobInstances[0].gameObject.SetActive(true);
    }


    private void FixedUpdate()
    {
        RaycastBall.SetActive(false);
    }
    private bool started;

    #region CONTROLS_LOOP

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        SmoothTerrainModify.Instance.LoadChunks(Player.Instance.gameObject.transform.position, 256);

        if (started)
        {
            switch (camMode)
            {
                case Enums.CameraMode.FPP:
                    FPPCamControls();
                    break;
                case Enums.CameraMode.Izometric:
                    IZOCamControls();
                    break;
                case Enums.CameraMode.Strategy:
                    STRATEGYCamControls();
                    break;
                case Enums.CameraMode.TPP:
                    TPPCamControls();
                    break;
            }
        }
        else
        {
            if (Input.GetButtonDown("BuildCityCentre"))
            {
                City.Instance.enabled = true;
                SmoothTerrainModify.Instance.spawnStartHouse((int)Player.Instance.gameObject.transform.position.x, (int)Player.Instance.gameObject.transform.position.z);
                started = true;
            }
        }
    }

    public GameObject GetBall()
    {
        return RaycastBall;
    }



    private void FPPCamControls()
    {
        if (Input.GetKeyUp(KeyCode.Tab))
        {
            SmoothTerrainModify.Instance.locked = !SmoothTerrainModify.Instance.locked;
        }

        if (Input.GetMouseButton(1) && !SmoothTerrainModify.Instance.locked)
        {
            SmoothTerrainModify.Instance.RightClickFpp(6.0f);
        }

        if (Input.GetMouseButton(0) && !SmoothTerrainModify.Instance.locked)
        {
            SmoothTerrainModify.Instance.LeftClickFpp(6.0f);
        }


        if (SmoothTerrainModify.Instance.locked)
        {
            this.gameObject.GetComponent<FirstPersonController>().LookEnabled = false;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            this.gameObject.GetComponent<FirstPersonController>().LookEnabled = true;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        if (Input.GetButtonDown("ToggleIzoCam") && !SmoothTerrainModify.Instance.locked)
        {
            SetIzometricCamera();
        }
    }

    private void IZOCamControls()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        if (!building)
        {
            if (Input.GetButtonDown("ToggleIzoCam"))
            {
                SetFPPCamera();
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            SmoothTerrainModify.Instance.LeftClickIzo();
        }

        if (Input.GetMouseButtonDown(1))
        {
            SmoothTerrainModify.Instance.RightClickIzo();
        }

        if (Input.GetButton("RotateIzoLeft"))
        {
            IzoCamera.gameObject.transform.RotateAround(Player.Instance.gameObject.transform.position, new Vector3(0, 1, 0), 1.0f);
        }

        if (Input.GetButton("RotateIzoRight"))
        {
            IzoCamera.gameObject.transform.RotateAround(Player.Instance.gameObject.transform.position, new Vector3(0, 1, 0), -1.0f);
        }
    }

    private void STRATEGYCamControls()
    {

    }

    private void TPPCamControls()
    {

    }

    #endregion

    #region CAMERA_MANAGERS

    private void SetFPPCamera()
    {
        IzoCamera.transform.position = IZOCAMResetTransform.position;
        IzoCamera.transform.rotation = IZOCAMResetTransform.rotation;
        Player.Instance.gameObject.GetComponent<FirstPersonController>().izometricView = false;
        BuildGUIManager.Instance.UnBindZone();
        SmoothTerrainModify.Instance.locked = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        FPPCamera.enabled = true;
        IzoCamera.enabled = false;
        camMode = Enums.CameraMode.FPP;
        NewBuildGUIManager.Instance.gameObject.SetActive(false);
    }

    private void SetIzometricCamera()
    {
        BuildGUIManager.Instance.UnBindZone();
        Player.Instance.gameObject.GetComponent<FirstPersonController>().izometricView = true;
        SmoothTerrainModify.Instance.locked = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        FPPCamera.enabled = false;
        IzoCamera.enabled = true;
        camMode = Enums.CameraMode.Izometric;
        NewBuildGUIManager.Instance.gameObject.SetActive(true);
    }

    private void SetStrategyCamera()
    {
        FPPCamera.enabled = false;
        IzoCamera.enabled = false;
        NewBuildGUIManager.Instance.gameObject.SetActive(true);
    }

    private void SetTPPCamera()
    {
        FPPCamera.enabled = false;
        IzoCamera.enabled = false;
        NewBuildGUIManager.Instance.gameObject.SetActive(false);
    }

    #endregion

    #region COROUTINES



    #endregion
}
