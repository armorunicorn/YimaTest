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
    public class DataSimluation
    {
        private Target targetAIS;
        private Target targetMerge;
        private Thread workThread;
        private StreamReader dataReader;

        private bool bothAdd = false;

        private int geoCoorMultiFactor;
        public DataSimluation(Target ais, Target merge, string file, int factor, bool b = false)
        {
            targetAIS = ais;
            targetMerge = merge;
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
            var d = (DataSimluation)cls;
            while (d.dataReader.EndOfStream != true)
            {
                string dataStr = d.dataReader.ReadLine();
                if (dataStr == "")
                    continue;
                try
                {
                    var tmp = dataStr.Split(',');
                    d.targetMerge.IMO = d.targetAIS.IMO = tmp[1];
                    d.targetMerge.MIMSI = d.targetAIS.MIMSI = tmp[2];
                    d.targetMerge.CallSign = d.targetAIS.CallSign = tmp[3];
                    d.targetMerge.Nationality = d.targetAIS.Nationality = tmp[4];
                    d.targetMerge.Speed = d.targetAIS.Speed = float.Parse(tmp[5]);
                    var p = new GeoPoint(Convert.ToInt32(float.Parse(tmp[6]) * d.geoCoorMultiFactor),
                        Convert.ToInt32(float.Parse(tmp[7]) * d.geoCoorMultiFactor));
                    var tp = new TrackPoint(p);
                    d.targetMerge.Track.Add(tp);
                    if(d.bothAdd)
                        d.targetAIS.Track.Add(tp);
                    d.targetMerge.Destination = d.targetAIS.Destination = tmp[8];
                    tp.Heading = d.targetMerge.Heading = d.targetAIS.Heading = 360 - int.Parse(tmp[9]);
                    d.targetMerge.Date = d.targetAIS.Date = tmp[11];
                    d.targetMerge.Time = d.targetAIS.Time = tmp[12];
                    tp.Time = d.targetMerge.Date + " " + d.targetMerge.Time;
                }
                catch(Exception e)
                {
                }
                Thread.Sleep(10);
            }
        }
    }
}
