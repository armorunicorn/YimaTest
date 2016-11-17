using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using YimaWF.data;

namespace YimaTest
{
    public class PlaybackSimluation
    {
        public Target OriginalTarget1;
        public Target OriginalTarget2;

        public Target PlaybackTarget1;
        public Target PlaybackTarget2;

        private Thread workThread;

        private int dataCount1 = 0;
        private int dataCount2 = 0;

        public PlaybackSimluation(Target t1, Target t2, Target pt1, Target pt2)
        {
            OriginalTarget1 = t1;
            OriginalTarget2 = t2;
            PlaybackTarget1 = pt1;
            PlaybackTarget2 = pt2;
            workThread = new Thread(new ParameterizedThreadStart(UpdateData));
            workThread.IsBackground = true;
            workThread.Start(this);
        }


        public static void UpdateData(Object cls)
        {
            var p = (PlaybackSimluation)cls;
            while(p.dataCount1 < p.OriginalTarget1.Track.Count && p.dataCount2 < p.OriginalTarget2.Track.Count)
            {
                if(p.dataCount1 < p.OriginalTarget1.Track.Count)
                {
                    p.PlaybackTarget1.Track.Add(p.OriginalTarget1.Track[p.dataCount1]);
                    p.dataCount1++;
                }

                if (p.dataCount2 < p.OriginalTarget2.Track.Count)
                {
                    p.PlaybackTarget2.Track.Add(p.OriginalTarget2.Track[p.dataCount2]);
                    p.dataCount2++;
                }
                Thread.Sleep(1000);
            }
        }
    }
}
