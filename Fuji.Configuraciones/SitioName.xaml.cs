using Fuji.Configuraciones.DataAccess;
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
using System.Windows.Shapes;

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
                        tbl_ConfigSitio mdl = new tbl_ConfigSitio();
                        mdl.vchClaveSitio = txtSitio.Text.Trim().ToUpper();
                        mdl.vchNombreSitio = txtNombre.Text.ToUpper();
                        mdl.bitActivo = true;
                        mdl.datFechaSistema = DateTime.Now;
                        mdl.vchUserChanges = 1;
                        bool success = false;
                        string mensaje = "";
                        int id_sitio = 0;
                        success = ConfigDA.setConfiguracion(mdl, ref mensaje, ref id_sitio);
                        if(success)
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
