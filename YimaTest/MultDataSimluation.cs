using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using YimaEncCtrl;
using YimaWF.data;

namespace YimaTest
{
    public class MultDataSimluation
    {
        private Target targetAIS;
        private Target targetMerge;
        private Target targetRadar1;
        private Target targetRadar2;
        private Thread workThread;
        private StreamReader dataReader;

        private bool bothAdd = false;

        private int geoCoorMultiFactor;
        public MultDataSimluation(Target ais, Target merge, Target radar1, Target radar2, string file, int factor, bool b = false)
        {
            targetAIS = ais;
            targetMerge = merge;
            targetRadar1 = radar1;
            targetRadar2 = radar2;
            bothAdd = b;
            geoCoorMultiFactor = factor;
            workThread = new Thread(new ParameterizedThreadStart(UpdateData));
            workThread.IsBackground = true;
            dataReader = File.OpenText(file);
            if (dataReader == null)
                return;
            workThread.Start(this);
        }

        public static void UpdateData(Object cls)
        {
            var d = (MultDataSimluation)cls;
            while (d.dataReader.EndOfStream != true)
            {
                string dataStr = d.dataReader.ReadLine();
                if (dataStr == "")
                    continue;
                try
                {
                    GeoPoint p;
                    TrackPoint tp;

                    var tmp = dataStr.Split(',');
                    d.targetRadar1.IMO = d.targetRadar2.IMO = d.targetMerge.IMO = d.targetAIS.IMO = tmp[1];
                    d.targetRadar1.MIMSI = d.targetRadar2.MIMSI = d.targetMerge.MIMSI = d.targetAIS.MIMSI = tmp[2];
                    d.targetRadar1.CallSign = d.targetRadar2.CallSign = d.targetMerge.CallSign = d.targetAIS.CallSign = tmp[3];
                    d.targetRadar1.Nationality = d.targetRadar2.Nationality = d.targetMerge.Nationality = d.targetAIS.Nationality = tmp[4];
                    d.targetRadar1.Speed = d.targetRadar2.Speed = d.targetMerge.Speed = d.targetAIS.Speed = float.Parse(tmp[5]);
                    d.targetRadar1.Heading = d.targetRadar2.Heading = d.targetMerge.Heading = d.targetAIS.Heading = 360 - int.Parse(tmp[9]);
                    d.targetRadar1.Date = d.targetRadar2.Date = d.targetMerge.Date = d.targetAIS.Date = tmp[11];
                    d.targetRadar1.Time = d.targetRadar2.Time = d.targetMerge.Time = d.targetAIS.Time = tmp[12];
                    if (tmp[6] == "")
                    {
                        if (d.targetAIS.Track.Count > 0)
                        {
                            d.targetAIS.Track.Clear();
                            d.targetMerge.Track.Clear();
                        }
                    }
                    else
                    {
                        p = new GeoPoint(Convert.ToInt32(float.Parse(tmp[6]) * d.geoCoorMultiFactor),
                        Convert.ToInt32(float.Parse(tmp[7]) * d.geoCoorMultiFactor));
                        tp = new TrackPoint(p);
                        d.targetMerge.Track.Add(tp);
                        d.targetAIS.Track.Add(tp);
                        tp.Heading = d.targetAIS.Heading;
                        tp.Time = d.targetMerge.Date + " " + d.targetMerge.Time;
                    }

                    d.targetRadar1.Destination = d.targetRadar2.Destination = d.targetMerge.Destination = d.targetAIS.Destination = tmp[8];



                    if (tmp[18] == "")
                    {
                        if (d.targetRadar1.Track.Count != 0)
                        {
                            d.targetRadar1.Track.Clear();
                            d.targetRadar2.Track.Clear();
                        }
                    }
                    else
                    {
                        p = new GeoPoint(Convert.ToInt32(float.Parse(tmp[18]) * d.geoCoorMultiFactor),
                            Convert.ToInt32(float.Parse(tmp[19]) * d.geoCoorMultiFactor));
                        tp = new TrackPoint(p);
                        d.targetRadar1.Track.Add(tp);
                        p = new GeoPoint(Convert.ToInt32(float.Parse(tmp[22]) * d.geoCoorMultiFactor),
                            Convert.ToInt32(float.Parse(tmp[23]) * d.geoCoorMultiFactor));
                        tp = new TrackPoint(p);
                        d.targetRadar2.Track.Add(tp);
                    }
                    d.targetAIS.ArriveTime = d.targetRadar1.ArriveTime = d.targetRadar2.ArriveTime = d.targetMerge.ArriveTime = tmp[25];
                    d.targetAIS.Name = d.targetRadar1.Name = d.targetRadar2.Name = d.targetMerge.Name = tmp[15];
                    if (tmp[26] == "1")
                        d.targetRadar1.IsApproach = d.targetRadar2.IsApproach = true;
                    else
                        d.targetRadar1.IsApproach = d.targetRadar2.IsApproach = false;
                    int warnNum = Convert.ToInt32(tmp[24]);
                    App app = (App)App.Current;
                    //if (app.CurMainWin != null)
                    {
                        //if(warnNum != 0)
                        //    app.CurMainWin.StartWarn(warnNum);
                    }
                }
                catch (Exception e)
                {
                }
                Thread.Sleep(1000);
            }
        }
    }
}
