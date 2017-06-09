using Fuji.Configuraciones.Entidades;
using Fuji.Configuraciones.Extensions;
using System;
using System.Linq;

namespace Fuji.Configuraciones.DataAccess
{
    public class ConfiguracionDataAccess
    {
        public dbConfigEntities ConfigDA;

        #region gets
        public tbl_ConfigSitio getConfiguracion(string vchClaveSito, ref string mensaje)
        {
            tbl_ConfigSitio response = new tbl_ConfigSitio();
            try
            {
                using (ConfigDA = new dbConfigEntities())
                {
                    if (ConfigDA.tbl_ConfigSitio.Any(item => item.vchClaveSitio.ToUpper().Trim() == vchClaveSito.Trim().ToUpper()))
                    {
                        var query = (from item in ConfigDA.tbl_ConfigSitio
                                     where item.vchClaveSitio.ToUpper().Trim() == vchClaveSito.ToUpper().Trim()
                                     select item);
                        if (query != null)
                        {
                            if (query.Count() > 0)
                            {
                                response = query.First();
                            }
                            else
                            {
                                mensaje = "No se encontro la Configuración para el sitio.";
                            }
                        }
                        else
                        {
                            mensaje = "No se encontro la Configuración para el sitio.";
                        }
                    }
                    else
                    {
                        mensaje = "No se encontro la Configuración para el sitio.";
                    }
                }
            }
            catch(Exception egc)
            {
                Log.EscribeLog("Existe un error al intentar obtener los datos de configuración del Sitio: " + egc.Message);
                response = null;
                mensaje = egc.Message;
            }
            return response;
        }

        public bool getVerificaSitio(string vchClaveSitio, ref string mensaje)
        {
            tbl_ConfigSitio _mdlConf = new tbl_ConfigSitio();
            bool valido = false;
            try
            {
                using (ConfigDA = new dbConfigEntities())
                {
                    if (ConfigDA.tbl_ConfigSitio.Any(item => (bool)item.bitActivo && item.vchClaveSitio == vchClaveSitio))
                    {
                        valido = true;
                        mensaje = "";
                    }
                    else
                    {
                        valido = false;
                        mensaje = "No se encontro la Configuración para el sitio.";
                    }
                }
            }
            catch (Exception egV)
            {
                valido = false;
                Log.EscribeLog("Existe un error en ConfiguracionDataAccess.getVerificaSitio: " + egV.Message);
            }
            return valido;
        }
        #endregion gets

        #region sets
        public bool setConfiguracion(tbl_ConfigSitio mdlConfig, ref string mensaje,ref int id_Sitio)
        {
            bool valido = false;
            try
            {
                ConfigDA = new dbConfigEntities();
                var validacion = (from item in ConfigDA.tbl_ConfigSitio
                                  where item.vchClaveSitio.Trim().ToUpper() == mdlConfig.vchClaveSitio.ToUpper().Trim()
                                  select item).ToList();
                if(validacion != null)
                {
                    if(validacion.Count() > 0)
                    {
                        mensaje = "La clave del sitio que deseas dar de alta ya está en uso, favor de editar.";
                        valido = false;
                    }
                    else
                    {
                        ConfigDA.tbl_ConfigSitio.Add(mdlConfig);
                        ConfigDA.SaveChanges();
                        id_Sitio = mdlConfig.id_Sitio;
                        valido = true;
                        mensaje = "Cambios correctos.";
                    }
                }
                else
                {
                    ConfigDA.tbl_ConfigSitio.Add(mdlConfig);
                    ConfigDA.SaveChanges();
                    id_Sitio = mdlConfig.id_Sitio;
                    valido = true;
                    mensaje = "Cambios correctos.";
                }
            }
            catch(Exception esC)
            {
                Log.EscribeLog("Existe un error al guardar la información" + esC.Message);
                valido = false;
                mensaje = esC.Message;
            }
            return valido;
        }

