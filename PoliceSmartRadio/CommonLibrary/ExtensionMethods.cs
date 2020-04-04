using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rage;
using Rage.Native;
using System.Drawing;
using System.Windows.Forms;
//using Albo1125.Common.CommonLibrary;
using System.Globalization;
using System.IO;

namespace Albo1125.Common.CommonLibrary
{
    public class TupleList<T1, T2> : List<Tuple<T1, T2>>
    {
        public TupleList(TupleList<T1, T2> tuplelist)
        {
            foreach (Tuple<T1, T2> tuple in tuplelist)
            {
                this.Add(tuple);
            }
        }
        public TupleList() { }
        public void Add(T1 item, T2 item2)
        {
            Add(new Tuple<T1, T2>(item, item2));
        }
    }
    public class TupleList<T1, T2, T3> : List<Tuple<T1, T2, T3>>
    {
        public TupleList() { }
        public TupleList(TupleList<T1, T2, T3> tuplelist)
        {
            foreach (Tuple<T1, T2, T3> tuple in tuplelist)
            {
                this.Add(tuple);
            }
        }
        public void Add(T1 item, T2 item2, T3 item3)
        {
            Add(new Tuple<T1, T2, T3>(item, item2, item3));
        }

    }
    public class TupleList<T1, T2, T3, T4> : List<Tuple<T1, T2, T3, T4>>
    {
        public TupleList() { }
        public TupleList(TupleList<T1, T2, T3, T4> tuplelist)
        {
            foreach (Tuple<T1, T2, T3, T4> tuple in tuplelist)
            {
                this.Add(tuple);
            }
        }
        public void Add(T1 item, T2 item2, T3 item3, T4 item4)
        {
            Add(new Tuple<T1, T2, T3, T4>(item, item2, item3, item4));
        }

    }

    public static class ExtensionMethods
    {


        //private static bool DisplayTime = false;

        public static int[] BlackListedNodeTypes = new int[] { 0, 8, 9, 10, 12, 40, 42, 136 };
        public static int GetNearestNodeType(this Vector3 pos)
        {
            bool get_property_success = false;
            uint node_prop_p1;
            int found_node_type;

            get_property_success = NativeFunction.Natives.GET_VEHICLE_NODE_PROPERTIES<bool>(pos.X, pos.Y, pos.Z, out node_prop_p1, out found_node_type);


            if (get_property_success)
            {
                return found_node_type;
            }
            else
            {
                return -1;
            }
        }

        public static bool IsNodeSafe(this Vector3 pos)
        {
            return !BlackListedNodeTypes.Contains(GetNearestNodeType(pos));
        }

        public static bool IsPointOnWater(this Vector3 position)
        {
            float height;

            return NativeFunction.Natives.GET_WATER_HEIGHT<bool>(position.X, position.Y, position.Z, out height);

        }

        public static void DisplayPopupTextBoxWithConfirmation(string Title, string Text, bool PauseGame)
        {
            new Popup(Title, Text, PauseGame, true).Display();
        }

        public static List<string> WrapText(this string text, double pixels, string fontFamily, float emSize, out double actualHeight)
        {
            string[] originalLines = text.Split(new string[] { " " },
                StringSplitOptions.None);

            List<string> wrappedLines = new List<string>();

            StringBuilder actualLine = new StringBuilder();
            double actualWidth = 0;
            actualHeight = 0;
            foreach (var item in originalLines)
            {
                System.Windows.Media.FormattedText formatted = new System.Windows.Media.FormattedText(item,
                    CultureInfo.CurrentCulture,
                    System.Windows.FlowDirection.LeftToRight,
                    new System.Windows.Media.Typeface(fontFamily), emSize, System.Windows.Media.Brushes.Black);


                actualWidth += formatted.Width;
                actualHeight = formatted.Height;

                if (actualWidth > pixels)
                {
                    wrappedLines.Add(actualLine.ToString());
                    actualLine.Clear();
                    actualWidth = 0;
                    actualLine.Append(item + " ");
                    actualWidth += formatted.Width;
                }
                else if (item == Environment.NewLine || item == "\n")
                {
                    wrappedLines.Add(actualLine.ToString());
                    actualLine.Clear();
                    actualWidth = 0;
                }
                else
                {
                    actualLine.Append(item + " ");
                }
            }
            if (actualLine.Length > 0)
                wrappedLines.Add(actualLine.ToString());

            return wrappedLines;
        }

        public static bool IsPolicePed(this Ped ped)
        {
            return ped.RelationshipGroup == "COP";
        }

        public static string GetKeyString(Keys MainKey, Keys ModifierKey)
        {
            if (ModifierKey == Keys.None)
            {
                return MainKey.ToString();
            }
            else
            {
                string strmodKey = ModifierKey.ToString();

                if (strmodKey.EndsWith("ControlKey") | strmodKey.EndsWith("ShiftKey"))
                {
                    strmodKey.Replace("Key", "");
                }

                if (strmodKey.Contains("ControlKey"))
                {
                    strmodKey = "CTRL";
                }
                else if (strmodKey.Contains("ShiftKey"))
                {
                    strmodKey = "Shift";
                }
                else if (strmodKey.Contains("Menu"))
                {
                    strmodKey = "ALT";
                }

                return string.Format("{0} + {1}", strmodKey, MainKey.ToString());
            }
        }

