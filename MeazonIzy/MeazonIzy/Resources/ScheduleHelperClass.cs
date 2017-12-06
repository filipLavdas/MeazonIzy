using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MeazonIzy.Localization;
using MeazonIzy.Models;

namespace MeazonIzy.Resources
{
   public class ScheduleHelperClass
    {
        private List<int> firstPosition;
        private List<int> lastPosition;
        private List<string> daysList;
        
        private ObservableCollection<MySchedule> MyScheduleList;


        public ObservableCollection<MySchedule> DisirializeSchedule(Schedule schedule)
        {

            var disirializedquarters = new bool[96 * 7];
            bool[,] Days = new bool[7, 96];
            firstPosition = new List<int>();
            lastPosition = new List<int>();
            daysList = new List<string>();
            MyScheduleList = new ObservableCollection<MySchedule>();



            try
            {


                disirializedquarters = StringToQuartersDesirialize(schedule.Events);
                //Set 2Dimension array of(7,96) each day of week
                for (int i = 0; i < disirializedquarters.Length; i += 96)
                {
                    for (int j = i; j < 96 + i; j++)
                    {
                        Days[i / 96, j % 96] = disirializedquarters[j];

                    }

                }


                for (var i = 0; i < 7; i++)
                {


                    for (int j = 0; j < 96; j++)
                    {
                        if (j - 1 == -1)
                        {
                            if (Days[i, j])
                            {
                                firstPosition.Add(0);
                                daysList.Add(IndexToDay(i));

                            }
                        }
                        else
                        {
                            if (Days[i, j] && !Days[i, j - 1])
                            {
                                firstPosition.Add(j);
                                daysList.Add(IndexToDay(i));


                            }
                        }
                        if (j + 1 == 96)
                        {
                            if (Days[i, 95])
                            {
                                lastPosition.Add(0);




                            }
                        }
                        else
                        {
                            if (Days[i, j] && !Days[i, j + 1])
                            {
                                lastPosition.Add(j + 1);

                            }
                        }




                    }



                }




              

                for (int i = 0; i < firstPosition.Count; i++)
                {
                    SetMySchedules(0, i);

                }


            }
            catch (Exception e)
            {
                var msg = "+++++++++++++++++++++++++++++++++" + e.Message;
                Debug.WriteLine(msg);
            }

            return MyScheduleList;


        }


        public string SerializeSchedule(ObservableCollection<MySchedule> collection)
        {
            bool[] quarters = new bool[96 * 7];
            for (int i = 0; i < quarters.Length; i++)
            {
                quarters[i] = false;

            }
            if (collection.Count != 0)
            {

                foreach (var col in collection)
                {
                    var daysChecked = new bool[7];
                    var days = col.Days.Split(' ').ToList();
                    var fromQt = (int)StringToQt(col.FromTime);
                    var toQt = (int)StringToQt(col.ToTime);

                    foreach (var day in days)
                    {
                        var index = DayToIndex(day);
                        daysChecked[index] = true;


                    }

                    if (toQt <= 96)
                    {
                        for (var d = 0; d < daysChecked.Length; d++)
                        {
                            if (daysChecked[d])
                            {

                                for (var k = fromQt; k < toQt; k++)
                                {
                                    quarters[k + (d * 96)] = true;

                                }


                            }
                        }
                    }


                }
            }

            var bits = new BitArray(quarters);

            var bytes = new List<byte>();

            var currentbyte = 0x00;
            for (int i = 0; i < bits.Length;)
            {
                currentbyte = 0x00;
                for (int j = 0; j < 8 && i < bits.Length; j++, i++)
                {
                    if (bits[i])
                    {
                        currentbyte |= (0x80 >> (i % 8));
                    }


                }
                bytes.Add((byte)currentbyte);
            }

            var b64String = Convert.ToBase64String(bytes.ToArray());
            return b64String;
        }

        public bool[] StringToQuartersDesirialize(string b64)
        {
            List<bool> disQt = new List<bool>();
            var bytes = Convert.FromBase64String(b64);
            //convert bytes to bools
            foreach (var b in bytes)
            {
                for (int i = 0; i < 8; i++)
                {
                    disQt.Add((b & (0x80 >> i)) > 0);
                }

            }

            return disQt.ToArray();
            //boolsList to arrayOfBools
        }

        private void SetMySchedules(int i, int j)
        {
            var myschedule = new MySchedule();
            if (MyScheduleList.Count == i)
            {

                myschedule.FromTime = QtToTime(firstPosition[j]);
                myschedule.ToTime = QtToTime(lastPosition[j]);
                myschedule.Days = daysList[j];
                
                MyScheduleList.Add(myschedule);


            }
            else if (StringToQt(MyScheduleList[i].FromTime) == firstPosition[j] &&
                     StringToQt(MyScheduleList[i].ToTime) == lastPosition[j])
            {

                MyScheduleList[i].Days += $" {daysList[j]}";



            }
            else
            {
                i++;
                SetMySchedules(i, j);

            }


        }

        public double StringToQt(string seltime)
        {
            List<string> tlList = seltime.Split(':').ToList();


            var timeHrIntQt = Double.Parse(tlList[0]) * 4;
            var timeMinIntQt = Double.Parse(tlList[1]) / 15;

            return timeHrIntQt + timeMinIntQt;
        }

        private int DayToIndex(string day)
        {
            int index;
          
            if (string.Equals (day, L10n.Localize("SatLabel")) )
            {
                index= 0;
            }else if (string.Equals(day, L10n.Localize("SunLabel")))
            {
                index= 1;
            }else if (string.Equals(day, L10n.Localize("MonLabel")))
            {
                index= 2;
            }  else if (string.Equals(day, L10n.Localize("TueLabel")))
            {
                index= 3;
            } else if (string.Equals(day, L10n.Localize("WedLabel")))
            {
                index= 4;
            }else if (string.Equals(day, L10n.Localize("ThuLabel")))
            {
                index= 5;
            }else
            {
                index= 6;
            }


            return index;



        }

        public string IndexToDay(int i)
        {
            switch (i)
            {
                case 0:
                    return L10n.Localize("SatLabel");

                case 1:
                    return L10n.Localize("SunLabel");

                case 2:
                    return L10n.Localize("MonLabel");

                case 3:
                    return L10n.Localize("TueLabel");

                case 4:
                    return L10n.Localize("WedLabel");

                case 5:
                    return L10n.Localize("ThuLabel");

                default:
                    return L10n.Localize("FriLabel");


            }

        }
        public string QtToTime(double num)
        {
            var qtToMinutes = (num % 4) * 15;
            var qtToHour = Math.Floor(num / 4);
            string msg = string.Format(" {0}:{1}", qtToHour, qtToMinutes);
            return msg;
        }


    }
}