        public bool updateConfiguracion(clsConfiguracion mdlConfig, ref string mensaje)
        {
            bool valido = false;
            try
            {
                using (ConfigDA = new dbConfigEntities())
                {
                    if (ConfigDA.tbl_ConfigSitio.Any(item => item.vchClaveSitio.Trim().ToUpper() == mdlConfig.vchClaveSitio.Trim().ToUpper()))
                    {
                        
                        //Validar que la nueva clave del sitio no exista
                        //if (ConfigDA.tbl_ConfigSitio.Any(item => item.vchClaveSitio.ToUpper().Trim() == mdlConfig.vchClaveSitio.Trim().ToUpper() && item.id_Sitio != mdlConfig.id_Sitio))
                        //{
                        //    mensaje = "La clave del sitio que deseas dar de alta ya está en uso, favor de editar.";
                        //    valido = false;
                        //}
                        //else// end validacion de nueva clave.
                        //{
                        tbl_ConfigSitio sitio = ConfigDA.tbl_ConfigSitio.First(i => i.vchClaveSitio.Trim().ToUpper() == mdlConfig.vchClaveSitio.Trim().ToUpper());
                        //sitio.vchClaveSitio = mdlConfig.vchClaveSitio;
                        sitio.vchAETitle = mdlConfig.vchAETitle;
                        sitio.vchIPCliente = mdlConfig.vchIPCliente;
                        sitio.vchMaskCliente = mdlConfig.vchMaskCliente;
                        sitio.intPuertoCliente = mdlConfig.intPuertoCliente;
                        sitio.vchnombreSitio = mdlConfig.vchNombreSitio;
                        sitio.vchPathLocal = mdlConfig.vchPathLocal;
                        //sitio.vchIPServidor = mdlConfig.vchIPServidor;
                        //sitio.intPuertoServer = mdlConfig.intPuertoServer;
                        sitio.vchUserAdmin = mdlConfig.vchUserChanges;
                        sitio.datFechaSistema = DateTime.Now;
                        ConfigDA.SaveChanges();
                        mensaje = "Cambios correctos.";
                        valido = true;
                        ConfigDA.Dispose();
                        //}
                    }
                    else
                    {
                        valido = false;
                        mensaje = "No es posible actualizar los datos.";
                    }
                }
            }
            catch (Exception esC)
            {
                Log.EscribeLog("Existe un error al guardar la información: " + esC.Message);
                valido = false;
                mensaje = esC.Message;
            }
            return valido;
        }

        public bool updateConfiguracionServer(clsConfiguracion mdlConfig, ref string mensaje)
        {
            bool valido = false;
            try
            {
                ConfigDA = new dbConfigEntities();
                if (ConfigDA.tbl_ConfigSitio.Any(item => item.vchClaveSitio == mdlConfig.vchClaveSitio))
                {
                    ConfigDA = new dbConfigEntities();
                    ////Validar que la nueva clave del sitio no exista
                    //if (ConfigDA.tbl_ConfigSitio.Any(item => item.vchClaveSitio.ToUpper().Trim() == mdlConfig.vchClaveSitio.Trim().ToUpper() && item.id_Sitio != mdlConfig.id_Sitio))
                    //{
                    //    mensaje = "La clave del sitio que deseas dar de alta ya está en uso, favor de editar.";
                    //    valido = false;
                    //}
                    //else// end validacion de nueva clave.
                    //{
                    tbl_ConfigSitio sitio = ConfigDA.tbl_ConfigSitio.First(i => i.vchClaveSitio.Trim().ToUpper() == mdlConfig.vchClaveSitio.Trim().ToUpper());
                    //sitio.vchClaveSitio = mdlConfig.vchClaveSitio;
                    //sitio.vchIPCliente = mdlConfig.vchIPCliente;
                    //sitio.vchMaskCliente = mdlConfig.vchMaskCliente;
                    //sitio.intPuertoCliente = mdlConfig.intPuertoCliente;
                    //sitio.vchNombreSitio = mdlConfig.vchNombreSitio;
                    sitio.vchIPServidor = mdlConfig.vchIPServidor;
                    sitio.in_tPuertoServer = mdlConfig.intPuertoServer;
                    sitio.vchAETitleServer = mdlConfig.vchAETitleServer;
                    sitio.vchUserAdmin = mdlConfig.vchUserChanges;
                    sitio.datFechaSistema = DateTime.Now;
                    ConfigDA.SaveChanges();
                    mensaje = "Cambios correctos.";
                    valido = true;
                    //}
                }
                else
                {
                    valido = false;
                    mensaje = "No es posible actualizar los datos.";
                }
            }
            catch (Exception esC)
            {
                Log.EscribeLog("Existe un error al actualizar la información: " + esC.Message);
                valido = false;
                mensaje = esC.Message;
            }
            return valido;
        }

        public string getXMLFileConfig(string vchClave)
        {
            string formato = "";
            try
            {
                using (ConfigDA = new dbConfigEntities())
                {
                    if (ConfigDA.tbl_CAT_Extensiones.Any(item => (bool)item.bitActivo && item.vchClave == vchClave))
                    {
                        var query = (from item in ConfigDA.tbl_CAT_Extensiones
                                    where item.vchClave.Trim().ToUpper() == vchClave.Trim().ToUpper()
                                    select item).First();
                        if(query != null)
                        {
                            formato = query.vchValor.ToString();
                        }
                    }
                }
            }
            catch(Exception eXML)
            {
                formato = "";
                Log.EscribeLog("Existe un error al obtener el formato XML: " + eXML.Message);
            }
            return formato;
        }
        #endregion sets
    }
}
