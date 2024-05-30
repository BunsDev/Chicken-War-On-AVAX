using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Thirdweb;
using UDEV.ChickenMerge;
using UDEV.DMTool;
using System.Threading;
using UnityEngine.UI;
using System.Numerics;

namespace UDEV.ChickenMerge {
    public class BuyGoldWithBlockchain : MonoBehaviour
    {
        private ThirdwebSDK sdk;
        private string gold50ContractAddress = "0xAD1E8389FA2B6885937c3B4De702249DBA6a0C54";

        private string m_buyingMessage;

        public Button ClaimGold50Tokenbtn;
        public Text ClaimGold50TokenbtnTxt;

        public Button Lotterybtn;
        public Text LotteryBtnTxt;

        public Button OpenLotterybtn;
        public Text OpenLotterybtnTxt;

        public Text AmountTxt;

        private void Start()
        {
            sdk = ThirdwebManager.Instance.SDK;
        }

        private void HideAllClaimBtn() {
            ClaimGold50Tokenbtn.interactable = false;
        }
        private void ShowAllClaimBtn()
        {
            ClaimGold50Tokenbtn.interactable = true;
        }
        private void ResetTextToNormal()
        {
            ClaimGold50TokenbtnTxt.text = "Claim Free";
        }

        public async void ClaimGold50Token()
        {
            HideAllClaimBtn();
            ClaimGold50TokenbtnTxt.text = "Claiming...";
            Contract contract = sdk.GetContract(gold50ContractAddress);
            var data = await contract.ERC20.Claim("1");
            Debug.Log("50 Gold were claimed!");
            BuyItem(100);
        }

        private void BuyItem(int coinValue)
        {
            UserDataHandler.Ins.coin += coinValue;
            UserDataHandler.Ins.SaveData();

            DialogDB.Ins.current.Close();
            RewardDialog rewardDialog = (RewardDialog)DialogDB.Ins.GetDialog(DialogType.Reward);

            if (rewardDialog)
            {
                rewardDialog.AddCoinRewardItem(coinValue);
                DialogDB.Ins.Show(rewardDialog);
            }
            ShowAllClaimBtn();
            ResetTextToNormal();
        }

        private int countdownTime = 0;

        public void BuyLotteryTicket() {
            ProduceRandomNumber();
            countdownTime = 30;
            Lotterybtn.interactable = false;
            OpenLotterybtn.gameObject.SetActive(false);
            //Run count down effect
            StartCoroutine(Countdown());            
        }

        IEnumerator Countdown()
        {
            while (countdownTime > 0)
            {
                LotteryBtnTxt.text = countdownTime.ToString();
                yield return new WaitForSeconds(1f);
                countdownTime--;
            }
            LotteryBtnTxt.text = "0";
            //Run Blockchain code

            Debug.Log("Count down end");
            //Show Open Button
            OpenLotterybtn.gameObject.SetActive(true);
            Lotterybtn.gameObject.SetActive(false);
            //Open Function
            Debug.Log("Open");
        }

