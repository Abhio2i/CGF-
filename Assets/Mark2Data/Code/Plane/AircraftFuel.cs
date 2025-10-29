#region Script info
//input: current fuel data max fuel quantity and fuel burn rate.
//output : The fuel is burning as given inputs.
#endregion
using UnityEngine;
using Data.Plane;
namespace AirPlane.Fuel
{
    public class AircraftFuel : MonoBehaviour
    {

        #region Fields
        [Header("Plane Data")]
        public PlaneData fueldata;

        [Header("Fuel Properties")]
        public float FuelCapacity = 10000f;//total capacity
        public float FuelBurnRate_inMin = 130f;//per min

        //[Header("Events")]
        //public UnityEvent OnFuelFull = new UnityEvent();
        private float _takeoffFuel = 7000f;
        private float _addedFuel = 0;
        private float _currentFuel;
        private float _normalizedFuel;

        #endregion

        #region Properties

        public float CurrentFuel
        {
            get { return _currentFuel; }
            set { _currentFuel = value; }
        }
        #endregion

        #region Custom Methods
        private void Awake()
        {
            CurrentFuel = 7000;
        }
        public float NormalizedFuel
        {
            get { return _normalizedFuel; }
        }

        public void InitFuel()
        {
            _currentFuel = FuelCapacity;
        }

        public void AddFuel(float amount)
        {
            if (CurrentFuel == FuelCapacity)
            {
                //OnFuelFull.Invoke();
                Debug.LogWarning("Fuel is full !. Can't add more.");
            }
            else
            {
                CurrentFuel += amount;
            }

            CurrentFuel = Mathf.Clamp(CurrentFuel, 0f, FuelCapacity);
        }

        public void ResetFuel()
        {
            CurrentFuel = FuelCapacity;
        }

        public void UpdateFuel(float percentage)
        {
            if (percentage > 1) 
            {
                FuelBurnRate_inMin = 1300f * percentage;
            }
            else
            {
                FuelBurnRate_inMin = 1300f;
            }
            percentage = Mathf.Clamp(percentage, 0.05f, 1);
            float currentBurn = ((FuelBurnRate_inMin * percentage) / 3600) * Time.deltaTime; // per min
            _currentFuel -= currentBurn;

            _currentFuel = Mathf.Clamp(_currentFuel, 0f, FuelCapacity);

            _normalizedFuel = CurrentFuel / (_takeoffFuel+_addedFuel);
            fueldata.planeFuel = _currentFuel;
        }

        #endregion
    }
}