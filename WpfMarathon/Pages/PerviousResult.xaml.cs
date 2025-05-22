using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;
using WpfMarathon.Base;
using Microsoft.Extensions.Logging;

namespace WpfMarathon.Pages
{
    /// <summary>
    /// Логика взаимодействия для PerviousResult.xaml
    /// </summary>
    public partial class PerviousResult : Page
    {
        public static MarafonEntities db = new MarafonEntities();
        List<Gonka> gonka = new List<Gonka>();
        int _age;
        int cnt;
        public PerviousResult(MainWindow mainWindow)
        {
            InitializeComponent();
            var gender = db.Gender.Select(x=> x.Gender1).ToList(); 
            List<string> age = new List<string> { "18-27", "27-36", "36-49" };
            cmbMarathon.ItemsSource = db.Marathon.Select(x => x.MarathonName).ToList();
            cmbDistance.ItemsSource = db.EventType.Select(x => x.EventTypeName).ToList();
            cmbGender.ItemsSource = gender;
            cmbAge.ItemsSource = age;
            gridResult.ItemsSource = db.Event.ToList();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            if (cmbGender.SelectedIndex != -1 && cmbAge.SelectedIndex != -1)
            {
                var r = db.Runner.Where(x => x.Gender1 == cmbGender.SelectedValue).ToList();
                List<Registration> rn = new List<Registration>();
                var revent = db.RegistrationEvent.ToList();
                foreach (var ru in r)
                {
                    rn.Add(db.Registration.FirstOrDefault(x => x.RunnerId == ru.RunnerId));
                }
                foreach (var re in rn)
                {
                    revent.Add(db.RegistrationEvent.FirstOrDefault(x => x.RegistrationId == re.RegistrationId));
                }
                foreach (var er in revent)
                {
                    var n = db.Registration.FirstOrDefault(x => x.RegistrationId == er.RegistrationId);
                    var na = db.Runner.FirstOrDefault(x => x.RunnerId == n.RunnerId);
                    var nam = db.User.FirstOrDefault(x => x.Email == na.Email);
                    TimeSpan TS = new TimeSpan(0, 0, (int)er.RaceTime);
                    Gonka go = new Gonka
                    {
                        Mesto = (short)er.BibNumber,
                        Time = TS,
                        Name = nam.FirstName,
                        Country = na.CountryCode
                    };
                    gonka.Add(go);
                }
                gridResult.ItemsSource = gonka.ToList();

            }
            if (cmbGender.SelectedIndex != -1 && cmbAge.SelectedIndex == -1)
            {
                var r =  db.Runner.Where(x => x.Gender1 == cmbGender.SelectedValue).ToList();
                if (cmbAge.SelectedItem == "18-27")
                {
                    r.Where(x => x.DateOfBirth.Value.Year >= 18).Where(x => x.DateOfBirth.Value.Year <= 27).ToList();
                }
                if (cmbAge.SelectedItem == "27-36")
                {
                    r.Where(x => x.DateOfBirth.Value.Year >= 27).Where(x => x.DateOfBirth.Value.Year <= 36).ToList();
                }
                if (cmbAge.SelectedItem == "36-49")
                {
                    r.Where(x => x.DateOfBirth.Value.Year >= 36).Where(x => x.DateOfBirth.Value.Year <= 49).ToList();
                }
                List<Registration> rn = new List<Registration>();
                var revent = db.RegistrationEvent.ToList();
                foreach (var ru in r)
                {
                    rn.Add(db.Registration.FirstOrDefault(x => x.RunnerId == ru.RunnerId));
                }
                foreach (var re in rn)
                {
                    revent.Add(db.RegistrationEvent.FirstOrDefault(x=> x.RegistrationId == re.RegistrationId));
                }
                foreach (var er in revent)
                {
                    var n = db.Registration.FirstOrDefault(x=> x.RegistrationId == er.RegistrationId);
                    var na = db.Runner.FirstOrDefault(x=> x.RunnerId == n.RunnerId);
                    var nam = db.User.FirstOrDefault(x=> x.Email == na.Email);
                    TimeSpan TS = new TimeSpan(0, 0, (int)er.RaceTime);
                    Gonka go = new Gonka {
                        Mesto = (short)er.BibNumber,
                        Time = TS,
                        Name = nam.FirstName,
                        Country = na.CountryCode
                    };
                    gonka.Add(go);
                }
                gridResult.ItemsSource = gonka.ToList();

            }
            if (cmbGender.SelectedIndex == -1 && cmbAge.SelectedIndex != -1)
            {
                var revent = db.RegistrationEvent.ToList();
                var r = from rdate in db.Runner.ToList()
                        select new { rdate.RunnerId, rdate.DateOfBirth};
                if (cmbAge.SelectedItem == "18-27")
                {
                    r.Where(x=> x.DateOfBirth.Value.Year>=18).Where(x => x.DateOfBirth.Value.Year <= 27).ToList();
                }
                if (cmbAge.SelectedItem == "27-36")
                {
                    r.Where(x => x.DateOfBirth.Value.Year >= 27).Where(x => x.DateOfBirth.Value.Year <= 36).ToList();
                }
                if (cmbAge.SelectedItem == "36-49")
                {
                    r.Where(x => x.DateOfBirth.Value.Year >= 36).Where(x => x.DateOfBirth.Value.Year <= 49).ToList();
                }
                List<Registration> rn = new List<Registration>();

                foreach (var ru in r)
                {
                    rn.Add(db.Registration.FirstOrDefault(x => x.RunnerId == ru.RunnerId));
                }
                
                foreach (var re in rn)
                {
                    revent.Add(db.RegistrationEvent.FirstOrDefault(x => x.RegistrationId == re.RegistrationId));
                }
                foreach (var er in revent)
                {
                    var n = db.Registration.FirstOrDefault(x => x.RegistrationId == er.RegistrationId);
                    var na = db.Runner.FirstOrDefault(x => x.RunnerId == n.RunnerId);
                    var nam = db.User.FirstOrDefault(x => x.Email == na.Email);

                    TimeSpan TS = new TimeSpan(0, 0, (int)er.RaceTime);
                    Gonka go = new Gonka
                    {
                        Mesto = (short)er.BibNumber,
                        Time = TS,
                        Name = nam.FirstName,
                        Country = na.CountryCode
                    };
                    gonka.Add(go);
                }
                gridResult.ItemsSource = gonka.ToList();
            }
            if (cmbDistance.SelectedIndex != -1 && cmbMarathon.SelectedIndex != -1)
            {
                var marathon = db.Marathon.FirstOrDefault(x => x.MarathonName == cmbMarathon.SelectedValue).MarathonId;
                var distance = db.EventType.FirstOrDefault(x => x.EventTypeName == cmbDistance.SelectedValue).EventTypeId;
                var even = db.Event.Where(x => x.EventTypeId == distance).Where(x => x.MarathonId == marathon).ToList();
                var ev = db.RegistrationEvent.ToList();
                string a1 = "";
                if (distance != null || marathon != null)
                {
                    if (db.Event.FirstOrDefault(x => x.EventTypeId == distance && x.MarathonId == marathon) == null)
                    {

                        MessageBox.Show("Null", "Ошибка");
                        return;
                    }
                    else
                    {
                        a1 = db.Event.FirstOrDefault(x => x.EventTypeId == distance && x.MarathonId == marathon).EventId;
                    }
                
                foreach (var rev in ev)
                {
                    if (rev.EventId == a1)
                    {
                        cnt += 1;
                    }
                }
                txtRunnerAllCount.Text = cnt.ToString();
                txtRunnerFinishCount.Text = cnt.ToString();

                //var evId = db.Event.FirstOrDefault(x => x.EventTypeId == distance && x.MarathonId == marathon);
                //var Ev = db.RegistrationEvent.ToList();
                //Ev = db.RegistrationEvent.Where(x => x.EventId == evId.EventId).ToList();
                //txtRunnerFinishCount.Text = Ev.Count.ToString();
                
                var rv = db.RegistrationEvent.FirstOrDefault(x => x.EventId == a1).RaceTime;
                if (rv != null)
                {
                    TimeSpan TS = new TimeSpan(0, 0, (int)rv);
                    txtTime.Text = TS.ToString();
                }
                else 
                {
                    TimeSpan TS = new TimeSpan(0, 0, 0);
                    txtTime.Text = TS.ToString();
                }
                }
                else
                {
                    MessageBox.Show("Нет данных", "Ошибка");
                    return;
                }
            }
            //if (cmbDistance.SelectedIndex != -1 && cmbMarathon.SelectedIndex == -1)
            //{
            //    var marathon = db.Marathon.FirstOrDefault(x => x.MarathonName == cmbMarathon.SelectedValue).MarathonId;
            //    var distance = db.EventType.FirstOrDefault(x => x.EventTypeName == cmbDistance.SelectedValue).EventTypeId;
            //    var even = db.Event.Where(x => x.EventTypeId == distance).Where(x => x.MarathonId == marathon);
            //    var ev = db.RegistrationEvent.ToList();
                
            //    foreach (var rev in ev)
            //    {
            //        if (rev.EventId == even.Select(x => x.EventId).ToString())
            //        {
            //            cnt+=1;
            //        }
            //    }
            //    txtRunnerAllCount.Text = cnt.ToString();
            //    txtRunnerFinishCount.Text = cnt.ToString();

            //    var evId = db.Event.FirstOrDefault(x => x.EventTypeId == distance && x.MarathonId == marathon);
            //    var Ev = db.RegistrationEvent.ToList();
            //    Ev = db.RegistrationEvent.Where(x => x.EventId == evId.EventId).ToList();
            //    //txtRunnerFinishCount.Text = Ev.Count.ToString();
            //    var rv = db.RegistrationEvent.FirstOrDefault(x=> x.EventId == even.Select(x1 => x1.EventId).ToString());
            //    TimeSpan TS = new TimeSpan(0, 0, (int)rv.RaceTime);
            //    txtTime.Text = TS.ToString();

            //}
            //if (cmbDistance.SelectedIndex == -1 && cmbMarathon.SelectedIndex != -1)
            //{
            //    var marathon = db.Marathon.FirstOrDefault(x => x.MarathonName == cmbMarathon.SelectedValue).MarathonId;
            //    var even = db.Event.Where(x => x.MarathonId == marathon);
            //    var ev = db.RegistrationEvent.ToList();

            //    foreach (var rev in ev)
            //    {
            //        if (rev.EventId == even.Select(x => x.EventId).ToString())
            //        {
            //            cnt += 1;
            //        }
            //    }
            //    txtRunnerAllCount.Text = cnt.ToString();
            //    txtRunnerFinishCount.Text = cnt.ToString();

            //    var evId = db.Event.FirstOrDefault(x => x.MarathonId == marathon);
            //    var Ev = db.RegistrationEvent.ToList();
            //    Ev = db.RegistrationEvent.Where(x => x.EventId == evId.EventId).ToList();
            //    //txtRunnerFinishCount.Text = Ev.Count.ToString();
            //    var rv = db.RegistrationEvent.FirstOrDefault(x => x.EventId == even.Select(x1 => x1.EventId).ToString());
            //    TimeSpan TS = new TimeSpan(0, 0, (int)rv.RaceTime);
            //    txtTime.Text = TS.ToString();
            //}
        }

