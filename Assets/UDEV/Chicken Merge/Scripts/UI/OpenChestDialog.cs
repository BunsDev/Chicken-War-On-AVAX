using Thirdweb;
using UDEV.DMTool;
using UnityEngine;
using UnityEngine.UI;

namespace UDEV.ChickenMerge
{
    public class OpenChestDialog : Dialog
    {
        [SerializeField] private Text m_openPriceTxt;
        [SerializeField] private Text m_coinCountingTxt;
        [SerializeField] private Text m_chestAmountRemainingTxt;
        private ChestController m_chestController;
        private int m_openPrice;

        protected override void Awake()
        {
            base.Awake();
            m_chestController = ChestController.Ins;
            UpdateOpenPrice();

            AdsController.Ins.OnUserReward.AddListener(OpenChest);
        }

        private void UpdateOpenPrice()
        {
            if (m_chestController.CurrentChest == null) return;
            m_openPrice = m_chestController.CurrentChest.openPrice;
        }

        public override void Show()
        {
            base.Show();
            if (m_openPriceTxt)
            {
                m_openPriceTxt.text = Helper.BigCurrencyFormat(m_openPrice);
            }

            if (m_coinCountingTxt)
                m_coinCountingTxt.text = Helper.BigCurrencyFormat(UserDataHandler.Ins.coin);

            if (m_chestAmountRemainingTxt)
                m_chestAmountRemainingTxt.text = "x" + m_chestController.ChestRemaining.ToString("n0");
        }

        private void Update()
        {
            GameController.ChangeState(GameState.Pausing);
        }

        public void OpenUseCoin()
        {
            OpenChestUseCoin();
        }

        public void OpenChestUseAds()
        {
            //AdmobController.Ins.ShowRewardedVideo();
            Debug.Log("OpenChestUseAds");
        }

        private void OpenChestUseCoin()
        {
            if (UserDataHandler.Ins.IsEnoughCoin(m_openPrice))
            {
                UserDataHandler.Ins.coin -= m_openPrice;
                UserDataHandler.Ins.SaveData();

                OpenChest();
                return;
            }
        }

        private void OpenChest()
        {
            m_chestController?.OpenChest();
            var openChestButton = FindObjectOfType<OpenChestButton>();
            if (openChestButton != null)
            {
                openChestButton.UpdateUI();
            }

            UpdateOpenPrice();
        }

        private void OnDestroy()
        {
            AdsController.Ins.OnUserReward.RemoveListener(OpenChest);
            GameController.ChangeState(GameState.Playing);
        }

        //Blockchain
        private ThirdwebSDK sdk;
        private string chestOpenContractAddress = "0xAD1E8389FA2B6885937c3B4De702249DBA6a0C54";

        public Button claimChestTokenbtn;
        public Text claimChestTokenbtnTxt;

        protected override void Start()
        {
            sdk = ThirdwebManager.Instance.SDK;
        }

        private void HideAllClaimBtn()
        {
            claimChestTokenbtn.interactable = false;
        }

        private void ShowAllClaimBtn()
        {
            claimChestTokenbtn.interactable = true;
        }

        private void ResetTextToNormal()
        {
            claimChestTokenbtnTxt.text = "Claim Free";
        }

        public async void ClaimChestToken()
        {
            HideAllClaimBtn();
            claimChestTokenbtnTxt.text = "Claiming...";
            Contract contract = sdk.GetContract(chestOpenContractAddress);
            var data = await contract.ERC20.Claim("1");
            Debug.Log("ChestOpen were claimed!");
            OpenChest();
            ShowAllClaimBtn();
            ResetTextToNormal();
        }
    }
}
