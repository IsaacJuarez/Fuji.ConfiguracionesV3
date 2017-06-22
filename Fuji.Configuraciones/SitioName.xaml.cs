using Fuji.Configuraciones.DataAccess;
using Fuji.Configuraciones.Feed2Service;
using System;
using System.Windows;

namespace Fuji.Configuraciones
{
    /// <summary>
    /// Lógica de interacción para SitioName.xaml
    /// </summary>
    public partial class SitioName : Window
    {
        private ConfiguracionDataAccess ConfigDA;
        public SitioName()
        {
            InitializeComponent();
        }

        private void btnSaveSitio_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtSitio.Text.Trim() != "" && txtNombre.Text.Trim() != "")
                {
                    if (txtSitio.Text.Trim().Length == 4)
                    {
                        //Guardar 
                        ConfigDA = new ConfiguracionDataAccess();
                        Feed2Service.tbl_ConfigSitio mdl = new Feed2Service.tbl_ConfigSitio();
                        mdl.vchClaveSitio = txtSitio.Text.Trim().ToUpper();
                        mdl.vchnombreSitio = txtNombre.Text.ToUpper();
                        mdl.bitActivo = true;
                        mdl.datFechaSistema = DateTime.Now;
                        mdl.vchUserAdmin = "ADMINISTRADOR";
                        bool success = false;
                        string mensaje = "";
                        int id_sitio = 0;
                        ClienteF2CResponse response = new ClienteF2CResponse();
                        response = ConfigDA.setConfiguracion(mdl, ref mensaje, ref id_sitio);
                        if(response.valido)
                        {
                            MessageBox.Show("Cambios correctos.");
                            MainWindow main = new MainWindow();
                            main.Show();
                            main.txtSitio.Text = txtSitio.Text;
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Existe un error al guardar la información: " + mensaje, "Error");
                        }
                        
                    }
                    else
                    {
                        MessageBox.Show("Se debe capturar 4 caracteres para la clave del Sitio. ");
                    }
                }
                else
                {
                    MessageBox.Show("Se requiere capturar la clave para el Sitio.");
                }
            }
            catch(Exception ebSS)
            {
                MessageBox.Show("Existe un error al iniciar el sitio: " + ebSS.Message);
            }
        }
    }
}