        //private void btnSearch_Click(object sender, RoutedEventArgs e)
        //{
        //    if (cmbDistance.SelectedIndex != -1 && cmbMarathon.SelectedIndex != -1)
        //    {
        //        var distance = db.EventType.FirstOrDefault(x => x.EventTypeName == cmbDistance.SelectedValue); 
        //        var marathon = db.Marathon.FirstOrDefault(x => x.MarathonName == cmbMarathon.SelectedValue);
        //        gridResult.ItemsSource = db.Event.Where(x => x.EventTypeId == distance.EventTypeId).Where(y => y.MarathonId == marathon.MarathonId).ToList();
        //    }
        //    if (cmbDistance.SelectedIndex != -1 && cmbMarathon.SelectedIndex == -1)
        //    {

        //        var distance = db.EventType.FirstOrDefault(x => x.EventTypeName == cmbDistance.SelectedValue).EventTypeId;
        //        gridResult.ItemsSource = db.Event.Where(x => x.EventTypeId == distance).ToList();
        //    }
        //    if (cmbDistance.SelectedIndex == -1 && cmbMarathon.SelectedIndex != -1)
        //    {
        //        var marathon = db.Marathon.FirstOrDefault(x => x.MarathonName == cmbMarathon.SelectedValue).MarathonId;
        //        gridResult.ItemsSource = db.Event.Where(y => y.MarathonId == marathon).ToList();
        //    }
        //}



    }

    public partial class Gonka
    {
        public short Mesto { get; set; }
        public TimeSpan Time { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
    }
}