        public static float CalculateHeadingTowardsEntity(this Entity ent, Entity TargetEntity)
        {
            Vector3 directionToTargetEnt = (TargetEntity.Position - ent.Position);
            directionToTargetEnt.Normalize();
            return MathHelper.ConvertDirectionToHeading(directionToTargetEnt);

        }

        public static float CalculateHeadingTowardsPosition(this Vector3 start, Vector3 Target)
        {
            Vector3 directionToTargetEnt = (Target - start);
            directionToTargetEnt.Normalize();
            return MathHelper.ConvertDirectionToHeading(directionToTargetEnt);

        }
        public static bool IsKeyDownComputerCheck(Keys KeyPressed)
        {


            if (Rage.Native.NativeFunction.Natives.UPDATE_ONSCREEN_KEYBOARD<int>() != 0)
            {

                return Game.IsKeyDown(KeyPressed);
            }
            else
            {
                return false;
            }



        }
        public static bool IsKeyDownRightNowComputerCheck(Keys KeyPressed)
        {


            if (Rage.Native.NativeFunction.Natives.UPDATE_ONSCREEN_KEYBOARD<int>() != 0)
            {
                return Game.IsKeyDownRightNow(KeyPressed);
            }
            else
            {
                return false;
            }



        }

        public static bool IsKeyCombinationDownComputerCheck(Keys MainKey, Keys ModifierKey)
        {
            if (MainKey != Keys.None)
            {
                return ExtensionMethods.IsKeyDownComputerCheck(MainKey) && (ExtensionMethods.IsKeyDownRightNowComputerCheck(ModifierKey)
                || (ModifierKey == Keys.None && !ExtensionMethods.IsKeyDownRightNowComputerCheck(Keys.Shift) && !ExtensionMethods.IsKeyDownRightNowComputerCheck(Keys.Control)
                && !ExtensionMethods.IsKeyDownRightNowComputerCheck(Keys.LControlKey) && !ExtensionMethods.IsKeyDownRightNowComputerCheck(Keys.LShiftKey)));
            }
            else
            {
                return false;
            }
        }
        public static bool IsKeyCombinationDownRightNowComputerCheck(Keys MainKey, Keys ModifierKey)
        {
            if (MainKey != Keys.None)
            {
                return ExtensionMethods.IsKeyDownRightNowComputerCheck(MainKey) && ((ExtensionMethods.IsKeyDownRightNowComputerCheck(ModifierKey)
                    || (ModifierKey == Keys.None && !ExtensionMethods.IsKeyDownRightNowComputerCheck(Keys.Shift) && !ExtensionMethods.IsKeyDownRightNowComputerCheck(Keys.Control)
                    && !ExtensionMethods.IsKeyDownRightNowComputerCheck(Keys.LControlKey) && !ExtensionMethods.IsKeyDownRightNowComputerCheck(Keys.LShiftKey))));
            }
            else
            {
                return false;
            }

        }

        public static string Reverse(this string s)
        {
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }

        public static T PickRandom<T>(this IEnumerable<T> source)
        {
            return source.PickRandom(1).Single();
        }
        public static IEnumerable<T> PickRandom<T>(this IEnumerable<T> source, int count)
        {
            return new List<T>(source).Shuffle().Take(count);
        }
        public static List<T> Shuffle<T>(this List<T> List)
        {
            List<T> list = new List<T>(List);
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = CommonVariables.rnd.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
            return list;

        }

        public static void MakeMissionPed(this Ped ped)
        {
            ped.BlockPermanentEvents = true;
            ped.IsPersistent = true;

        }
        public static Ped ClonePed(this Ped oldPed)
        {
            Vector3 oldPedPosition = oldPed.Position;
            float oldPedHeading = oldPed.Heading;
            bool spawnInVehicle = false;
            Vehicle car = null;
            int seatindex = 0;
            int oldarmor = oldPed.Armor;
            int oldhealth = oldPed.Health;
            if (oldPed.IsInAnyVehicle(false))
            {
                car = oldPed.CurrentVehicle;
                seatindex = oldPed.SeatIndex;
                spawnInVehicle = true;
            }
            Ped newPed = NativeFunction.Natives.ClonePed<Ped>(oldPed, oldPed.Heading, false, true);
            if (oldPed.Exists() && oldPed.IsValid())
            {
                oldPed.Delete();
            }
            newPed.Position = oldPedPosition;
            newPed.Heading = oldPedHeading;

            if (spawnInVehicle)
            {
                newPed.WarpIntoVehicle(car, seatindex);
            }
            newPed.Health = oldhealth;
            newPed.Armor = oldarmor;
            newPed.BlockPermanentEvents = true;
            newPed.IsPersistent = true;
            return newPed;
        }

    }  
}