        private async void ProduceRandomNumber()
        {
            var contract = ThirdwebManager.Instance.SDK.GetContract(
                    "0xD0dF3E0Fd752F8391926621Aa6B949c1f0c3Aa17",
                    "[{\"type\":\"constructor\",\"name\":\"\",\"inputs\":[{\"type\":\"uint256\",\"name\":\"subscriptionId\",\"internalType\":\"uint256\"}],\"outputs\":[],\"stateMutability\":\"nonpayable\"},{\"type\":\"error\",\"name\":\"OnlyCoordinatorCanFulfill\",\"inputs\":[{\"type\":\"address\",\"name\":\"have\",\"internalType\":\"address\"},{\"type\":\"address\",\"name\":\"want\",\"internalType\":\"address\"}],\"outputs\":[]},{\"type\":\"error\",\"name\":\"OnlyOwnerOrCoordinator\",\"inputs\":[{\"type\":\"address\",\"name\":\"have\",\"internalType\":\"address\"},{\"type\":\"address\",\"name\":\"owner\",\"internalType\":\"address\"},{\"type\":\"address\",\"name\":\"coordinator\",\"internalType\":\"address\"}],\"outputs\":[]},{\"type\":\"error\",\"name\":\"ZeroAddress\",\"inputs\":[],\"outputs\":[]},{\"type\":\"event\",\"name\":\"CoordinatorSet\",\"inputs\":[{\"type\":\"address\",\"name\":\"vrfCoordinator\",\"indexed\":false,\"internalType\":\"address\"}],\"outputs\":[],\"anonymous\":false},{\"type\":\"event\",\"name\":\"OwnershipTransferRequested\",\"inputs\":[{\"type\":\"address\",\"name\":\"from\",\"indexed\":true,\"internalType\":\"address\"},{\"type\":\"address\",\"name\":\"to\",\"indexed\":true,\"internalType\":\"address\"}],\"outputs\":[],\"anonymous\":false},{\"type\":\"event\",\"name\":\"OwnershipTransferred\",\"inputs\":[{\"type\":\"address\",\"name\":\"from\",\"indexed\":true,\"internalType\":\"address\"},{\"type\":\"address\",\"name\":\"to\",\"indexed\":true,\"internalType\":\"address\"}],\"outputs\":[],\"anonymous\":false},{\"type\":\"event\",\"name\":\"RequestFulfilled\",\"inputs\":[{\"type\":\"uint256\",\"name\":\"requestId\",\"indexed\":false,\"internalType\":\"uint256\"},{\"type\":\"uint256[]\",\"name\":\"randomWords\",\"indexed\":false,\"internalType\":\"uint256[]\"}],\"outputs\":[],\"anonymous\":false},{\"type\":\"event\",\"name\":\"RequestSent\",\"inputs\":[{\"type\":\"uint256\",\"name\":\"requestId\",\"indexed\":false,\"internalType\":\"uint256\"},{\"type\":\"uint32\",\"name\":\"numWords\",\"indexed\":false,\"internalType\":\"uint32\"}],\"outputs\":[],\"anonymous\":false},{\"type\":\"function\",\"name\":\"acceptOwnership\",\"inputs\":[],\"outputs\":[],\"stateMutability\":\"nonpayable\"},{\"type\":\"function\",\"name\":\"callbackGasLimit\",\"inputs\":[],\"outputs\":[{\"type\":\"uint32\",\"name\":\"\",\"internalType\":\"uint32\"}],\"stateMutability\":\"view\"},{\"type\":\"function\",\"name\":\"getRequestStatus\",\"inputs\":[{\"type\":\"string\",\"name\":\"_requestId\",\"internalType\":\"string\"}],\"outputs\":[{\"type\":\"string\",\"name\":\"firstRandomWord\",\"internalType\":\"string\"}],\"stateMutability\":\"view\"},{\"type\":\"function\",\"name\":\"keyHash\",\"inputs\":[],\"outputs\":[{\"type\":\"bytes32\",\"name\":\"\",\"internalType\":\"bytes32\"}],\"stateMutability\":\"view\"},{\"type\":\"function\",\"name\":\"lastRequestId\",\"inputs\":[],\"outputs\":[{\"type\":\"uint256\",\"name\":\"\",\"internalType\":\"uint256\"}],\"stateMutability\":\"view\"},{\"type\":\"function\",\"name\":\"numWords\",\"inputs\":[],\"outputs\":[{\"type\":\"uint32\",\"name\":\"\",\"internalType\":\"uint32\"}],\"stateMutability\":\"view\"},{\"type\":\"function\",\"name\":\"owner\",\"inputs\":[],\"outputs\":[{\"type\":\"address\",\"name\":\"\",\"internalType\":\"address\"}],\"stateMutability\":\"view\"},{\"type\":\"function\",\"name\":\"rawFulfillRandomWords\",\"inputs\":[{\"type\":\"uint256\",\"name\":\"requestId\",\"internalType\":\"uint256\"},{\"type\":\"uint256[]\",\"name\":\"randomWords\",\"internalType\":\"uint256[]\"}],\"outputs\":[],\"stateMutability\":\"nonpayable\"},{\"type\":\"function\",\"name\":\"requestConfirmations\",\"inputs\":[],\"outputs\":[{\"type\":\"uint16\",\"name\":\"\",\"internalType\":\"uint16\"}],\"stateMutability\":\"view\"},{\"type\":\"function\",\"name\":\"requestIds\",\"inputs\":[{\"type\":\"uint256\",\"name\":\"\",\"internalType\":\"uint256\"}],\"outputs\":[{\"type\":\"uint256\",\"name\":\"\",\"internalType\":\"uint256\"}],\"stateMutability\":\"view\"},{\"type\":\"function\",\"name\":\"requestRandomWords\",\"inputs\":[{\"type\":\"bool\",\"name\":\"enableNativePayment\",\"internalType\":\"bool\"}],\"outputs\":[{\"type\":\"uint256\",\"name\":\"requestId\",\"internalType\":\"uint256\"}],\"stateMutability\":\"nonpayable\"},{\"type\":\"function\",\"name\":\"s_requests\",\"inputs\":[{\"type\":\"uint256\",\"name\":\"\",\"internalType\":\"uint256\"}],\"outputs\":[{\"type\":\"bool\",\"name\":\"fulfilled\",\"internalType\":\"bool\"},{\"type\":\"bool\",\"name\":\"exists\",\"internalType\":\"bool\"}],\"stateMutability\":\"view\"},{\"type\":\"function\",\"name\":\"s_subscriptionId\",\"inputs\":[],\"outputs\":[{\"type\":\"uint256\",\"name\":\"\",\"internalType\":\"uint256\"}],\"stateMutability\":\"view\"},{\"type\":\"function\",\"name\":\"s_vrfCoordinator\",\"inputs\":[],\"outputs\":[{\"type\":\"address\",\"name\":\"\",\"internalType\":\"contractIVRFCoordinatorV2Plus\"}],\"stateMutability\":\"view\"},{\"type\":\"function\",\"name\":\"setCoordinator\",\"inputs\":[{\"type\":\"address\",\"name\":\"_vrfCoordinator\",\"internalType\":\"address\"}],\"outputs\":[],\"stateMutability\":\"nonpayable\"},{\"type\":\"function\",\"name\":\"transferOwnership\",\"inputs\":[{\"type\":\"address\",\"name\":\"to\",\"internalType\":\"address\"}],\"outputs\":[],\"stateMutability\":\"nonpayable\"}]"
                );
            await contract.Write("requestRandomWords", (bool)false);

            Debug.Log("ProduceRandomNumber");
        }

