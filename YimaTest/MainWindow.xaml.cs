using System;
using System.Collections.Generic;
using System.Drawing;
using System.Timers;
using System.Windows;
using YimaEncCtrl;
using YimaWF.data;

namespace YimaTest
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private Dictionary<int, Target> targetDic;

        private Timer updateTargetTimer = new Timer(1000);

        private MultDataSimluation dataSimluation;
        private MultDataSimluation dataSimluation2;
        private MultDataSimluation dataSimluation3;
        private MultDataSimluation dataSimluation4;
        private MultDataSimluation dataSimluation5;
        private MultDataSimluation dataSimluation6;
        private MultDataSimluation dataSimluation7;
        private MultDataSimluation dataSimluation8;
        private MultDataSimluation dataSimluation9;
        private List<MultDataSimluation> dataSimulationList = new List<MultDataSimluation>();

        public MainWindow()
        {
            InitializeComponent();
            TestCode();
            var a = yimaEncCtrl.YimaEnc.GetGeoCoorMultiFactor();
            Target t, t2, t3, t4;
            int aisID = 0, mergeID = 0, radarID = 0, fileID = 1;
            string filename = "target{0}.txt";
            TargetType[] typeList = new TargetType[] { TargetType.WorkBoat, TargetType.WorkBoat,
             TargetType.Unknow, TargetType.MerChantBoat, TargetType.MerChantBoat, TargetType.Yacht,
             TargetType.FishingBoat, TargetType.FishingBoat, TargetType.VietnamFishingBoat};
            for (int i =0; i < 9; i++)
            {
                t = new Target(aisID++, 0, 0);
                t.Source = TargetSource.AIS;
                t.Type = typeList[i];
                yimaEncCtrl.AISTargetDic.Add(t.ID, t);
                t2 = new Target(mergeID++, 0, 0);
                t2.Source = TargetSource.Merge;
                t2.Type = typeList[i];
                yimaEncCtrl.MergeTargetDic.Add(t2.ID, t2);
                t3 = new Target(radarID++, 0, 0);
                t3.Source = TargetSource.Radar;
                t3.Type = typeList[i];
                yimaEncCtrl.RadarTargetDic.Add(t3.ID, t3);
                t4 = new Target(radarID++, 0, 0);
                t4.Source = TargetSource.Radar;
                t4.Type = typeList[i];
                yimaEncCtrl.RadarTargetDic.Add(t4.ID, t4);
                dataSimulationList.Add(new MultDataSimluation(t, t2, t3, t4, string.Format(filename, fileID++), a, true));
            }
            /*t = new Target(aisID++, 0, 0);
            t.Source = TargetSource.AIS;
            t.Type = TargetType.WorkBoat;
            yimaEncCtrl.AISTargetDic.Add(t.ID, t);
            t2 = new Target(mergeID++, 0, 0);
            t2.Source = TargetSource.Merge;
            t2.Type = TargetType.WorkBoat;
            yimaEncCtrl.MergeTargetDic.Add(t2.ID, t2);
            t3 = new Target(radarID++, 0, 0);
            t3.Source = TargetSource.Radar;
            t3.Type = TargetType.WorkBoat;
            yimaEncCtrl.RadarTargetDic.Add(t3.ID, t3);
            t4 = new Target(radarID++, 0, 0);
            t4.Source = TargetSource.Radar;
            t4.Type = TargetType.WorkBoat;
            yimaEncCtrl.RadarTargetDic.Add(t4.ID, t4);
            dataSimulationList.Add(new MultDataSimluation(t, t2, t3, t4, "target1.txt", a, true));*/

            /*//4个渔船
            var t5 = new Target(3, 10, 13);
            t5.Source = TargetSource.AIS;
            t5.Type = TargetType.MerChantBoat;
            //t5.Track.Add(new GeoPoint(1077099500, 204688160));
            yimaEncCtrl.AISTargetDic.Add(t5.ID, t5);
            //dataSimluation3 = new DataSimluation(t5, t5, "target3.txt", a);
            t5 = new Target(4, 30, 10);
            t5.Source = TargetSource.AIS;
            t5.Type = TargetType.MerChantBoat;
            //t5.Track.Add(new GeoPoint(1070598330, 201956860));
            yimaEncCtrl.AISTargetDic.Add(t5.ID, t5);
            //dataSimluation4 = new DataSimluation(t5, t5, "target4.txt", a);
            t5 = new Target(5, 90, 10);
            t5.Source = TargetSource.AIS;
            t5.Type = TargetType.Yacht;
            //t5.Track.Add(new TrackPoint(new GeoPoint(1084959660, 195031930)));
            yimaEncCtrl.AISTargetDic.Add(t5.ID, t5);
            //dataSimluation5 = new DataSimluation(t5, t5, "target5.txt", a);
            t5 = new Target(6, 10, 10);
            t5.Source = TargetSource.AIS;
            t5.Type = TargetType.FishingBoat;
            //t5.Track.Add(new TrackPoint(new GeoPoint(1088672300, 198088120)));
            yimaEncCtrl.AISTargetDic.Add(t5.ID, t5);
            //dataSimluation6 = new DataSimluation(t5, t5, "target6.txt", a);
            //商船
            t5 = new Target(7, 50, 10);
            t5.Source = TargetSource.Merge;
            t5.Type = TargetType.MerChantBoat;
            t5.Track.Add(new TrackPoint(new GeoPoint(1067007760, 183531030)));
            yimaEncCtrl.MergeTargetDic.Add(t5.ID, t5);
            //可疑目标
            t5 = new Target(8, 180, 10);
            t5.Source = TargetSource.Radar;
            t5.Type = TargetType.Unknow;
            t5.Track.Add(new TrackPoint(new GeoPoint(1086115210, 182400920)));
            yimaEncCtrl.RadarTargetDic.Add(t5.ID, t5);
            t5 = new Target(9, 50, 10);
            t5.Source = TargetSource.Radar;
            t5.Type = TargetType.Unknow;
            t5.Track.Add(new TrackPoint(new GeoPoint(1086625510, 182381150)));
            yimaEncCtrl.RadarTargetDic.Add(t5.ID, t5);
            //
            //游艇
            t5 = new Target(10, 100, 10);
            t5.Source = TargetSource.Radar;
            t5.Type = TargetType.Yacht;
            t5.Track.Add(new TrackPoint(new GeoPoint(1078781670, 175257410)));
            yimaEncCtrl.RadarTargetDic.Add(t5.ID, t5);*/


            var z = new ProtectZone(new GeoPoint(1122886720, 182840461), 500, Color.Red);
            //yimaEncCtrl.ProtectZoneList.Add(z);
            z = new ProtectZone(new GeoPoint(1122886720, 182840461), 1000, Color.Blue);
            //yimaEncCtrl.ProtectZoneList.Add(z);
            z = new ProtectZone(new GeoPoint(1122886720, 182840461), 10000, Color.Orange);
            yimaEncCtrl.ProtectZoneList.Add(z);
            z = new ProtectZone(new GeoPoint(1122886720, 182840461), 20000, Color.YellowGreen);
            yimaEncCtrl.ProtectZoneList.Add(z);
            ForbiddenZone fz = new ForbiddenZone();
            fz.PointList.Add(new GeoPoint(1079536200, 189854000));
            fz.PointList.Add(new GeoPoint(1079068250, 189285000));
            fz.PointList.Add(new GeoPoint(1079068250, 188523000));
            fz.PointList.Add(new GeoPoint(1080074240, 188523000));
            fz.PointList.Add(new GeoPoint(1080074240, 189285000));
            yimaEncCtrl.ForbiddenZoneList.Add(fz);

            //管道
            PipeLine p = new PipeLine();
            p.PointList.Add(new GeoPoint(1072580759, 189722608));
            p.PointList.Add(new GeoPoint(1073537840, 189916000));
            p.PointList.Add(new GeoPoint(1074358100, 189261630));
            p.PointList.Add(new GeoPoint(1074519520, 189272200));
            yimaEncCtrl.PipLineList.Add(p);


            updateTargetTimer.AutoReset = true;
            updateTargetTimer.Elapsed += UpdateTargetTimer_Elapsed;
            //updateTargetTimer.Enabled = true;
            yimaEncCtrl.TargetSelect += YimaEncCtrl_TargetSelect;
            //yimaEncCtrl.ShowTargetTrack(t2);
        }

        private void TestCode()
        {
            var a = yimaEncCtrl.GetMapInfo(0);
        }

        private void YimaEncCtrl_TargetSelect(Target t)
        {
            var a = t;
        }

        private void UpdateTargetTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            foreach (var t in targetDic.Values)
            {
               // var newPoint = new GeoPoint(t.Track.Last());
                //newPoint.x += 100000;
                //t.Track.Add(newPoint);
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            /*var t1 = yimaEncCtrl.AISTargetDic[0];
            var t2 = yimaEncCtrl.AISTargetDic[1];
            var pt1 = new Target(t1.ID, t1.Heading, t1.Speed);
            var pt2 = new Target(t2.ID, t2.Heading, t2.Speed);
            pt1.Type = t1.Type;
            pt1.Source = t1.Source;
            pt1.CallSign = t1.CallSign;
            pt2.Type = t2.Type;
            pt2.Source = t2.Source;
            pt2.CallSign = t2.CallSign;
            yimaEncCtrl.AISTargetPlaybackDic.Add(pt1.ID, pt1);
            yimaEncCtrl.AISTargetPlaybackDic.Add(pt2.ID, pt2);
            yimaEncCtrl.StartPlayback();
            var p = new PlaybackSimluation(t1, t2, pt1, pt2);*/

            yimaEncCtrl.StartAddPipeLine();

        }

        private void MenuItem2_Click(object sender, RoutedEventArgs e)
        {
            yimaEncCtrl.EndAddPipeLine();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            yimaEncCtrl.CenterMap(yimaEncCtrl.platformGeoPo.x, yimaEncCtrl.platformGeoPo.y);
        }
    }
}
