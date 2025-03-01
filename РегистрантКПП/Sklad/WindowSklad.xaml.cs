﻿using System;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Effects;

namespace РегистрантКПП.Sklad
{
    public partial class WindowSklad : Window
    {
        protected DB.Registrants registrants;
        private Thread thread;

        public WindowSklad()
        {
            InitializeComponent();

            if (Registrant.Default.LoadAll == true)
            {
                ch_loadall.IsChecked = true;
            }
            else if (Registrant.Default.LoadAll == false)
            {
                ch_loadall.IsChecked = false;
            }

            Refresh();
            thread = new Thread(RefreshThread);
            thread.Start();
        }

        public void RefreshThread()
        {
            do
            {
                Thread.Sleep(60000);
                Dispatcher.Invoke(() => Drivers.ItemsSource = null);
                Driver driver = new Driver();

                if (Dispatcher.Invoke(() => tb_search.Text == ""))
                {
                    if (Dispatcher.Invoke(() => ch_loadall.IsChecked == true))
                    {
                        driver.LoadListAll();
                        Dispatcher.Invoke(() => Drivers.ItemsSource = driver.driverVs.ToList());
                    }
                    else
                    {
                        driver.LoadList();
                        Dispatcher.Invoke(() => Drivers.ItemsSource = driver.driverVs.ToList());
                    }
                }
            } while (true);
        }

        void Refresh()
        {
            Drivers.ItemsSource = null;
            Driver driver = new Driver();

            if (ch_loadall.IsChecked == true)
            {
                driver.LoadListAll();
                Drivers.ItemsSource = driver.driverVs.ToList();
            }
            else
            {
                driver.LoadList();
                Drivers.ItemsSource = driver.driverVs.ToList();
            }
        }

        private void btn_refresh_Click(object sender, RoutedEventArgs e)
        {
            Refresh();
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
           /* btn_done_timeLeft.Visibility = Visibility.Collapsed;
            btn_done_TimeArrive.Visibility = Visibility.Collapsed;
            new_driver.Visibility = Visibility.Collapsed;
            Driver_Info.Visibility = Visibility.Visible;


            var current_driver = Drivers.SelectedItem as DriverV;

            try
            {
                DB.RegistrantEntities ef = new DB.RegistrantEntities();
                var temp = ef.Registrants.Where(x => x.Id == current_driver.Id).FirstOrDefault();

                
                Driver_Info.DataContext = temp;

            }
            catch (Exception)
            {

                throw;
            }


            if (tb_DateTime.Text != null)
            {
                if (tb_TimeLeft.Text != null)
                {
                    btn_done_TimeArrive.Visibility = Visibility.Visible;

                    if (tb_TimeArrive.Text != null)
                    {
                        btn_done_timeLeft.Visibility = Visibility.Visible;
                    }
                }
                
            } */
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tb_search.Text == "")
            {
                Refresh();
                Grid_Banana.Visibility = Visibility.Hidden;
            }
            else
            {
                btn_search_Click(sender, e);
            }

        }

        private void btn_exit_Click(object sender, RoutedEventArgs e)
        {
            Driver_Info.Visibility = Visibility.Hidden;
            Grid_ChooseDriver.Visibility = Visibility.Visible;
            Refresh();
        }

        private void btn_save_Click(object sender, RoutedEventArgs e)
        {
            BlurEffect effect = new BlurEffect {Radius = 50};
            MainGrid.Effect = effect;

            EditDriver edit = new EditDriver(Convert.ToInt32(tb_id.Text));
            edit.ShowDialog();
            Refresh();

            /* MessageBoxResult result = MessageBox.Show("Вы действительно хотите обновить данные?","Внимание!", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                try
                { /*
                    DB.RegistrantEntities ef = new DB.RegistrantEntities();
                    var driv = ef.Registrants.Where(x => x.Id.ToString() == tb_id.Text).FirstOrDefault();
                    driv.FirstName = tb_firstname.Text;
                    driv.SecondName = tb_secondname.Text;
                    driv.Phone = tb_phone.Text;
                    driv.Info = tb_info.Text + "\n" + DateTime.Now + " внесены изменения";

                    ef.SaveChanges();
                    ef.Dispose();
                    Refresh();

                    MessageBox.Show("Данные успешно обновлены", "Готово", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception)
                {
                    MessageBox.Show("Произошла ошибка при сохранении данных. Проверьте подключение к БД/правильность данных и еще что нибудь да проверьте", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            } */
            MainGrid.Effect = null;

        }