        private async void GetRandomNumber()
        {
            var contract = ThirdwebManager.Instance.SDK.GetContract(
                    "0xD0dF3E0Fd752F8391926621Aa6B949c1f0c3Aa17",
                    "[{\"type\":\"constructor\",\"name\":\"\",\"inputs\":[{\"type\":\"uint256\",\"name\":\"subscriptionId\",\"internalType\":\"uint256\"}],\"outputs\":[],\"stateMutability\":\"nonpayable\"},{\"type\":\"error\",\"name\":\"OnlyCoordinatorCanFulfill\",\"inputs\":[{\"type\":\"address\",\"name\":\"have\",\"internalType\":\"address\"},{\"type\":\"address\",\"name\":\"want\",\"internalType\":\"address\"}],\"outputs\":[]},{\"type\":\"error\",\"name\":\"OnlyOwnerOrCoordinator\",\"inputs\":[{\"type\":\"address\",\"name\":\"have\",\"internalType\":\"address\"},{\"type\":\"address\",\"name\":\"owner\",\"internalType\":\"address\"},{\"type\":\"address\",\"name\":\"coordinator\",\"internalType\":\"address\"}],\"outputs\":[]},{\"type\":\"error\",\"name\":\"ZeroAddress\",\"inputs\":[],\"outputs\":[]},{\"type\":\"event\",\"name\":\"CoordinatorSet\",\"inputs\":[{\"type\":\"address\",\"name\":\"vrfCoordinator\",\"indexed\":false,\"internalType\":\"address\"}],\"outputs\":[],\"anonymous\":false},{\"type\":\"event\",\"name\":\"OwnershipTransferRequested\",\"inputs\":[{\"type\":\"address\",\"name\":\"from\",\"indexed\":true,\"internalType\":\"address\"},{\"type\":\"address\",\"name\":\"to\",\"indexed\":true,\"internalType\":\"address\"}],\"outputs\":[],\"anonymous\":false},{\"type\":\"event\",\"name\":\"OwnershipTransferred\",\"inputs\":[{\"type\":\"address\",\"name\":\"from\",\"indexed\":true,\"internalType\":\"address\"},{\"type\":\"address\",\"name\":\"to\",\"indexed\":true,\"internalType\":\"address\"}],\"outputs\":[],\"anonymous\":false},{\"type\":\"event\",\"name\":\"RequestFulfilled\",\"inputs\":[{\"type\":\"uint256\",\"name\":\"requestId\",\"indexed\":false,\"internalType\":\"uint256\"},{\"type\":\"uint256[]\",\"name\":\"randomWords\",\"indexed\":false,\"internalType\":\"uint256[]\"}],\"outputs\":[],\"anonymous\":false},{\"type\":\"event\",\"name\":\"RequestSent\",\"inputs\":[{\"type\":\"uint256\",\"name\":\"requestId\",\"indexed\":false,\"internalType\":\"uint256\"},{\"type\":\"uint32\",\"name\":\"numWords\",\"indexed\":false,\"internalType\":\"uint32\"}],\"outputs\":[],\"anonymous\":false},{\"type\":\"function\",\"name\":\"acceptOwnership\",\"inputs\":[],\"outputs\":[],\"stateMutability\":\"nonpayable\"},{\"type\":\"function\",\"name\":\"callbackGasLimit\",\"inputs\":[],\"outputs\":[{\"type\":\"uint32\",\"name\":\"\",\"internalType\":\"uint32\"}],\"stateMutability\":\"view\"},{\"type\":\"function\",\"name\":\"getRequestStatus\",\"inputs\":[{\"type\":\"string\",\"name\":\"_requestId\",\"internalType\":\"string\"}],\"outputs\":[{\"type\":\"string\",\"name\":\"firstRandomWord\",\"internalType\":\"string\"}],\"stateMutability\":\"view\"},{\"type\":\"function\",\"name\":\"keyHash\",\"inputs\":[],\"outputs\":[{\"type\":\"bytes32\",\"name\":\"\",\"internalType\":\"bytes32\"}],\"stateMutability\":\"view\"},{\"type\":\"function\",\"name\":\"lastRequestId\",\"inputs\":[],\"outputs\":[{\"type\":\"uint256\",\"name\":\"\",\"internalType\":\"uint256\"}],\"stateMutability\":\"view\"},{\"type\":\"function\",\"name\":\"numWords\",\"inputs\":[],\"outputs\":[{\"type\":\"uint32\",\"name\":\"\",\"internalType\":\"uint32\"}],\"stateMutability\":\"view\"},{\"type\":\"function\",\"name\":\"owner\",\"inputs\":[],\"outputs\":[{\"type\":\"address\",\"name\":\"\",\"internalType\":\"address\"}],\"stateMutability\":\"view\"},{\"type\":\"function\",\"name\":\"rawFulfillRandomWords\",\"inputs\":[{\"type\":\"uint256\",\"name\":\"requestId\",\"internalType\":\"uint256\"},{\"type\":\"uint256[]\",\"name\":\"randomWords\",\"internalType\":\"uint256[]\"}],\"outputs\":[],\"stateMutability\":\"nonpayable\"},{\"type\":\"function\",\"name\":\"requestConfirmations\",\"inputs\":[],\"outputs\":[{\"type\":\"uint16\",\"name\":\"\",\"internalType\":\"uint16\"}],\"stateMutability\":\"view\"},{\"type\":\"function\",\"name\":\"requestIds\",\"inputs\":[{\"type\":\"uint256\",\"name\":\"\",\"internalType\":\"uint256\"}],\"outputs\":[{\"type\":\"uint256\",\"name\":\"\",\"internalType\":\"uint256\"}],\"stateMutability\":\"view\"},{\"type\":\"function\",\"name\":\"requestRandomWords\",\"inputs\":[{\"type\":\"bool\",\"name\":\"enableNativePayment\",\"internalType\":\"bool\"}],\"outputs\":[{\"type\":\"uint256\",\"name\":\"requestId\",\"internalType\":\"uint256\"}],\"stateMutability\":\"nonpayable\"},{\"type\":\"function\",\"name\":\"s_requests\",\"inputs\":[{\"type\":\"uint256\",\"name\":\"\",\"internalType\":\"uint256\"}],\"outputs\":[{\"type\":\"bool\",\"name\":\"fulfilled\",\"internalType\":\"bool\"},{\"type\":\"bool\",\"name\":\"exists\",\"internalType\":\"bool\"}],\"stateMutability\":\"view\"},{\"type\":\"function\",\"name\":\"s_subscriptionId\",\"inputs\":[],\"outputs\":[{\"type\":\"uint256\",\"name\":\"\",\"internalType\":\"uint256\"}],\"stateMutability\":\"view\"},{\"type\":\"function\",\"name\":\"s_vrfCoordinator\",\"inputs\":[],\"outputs\":[{\"type\":\"address\",\"name\":\"\",\"internalType\":\"contractIVRFCoordinatorV2Plus\"}],\"stateMutability\":\"view\"},{\"type\":\"function\",\"name\":\"setCoordinator\",\"inputs\":[{\"type\":\"address\",\"name\":\"_vrfCoordinator\",\"internalType\":\"address\"}],\"outputs\":[],\"stateMutability\":\"nonpayable\"},{\"type\":\"function\",\"name\":\"transferOwnership\",\"inputs\":[{\"type\":\"address\",\"name\":\"to\",\"internalType\":\"address\"}],\"outputs\":[],\"stateMutability\":\"nonpayable\"}]"
                );
            BigInteger latestRequestId = await contract.Read<BigInteger>("lastRequestId");
            Debug.Log("latestRequestId" + latestRequestId);
            string latestRequestIdString = latestRequestId.ToString();
            Debug.Log("latestRequestIdString" + latestRequestIdString);
            string result = await contract.Read<string>("getRequestStatus", latestRequestIdString);
            Debug.Log("result" + result);
            EvaluateString(result);
        }

        public void claim()
        {
            GetRandomNumber();
        }

        public void EvaluateString(string input)
        {
            if (int.TryParse(input, out int result))
            {
                switch (result)
                {
                    case 1:
                        AmountTxt.text = "1000";
                        BuyItem(1000);                       
                        break;
                    case 2:
                    case 3:
                        AmountTxt.text = "300";
                        BuyItem(300);                        
                        break;
                    case 4:
                    case 5:
                        AmountTxt.text = "200";
                        BuyItem(200);
                        break;
                    case 6:
                    case 7:
                        AmountTxt.text = "100";
                        BuyItem(100);
                        break;
                    default:
                        AmountTxt.text = "10";
                        BuyItem(10);
                        break;
                }
            }
        }
    }
}



