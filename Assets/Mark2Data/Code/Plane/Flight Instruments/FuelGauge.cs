#region Script Info
//inp: Plane current fuel capacity and burring rate of fuel.
//out: UI mode of fuel Gauge is move according to the capacity of fuel and burn rate.
#endregion
using UnityEngine;
using AirPlane.Fuel;
using InputActions;
using TMPro;
using UnityEngine.UI;

namespace UI.Flight_Instruments
{
    public class FuelGauge : MonoBehaviour
    {

        #region 
        /*
        #region Global Perimeters
        public InputActionManager thrustData;
        public RectTransform Pointer;
        public AircraftFuel _fuel;
        //public FlightEngine Engine;
        [Range(0f, 2f)]
        public float FuelBurnRate;
        public float MinRotation = -40f;
        public float MaxRotation = 40f;
        #endregion

        #region Global Function
        public void HandleAirplaneUI()
        {
            //if (Engine && _fuel)
            //{
            //    float rotation = Mathf.Lerp(MinRotation, MaxRotation, _fuel.NormalizedFuel);
            //    Pointer.rotation = Quaternion.Euler(0f, 0f, -rotation);
            //}

            float rotation = Mathf.Lerp(MinRotation, MaxRotation, _fuel.NormalizedFuel);
            Pointer.rotation = Quaternion.Euler(0f, 0f, -rotation);
        }
        #endregion

        #region Local Function
        private void Awake()
        {
            //if (Engine)
            //{
            //    _fuel = Engine.GetComponent<AircraftFuel>();
            //}
        }
        private void Update()
        {
            if (_fuel.NormalizedFuel < 0) return;
            FuelBurnRate = thrustData.thrust;
            _fuel.UpdateFuel(FuelBurnRate);
            HandleAirplaneUI();
        }
        #endregion*/
        #endregion
        [SerializeField] GameObject leftTank;
        [SerializeField] GameObject centerTank;
        [SerializeField] GameObject rightTank;
        [SerializeField] TextMeshProUGUI LeftText;
        [SerializeField] TextMeshProUGUI RightText;
        [SerializeField] TextMeshProUGUI centerText;
        [SerializeField] public TextMeshProUGUI RemainText;
        [SerializeField] TextMeshProUGUI FuelText;
        [SerializeField] RectTransform fuelMeter;
        [SerializeField] float maxRotation;
        [SerializeField] float minRotation;

        [SerializeField] SilantroController controller;
        public MasterSpawn masterSpawn;
        public MissionPlan missionPlan;
        [SerializeField] float maxFuel;
        [SerializeField] float currentFuel;
        [SerializeField] float percentageCompleted;

        public GameObject externalFuelTank1;
        public GameObject externalFuelTank2;

        float totalAngle;
        bool signal;
        Vector3 rotateAngles;
        public Slider leftTk;
        public Slider centerTk;
        public Slider rightTk;

        private void Awake()
        {
            if (missionPlan.ally_spawnPlanes[0].Hardpoints[0].ToLower().Contains("fuel"))
            {
                externalFuelTank1.SetActive(true);
            }

            if (missionPlan.ally_spawnPlanes[0].Hardpoints[0].ToLower().Contains("fuel"))
            {
                externalFuelTank2.SetActive(true);
            }
        }

        private void Start()
        {
            
            Invoke(nameof(Initialize), 4f);
        }
        void Initialize()
        {
            maxFuel = controller.TotalFuelCapacity;
            totalAngle = maxRotation - minRotation;
            signal = true;
            
        }
        private void FixedUpdate()
        {
            if (!signal) return;
            //rotateAngles = fuelMeter.transform.rotation.eulerAngles;
            Vector3 angles = new Vector3(0,0, CalculateConsumption());
            //fuelMeter.rotation = Quaternion.Euler(angles);
            var left = controller.fuelTanks[0].CurrentAmount/controller.fuelTanks[0].Capacity;
            var center = controller.fuelTanks[1].CurrentAmount / controller.fuelTanks[1].Capacity;
            var right = controller.fuelTanks[2].CurrentAmount / controller.fuelTanks[2].Capacity;
            if (leftTk == null)
            {
                leftTk = leftTank.transform.GetComponent<Slider>();
            }
            if (centerTk == null)
            {
                centerTk = centerTank.transform.GetComponent<Slider>();
            }
            if (rightTk == null)
            {
                rightTk = rightTank.transform.GetComponent<Slider>();
            }

            leftTk.value = left;
            centerTk.value = center;
            rightTk.value = right;
            LeftText.text = controller.fuelTanks[0].CurrentAmount.ToString("000");
            centerText.text = controller.fuelTanks[1].CurrentAmount.ToString("000");
            RightText.text = controller.fuelTanks[2].CurrentAmount.ToString("000");
            float remain = 0;
            foreach(SilantroFuelTank f in controller.fuelTanks)
            {
                remain += f.CurrentAmount;
            }
            RemainText.text = remain.ToString("0000");
            //RemainText.text = (controller.fuelTanks[0].CurrentAmount + controller.fuelTanks[1].CurrentAmount + controller.fuelTanks[2].CurrentAmount).ToString("0000");
            FuelText.text = RemainText.text;
        }
        float CalculateConsumption()
        {
            currentFuel = controller.fuelLevel;
            percentageCompleted = 1-(currentFuel / maxFuel);
            percentageCompleted = Mathf.Clamp(percentageCompleted, 0, 1);
            return minRotation+(percentageCompleted)*totalAngle;
        }
    }
}
