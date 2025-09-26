using UnityEngine;

public class MinigameLevel : MonoBehaviour
{
    [SerializeField] RenderTexture LeftTexture, RightTexture, LeftTextureBackup, RightTextureBackup;
    [SerializeField] private Camera _levelCam;

    public Player Player;

    bool _inPlay = false;

    public bool InPlay => _inPlay;


    private void Start()
    {
        GameManager.Instance.AddGameStartListener(() =>
        {
            _inPlay = true;
        });

        GameManager.Instance.AddGameEndListener(() =>
        {
            _inPlay = false;
        });
    }

    public enum CameraTexture
    {
        Left,
        Right,
        BackupLeft,
        BackupRight
    }

    public void SetCameraTexture(CameraTexture camTex)
    {
        switch (camTex)
        {
            case CameraTexture.Left:
                _levelCam.targetTexture = LeftTexture;
                break;
            case CameraTexture.Right:
                _levelCam.targetTexture = RightTexture;
                break;
            case CameraTexture.BackupLeft:
                _levelCam.targetTexture = LeftTextureBackup;
                break;
            case CameraTexture.BackupRight:
                _levelCam.targetTexture = RightTextureBackup;
                break;
        }
    }
}
