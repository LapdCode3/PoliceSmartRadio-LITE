using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rage;
using System.Reflection;

namespace Traffic_Policer.API
{
    public static class Functions
    {
        /// <summary>
        /// Check whether the vehicle is insured as per the insurance system.
        /// </summary>
        /// <param name="veh"></param>
        /// <returns></returns>
        
        public static bool IsVehicleInsured(Vehicle veh)
        {
            if (veh.Exists())
            {
                
                Traffic_Policer.EVehicleDetailsStatus insurancestatus = VehicleDetails.GetInsuranceStatusForVehicle(veh);
                return insurancestatus == EVehicleDetailsStatus.Valid;
                
            }
            else
            {
                return true;
            }
        }

        private static Random rnd = new Random();
        /// <summary>
        /// Sets the insurance status for a vehicle. Used when vehicle is checked.
        /// </summary>
        /// <param name="vehicle"></param>
        /// <param name="Insured">If false, sets insurance status to expired/none at random.</param>
        public static void SetVehicleInsuranceStatus(Vehicle vehicle, bool Insured)
        {
            if (Insured)
            {
                VehicleDetails.SetInsuranceStatusForVehicle(vehicle, EVehicleDetailsStatus.Valid);
            }
            else
            {
                if (rnd.Next(5) < 2)
                {
                    VehicleDetails.SetInsuranceStatusForVehicle(vehicle, EVehicleDetailsStatus.None);
                }
                else
                {
                    VehicleDetails.SetInsuranceStatusForVehicle(vehicle, EVehicleDetailsStatus.Expired);
                }
            }
        }

        /// <summary>
        /// Sets the insurance status for a vehicle. Used when vehicle is checked.
        /// </summary>
        /// <param name="vehicle"></param>
        /// <param name="InsuranceStatus"></param>
        public static void SetVehicleInsuranceStatus(Vehicle vehicle, EVehicleDetailsStatus InsuranceStatus)
        {
            VehicleDetails.SetInsuranceStatusForVehicle(vehicle, InsuranceStatus);
        }

        /// <summary>
        /// Sets the registration status for a vehicle. Used when vehicle is checked.
        /// </summary>
        /// <param name="vehicle"></param>
        /// <param name="registrationValid">If false, sets status to expired/none at random </param>
        public static void SetVehicleRegistrationStatus(Vehicle vehicle, bool registrationValid)
        {
            if (registrationValid)
            {
                VehicleDetails.SetRegistrationStatusForVehicle(vehicle, EVehicleDetailsStatus.Valid);
            }
            else
            {
                if (rnd.Next(5) < 2)
                {
                    VehicleDetails.SetRegistrationStatusForVehicle(vehicle, EVehicleDetailsStatus.None);
                }
                else
                {
                    VehicleDetails.SetRegistrationStatusForVehicle(vehicle, EVehicleDetailsStatus.Expired);
                }
            }
        }

        /// <summary>
        /// Sets the registration status for a vehicle. Used when vehicle is checked.
        /// </summary>
        /// <param name="vehicle"></param>
        /// <param name="RegistrationStatus"></param>
        public static void SetVehicleRegistrationStatus(Vehicle vehicle, EVehicleDetailsStatus RegistrationStatus)
        {
            VehicleDetails.SetRegistrationStatusForVehicle(vehicle, RegistrationStatus);
        }

        /// <summary>
        /// Gets the registration status for a vehicle. Used when vehicle is checked.
        /// </summary>
        /// <param name="veh"></param>
        /// <returns></returns>
        public static EVehicleDetailsStatus GetVehicleRegistrationStatus(Vehicle veh)
        {
            return VehicleDetails.GetRegistrationStatusForVehicle(veh);
        }

        /// <summary>
        /// Gets the insurance status for a vehicle. Used when vehicle is checked.
        /// </summary>
        /// <param name="veh"></param>
        /// <returns></returns>
        public static EVehicleDetailsStatus GetVehicleInsuranceStatus(Vehicle veh)
        {
            return VehicleDetails.GetInsuranceStatusForVehicle(veh);
        }


        /// <summary>
        /// Use this only if you don't want the vehicle details to appear after typing in a licence plate in a custom window. Remember to reactivate this after you're done fetching the input.
        /// </summary>
        /// <param name="enabled"></param>
        public static void SetAutomaticVehicleDeatilsChecksEnabled(bool enabled)
        {
            Game.LogTrivial("Traffic Policer API: Assembly " + Assembly.GetCallingAssembly().GetName().Name + " setting automatic vehicle details checks to: " + enabled.ToString());
            VehicleDetails.AutomaticDetailsChecksEnabled = enabled;
        }

    }
}
