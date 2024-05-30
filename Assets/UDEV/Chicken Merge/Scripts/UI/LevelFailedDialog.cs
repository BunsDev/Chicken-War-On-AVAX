using UDEV.DMTool;

namespace UDEV.ChickenMerge
{
    public class LevelFailedDialog : Dialog
    {
        public override void Show()
        {
            base.Show();
            AdmobController.Ins.ShowInterstitial();
        }

        public void BackHome()
        {
            Close();
            Helper.LoadScene(GameScene.Menu.ToString(), true);
        }

        public void Replay()
        {
            Close();
            GameController.Ins?.Replay();
        }
    }

}