        private void btn_delete_Click(object sender, RoutedEventArgs e)
        {
            BlurEffect effect = new BlurEffect {Radius = 50};
            MainGrid.Effect = effect;
            
             MessageBoxResult result = MessageBox.Show("Вы действительно ходите удалить карточку №" + tb_id.Text + " на имя " + tb_firstname.Text + " " + tb_secondname.Text, "Внимание!", MessageBoxButton.YesNo, MessageBoxImage.Information);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    using (DB.RegistrantEntities ef = new DB.RegistrantEntities())
                    {
                        var driver = ef.Registrants.FirstOrDefault(x => x.Id.ToString() == tb_id.Text);
                        if (driver != null)
                        {
                            driver.Deleted = "D";
                            driver.Info =
                                $"{driver.Info}\n[I]{DateTime.Now}({Registrant.Default.LastLogin}) удалил карточку";
                        }
                        ef.SaveChanges();
                    }
                    Refresh();
                    Driver_Info.Visibility = Visibility.Hidden;
                    Grid_ChooseDriver.Visibility = Visibility.Visible;

                    MessageBox.Show("Карточка успешно удалена", "Готово", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception)
                {
                    MessageBox.Show("Произошла ошибка при сохранении данных. Проверьте подключение к БД/правильность данных и еще что нибудь да проверьте", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            MainGrid.Effect = null;
        }


        private void btn_done_TimeArrive_Click(object sender, RoutedEventArgs e)
        {
            BlurEffect effect = new BlurEffect {Radius = 50};
            MainGrid.Effect = effect;

            MessageBoxResult result = MessageBox.Show("Вы действительно хотите обнвить статус водителя " + tb_firstname.Text + " " + tb_secondname.Text + " на ПРИБЫЛ?", "Внимание!", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    using (DB.RegistrantEntities ef = new DB.RegistrantEntities())
                    {
                        var driver = ef.Registrants.FirstOrDefault(x => x.Id.ToString() == tb_id.Text);
                        if (driver != null)
                        {
                            driver.TimeArrive = DateTime.Now;
                            ef.SaveChanges();
                        }
                    }
                    Refresh();
                    btn_done_TimeArrive.Visibility = Visibility.Collapsed;

                    MessageBox.Show("Данные успешно обновлены", "Готово", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception)
                {
                    MessageBox.Show("Произошла ошибка при сохранении данных. Проверьте подключение к БД/правильность данных и еще что нибудь да проверьте", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            MainGrid.Effect = null;
        }

        private void btn_done_timeLeft_Click(object sender, RoutedEventArgs e)
        {
            BlurEffect effect = new BlurEffect {Radius = 50};
            MainGrid.Effect = effect;

            MessageBoxResult result = MessageBox.Show("Вы действительно хотите обнвить статус водителя " + tb_firstname.Text + " " + tb_secondname.Text + " на ПОКИНУЛ?", "Внимание!", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    using (DB.RegistrantEntities ef = new DB.RegistrantEntities())
                    {
                        var driver = ef.Registrants.FirstOrDefault(x => x.Id.ToString() == tb_id.Text);
                        if (driver != null)
                        {
                            driver.TimeLeft = DateTime.Now;
                        }
                        ef.SaveChanges();
                    }
                    Refresh();

                    btn_done_TimeArrive.Visibility = Visibility.Collapsed;
                    btn_done_timeLeft.Visibility = Visibility.Collapsed;

                    MessageBox.Show("Данные успешно обновлены", "Готово", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception)
                {
                    MessageBox.Show("Произошла ошибка при сохранении данных. Проверьте подключение к БД/правильность данных и еще что нибудь да проверьте", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            MainGrid.Effect = null;
        }

        private void btn_newdriver_Click(object sender, RoutedEventArgs e)
        {
            BlurEffect effect = new BlurEffect {Radius = 20};
            MainGrid.Effect = effect;

            NewDriver newDriver = new NewDriver();
            newDriver.ShowDialog();
            Refresh();
            MainGrid.Effect = null;
        }

        private void btn_opentable_Click(object sender, RoutedEventArgs e)
        {
            Table table = new Table();
            table.Show();
        }

        private void btn_search_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Drivers.ItemsSource = null;
                Driver driver = new Driver();
                driver.LoadListAllWithDel();
                var data = driver.driverVs.Where(t => t.SecondName.ToUpper().StartsWith(tb_search.Text.ToUpper())).ToList();
                var sDOP = driver.driverVs.Where(t => t.SecondName.ToUpper().Contains(tb_search.Text.ToUpper())).ToList();
                data.AddRange(sDOP);

                // data.Distinct().ToArray();
                var noDupes = data.Distinct().ToList();
                Drivers.ItemsSource = noDupes;

                if (noDupes.Count == 0)
                {
                    Grid_Banana.Visibility = Visibility.Visible;
                }
                else
                {
                    Grid_Banana.Visibility = Visibility.Hidden;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Произошла ошибка при обновление списка. Пожалуйста обратитесь к персоналу. Проверьте подключение к БД/интернет или еще что нибудь", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Drivers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btn_done_timeLeft.Visibility = Visibility.Visible;
            btn_done_TimeArrive.Visibility = Visibility.Visible;

            var currentDriver = Drivers.SelectedItem as DriverV;
            Driver_Info.DataContext = currentDriver;

            if (tb_TimeArrive.Text != "")
            {
                btn_done_TimeArrive.Visibility = Visibility.Collapsed;

                if (tb_TimeLeft.Text != "")
                {
                    btn_done_timeLeft.Visibility = Visibility.Collapsed;
                }
            }

            if (tb_id.Text == "")
            {
                Driver_Info.Visibility = Visibility.Hidden;
                Grid_ChooseDriver.Visibility = Visibility.Visible;
            }
            else
            {
                Driver_Info.Visibility = Visibility.Visible;
                Grid_ChooseDriver.Visibility = Visibility.Hidden;
            }

        }

        private void ch_loadall_Checked(object sender, RoutedEventArgs e)
        {
            Registrant.Default.LoadAll = true;
            Registrant.Default.Save();
        }

        private void ch_loadall_Unchecked(object sender, RoutedEventArgs e)
        {
            Registrant.Default.LoadAll = false;
            Registrant.Default.Save();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            thread.Abort();
        }
    }
}
