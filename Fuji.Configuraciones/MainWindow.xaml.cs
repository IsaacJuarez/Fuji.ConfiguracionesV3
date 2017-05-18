using Fuji.Configuraciones.DataAccess;
using Fuji.Configuraciones.Entidades;
using Fuji.Configuraciones.Extensions;
using System;
using System.IO;
using System.Windows;

namespace Fuji.Configuraciones
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private LoginDataAccess LoginDA;
        private ConfiguracionDataAccess ConfigDA;
        public static int id_sitio = 0;
        public static clsConfiguracion _configura;
        public MainWindow()
        {
            try
            {
                InitializeComponent();
                _configura = new clsConfiguracion();
                //ConfigDA = new ConfiguracionDataAccess();
                //tbl_ConfigSitio _config = new tbl_ConfigSitio();
                //string mensaje = "";
                if (File.Exists("info.xml"))
                {
                    _configura = XMLConfigurator.getXMLfile();
                    if (_configura != null)
                    {
                        if (_configura.vchClaveSitio != "")
                        {
                            txtSitio.Text = _configura.vchClaveSitio;
                        }
                        else
                        {
                            SitioName site = new SitioName();
                            site.Show();
                            this.Close();
                        }
                    }
                    txtSitio.IsEnabled = false;
                }
                else
                {
                    txtSitio.IsEnabled = true;
                    //    string mensaje = "";
                    //    bool existe = ConfigDA.getVerificaSitio(txtSitio.Text, ref mensaje);
                    //    if (existe)
                    //    {
                    //        tbl_ConfigSitio mdl = new tbl_ConfigSitio();
                    //        ConfigDA.getConfiguracion(txtSitio.Text, ref mensaje);
                    //    }

                }
            }
            catch (Exception eMW)
            {
                MessageBox.Show("Existe un error al iniciar la aplicación: " + eMW.Message);
            }
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtPassword.Password != "" && txtUsuario.Text != "")
                {
                    //verificar en base de datos.
                    //Agregar un superUsuario
                    //LoginDA = new LoginDataAccess();
                    //string mensaje = "";
                    //tbl_CAT_Usuarios mdl = new tbl_CAT_Usuarios();
                    //bool success = LoginDA.Logear(txtUsuario.Text, Security.Encrypt(txtPassword.Password), ref mdl, ref mensaje);
                    bool successPass = false;
                    bool successUser = false;
                    if(_configura!= null)
                    {
                        if(_configura.vchUsuario != "" && _configura.vchPassword != "")
                        {
                            string pass = Security.Decrypt(_configura.vchPassword);
                            successUser = txtUsuario.Text.Trim().ToUpper() == _configura.vchUsuario.Trim().ToUpper();
                            successPass = txtPassword.Password == pass;
                        }
                    }

                    if (successPass && successUser)
                    {
                        Configuracion conf = new Configuracion(txtSitio.Text, txtUsuario.Text, 1,1);
                        conf.Show();
                        this.Close();
                    }
                    else
                    {
                        string mensaje = "";
                        if(!successUser && !successPass)
                        {
                            mensaje = "Usuario y contraseña incorrecta.";
                        }
                        else{
                            if (!successPass)
                            {
                                mensaje = "Contraseña incorrecta.";
                            }
                            if (!successUser)
                            {
                                mensaje = "Usuario incorrecto.";
                            }
                        }
                        MessageBox.Show(mensaje,"Error");
                    }
                }
                else
                {
                    MessageBox.Show("Los campos de usuario y contraseña son requeridos");
                }
            }
            catch (Exception eLogin)
            {
                Log.EscribeLog("Error en el Login: " + eLogin.Message);
                MessageBox.Show("Existe un error, favor de verificar: " + eLogin.Message);
            }
        }

        private void btnSol_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("http://localhost:50246/AgregarSitio/AgregarSitio.aspx");
            }
            catch(Exception eBS)
            {
                Log.EscribeLog("Error al abrir el portar para solicitud de Sitio. " + eBS.Message);
            }
        }
    }
}
